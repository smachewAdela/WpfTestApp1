<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="WebBalanceTracker.Transactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-3">

        <div class="col-5">

            <div class="col-12 row">

                <div class="col-12 text-center text-info py-2 h3 bg-darker">
                    עדכון תנועה אחרונה
                </div>

                <table class="text-center table bg-dark" id="bv" dir="rtl">
                    <thead>
                        <tr class="text-warning font-weight-bold  custom-text">
                            <th>שם</th>
                            <th>תיאור תנועה אחרונה</th>
                            <th>עדכן</th>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach (WebBalanceTracker.CheckPointData cPoint in CheckPoints)
                            { %>


                        <tr class="text-white h4">
                            <td class="w-25"><% =string.Format("{0:n0}",cPoint.Name) %></td>
                            <td class="w-50">
                                <input type="text" class="form-control w-100 h-100 align-bottom text-center text-white" value="<% =cPoint.Description %>" id="edtCheckpoint<% =cPoint.Id %>">
                            </td>
                            <td class="w-25">
                                <button id="btncpoint" onclick="saveCheckpoint('<% =cPoint.Id %>','edtCheckpoint<% =cPoint.Id %>'); return false;"
                                    class="h-100 border-0 text-info bg-transparent">
                                    <i class="material-icons">update</i>
                                </button>
                            </td>

                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="col-7 row">

            <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
                { %>
            <div class="col-12 text-center text-info py-2 h3 bg-darker">
                <% =budgetGroup.GroupName %>
            </div>

            <div class="col-12">
                <table class="text-center table bg-dark my-2" id="tbl" dir="rtl">
                    <thead>
                        <tr class=" text-warning custom-text">
                            <th>קטגוריה</th>
                            <th>תקציב</th>
                            <th>אחוז השלמה</th>
                            <th>מצב</th>
                            <th></th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>

                        <% foreach (WebBalanceTracker.BudgetData catData in budgetGroup.BudgetGroups)
                            { %>
                        <tr class="text-white h4">
                            <td><% =string.Format("{0:n0}",catData.CategoryName) %></td>
                            <td><% =string.Format("{0:n0}",catData.BudgetAmount) %></td>
                            <td class="text-info"><% =catData.Ratio %>%</td>
                            <td id="dspStat<% =catData.Id %>">
                                <% =string.Format("{0:n0}",catData.StatusAmount) %>
                            </td>
                            <td class="w-25">
                                <input type="text" class="form-control w-50 h-100 align-bottom text-center text-white" placeholder="" id="edtTran<% =catData.Id %>"></td>
                            <td>
                                <button id="btnclk" onclick="addTransaction('<% =catData.Id %>','edtTran<% =catData.Id %>'); return false;"
                                    class="h-100 border-0 text-info bg-transparent">
                                    <i class="material-icons">add</i>
                                </button>
                            </td>
                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
            <% } %>
        </div>
    </div>

    <script>



        function addTransaction(catId, ctrlName) {

            var amountToAdd = $('#' + ctrlName).val();
            //alert(catId);
            //alert(amountToAdd);
            var statusFieldSelector = '#dspStat' + catId;

            var currentStatus = $(statusFieldSelector).text();
            var newStatusAmount = Number(currentStatus) + Number(amountToAdd);
            alert(newStatusAmount);
            var mobj = {
                amountToAdd: amountToAdd,
                budgetItemId: catId
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Transactions.aspx/addTransaction",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    $('#' + ctrlName).val('')
                    $(statusFieldSelector).text(newStatusAmount);
                    console.log(result);
                    showNotification('פעולה בוצעה בהצלחה !', 'success')
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()
        }

        function saveCheckpoint(checkPointId, ctrlName) {
            var checkPointDescription = $('#' + ctrlName).val();
            //alert(newcp);

            var mobj = {
                checkPointDescription: checkPointDescription,
                checkPointId: checkPointId
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Transactions.aspx/saveCheckpoint",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something

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
