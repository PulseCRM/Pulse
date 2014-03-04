<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingAccountBalanceTransactions.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Marketing.MarketingAccountBalanceTransactions" %>


<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        $(document).ready(function () {
            $(".DateField").datepick();
        });

        function showMarketingInfo(sFileId, sLoanStatus) {
            if (sLoanStatus.toLowerCase() == "prospect") {
                window.parent.location.href = '../Prospect/ProspectLoanDetails.aspx?tab=Marketing&FromPage=<%= FromURL %>&FileID=' + sFileId + "&FileIDs=" + sFileId;
            }
            else {
                window.parent.location.href = '../LoanDetails/LoanDetails.aspx?tab=Marketing&FromPage=<%= FromURL %>&fieldid=' + sFileId + "&fieldids=" + sFileId;
            }
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-left: 0px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 0px;">
            <table cellpadding="0" cellspacing="0" style="margin-top: 0px;">
                <tr>
                    <td>
                        Action:
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlAction" runat="server" Style="width: 100px">
                            <asp:ListItem Value="">All Actions</asp:ListItem>
                            <asp:ListItem Value="Add">Add</asp:ListItem>
                            <asp:ListItem Value="Charge">Charge</asp:ListItem>
                            <asp:ListItem Value="Credit">Credit</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 10px;">
                        Date Range:
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:TextBox ID="tbStartDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 3px;">
                        <asp:TextBox ID="tbEndDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0" style="width: 1000px;">
                <tr>
                    <td style="width: 40px;">
                    </td>
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
    </div>
    <div id="divDivision" class="ColorGrid" style="width: 1000px; margin-top: 5px;">
        <asp:GridView ID="gridList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
            OnRowDataBound="gridList_RowDataBound" CellPadding="3" GridLines="None" DataKeyNames="TransId,UserId,LoanMarketingId,FileId,LoanStatus,Description"
            OnSorting="gridList_Sorting">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:BoundField DataField="TransTime" HeaderText="Date" SortExpression="TransTime" DataFormatString="{0:g}" />
                <asp:BoundField DataField="Action" HeaderText="Action" SortExpression="Action" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance"
                    ItemStyle-HorizontalAlign="Right" DataFormatString="{0:C}" />
                <asp:TemplateField HeaderText="Description" SortExpression="Description">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnDesc" runat="server"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    </div>
    </form>
</body>
</html>