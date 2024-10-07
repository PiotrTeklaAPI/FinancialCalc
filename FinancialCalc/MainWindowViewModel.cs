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
        private readonly FileWatcherService fileWatcherService;
        private ProductViewModel productViewModel;
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
            OnDoubleClickCommand = new DelegateCommand(OnDoubleMouseClicked);
            OnLoadCommand = new AsyncCommand(LoadProductsAndFileInfoAsync);

            StatusBarMessage = EntryMessage;
            FileInformation = new FileInfo(pathHelper.GetCurrentFullFilePath(), pathHelper.GetCurrentFileName(), DateTime.Now);

            fileWatcherService = new FileWatcherService(pathHelper.GetDataFolderPath(), "*.json");
            fileWatcherService.FileChanged += OnFileChanged;

            FileNames = GetFullFileNames(pathHelper.GetDataFolderPath());
            if(FileNames.Any())
            {
                SelectedFileName = FileNames.FirstOrDefault();
            }
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

        private bool isModifyWindowOpen = false;
        public bool IsModifyWindowOpen
        {
            get => isModifyWindowOpen;
            set
            {
                isModifyWindowOpen = value;
                NotifyPropertyChanged();
            }
        }

        private List<string> fileNames = new();

        public List<string> FileNames
        {
            get => fileNames;
            set
            {
                fileNames = value;
                NotifyPropertyChanged();
            }
        }

        private string selectedFileName = string.Empty;
        public string SelectedFileName
        {
            get => selectedFileName;
            set
            {
                selectedFileName = value;
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

        public ICommand OnDoubleClickCommand { get; protected set; }

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
            if (IsModifyWindowOpen)
            {
                UpdateProductViewModel(selectedProduct);
            }
            else
            {
                productViewModel = new(SelectedProduct, FileInformation);
                productViewModel.AddRequested += ProductViewModel_AddModifyRequested;
                productViewModel.CloseRequested += OnCloseModifyViewModel;
                productViewModel.XButtonClicked += OnCloseModifyViewModel;
                ProductView productView = new(productViewModel);
                productView.Show();

                IsModifyWindowOpen = true;
            }
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

            StatusBarMessage = "File saved.";
        }

        private void OnDoubleMouseClicked(object obj)
        {
            if (obj is Product selectedProduct)
            {
                if (IsModifyWindowOpen)
                {
                    UpdateProductViewModel(selectedProduct);
                }
                else
                {
                    productViewModel = new(selectedProduct, FileInformation);
                    productViewModel.AddRequested += ProductViewModel_AddModifyRequested;
                    productViewModel.CloseRequested += OnCloseModifyViewModel;
                    productViewModel.XButtonClicked += OnCloseModifyViewModel;
                    ProductView productView = new(productViewModel);
                    productView.Show();
                    IsModifyWindowOpen = true;
                }
            }
        }

        private void OnCloseModifyViewModel(object obj, FinancialCalcEventArgs e)
        {
            IsModifyWindowOpen = false;
        }

        private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            // Update the FileNames list on file change
            FileNames = GetFullFileNames(pathHelper.GetDataFolderPath());
        }

        #endregion

        #region Methods

        private async Task LoadProductsAndFileInfoAsync()
        {
            var filePath = pathHelper.GetFilePath(SelectedFileName);

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

        private List<string> GetFullFileNames(string path)
        {
            var existingFileNames = new List<string>();
            var months = GeteMonths();

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

        private IEnumerable<string> GeteMonths()
        {
            return new CultureInfo("en-US").DateTimeFormat.MonthNames.Where(month => !string.IsNullOrEmpty(month));
        }

        private void UpdateProductViewModel(Product product)
        {
            var currentSelectedProduct = product.Clone() as Product;
            productViewModel.Product = product;
        }

        #endregion
    }
}
