namespace EInfrastructure.Infrastructure.Key
{
    /// <summary>
    /// 错误码
    /// </summary>
    public enum CodeKey
    {
        /// <summary>
        /// 重复请求
        /// </summary>
        RepeatRequest = 101,
        /// <summary>
        /// 定位失败
        /// </summary>
        LocationLose = 102,
        /// <summary>
        /// 请求非法
        /// </summary>
        Illegal = 110,
        /// <summary>
        /// 执行成功
        /// </summary>
        Ok = 200,
        /// <summary>
        /// 业务异常，
        /// </summary>
        Err = 201,
        /// <summary>
        /// 未绑定手机资源
        /// </summary>
        NoBindPhoneResource = 202,
        /// <summary>
        /// 未绑定微信资源
        /// </summary>
        NoBindWebChatResource = 203,
        /// <summary>
        /// 未授权，身份认证失败
        /// </summary>
        NoAuthorization = 401,
        /// <summary>
        /// 请求被禁止
        /// </summary>
        Forbid = 403,
        /// <summary>
        /// 请求网页或者方法未找到
        /// </summary>
        NoFound = 404,
        /// <summary>
        /// 系统错误
        /// </summary>
        SystemErr = 500,
        /// <summary>
        /// 询问框
        /// </summary>
        Confirm = 501,
        /// <summary>
        /// 响应超时
        /// </summary>
        Timeout = 504,
        /// <summary>
        /// 系统错误
        /// </summary>
        System = 505,
    }
}
