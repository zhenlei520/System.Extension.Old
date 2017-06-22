using EInfrastructure.HelpCommon;
using EInfrastructure.HelpCommon.Serialization;
using EInfrastructure.HelpCommon.Serialization.JsonAdapter;
using EInfrastructure.Infrastructure.Enum;

namespace EInfrastructure.Infrastructure.Exception
{
    /// <summary>
    /// 业务错误
    /// </summary>
    public class BusinessException:System.Exception
    {
        /// <summary>
        /// 业务异常
        /// </summary>
        /// <param name="code">状态码</param>
        /// <param name="content">异常详情</param>
        public BusinessException(string content, CodeStatusEnum code = CodeStatusEnum.Err) :
            base(new JsonCommon(EnumJsonMode.Newtonsoft).Serializer(new { code = code, content = content }))
        {

        }
    }
}
