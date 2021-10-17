<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Incomes.aspx.cs" Inherits="WebBalanceTracker.Incomes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">

        <div class="col-1">
        
        </div>
         <div class="col-lg-3  col-sm-12">

             <div class="card my-0">
                <div class="card-header text-info h3 bg-dark my-0 text-center ">
                    פעולות
                </div>
                <div class="card-body row">

                    <div class="col-12">
                        <button type="button" class="btn btn-outline-info text-center h4" onclick="showIncomeModal(); return false;">
                            הוספה
                         <i class="material-icons text-center ">add</i>
                        </button>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-lg-6  col-sm-12">
            <div class="table-responsive">
                <table class="table table-shopping border-0 bg-white" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>הכנסה</th>
                        <th>סכום</th>
                        <th></th>
                        <th></th>
                    </tr>
                    <tbody>
                        <% foreach (System.Data.DataRow BudgetGroup in BudgetIncomes.Rows)
                            { %>
                        <tr class="text-center  text-info  <% =BudgetGroup[4]!="0" ? "bg-darker" : "" %> border-bottom">

                            <td>
                                <% =BudgetGroup[2] %>
                            </td>
                            <td id="dspStat<% =BudgetGroup[0] %>">
                                <% =BudgetGroup[3] %>
                            </td>
                            <td class="w-25">
                                <% if (BudgetGroup[4] == "0")
                                    { %>
                                <input type="text" class="form-control w-50 h-100 align-bottom text-center" placeholder="" id="edtIncome<% =BudgetGroup[0] %>">
                                <%}%>

                             
                            </td>

                            <td>
                                <% if (BudgetGroup[4] == "0")
                                    { %>
                                <button id="btnclk" onclick="updateIncome('<% =BudgetGroup[0] %>','<% =BudgetGroup[1] %>','<% =BudgetGroup[2] %>','<% =BudgetGroup[3] %>'); return false;"
                                    class="h-100 border-0 text-info bg-transparent">
                                    <i class="material-icons">add</i>
                                </button>
                                <%} %>
                            </td>
                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>
       
        <div class="col-2"></div>
        <!-- Modal -->
        <div class="modal fade my-4" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" dir="rtl">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-dark text-info">
                        <h3 class="modal-title py-2" id="myModalLabel">Modal title</h3>
                    </div>
                    <div class="modal-body">

                        <div class="row">

                            <div class="col-4 text-lg-center h3 h-100 text-info">שם : </div>
                            <div class="col-8">
                                <input type="text" class="form-control w-100 h-100" placeholder="" id="edtName">
                            </div>
                        </div>
                        <div class="modal-footer">
                              <button type="button" class="btn btn-info text-lg-center h4 w-50 my-auto" data-dismiss="modal">
                                <i class="material-icons text-lg-center ">clear</i>    ביטול    
                            </button>
                            <button type="button" class="btn btn-outline-info text-lg-center h4 w-50 my-auto" onclick="addNewIncome(); return false;">
                                <i class="material-icons text-lg-center ">save</i>    שמור    
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>

        function updateIncome(id, budgetId, name, amount) {

            var ctrlName = 'edtIncome' + id;
            var amounytToAdd = $('#' + ctrlName).val().replace(',', '');;
            var statusFieldSelector = '#dspStat' + id;
            var currentStatus = $(statusFieldSelector).text();
            var newStatusAmount = Number(currentStatus) + Number(amounytToAdd);


            var totalStatusSelector = '#dspStat0';
            var currentTotalStatus = $(totalStatusSelector).text().replace(',', '');
            var newTotalStatusAmount = Number(currentTotalStatus) + Number(amounytToAdd);

            

            var mobj = {
                amountToAdd: amounytToAdd,
                incomeName: name,
                incomeId: id
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Incomes.aspx/appendIncome",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    $('#' + ctrlName).val('')
                    console.log(result);
                    $(statusFieldSelector).text(newStatusAmount.toLocaleString());

                    $(totalStatusSelector).text(newTotalStatusAmount.toLocaleString());
                    showNotification('פעולה בוצעה בהצלחה !', 'success')
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()

        }

        function showIncomeModal() {

            $('#myModalLabel').text('צור הכנסה');
            $('#edtName').val('').change();
            $('#myModal').modal('show')
        }

        function addNewIncome() {

            var incomeName = $('#edtName').val();
            var mobj = {
                incomeName: incomeName
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Incomes.aspx/addIncome",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    //alert("SUCCESS = " + result.d);
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
