using EInfrastructure.HelpCommon;
using EInfrastructure.HelpCommon.Serialization;
using EInfrastructure.HelpCommon.Serialization.JsonAdapter;

namespace EInfrastructure.Infrastructure.Exception
{
    /// <summary>
    /// 友好异常
    /// </summary>
    public class FriendlyException : System.Exception
    {
        /// <summary>
        /// 友好异常,可以将hint消息直接返回给用户
        /// </summary>
        /// <param name="hint">返回给用户友好的异常</param>
        /// <param name="err">错误信息</param>
        public FriendlyException(string hint, string err):base(new JsonCommon(EnumJsonMode.Newtonsoft).Serializer(new { Hint = hint, Err = err }))
        {
        }
    }
}
