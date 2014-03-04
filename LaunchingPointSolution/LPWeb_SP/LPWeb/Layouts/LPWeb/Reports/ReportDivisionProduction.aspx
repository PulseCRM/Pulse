<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Division Production Goals Report" Language="C#" AutoEventWireup="true"
    CodeBehind="ReportDivisionProduction.aspx.cs" Inherits="LPWeb.Reports.ReportDivisionProduction" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css"
        runat="server" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <style>
        .alert
        {
            width: 65px;
        }
        .alert img
        {
            float: left;
        }
        .progressbar
        {
        }
        .progressbar table
        {
            margin: 0;
            padding: 0;
            border-collapse: collapse;
            border: 0;
            border-color: transparent;
            font-size: 8px;
            line-height: 20px;
        }
        .progressbar table td
        {
            margin: 0;
            padding: 0;
            border: 0;
            border-color: transparent;
            font-size: 8px;
            line-height: 20px;
        }
        .progressbar table td
        {
            margin: 0;
            padding: 0;
            border: 0;
            border-color: transparent;
            font-size: 8px;
            line-height: 20px;
        }
        .ProgressContainer, completed, content
        {
            background-image: url(../images/ProgressBarBG.gif);
            background-repeat: no-repeat;
            width: 142px;
            height: 20px;
            padding: 0;
            margin: 0;
            position: relative;
        }
        .completed
        {
            background-image: url(../images/Progress.gif);
            background-repeat: repeat-x;
            background-position: 0 50%;
            position: absolute;
            max-width: 139.5px;
            width: 0;
            left: 1px;
            top: 2px;
        }
        .content
        {
            background-image: none;
            background-color: transparent;
            width: 142px;
            height: 20px;
            text-align: center;
            position: absolute;
            left: 0;
            top: 3px;
        }
        a.loanDetails:link, :visited, :active
        {
            color: #818892;
        }
        a.loanDetails:hover
        {
            color: Blue;
        }
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">    
// <![CDATA[
        $(document).ready(function () {
            // set parent iframe height
            var h = document.body.scrollHeight + 10;
            if (h < 200)
                h = 200;
            $("#ifReportList", window.parent.document).height(h);
        });
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="ReportContainer">
        <div id="divToolBar" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 40px;">
                        <%--<asp:Button ID="btnExport" runat="server" Text="Export" CssClass="Btn-66" OnClick="btnExport_Click" />--%>
                        <ul class="ToolStrip">
                            <li>
                                <asp:LinkButton ID="lbtnExport" runat="server" OnClick="btnExport_Click" Text="Export"></asp:LinkButton></li>
                        </ul>
                    </td>
                    <td style="width: 300px;">
                    </td>
                    <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
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
        <div id="divDivision" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gvDivisionReport" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                OnSorting="gvDivisionReport_Sorting" CellPadding="3" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderText="Division" SortExpression="DivisionName" ItemStyle-Wrap="false"
                        ItemStyle-Width="120">
                        <ItemTemplate>
                            <%# Eval("DivisionName")%>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Progress" SortExpression="Progress" HeaderStyle-Width="145">
                        <ItemTemplate>
                            <div class="ProgressContainer">
                                <div class="completed" style='width: <%# Eval("Progress", "{0:N2}")%>%'>
                                    &nbsp;</div>
                                <div class="content">
                                    <%# Eval("Progress", "{0:N2}")%>%
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Running Total" SortExpression="RunningTotal" ItemStyle-Wrap="false"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            $<%# Eval("RunningTotal", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Low Range" SortExpression="LowRange" ItemStyle-Wrap="false"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            $<%# Eval("LowRange", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Medium Range" SortExpression="MediumRange" ItemStyle-Wrap="false"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            $<%# Eval("MediumRange", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="High Range" SortExpression="HighRange" ItemStyle-Wrap="false"
                        ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            $<%# Eval("HighRange", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    </form>
</body>
</html>
