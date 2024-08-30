using FinancialCalc.BaseClasses;
using FinancialCalc.Enums;
using Newtonsoft.Json;

namespace FinancialCalc.Objects
{
    public class Product
    {
        [JsonConstructor]
        public Product() { }

        public Product(string  name, VatRateType rateType, double costNet)
        {
            Name = name;
            VatRateType = rateType;
            CostNet = costNet;
        }

        [JsonProperty]
        public VatRateType VatRateType { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public double CostNet { get; set; }

        [JsonProperty]
        public double CostGross
        {
            get
            {
                return CostNet * VatRateType.ToPercentage();
            }
        }
    }
}
