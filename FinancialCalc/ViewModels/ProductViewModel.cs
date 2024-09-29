using FinancialCalc.BaseClasses;
using FinancialCalc.Command;
using FinancialCalc.Enums;
using FinancialCalc.EventArgs;
using FinancialCalc.Objects;
using FinancialCalc.Views;
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
            OnCloseCommand = new DelegateCommand(OnClose);
            OnXButtonCommand = new DelegateCommand(OnXButton);
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

        public event EventHandler<FinancialCalcEventArgs> XButtonClicked;

        public event EventHandler<FinancialCalcEventArgs> CloseRequested;

        public ICommand OnOkCommand { get; protected set; }

        public ICommand OnCloseCommand { get; protected set; }

        public ICommand OnXButtonCommand { get; protected set; }

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

        private void OnClose(object obj)
        {
            if (obj is ProductView view)
            {
                var eventArgs = new FinancialCalcEventArgs();
                CloseRequested?.Invoke(this, eventArgs);
                view.Close();
            }
        }

        private void OnXButton(object obj)
        {
            var eventArgs = new FinancialCalcEventArgs();
            XButtonClicked?.Invoke(this, eventArgs);
        }

        private void SetValues()
        {
            VatRates = XEnum.EnumToList<VatRateType>();
        }

        #endregion
    }
}
