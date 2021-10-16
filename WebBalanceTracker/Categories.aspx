<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="WebBalanceTracker.Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row px-1">
        <div class="col-1">
        </div>

        <div class="col-lg-4  col-sm-12" dir="rtl">

            <div class="card my-0">
                <div class="card-header text-info h3 bg-dark my-0 text-center w-100 ">
                    פעולות
                </div>
                <div class="card-body row">

                    <div class="col-4">
                        <button type="button" class="btn btn-outline-info text-lg-center h4" onclick="showInsertCategory(); return false;">
                            הוספה
                         <i class="material-icons text-lg-center ">add</i>
                        </button>
                    </div>

                    <div class="col-8">

                        <%--  <div class="pull-right mx-2 my-2">
                            <span class="material-icons-outlined pull-left  mx-3"><i class="material-icons text-info">arrow_drop_down</i>
                            </span>
                            <select id="groupSelector" class="h3  border-0 text-white text-info   h-100 py-2 mx-2 my-auto">
                                <option value="">....קבוצה לסינון</option>
                                <% foreach (WebBalanceTracker.GroupData budgetGroup in BudgetGroups)
                                    { %>
                                <option value="<% =budgetGroup.Id %>" class=" text-center text-info w-100 border-0 my-2 mx-auto"><% =budgetGroup.GroupName %></option>
                                <% } %>
                            </select>
                        </div>--%>

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
        <div class="col-lg-6 col-sm-12">
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
                        <tr class="groupData text-center text-dark font-weight-bold border-bottom " groupdata="<% =BudgetGroup[3] %>">

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
           <div class="col-1">
          
        </div>
        <!-- Modal -->
        <div class="modal fade my-4" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" dir="rtl">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-dark text-info">
                        <h3 class="modal-title py-2" id="myModalLabel">Modal title</h3>
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
                                <asp:DropDownList ID="cmbGroups" CssClass="form-control w-100 text-info  h-100" ClientIDMode="Static" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-4 text-lg-center h3 h-100">תקציב : </div>
                            <div class="col-8">
                                <input type="text" class="form-control w-100 h-100" placeholder="" id="edtBudget">
                            </div>
                        </div>
                        <div class="modal-footer">
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
        $("#groupSelector").change(function () {
            var selectedGroupId = this.value;
            var numItems = $('.groupData').length;
            //alert(numItems);

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
