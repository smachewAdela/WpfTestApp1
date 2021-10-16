<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LogPage.aspx.cs" Inherits="WebBalanceTracker.LogPage" %>

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
                </div>
            </div>

        </div>
        <div class="col-lg-6  col-sm-12">
            <div class="table-responsive">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th>אירוע</th>
                     <%--   <th>תיאור</th>
                        <th>מידע נוסף</th>--%>
                        <th>תאריך</th>
                    </tr>
                    <tbody>
                        <% foreach (WebBalanceTracker.LogInfo bi in Logs)
                            { %>
                        <tr class="text-center text-info  bg-white border-bottom">

                            <td class="ltext">
                                <% =bi.Title %>
                            </td>
                        <%--    <td class="ltext">
                                <% =bi.Message %> 
                            </td>
                            <td class="ltext">
                                <% =bi.ExtraData %>
                            </td>--%>

                            <td>
                                <% =bi.Date %>
                            </td>
                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>


        <div class="col-1"></div>

    </div>

    <script type="text/javascript">// <![CDATA[
        $(function () {
            $(".ltext").each(function (i) {
                len = $(this).text().length;
                alert(len);
                if (len > 30) {
                    $(this).text($(this).text().substr(0, 80) + '...');
                }
            });
        });
// ]]></script>

</asp:Content>
