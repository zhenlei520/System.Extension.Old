using System;
using System.Web;

namespace EInfrastructure.HelpCommon
{
    /// <summary>
    /// Cookie操作帮助,此类需要添加对System.Web的引用
    /// </summary>
    public static class CookieCommon
    {
        #region  删除指定名称的Cookie
        /// <summary>
        /// 删除指定名称的Cookie
        /// </summary>
        /// <param name="name">Cookie名称</param>
        public static void DeleteCookie(this string name)
        {
            HttpCookie cookie = new HttpCookie(name) { Expires = DateTime.Now.AddYears(-1) };
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 获得指定名称的Cookie值
        /// <summary>
        /// 获得指定名称的Cookie值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <returns></returns>
        public static string GetCookie(this string name)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
                return cookie.Value;

            return string.Empty;
        }
        #endregion

        #region 获得指定名称的Cookie中特定键的值
        /// <summary>
        /// 获得指定名称的Cookie中特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetCookie(this string name, string key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null && cookie.HasKeys)
            {
                string v = cookie[key];
                if (v != null)
                    return v;
            }
            return string.Empty;
        }
        #endregion

        #region 设置指定名称的Cookie的值
        /// <summary>
        /// 设置指定名称的Cookie的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">值</param>
        public static void SetCookie(this string name, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null)
                cookie.Value = value;
            else
                cookie = new HttpCookie(name, value);

            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 设置指定名称的Cookie的值
        /// <summary>
        /// 设置指定名称的Cookie的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(this string name, string value, double expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name] ?? new HttpCookie(name);

            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 设置指定名称的Cookie特定键的值
        /// <summary>
        /// 设置指定名称的Cookie特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetCookie(this string name, string key, string value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name] ?? new HttpCookie(name);

            cookie[key] = value;
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

        #region 设置指定名称的Cookie特定键的值
        /// <summary>
        /// 设置指定名称的Cookie特定键的值
        /// </summary>
        /// <param name="name">Cookie名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expires">过期时间</param>
        public static void SetCookie(this string name, string key, string value, double expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[name] ?? new HttpCookie(name);

            cookie[key] = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        #endregion

    }
}
