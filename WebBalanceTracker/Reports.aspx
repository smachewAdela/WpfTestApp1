<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="WebBalanceTracker.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-4 bg-white">

        <div class="col-4">
            <div class="dropdown ">
                <button class="bg-info w-100 h-100 btn dropdown-toggle" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    בחר דו"ח להפקה
                </button>
                <div class="dropdown-menu w-100" aria-labelledby="dropdownMenuButton">

                    <% foreach (var reportInfo in ReeportNames)
                        { %>
                    <a class="dropdown-item w-100 text-center" onclick="generateReport('<% =reportInfo.Key %>'); return false;"><% =reportInfo.Value %></a>
                    <%}%>
                </div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-1"></div>
        <div class="col-10 py-3 px-2">
            <table id="excelDataTable" class="table table-shopping border-0 w-100 bg-secondary text-white text-center" dir="rtl">
            </table>
        </div>
        <div class="col-1"></div>
    </div>

    <script>

        function generateReport(reportType) {

            var mobj = {
                reportType: reportType
            };
            var DTO = { 'userdata': JSON.stringify(mobj) };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "Reports.aspx/generateReport",
                data: JSON.stringify(DTO),
                datatype: "json",
                success: function (result) {
                    console.log(result.d);
                    buildHtmlTable('#excelDataTable', JSON.parse(result.d));
                },
                error: function (xmlhttprequest, textstatus, errorthrown) {
                    //alert(" conection to the server failed ");
                    alert("error: " + errorthrown);
                }
            });//end of $.ajax()
        }


        // Builds the HTML Table out of myList.
        function buildHtmlTable(selector, myList) {
            $(selector).empty();
            var columns = addAllColumnHeaders(myList, selector);
            //alert(columns.length);

            for (var i = 0; i < myList.length; i++) {
                var row$ = $('<tr class="text-lg-center text-white h3 "/>');
                for (var colIndex = 0; colIndex < columns.length; colIndex++) {
                    var cellValue = myList[i][columns[colIndex]];
                    if (cellValue == null) cellValue = "";
                    row$.append($('<td/>').html(cellValue));
                }
                $(selector).append(row$);
            }
        }

        // Adds a header row to the table and returns the set of columns.
        // Need to do union of keys from all records as some records may not contain
        // all records.
        function addAllColumnHeaders(myList, selector) {
            var columnSet = [];
            var headerTr$ = $('<tr  class="text-lg-center text-info h3 bg-dark"/>');

            for (var i = 0; i < myList.length; i++)
            {
                var rowHash = myList[i];
                for (var key in rowHash)
                {
                    if ($.inArray(key, columnSet) == -1)
                    {
                        columnSet.push(key);
                        headerTr$.append($('<th/>').html(key));
                    }
                }
            }
            $(selector).append(headerTr$);

            return columnSet;
        }
    </script>
</asp:Content>
