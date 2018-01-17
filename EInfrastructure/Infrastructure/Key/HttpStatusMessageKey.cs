namespace EInfrastructure.Infrastructure.Key
{
    /// <summary>
    /// Http请求错误信息key
    /// </summary>
    public class HttpStatusMessageKey
    {
        /// <summary>
        /// 服务器异常
        /// </summary>
        public const string InternalServerError = "服务器链接异常，稍后再试吧";

        /// <summary>
        /// 请求超时
        /// </summary>
        public const string RequestTimeout = "请求超时";

        /// <summary>
        /// 授权失败
        /// </summary>
        public const string NoAuthorization = "授权失败,请重新登录";

        /// <summary>
        /// 数据库异常
        /// </summary>
        public const string DbEntityValidationException = "数据库查询异常";

        /// <summary>
        /// 定位失败
        /// </summary>
        public const string LocationLoseException = "定位失败，请尝试开启Gps后重试操作";

        /// <summary>
        /// 模拟请求，请求非法
        /// </summary>
        public const string IllegalRequestException = "请求非法";

        /// <summary>
        /// 请求禁止
        /// </summary>
        public const string RequestForbit = "请求禁止";
    }
}
