<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailInbox.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.EmailInbox"
    DynamicMasterPageFile="~masterurl/default.master" %>--%>
    
<%@ Register TagPrefix="WpNs0" Namespace="Microsoft.SharePoint.Portal.WebControls"
    Assembly="Microsoft.SharePoint.Portal, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" CodeBehind="EmailInbox.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.EmailInbox,LPWeb,Version=1.0.0.0,Culture=neutral,PublicKeyToken=a2c3274f2ef313f2"
    meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document" MasterPageFile="~/_layouts/LPWeb/MasterPage/Super.master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <div id="PageContent">
        <div>
    <WebPartPages:WebPartZone runat="server" Title="loc:FullPage" ID="FullPage" FrameType="TitleBarOnly" AllowCustomization="false" AllowLayoutChange="false" AllowPersonalization="false"><ZoneTemplate> 
<WpNs0:OWAInboxPart runat="server" __MarkupType="xmlmarkup" WebPart="true" __WebPartId="{A40C7167-BDFB-4C65-B669-2D677096CFE5}" >
<WebPart xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://schemas.microsoft.com/WebPart/v2">
  <Title>My Inbox</Title>
  <FrameType>Default</FrameType>
  <Description>Displays your inbox using Outlook Web Access for Microsoft Exchange Server 2003 or later.</Description>
  <IsIncluded>true</IsIncluded>
  <ZoneID>wpz</ZoneID>
  <PartOrder>8</PartOrder>
  <FrameState>Normal</FrameState>
  <Height>350px</Height>
  <Width />
  <AllowRemove>true</AllowRemove>
  <AllowZoneChange>true</AllowZoneChange>
  <AllowMinimize>true</AllowMinimize>
  <AllowConnect>true</AllowConnect>
  <AllowEdit>true</AllowEdit>
  <AllowHide>true</AllowHide>
  <IsVisible>true</IsVisible>
  <DetailLink />
  <HelpLink />
  <HelpMode>Modeless</HelpMode>
  <Dir>Default</Dir>
  <PartImageSmall />
  <MissingAssembly>Cannot import this Web Part.</MissingAssembly>
  <PartImageLarge>/_layouts/images/wp_pers.gif</PartImageLarge>
  <IsIncludedFilter />
  <ExportControlledProperties>true</ExportControlledProperties>
  <ConnectionID>00000000-0000-0000-0000-000000000000</ConnectionID>
  <ID>g_a2493b89_a22c_40a2_928e_d3873c0cca02</ID>
  <ExchangeAutodiscoveryInitialized xmlns="urn:schemas-microsoft-com:owapart">true</ExchangeAutodiscoveryInitialized>
  <ViewName xmlns="urn:schemas-microsoft-com:owainboxwebpart">Two Line</ViewName>
</WebPart>
</WpNs0:OWAInboxPart>
    </ZoneTemplate></WebPartPages:WebPartZone>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="PlaceHolderLeftNavBar">
</asp:Content>