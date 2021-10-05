using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
using WpfTestApp1.MVVM.Model.Automation;

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

            var filename = @"D:\WpfTestApp1 Test files\rp1.html";

            //DatatbleToHtml(filename);
            // html to xslx


            // xlsx to datatble
            //var newTbl = ExcelUtlity.ConvertExcelToDataTable(filename);
        }

        private static void DatatbleToHtml(string filename)
        {
            var lpKeys = new ExcelParameters();
            //lpKeys.MainHeaders.Add("תכניות לימוד");
            var lpsTablep = lpKeys.GetNewtableParameter();

            lpsTablep.table = GlobalsProviderBL.Db.ListTableBySql("select *  FROM [deskopBalanceDb_prod_backUp].[dbo].[FinandaTransactions]");

            using (var fst = new FileStream(filename, FileMode.Create))
            {
                ExcelUtlity.CreateExcelDocument(lpKeys, fst);
            }
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void btnIncrement_Click(object sender, RoutedEventArgs e)
        {
            //var latestBudget = GlobalsProviderBL.Db.GetData<Budget>().OrderByDescending(x => x.Month).First();
            //AutomationHelper.GenerateBudget(GlobalsProviderBL.Db, latestBudget);
            RefreshView();
        }
    }
}
