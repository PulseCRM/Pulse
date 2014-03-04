<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegionSetupExecutiveSelection.aspx.cs" Inherits="LPWeb.Settings.RegionSetupExecutiveSelection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
     <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
     <script type="text/javascript">
         // check/decheck all
         function CheckAll2(CheckBox) {
             var operGridId = "#" + $(CheckBox).parents("table").attr("id");
             if (CheckBox.checked) {
                 $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
             }
             else {

                 $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
             }
         }

         var gridExeSelectionClientId = "#<%= gridExecutiveSelectionList.ClientID %>";
         
     </script>
</head>
<body STYLE='OVERFLOW:SCROLL;OVERFLOW-X:HIDDEN;OVERFLOW-Y:HIDDEN' onload="window.parent.CheckExistMembers()";>
    <form id="form1" runat="server">
    <div style="width: 418px; height: 370px; overflow: auto;">
         <div id="div6" class="ColorGrid" style="width: 400px;">
                <asp:GridView ID="gridExecutiveSelectionList" runat="server" DataKeyNames="UserId"
                    AutoGenerateColumns="false" EmptyDataText="There is no executive for selection." CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this,'#<%= gridExecutiveSelectionList.ClientID %>')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Checkbox2" type="checkbox" title="<%# Eval("UserId") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                ID
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((GridViewRow)Container).RowIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Full Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("LastName") %>.<%# Eval("FirstName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                    </Columns>
                </asp:GridView>
                 <div class="GridPaddingBottom">
                    &nbsp;</div>
        </div>
        </div>
    </form>
</body>
</html>
