<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebBalanceTracker._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="table-responsive">
  <table class="table table-shopping" dir="rtl">
      <thead>
          <tr class="h-1 bg-dark text-warning text-center">
              <th >קבוצה</th>
              <th >תקציב</th>
              <th >מצב</th>
              <th>השלמה</th>
          </tr>
      </thead>
      <tbody>
               <% foreach (var BudgetGroup in BudgetGroups)
                    { %>
          <tr>
              
              <td>
                  <a href="#jacket"><% =BudgetGroup.Name %></a>
              </td>
              <td>
                  <% =BudgetGroup.BudgetStr %>
              </td>
              <td>
                  <% =BudgetGroup.StatusStr %>
              </td>
              <td class="td-number">
                  <small>&#x20AC;</small><% =BudgetGroup.Ratio %> %
              </td>
          </tr>

             <% } %>
      </tbody>
  </table>
</div>
    
</asp:Content>
