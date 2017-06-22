using System;
using System.IO;
using System.Web;

namespace EInfrastructure.FileCommon
{
    /// <summary>
    /// 文件帮助类
    /// </summary>
    public class FileCommon
    {
        #region 建立文件夹
        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="absolutePath">文件夹绝对路径</param>
        /// <returns></returns>
        public static bool CreateDirectory(string absolutePath)
        {
            if (!ExistDirectory(absolutePath))
                Directory.CreateDirectory(absolutePath);
            return true;
        }
        #endregion

        #region 删除文件夹
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="absolutePath">文件夹绝对路径</param>
        /// <returns></returns>
        public static bool DeleteDirectory(string absolutePath)
        {
            if (ExistDirectory(absolutePath))
                Directory.Delete(absolutePath);
            return true;
        }
        #endregion

        #region 修改文件夹名称
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="absolutePath">文件夹绝对路径</param>
        /// <param name="newAbsolutePathName">新文件夹绝对路径</param>
        /// <returns></returns>
        public static bool ReNameDirectory(string absolutePath,string newAbsolutePathName)
        {
            if (ExistDirectory(absolutePath))
                Directory.Move(absolutePath, newAbsolutePathName);
            return true;
        }

        #endregion

        #region 判断是否存在某个文件夹
        /// <summary>
        /// 判断是否存在某个文件夹
        /// </summary>
        /// <param name="absolutePath">文件夹绝对路径</param>
        /// <returns></returns>
        public static bool ExistDirectory(string absolutePath)
        {
            return Directory.Exists(absolutePath);
        }
        #endregion

        #region 获得当前绝对路径
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(strPath);
            }
            else //非web程序引用
            {
                strPath = strPath.Replace("/", "\\");
                if (strPath.StartsWith("\\"))
                {
                    strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
            }
        } 
        #endregion
    }
}
