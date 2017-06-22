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
    /// Request������
    /// </summary>
    public class DntRequest
    {
        /// <summary>
        /// �жϵ�ǰҳ���Ƿ���յ���Post����
        /// </summary>
        /// <returns>�Ƿ���յ���Post����</returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.ToUpper().Equals("POST");
        }
        /// <summary>
        /// �жϵ�ǰҳ���Ƿ���յ���Get����
        /// </summary>
        /// <returns>�Ƿ���յ���Get����</returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.ToUpper().Equals("GET");
        }

        /// <summary>
        /// ��ȡ������Ϣ��form��get�ύ��
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
        /// ��ȡ����������Ϣ��form��get�ύ��
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
        /// ����ָ���ķ�����������Ϣ
        /// </summary>
        /// <param name="strName">������������</param>
        /// <returns>������������Ϣ</returns>
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
        /// ������һ��ҳ��ĵ�ַ
        /// </summary>
        /// <returns>��һ��ҳ��ĵ�ַ</returns>
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
        /// �õ���ǰ��������ͷ
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
        /// �õ�����ͷ
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            return HttpContext.Current.Request.Url.Host;
        }


        /// <summary>
        /// ��ȡ��ǰ�����ԭʼ URL(URL ������Ϣ֮��Ĳ���,������ѯ�ַ���(�������))
        /// </summary>
        /// <returns>ԭʼ URL</returns>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }

        /// <summary>
        /// �жϵ�ǰ�����Ƿ�������������
        /// </summary>
        /// <returns>��ǰ�����Ƿ�������������</returns>
        public static bool IsBrowserGet()
        {
            string[] browserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
            string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
            return browserName.Any(t => curBrowser.IndexOf(t, StringComparison.Ordinal) >= 0);
        }


        /// <summary>
        /// �ж��Ƿ�����������������
        /// </summary>
        /// <returns>�Ƿ�����������������</returns>
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
        /// ��õ�ǰ����Url��ַ
        /// </summary>
        /// <returns>��ǰ����Url��ַ</returns>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }


        /// <summary>
        /// ���ָ��Url������ֵ
        /// </summary>
        /// <param name="strName">Url����</param>
        /// <returns>Url������ֵ</returns>
        public static string GetQueryString(string strName)
        {
            if (HttpContext.Current.Request.QueryString[strName] == null)
            {
                return "";
            }
            return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString[strName]);
        }

        /// <summary>
        /// ��ȡָ����URL��������ֵ
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
        /// ��õ�ǰҳ�������
        /// </summary>
        /// <returns>��ǰҳ�������</returns>
        public static string GetPageName()
        {
            string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
            return urlArr[urlArr.Length - 1].ToLower();
        }

        /// <summary>
        /// ���ر���Url�������ܸ���
        /// </summary>
        /// <returns></returns>
        public static int GetParamCount()
        {
            return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
        }


        /// <summary>
        /// ���ָ����������ֵ
        /// </summary>
        /// <param name="strName">������</param>
        /// <returns>��������ֵ</returns>
        public static string GetFormString(string strName)
        {
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// ���ָ��������������ֵ���Ϊ���ⷵ��-1
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
        /// ���Url���������ֵ, ���ж�Url�����Ƿ�Ϊ���ַ���, ��ΪTrue�򷵻ر�������ֵ
        /// </summary>
        /// <param name="strName">����</param>
        /// <returns>Url���������ֵ</returns>
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
        /// ��õ�ǰҳ��ͻ��˵�IP
        /// </summary>
        /// <returns>��ǰҳ��ͻ��˵�IP</returns>
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
        /// �����û��ϴ����ļ�
        /// </summary>
        /// <param name="path">����·��</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }
        /// <summary>
        /// �Ƴ�query�����е�ĳ������
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
            //�߼��������ٽ��ж���ɸѡ�������
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
        /// �Ƴ�query�����е�ĳ�����������Ƴ�page����
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
            //�߼��������ٽ��ж���ɸѡ�������
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
