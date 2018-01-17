﻿
using EInfrastructure.Infrastructure.Key;

namespace EInfrastructure.Infrastructure.Exception
{
  /// <summary>
  /// 权限不足异常信息
  /// </summary>
  public class AuthException : System.Exception
  {
    public AuthException(string msg = HttpStatusMessageKey.NoAuthorization)
       : base(msg)
    {

    }
  }
}
