﻿using System;
using System.Text.RegularExpressions;
using System.Web;
using EInfrastructure.Infrastructure.Entity;

namespace EInfrastructure.HelpCommon
{
    public static class ValidateCommon
    {
        //邮件正则表达式
        private static readonly Regex Emailregex = new Regex(@"^([a-z0-9]*[-_]?[a-z0-9]+)*@([a-z0-9]*[-_]?[a-z0-9]+)+[\.][a-z]{2,3}([\.][a-z]{2})?$", RegexOptions.IgnoreCase);
        //手机号正则表达式
        private static readonly Regex Mobileregex = new Regex("^(13|14|15|16|17|18|19)[0-9]{9}$");
        //固话号正则表达式
        private static readonly Regex Phoneregex = new Regex(@"^(\d{3,4}-?)?\d{7,8}$");
        //邮政编码正则表达式
        private static readonly Regex ZipCodeRegex = new Regex(@"^\d{6}$");
        //数字正则表达式
        private static readonly Regex Numberregex = new Regex(@"^\d*");
        //IP正则表达式
        private static readonly Regex Ipregex = new Regex(@"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        //网址正则表达式
        private static readonly Regex WebSiteRegex = new Regex(@"((http|https)://)?(www.)?[a-z0-9\.]+(\.(com|net|cn|com\.cn|com\.net|net\.cn))(/[^\s\n]*)?");

        #region 是否邮政编码
        /// <summary>
        /// 是否邮政编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsZipCode(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return ZipCodeRegex.IsMatch(s);
        }
        #endregion

        #region 是否为邮箱名
        /// <summary>
        /// 是否为邮箱名
        /// </summary>
        public static bool IsEmail(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return Emailregex.IsMatch(s);
        }
        #endregion

        #region 是否为手机号
        /// <summary>
        /// 是否为手机号
        /// </summary>
        public static bool IsMobile(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return Mobileregex.IsMatch(s);
        }
        #endregion

        #region 是否为固话号
        /// <summary>
        /// 是否为固话号
        /// </summary>
        public static bool IsPhone(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return Phoneregex.IsMatch(s);
        }
        #endregion

        #region 是否数字
        /// <summary>
        /// 是否数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNumber(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            return Numberregex.IsMatch(s);
        }
        #endregion

        #region 是否为纯数字
        /// <summary>
        /// 是否为纯数字
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsOnlyNumber(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return false;
            int? result;
            try
            {
                result = int.Parse(s);
            }
            catch (Exception)
            {
                result = null;
            }
            return result != null;
        }
        #endregion

        #region 是否是身份证号
        /// <summary>
        /// 是否是身份证号
        /// </summary>
        public static bool IsIdCard(this string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;
            if (id.Length == 18)
                return CheckIdCard18(id);
            else if (id.Length == 15)
                return CheckIdCard15(id);
            else
                return false;
        }

        /// <summary>
        /// 是否为18位身份证号
        /// </summary>
        private static bool CheckIdCard18(this string id)
        {
            long n;
            if (long.TryParse(id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(id.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2), StringComparison.Ordinal) == -1)
                return false;//省份验证

            string birth = id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());

            int y;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != id.Substring(17, 1).ToLower())
                return false;//校验码验证

            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 是否为15位身份证号
        /// </summary>
        private static bool CheckIdCard15(this string id)
        {
            long n;
            if (long.TryParse(id, out n) == false || n < Math.Pow(10, 14))
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(id.Remove(2), StringComparison.Ordinal) == -1)
                return false;//省份验证

            string birth = id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time;
            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            return true;//符合15位身份证标准
        }
        #endregion

        #region 是否为IP
        /// <summary>
        /// 是否为IP
        /// </summary>
        public static bool IsIp(string s)
        {
            return Ipregex.IsMatch(s);
        } 
        #endregion

        #region 判断是否网址
        /// <summary>
        ///  判断是否网址
        /// </summary>
        /// <param name="webUrl">网址</param>
        /// <returns></returns>
        public static bool IsWebSite(this string webUrl)
        {
            if (string.IsNullOrEmpty(webUrl))
                return false;
            return WebSiteRegex.IsMatch(webUrl);
        }
        #endregion

        #region 判断请求类型

        #region 是否是get请求
        /// <summary>
        /// 是否是get请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod == "GET";
        }
        #endregion

        #region 是否是post请求
        /// <summary>
        /// 是否是post请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod == "POST";

        }
        #endregion

        #region 是否是Ajax请求
        /// <summary>
        /// 是否是Ajax请求
        /// </summary>
        /// <returns></returns>
        public static bool IsAjax()
        {
            return HttpContext.Current.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        #endregion

        #endregion

        #region 判断是否是浏览器请求
        /// <summary>
        /// 判断是否是浏览器请求
        /// </summary>
        /// <returns></returns>
        public static bool IsBrowser()
        {
            string name = WebHelpCommon.GetBrowserName();
            foreach (string item in WebEntity.Browserlist)
            {
                if (name.Contains(item))
                    return true;
            }
            return false;
        }
        #endregion

        #region 是否是移动设备请求
        /// <summary>
        /// 是否是移动设备请求
        /// </summary>
        /// <returns></returns>
        public static Boolean IsMobile()
        {
            bool isMoblie = false;
            if (HttpContext.Current.Request.UserAgent != null)
            {
                foreach (string item in WebEntity.MobileAgents)
                {
                    if (HttpContext.Current.Request.UserAgent.ToLower().IndexOf(item, StringComparison.Ordinal) >= 0)
                    {
                        isMoblie = true;
                        break;
                    }
                }
            }
            return isMoblie;
        }
        #endregion

        #region 判断是否是搜索引擎爬虫请求
        /// <summary>
        /// 判断是否是搜索引擎爬虫请求
        /// </summary>
        /// <returns></returns>
        public static bool IsCrawler()
        {
            bool result = HttpContext.Current.Request.Browser.Crawler;
            if (!result)
            {
                string referrer = WebHelpCommon.GetUrlReferrer();
                if (referrer.Length > 0)
                {
                    foreach (string item in WebEntity.Searchenginelist)
                    {
                        if (referrer.Contains(item))
                            return true;
                    }
                }
            }
            return result;
        } 
        #endregion


    }
}
