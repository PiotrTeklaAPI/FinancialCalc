using FinancialCalc.BaseClasses;
using FinancialCalc.Enums;
using FinancialCalc.EventArgs;
using FinancialCalc.Objects;
using System;
using System.Collections.Generic;

namespace FinancialCalc.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        private readonly FileInfo fineInformation;
        public ProductViewModel(Product product, FileInfo fineInformation)
        {
            Product = product.Clone() as Product;
            this.fineInformation = fineInformation;

            SetValues();
        }

        public ProductViewModel()
        {        
            SetValues();
        }

        #region Properties

        private Product product;

        public Product Product
        {
            get => product;
            set
            {
                product = value;
                NotifyPropertyChanged();
            }
        }

        public List<VatRateType> VatRates { get; set; } = new List<VatRateType>();

        #endregion

        #region Events

        public event EventHandler<FinancialCalcEventArgs> AddRequested;

        #endregion

        #region Methods

        private void SetValues()
        {
            VatRates = XEnum.EnumToList<VatRateType>();
        }

        #endregion
    }
}
