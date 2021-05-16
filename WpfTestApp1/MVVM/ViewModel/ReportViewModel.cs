using QBalanceDesktop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfTestApp1.Core;
using WpfTestApp1.MVVM.Model;

namespace WpfTestApp1.MVVM.ViewModel
{
    public class ReportViewModel : ObservableObject, IRefreshAble
    {
        public object SelectedReportType { get; set; }
        public RelayCommand ShowReportCommand { get; set; }

        public ReportContent CurrentReportContent { get; set; }
        private DataTable _g;
        public DataTable ReportDS
        {
            get
            {
                return _g;
            }
            set
            {
                _g = value;
                OnPropertyChanged();
                OnPropertyChanged("CurrentReportTitle");
            }
        }

        public string CurrentReportTitle
        {
            get
            {
                return CurrentReportContent?.Title;
            }
        }

        public ReportViewModel()
        {
            LoadReport();
            ShowReportCommand = new RelayCommand(parameter =>
            {
                LoadReport();
            });
        }

        private void LoadReport()
        {
            if (SelectedReportType != null)
            {
                var currentReport = (ReportTypeEnum)Convert.ToInt32(SelectedReportType);
                var manager = new ReportManager();
                CurrentReportContent = manager.GenerateReport(currentReport);
                ReportDS = CurrentReportContent.Table;
            }
        }

        public void Refresh()
        {
            LoadReport();
        }
    }
}
