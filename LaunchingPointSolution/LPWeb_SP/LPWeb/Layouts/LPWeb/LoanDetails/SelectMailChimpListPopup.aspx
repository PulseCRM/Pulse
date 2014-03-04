<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectMailChimpListPopup.aspx.cs"
    Inherits="SelectMailChimpListPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Select MailChimp List Popup</title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css"
        runat="server" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">

        function BeforeSelect() {

            var SelectedCount = $("#gvMailChimpLists tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No mail chimp list was selected.");
                return;
            }

            if (SelectedCount > 1) {

                alert("Only one mail chimp list can be selected.");
                return;
            }

            var MailChimpListIDs = "";
            $("#gvMailChimpLists tr:not(:first) td :checkbox:checked").each(function () {

                var ChimpListID = $(this).attr("tag");
                if (MailChimpListIDs == "") {

                    MailChimpListIDs = ChimpListID;
                }
                else {

                    MailChimpListIDs += "," + ChimpListID;
                }
            });

            var GetIDsFunction = GetQueryString1("GetIDsFunction");
            var InvokeStr = GetIDsFunction + "('" + MailChimpListIDs + "')";

            eval(InvokeStr);
        }


        function CheckOne(CheckBox) {

            var IsChecked = CheckBox.checked;

            $("#gvMailChimpLists tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }
        }

        // cancel
        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);

        }
    </script>
</head>
<body style="width: 620;">
    <form id="form1" runat="server">
    <div>
        <div>
            <div style="margin-top: 15px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-left: 15px; padding-right: 5px;">
                            Branch:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlBranchs" runat="server" Width="150px" OnSelectedIndexChanged="ddlBranchs_OnSelectedIndexChanged" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 15px; padding-right: 5px;">
                            User:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlUsers" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 10px; width: 219px;">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="Btn-66" OnClick="btnFilter_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="padding-left: 10px; padding-top: 10px;">
                            <input type="button" class="Btn-66" id="btnSelect" value="Select" onclick="BeforeSelect();" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="padding-left: 10px; padding-right: 10px;">
                <div id="divToolBar" style="margin-top: 13px;">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 530px;">
                        <tr>
                            <td style="letter-spacing: 1px; text-align: right; font-size: 12px; margin-right: 10px;">
                                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
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
            <div id="divGrid" class="ColorGrid" style="width: 530px; margin-top: 5px; margin-left: 5px;">
                <asp:GridView ID="gvMailChimpLists" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                    Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                    OnSorting="gvMailChimpLists_Sorting" CellPadding="3" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <ItemTemplate>
                                <input type="checkbox" tag='<%# Eval("LId") %>' onclick="CheckOne(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch" SortExpression="Branch" HeaderStyle-Width="240">
                            <ItemTemplate>
                                <%# Eval("Branch")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User" SortExpression="UserName" HeaderStyle-Width="240">
                            <ItemTemplate>
                                <%# Eval("UserName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="List" SortExpression="List" HeaderStyle-Width="240px"
                            ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("List")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">
                    &nbsp;</div>
            </div>
        </div>
        <asp:HiddenField ID="hfdFileId" runat="server" />
        <asp:HiddenField ID="hfdContactID" runat="server" />
        <asp:HiddenField ID="hdnCloseDialogCodes" runat="server" />
    </div>
    </form>
</body>
</html>
