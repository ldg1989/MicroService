using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NginxDemoNew.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class IndexController : ControllerBase
  {
    static readonly string DeployUrl = Config.GetAppSetting("DeployUrl");

    [HttpGet]
    [Route("get1")]
    public IActionResult Get()
    {
      Console.WriteLine("这里是：" + DeployUrl);

      return new JsonResult("这里是：" + DeployUrl);
    }

    [HttpGet]
    [Route("get2")]
    public IActionResult Get2()
    {
      Console.WriteLine("心跳：" + Guid.NewGuid());

      return new JsonResult("心跳：" + Guid.NewGuid());
    }


  }
}
