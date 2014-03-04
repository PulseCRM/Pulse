<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Activity History List" Language="C#" AutoEventWireup="true" Inherits="LPWeb.Prospect.ProspectActivityTab"
    CodeBehind="ProspectActivityTab.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Client Detail - Activities</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        $(document).ready(function () {
            var startDate = $("#" + '<%=tbStartDate.ClientID %>');
            var endDate = $("#" + '<%=tbEndDate.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");
        });
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlActivityType" runat="server" DataValueField="ContactId" DataTextField="ActivityTypeName" Width="300px">
                    </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        Performed By: 
                            <asp:DropDownList ID="ddlPerformedBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPerformedBy_SelectedIndexChanged">
                                <asp:ListItem Value="All" Text="All"></asp:ListItem>
                            </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        Start Date:
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="tbStartDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 15px;">
                        End Date:
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="tbEndDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="Btn-66" OnClick="btnFilter_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                   
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                            FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                            ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                            LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridList" runat="server" DataKeyNames="ActivityId" EmptyDataText="There is no data in database."
                AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None"
                AllowSorting="true" OnSorting="gridList_Sorting">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:BoundField DataField="ActivityTime" SortExpression="ActivityTime" HeaderText="Date Time" />
                    <asp:BoundField DataField="ActivityType" SortExpression="ActivityType" HeaderText="Type" />
                    <asp:BoundField DataField="ActivityFile" SortExpression="ActivityFile" HeaderText="Loan File" />
                    <asp:BoundField DataField="ActivityName" SortExpression="ActivityName" HeaderText="Activity Name" />
                    <asp:BoundField DataField="PerformedBy" SortExpression="PerformedBy" HeaderText="Performed By" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">
                &nbsp;</div>
        </div>
    </div>
    </form>
</body>
</html>
