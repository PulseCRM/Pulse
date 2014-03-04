﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportPointPipeline.aspx.cs" Inherits="LPWeb.Reports.ReportPointPipeline" %>

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


            var w = document.body.scrollWidth + 10;
            if (w < 200)
                w = 200;
            $("#ifReportList", window.parent.document).width(w);
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
            <asp:GridView ID="gvPointPipelineReport" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                OnSorting="gvPipelineView_Sorting" CellPadding="3" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                     <asp:TemplateField HeaderText="Branch" SortExpression="Branch" ItemStyle-Wrap="false"
                        ItemStyle-Width="60"   HeaderStyle-Width="20" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("Branch")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Borrower Last Name" SortExpression="LastName" ItemStyle-Wrap="false"
                        ItemStyle-Width="80"  HeaderStyle-Width="30" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LastName")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Borrower First Name" SortExpression="FirstName" ItemStyle-Wrap="false"
                        ItemStyle-Width="80" HeaderStyle-Width="30" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("FirstName")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Lender" SortExpression="Lender" ItemStyle-Wrap="false"
                        ItemStyle-Width="40" HeaderStyle-Width="25" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("Lender")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Loan Originator" SortExpression="LoanOriginator" ItemStyle-Wrap="false"
                         ItemStyle-Width="60" HeaderStyle-Width="40" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LoanOriginator")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Loan Amount" SortExpression="LoanAmount" ItemStyle-Wrap="false"
                        ItemStyle-Width="80"  HeaderStyle-Width="30" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LoanAmount")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Note Rate" SortExpression="NoteRate" ItemStyle-Wrap="false"
                        ItemStyle-Width="60" HeaderStyle-Width="40" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("NoteRate")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-Wrap="false"
                        ItemStyle-Width="150" HeaderStyle-Width="50" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("Status")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Status Date" SortExpression="StatusDate" ItemStyle-Wrap="false"
                        ItemStyle-Width="80" ItemStyle-Height="17" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("StatusDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Loan Processor" SortExpression="LoanProcessor" ItemStyle-Wrap="false"
                        ItemStyle-Width="80" ItemStyle-Height="50" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LoanProcessor")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Loan Program" SortExpression="LoanProgram" ItemStyle-Wrap="false"
                        ItemStyle-Width="60" ItemStyle-Height="20" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LoanProgram")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Loan Origination Fee" SortExpression="LoanOriginationFee" ItemStyle-Wrap="false"
                        ItemStyle-Width="100"  HeaderStyle-Width="30"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LoanOriginationFee")%>
                        </ItemTemplate>
                    </asp:TemplateField>


                    <asp:TemplateField HeaderText="GFE Date" SortExpression="GFEDate" ItemStyle-Wrap="false"
                        ItemStyle-Width="60" ItemStyle-Height="30" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("GFEDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="LTV Ratio" SortExpression="LTVRatio" ItemStyle-Wrap="false"
                        ItemStyle-Width="100" HeaderStyle-Width="30"  ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LTVRatio")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Net Adjusted Price" SortExpression="NetAdjustedPrice" ItemStyle-Wrap="false"
                        ItemStyle-Width="50" HeaderStyle-Width="20" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("NetAdjustedPrice")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Lock Date" SortExpression="LockDate" ItemStyle-Wrap="false"
                        ItemStyle-Width="60" ItemStyle-Height="30" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LockDate")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="WebLender Loan Number" SortExpression="WebLenderLoanNumber" ItemStyle-Wrap="false"
                        ItemStyle-Width="30" HeaderStyle-Width="30" ItemStyle-Height="30" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("WebLenderLoanNumber")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Lender Case #" SortExpression="LenderCase" ItemStyle-Wrap="false"
                        ItemStyle-Width="50" HeaderStyle-Width="30" ItemStyle-Height="30" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("LenderCase")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Agency Case # " SortExpression="AgencyCase" ItemStyle-Wrap="false"
                        ItemStyle-Width="50" HeaderStyle-Width="30" ItemStyle-Height="30" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("AgencyCase")%>
                        </ItemTemplate>
                    </asp:TemplateField>

<%--                    

                    <asp:TemplateField HeaderText="Progress" SortExpression="Progress" HeaderStyle-Width="145"
                        HeaderStyle-HorizontalAlign="Center">
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
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            $<%# Eval("RunningTotal", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Low Range" SortExpression="LowRange" ItemStyle-Wrap="false"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            $<%# Eval("LowRange", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Medium Range" SortExpression="MediumRange" ItemStyle-Wrap="false"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            $<%# Eval("MediumRange", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="High Range" SortExpression="HighRange" ItemStyle-Wrap="false"
                        HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            $<%# Eval("HighRange", "{0:N2}")%>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">
                &nbsp;</div>
        </div>
    </div>
    </form>
</body>
</html>
