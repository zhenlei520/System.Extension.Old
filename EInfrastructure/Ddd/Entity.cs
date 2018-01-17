using System;

namespace EInfrastructure.Ddd
{
  public abstract class Entity : IEntity
  {
    public Entity()
    {
      Id = Guid.NewGuid();
    }

    public virtual Guid Id { get; set; }
  }
}
