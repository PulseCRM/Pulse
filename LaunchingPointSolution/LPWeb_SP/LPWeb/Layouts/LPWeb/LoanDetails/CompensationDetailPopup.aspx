<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompensationDetailPopup.aspx.cs" Inherits="CompensationDetailPopup" %>

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
    <title>Compensation Detail</title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divModuleName" class="ModuleTitle" style="margin-left:15px;">Compensation Detail</div>
    <div style="margin-top: 15px; margin-left:20px;">Loan Amount: $<asp:Label ID="labLoanamount" runat="server" Text="0"></asp:Label> </div>
    <div id="div1" class="ColorGrid" style="margin-top: 15px; margin-left:15px; width:500px;">
        <asp:GridView ID="gvCompDetail" runat="server" DataKeyNames="Type" EmptyDataText="No data in the database."
            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:BoundField DataField="Type" HeaderText="Type"><ItemStyle HorizontalAlign="Left"></ItemStyle> </asp:BoundField>
                <asp:BoundField DataField="Name" HeaderText="Name"><ItemStyle HorizontalAlign="Left"></ItemStyle> </asp:BoundField>
                <asp:BoundField DataField="Rate" HeaderText="%"  DataFormatString="{0:0.000}"><ItemStyle HorizontalAlign="Right"></ItemStyle> </asp:BoundField>
                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:c}"><ItemStyle HorizontalAlign="Right"></ItemStyle> </asp:BoundField>
            </Columns>
            <SelectedRowStyle BackColor="#E4E7EF" />
        </asp:GridView>

        <div class="GridPaddingBottom">
            &nbsp;</div>

    </div>

    </form>
</body>
</html>