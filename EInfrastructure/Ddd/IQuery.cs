using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EInfrastructure.Ddd.Repository;

namespace EInfrastructure.Ddd
{
  public interface IQuery<TEntity> where TEntity : IEntity
  {
    #region 判断是否存在数据

    /// <summary>
    /// 判断是否存在数据
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    bool Any(Expression<Func<TEntity, bool>> condition);

    #endregion

    #region 查找符合条件的数据

    #region 查找符合条件的数据(IQueryable集合)

    /// <summary>
    /// 查找符合条件的数据(IQueryable集合)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="condition"></param>
    /// <returns></returns>
    IQueryable<TEntity> FindIQueryable(Expression<Func<TEntity, bool>> condition);
    #endregion

    #region 找到满足条件的第一个元素

    /// <summary>
    /// 找到满足条件的第一个元素
    /// </summary>
    /// <param name="condition">搜索条件</param>
    /// <returns></returns>
    TEntity FindFirstOrDefault(Expression<Func<TEntity, bool>> condition);
    #endregion

    #region 查找符合条件的数据(映射查找单条)

    /// <summary>
    /// 查找符合条件的数据
    /// </summary>
    /// <typeparam name="TS">目标数据类</typeparam>
    /// <param name="condition">条件</param>
    /// <returns></returns>
    TS FindFirstOrDefaultTo<TS>(Expression<Func<TEntity, bool>> condition);
    #endregion

    #region 查找符合条件的数据数量

    /// <summary>
    /// 查找符合条件的数据数量
    /// </summary>
    /// <param name="condition">条件</param>
    /// <returns></returns>
    int FindCount(Expression<Func<TEntity, bool>> condition);

    #endregion

    #endregion

    #region 获得所有的数据
    /// <summary>
    /// 获得所有数据
    /// </summary>
    /// <returns></returns>
    IEnumerable<TEntity> GetList();

    #endregion

    #region 全部列表

    #region 查找符合条件的所有数据信息(全部数据,数据列)

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="condition"></param>
    /// <returns></returns>
    List<TEntity> FindAll(Expression<Func<TEntity, bool>> condition);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    List<TEntity> FindAll();
    #endregion

    #region 查找符合条件的所有数据信息(全部数据,数据列,映射)

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列,映射)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TOpt">目标类型</typeparam>
    /// <param name="condition">条件</param>
    /// <returns></returns>
    List<TOpt> FindAllTo<TOpt>(Expression<Func<TEntity, bool>> condition);

    /// <summary>
    /// 查找所有数据信息(全部数据,数据列,映射)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TOpt">目标类型</typeparam>
    /// <returns></returns>
    List<TOpt> FindAllTo<TOpt>();
    #endregion

    #region 查找符合条件的所有数据信息(全部数据,数据列,排序)

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列,排序，根据lambda表达式排序)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TS"></typeparam>
    /// <param name="condition">条件</param>
    /// <param name="orderExpression">排序</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindAll<TS>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TS>> orderExpression,
        bool isDesc);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列，可以排序，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="orderByProperty">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindAll(Expression<Func<TEntity, bool>> condition, string orderByProperty, bool isDesc);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列，可以排序，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="orderByDictionary">多个排序条件</param>
    /// <returns></returns>
    List<TEntity> FindAll(Expression<Func<TEntity, bool>> condition, Dictionary<string, bool> orderByDictionary);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列，可以排序，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TS">排序类型</typeparam>
    /// <param name="orderExpression">排序表达式</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindAll<TS>(Expression<Func<TEntity, TS>> orderExpression, bool isDesc);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列，可以排序，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <param name="orderByProperty">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindAll(string orderByProperty, bool isDesc);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列，可以排序，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <param name="orderByDictionary">多个排序条件</param>
    /// <returns></returns>
    List<TEntity> FindAll(Dictionary<string, bool> orderByDictionary);
    #endregion

    #region 查找符合条件的所有数据信息(全部数据,数据列,映射,排序)

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列,排序，根据lambda表达式排序)
    /// </summary>
    /// <typeparam name="TOpt">目标类型</typeparam>
    /// <typeparam name="TS"></typeparam>
    /// <param name="condition">条件</param>
    /// <param name="orderExpression">排序</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TOpt> FindAllTo<TOpt, TS>(Expression<Func<TEntity, bool>> condition,
        Expression<Func<TEntity, TS>> orderExpression, bool isDesc);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列，可以排序，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TOpt">目标类型</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="orderByProperty">排序字段</param>
    /// <param name="isDesc"></param>
    /// <returns></returns>
    List<TOpt> FindAllTo<TOpt>(Expression<Func<TEntity, bool>> condition, string orderByProperty, bool isDesc);

    /// <summary>
    /// 查找符合条件的所有数据信息(全部数据,数据列，可以排序，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TOpt">目标类型</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="orderByDictionary">多个排序条件</param>
    /// <returns></returns>
    List<TOpt> FindAllTo<TOpt>(Expression<Func<TEntity, bool>> condition,
        Dictionary<string, bool> orderByDictionary);
    #endregion

    #endregion

    #region 分页列表

    #region 分页,查找所有源数据信息

    /// <summary>
    /// 分页,查找所有源数据信息
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="condition">条件</param>
    /// <param name="orderByExpression">升降序条件</param>
    /// <param name="isDesc">是否降序</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TEntity> FindPageDataAll<TKey>(Expression<Func<TEntity, bool>> condition, int pageIndex, int pageSize,
        Expression<Func<TEntity, TKey>> orderByExpression, bool isDesc, bool isTotal);

    /// <summary>
    /// 分页,查找所有源数据信息（根据属性名称排序）
    /// </summary>
    /// <param name="condition">条件</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="orderByProperty">排序条件</param>
    /// <param name="isDesc">是否降序</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TEntity> FindPageDataAll(Expression<Func<TEntity, bool>> condition, int pageIndex, int pageSize,
        string orderByProperty, bool isDesc, bool isTotal);

    /// <summary>
    /// 分页,查找所有源数据信息（根据属性名称排序）
    /// </summary>
    /// <param name="condition">条件</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="orderByDictionary">多个排序条件</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TEntity> FindPageDataAll(Expression<Func<TEntity, bool>> condition, int pageIndex, int pageSize,
        Dictionary<string, bool> orderByDictionary, bool isTotal);
    #endregion

    #region 分页,查找所有目标数据信息(映射)

    /// <summary>
    /// 分页,查找所有目标数据信息(映射)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TOpt"></typeparam>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="condition">条件</param>
    /// <param name="orderByExpression">升降序条件</param>
    /// <param name="isDesc">是否降序</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TOpt> FindPageDataTo<TKey, TOpt>(Expression<Func<TEntity, bool>> condition, int pageIndex, int pageSize,
        Expression<Func<TEntity, TKey>> orderByExpression, bool isDesc, bool isTotal);

    /// <summary>
    /// 分页,查找所有目标数据信息(映射，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TOpt"></typeparam>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="condition">条件</param>
    /// <param name="orderByProperty">排序条件</param>
    /// <param name="isDesc">是否降序</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TOpt> FindPageDataTo<TOpt>(Expression<Func<TEntity, bool>> condition, int pageIndex, int pageSize,
        string orderByProperty, bool isDesc, bool isTotal);

    /// <summary>
    /// 分页,查找所有目标数据信息(映射，根据属性名称排序)
    /// </summary>
    /// <typeparam name="TEntity">源数据</typeparam>
    /// <typeparam name="TOpt"></typeparam>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="condition">条件</param>
    /// <param name="orderByDictionary">多个排序条件</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TOpt> FindPageDataTo<TOpt>(Expression<Func<TEntity, bool>> condition, int pageIndex, int pageSize,
        Dictionary<string, bool> orderByDictionary, bool isTotal);
    #endregion

    #region 分页,查找所有目标数据信息(映射)

    /// <summary>
    /// 分页,查找所有目标数据信息(映射)
    /// </summary>
    /// <typeparam name="TOpt">返回的实体类</typeparam>
    /// <param name="iQueryable">查找符合条件的数据(IQueryable集合)</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TOpt> FindPageDataTo<TOpt>(IQueryable<TEntity> iQueryable, int pageIndex, int pageSize, bool isTotal);

    /// <summary>
    /// 分页,查找所有目标数据信息(映射)
    /// </summary>
    /// <typeparam name="TOpt">返回的实体类</typeparam>
    /// <param name="iQueryable">查找符合条件的数据(IQueryable集合)</param>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">页大小</param>
    /// <param name="isTotal">是否计算总数</param>
    /// <returns></returns>
    PageData<TOpt> FindPageDataTo<TOpt>(IQueryable<TOpt> iQueryable, int pageIndex, int pageSize, bool isTotal);

    #endregion

    #endregion

    #region 返回前N条记录

    #region 返回前N条记录

    /// <summary>
    /// 返回前n条记录
    /// </summary>
    /// <param name="topN"></param>
    /// <returns></returns>
    List<TEntity> FindListByTopN(int topN);
    #endregion

    #region 返回前N条记录（映射）

    /// <summary>
    /// 返回前n条记录（映射）
    /// </summary>
    /// <typeparam name="TOpt">返回类型</typeparam>
    /// <param name="topN">前N条</param>
    /// <returns></returns>
    List<TOpt> FindListByTopNTo<TOpt>(int topN);
    #endregion

    #region 返回指定条件的前几条数据（排序）

    /// <summary>
    /// 返回指定条件的前几条数据（lambda表达式）
    /// </summary>
    /// <typeparam name="TS">排序类型</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="topN">前N条</param>
    /// <param name="orderByExpression">排序Lambda表达式</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindListByTopN<TS>(Expression<Func<TEntity, bool>> condition, int topN,
        Expression<Func<TEntity, TS>> orderByExpression, bool isDesc);

    /// <summary>
    /// 返回指定条件的前几条数据
    /// </summary>
    /// <param name="condition">条件</param>
    /// <param name="topN">前N条</param>
    /// <param name="orderByProperty">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindListByTopN(Expression<Func<TEntity, bool>> condition, int topN, string orderByProperty,
        bool isDesc);

    /// <summary>
    /// 返回指定条件的前几条数据
    /// </summary>
    /// <param name="condition">条件</param>
    /// <param name="topN">前N条</param>
    /// <param name="orderByDictionary">多个排序字段</param>
    /// <returns></returns>
    List<TEntity> FindListByTopN(Expression<Func<TEntity, bool>> condition, int topN,
        Dictionary<string, bool> orderByDictionary);
    #endregion

    #region 根据排序条件返回前n条记录（映射，排序）

    /// <summary>
    /// 根据排序条件返回前n条记录(根据lambda得到前N条)
    /// </summary>
    /// <typeparam name="TS">排序类型</typeparam>
    /// <param name="topN">前N条</param>
    /// <param name="orderByExpression">排序lambda表达式</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindListByTopN<TS>(int topN, Expression<Func<TEntity, TS>> orderByExpression, bool isDesc);

    /// <summary>
    /// 返回指定条件的前几条数据(根据属性名称得到前N条)
    /// </summary>
    /// <param name="topN">前N条</param>
    /// <param name="orderByProperty">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TEntity> FindListByTopN(int topN, string orderByProperty, bool isDesc);


    /// <summary>
    /// 返回指定条件的前几条数据(根据属性名称得到前N条)
    /// </summary>
    /// <param name="topN">前N条</param>
    /// <param name="orderByDictionary">多个排序字段</param>
    /// <returns></returns>
    List<TEntity> FindListByTopN(int topN, Dictionary<string, bool> orderByDictionary);
    #endregion

    #region 返回指定条件前n条记录（映射，排序）

    /// <summary>
    /// 返回指定条件的前几条数据(根据属性名称得到前N条)
    /// </summary>
    /// <typeparam name="TOpt">映射类型</typeparam>
    /// <typeparam name="TS">排序类型</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="topN">前N条</param>
    /// <param name="orderByExpression">排序lambda表达式</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TOpt> FindListByTopNTo<TOpt, TS>(Expression<Func<TEntity, bool>> condition, int topN,
        Expression<Func<TEntity, TS>> orderByExpression, bool isDesc);

    /// <summary>
    /// 返回指定条件的前几条数据(根据属性名称得到前N条)
    /// </summary>
    /// <typeparam name="TOpt">映射类型</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="topN">前N条</param>
    /// <param name="orderByProperty">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TOpt> FindListByTopNTo<TOpt>(Expression<Func<TEntity, bool>> condition, int topN, string orderByProperty,
        bool isDesc);

    /// <summary>
    /// 返回指定条件的前几条数据(根据属性名称得到前N条)
    /// </summary>
    /// <typeparam name="TOpt">映射类型</typeparam>
    /// <param name="condition">条件</param>
    /// <param name="topN">前N条</param>
    /// <param name="orderByDictionary">多个排序字段</param>
    /// <returns></returns>
    List<TOpt> FindListByTopNTo<TOpt>(Expression<Func<TEntity, bool>> condition, int topN,
        Dictionary<string, bool> orderByDictionary);
    #endregion

    #region 根据排序条件返回前n条记录（映射，排序）

    /// <summary>
    /// 根据排序条件返回前n条记录(根据lambda得到前N条)
    /// </summary>
    /// <typeparam name="TOpt">映射类型</typeparam>
    /// <typeparam name="TS">排序类型</typeparam>
    /// <param name="topN">前N条</param>
    /// <param name="orderByExpression">排序lambda表达式</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TOpt> FindListByTopNTo<TOpt, TS>(int topN, Expression<Func<TEntity, TS>> orderByExpression, bool isDesc);

    /// <summary>
    /// 返回指定条件的前几条数据(根据属性名称得到前N条)
    /// </summary>
    /// <typeparam name="TOpt">映射类型</typeparam>
    /// <param name="topN">前N条</param>
    /// <param name="orderByProperty">排序字段</param>
    /// <param name="isDesc">是否降序</param>
    /// <returns></returns>
    List<TOpt> FindListByTopNTo<TOpt>(int topN, string orderByProperty, bool isDesc);

    /// <summary>
    /// 返回指定条件的前几条数据(根据属性名称得到前N条)
    /// </summary>
    /// <typeparam name="TOpt">映射类型</typeparam>
    /// <param name="topN">前N条</param>
    /// <param name="orderByDictionary">多个排序字段</param>
    /// <returns></returns>
    List<TOpt> FindListByTopNTo<TOpt>(int topN, Dictionary<string, bool> orderByDictionary);
    #endregion

    #endregion

  }
}
