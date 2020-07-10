using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NginxAndConsul.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class HomeController : ControllerBase
  {
    
    private static int _index = 0; 

    [HttpGet]
    public IActionResult Get()
    {
      ConsulClient client = new ConsulClient(a =>
      {
        a.Address = new Uri("http://localhost:8500/");
        a.Datacenter = "dcl";
      });
      var response = client.Agent.Services().Result.Response;

      var url = "http://TestService/api/index/get1";

      Uri uri = new Uri(url);
      var groupName = uri.Host;

      var serviceDic = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();

      //轮训
      AgentService agentService = serviceDic[_index++ % 2].Value;

      //随机
      // AgentService agentService1 = serviceDic[new Random(_index++).Next(0, serviceDic.Length)].Value;


      url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";
      Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");
      Console.WriteLine("请求地址：" + url);

      var retString = "";

      var responseData = HttpHelper.CreateGetHttpResponse(url);

      if (responseData.StatusCode == HttpStatusCode.OK)
      {
        Stream myResponseStream = responseData.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream ?? throw new InvalidOperationException(), Encoding.GetEncoding("UTF-8"));
          retString = myStreamReader.ReadToEnd();

        Console.WriteLine("请求结果：" + retString);
        myStreamReader.Close();
        myResponseStream.Close();
      
      }

   

      Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!");

      return new JsonResult(retString);
    }

  }
}
