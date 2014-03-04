<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanReassignContactTab.aspx.cs"
    Inherits="LoanReassignContactTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Reassign Contact</title>
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
    <script type="text/javascript">

        function BeforeSave() {

//            if ($("#<%=gvContacts.ClientID %> tr td :checkbox[checked=true]").length == 0) {
//                alert("Please select one record .");
//                return false;
//            }

//            if ($("#<%=gvContacts.ClientID %> tr td :checkbox[checked=true]").length > 0) {
//                alert("Please select one record .");
//                return false;
//            }

            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("Please select one row .");
                return false;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return false;
            }

            $("#<%= hfdContactID.ClientID %>").val(checkedIds.pop());

            return true;
        }


        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gvContacts.ClientID %> :checkbox").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        // cancel
        function btnCancel_onclick() {

            //window.parent.DialogContactAssignClose();
            // cancel

            var CloseDialogCodes = $("#hdnCloseDialogCodes").val();
            if (CloseDialogCodes == "") {
                window.parent.DialogContactAssignClose();
                return;
            }

            eval(CloseDialogCodes);
        }
    </script>
</head>
<body style="width: 620;">
    <form id="form1" runat="server">
    <div>
        <div>
            <div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 82px;">
                            Borrower:
                        </td>
                        <td style="padding-left: 15px; width: 150px;">
                            <asp:Label ID="lbBorrower" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            Property:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:Label ID="lbProperty" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 15px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Service Type:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlServiceTypes" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 15px; width: 219px;">
                            <asp:Button ID="btnDisplay" runat="server" Text="Display" CssClass="Btn-66" OnClick="btnDisplay_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 15px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            Contact Role:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:DropDownList ID="ddlContactRole" runat="server" Width="150px">
                            </asp:DropDownList>
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()"
                                OnClick="btnSave_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="padding-left: 10px; padding-right: 10px;">
                <div id="divToolBar" style="margin-top: 13px;">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 600px;">
                        <tr>
                            <td style="letter-spacing: 1px; text-align: right; font-size: 12px; margin-right:10px;">
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
            <div id="divGrid" class="ColorGrid" style="width: 572px; margin-top: 5px; margin-left: 5px;
                overflow-x: scroll">
                <asp:GridView ID="gvContacts" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                    Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                    OnSorting="gvContacts_Sorting" CellPadding="3" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                Check
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" tag='<%# Eval("ContactId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service" SortExpression="ServiceTypes" HeaderStyle-Width="100">
                            <ItemTemplate>
                                <%# Eval("ServiceTypes")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Company" SortExpression="CompanyName" HeaderStyle-Width="100px"
                            ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("CompanyName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" Name" SortExpression="ContactsName" ItemStyle-Wrap="false"
                            ItemStyle-Width="100">
                            <ItemTemplate>
                                <%# Eval("ContactsName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City" SortExpression="MailingCity" ItemStyle-Wrap="false"
                            ItemStyle-Width="100">
                            <ItemTemplate>
                                <%# Eval("MailingCity")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Zip " SortExpression="MailingZip" ItemStyle-Wrap="false"
                            ItemStyle-Width="60">
                            <ItemTemplate>
                                <%# Eval("MailingZip")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        <input id="hfdFileId" runat="server" type="text" style="display: none;" />
        <asp:HiddenField ID="hfdContactID" runat="server" />
        <asp:HiddenField ID="hdnCloseDialogCodes" runat="server" />
    </div>
    </form>
</body>
</html>