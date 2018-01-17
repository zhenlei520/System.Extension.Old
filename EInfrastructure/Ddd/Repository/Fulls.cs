﻿using System;

namespace EInfrastructure.Ddd.Repository
{
  /// <summary>
  /// 添加/修改/删除聚合根
  /// </summary>
  public class Fulls : AddAndUpdate
  {
    public Fulls()
    {
      IsDel = false;
    }

    public Fulls(Guid accountId) : base(accountId)
    {
      IsDel = false;
    }

    /// <summary>
    /// 删除信息
    /// </summary>
    /// <param name="accountId">账户id</param>
    public void DeleteInfo(Guid accountId)
    {
      IsDel = true;
      DelAccountId = accountId;
      DelTime = DateTime.Now;
      UpdateInfo(accountId);
    }

    /// <summary>
    /// 是否删除
    /// </summary>
    public bool IsDel { get; set; }

    /// <summary>
    /// 删除用户id
    /// </summary>
    public Guid? DelAccountId { get; set; }

    /// <summary>
    /// 删除时间
    /// </summary>
    public DateTime? DelTime { get; set; }
  }
}
