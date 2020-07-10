using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NginxDemoNew
{
  public class Config
  {
    private static IConfigurationBuilder builder;
    private static IConfigurationRoot configRoot;
    static Config()
    {
      builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
      configRoot = builder.Build();
    }

    /// <summary>
    /// 根据Key取Value值
    /// </summary>
    /// <param name="sectionName">节点名称</param>
    public static string GetValue(string sectionName)
    {
      var value = configRoot.GetSection(sectionName);
      return value.Value;
    }

    /// <summary>
    /// 获取appSetting下的节点值
    /// </summary>
    /// <param name="sectionName">节点名称</param>
    public static string GetAppSetting(string sectionName)
    {
      var value = configRoot.GetSection("appSettings").GetSection(sectionName);
      return value.Value;
    }
  }
}
