<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BudgetGroups.aspx.cs" Inherits="WebBalanceTracker.BudgetGroups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-1">

        <div class="col-3">
            <button class="bg-transparent text-warning border-0 w-100" onclick="showCreateGroup(); return false;">
                <i class="material-icons fa-2x">add</i>
            </button>
        </div>
        <div class="col-6">
            <div class="table-responsive">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>קבוצה</th>
                        <th>עדכון</th>
                    </tr>
                    <tbody>
                        <% foreach (System.Data.DataRow BudgetGroup in BudgetGroupItems.Rows)
                            { %>
                        <tr class="text-center  bg-darker text-white border-bottom">

                            <td>
                                <% =BudgetGroup[0] %>
                            </td>
                            <td>
                                <button id="btnclk" onclick="showUpdateGroup('<% =BudgetGroup[0] %>','<% =BudgetGroup[1] %>'); return false;" class="h-100 border-0 text-info bg-transparent w-100">
                                    <i class="material-icons">edit</i>
                                </button>
                            </td>
                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-3"></div>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" dir="rtl">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title py-2 text-warning" id="myModalLabel">Modal title</h3>
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
                        <button type="button" class="btn btn-danger" data-dismiss="modal">ביטול</button>
                        <button type="button" class="btn btn-success" onclick="CreateGroup(); return false;">שמור</button>
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
            $('#edtName').attr('value',groupName);
            $('#edtId').attr('value', groupId);
            $('#myModal').modal('show');
        }

        function CreateGroup() {
            var groupName = $('#edtName').val();
            var groupId = $('#edtId').val();
            var mobj = {
                groupName: groupName,
                groupId :groupId
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

    </script>
</asp:Content>
