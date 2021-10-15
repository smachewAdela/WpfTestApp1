<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StatusPage.aspx.cs" Inherits="WebBalanceTracker.StatusPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <div class="row">

        <div class="col-12 page-content page-container" id="page-content">
            <div class="padding">
                <div class="row container d-flex justify-content-center">
                    <div class="col-lg-8 grid-margin stretch-card">
                        <div class="card">
                            <div class="card-header card-chart card-header-info">
                                <div class="card-icon bg-transparent">
                                    <i class="material-icons">balance</i>
                                </div>
                                
                            </div>
                            <div class="card-body">
                                <div class="table-responsive">
                                    <table class="table rtl">
                                        <thead>
                                            <tr class="bg-dark text-center text-info h2 ">
                                                <th>השלמה</th>
                                                <th>סטטוס</th>
                                                <th>תקציב</th>
                                                <th>שם</th>
                                            </tr>
                                        </thead>
                                        <tbody>

                                            <% foreach (System.Data.DataRow BudgetGroup in BudgetGroups.Rows)
                                                { %>

                                            <%-- <div class="col-md-3">
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
        </div>--%>


                                            <tr class="text-center h3 <% =BudgetGroup[4] == "1" ? "text-danger" : "text-info" %>  <% =BudgetGroup[5] == "1" ? "bg-darker" : "" %>">
                                                <td><% =BudgetGroup[3] %></td>
                                                <td><% =BudgetGroup[2] %></td>
                                                <td><% =BudgetGroup[1] %></td>
                                                <td>
                                                    <% =BudgetGroup[0] %>
                                                </td>
                                            </tr>

                                            <% } %>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
