using System;

namespace EInfrastructure.Ddd.Repository
{
  /// <summary>
  /// 添加并且修改聚合根
  /// </summary>
  public class AddAndUpdate : Adds
  {
    public AddAndUpdate()
    {
      EditTime = DateTime.Now;
    }

    public AddAndUpdate(Guid accountId) : base(accountId)
    {
      EditAccountId = accountId;
      EditTime = DateTime.Now;
    }

    /// <summary>
    /// 编辑信息
    /// </summary>
    /// <param name="accountId">账户id</param>
    public void UpdateInfo(Guid accountId)
    {
      EditAccountId = accountId;
      EditTime = DateTime.Now;
    }

    /// <summary>
    /// 修改用户id
    /// </summary>
    public Guid EditAccountId { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public DateTime EditTime { get; set; }
  }
}
