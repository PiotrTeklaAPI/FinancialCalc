using FinancialCalc.BaseClasses;
using FinancialCalc.Enums;
using Newtonsoft.Json;
using System;

namespace FinancialCalc.Objects
{
    public class Product : ObjectToObserve, ICloneable
    {
        [JsonConstructor]
        public Product() { }

        public Product(string  name, VatRateType rateType, double costNet)
        {
            Name = name;
            VatRateType = rateType;
            CostNet = costNet;
            Date = DateTime.Now;
        }

        private VatRateType vatRateType;

        [JsonProperty]
        public VatRateType VatRateType
        {
            get => vatRateType;
            set
            {
                vatRateType = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CostGross));
            }
        }

        private string name = string.Empty;

        [JsonProperty]
        public string Name
        {
            get => name;
            set
            {
                name = value;
                NotifyPropertyChanged();
            }
        }

        private double costNet;

        [JsonProperty]
        public double CostNet
        {
            get => costNet;
            set
            {
                costNet = value;
                NotifyPropertyChanged();
                NotifyPropertyChanged(nameof(CostGross));
            }
        }

        [JsonProperty]
        public double CostGross
        {
            get
            {
                return CostNet * VatRateType.ToPercentage() + CostNet;
            }
        }

        private DateTime? date;

        [JsonProperty]
        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                NotifyPropertyChanged();
            }
        }

        public object Clone()
        {
            return new Product
            {
                Name = Name,
                VatRateType = VatRateType,
                CostNet = CostNet,
                Date = Date
            };
        }

        public override bool Equals(object obj)
        {
            if(obj is Product product)
            {
                return Name.Equals(product.Name) &&
                       CostNet == product.CostNet &&
                       VatRateType == product.VatRateType;
            }
            
            return false;
        }
    }
}
