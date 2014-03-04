<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingCampaignSelection.aspx.cs" Inherits="Settings_MarketingCampaignSelection" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Marketing Campaign</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            var CategoryID = GetQueryString1("CategoryID");

            if (CategoryID != "") {

                $("#ddlMarketingCategory").val(CategoryID);
            }
        });

        function CheckOne(CheckBox) {

            var IsChecked = CheckBox.checked;

            $("#gridCampaignList tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }
        }

        function btnSelect_onclick() {

            if ($("#gridCampaignList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("Please select a campaign.");
                return;
            }

            var CategoryID = $("#gridCampaignList tr:not(:first) td :checkbox:checked").attr("myCategoryID");
            var CategoryName = $("#gridCampaignList tr:not(:first) td :checkbox:checked").attr("myCategoryName");

            var CampaignID = $("#gridCampaignList tr:not(:first) td :checkbox:checked").attr("myCampaignID");
            var CampaignName = $("#gridCampaignList tr:not(:first) td :checkbox:checked").attr("myCampaignName");

            //alert(RuleID);
            //alert(RuleName);

            window.parent.SetCampaignInfo(CategoryID, CategoryName, CampaignID, CampaignName);

            Cancel();
        }

        function btnDisplay_onclick() {

            var SelCategoryID = $("#ddlMarketingCategory").val();

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var NewUrl = "MarketingCampaignSelection.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()&CategoryID=" + SelCategoryID;

            window.location.href = NewUrl;
        }

        function Cancel() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 500px; border: solid 0px red;">
        <table cellspacing="4">
            <tr>
                <td>Marketing Category:</td>
                <td>
                    <asp:DropDownList ID="ddlMarketingCategory" runat="server" DataTextField="CategoryName" DataValueField="CategoryId" Width="250px">
                    </asp:DropDownList>
                </td>
                <td>
                    <input id="btnDisplay" type="button" value="Display" class="Btn-66" onclick="btnDisplay_onclick()" />
                </td>
            </tr>
        </table>
        <div style="margin-top: 10px;">
            <input id="btnSelect" type="button" value="Select" class="Btn-66" onclick="btnSelect_onclick()" />
        </div>
        <div style="text-align: right;">
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" 
                LayoutType="Table" onpagechanged="AspNetPager1_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <div id="divCampaignList" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridCampaignList" runat="server" EmptyDataText="There is no campaign." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <ItemTemplate>
                            <input id="chkSelected" type="checkbox" myCampaignID="<%# Eval("CampaignId")%>" myCampaignName="<%# LPWeb.Common.Encrypter.Base64Encode(Eval("CampaignName").ToString()) %>" 
                            myCategoryID="<%# Eval("CategoryId")%>" myCategoryName="<%# LPWeb.Common.Encrypter.Base64Encode(Eval("CategoryName").ToString()) %>" onclick="CheckOne(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CampaignName" HeaderText="Marketing Campaign" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
    </div>
    </form>
</body>
</html>