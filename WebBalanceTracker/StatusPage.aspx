<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatusPage.aspx.cs" Inherits="WebBalanceTracker.StatusPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <div class="row">
        <% foreach (System.Data.DataRow BudgetGroup in BudgetGroups.Rows)
            { %>

      <%--  <div class="col-xl-3 col-lg-6 col-md-6 col-sm-6">
            <div class="card card-stats">
                <div class="card-header card-header-info card-header-icon">
                    <div class="card-icon">
                        <i class="material-icons">payments</i>
                    </div>
                    <div class="h2"> <% =BudgetGroup[0] %></div>
                    <h3 class="card-title text-info"> <% =BudgetGroup[2] %>/ <% =BudgetGroup[1] %>
                    </h3>
                </div>
                <div class="card-footer ">
                    <div class="stats <% =BudgetGroup[4] == "1" ? "text-danger" : "text-info" %>">
                        <i class="material-icons ">timeline</i>
                         <% =BudgetGroup[3] %>
                    </div>
                </div>
            </div>
        </div>--%>

          <div class="col-md-3">
            <div class="card rtl">
                <div class="card-header card-chart card-header-info">
                    <div class="card-icon bg-transparent">
                        <i class="material-icons">balance</i>
                    </div>
                    <div class="card-title w-100 text-right h3 h-100 pr-2 py-2"> <% =BudgetGroup[0] %></div>
                </div>
                <div class="card-body ">
                    <p class="card-category h2 text-info w-100 text-center text-capitalize"><% =BudgetGroup[2] %>/ <% =BudgetGroup[1] %></p>
                </div>
                <div class="card-footer">
                     <div class="stats <% =BudgetGroup[4] == "1" ? "text-danger" : "text-info" %>">
                        <i class="material-icons ">timeline</i>
                         <% =BudgetGroup[3] %>
                    </div>
                </div>
            </div>
        </div>

        <% } %>
    </div>

</asp:Content>
