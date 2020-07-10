using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NginxDemoNew
{
  public class Program
  {
    static readonly string DeployUrl = Config.GetAppSetting("DeployUrl");
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseKestrel(
                  options =>
                  {
                    Uri uri = new Uri(DeployUrl);
                    IPAddress ip = IPAddress.Parse(uri.Host);
                    int port = uri.Port;
                    options.Listen(ip, port);
                    //设置响应时间
                    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);
                  });
              webBuilder.UseStartup<Startup>();
            });
  }
}
