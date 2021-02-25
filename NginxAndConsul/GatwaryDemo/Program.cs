using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.DependencyInjection;



namespace GatwaryDemo
{
  public class Program
  {
    public static void Main(string[] args)
    {
      new WebHostBuilder()
                    .UseKestrel(options =>
                    {
                      Uri uri = new Uri("http://192.168.30.134:6677/");
                      IPAddress ip = IPAddress.Parse(uri.Host);
                      int port = uri.Port;
                      options.Listen(ip, port);
                      //设置响应时间
                      options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(20);
                      options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(20);
                    })
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                      config
                      .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                      .AddJsonFile("appsettings.json", true, true)
                      .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                      .AddJsonFile("configuration.json")
                      .AddEnvironmentVariables();
                    })
                    .ConfigureServices(services =>
                    {
                      services.AddOcelot().AddConsul();
                    })
                    .ConfigureLogging((hostingContext, logging) =>
                    {
                  //add your logging
                })
                    .UseIISIntegration()
                    .Configure(app =>
                    {
                      app.UseOcelot().Wait();
                    })
                    .Build()
                    .Run();
    }

    //dotnet XXX.dll --urls="http://*:6666" --ip="127.0.0.1" --port=6666


    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args) 
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseKestrel(
                  options =>
                  {
                    Uri uri = new Uri("http://192.168.30.193:6677/");
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
