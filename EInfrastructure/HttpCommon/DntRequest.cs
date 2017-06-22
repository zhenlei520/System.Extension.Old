using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using EInfrastructure.FileCommon;
using EInfrastructure.HelpCommon;

namespace EInfrastructure.HttpCommon
{
    /// <summary>
    /// Request操作类
    /// </summary>
    public class DntRequest
    {
        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns>是否接收到了Post请求</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.ToUpper().Equals("POST");
        }
        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns>是否接收到了Get请求</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.ToUpper().Equals("GET");
        }

        /// <summary>
        /// 获取参数信息（form或get提交）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetParamString(string param)
        {
            if (IsPost())
            {
                return GetFormString(param);
            }
            else if (IsGet())
            {
                return GetQueryString(param);
            }
            return "";
        }

        /// <summary>
        /// 获取参数数字信息（form或get提交）
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetParamNumber(string param)
        {
            if (IsPost())
            {
                return GetFormNumber(param);
            }
            else if (IsGet())
            {
                return GetQueryNumber(param);
            }
            return "-1";
        }

        /// <summary>
        /// 返回指定的服务器变量信息
        /// </summary>
        /// <param name="strName">服务器变量名</param>
        /// <returns>服务器变量信息</returns>
        public static string GetServerString(string strName)
        {
            //
            if (HttpContext.Current.Request.ServerVariables[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Request.ServerVariables[strName];
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch
            {
                // ignored
            }

            if (retVal == null)
                return "";

            return retVal;

        }

        /// <summary>
        /// 得到当前完整主机头
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentFullHost()
        {
            HttpRequest request = HttpContext.Current.Request;
            if (!request.Url.IsDefaultPort)
            {
                return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
            }
            return request.Url.Host;
        }

        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }


        /// <summary>
        /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        /// <returns>原始 URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// 判断当前访问是否来自浏览器软件
        /// </summary>
        /// <returns>当前访问是否来自浏览器软件</returns>
        public static bool IsBrowserGet()
        {
            string[] browserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            return browserName.Any(t => curBrowser.IndexOf(t, StringComparison.Ordinal) >= 0);
        }


        /// <summary>
        /// 判断是否来自搜索引擎链接
        /// </summary>
        /// <returns>是否来自搜索引擎链接</returns>
        public static bool IsSearchEnginesGet()
        {
            if (HttpContext.Current.Request.UrlReferrer == null)
            {
                return false;
            }
            string[] searchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
            string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
            return searchEngine.Any(t => tmpReferrer.IndexOf(t, StringComparison.Ordinal) >= 0);
        }

        /// <summary>
        /// 获得当前完整Url地址
        /// </summary>
        /// <returns>当前完整Url地址</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }


        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString[strName]);
        }

        /// <summary>
        /// 获取指定的URL参数数字值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="outstr"></param>
        /// <returns></returns>
        public static string GetQueryNumber(string strName, string outstr = "-1")
        {
            if (outstr == null) throw new ArgumentNullException("outstr");
            if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString[strName]))
            {
                return outstr;
            }
            return HttpContext.Current.Request.QueryString[strName];
        }

        /// <summary>
        /// 获得当前页面的名称
        /// </summary>
        /// <returns>当前页面的名称</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// 返回表单或Url参数的总个数
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }


        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string GetFormString(string strName)
        {
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得指定表单参数的数字值如果为空这返回-1
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public static string GetFormNumber(string strName)
        {
            if (string.IsNullOrEmpty(HttpContext.Current.Request.Form[strName]))
            {
                return "-1";
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
        /// </summary>
        /// <param name="strName">参数</param>
        /// <returns>Url或表单参数的值</returns>
        public static string GetString(string strName)
        {
            if ("".Equals(GetQueryString(strName)))
            {
                return GetFormString(strName);
            }
            else
            {
                return GetQueryString(strName);
            }
        }


        /// <summary>
        /// 获得当前页面客户端的IP
        /// </summary>
        /// <returns>当前页面客户端的IP</returns>
        public static string GetIp()
        {
            var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }

            if (string.IsNullOrEmpty(result) || !ValidateCommon.IsIp(result))
            {
                return "127.0.0.1";
            }

            return result;

        }

        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }
        /// <summary>
        /// 移出query参数中的某个参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string RemoveQueryParam(string param)
        {
            string querys = HttpContext.Current.Request.Url.Query.Replace("?", string.Empty);
            List<string> list = querys.ConvertStrToList<string>('&');
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
            //高级搜索后再进行二次筛选会出问题
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


        /// <summary>
        /// 移出query参数中的某个参数并且移出page参数
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string RemoveQueryParam_new(string param)
        {
            string querys = HttpContext.Current.Request.Url.Query.Replace("?", string.Empty);
            List<string> list = querys.ConvertStrToList<string>('&');
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
            //高级搜索后再进行二次筛选会出问题
            foreach (string p in list)
            {
                if (p.Contains(param))
                {
                    list.Remove(p);
                    break;
                }
            }

            foreach (string p in list)
            {
                if (p.Contains("page"))
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
    }
}
