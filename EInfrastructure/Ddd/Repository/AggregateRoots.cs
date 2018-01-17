using System;

namespace EInfrastructure.Ddd.Repository
{
  /// <summary>
  /// 
  /// </summary>
  public class AggregateRoots : AggregateRoot
  {
    public AggregateRoots()
    {
      Id = Guid.NewGuid();
    }

    /// <summary>
    /// 账户id
    /// </summary>
    public override Guid Id { get; set; }

    /// <summary>
    /// 得到聚合根id
    /// </summary>
    /// <returns></returns>
    public Guid GetAggregateRootId()
    {
      return Id;
    }
  }
}
