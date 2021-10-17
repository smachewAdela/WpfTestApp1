<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebBalanceTracker._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-3 py-3">

        <div class="col-md-3">
            <div class="card rtl">
                <div class="card-header card-chart card-header-danger">
                    <div class="card-icon">
                        <i class="material-icons">credit_card</i>
                    </div>
                    <div class="card-title w-100 text-right h3 h-100 pr-2 py-2">הוצאות החודש</div>
                </div>
                <div class="card-body ">
                    <p class="card-category h2 text-danger w-100 text-center text-capitalize"><%=TotalExpenses %></p>
                </div>
                <div class="card-footer">
                    <div class="stats">
                        <%--<i class="material-icons">access_time</i> updated 2 minutes ago--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card rtl">
                <div class="card-header card-chart card-header-success">
                    <div class="card-icon">
                        <i class="material-icons">account_balance</i>
                    </div>
                    <div class="card-title w-100 text-right h3 h-100 pr-2 py-2">הכנסות החודש</div>
                </div>
                <div class="card-body ">
                    <p class="card-category h2 text-success w-100 text-center text-capitalize"><%=Totalincomes %></p>
                </div>
                <div class="card-footer">
                    <div class="stats">
                        <%--<i class="material-icons">access_time</i> updated 2 minutes ago--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card rtl">
                <div class="card-header card-chart card-header-info">
                    <div class="card-icon bg-transparent">
                        <i class="material-icons">balance</i>
                    </div>
                    <div class="card-title w-100 text-right h3 h-100 pr-2 py-2">יתרה לתקציב</div>
                </div>
                <div class="card-body ">
                    <p class="card-category h2 text-info w-100 text-center text-capitalize"><%=LefttoUse %></p>
                </div>
                <div class="card-footer">
                    <div class="stats">
                        <%--<i class="material-icons">access_time</i> updated 2 minutes ago--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-3">
            <div class="card rtl">
                <div class="card-header card-chart card-header-primary">
                    <div class="card-icon bg-transparent">
                        <i class="material-icons">balance</i>
                    </div>
                    <div class="card-title w-100 text-right h3 h-100 pr-2 py-2">יתרה מהכנסות</div>
                </div>
                <div class="card-body ">
                    <p class="card-category h2 text-primary w-100 text-center text-capitalize"><%=LeftFromIncome %></p>
                </div>
                <div class="card-footer">
                    <div class="stats">
                        <%--<i class="material-icons">access_time</i> updated 2 minutes ago--%>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-sm-6 col-lg-12">
            <div class="card card-chart">
                <div class="card-header card-header-info">
                    <div class="ct-chart xchart" id="crt1" xchartsource="6MonthsDiff"></div>
                </div>
                <div class="card-body">
                    <div class="card-title w-100 text-right h3 text-secondary pr-3"></div>
                    <p class="card-category text-info">מאזן בסוף חודש
                    </p>
                </div>
                <div class="card-footer">
                    <div class="stats">
                    </div>
                </div>
            </div>
        </div>

        <%--        <div class="col-sm-6 col-lg-12 rtl">
            <div class="card">
                <div class="card-header card-header-icon card-header-rose">
                    <div class="card-icon">
                        <i class="material-icons">account_balance</i>
                    </div>
                    <div class="h2 w-100 text-center text-info bg-secondary">
                        <%=BudgetDataName %>
                    </div>
                </div>
                <div class="card-body">
                    <ul class="list-group list-group-flush">
                        <li class="list-group-item w-100 text-center font-weight-bold h3 py-2 card-title">סה"כ הוצאות לחודש נוכחי - 
                              <%=TotalExpenses %>
                        </li>
                        <li class="list-group-item w-100 text-center font-weight-bold h3 py-2 card-title">סה"כ הכנסות לחודש נוכחי - 
                               <%=Totalincomes %>
                        </li>
                        <li class="list-group-item w-100 text-center font-weight-bold h3 py-2 card-title">יתרה לניצול החודש - 
                               <%=LefttoUse %>
                        </li>
                    </ul>
                </div>
                <div class="card-footer">
                    <div class="progress-container progress-info w-100">
                        <span class="progress-badge w-100 text-right"><%=Ratio %>%</span>
                        <div class="progress">
                            <div class="progress-bar progress-bar-info text-info" role="progressbar" aria-valuenow="<%=Ratio %>" aria-valuemin="0" aria-valuemax="100" style="width: <%=Ratio %>%;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>



    <%--    <div class="xchart"  xChartSource="smach" id="crt1"> </div>
    <div class="xchart"  xChartSource="noa" id="crt2"> </div>--%>
</asp:Content>
