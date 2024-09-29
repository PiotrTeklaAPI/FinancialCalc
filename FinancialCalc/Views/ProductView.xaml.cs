using FinancialCalc.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace FinancialCalc.Views
{
    /// <summary>
    /// Interaction logic for ProductView.xaml
    /// </summary>
    public partial class ProductView : Window
    {
        public ProductView(ProductViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            if(datePicker.SelectedDate is null)
            {
                datePicker.SelectedDate = System.DateTime.Now;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is ProductViewModel vm)
            {
                vm.OnXButtonCommand.Execute(null);
            }
            base.OnClosing(e);
        }
    }
}
