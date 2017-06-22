﻿using EInfrastructure.HelpCommon.Serialization.JsonAdapter;
using System;
using EInfrastructure.Infrastructure.Exception;

namespace EInfrastructure.HelpCommon.Serialization
{
    /// <summary>
    /// json 序列化方式
    /// </summary>
    public class JsonCommon : BaseJsonProvider
    {
        private readonly EnumJsonMode _jsonMode;
        public JsonCommon(EnumJsonMode mode = EnumJsonMode.Newtonsoft)
        {
            _jsonMode = mode;
        }

        private BaseJsonProvider CreateJsonProvider()
        {
            if (_jsonMode == EnumJsonMode.JavaScript)
            {
                return new JavaScriptJsonProvider();
            }
            if (_jsonMode == EnumJsonMode.JavaScriptBussiness)
            {
                return new JavaScriptBussinessJsonProvider();
            }
            else if (_jsonMode == EnumJsonMode.Newtonsoft)
            {
                return new NewtonsoftJsonProvider();
            }
            else if (_jsonMode == EnumJsonMode.DataContract)
            {
                return new DataContractJsonProvider();
            }
            throw new BusinessException("未找到相应的json序列化Provider");
        }

        /// <summary>
        /// jason序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public override string Serializer(object o)
        {
            try
            {
                return CreateJsonProvider().Serializer(o);
            }
            catch (Exception)
            {
                throw new BusinessException($"json序列化出错,jsonMode:{1},序列化类型：{o.GetType().FullName}");
            }
        }

        /// <summary>
        /// jason反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="s"></param>
        /// <returns></returns>
        public T Deserialize<T>(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return default(T);
            }
            try
            {
                return (T)CreateJsonProvider().Deserialize(s, typeof(T));
            }
            catch (Exception)
            {
                throw new BusinessException($"json反序列化出错,jsonMode:{1},内容：{s}");
            }
        }

        /// <summary>
        /// jason反序列化
        /// </summary>
        /// <param name="s"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public override object Deserialize(string s, Type type)
        {
            try
            {
                return CreateJsonProvider().Deserialize(s, type);
            }
            catch (Exception)
            {
                throw new BusinessException($"json反序列化出错,jsonMode:{1},内容：{s}");
            }
        }
    }
}
