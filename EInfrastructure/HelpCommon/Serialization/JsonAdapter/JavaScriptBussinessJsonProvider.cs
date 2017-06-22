using System;
using System.Text.RegularExpressions;

namespace EInfrastructure.HelpCommon.Serialization.JsonAdapter
{
    /// <summary>
    /// JavaScriptSerializer 业务方式Json序列化
    /// System.Web.Script.Serialization
    /// 备注:解决常规业务中对json序列化的要求，形成序列化标准，比如datetime格式要求
    /// </summary>
    public class JavaScriptBussinessJsonProvider: JavaScriptJsonProvider
    {
        public override string Serializer(object o)
        {
            var json = base.Serializer(o);
            //通过正则表达式解决datetime格式的问题,性能会些许浪费,但对于业务通用性还是值得的
            //此格式可以被正常反序列化
            json = Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match =>
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                dt = dt.ToLocalTime();
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            });
            return json;
        }

       
    }
}
