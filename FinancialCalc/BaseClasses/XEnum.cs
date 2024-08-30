using FinancialCalc.Attributes;
using System;
using System.Linq;

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
            if (value == null)
            {
                return 0.0;
            }

            return value.GetAttribute<Percentage>()?.Value ?? 0.0;
        }
    }
}
