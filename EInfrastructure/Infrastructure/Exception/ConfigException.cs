using EInfrastructure.HelpCommon;
using EInfrastructure.Infrastructure.Enum;

namespace EInfrastructure.Infrastructure.Exception
{
    /// <summary>
    /// 配置异常
    /// </summary>
    public class ConfigException : System.Exception
    {
        /// <summary>
        /// 配置异常
        /// </summary>
        /// <param name="config">配置信息</param>
        /// <param name="configName">配置名称</param>
        public ConfigException(ConfigEnum config = ConfigEnum.AppSetting, string configName = "") : base(string.Format(EnumCommon.GetDescription(config), string.IsNullOrEmpty(configName) ? "" : ",名称:" + configName))
        {

        }
    }
}
