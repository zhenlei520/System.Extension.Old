using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace EDapper.Extension
{
    /// <summary>
    /// sqlHelper帮助类
    /// </summary>
    public static class SqlMapperExtension
    {
        #region 私有属性

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private static string ConnectionString { get; set; }

        /// <summary>
        /// 数据接口链接
        /// </summary>
        private static IDbConnection Connection { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        private static DataBaseType DataBaseType { get; set; }


        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> ParamCache = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        #endregion

        #region Methods

        #region 添加数据方式

        #region 添加数据方法
        /// <summary>
        /// 添加数据方法
        /// </summary>
        /// <param name="data">数据类型，动态参数</param>
        /// <param name="table">表名</param>
        /// <param name="connection">数据源，如果使用的非SqlServer数据库的话，此值必须传值</param>
        /// <param name="transaction">存储过程</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static long Insert(dynamic data, string table, IDbConnection connection = null, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            connection = FindDbConnection(connection);
            var obj = data as object;
            var properties = FindPropertiesNameList(obj);
            var columns = string.Join(",", properties);
            var values = string.Join(",", properties.Select(p => "@" + p));
            var sql = string.Format("insert into {0} ({1}) values ({2}) select cast(scope_identity() as bigint)", table, columns, values);
            return connection.ExecuteScalar<long>(sql, obj, transaction, commandTimeout);
        }
        #endregion

        #endregion

        #region 删除数据表

        #region 删除表
        /// <summary>
        /// 删除表
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="table">表名</param>
        /// <param name="connection">数据接口链接</param>
        /// <param name="transaction">事务</param>
        /// <param name="commandTimeout">命令超时时间</param>
        /// <returns></returns>
        public static int Delete(dynamic condition, string table, IDbConnection connection = null,
            IDbTransaction transaction = null, int? commandTimeout = null)
        {
            connection = FindDbConnection(connection);
            var conditionObj = condition as object;
            var whereFields = string.Empty;
            var whereProperties = FindPropertiesNameList(conditionObj);
            if (whereProperties.Count > 0)
            {
                whereFields = " where " + string.Join(" and ", whereProperties.Select(p => p + " = @" + p));
            }
            var sql = string.Format("delete from {0}{1}", table, whereFields);
            return connection.Execute(sql, conditionObj, transaction, commandTimeout);
        }
        #endregion
        
        #endregion

        #region 更新数据表
        /// <summary>
        /// 更新数据表
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="data"></param>
        /// <param name="condition"></param>
        /// <param name="table"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static int Update(this IDbConnection connection, dynamic data, dynamic condition, string table, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var obj = data as object;
            var conditionObj = condition as object;

            var updatePropertyInfos = FindPropertyInfoList(obj);
            var wherePropertyInfos = FindPropertyInfoList(conditionObj);

            var updateProperties = updatePropertyInfos.Select(p => p.Name);
            var whereProperties = wherePropertyInfos.Select(p => p.Name);

            var updateFields = string.Join(",", updateProperties.Select(p => p + " = @" + p));
            var whereFields = string.Empty;

            var properties = whereProperties as string[] ?? whereProperties.ToArray();
            if (properties.Any())
            {
                whereFields = " where " + string.Join(" and ", properties.Select(p => p + " = @w_" + p));
            }

            var sql = string.Format("update {0} set {1}{2}", table, updateFields, whereFields);

            var parameters = new DynamicParameters(data);
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;
            wherePropertyInfos.ForEach(p => expandoObject.Add("w_" + p.Name, p.GetValue(conditionObj, null)));
            parameters.AddDynamicParams(expandoObject);

            return connection.Execute(sql, parameters, transaction, commandTimeout);
        } 
        #endregion

        #region 查询列表

        #region 得到查询列表
        /// <summary>
        /// 得到查询列表
        /// </summary>
        /// <param name="connection">数据源信息</param>
        /// <param name="condition">动态条件</param>
        /// <param name="table">表名</param>
        /// <param name="columns">查询列名</param>
        /// <param name="isOr">或者或者并且</param>
        /// <param name="transaction">需要执行的事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        private static IEnumerable<dynamic> QueryList(this IDbConnection connection, dynamic condition, string table, string columns = "*", bool isOr = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return QueryList<dynamic>(connection, condition, table, columns, isOr, transaction, commandTimeout);
        }
        #endregion

        #region 查询列表
        /// <summary>
        /// 查询列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">数据源信息</param>
        /// <param name="condition">动态条件</param>
        /// <param name="table">表名</param>
        /// <param name="columns">查询列名</param>
        /// <param name="isOr">或者或者并且</param>
        /// <param name="transaction">需要执行的事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static IEnumerable<T> QueryList<T>(this IDbConnection connection, object condition, string table, string columns = "*", bool isOr = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            return connection.Query<T>(BuildQuerySql(condition, table, columns, isOr), condition, transaction, true, commandTimeout);
        }
        #endregion

        
        #endregion

        #region 得到分页的IEnumerable类型
        /// <summary>
        /// 得到分页的IEnumerable类型
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="connection">数据源</param>
        /// <param name="condition">动态条件</param>
        /// <param name="table">表名</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="columns">查询列</param>
        /// <param name="isOr">或者或者并且</param>
        /// <param name="transaction">事物</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static IEnumerable<T> QueryPaged<T>(this IDbConnection connection, dynamic condition, string table, string orderBy, int pageIndex, int pageSize, string columns = "*", bool isOr = false, IDbTransaction transaction = null, int? commandTimeout = null)
        {
            var conditionObj = condition as object;
            var whereFields = string.Empty;
            var properties = FindPropertiesNameList(conditionObj);
            if (properties.Count > 0)
            {
                var separator = isOr ? " OR " : " AND ";
                whereFields = " WHERE " + string.Join(separator, properties.Select(p => p + " = @" + p));
            }
            var sql = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER (ORDER BY {1}) AS RowNumber, {0} FROM {2}{3}) AS Total WHERE RowNumber >= {4} AND RowNumber <= {5}", columns, orderBy, table, whereFields, (pageIndex - 1) * pageSize + 1, pageIndex * pageSize);

            return connection.Query<T>(sql, conditionObj, transaction, true, commandTimeout);
        }

        #endregion

        #endregion

        #region Public Methods

        #region 得到参数的名字集合
        /// <summary>
        /// 得到参数的名字集合
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static List<string> FindPropertiesNameList(object obj)
        {
            if (obj == null)
            {
                return new List<string>();
            }
            var parameters = obj as DynamicParameters;
            if (parameters != null)
            {
                return parameters.ParameterNames.ToList();
            }
            return FindPropertyInfoList(obj).Select(x => x.Name).ToList();
        }
        #endregion

        #region 得到动态参数的参数名与参数值
        /// <summary>
        /// 得到动态参数的参数名与参数值
        /// </summary>
        /// <param name="obj">动态参数</param>
        /// <returns></returns>
        private static List<PropertyInfo> FindPropertyInfoList(object obj)
        {
            if (obj == null)
            {
                return new List<PropertyInfo>();
            }
            List<PropertyInfo> properties;
            if (ParamCache.TryGetValue(obj.GetType(), out properties))
                return properties.ToList();
            properties = obj.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).ToList();
            ParamCache[obj.GetType()] = properties;
            return properties;
        }
        #endregion

        #region 得到连接数据源信息

        /// <summary>
        /// 得到连接数据源信息
        /// </summary>
        /// <param name="connection">用户指定的连接源</param>
        /// <returns></returns>
        private static IDbConnection FindDbConnection(IDbConnection connection)
        {
            if (connection == null && ConnectionString == null)
            {
                throw new Exception("数据源接口有误");
            }
            else if (connection == null)
            {
                connection = new SqlConnection(ConnectionString);
            }
            return connection;
        }

        #endregion

        #region 构建查询Sql语句
        /// <summary>
        /// 构建查询Sql语句
        /// </summary>
        /// <param name="condition">条件</param>
        /// <param name="table">表名</param>
        /// <param name="selectPart">查询字段</param>
        /// <param name="isOr">Or 或者 And</param>
        /// <returns></returns>
        private static string BuildQuerySql(dynamic condition, string table, string selectPart = "*", bool isOr = false)
        {
            var conditionObj = condition as object;
            var properties = FindPropertiesNameList(conditionObj);
            if (properties.Count == 0)
            {
                return string.Format("SELECT {1} FROM {0}", table, selectPart);
            }

            var separator = isOr ? " OR " : " AND ";
            var wherePart = string.Join(separator, properties.Select(p => p + " = @" + p));

            return string.Format("SELECT {2} FROM {0} WHERE {1}", table, wherePart, selectPart);
        } 
        #endregion

        #endregion

        #region 初始化规则配置
        /// <summary>
        /// 初始化规则配置(初始化配置不支持Mysql，因为支持MySql还需要在引用mysql的相关类库)
        /// </summary>
        /// <param name="connectionString">数据库连接字符串，例如：server=.;user id=root;password=;Character Set=gbk;database=dbBase</param>
        public static void InitRegularConfig(string connectionString = "")
        {
            if (!string.IsNullOrEmpty(connectionString))
                ConnectionString = connectionString;
        }

        #endregion
    }

    #region 参数类型
    /// <summary>
    /// 参数类型
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// int，适用于除int,short,long,等数字类型，bool类型
        /// </summary>
        Int = 0,
        /// <summary>
        /// 字符串，需要加单引号，适用于除int,short,long,等数字类型,布尔类型外的其他所有类型
        /// </summary>
        String = 1,
        /// <summary>
        /// 可空
        /// </summary>
        NullAble = 2
    }
    #endregion

    #region 数据库类型
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DataBaseType
    {
        /// <summary>
        /// 微软SqlServer
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// Mysql数据库
        /// </summary>
        Mysql = 1,
    }

    #endregion
}
