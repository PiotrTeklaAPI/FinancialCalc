using FinancialCalc.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FinancialCalc.BaseClasses
{
    public static class XEnum
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            return value.GetType().GetMember(value.ToString())[0].GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault() as T;
        }

        public static double ToPercentage(this Enum value)
        {
            if (value is null)
            {
                return 0.0;
            }

            return value.GetAttribute<Percentage>()?.Value ?? 0.0;
        }

        public static string ToDescription(this Enum value)
        {
            if (value is null)
            {
                return string.Empty;
            }

            return value.GetAttribute<Description>().Value ?? string.Empty;
        }

        public static List<T> EnumToList<T>() where T : Enum
        {
            if(!typeof(T).IsEnum)
            {
                throw new ArgumentException("Given T value isn't enum.");
            }

            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static List<double> EnumPercentageToList<T>() where T : Enum
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Given T value isn't enum.");
            }

            var enums = EnumToList<T>();
            return enums.Select(x => x.ToPercentage()).ToList();
        }

        public static List<string> EnumDescriptionToList<T>() where T : Enum
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("Given T value isn't enum.");
            }

            var enums = EnumToList<T>();
            return enums.Select(x => x.ToDescription()).ToList();
        }

        public static bool HasAttribute<T>(this Enum value) where T : Attribute
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            if(fieldInfo.GetCustomAttribute(typeof(T), false) != null)
            {
                return true;
            }

            return false;
        }
    }
}
