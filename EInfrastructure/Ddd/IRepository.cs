using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EInfrastructure.Ddd
{
  public interface IRepository<TEntity> where TEntity : IAggregateRoot
  {

    #region 添加数据

    #region 添加单条数据

    /// <summary>
    /// 添加单条数据
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="t"></param>
    void AddModel(TEntity t);
    #endregion

    #region 添加多条数据

    /// <summary>
    /// 添加多条数据
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="t"></param>
    void AddMultiModel(IEnumerable<TEntity> t);
    #endregion

    #endregion

    #region 移除数据

    #region 移除第一个满足条件的数据

    /// <summary>
    /// 移除第一个满足条件的数据
    /// </summary>
    /// <param name="condition">条件</param>
    void RemoveAt(Expression<Func<TEntity, bool>> condition);
    #endregion

    #region 移除所有满足条件的数据

    /// <summary>
    /// 移除所有满足条件的数据
    /// </summary>
    /// <param name="condition">条件</param>
    void RemoveAll(Expression<Func<TEntity, bool>> condition);

    #endregion

    #region 移除满足条件的数据
    /// <summary>
    /// 移除满足条件的数据
    /// </summary>
    /// <param name="t">被ef跟踪的实体</param>
    void RemoveAt(TEntity t);
    #endregion

    #region 移除满足条件的数据集合

    /// <summary>
    /// 移除满足条件的集合数据集合
    /// </summary>
    /// <param name="t">被ef跟踪的实体</param>
    void RemoveAll(List<TEntity> t);

    #endregion

    #endregion

    #region 更新数据
    /// <summary>
    /// 更新数据
    /// </summary>
    /// <param name="t"></param>
    void Update(TEntity t);
    #endregion

    #region 加载数据
    /// <summary>
    /// 加载数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    TEntity LoadEntity(Guid id);
    #endregion

    #region 查找符合条件的数据(IQueryable集合)
    /// <summary>
    /// 查找符合条件的数据(IQueryable集合)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="condition"></param>
    /// <returns></returns>
    IQueryable<TEntity> FindIQueryable(Expression<Func<TEntity, bool>> condition);
    #endregion
  }
}
