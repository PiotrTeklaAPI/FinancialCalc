using FinancialCalc.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FinancialCalc.Views
{
    /// <summary>
    /// Interaction logic for DataGridView.xaml
    /// </summary>
    public partial class DataGridView : UserControl
    {
        public DataGridView()
        {
            InitializeComponent();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(DataGrid.DataContext is MainWindowViewModel mainWindowViewModel))
            {
                return;
            }

            if (sender is DataGrid dataGrid &&
                GetClickedRow(dataGrid, e) is { } row &&
                row != null)
            {
                if (dataGrid.SelectedItem is Product selectedItem &&
                    IsRowInSelectedRow(row, selectedItem))
                {
                    mainWindowViewModel.OnDoubleClickCommand.Execute(selectedItem);
                }
            }
        }

        private DataGridRow GetClickedRow(DataGrid dataGrid, MouseButtonEventArgs eventArgs)
        {
            if (!(eventArgs.OriginalSource is DependencyObject hitRow))
            {
                return null;
            }

            while (hitRow != null && !(hitRow is DataGridRow))
            {
                hitRow = VisualTreeHelper.GetParent(hitRow);
            }

            if (hitRow is DataGridRow clickedRow)
            {
                return clickedRow;
            }

            return null;
        }

        private bool IsRowInSelectedRow(DataGridRow row, Product selectedRow)
        {
            if (row != null && row.DataContext is Product rowDataContext)
            {
                return rowDataContext == selectedRow;
            }

            return false;
        }

        private DataGridRow GetDataGridRow(DataGrid dataGrid, DataGridCell cell)
        {
            var row = VisualTreeHelper.GetParent(cell);
            while (row != null && !(row is DataGridRow))
            {
                row = VisualTreeHelper.GetParent(row);
            }
            return row as DataGridRow;
        }
    }
}
