<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogPage.aspx.cs" Inherits="WebBalanceTracker.LogPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-1">
        <div class="col-1">
            <%--    <button class="bg-transparent text-warning border-0 w-100" onclick="generateBudget(); return false;">
                <i class="material-icons fa-2x">add</i>
            </button>--%>
        </div>
        <div class="col-7">
            <div class="table-responsive">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>תקציב</th>
                        <th>רשומות תקציב</th>
                        <th>רשומות הכנסה</th>
                        <th>תנועות יומן</th>
                        <th>מחיקה</th>
                    </tr>
                    <tbody>
                      <%--  <% foreach (WpfTestApp1.MVVM.Model.I_Message bi in logs)
                            { %>
                        <tr class="text-center text-info  bg-white border-bottom">

                            <td>
                                <% =bi.Title %>
                            </td>
                            <td>
                                <% =bi.BudgetItems %> 
                            </td>
                            <td>
                                <% =bi.Incomes %>
                            </td>

                            <td>
                                <% =bi.CheckPoints %>
                            </td>
                            <td>
                                <button id="btnclk" onclick="deleteBudget('<% =bi.Id %>'); return false;"
                                    class="h-100 border-0 text-info bg-transparent">
                                    <i class="material-icons">delete</i>
                                </button>
                            </td>
                        </tr>

                        <% } %>--%>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="col-3">

            <div class="card my-0">
                <div class="card-header text-info h3 bg-dark my-0 text-center ">
                    פעולות
                </div>
                <div class="card-body row">

                    <div class="col-12">
                        <button type="button" class="btn btn-outline-info text-center h4" onclick="generateBudget(); return false;">
                            הפק תקציב
                         <i class="material-icons text-center ">add</i>
                        </button>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-1"></div>

    </div>



</asp:Content>
