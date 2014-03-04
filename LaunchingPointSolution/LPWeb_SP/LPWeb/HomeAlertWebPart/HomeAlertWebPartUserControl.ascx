<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeAlertWebPartUserControl.ascx.cs" Inherits="LPWeb.HomeAlertWebPart.HomeAlertWebPartUserControl" %>
<div id="divAlerts" class="Widget" style="width: 350px; margin-bottom: 9px;">
    <div class="Widget_Header_Orange">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td class="wTitle">Alerts</td>
                    <td><a href="Pipeline/AlertList.aspx"><img src="images/Widget-More-Orange.gif" alt="more" /></a></td>
                </tr>
            </table>
        </div>
    </div>
    <div class="Widget_Body" style="padding-top: 3px; padding-left: 5px; padding-right: 5px; padding-bottom: 5px; height: 225px;">
        <select id="ddlDueDateFilter" runat="server" style="width: 200px;">
            <option value="Overdue">Overdue</option>
            <option value="OverToday">Overdue + Today</option>
            <option value="In0">Due today</option>
            <option value="In1">Due tomorrow</option>
            <option value="In7">Due in the next 7 days</option>
            <option value="In14">Due in the next 2 weeks</option>
            <option value="In30">Due in the next month</option>
        </select>
        <table cellpadding="2" cellspacing="2" style="margin-top: 5px;">
            <asp:Repeater ID="rpAlertList" runat="server">
                <ItemTemplate>
                    <tr>
                        <td style="width: 120px;">
                            <a href="javascript:GoToLoanDetails(<%# Eval("FileId")%>)" style="color: #818892; width: 110px;" class="TextEllipsis" title="<%# Eval("FullName")%>"><%# Eval("FullName")%></a>
                        </td>
                        <td>
                            <a onclick="javascript:NewItem2(event, '<%# Eval("HRef") %>'); javascript:return false;" style="color: #b4bccb; width: 210px;" class="TextEllipsis" href="<%# Eval("HRef") %>" target="_self"><%# Eval("Desc") %></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>                     
        </table>
    </div>
</div>


