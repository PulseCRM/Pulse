<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingActivitiesEvents.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Marketing.MarketingActivitiesEvents" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
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

        function completedStateChanged(me, eventId) {
            $.ajax({
                url: "MarketingActivitiesEvents.aspx?unsync=1&eventId=" + eventId + "&t=" + Math.random().toString(),
                success: function (msg) {
                    if ("" == msg)
                        $(me).attr("disabled", "disabled");
                    else {
                        $(me).attr("checked", "");
                        alert(msg);
                    }
                }
            });
        }

        function ShowEventContent(sEventId) {
            window.open("LoanCompaignEventContent.aspx?eventId=" + sEventId);
        }

        function showLoanDetail(sFileId) {
            window.parent.location.href = '../LoanDetails/LoanDetails.aspx?FromPage=<%= FromURL %>&fieldid=' + sFileId + "&fieldids=" + sFileId;
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
                        <asp:DropDownList ID="ddlCategories" runat="server" DataTextField="CategoryName"
                            DataValueField="CategoryId" Style="width: 100px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlCampaigns" runat="server" DataTextField="CampaignName" DataValueField="CampaignId"
                            Style="width: 200px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlStatuses" runat="server" Width="100px">
                            <asp:ListItem Value="">All Statuses</asp:ListItem>
                            <asp:ListItem Value="Active">Active</asp:ListItem>
                            <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
                            <asp:ListItem Value="Completed">Completed</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlType" runat="server" Width="100px">
                            <asp:ListItem Value="">All Types</asp:ListItem>
                            <asp:ListItem Value="Auto">Auto</asp:ListItem>
                            <asp:ListItem Value="Manual">Manual</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlStartBy" runat="server" DataTextField="StartedByUserName"
                            DataValueField="StartedBy" Width="120px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 10px;">
                        Execution Date:
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
                        <asp:DropDownList ID="ddlAlphabet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlphabet_SelectedIndexChanged">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="A">A</asp:ListItem>
                            <asp:ListItem Value="B">B</asp:ListItem>
                            <asp:ListItem Value="C">C</asp:ListItem>
                            <asp:ListItem Value="D">D</asp:ListItem>
                            <asp:ListItem Value="E">E</asp:ListItem>
                            <asp:ListItem Value="F">F</asp:ListItem>
                            <asp:ListItem Value="G">G</asp:ListItem>
                            <asp:ListItem Value="H">H</asp:ListItem>
                            <asp:ListItem Value="I">I</asp:ListItem>
                            <asp:ListItem Value="J">J</asp:ListItem>
                            <asp:ListItem Value="K">K</asp:ListItem>
                            <asp:ListItem Value="L">L</asp:ListItem>
                            <asp:ListItem Value="M">M</asp:ListItem>
                            <asp:ListItem Value="N">N</asp:ListItem>
                            <asp:ListItem Value="O">O</asp:ListItem>
                            <asp:ListItem Value="P">P</asp:ListItem>
                            <asp:ListItem Value="Q">Q</asp:ListItem>
                            <asp:ListItem Value="R">R</asp:ListItem>
                            <asp:ListItem Value="S">S</asp:ListItem>
                            <asp:ListItem Value="T">T</asp:ListItem>
                            <asp:ListItem Value="U">U</asp:ListItem>
                            <asp:ListItem Value="V">V</asp:ListItem>
                            <asp:ListItem Value="W">W</asp:ListItem>
                            <asp:ListItem Value="X">X</asp:ListItem>
                            <asp:ListItem Value="Y">Y</asp:ListItem>
                            <asp:ListItem Value="Z">Z</asp:ListItem>
                        </asp:DropDownList>
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
            OnRowDataBound="gridList_RowDataBound" CellPadding="3" GridLines="None" DataKeyNames="LoanMarketingEventId,FileId,BorrowerName,LoanStatus,PointFileName,Action,EventContent,Completed"
            OnSorting="gridList_Sorting">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField ItemStyle-CssClass="CheckBoxColumn" HeaderText="Completed" SortExpression="Completed">
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbCompleted" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CampaignName" HeaderText="Campaign" SortExpression="CampaignName" />
                <asp:TemplateField HeaderText="Event" SortExpression="Event">
                    <ItemTemplate>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="border: 0px;">
                                    <asp:Literal ID="litAction" runat="server"></asp:Literal>
                                </td>
                                <td style="padding-left: 3px; border: 0px;">
                                    <asp:Label ID="lblEvent" runat="server" Text='<%# Bind("Event") %>'></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Client" SortExpression="BorrowerName">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnClient" runat="server"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ExecutionDate" HeaderText="Execution Date" SortExpression="ExecutionDate"
                    DataFormatString="{0:MM/dd/yyyy}" ItemStyle-HorizontalAlign="Right" />
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    </form>
</body>
</html>
