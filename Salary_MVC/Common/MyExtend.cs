using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Salary_MVC.Common
{
    public static class MyExtend
    {
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstend">当枚举没有定义DescriptionAttribute,是否用枚举名代替，默认使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this System.Enum value, bool nameInstend = true)
        {
            Type type = value.GetType();
            string name = System.Enum.GetName(type, value);
            if (name == null)
            {
                return null;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstend == true)
            {
                return name;
            }
            return attribute == null ? null : attribute.Description;
        }

        public static DataTable ToDataTable<T>(this List<T> items)
        {
            var tb = new DataTable(typeof(T).Name);

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in props)
            {
                Type t = GetCoreType(prop.PropertyType);
                tb.Columns.Add(prop.Name, t);
            }

            foreach (T item in items)
            {
                var values = new object[props.Length];

                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                tb.Rows.Add(values);
            }

            return tb;
        }

        /// <summary>
        /// Determine of specified type is nullable
        /// </summary>
        public static bool IsNullable(Type t)
        {
            return !t.IsValueType || (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Return underlying type if type is Nullable otherwise return the type
        /// </summary>
        public static Type GetCoreType(Type t)
        {
            if (t != null && IsNullable(t))
            {
                if (!t.IsValueType)
                {
                    return t;
                }
                else
                {
                    return Nullable.GetUnderlyingType(t);
                }
            }
            else
            {
                return t;
            }
        }

        public static decimal DecimalValue(this string value)
        {
            decimal result;
            if(!decimal.TryParse(value, out result))
            {
                result = 0;
            }
            return result;
        }

        public static string ToString2(this DateTime value, DateTimeFormat format)
        {
            string tmp = string.Empty;
            switch (format)
            {
                case DateTimeFormat.yyyyMMdd:
                    tmp = "yyyy-MM-dd";
                    break;
                case DateTimeFormat.yyyyMM:
                    tmp = "yyyy-MM";
                    break;
                case DateTimeFormat.yyyyMMddHHmm:
                    tmp = "yyyy-MM-dd HH:mm";
                    break;
                default:
                    throw new ArgumentException("不支持的format");
            }
            return value.ToString(tmp);
        }

        
    }
    public enum DateTimeFormat
    {
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        yyyyMMdd,
        /// <summary>
        /// yyyy-MM
        /// </summary>
        yyyyMM,
        /// <summary>
        /// yyyy-MM-dd HH:mm
        /// </summary>
        yyyyMMddHHmm
    }
}