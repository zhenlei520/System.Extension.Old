using System;

namespace EInfrastructure.HelpCommon
{
    /// <summary>
    /// 唯一方法实现
    /// </summary>
    public class UniqueCommon
    {
        #region 全局唯一Guid
        /// <summary>
        /// 全局唯一Guid
        /// </summary>
        public static string Guids => Guid.NewGuid().ToString().Replace("-", "");

        #endregion

        #region 判断是否为Guid
        /// <summary>
        /// 判断是否为Guid
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static bool IsGuid(string strSrc)
        {
            Guid g;
            return Guid.TryParse(strSrc, out g);
        }
        #endregion
    }
}

