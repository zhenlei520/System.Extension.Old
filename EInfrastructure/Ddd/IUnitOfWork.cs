
using System.Threading.Tasks;

namespace EInfrastructure.Ddd
{
    public interface IUnitOfWork
    {
        /// <summary>
        /// 提交
        /// </summary>
        /// <returns></returns>
        bool Commit();

        /// <summary>
        /// 异步提交更改
        /// </summary>
        /// <returns></returns>
        Task<int> CommitAsync();
    }

}
