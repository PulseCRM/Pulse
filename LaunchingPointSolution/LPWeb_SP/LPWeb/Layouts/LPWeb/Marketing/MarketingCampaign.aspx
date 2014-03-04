<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingCampaign.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Marketing.MarketingCampaign" MasterPageFile="~/_layouts/LPWeb/MasterPage/Marketing.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    

    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">


        function GoToLink(CampaignID) {
            document.getElementById("<%= this.iFrmURL.ClientID %>").src = "MarketingCampaignDetail.aspx?campaignId=" + CampaignID + "&pageindex=" + <%= iPageIndex %>;
        }

        var currentRowId = 0;
        function SelectRow() {
            if (event.keyCode == 40)
                MarkRow(currentRowId + 1);
            else if (event.keyCode == 38)
                MarkRow(currentRowId - 1);
        } 

        function MarkRow(rowId) {
            if (document.getElementById(rowId) == null)
            {
                return;
            }
            if (document.getElementById(currentRowId) != null)
            {
                document.getElementById(currentRowId).style.backgroundColor = '#ffffff';
                document.getElementById(currentRowId).style.color = "#818892";
                $("#" + currentRowId + " a").css("color", "#587ec6");
             }
            currentRowId = rowId;
            document.getElementById(rowId).style.backgroundColor = '#5880B3';
            document.getElementById(rowId).style.color = "Orange";
            $("#" + rowId + " a").css("color", "Orange");
   
        } 

// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer">
        <table width="100%" style="height: 460px;">
            <tr>
                <td style="vertical-align: top; width: 300px">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlCategory" runat="server" Style="width: 200px">
                                    <asp:ListItem Text="All Categories" Value="-1" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 10px;">
                                <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click" />
                            </td>
                        </tr>
                    </table>
                    <div style="text-align: left; padding-right: 5px;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex1" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            OnPageChanged="AspNetPager1_PageChanged" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                            ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                            LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </div>
                    <div id="divMarketingCampaign" class="ColorGrid" style="margin-top: 5px;">
                        <asp:GridView ID="gridMarketingCampaign" runat="server" EmptyDataText="There is no marketing campaign."
                            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" OnRowDataBound="gridMarketingCampaign_RowDataBound"
                            GridLines="None">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <Columns>
                                <asp:TemplateField HeaderText="Marketing Campaign" ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <a class="aa" href="javascript:GoToLink(<%# Eval("CampaignId")%>)" campaignid="<%# Eval("CampaignId")%>">
                                            <%# Eval("CampaignName")%></a>
                                        <asp:TextBox ID="tbID" runat="server" Text='<%# Eval("CampaignId")%>' Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <SelectedRowStyle BackColor="LightCyan" ForeColor="DarkBlue" Font-Bold="true" />
                        </asp:GridView>
                        <div class="GridPaddingBottom">
                            &nbsp;</div>
                    </div>
                </td>
                <td style="padding-left: 15px; vertical-align: top; width：800px">
                    <iframe id="iFrmURL" runat="server" style="width: 100%; height: 450px" frameborder="0">
                    </iframe>
                </td>
            </tr>
        </table>
    </div>
    <script language="javascript" type="text/javascript">
        var sCampaignID = "<%=iCampaignId %>";
        var sSelID = "<%=_Queryi %>";
        if (sCampaignID != 0) {
            MarkRow(sCampaignID);
            GoToLink(sCampaignID);
        }
    </script>
</asp:Content>
