<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CloseLoanPopupWindowOpen.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.LoanDetails.CloseLoanPopupWindowOpen" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Move Point file to Archived Folder</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        // <![CDATA[
        function ckbClicked(me) {
            var bCheck = $(me).attr('checked');
            if (bCheck) {
                $('input:checkbox', $('#' + '<%=gvFolder.ClientID %>')).each(function () {
                    if (this != me)
                        $(this).attr('checked', false);
                });
            }
        }
        // ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 600px;" class="AlignCenter">
    <div id="divModuleName" class="ModuleTitle">Move Point file to Archived/Closed Loan Folder</div>
    <div class="SplitLine"></div>
    <div id="div1" class="ColorGrid" style="margin-top: 15px;">
        <asp:GridView ID="gvFolder" runat="server" DataKeyNames="FolderId,Path" EmptyDataText="No archived/closed folder in the database."
            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" onclick="ckbClicked(this)" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="Path" HeaderText="Path" />
                <asp:BoundField DataField="StatusName" HeaderText="Loan Status" />
            </Columns>
            <SelectedRowStyle BackColor="#E4E7EF" />
        </asp:GridView>

        <div class="GridPaddingBottom"> &nbsp;</div>
        <asp:Button ID="lbtnSelect" runat="server" Text="Move Point File" Width="150px" onclick="lbtnSelect_Click" />
    </div>
    </div>
    <asp:HiddenField ID="hdnUserId" runat="server"/>
    <asp:HiddenField ID="hdnFileId" runat="server"/>
    </form>
</body>
</html>