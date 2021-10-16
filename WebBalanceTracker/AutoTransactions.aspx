<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AutoTransactions.aspx.cs" Inherits="WebBalanceTracker.AutoTransactions" %>

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
                        <button type="button" class="btn btn-outline-info text-center h4" onclick="showInsertAutoTransaction(); return false;">
                            הוספה
                         <i class="material-icons text-center ">add</i>
                        </button>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-lg-6  col-sm-12">
            <div class="table-responsive">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>שם</th>
                        <th>קטגוריה</th>
                        <th>סכום</th>
                        <th>פעיל</th>
                        <th>יום בחודש</th>
                        <th>תנועה אחרונה</th>
                        <th>עריכה</th>
                    </tr>
                    <tbody>
                        <% foreach (System.Data.DataRow BudgetGroup in AutoTransactionItems.Rows)
                            { %>
                        <tr class="text-center  bg-darker text-white border-bottom">

                            <td>
                                <% =BudgetGroup[0] %>
                            </td>
                            <td>
                                <% =BudgetGroup[1] %>
                            </td>
                            <td>
                                <% =BudgetGroup[2] %>
                            </td>
                            <td>
                                <% =BudgetGroup[3]%>
                            </td>
                            <td>
                                <% =BudgetGroup[4] %>
                            </td>
                            <td>
                                <% =BudgetGroup[5] %>
                            </td>
                            <td>
                                <button id="btnclk" onclick="showUpdateAutoTransaction('<% =BudgetGroup[0] %>','<% =BudgetGroup[7] %>','<% =BudgetGroup[2] %>','<% =BudgetGroup[3] %>','<% =BudgetGroup[4] %>','<% =BudgetGroup[6] %>'); return false;" class="h-100 border-0 text-info bg-transparent w-100">
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
                            <div class="col-4 text-lg-center h3">קטגוריה : </div>
                            <div class="col-8">
                                <asp:DropDownList ID="cmbAbstractCategories" CssClass="form-control w-100  h-100" ClientIDMode="Static" runat="server"></asp:DropDownList>
                            </div>
                            <div class="col-4 text-lg-center h3 h-100">סכום : </div>
                            <div class="col-8">
                                <input type="text" class="form-control w-100 h-100" placeholder="" id="edtAmount">
                            </div>
                            <div class="col-4 text-lg-center h3 h-100">פעיל : </div>
                            <div class="col-2 pt-3">
                                <input class="form-control w-100 h-50 pull-right" type="checkbox" value="1" id="edtActive">
                            </div>
                            <div class="col-6"></div>
                            <div class="col-4 text-lg-center h3 h-100">יום בחודש : </div>
                            <div class="col-8">
                                <input type="text" class="form-control w-100 h-100" placeholder="" id="edtDayInMonth">
                            </div>
                        </div>
                        <div class="modal-footer">
                             <button type="button" class="btn btn-info text-lg-center h4 w-50 my-auto" data-dismiss="modal">
                                <i class="material-icons text-lg-center ">clear</i>    ביטול    
                            </button>
                            <button type="button" class="btn btn-outline-info text-lg-center h4 w-50 my-auto" onclick="performUpsertAutoTransaction(); return false;">
                                <i class="material-icons text-lg-center ">save</i>    שמור    
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <script>

        function showInsertAutoTransaction() {
            //alert(dir);
            $('#myModalLabel').text(' צור תנועה');
            $('#edtName').attr('value', '');
            $('#edtId').attr('value', '0');
            $('#edtAmount').attr('value', '');
            $('#edtActive').attr('value', '0');
            $('#edtDayInMonth').attr('value', '');
            $('#cmbAbstractCategories').val('').change();
            $('#myModal').modal('show')
        }

        function showUpdateAutoTransaction(name, catId, defaultamount, active, dayInMonth, id) {
            //alert(active);
            $('#myModalLabel').text(name);
            $('#edtName').attr('value', name);
            $('#edtId').attr('value', id);
            $('#edtActive').attr('value', active);
            $('#edtDayInMonth').attr('value', dayInMonth);
            $('#edtAmount').attr('value', defaultamount);
            $('#cmbAbstractCategories').val(catId).change();
            $('#myModal').modal('show')
        }

        function performUpsertAutoTransaction() {

            var editedId = $('#edtId').val();
            var name = $('#edtName').val();
            var abstractCategoryId = $('#cmbAbstractCategories').val();
            var defaultAmount = $('#edtAmount').val();
            var dayOfTheMonth = $('#edtDayInMonth').val();
            var active = false;// $('#edtActive').val();
            if ($('#edtActive').is(":checked"))
                active = true;

            //alert(active);

            if (name == '' || name == undefined) {
                alert('please enter transaction name');
                return;
            }
            if (abstractCategoryId == '' || abstractCategoryId == undefined) {
                alert('please select category');
                return;
            }
            if (defaultAmount == '' || defaultAmount == undefined) {
                alert('please select amount');
                return;
            }
            if (dayOfTheMonth == '' || dayOfTheMonth == undefined) {
                alert('please select day in month');
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
                name: name,
                abstractCategoryId: abstractCategoryId,
                defaultAmount: defaultAmount,
                dayOfTheMonth: dayOfTheMonth,
                active: active
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "AutoTransactions.aspx/upsertTransaction",
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
