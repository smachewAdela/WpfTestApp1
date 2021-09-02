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
                <tr class="text-center  bg-darker text-white">

                    <td>
                        <% =BudgetGroup[0] %>
                    </td>
                    <td>
                        <% =BudgetGroup[1] %>
                    </td>
                    <td>

                        <button id="btnclk" onclick="updateCategory('<% =BudgetGroup[2] %>'); return false;" class="h-100 border-0 text-info bg-transparent w-100">
                            <i class="fa fa-edit"></i>
                        </button>
                    </td>
                </tr>

                <% } %>
            </tbody>
        </table>
    </div>


    <script>
        function updateCategory(dir) {
            alert(dir);
        }
    </script>
</asp:Content>
