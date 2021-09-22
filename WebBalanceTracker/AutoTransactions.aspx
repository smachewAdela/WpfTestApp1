<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AutoTransactions.aspx.cs" Inherits="WebBalanceTracker.AutoTransactions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-1">
        <div class="col-1">
            <button class="bg-transparent text-warning border-0 w-100" onclick="showInsertAutoTransaction(); return false;">
                <i class="material-icons fa-2x">add</i>
            </button>
        </div>
        <div class="col-10">
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
                            <button type="button" class="btn btn-danger" data-dismiss="modal">ביטול</button>
                            <button type="button" class="btn btn-success" onclick="performUpsertAutoTransaction(); return false;">שמור</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <script>

        function showInsertAutoTransaction() {
            //alert(dir);
            $('#myModalLabel').text(' צור\ערוך תנועה');
            $('#edtName').attr('value', '');
            $('#edtId').attr('value', '0');
            $('#edtAmount').attr('value', '');
            $('#edtActive').attr('value', '0');
            $('#edtDayInMonth').attr('value', '');
            $('#cmbAbstractCategories').val('').change();
            $('#myModal').modal('show')
        }

        function showUpdateAutoTransaction(name, catId, defaultamount, active,dayInMonth,id) {
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
