namespace EInfrastructure.Infrastructure.Enum
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum CodeStatusEnum
    {
        /// <summary>
        /// 重复
        /// </summary>
        Repeat = 101,
        /// <summary>
        /// 授权失败
        /// </summary>
        NoAuthorization = 110,
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 200,
        /// <summary>
        /// 时间超时
        /// </summary>
        Timeout = 303,
        /// <summary>
        /// 未发现
        /// </summary>
        NoFind = 404,
        /// <summary>
        /// 错误
        /// </summary>
        Err = 500,
        /// <summary>
        /// 系统错误
        /// </summary>
        SystemErr = 505,
    }
}
