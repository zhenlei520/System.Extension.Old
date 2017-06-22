using System.Collections.Generic;

namespace EInfrastructure.Infrastructure.Repository
{
    /// <summary>
    /// 分页数据集合
    /// </summary>
    public class PageData<T>
    {
        #region Fields

        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 当前页数据集合
        /// </summary>
        public List<T> DataList { get; set; }
        #endregion
    }
}
