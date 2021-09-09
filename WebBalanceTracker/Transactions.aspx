<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="WebBalanceTracker.Transactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-3">
        <%-- <div class="col-4">
            <asp:Button ID="btnDownloadFile" runat="server" CssClass="btn w-100 h-100" Text="הורד קובץ" OnClick="btnDownloadFile_Click" />
        </div>

        <div class="col-4">
        </div>

        <div class="col-4">
            <asp:Button ID="btnUploadFile" runat="server" CssClass="btn w-100 h-100" Text="טען קובץ" OnClick="btnUploadFile_Click" />
        </div>--%>

        <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
            { %>
        <div class="col-12 text-center text-info py-2 h3 bg-darker">
            <% =budgetGroup.GroupName %>
        </div>

        <div class="col-12">
            <table class="text-center table bg-secondary my-2" id="tbl" dir="rtl">
                <thead>
                    <tr class="h4 text-gray bg-dark">
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
                        <td><% =string.Format("{0:n0}",catData.StatusAmount) %></td>
                        <td class="w-25">
                            <input type="text" class="form-control w-50 h-100 align-bottom text-center" placeholder="" id="edtTran<% =catData.Id %>"></td>
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

    <script>

        function addTransaction(catId, ctrlName) {

            var amountToAdd = $('#' + ctrlName).val();
            //alert(catId);
            //alert(amounytToAdd);

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
                    console.log(result);
                    location.reload();
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()
        }
    </script>

</asp:Content>
