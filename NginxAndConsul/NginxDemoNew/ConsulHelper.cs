using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NginxDemoNew
{
  public static class ConsulHelper
  {
    static readonly string DeployUrl = Config.GetAppSetting("DeployUrl");
    public static void ConsulRegist(this IConfiguration configuration)
    {

      ConsulClient client = new ConsulClient(a =>
      {
        a.Address = new Uri("http://localhost:8500/");
        a.Datacenter = "dcl";
      });
      string ip = configuration["ip"];
      int port = int.Parse(configuration["port"]);
      int weight = string.IsNullOrWhiteSpace(configuration["weight"])?1:int.Parse(configuration["weight"]);

      var id = Guid.NewGuid();
      client.Agent.ServiceRegister(new AgentServiceRegistration()
      {
        ID = "service" + id, // 服务Id 唯一标识
        Name = "TestService",// 组名 group
        Address = ip,//ip
        Port = port,//端口
        Tags = new string[] { weight.ToString() },//标签
        Check = new AgentServiceCheck()
        {
          Interval = TimeSpan.FromSeconds(15),// 请求间隔时间
          HTTP = $"{DeployUrl}/api/index/get2",//接口地址
          Timeout = TimeSpan.FromSeconds(5),//检测等待时间
          DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60)//失败后多久移除
        }

      });
      Console.WriteLine($"{ip}:{port}--weight:{weight}");

    }
  }
}
