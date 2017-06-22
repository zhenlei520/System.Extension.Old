using System;
using System.Web.Script.Serialization;

namespace EInfrastructure.HelpCommon.Serialization.JsonAdapter
{
    /// <summary>
    /// JavaScriptSerializer 方式Json序列化
    /// System.Web.Script.Serialization
    /// </summary>
    public class JavaScriptJsonProvider : BaseJsonProvider
    {
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override string Serializer(object o)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(o);

        }


        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object Deserialize(string s, Type type)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Deserialize(s, type);

        }
    }
}
