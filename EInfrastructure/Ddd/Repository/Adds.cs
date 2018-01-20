using System;

namespace EInfrastructure.Ddd.Repository
{
  /// <summary>
  /// 添加信息聚合根
  /// </summary>
  public class Adds : AggregateRoot
  {
    public Adds()
    {
      AddTime = DateTime.Now;
    }

    public Adds(Guid accountId)
    {
      AddAccountId = accountId;
      AddTime = DateTime.Now;
    }

    /// <summary>
    /// 添加用户id
    /// </summary>
    public Guid AddAccountId { get; set; }

    /// <summary>
    /// 添加时间
    /// </summary>
    public DateTime AddTime { get; set; }
  }
}
