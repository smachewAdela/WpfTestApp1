<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebBalanceTracker._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-3 py-3">


        <div class="col-6">
            <div class="card card-chart">
                <div class="card-header card-header-success">
                    <div class="ct-chart" id="dailySalesChart"></div>
                </div>
                <div class="card-body">
                    <h4 class="card-title">Daily Sales</h4>
                    <p class="card-category">
                        <span class="text-success"><i class="fa fa-long-arrow-up"></i>55% </span>increase in today sales.
                    </p>
                </div>
                <div class="card-footer">
                    <div class="stats">
                    </div>
                </div>
            </div>
        </div>

        <div class="col-6 rtl">
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
        </div>
    </div>

</asp:Content>
