<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MergeContactsPopup.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Pipeline.MergeContactsPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <title>Merge Contacts</title>
    <script type="text/javascript">
        function returnFn() {

        }

        // call back
        function callBack(sReturn) {
            if (window.parent && window.parent.getFolderSelectionReturn)
                window.parent.getFolderSelectionReturn(sReturn);
        }
        $(document).ready(function () {

            $("#<%= gvContacts.ClientID %> input[type='radio']").each(function () {
                $(this).attr("name", "to");
                $(this).click(function () {
                    var $this = $(this);
                    $this.parent().prev().children("input[type='checkbox']").attr("checked", false);
                    //                    console.info($this.parent().prevAll("td:last").text());
                });
            });

            $("#<%= gvContacts.ClientID %> input[type='checkbox']").each(function () {
                $(this).click(function () {
                    var $this = $(this);
                    $this.parent().next().children("input[type='radio']").attr("checked", false);
                    //                    console.info($this.parent().prevAll("td:last").text());
                });
            });

            $("#<%= btnMerge.ClientID %>").click(function () {
                return getSelectedItems();
            });

        });

        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%= gvContacts.ClientID %>  :checkbox").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            
            $("#<%=htTo.ClientID %>").val("");
             $("#<%=hfFroms.ClientID %>").val("");
            $("#<%= gvContacts.ClientID %>  :radio:checked").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    $("#<%=htTo.ClientID %>").val(item.attr("tag"));
                }
            });

            $("#<%=hfFroms.ClientID %>").val(selctedItems.join(","));

            if ($("#<%=hfFroms.ClientID %>").val() == "") {
                alert("Please select from prospect(s).");
                return false;
            }

            if ($("#<%=htTo.ClientID %>").val() == "") {
                alert("Please select to prospect.");
                return false;
            }
        }
        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }
    </script>
    <style>
        .hide
        {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin-top: 20px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Button ID="btnMerge" runat="server" Text="Merge" CssClass="Btn-66" OnClick="btnMerge_Click" />
                </td>
                <td style="padding-left: 8px;">
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick();" />
                </td>
            </tr>
        </table>
    </div>
    <div id="div1" class="ColorGrid" style="margin-top: 5px; width: 600px;">
        <asp:GridView ID="gvContacts" runat="server" DataKeyNames="contactid" EmptyDataText="There is no data in database."
            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:BoundField DataField="contactid" HeaderText="Client" HeaderStyle-CssClass="hide"
                    ItemStyle-CssClass="hide" />
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        From
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("ContactId") %>' />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        To
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input id="rbTo" runat="server" name="to" type="radio"  tag='<%# Eval("ContactId") %>' />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="Contact" HeaderText="Contact" HeaderStyle-CssClass="itemHeader" />
                <asp:BoundField DataField="Service Type" HeaderText="Service Type" HeaderStyle-CssClass="itemHeader" />
                <asp:BoundField DataField="Branch" HeaderText="Branch" HeaderStyle-CssClass="itemHeader" />
                <asp:BoundField DataField="Company" HeaderText="Company" HeaderStyle-CssClass="itemHeader" HeaderStyle-Width="100"/>
            </Columns>
            <SelectedRowStyle BackColor="#E4E7EF" />
        </asp:GridView>
        <asp:HiddenField ID="hfFroms" runat="server" />
        <asp:HiddenField ID="htTo" runat="server" />
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    </form>
</body>
</html>
