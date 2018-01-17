using EInfrastructure.HelpCommon.Serialization;
using EInfrastructure.Infrastructure.Key;

namespace EInfrastructure.Infrastructure.Exception
{
  /// <inheritdoc />
  /// <summary>
  /// 业务异常,可以将Exception消息直接返回给用户
  /// </summary>
  public class BusinessException : System.Exception
  {
    /// <summary>
    /// 业务异常
    /// </summary>
    /// <param name="code">状态码</param>
    /// <param name="content">异常详情</param>
    public BusinessException(string content, CodeKey code = CodeKey.Err) :
        base(new JsonCommon().Serializer(new { code = (int)code, content = content }))
    {

    }
  }
}
