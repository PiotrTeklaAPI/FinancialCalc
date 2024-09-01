using FinancialCalc.BaseClasses;
using FinancialCalc.Command;
using FinancialCalc.Constants;
using FinancialCalc.EventArgs;
using FinancialCalc.Helpers;
using FinancialCalc.Objects;
using FinancialCalc.Services;
using FinancialCalc.ViewModels;
using FinancialCalc.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using FileInfo = FinancialCalc.Objects.FileInfo;

namespace FinancialCalc
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly JsonService jsonService;
        private readonly ProjectConstants constants;
        private readonly PathHelper pathHelper;
        private const string EntryMessage = "Financial Calc";

        public MainWindowViewModel()
        {
            jsonService = new JsonService();
            constants = new ProjectConstants();
            pathHelper = new PathHelper(constants);

            OnAddCommand = new DelegateCommand(OnAdd);
            OnModifyCommand = new DelegateCommand(OnModify);
            OnDeleteCommand = new DelegateCommand(OnDelete);
            OnSaveCommand = new DelegateCommand(OnSave);
            OnLoadCommand = new DelegateCommand(OnLoad);

            StatusBarMessage = EntryMessage;
            FileInformation = new FileInfo(pathHelper.GetCurrentFullFilePath(), pathHelper.GetCurrentFileName(), DateTime.Now);
        }

        #region Properties

        public FileInfo FileInformation { get; private set; }

        private Product selectedProduct;

        public Product SelectedProduct
        {
            get => selectedProduct;
            set
            {
                selectedProduct = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        public ObservableCollection<Product> Products
        {
            get => products;
            set
            {
                products = value;
                NotifyPropertyChanged();
            }
        }

        private string statusBarMessage = string.Empty;

        public string StatusBarMessage
        {
            get => statusBarMessage;
            set
            {
                statusBarMessage = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Events

        public ICommand OnAddCommand { get; protected set; }

        public ICommand OnModifyCommand { get; protected set; }

        public ICommand OnDeleteCommand { get; protected set; }

        public ICommand OnSaveCommand { get; protected set; }

        public ICommand OnLoadCommand { get; protected set; }

        private void OnAdd(object obj)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.AddRequested += ProductViewModel_AddRequested;
            ProductView productView = new ProductView(productViewModel);
            productView.Show();
        }

        private void ProductViewModel_AddRequested(object sender, FinancialCalcEventArgs e)
        {
            var newProduct = e.Product;

            if (Products.Any(x => x.Equals(newProduct)))
            {
                StatusBarMessage = "Cannot load new cost. It has been already added.";
                return;
            }

            Products.Add(newProduct);
            NotifyPropertyChanged(nameof(Products));
        }

        private void OnModify(object obj)
        {

        }

        private void OnDelete(object obj)
        {
            if (!(obj is Product product))
            {
                return;
            }
        }

        private void OnSave(object obj)
        {
            if (SerializeToFile(FileInformation.Path) is false)
            {
                StatusBarMessage = "Failed to save file.";
                return;
            }
        }

        private void OnLoad(object obj)
        {
            var productsFromFile = GetProducts();
            Products.Clear();
            foreach (var product in productsFromFile)
            {
                Products.Add(product);
            }

            NotifyPropertyChanged(nameof(Products));
        }

        #endregion

        #region Methods

        private List<Product> GetProducts()
        {
            var products = new List<Product>();
            var filePath = pathHelper.GetCurrentFullFilePath();

            if(XFile.FileExists(filePath) is false)
            {
                StatusBarMessage = "File for current month doesn't exist yet.";
                return products;
            }

            if (XFile.ReadFile(filePath, out string data) is false)
            {
                return products;
            }

            if (jsonService.TryDeserializeObject(data, out products) is false)
            {
                return products;
            }

            return products;
        }

        private bool SerializeToFile(string filePath)
        {
            //if (jsonService.TrySerializeObject(FileInformation, out string fineInfoData) is false)
            //{
            //    return false;
            //}

            //if (XFile.SaveToFile(fineInfoData, filePath) is false)
            //{
            //    return false;
            //}

            if (jsonService.TrySerializeObject(Products, out string productsData) is false)
            {
                return false;
            }

            if (XFile.SaveToFile(productsData, filePath) is false)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
