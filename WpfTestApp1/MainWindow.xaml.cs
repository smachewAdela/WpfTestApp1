using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace WpfTestApp1
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

        public MVVM.ViewModel.MaunViewModel ViewModel
        {
            get { return (MVVM.ViewModel.MaunViewModel)this.DataContext; }
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
          
            RefreshView();
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            //BindingOperations.GetBindingExpressionBase(txTitle, TextBlock.TextProperty).UpdateTarget();
            //BindingOperations.GetBindingExpressionBase(dContent, ContentControl.ContentProperty).UpdateSource();
        }
    }
}
