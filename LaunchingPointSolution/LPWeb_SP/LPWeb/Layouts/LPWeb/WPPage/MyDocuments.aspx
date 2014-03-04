<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyDocuments.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.MyDocuments"
    DynamicMasterPageFile="~masterurl/default.master" %>--%>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" CodeBehind="MyDocuments.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.MyDocuments,LPWeb,Version=1.0.0.0,Culture=neutral,PublicKeyToken=a2c3274f2ef313f2"
    AutoEventWireup="true" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document"
    MasterPageFile="~/_layouts/LPWeb/MasterPage/Super.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <div id="PageContent">
        <div>
            <div id="divModuleName" class="ModuleTitle">
                My Documents</div>
            <div class="SplitLine">
            </div>
            <div id="divDivision" class="ColorGrid" style="width: 1000px; margin-top: 10px;">
                <asp:GridView ID="gridList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                    Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                    CellPadding="3" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Modified" HeaderText="Modified" DataFormatString="{0:MM/dd/yyyy}"
                            ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">
                    &nbsp;</div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="PlaceHolderLeftNavBar">
</asp:Content>
