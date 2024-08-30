using System;

namespace FinancialCalc.Attributes
{
    public class Percentage : Attribute
    {
        public double Value { get; set; }

        public Percentage(double value)
        {
            Value = value;
        }
    }
}
