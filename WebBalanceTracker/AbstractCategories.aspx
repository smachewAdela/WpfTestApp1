<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AbstractCategories.aspx.cs" Inherits="WebBalanceTracker.AbstractCategories" %>

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
                        <button type="button" class="btn btn-outline-info text-center h4" onclick="showInsertCategory(); return false;">
                            הוספה
                         <i class="material-icons text-center ">add</i>
                        </button>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-lg-6  col-sm-12">
            <div class="table-responsive w-100">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>קטגוריה</th>
                        <th>קבוצה</th>
                        <th>תקציב</th>
                        <th>עדכון</th>
                    </tr>
                    <tbody>
                        <% foreach (System.Data.DataRow BudgetGroup in BudgetCategories.Rows)
                            { %>
                        <tr class="text-center  text-info bg-white border-bottom">

                            <td>
                                <% =BudgetGroup[0] %>
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
        <div class="modal fade my-4" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" dir="rtl">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-dark text-info">
                        <h3 class="modal-title py-2 text-info" id="myModalLabel">Modal title</h3>
                    </div>
                    <div class="modal-body">
                        <div class="row text-info">
                            <input type="hidden" id="edtId" value="0" />

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
                            <%--<button type="button" class="btn btn-danger" data-dismiss="modal">ביטול</button>
                            <button type="button" class="btn btn-success" onclick="performUpsertCategory(); return false;">שמור</button>--%>

                            <button type="button" class="btn btn-info text-lg-center h4 w-50 my-auto" data-dismiss="modal">
                                <i class="material-icons text-lg-center ">clear</i>    ביטול    
                            </button>
                            <button type="button" class="btn btn-outline-info text-lg-center h4 w-50 my-auto" onclick="performUpsertCategory(); return false;">
                                <i class="material-icons text-lg-center ">save</i>    שמור    
                            </button>
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
                budget: budget
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AbstractCategories.aspx/upsertCategory",
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
