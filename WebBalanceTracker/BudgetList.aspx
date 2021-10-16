<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetList.aspx.cs" Inherits="WebBalanceTracker.BudgetList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-1">
        <div class="col-1">
        </div>

        <div class="col-lg-4  col-sm-12">

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
        <div class="col-lg-6  col-sm-12">
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
                        <% foreach (var bi in Budgets)
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

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="col-1"></div>

    </div>


    <script>
        function deleteBudget(budgetId) {

            var mobj = {
                budgetId: budgetId
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "BudgetList.aspx/deleteBudget",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    //alert("SUCCESS = " + result.d);
                    console.log(result);
                    showNotification('פעולה בוצעה בהצלחה !', 'success')
                    window.location.reload();
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()
        }

        function generateBudget() {
            //alert('generateBudget');

            var mobj = {
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "BudgetList.aspx/generateBudget",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    //$('#' + ctrlName).val('')
                    console.log(result);
                    showNotification('פעולה בוצעה בהצלחה !', 'success')
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()

        }
    </script>

</asp:Content>
