using Newtonsoft.Json;
using System;

namespace EInfrastructure.HelpCommon.Serialization.JsonAdapter
{
    public class NewtonsoftJsonProvider : BaseJsonProvider
    {
        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override string Serializer(object o)
        {
            return JsonConvert.SerializeObject(o);
        }

        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object Deserialize(string s, Type type)
        {
            return JsonConvert.DeserializeObject(s, type);
        }
    }
}
