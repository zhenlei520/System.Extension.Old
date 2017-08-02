﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EInfrastructure.HelpCommon
{
    /// <summary>
    /// 枚举
    /// </summary>
    public static class EnumCommon
    {
        private static Hashtable _enumDesciption;

        static EnumCommon()
        {
            _enumDesciption = GetDescriptionContainer();
        }

        private static Hashtable GetDescriptionContainer()
        {
            _enumDesciption = new Hashtable();
            return _enumDesciption;
        }

        #region 得到枚举字典（key对应枚举的值，value对应枚举的注释）
        /// <summary>
        /// 得到枚举字典（key对应枚举的值，value对应枚举的注释）
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> ToDescriptionDictionary<TEnum>()
        {
            Array values = Enum.GetValues(typeof(TEnum));
            Dictionary<int, string> nums = new Dictionary<int, string>();
            foreach (Enum value in values)
            {
                nums.Add(Convert.ToInt32(value), GetDescription(value));
            }
            return nums;
        }
        #endregion

        #region 得到枚举字典（key与value与字典值一致）
        /// <summary>
        /// 得到枚举字典（key与value与字典值一致）
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public static Dictionary<int, string> ToDictionary<TEnum>()
        {
            Array values = Enum.GetValues(typeof(TEnum));
            Dictionary<int, string> nums = new Dictionary<int, string>();
            foreach (Enum value in values)
            {
                nums.Add(Convert.ToInt32(value), value.ToString());
            }
            return nums;
        }
        #endregion

        #region 返回枚举项的描述信息
        /// <summary>
        /// 返回枚举项的描述信息。
        /// </summary>
        /// <param name="value">要获取描述信息的枚举项。</param>
        /// <returns>枚举想的描述信息。</returns>
        public static string GetDescription(this Enum value)
        {
            Type enumType = value.GetType();
            // 获取枚举常数名称。
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                // 获取枚举字段。
                FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    // 获取描述的属性。
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }
        #endregion

        #region 判断值是否在枚举类型中存在
        /// <summary>
        /// 判断值是否在枚举中存在
        /// </summary>
        /// <param name="enumValue">需要判断的参数</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static bool IsExist(this int enumValue, Type enumType)
        {
            return Enum.IsDefined(enumType, enumValue);
        }

        /// <summary>
        /// 判断值是否在枚举中存在
        /// </summary>
        /// <param name="enumValue">需要判断的参数</param>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static bool IsExist(this int? enumValue, Type enumType)
        {
            if (enumValue == null)
                return false;
            return ((int)enumValue).IsExist(enumType);
        }
        #endregion
    }
}
