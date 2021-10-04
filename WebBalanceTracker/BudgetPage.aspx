<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetPage.aspx.cs" Inherits="WebBalanceTracker.BudgetPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="row px-3">
        
        <div class="col-4 pull-left">
            <span class="material-icons-outlined h-100"><i class="material-icons text-info">arrow_drop_down</i>
            </span>
            <select id="groupSelector" class="w-100 h3 py-1 bg-transparent text-center border-0 text-white">
                <option value="">בחר קבוצה...</option>
                <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
                    { %>
                <option value="<% =budgetGroup.Id %>" class="bg-transparent text-center text-white w-100"><% =budgetGroup.GroupName %></option>
                <% } %>
            </select>
        </div>
        <div class="col-5"></div>
        <div class="col-2 pull-right text-warning h4">
            הפק תקציב מחודש זה
        </div>
        <div class="col-1">
             <button class="bg-transparent text-warning border-0 w-100" onclick="generateBudget(); return false;">
                <i class="material-icons fa-2x">add</i>
            </button>
        </div>


        <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
            { %>

        <div class="col-12 groupData" groupdata="<% =budgetGroup.Id %>">
            <div class="text-center text-info py-2 h3 bg-darker">
                <% =budgetGroup.GroupName %>
            </div>

            <div class="col-12">
                <table class="text-center table bg-secondary my-2" id="tbl" dir="rtl">
                    <thead>
                        <tr class=" text-gray bg-dark custom-text">
                            <th>קטגוריה</th>
                            <th>אחוז השלמה</th>
                            <th>תקציב</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>

                        <% foreach (WebBalanceTracker.BudgetData catData in budgetGroup.BudgetGroups)
                            { %>
                        <tr class="text-white h4">
                            <td><% =string.Format("{0:n0}",catData.CategoryName) %></td>
                            <td class="text-info"><% =catData.Ratio %>%</td>
                            <td class="w-25">
                                <input type="text" class="form-control w-50 h-100 align-bottom text-center text-white" placeholder="" id="edtTran<% =catData.Id %>" value="<% =catData.BudgetAmount %>"></td>
                            <td>
                                <button id="btnclk" onclick="updateBudget('<% =catData.Id %>','edtTran<% =catData.Id %>'); return false;"
                                    class="h-100 border-0 text-info bg-transparent">
                                    <i class="material-icons">add</i>
                                </button>
                            </td>
                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>

        <% } %>
    </div>

    <script>

        function updateBudget(catId, ctrlName) {

            var newBudgetAmount = $('#' + ctrlName).val();
            //alert(catId);
            //alert(newBudgetAmount);

            var mobj = {
                newBudgetAmount: newBudgetAmount,
                budgetItemId: catId
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "BudgetPage.aspx/updateBudget",
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

        function generateBudget() {
            alert('generateBudget');
        }
    </script>

    <script>
        $("#groupSelector").change(function () {
            var selectedGroupId = this.value;
            $(".groupData").each(function () {
                var gid = $(this).attr('groupData');
                var visible = gid == selectedGroupId;
                if (visible) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }

            });
        });
    </script>

</asp:Content>
