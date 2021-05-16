using QBalanceDesktop;
using System.Data;

namespace WpfTestApp1.MVVM.Model
{
    public class ReportContent
    {
        public DataTable Table { get; set; }
        public string Title { get; set; }
        public ReportTypeEnum ReportType { get; set; }

        public ReportContent(ReportTypeEnum typeEnum)
        {
            Title = typeEnum.GetEnumDescription();
            Table = new DataTable();
            ReportType = typeEnum;
        }
    }
}
