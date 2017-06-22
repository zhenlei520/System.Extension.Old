using System;

namespace EInfrastructure.HelpCommon
{
    /// <summary>
    /// 抛出错误异常
    /// </summary>
    public class Assert
    {
        /// <summary>
        /// 为空错误
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isExcu">判断是否需要执行</param>
        public static void IsNotNull(string name, bool isExcu = false)
        {
            if (isExcu || !string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{name}不能为空");
            }
        }

        /// <summary>
        /// 为空错误
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isExcu">判断是否需要执行</param>
        public static void IsNotNullOrEmpty(string name, bool isExcu = false)
        {
            if (isExcu || !string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{name}不能为空");
            }
        }

        /// <summary>
        /// 为空错误
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isExcu">判断是否需要执行</param>
        public static void IsNotNullOrWhiteSpace(string name, bool isExcu = false)
        {
            if (isExcu || !string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{name}不能为空");
            }
        }

        /// <summary>
        /// 类型错误
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isExcu">判断是否需要执行</param>
        public static void IsTypeErr(string name, bool isExcu = false)
        {
            if (isExcu || !string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{name}类型错误");
            }
        }

        /// <summary>
        /// 完全自定义异常
        /// </summary>
        /// <param name="name"></param>
        public static void IsException(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                throw new ArgumentException(name);
            }
        }

        /// <summary>
        /// 两个字符串不相等
        /// </summary>
        /// <param name="id1">字符串1</param>
        /// <param name="id2">字符串2</param>
        /// <param name="errorMessageFormat">错误内容</param>
        public static void AreEqual(string id1, string id2, string errorMessageFormat)
        {
            if (id1 != id2)
            {
                throw new ArgumentException(string.Format(errorMessageFormat, id1, id2));
            }
        }
    }
}
