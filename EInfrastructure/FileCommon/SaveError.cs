using System;
using System.Collections.Generic;
using System.Web.UI;

namespace EInfrastructure.FileCommon
{
    /// <summary>
    /// 保存错误
    /// </summary>
    public partial class SaveError : Page
    {
        #region Fields
        /// <summary>
        /// Fields
        /// </summary>
        public static string Xmlpath;
        #endregion

        #region Methods

        #region 得到错误文件的地址
        public void Getpath(string path)
        {
            Xmlpath = Server.MapPath(path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".xml");
        }
        #endregion

        #region 保存错误文件
        /// <summary>
        /// 保存错误文件
        /// </summary>
        /// <param name="attDic"></param>
        /// <param name="path"></param>
        public static void Save(Dictionary<string, string> attDic, string path)
        {
            try
            {
                if (Xmlpath == null)
                {
                    new SaveError().Getpath(path);
                }
                if (XmlUtil.CreateDocument(Xmlpath, "root"))
                {
                    XmlUtil.InsertElement(Xmlpath, "root", "error", attDic, "");
                }
            }
            catch
            {
                // ignored
            }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 保存错误方法扩展
    /// </summary>
    public partial class SaveError
    {
        #region 保存错误
        /// <summary>
        /// 保存错误
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="errUrl">出错的url地址</param>
        /// <param name="filePath">错误地址</param>
        /// <param name="urlparam">url参数</param>
        /// <param name="formparam">form参数</param>
        /// <param name="userDefineErr">用户自定义错误</param>
        public static void Save(Exception ex, string errUrl, string filePath, string urlparam, string formparam, string userDefineErr)
        {
            string exurl = errUrl;
            Dictionary<string, string> attDic = new Dictionary<string, string>
            {
                {"错误的信息", ex.Message},
                {"错误的堆栈", ex.StackTrace},
                {"出错的方法名", ex.TargetSite.Name}
            };
            if (ex.TargetSite.DeclaringType != null) attDic.Add("出错的类名", ex.TargetSite.DeclaringType.FullName);
            if (string.IsNullOrEmpty(formparam))
            {
                attDic.Add("form参数", formparam);
            }
            if (ex.Message.Contains("要复制的 LOB 数据的长度"))
            {
                attDic.Add("url参数", "输入的内容过长");
            }
            else
            {
                if (!string.IsNullOrEmpty(urlparam))
                {
                    attDic.Add("url参数", urlparam);
                }
            }
            if (!string.IsNullOrEmpty(exurl))
            {
                attDic.Add("出错页面", exurl);
            }
            if (!string.IsNullOrEmpty(userDefineErr))
            {
                attDic.Add("自定义错误", userDefineErr);
            }
            attDic.Add("出错时间", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
            Save(attDic, filePath);
        }
        #endregion
    }
}
