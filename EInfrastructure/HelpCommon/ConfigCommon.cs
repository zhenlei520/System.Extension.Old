using System;
using System.Configuration;
using System.Web.Configuration;
using EInfrastructure.Infrastructure.Enum;

namespace EInfrastructure.HelpCommon
{
  /// <summary>
  /// 提供对.config文件的访问
  /// 如果不使用此类库，可移除对System.Web.Configuration的引用
  /// </summary>
  public class ConfigCommon
  {
    #region 获取数据库连接字符串(&lt;connectionStrings&gt;节点)
    /// <summary>
    /// 获取数据库连接字符串(&lt;connectionStrings&gt;节点)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetConnString(string key)
    {
      var conn = ConfigurationManager.ConnectionStrings[key];
      if (conn == null)
        throw new Exception(EnumCommon.GetDescription(ConfigEnum.AppSetting));
      return ConfigurationManager.ConnectionStrings[key].ConnectionString;
    }
    #endregion

    #region  设置/重写一个key:value对

    /// <summary>
    /// 设置/重写一个key:value对
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool SetSetting(string key, string value)
    {
      try
      {
        Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
        AppSettingsSection section = config.AppSettings;
        if (section.Settings[key] == null)
        {
          section.Settings.Add(key, value);
        }
        else
        {
          section.Settings[key].Value = value;
        }
        config.Save();
        return true;
      }
      catch
      {
        return false;
      }
    }
    #endregion

    #region 设置/重写一个数据库连接串
    /// <summary>
    /// 设置/重写一个数据库连接串
    /// </summary>
    /// <param name="key"></param>
    /// <param name="connString"></param>
    /// <returns></returns>
    public static bool SetConnString(string key, string connString)
    {
      try
      {
        Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
        ConnectionStringsSection section = config.ConnectionStrings;
        if (section.ConnectionStrings[key] == null)
        {
          section.ConnectionStrings.Add(new ConnectionStringSettings(key, connString));
        }
        else
        {
          section.ConnectionStrings[key].ConnectionString = connString;
        }
        config.Save();
        return true;
      }
      catch
      {
        return false;
      }
    }
    #endregion

    #region 获取AppSetting配置字符串(&lt;add key='' Value='' /&gt;)
    /// <summary>
    /// 获取AppSetting配置字符串(&lt;add key='' Value='' /&gt;)
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetConfig(string key)
    {
      return ConfigurationManager.AppSettings[key];
    }
    #endregion

    #region 删除一个key:value节点
    /// <summary>
    /// 删除一个key:value节点
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static bool RemoveSetting(string key)
    {
      try
      {
        Configuration config = WebConfigurationManager.OpenWebConfiguration(null);
        AppSettingsSection section = config.AppSettings;
        if (section.Settings[key] != null)
        {
          section.Settings.Remove(key);
        }
        config.Save();
        return true;
      }
      catch
      {
        return false;
      }
    }
    #endregion
  }
}

