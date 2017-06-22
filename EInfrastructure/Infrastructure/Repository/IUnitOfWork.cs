using System;
using System.Threading.Tasks;

namespace EInfrastructure.Infrastructure.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        #region Interface

        #region Commit
        /// <summary>
        /// Commit all changes made in  a container.
        /// </summary>
        ///<remarks>
        /// If entity have fixed properties and optimistic concurrency problem exists 
        /// exception is thrown
        ///</remarks>
        int Commit();
        #endregion

        Task<int> CommitAsync();

        #region CommitAndRefreshChanges
        /// <summary>
        /// Commit all changes made in  a container.
        /// </summary>
        ///<remarks>
        /// If entity have fixed properties and optimistic concurrency problem exists 
        /// client changes are refereshed
        ///</remarks>
        void CommitAndRefreshChanges();
        #endregion

        #region RollbackChanges
        /// <summary>
        /// Rollback changes not stored in databse at 
        /// this moment. See references of UnitOfWork pattern
        /// </summary>
        void RollbackChanges();
        #endregion

        #endregion
    }
}
