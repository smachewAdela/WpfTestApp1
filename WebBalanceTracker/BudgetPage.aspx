<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetPage.aspx.cs" Inherits="WebBalanceTracker.BudgetPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



    <div class="row">

        <div class="col-1"></div>
        <div class="col-6  mt-0 pt-0">
            <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
                { %>

            <div class="mt-0 groupData" groupdata="<% =budgetGroup.Id %>">
                <div class="text-center text-info py-2 h3 bg-darker">
                    <% =budgetGroup.GroupName %>
                </div>

                <div class="col-12">
                    <table class="text-center  my-2 table" id="tbl" dir="rtl">
                        <thead>
                            <tr class=" text-info bg-dark custom-text">
                                <th>קטגוריה</th>
                                <th>אחוז השלמה</th>
                                <th>תקציב</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>

                            <% foreach (WebBalanceTracker.BudgetData catData in budgetGroup.BudgetGroups)
                                { %>
                            <tr class="text-info h4">
                                <td><% =string.Format("{0:n0}",catData.CategoryName) %></td>
                                <td class="text-info"><% =catData.Ratio %>%</td>
                                <td class="w-25">
                                    <input type="text" class="form-control w-50 h-100 align-bottom text-center text-dark" placeholder="" id="edtTran<% =catData.Id %>" value="<% =catData.BudgetAmount %>"></td>
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

        <div class="col-4">
            <div class="card bg-transparent mt-4">
                <div class="card-header text-info h3 bg-dark my-0 text-center  ">
                    פעולות
                </div>
                <div class="card-body mt-0">

                    <div class="row">

                        <%--                        <div class="col-12 text-center text-info py-2 h3 bg-darker">
                            סינון
                        </div>--%>
                        <div class="col-12 pull-right mx-2 my-2">
                            <%-- <span class="material-icons-outlined pull-left  mx-3"><i class="material-icons text-info">arrow_drop_down</i>
                            </span>
                            <select id="groupSelector" class="h3  border-0 text-white text-info   h-100 py-2 mx-2 my-auto">
                                <option value="">....קבוצה לסינון</option>
                                <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
                                    { %>
                                <option value="<% =budgetGroup.Id %>" class=" text-center text-info w-100 border-0 my-2 mx-auto"><% =budgetGroup.GroupName %></option>
                                <% } %>
                            </select>--%>

                            <div class="dropdown">
                                <button class=" w-100 h-100 btn btn-outline-info dropdown-toggle h4 " type="button" id="groupSelector" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    ...קבוצה לסינון
                                </button>
                                <div class="dropdown-menu w-100" aria-labelledby="dropdownMenuButton">

                                    <% foreach (var reportInfo in BudgetGroups)
                                        { %>
                                    <a class="dropdown-item w-100 text-center h5 btn-info " onclick="filterByGroup('<% =reportInfo.Id %>'); return false;">
                                        <div class="text-center w-100"><% =reportInfo.GroupName %></div>
                                    </a>
                                    <%}%>
                                </div>
                            </div>
                        </div>



                    </div>
                </div>
            </div>
        </div>


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

        //function generateBudget() {
        //    //alert('generateBudget');

        //      var mobj = {
        //    };
        //    var DTO = { 'userdata': JSON.stringify(mobj) };
        //    $.ajax({
        //        type: "POST",
        //        contentType: "application/json; charset=utf-8",
        //        url: "BudgetPage.aspx/generateBudget",
        //        data: JSON.stringify(DTO),
        //        datatype: "json",
        //        success: function (result) {
        //            //do something
        //            //$('#' + ctrlName).val('')
        //            console.log(result);
        //            showNotification('פעולה בוצעה בהצלחה !', 'success')
        //        },
        //        error: function (xmlhttprequest, textstatus, errorthrown) {
        //            //alert(" conection to the server failed ");
        //            alert("error: " + errorthrown);
        //        }
        //    });//end of $.ajax()

        //}
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

        function filterByGroup(groupId) {
            $(".groupData").each(function () {
                var gid = $(this).attr('groupData');
                var visible = gid == groupId;
                if (visible) {
                    $(this).show();
                }
                else {
                    $(this).hide();
                }

            });
        }
    </script>

</asp:Content>
