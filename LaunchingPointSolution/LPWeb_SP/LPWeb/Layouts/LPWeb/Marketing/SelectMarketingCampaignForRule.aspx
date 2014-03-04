<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectMarketingCampaignForRule.aspx.cs" Inherits="SelectMarketingCampaignForRule"  %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Select Marketing Campaign</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {


        });

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridMarketingCampaignList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridMarketingCampaignList tr td :checkbox").attr("checked", "");
            }
        }

        function BeforeSubmit() {

            if ($("#gridMarketingCampaignList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("Please select a marketing campaign.");
                return false;
            }

            var type = GetQueryString1("type");

            var SelectedCampaignIDs = "";
            if ($("#gridMarketingCampaignList tr:not(:first) td :checkbox:checked").length > 1) {

                alert("You can select only one record.");
                return false;
            }
            $("#gridMarketingCampaignList tr:not(:first) td :checkbox:checked").each(function (i) {

                var CampaignID = $(this).attr("CampaignID");

                if (i == 0) {

                    SelectedCampaignIDs = CampaignID;
                }
                else {

                    SelectedCampaignIDs += "," + CampaignID;
                }
            });


            $("#hdnSelectedCampaignIds").val(SelectedCampaignIDs);

            return true;
        }

        // call back
        function callBack(sReturn, text, CategoryName) {
            if (window.parent && window.parent.getCampaignSelectionReturn)
                window.parent.getCampaignSelectionReturn(sReturn, text, CategoryName);
            else {
                btnCancel_onclick();
                var sRecieveDataCodes = GetQueryString1("RecieveDataCodes");
                sRecieveDataCodes = sRecieveDataCodes.replace("returnValue", sReturn);
                eval(sRecieveDataCodes);
            }
        }

        function btnCancel_onclick() {
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            if ($.browser.msie == true) {
                eval(CloseDialogCodes);
            }
        }


// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="PopupContainer">
         <div id="div1" class="Heading" style="display:none;">
           Select Marketing Campaign
        </div>
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="margin-left:35px; width:100px;">
                       Marketing Category :
                    </td>
                    <td style="margin-left:20px;">
                        <asp:DropDownList ID="ddlCategory" runat="server" Style="width: 250px">
                            <asp:ListItem Text="All Categories" Value="-1" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 30px; width:100px">
                        <asp:Button ID="btnFilter" runat="server" Text="Display" class="Btn-66" OnClick="btnFilter_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divButtons" style="margin-top: 20px;">
            <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="Btn-66" OnClientClick="return BeforeSubmit();" onclick="btnSelect_Click" />&nbsp;
            
        </div>
        <div id="divGridContainer" style="margin-top: 5px;">
            
            <div  style="letter-spacing: 1px; text-align: right; font-size: 12px; height:20px;" >
                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                    OnPageChanged="AspNetPager1_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <div id="divMarketingCampaignList" class="ColorGrid">
                <asp:GridView ID="gridMarketingCampaignList" DataKeyNames="CampaignId" runat="server" EmptyDataText="There is no Marketing Campaign." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" CampaignId="<%# Eval("CampaignId") %>" CampaignName="<%# Eval("CampaignName") %>"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CampaignName" ControlStyle-Width="300px" HeaderText="Marketing Campaign" HeaderStyle-CssClass="CheckBoxHeader"/>
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        <asp:HiddenField ID="hdnSelectedCampaignIds" runat="server" />
    </div>
    </form>
</body>
</html>
