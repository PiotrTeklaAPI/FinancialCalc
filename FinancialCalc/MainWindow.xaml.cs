using Microsoft.Win32;
using System.Windows;

namespace FinancialCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SettingsControl_OpenClicked(object sender, System.EventArgs e)
        {
            if (DataContext is not MainWindowViewModel viewModel)
            {
                MessageBox.Show($"Failed to get data context.");
                return;
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Select a file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                viewModel.FileInformation.Path = openFileDialog.FileName;
            }
        }
    }
}
