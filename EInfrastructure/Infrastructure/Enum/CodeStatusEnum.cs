namespace EInfrastructure.Infrastructure.Enum
{
    /// <summary>
    /// 状态
    /// </summary>
    public enum CodeStatusEnum
    {
        /// <summary>
        /// 成功
        /// </summary>
        Ok = 200,
        /// <summary>
        /// 错误
        /// </summary>
        Err = 500,
        /// <summary>
        /// 未发现
        /// </summary>
        NoFind=404,
        /// <summary>
        /// 重复
        /// </summary>
        Repeat=101
    }
}
