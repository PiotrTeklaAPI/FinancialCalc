using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinancialCalc.UserControls
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        [Category("Buttons")]
        [Description("Raised when Open button is clicked")]
        public event EventHandler OpenClicked;

        #region Properties

        public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register(
         "AddCommand", typeof(ICommand), typeof(SettingsControl), new PropertyMetadata());

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
        "DeleteCommand", typeof(ICommand), typeof(SettingsControl), new PropertyMetadata());

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly DependencyProperty ModifyCommandProperty = DependencyProperty.Register(
        "ModifyCommand", typeof(ICommand), typeof(SettingsControl), new PropertyMetadata());

        public ICommand ModifyCommand
        {
            get { return (ICommand)GetValue(ModifyCommandProperty); }
            set { SetValue(ModifyCommandProperty, value); }
        }

        public static readonly DependencyProperty SaveCommandProperty = DependencyProperty.Register(
        "SaveCommand", typeof(ICommand), typeof(SettingsControl), new PropertyMetadata());

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }

        public static readonly DependencyProperty LoadCommandProperty = DependencyProperty.Register(
        "LoadCommand", typeof(ICommand), typeof(SettingsControl), new PropertyMetadata());

        public ICommand LoadCommand
        {
            get { return (ICommand)GetValue(LoadCommandProperty); }
            set { SetValue(LoadCommandProperty, value); }
        }

        #endregion

        public void open_Click(object sender, RoutedEventArgs e)
        {
            this.OpenClicked?.Invoke(this, e);
        }
    }
}
