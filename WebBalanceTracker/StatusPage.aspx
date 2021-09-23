<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatusPage.aspx.cs" Inherits="WebBalanceTracker.StatusPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

   
    <div class="row">
        <% foreach (System.Data.DataRow BudgetGroup in BudgetGroups.Rows)
            { %>
        <div class="col-xl-3 col-lg-6 col-md-6 col-sm-6">
            <div class="card card-stats bg-darker">
                <div class="card-header card-header-info card-header-icon">
                    <div class="card-icon">
                        <i class="material-icons">payments</i>
                    </div>
                    <p class="card-category"> <% =BudgetGroup[0] %></p>
                    <h3 class="card-title text-info"> <% =BudgetGroup[2] %>/ <% =BudgetGroup[1] %>
                    </h3>
                </div>
                <div class="card-footer">
                    <div class="stats">
                        <i class="material-icons text-warning">timeline</i>
                         <% =BudgetGroup[3] %>
                    </div>
                </div>
            </div>
        </div>


        <% } %>
    </div>

</asp:Content>
