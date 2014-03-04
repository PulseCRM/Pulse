<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectDetailRelationshipTab.aspx.cs" Inherits="Prospect_ProspectDetailRelationshipTab" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Client Detail</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridRelationshipList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridRelationshipList tr td :checkbox").attr("checked", "");
            }
        }

        function ShowDialog_SearchRelationship() {

            var ContactID = GetQueryString1("ContactID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SearchRelationshipPopup.aspx?sid=" + RadomStr + "&ContactID=" + ContactID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 500;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 460;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Search Relationship", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function BeforeDelete() {

            var SelectedCount = $("#gridRelationshipList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No relationship record was selected.");
                return false;
            }

            var Result = confirm("Are you sure you want to continue?");
            if (Result == false) {

                return false;
            }

            // DelContactIDs
            var DelContactIDs = "";
            var Directions = "";
            $("#gridRelationshipList tr:not(:first) td :checkbox:checked").each(function (i) {

                var DelContactID = $(this).attr("myRelContactID");
                var Direction = $(this).attr("myDirection");
                
                if (i == 0) {

                    DelContactIDs = DelContactID;
                    Directions = Direction;
                }
                else {

                    DelContactIDs += "," + DelContactID;
                    Directions += "," + Direction;
                }
            });

            $("#hdnDelContactIDs").val(DelContactIDs);
            $("#hdnDirections").val(Directions);

            return true;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        <div id="divToolBar" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0" style="width: 700px;">
                <tr>
                    <td style="width: 130px;">
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aAdd" href="javascript:ShowDialog_SearchRelationship()">Add</a><span>|</span></li>
                            <li><asp:LinkButton ID="lnkRemove" runat="server" OnClientClick="return BeforeDelete()" OnClick="lnkRemove_Click">Remove</asp:LinkButton></li>
                        </ul>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                            CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divRelationshipList" class="ColorGrid" style="margin-top: 1px; width: 700px;">
            <asp:GridView ID="gridRelationshipList" runat="server" EmptyDataText="There is no relationship." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkChecked" type="checkbox" myRelContactID="<%# Eval("RelContactID") %>" myDirection="<%# Eval("Direction") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ContactName" HeaderText="Contact" />
                    <asp:BoundField DataField="Relationship" HeaderText="Relationship" ItemStyle-Width="150px" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:HiddenField ID="hdnDelContactIDs" runat="server" />
        <asp:HiddenField ID="hdnDirections" runat="server" />
    </div>
    </form>
</body>
</html>