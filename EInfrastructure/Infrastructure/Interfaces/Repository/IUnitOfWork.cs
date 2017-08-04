using System;
using System.Threading.Tasks;

namespace EInfrastructure.Infrastructure.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {

        /// <summary>
        /// 提交更改
        /// </summary>
        /// <returns></returns>
        int Commit();

        /// <summary>
        /// 异步提交
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();

        /// <summary>
        /// 提交并刷新更改
        /// </summary>
        void CommitAndRefreshChanges();

        /// <summary>
        /// 回滚
        /// </summary>
        void RollbackChanges();

        /// <summary>
        /// 使用事物，为保存存储过程与上下文一致（后续需要存储过程的返回值）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="func">委托（得到存储过程返回的值）</param>
        /// <param name="action">委托（执行剩余的业务）</param>
        /// <param name="funResult">存储过程返回值信息</param>
        /// <returns></returns>
        int UseTransaction<T>(Func<T> func, Action<T> action, out T funResult);

        /// <summary>
        /// 使用事物，为保存存储过程与上下文一致（后续需要存储过程的返回值）
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="func">委托（得到存储过程返回的值）</param>
        /// <param name="action">委托（执行剩余的业务）</param>
        /// <returns></returns>
        int UseTransaction<T>(Func<T> func, Action<T> action);

        /// <summary>
        /// 使用事物，为保存存储过程与上下文一致（两个委托无任务关联）
        /// </summary>
        /// <param name="action1">委托1，执行事物的委托</param>
        /// <param name="action2">委托2，一般委托</param>
        /// <returns></returns>
        int UseTransaction(Action action1, Action action2);
    }
}
