<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="WebBalanceTracker.Transactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row py-2">


        <div class="col-1"></div>
        
        <div class="col-lg-4  col-sm-12">
            <div class="card my-0">
                <div class="card-header text-info h3 bg-dark my-0 text-center ">
                    פעולות
                </div>
                <div class="card-body mt-0">

                    <div class="row">

                        <div class="col-12 text-center text-info py-2 h3 bg-darker">
                            סינון
                        </div>
                        <div class="col-12 pull-right mx-2 my-2">


                            <div class="dropdown">
                                <button class=" w-100 h-100 btn btn-outline-info dropdown-toggle h4 " type="button" id="groupSelector" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                    ...קבוצה לסינון
                                </button>
                                <div class="dropdown-menu w-100" aria-labelledby="dropdownMenuButton">

                                    <% foreach (var reportInfo in BudgetGroups)
                                        { %>
                                    <a class="dropdown-item w-100 text-center h5 btn-info " onclick="filterByGroup('<% =reportInfo.Id %>'); return false;">
                                        <div class="text-center w-100 h4"><% =reportInfo.GroupName %></div>
                                    </a>
                                    <%}%>
                                </div>
                            </div>

                        </div>



                        <div class="col-12 text-center text-info py-2 h3 bg-darker">
                            עדכון תנועה אחרונה
                        </div>

                        <div class="col-12">
                            <table class="text-center table bg-transparent" id="bv" dir="rtl">
                                <thead>
                                    <tr class="text-info bg-dark font-weight-bold  custom-text">
                                        <th>שם</th>
                                        <th>תיאור תנועה אחרונה</th>
                                        <th>עדכן</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <% foreach (WebBalanceTracker.CheckPointData cPoint in CheckPoints)
                                        { %>


                                    <tr class="text-info h4">
                                        <td class="w-25"><% =string.Format("{0:n0}",cPoint.Name) %></td>
                                        <td class="w-50">
                                            <input type="text" class="form-control w-100 h-100 align-bottom text-center text-dark" value="<% =cPoint.Description %>" id="edtCheckpoint<% =cPoint.Id %>">
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
                </div>
            </div>
        </div>
        <div class="col-lg-6  col-sm-12 row">

            <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
                { %>
            <div class="col-12 groupData" groupdata="<% =budgetGroup.Id %>">

                <div class="text-center text-info py-2 h3 bg-darker w-100 my-0">
                    <% =budgetGroup.GroupName %>
                </div>

                <div class="col-12">
                    <table class="text-center table my-2 w-100 bg-white" id="tbl" dir="rtl">
                        <thead>
                            <tr class=" text-info custom-text bg-dark">
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
                            <tr class="text-dark h4">
                                <td><% =string.Format("{0:n0}",catData.CategoryName) %></td>
                                <td><% =string.Format("{0:n0}",catData.BudgetAmount) %></td>
                                <td class="text-info"><% =catData.Ratio %>%</td>
                                <td id="dspStat<% =catData.Id %>">
                                    <% =string.Format("{0:n0}",catData.StatusAmount) %>
                                </td>
                                <td class="w-25">
                                    <input type="text" class="form-control w-50 h-100 align-bottom text-center text-info" placeholder="" id="edtTran<% =catData.Id %>"></td>
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

            </div>
            <% } %>
        </div>
        <div class="col-1"></div>
    </div>

    <script>



        function addTransaction(catId, ctrlName) {

            var amountToAdd = $('#' + ctrlName).val();
            //alert(catId);
            //alert(amountToAdd);
            var statusFieldSelector = '#dspStat' + catId;

            var currentStatus = $(statusFieldSelector).text();
            var newStatusAmount = Number(currentStatus) + Number(amountToAdd);
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
