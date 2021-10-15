<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetGroups.aspx.cs" Inherits="WebBalanceTracker.BudgetGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-1">

        <div class="col-1">
        </div>
        <div class="col-7">
            <div class="table-responsive">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>קבוצה</th>
                        <th>עדכון</th>
                        <th>מחיקה</th>
                    </tr>
                    <tbody>
                        <% foreach (System.Data.DataRow BudgetGroup in BudgetGroupItems.Rows)
                            { %>
                        <tr class="text-center  bg-white text-info border-bottom">

                            <td>
                                <% =BudgetGroup[0] %>
                            </td>
                            <td>
                                <button id="btnclk" onclick="showUpdateGroup('<% =BudgetGroup[0] %>','<% =BudgetGroup[1] %>'); return false;" class="h-100 border-0 text-info bg-transparent w-100">
                                    <i class="material-icons">edit</i>
                                </button>
                            </td>
                            <td>
                                <button id="btnDlt" onclick="deleteGroup('<% =BudgetGroup[1] %>','<% =BudgetGroup[0] %>'); return false;" class="h-100 border-0 text-danger bg-transparent w-100">
                                    <i class="material-icons">delete</i>
                                </button>
                            </td>
                        </tr>

                        <% } %>
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
                        <button type="button" class="btn btn-outline-info text-center h4" onclick="showCreateGroup(); return false;">
                            הוספה
                         <i class="material-icons text-center ">add</i>
                        </button>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-1"></div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" dir="rtl">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-dark text-info">
                    <h3 class="modal-title py-2 text-info" id="myModalLabel">Modal title</h3>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <input type="hidden" id="edtId" value="0" />

                        <div class="col-4 text-lg-center h3 h-100">שם : </div>
                        <div class="col-8">
                            <input type="text" class="form-control w-100 h-100" placeholder="" id="edtName">
                        </div>
                    </div>
                    <div class="modal-footer">

                        <button type="button" class="btn btn-info text-lg-center h4 w-50 my-auto" data-dismiss="modal">
                            <i class="material-icons text-lg-center ">clear</i>    ביטול    
                        </button>
                        <button type="button" class="btn btn-outline-info text-lg-center h4 w-50 my-auto" onclick="CreateGroup(); return false;">
                            <i class="material-icons text-lg-center ">save</i>    שמור    
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>

        function showCreateGroup() {
            $('#myModalLabel').text('צור קבוצה');
            $('#edtName').attr('value', '');
            $('#edtId').attr('value', '0');
            $('#myModal').modal('show');
        }

        function showUpdateGroup(groupName, groupId) {

            $('#myModalLabel').text('ערוך קבוצה : ' + groupName);
            $('#edtName').attr('value', groupName);
            $('#edtId').attr('value', groupId);
            $('#myModal').modal('show');
        }

        function CreateGroup() {
            var groupName = $('#edtName').val();
            var groupId = $('#edtId').val();
            var mobj = {
                groupName: groupName,
                groupId: groupId
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "BudgetGroups.aspx/upsertGroup",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    console.log(result);
                    showNotification('פעולה בוצעה בהצלחה !', 'success');
                    document.location.reload();
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()
        }


        function deleteGroup(groupId, groupName) {

            //alert(groupName + ' Deleted !');
            var mobj = {
                groupId: groupId
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "BudgetGroups.aspx/deleteGroup",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    console.log(result);
                    showNotification('פעולה בוצעה בהצלחה !', 'success');
                    document.location.reload();
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()
        }

    </script>
</asp:Content>
