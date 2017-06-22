﻿using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EInfrastructure.HelpCommon.Serialization.JsonAdapter
{
    public class DataContractJsonProvider : BaseJsonProvider
    {
        /// <summary>
        /// 默认格式 UTF8
        /// </summary>
        public Encoding EncodingFormat = Encoding.UTF8;

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override string Serializer(object o)
        {
            DataContractJsonSerializer json = new DataContractJsonSerializer(o.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                json.WriteObject(stream, o);
                return EncodingFormat.GetString(stream.ToArray());
            }
        }


        /// <summary>
        /// json反序列化
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object Deserialize(string s, Type type)
        {
            using (MemoryStream ms = new MemoryStream(EncodingFormat.GetBytes(s)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
                return serializer.ReadObject(ms);
            }
        }
    }
}
