namespace EInfrastructure.Infrastructure.Interfaces
{
    /// <summary>
    /// 启动运行类
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        void Execute();

        /// <summary>
        /// 得到执行顺序
        /// </summary>
        /// <returns></returns>
        int GetOrder();
    }
}
