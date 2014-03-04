<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLoanRepSelection.aspx.cs"
    Inherits="LPWeb.Settings.UserLoanRepSelection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <title>User Loan Rep Selection</title>
    <script type="text/javascript">
        // check/decheck all
        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#" + '<%=gridLoanRep.ClientID %>' + " tr td input[type=checkbox]").each(function () {
                    if (this.disabled) return;
                    this.checked = true;
                });
            }
            else {
                $("#" + '<%=gridLoanRep.ClientID %>' + " tr td input[type=checkbox]").each(function () {
                    if (this.disabled) return;
                    this.checked = false;
                });
            }
        }
        function IsRowSelected(args) {
            if ($("#" + '<%=gridLoanRep.ClientID %>' + " tr td :checkbox[checked=true]").length == 0) {
                ShowMsg("noRowSelected");
            }
            else {
                // give confirm info
                if ("disable" == args) {
                    ShowMsg("disableCfm");
                }
                else if ("delete" == args) {
                    ShowMsg("deleteCfm");
                }
            }
            return false;
        }

        function returnFn() {
            <%=this.ClientScript.GetPostBackEventReference(this.lbtnAdd, null) %>
        }

        // call back
        function callBack(sReturn)
        {
            if(window.parent && window.parent.getLoanRepSelectionReturn)
                window.parent.getLoanRepSelectionReturn(sReturn);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnFilterLastName">
    Loan Officer Last Name:
    <asp:TextBox ID="txbLastName" runat="server"></asp:TextBox>
    <asp:Button ID="btnFilterLastName"  runat="server" Text="Filter" class="Btn-66" OnClick="btnFilterLastName_Click" />
    <div id="div1" class="ColorGrid" style="margin-top: 5px;">
        <asp:GridView ID="gridLoanRep" runat="server" DataKeyNames="NameId,Name" EmptyDataText="There is no record in database."
            AutoGenerateColumns="False" OnRowDataBound="gridLoanRep_RowDataBound" CellPadding="3" CssClass="GrayGrid" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="NameId" HeaderText="ID" />
                <asp:BoundField DataField="Name" HeaderText="Name" />
            </Columns>
            <SelectedRowStyle BackColor="#E4E7EF" />
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    <asp:LinkButton ID="lbtnAdd" runat="server" onclick="lbtnAdd_Click"></asp:LinkButton>
    </form>
</body>
</html>
