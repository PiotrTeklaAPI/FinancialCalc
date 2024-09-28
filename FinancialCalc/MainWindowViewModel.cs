using FinancialCalc.BaseClasses;
using FinancialCalc.Command;
using FinancialCalc.Constants;
using FinancialCalc.EventArgs;
using FinancialCalc.Helpers;
using FinancialCalc.Objects;
using FinancialCalc.Services;
using FinancialCalc.ViewModels;
using FinancialCalc.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
            OnLoadCommand = new AsyncCommand(LoadProductsAndFileInfoAsync);

            StatusBarMessage = EntryMessage;
            FileInformation = new FileInfo(pathHelper.GetCurrentFullFilePath(), pathHelper.GetCurrentFileName(), DateTime.Now);
            FileMonths = SetFileMonths();
            SelectedFileMonth = FileMonths.FirstOrDefault();
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

        private ObservableCollection<Product> products = new();

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

        public List<string> FileMonths { get; private set; }

        private string selectedFileMonth = string.Empty;

        public string SelectedFileMonth
        {
            get => selectedFileMonth;
            set
            {
                selectedFileMonth = value;
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
            productViewModel.AddRequested += ProductViewModel_AddModifyRequested;
            ProductView productView = new ProductView(productViewModel);
            productView.Show();
        }

        private void ProductViewModel_AddModifyRequested(object sender, FinancialCalcEventArgs e)
        {
            var newProduct = e.Product;

            if (Products.FirstOrDefault(product => product.Name.Equals(newProduct.Name)) is { } editableProduct && editableProduct != null)
            {
                editableProduct.CostNet = newProduct.CostNet;
                editableProduct.VatRateType = newProduct.VatRateType;
                editableProduct.Date = newProduct.Date;
            }
            else
            {
                Products.Add(newProduct);
            }
            NotifyPropertyChanged(nameof(Products));
        }

        private void OnModify(object obj)
        {
            ProductViewModel productViewModel = new(SelectedProduct, FileInformation);
            productViewModel.AddRequested += ProductViewModel_AddModifyRequested;
            ProductView productView = new(productViewModel);
            productView.Show();
        }

        private void OnDelete(object obj)
        {
            if (Products.Remove(SelectedProduct))
            {
                StatusBarMessage = "Selected cost has been removed.";
                NotifyPropertyChanged(nameof(Products));
            }
            else
            {
                StatusBarMessage = "Failed to remove selected cost.";
            }
        }

        private void OnSave(object obj)
        {
            if (SerializeToFile(FileInformation.Path) is false)
            {
                StatusBarMessage = "Failed to save file.";
                return;
            }

            FileMonths = SetFileMonths();
        }

        private void OnLoad(object obj)
        {
            var filePath = pathHelper.GetCurrentFullFilePath();
            if (TryLoadProductsAndFileInfo(filePath) is false)
            {
                return;
            }

            NotifyPropertyChanged(nameof(FileInformation));
            NotifyPropertyChanged(nameof(Products));
        }

        #endregion

        #region Methods

        private bool TryLoadProductsAndFileInfo(string filePath)
        {
            if (XFile.FileExists(filePath) is false)
            {
                StatusBarMessage = "File for picked month doesn't exist.";
                return false;
            }

            if (XFile.ReadFile(filePath, out string data) is false)
            {
                StatusBarMessage = "Invalid file. Failed to read file.";
                return false;
            }

            if (DeserializeToObjects(filePath, out FileInfo fileInfo, out List<Product> products) is false)
            {
                return false;
            }

            Products.Clear();
            foreach (var product in products)
            {
                Products.Add(product);
            }
            FileInformation = fileInfo;

            return true;
        }

        private async Task LoadProductsAndFileInfoAsync()
        {
            var filePath = FileInformation.Path;

            if (XFile.FileExists(filePath) is false)
            {
                StatusBarMessage = "File for picked month doesn't exist.";
                return;
            }

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    var serializedData = await reader.ReadToEndAsync();

                    if (serializedData is null)
                    {
                        throw new Exception("Serialized data is null.");
                    }

                    if (jsonService.TryDeserializeObject(serializedData, out FileData fileData) is false)
                    {
                        throw new Exception("Failed to deserialize file info.");
                    }

                    if (fileData.Products.Any() is false)
                    {
                        throw new Exception("No products stored in the file.");
                    }

                    Products.Clear();
                    foreach (var product in fileData.Products)
                    {
                        Products.Add(product);
                    }

                    FileInformation = fileData.FileInfo;
                }
            }
            catch (Exception ex)
            {
                StatusBarMessage = ex.Message;
            }
        }

        private bool SerializeToFile(string filePath)
        {
            var data = new
            {
                FileInfo = FileInformation,
                Products
            };

            var serializeData = JsonConvert.SerializeObject(data, Formatting.Indented);

            if (XFile.SaveToFile(serializeData, filePath, true) is false)
            {
                return false;
            }

            return true;
        }

        private bool DeserializeToObjects(string filePath, out FileInfo fileInfo, out List<Product> products)
        {
            fileInfo = new FileInfo();
            products = new List<Product>();

            if (string.IsNullOrEmpty(filePath))
                return false;

            if (XFile.ReadFile(filePath, out string serializedData) is false)
                return false;

            var data = JsonConvert.DeserializeObject<dynamic>(serializedData);

            if (jsonService.TryDeserializeObject<FileInfo>(Convert.ToString(data.FileInfo), out fileInfo) is false)
            {
                StatusBarMessage = "Failed to deserialize file info.";
                return false;
            }

            if (jsonService.TryDeserializeObject<List<Product>>(Convert.ToString(data.Products), out products) is false)
            {
                StatusBarMessage = "Failed to deserialize products.";
                return false;
            }

            if (products.Any() is false)
            {
                StatusBarMessage = "No products stored in the file.";
                return false;
            }

            return true;
        }

        private List<string> GetFileNames(string path, string[] months)
        {
            var existingFileNames = new List<string>();

            foreach (var month in months)
            {
                if (string.IsNullOrEmpty(month))
                {
                    continue;
                }
                var monthFilePaths = Directory.GetFiles(path, month + "*").ToList();
                monthFilePaths.ForEach(monthFilePath => existingFileNames.Add(Path.GetFileName(monthFilePath)));
            }

            return existingFileNames;
        }

        private List<string> SetFileMonths()
        {
            return GetFileNames(PathHelper.GetDesktopPath(), new CultureInfo("en-US").DateTimeFormat.MonthNames);
        }
        #endregion
    }
}
