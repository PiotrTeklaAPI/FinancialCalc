using FinancialCalc.ViewModels;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
