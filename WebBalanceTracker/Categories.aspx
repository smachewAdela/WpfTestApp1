<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="WebBalanceTracker.Categories" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="table-responsive">
        <table class="table table-shopping border-0" dir="rtl">
            <tr class="text-lg-center text-info h3 bg-dark">
                <th>קטגוריה</th>
                <th>קבוצה</th>
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
                        <% =BudgetGroup[1] %>
                    </td>
                    <td>
                        <button id="btnclk" onclick="updateCategory('<% =BudgetGroup[0] %>','<% =BudgetGroup[1] %>','<% =BudgetGroup[2] %>'); return false;" class="h-100 border-0 text-info bg-transparent w-100">
                            <i class="material-icons">edit</i>
                        </button>
                    </td>
                </tr>

                <% } %>
            </tbody>
        </table>
    </div>

    <!-- Modal -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">

                    <h3 class="modal-title py-2 text-warning" id="myModalLabel">Modal title</h3>
                </div>
                <div class="modal-body">
                    <div class="form-check">
                        <input class="form-check-input" type="checkbox" value="" id="flexCheckDefault">
                        <label class="form-check-label" for="flexCheckDefault">
                            Default checkbox
                        </label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">ביטול</button>
                    <button type="button" class="btn btn-success">שמור</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function updateCategory(catName, catGroupId, catId) {
            //alert(dir);
            $('#myModalLabel').text(catName);
            $('#myModal').modal('show')
        }
    </script>
</asp:Content>
