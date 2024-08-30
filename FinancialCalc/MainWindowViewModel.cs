﻿using FinancialCalc.BaseClasses;
using FinancialCalc.Constants;
using FinancialCalc.Helpers;
using FinancialCalc.Objects;
using FinancialCalc.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FinancialCalc
{
    public class MainWindowViewModel : ViewModel
    {
        private readonly JsonService _jsonService;
        private readonly ProjectConstants _constants;

        public MainWindowViewModel()
        {
            _jsonService = new JsonService();
            _constants = new ProjectConstants();
        }

        #region Properties

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

        #endregion

        #region Methods

        private List<Product> GetProducts()
        {
            var products = new List<Product>();
            var filePath = PathHelper.GetDesktopPath();

            if(XFile.ReadFile(filePath, out string data) is false)
            {
                return products;
            }

            if(_jsonService.TryDeserializeObject(data, out products) is false)
            {
                return products;
            }

            return products;
        }


        #endregion
    }
}
