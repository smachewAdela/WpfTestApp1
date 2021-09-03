﻿<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebBalanceTracker._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row px-1">
        <div class="col-1">
        </div>
        <div class="col-10">

            <div class="table-responsive">
                <table class="table table-shopping border-0" dir="rtl">
                    <tr class="text-lg-center text-info h3 bg-dark">
                        <th></th>
                        <th>תקציב</th>
                        <th>מצב</th>
                        <th></th>
                    </tr>
                    <tbody>
                        <% foreach (System.Data.DataRow BudgetGroup in BudgetGroups.Rows)
                            { %>
                        <tr class="  text-center  bg-darker <% =BudgetGroup[4]=="0" ? "text-white" : "text-info" %>">

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
                                <% =BudgetGroup[3] %> 
                            </td>
                        </tr>

                        <% } %>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="col-1">
        </div>
    </div>


</asp:Content>
