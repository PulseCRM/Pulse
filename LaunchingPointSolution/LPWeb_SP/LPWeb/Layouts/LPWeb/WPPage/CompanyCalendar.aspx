<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyCalendar.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.WPPage.CompanyCalendar" DynamicMasterPageFile="~masterurl/default.master" %>--%>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" CodeBehind="CompanyCalendar.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.CompanyCalendar,LPWeb,Version=1.0.0.0,Culture=neutral,PublicKeyToken=a2c3274f2ef313f2"
    AutoEventWireup="true" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document"
    MasterPageFile="~/_layouts/LPWeb/MasterPage/Super.master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <div id="PageContent">
        <div>
            <WebPartPages:WebPartZone runat="server" Title="loc:FullPage" ID="FullPage" FrameType="TitleBarOnly"
                AllowCustomization="false" AllowLayoutChange="false" AllowPersonalization="false">
                <ZoneTemplate>
                    <WebPartPages:ListViewWebPart ID="lvwpComCal" runat="server" __MarkupType="xmlmarkup"
                        WebPart="true" __WebPartId="{CB1E95C5-D036-4AEA-AF8C-8B583B1B016B}">
<WebPart xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://schemas.microsoft.com/WebPart/v2">
  <Title>Calendar</Title>
  <FrameType>Default</FrameType>
  <Description>Use the Calendar list to keep informed of upcoming meetings, deadlines, and other important events.</Description>
  <IsIncluded>true</IsIncluded>
  <PartOrder>0</PartOrder>
  <FrameState>Normal</FrameState>
  <Height />
  <Width />
  <AllowRemove>true</AllowRemove>
  <AllowZoneChange>true</AllowZoneChange>
  <AllowMinimize>true</AllowMinimize>
  <AllowConnect>true</AllowConnect>
  <AllowEdit>true</AllowEdit>
  <AllowHide>true</AllowHide>
  <IsVisible>true</IsVisible>
  <DetailLink>/Lists/Calendar</DetailLink>
  <HelpLink />
  <HelpMode>Modeless</HelpMode>
  <Dir>Default</Dir>
  <PartImageSmall />
  <MissingAssembly>Cannot import this Web Part.</MissingAssembly>
  <PartImageLarge>/_layouts/images/itevent.png</PartImageLarge>
  <IsIncludedFilter />
  <ExportControlledProperties>false</ExportControlledProperties>
  <ConnectionID>00000000-0000-0000-0000-000000000000</ConnectionID>
  <ID>g_dc029088_457a_4c2e_9ca2_b2d8356f937b</ID>
  <WebId xmlns="http://schemas.microsoft.com/WebPart/v2/ListView">00000000-0000-0000-0000-000000000000</WebId>
  <ListViewXml xmlns="http://schemas.microsoft.com/WebPart/v2/ListView">&lt;View Name="{CB1E95C5-D036-4AEA-AF8C-8B583B1B016B}" MobileView="TRUE" Type="CALENDAR" Hidden="TRUE" TabularView="FALSE" RecurrenceRowset="TRUE" DisplayName="" Url="/SitePages/Test Inbox.aspx" Level="1" BaseViewID="2" ContentTypeID="0x" MobileUrl="_layouts/mobile/viewdaily.aspx" ImageUrl="/_layouts/images/events.png"&gt;&lt;Toolbar Type="Standard"/&gt;&lt;ViewHeader/&gt;&lt;ViewBody/&gt;&lt;ViewFooter/&gt;&lt;ViewEmpty/&gt;&lt;ParameterBindings&gt;&lt;ParameterBinding Name="NoAnnouncements" Location="Resource(wss,noXinviewofY_LIST)"/&gt;&lt;ParameterBinding Name="NoAnnouncementsHowTo" Location="Resource(wss,noXinviewofY_DEFAULT)"/&gt;&lt;/ParameterBindings&gt;&lt;ViewFields&gt;&lt;FieldRef Name="EventDate"/&gt;&lt;FieldRef Name="EndDate"/&gt;&lt;FieldRef Name="fRecurrence"/&gt;&lt;FieldRef Name="EventType"/&gt;&lt;FieldRef Name="Attachments"/&gt;&lt;FieldRef Name="WorkspaceLink"/&gt;&lt;FieldRef Name="Title"/&gt;&lt;FieldRef Name="Location"/&gt;&lt;FieldRef Name="Description"/&gt;&lt;FieldRef Name="Workspace"/&gt;&lt;FieldRef Name="MasterSeriesItemID"/&gt;&lt;FieldRef Name="fAllDayEvent"/&gt;&lt;/ViewFields&gt;&lt;ViewData&gt;&lt;FieldRef Name="Title" Type="CalendarMonthTitle"/&gt;&lt;FieldRef Name="Title" Type="CalendarWeekTitle"/&gt;&lt;FieldRef Name="Location" Type="CalendarWeekLocation"/&gt;&lt;FieldRef Name="Title" Type="CalendarDayTitle"/&gt;&lt;FieldRef Name="Location" Type="CalendarDayLocation"/&gt;&lt;/ViewData&gt;&lt;Query&gt;&lt;Where&gt;&lt;DateRangesOverlap&gt;&lt;FieldRef Name="EventDate"/&gt;&lt;FieldRef Name="EndDate"/&gt;&lt;FieldRef Name="RecurrenceID"/&gt;&lt;Value Type="DateTime"&gt;&lt;Month/&gt;&lt;/Value&gt;&lt;/DateRangesOverlap&gt;&lt;/Where&gt;&lt;/Query&gt;&lt;/View&gt;</ListViewXml>
  <ListName xmlns="http://schemas.microsoft.com/WebPart/v2/ListView">{6F0DC870-1BEC-423C-B7EF-3B7C84785CA9}</ListName>
  <ListId xmlns="http://schemas.microsoft.com/WebPart/v2/ListView">6f0dc870-1bec-423c-b7ef-3b7c84785ca9</ListId>
  <ViewFlag xmlns="http://schemas.microsoft.com/WebPart/v2/ListView">8921097</ViewFlag>
  <ViewFlags xmlns="http://schemas.microsoft.com/WebPart/v2/ListView">Html Hidden RecurrenceRowset Calendar Mobile</ViewFlags>
  <ViewContentTypeId xmlns="http://schemas.microsoft.com/WebPart/v2/ListView">0x</ViewContentTypeId>
</WebPart>
                    </WebPartPages:ListViewWebPart>
                </ZoneTemplate>
            </WebPartPages:WebPartZone>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="PlaceHolderLeftNavBar">
</asp:Content>
