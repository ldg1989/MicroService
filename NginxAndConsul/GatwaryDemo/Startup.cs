using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace GatwaryDemo
{
  public class Startup
  {
    public Startup(IConfiguration configuration, IHostEnvironment env)
    {
      var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
      builder.SetBasePath(env.ContentRootPath)
        .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
          .AddJsonFile("appsettings.json", true, true)
        .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddOcelot()
        .AddConsul();
      //app.AddOcelot(); services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseOcelot().Wait();  //整个管道换成 ocelot
      //if (env.IsDevelopment())
      //{
      //  app.UseDeveloperExceptionPage();
      //}

      //app.UseRouting();

      //app.UseAuthorization();

      //app.UseEndpoints(endpoints =>
      //{
      //  endpoints.MapControllers();
      //});
    }
  }
}
