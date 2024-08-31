using System;

namespace FinancialCalc.Attributes
{
    public class Description : Attribute
    {
        public string Value { get; set; }

        public Description(string value)
        {
            Value = value;
        }
    }
}
