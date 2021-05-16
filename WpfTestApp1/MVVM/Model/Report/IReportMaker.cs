using QBalanceDesktop;

namespace WpfTestApp1.MVVM.Model
{
    public interface IReportMaker
    {
        void FillData(ReportContent content, DbAccess db, ReportSourceData data);
    }
}
