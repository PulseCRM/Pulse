<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" masterpagefile="/_catalogs/masterpage/LPWeb/Super.master" title="Testing This Page" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
     
<asp:Content id="Content1" runat="server" contentplaceholderid="PlaceHolderMain">
    <WebPartPages:WebPartZone runat="server" Title="loc:FullPage" ID="FullPage" FrameType="TitleBarOnly"
        AllowCustomization="false" AllowLayoutChange="false" AllowPersonalization="false" EmptyZoneText="List does not exist. The page you selected contains a list that does not exist. It may have been deleted by another user.">
        <ZoneTemplate>
        </ZoneTemplate>
    </WebPartPages:WebPartZone>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="PlaceHolderLeftNavBar">
</asp:Content>