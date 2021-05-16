using System;
using System.Collections.Generic;
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

        private List<object> _g;
        public List<object> ReportDS
        {
            get
            {
                return _g;
            }
            set
            {
                _g = value;
                OnPropertyChanged();
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

            }
        }

        public void Refresh()
        {
            LoadReport();
        }
    }
}
