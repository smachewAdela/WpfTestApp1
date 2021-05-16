using QBalanceDesktop;
using System;

namespace WpfTestApp1.MVVM.Model
{
    public class BaseReportMaker : IReportMaker
    {
        public virtual bool ExcludeCurrentMonth { get; set; }

        public BaseReportMaker()
        {
            ExcludeCurrentMonth = false;
        }

        public void FillData(ReportContent content, DbAccess db, object data)
        {
            throw new NotImplementedException();
        }
    }
}
