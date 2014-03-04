<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingActiviteCompain.aspx.cs"
    Inherits="MarketingActiviteCompain" %>

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
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
     
    <script src="../js/jquery.datepick.js" type="text/javascript"></script> 
    <script src="../js/date.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[ 
        var ddlStartedBy = "<%= this.ddlStartedBy.ClientID %>";
        $(document).ready(function () {
            $(".DateField").datepick();

            // reset container iframe height
            $("#tabFrame", window.parent.document).height($(document.body).height() + 10);
        });

        function GoToEvents(CampaignId) {
            window.parent.SetTab('MarketingActivitiesEvents.aspx', 1, CampaignId);
        }

        function GoToLoanDetails(LoanID) {
            window.parent.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=" + encodeURIComponent(window.location.href) + "&fieldid=" + LoanID + "&fieldids=" + LoanID;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 110px;">
                            <asp:DropDownList ID="ddlCategories" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategories_SelectedIndexChanged"
                                DataValueField="CategoryId" DataTextField="CategoryName" Width="100px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 210px;">
                            <asp:DropDownList ID="ddlCampaigns" runat="server" DataValueField="CampaignId" DataTextField="CampaignName"
                                Width="200px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:DropDownList ID="ddlStatus" runat="server" Width="100px">
                                <asp:ListItem>All Statuses</asp:ListItem>
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 110px;">
                            <asp:DropDownList ID="ddlType" runat="server" Width="100px">
                                <asp:ListItem>All Types</asp:ListItem>
                                <asp:ListItem>Auto</asp:ListItem>
                                <asp:ListItem>Manual</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 130px;">
                            <asp:DropDownList ID="ddlStartedBy" runat="server" DataValueField="StartedByID" DataTextField="StartedByName"
                                Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td>
                            Start Date:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:TextBox ID="txbFromDate" runat="server" CssClass="DateField"></asp:TextBox>
                        </td>
                        <td style="padding-left: 3px;">
                            <asp:TextBox ID="txbToDate" runat="server" CssClass="DateField"></asp:TextBox>
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:Button ID="btnFilter" class="Btn-66" runat="server" Text="Filter" OnClick="btnFilter_Click" />
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="0" style="width: 100%; padding-top:10px;">
                    <tr>
                        <td>
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
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
                                UrlPaging="false" FirstPageText="<<" LastPageText=">>" OnPageChanged="AspNetPager1_PageChanged"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>
                <div id="divMarketingActivityList" class="ColorGrid" style="margin-top: 5px;">
                    <asp:GridView ID="gridMarketingActivityList" runat="server" DataSourceID="MarketingActivitySqlDataSource"
                        EmptyDataText="There is no marketing activity." AutoGenerateColumns="False" CellPadding="3"
                        CssClass="GrayGrid" GridLines="None" AllowSorting="true" OnSorting="gridList_Sorting">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <img src="../images/Marketing-Red.gif" title="<%# Eval("Error") %>" style="margin-right: 5px;
                                        <%# Eval("Success").ToString() == "False" ? "": "display:none;" %>" /><%# Eval("Status") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Auto" ItemStyle-CssClass="CheckBoxColumn">
                                <ItemTemplate>
                                    <input id="Checkbox1" type="checkbox" disabled <%# Eval("Type").ToString() == "Auto" ? "checked" : "" %> />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Campaign" SortExpression="CampaignName" ItemStyle-Width="200px">
                                <ItemTemplate>
                                    <a href="javascript:GoToEvents('<%# Eval("CampaignId")%>')">
                                        <%# Eval("CampaignName")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="ClientName" HeaderText="Client" SortExpression="ClientName"
                                ItemStyle-Width="200px" />
                            <asp:TemplateField HeaderText="Loan" SortExpression="LoanName" ItemStyle-Width="300px">
                                <ItemTemplate>
                                    <a href="javascript:GoToLoanDetails('<%# Eval("FileId")%>')">
                                        <%# Eval("LoanName")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="StartedByName" HeaderText="Started By" SortExpression="StartedByName"
                                ItemStyle-Width="100px" />
                            <asp:BoundField DataField="Started" HeaderText="Started" SortExpression="Started"
                                ItemStyle-Width="70px" DataFormatString="{0:d}" ItemStyle-HorizontalAlign="Right" />
                        </Columns>
                    </asp:GridView>
                    <div class="GridPaddingBottom">
                        &nbsp;</div>
                </div>
                <asp:SqlDataSource ID="MarketingActivitySqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager"
                    SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
                    <SelectParameters>
                        <asp:Parameter Name="OrderByField" Type="String" DefaultValue="ClientName" />
                        <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                        <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                        <asp:Parameter Name="DbTable" Type="String" DefaultValue="" />
                        <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                        <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex"
                            PropertyName="StartRecordIndex" Type="Int32" />
                        <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="10" Name="EndIndex"
                            PropertyName="EndRecordIndex" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
    </form>
</body>
</html>
