<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="WebBalanceTracker.Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row px-1">
        <div class="col-1">
            <button class="bg-transparent text-warning border-0 w-100" onclick="showInsertCategory(); return false;">
                <i class="material-icons fa-2x">add</i>
            </button>
        </div>
        <div class="col-10">
            <div class="table-responsive">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>קטגוריה</th>
                        <th>ברירת מחדל</th>
                        <th>קבוצה</th>
                         <th>תקציב</th>
                        <th>עדכון</th>
                    </tr>
                    <tbody>
                        <% foreach (System.Data.DataRow BudgetGroup in BudgetCategories.Rows)
                            { %>
                        <tr class="text-center  bg-darker text-white border-bottom">

                            <td>
                                <% =BudgetGroup[0] %>
                            </td>
                             <td>
                                <% =BudgetGroup[5] %>
                            </td>
                            <td>
                                <% =BudgetGroup[1] %>
                            </td>
                               <td>
                                <% =BudgetGroup[4] %>
                            </td>
                            <td>
                                <button id="btnclk" onclick="showUpdateCategory('<% =BudgetGroup[0] %>','<% =BudgetGroup[3] %>','<% =BudgetGroup[2] %>','<% =BudgetGroup[4] %>'); return false;" class="h-100 border-0 text-info bg-transparent w-100">
                                    <i class="material-icons">edit</i>
                                </button>
                            </td>
                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-1"></div>
        <!-- Modal -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" dir="rtl">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h3 class="modal-title py-2 text-warning" id="myModalLabel">Modal title</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <input type=hidden id="edtId" value="0"/>

                            <div class="col-4 text-lg-center h3 h-100">שם : </div>
                            <div class="col-8">
                                <input type="text" class="form-control w-100 h-100" placeholder="" id="edtName">
                            </div>

                            <div class="col-4 text-lg-center h3">קבוצה : </div>
                            <div class="col-8">
                                <asp:DropDownList ID="cmbGroups" CssClass="form-control w-100  h-100" ClientIDMode="Static" runat="server"></asp:DropDownList>
                            </div>
                             <div class="col-4 text-lg-center h3 h-100">תקציב : </div>
                            <div class="col-8">
                                <input type="text" class="form-control w-100 h-100" placeholder="" id="edtBudget">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-danger" data-dismiss="modal">ביטול</button>
                            <button type="button" class="btn btn-success" onclick="performUpsertCategory(); return false;">שמור</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <script>

        function showInsertCategory() {
            //alert(dir);
            $('#myModalLabel').text('צור קטגוריה');
            $('#edtName').attr('value', '');
            $('#edtId').attr('value', '0');
            $('#edtBudget').attr('value', '0');
            $('#cmbGroups').val('').change();
            $('#myModal').modal('show')
        }

        function showUpdateCategory(catName, catGroupId, catId, budget) {
            //alert(dir);
            $('#myModalLabel').text(catName);
            $('#edtName').attr('value', catName);
            $('#edtId').attr('value', catId);
              $('#edtBudget').attr('value', budget);
            $('#cmbGroups').val(catGroupId).change();
            $('#myModal').modal('show')
        }

        function performUpsertCategory() {

            var editedId = $('#edtId').val();
            var catName = $('#edtName').val();
            var groupId = $('#cmbGroups').val();
            var budget = $('#edtBudget').val();
            //alert(groupId);

            if (catName == '' || catName == undefined) {
                alert('please enter category name');
                return;
            }
             if (groupId == '' || groupId == undefined) {
                alert('please select group');
                return;
            }
            //if (editedId == '0') {
            //    // insert new
            //    alert('insert');
            //}
            //else {
            //    //
            //    alert('edit');
            //}

            var mobj = {
                editedId: editedId,
                catName: catName,
                groupId: groupId,
                budget : budget
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Categories.aspx/upsertCategory",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    //do something
                    //alert("SUCCESS = " + result.d);
                    console.log(result);
                    $('#myModal').modal('hide');
                    showNotification('פעולה בוצעה בהצלחה !', 'success');
                    window.location.reload();
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()
        }
    </script>
</asp:Content>
