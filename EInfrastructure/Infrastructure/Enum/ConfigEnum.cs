using System.ComponentModel;

namespace EInfrastructure.Infrastructure.Enum
{
    /// <summary>
    /// 配置信息
    /// </summary>
    public enum ConfigEnum
    {
        /// <summary>
        /// 未配置appSettings信息
        /// </summary>
        [Description("未配置appSettings信息{0}")]
        AppSetting = 0,
        /// <summary>
        /// 未配置configSections信息
        /// </summary>
        [Description("未配置configSections信息{0}")]
        Sections = 1,
        /// <summary>
        /// 未配置connectionStrings信息
        /// </summary>
        [Description("未配置connectionStrings信息{0}")]
        ConnectionString = 2,

    }
}
