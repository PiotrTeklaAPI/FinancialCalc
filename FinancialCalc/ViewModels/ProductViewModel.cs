using FinancialCalc.BaseClasses;
using FinancialCalc.Command;
using FinancialCalc.Enums;
using FinancialCalc.EventArgs;
using FinancialCalc.Objects;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace FinancialCalc.ViewModels
{
    public class ProductViewModel : ViewModelBase
    {
        #region Fields

        private readonly FileInfo fileInformation;

        #endregion

        #region Constructor

        public ProductViewModel(Product product, FileInfo fileInformation)
        {
            Product = product.Clone() as Product;
            this.fileInformation = fileInformation;

            SetValues();
            OnOkCommand = new DelegateCommand(OnOk);
        }

        public ProductViewModel()
        {        
            SetValues();

            OnOkCommand = new DelegateCommand(OnOk);
            Product = new Product();
        }

        #endregion

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

        public ICommand OnOkCommand { get; protected set; }

        #endregion

        #region Methods

        private void OnOk(object parameter)
        {
            var eventArgs = new FinancialCalcEventArgs
            {
                Product = this.Product
            };

            AddRequested?.Invoke(this, eventArgs);
        }

        private void SetValues()
        {
            VatRates = XEnum.EnumToList<VatRateType>();
        }

        #endregion
    }
}
