using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;

namespace EInfrastructure.HelpCommon
{
    /// <summary>
    /// Web帮助类
    /// </summary>
    public static class WebHelpCommon
    {
        #region 获得请求的浏览器名称
        /// <summary>
        /// 获得请求的浏览器名称
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserName()
        {
            string name = HttpContext.Current.Request.Browser.Browser;
            if (string.IsNullOrEmpty(name) || name == "unknown")
                return "未知";

            return name.ToLower();
        }
        #endregion

        #region 获得请求的浏览器版本
        /// <summary>
        /// 获得请求的浏览器版本
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserVersion()
        {
            string version = HttpContext.Current.Request.Browser.Version;
            if (string.IsNullOrEmpty(version) || version == "unknown")
                return "未知";

            return version;
        }
        #endregion

        #region 获得请求的浏览器类型
        /// <summary>
        /// 获得请求的浏览器类型
        /// </summary>
        /// <returns></returns>
        public static string GetBrowserType()
        {
            string type = HttpContext.Current.Request.Browser.Type;
            if (string.IsNullOrEmpty(type) || type == "unknown")
                return "未知";

            return type.ToLower();
        } 
        #endregion

        #region 获得请求客户端的操作系统名称
        /// <summary>
        /// 获得请求客户端的操作系统名称
        /// </summary>
        /// <returns></returns>
        public static string GetOsName()
        {
            string name = HttpContext.Current.Request.Browser.Platform;
            if (string.IsNullOrEmpty(name))
                return "未知";

            return name;
        }
        #endregion

        #region 获得请求的ip
        /// <summary>
        /// 获得请求的ip
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string ip = string.Empty;
            if (HttpContext.Current != null)
            {
                ip = HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (string.IsNullOrEmpty(ip) || !ValidateCommon.IsIp(ip))
                ip = "127.0.0.1";
            return ip;
        } 
        #endregion

        #region 获得上次请求的url
        /// <summary>
        /// 获得上次请求的url
        /// </summary>
        /// <returns></returns>
        public static string GetUrlReferrer()
        {
            Uri uri = HttpContext.Current.Request.UrlReferrer;
            if (uri == null)
                return string.Empty;
            return uri.ToString();
        }
        #endregion

        #region 转换为静态html
        /// <summary>
        /// 转换为静态html
        /// </summary>
        public static void TransHtml(string path, string outpath)
        {
            Page page = new Page();
            StringWriter writer = new StringWriter();
            page.Server.Execute(path, writer);
            FileStream fs;
            if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
            {
                File.Delete(page.Server.MapPath("") + "\\" + outpath);
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            else
            {
                fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            }
            byte[] bt = Encoding.Default.GetBytes(writer.ToString());
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }
        #endregion

        #region 获取请求参数

        #region 获得参数列表
        /// <summary>
        /// 获得参数列表
        /// </summary>
        /// <param name="urlParameter">url参数</param>
        /// <returns></returns>
        public static NameValueCollection GetParmList(string urlParameter)
        {
            NameValueCollection parmList = new NameValueCollection(StringComparer.OrdinalIgnoreCase);
            if (!string.IsNullOrEmpty(urlParameter))
            {
                int length = urlParameter.Length;
                for (int i = 0; i < length; i++)
                {
                    int startIndex = i;
                    int endIndex = -1;
                    while (i < length)
                    {
                        char c = urlParameter[i];
                        if (c == '=')
                        {
                            if (endIndex < 0)
                                endIndex = i;
                        }
                        else if (c == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string key;
                    string value;
                    if (endIndex >= 0)
                    {
                        key = urlParameter.Substring(startIndex, endIndex - startIndex);
                        value = urlParameter.Substring(endIndex + 1, (i - endIndex) - 1);
                    }
                    else
                    {
                        key = urlParameter.Substring(startIndex, i - startIndex);
                        value = string.Empty;
                    }
                    parmList[key] = value;
                    if ((i == (length - 1)) && (urlParameter[i] == '&'))
                        parmList[key] = string.Empty;
                }
            }
            return parmList;
        } 
        #endregion

        #region 移出query参数中的某个参数
        /// <summary>
        /// 移出query参数中的某个参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string RemoveQueryParam(string param)
        {
            string querys = HttpContext.Current.Request.Url.Query.Replace("?", string.Empty);
            List<string> list = querys.ConvertStrToList<string>();
            if (list.Count < 1)
                return string.Empty;
            foreach (string p in list)
            {
                if (p.Contains(param))
                {
                    list.Remove(p);
                    break;
                }
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("?");
            foreach (string q in list)
            {
                sb.Append(q + "&");
            }
            return sb.ToString();
        }
        #endregion

        #region Url参数转T
        /// <summary>
        /// 根据Url参数得到实体类 （允许存在url参数多于实体属性的情况，未赋值字段均为默认值）
        /// 实体类属性允许类型int,int?,string,datetime,datetime?,byte,byte?,Guid,Guid?
        /// 例如:User:id,name,sex
        /// 传值为：id=1&name=zhangsan
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objString"></param>
        /// <returns></returns>
        public static T GetObjectByUrlParameter<T>(string objString) where T : class
        {
            string[] objArray = objString.Split('&');
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            PropertyInfo[] propertties = typeof(T).GetProperties();
            Object obj = assembly.CreateInstance(typeof(T).FullName);
            foreach (PropertyInfo propertyInfo in propertties)
            {
                foreach (var item in objArray)
                {
                    if (propertyInfo.Name == item.Split('=')[0])
                    {
                        SetValue(obj, propertyInfo, item.Split('=')[1]);
                        break;
                    }
                }
            }
            return obj as T;
        }

        #region 为属性赋值

        /// <summary>
        /// 为属性赋值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="propertyInfo"></param>
        /// <param name="par"></param>
        /// <returns></returns>
        private static void SetValue(Object obj, PropertyInfo propertyInfo, string par)
        {
            if (propertyInfo.PropertyType == typeof(string))
                propertyInfo.SetValue(obj, par);
            else if (propertyInfo.PropertyType == typeof(int)|| propertyInfo.PropertyType == typeof(int?))
            {
                propertyInfo.SetValue(obj, par.ConvertToInt());
            }
            else if (propertyInfo.PropertyType == typeof(byte)|| propertyInfo.PropertyType == typeof(byte?))
            {
                propertyInfo.SetValue(obj, par.ConvertToByte());
            }
            else if (propertyInfo.PropertyType == typeof(bool)|| propertyInfo.PropertyType == typeof(bool?))
            {
                propertyInfo.SetValue(obj, par.ConvertToBool());
            }
            else if (propertyInfo.PropertyType == typeof(Guid)|| propertyInfo.PropertyType == typeof(Guid?))
            {
                propertyInfo.SetValue(obj, par.ConvertToGuid());
            }
            else if (propertyInfo.PropertyType == typeof(DateTime)|| propertyInfo.PropertyType == typeof(DateTime?))
            {
                propertyInfo.SetValue(obj, par.ConvertToDateTime());
            }
        }

        #endregion

        #endregion

        #endregion

    }
}
