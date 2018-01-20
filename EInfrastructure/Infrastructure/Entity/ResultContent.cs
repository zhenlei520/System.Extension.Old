using EInfrastructure.Infrastructure.Enum;

namespace EInfrastructure.Infrastructure.Entity
{
    /// <summary>
    /// 返回信息
    /// </summary>
    public class ResultContent
    {
        public ResultContent(CodeStatusEnum code, string msg, object data)
        {
            Code = (int)code;
            Msg = msg;
            Data = data;
        }

        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="data">结果</param>
        /// <param name="msg">返回提示信息</param>
        /// <returns></returns>
        public static ResultContent Success(object data = null, string msg = "")
        {
            return new ResultContent(CodeStatusEnum.Ok, msg, data);
        }

        /// <summary>
        /// 成功返回
        /// </summary>
        /// <param name="msg">返回提示信息</param>
        /// <returns></returns>
        public static ResultContent Success(string msg = "")
        {
            return new ResultContent(CodeStatusEnum.Ok, msg, null);
        }

        /// <summary>
        /// 错误返回
        /// </summary>
        /// <param name="msg">错误内容</param>
        /// <returns></returns>
        public static ResultContent Err(string msg)
        {
            return new ResultContent(CodeStatusEnum.Err, msg, null);
        }

        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回信息
        /// </summary>
        public object Data { get; set; }
    }
}
