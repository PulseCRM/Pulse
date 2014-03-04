<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SharedDocuments.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.WPPage.SharedDocuments" DynamicMasterPageFile="~masterurl/default.master" %>--%>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" CodeBehind="SharedDocuments.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.SharedDocuments,LPWeb,Version=1.0.0.0,Culture=neutral,PublicKeyToken=a2c3274f2ef313f2"
    AutoEventWireup="true" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document"
    MasterPageFile="~/_layouts/LPWeb/MasterPage/Super.master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <div id="PageContent">
        <div>
            <WebPartPages:WebPartZone runat="server" Title="loc:FullPage" ID="FullPage" FrameType="TitleBarOnly"
                AllowCustomization="false" AllowLayoutChange="false" AllowPersonalization="false">
                <ZoneTemplate>
                    <WebPartPages:XsltListViewWebPart runat="server" Description="Share a document with the team by adding it to this document library."
                        ListDisplayName="" PartOrder="0" HelpLink="" AllowRemove="True" IsVisible="True"
                        AllowHide="True" UseSQLDataSourcePaging="True" ExportControlledProperties="False"
                        IsIncludedFilter="" DataSourceID="" Title="Shared Documents" ViewFlag="8388621"
                        NoDefaultStyle="" AllowConnect="True" FrameState="Normal" CatalogIconImageUrl="/_layouts/images/itdl.png"
                        PageSize="-1" PartImageLarge="/_layouts/images/itdl.png" AsyncRefresh="False"
                        Dir="Default" DetailLink="/Shared Documents" ShowWithSampleData="False" ListId="6ce7f58c-bcc3-4ff3-be6e-c98032e22ea7"
                        ListName="{6CE7F58C-BCC3-4FF3-BE6E-C98032E22EA7}" FrameType="Default" PartImageSmall=""
                        IsIncluded="True" SuppressWebPartChrome="False" AllowEdit="True" ViewGuid="{EB1B4BDC-CFA1-4BC1-8DAB-1C3DE3486C92}"
                        AutoRefresh="False" AutoRefreshInterval="60" AllowMinimize="True" WebId="00000000-0000-0000-0000-000000000000"
                        ViewContentTypeId="0x" InitialAsyncDataFetch="False" GhostedXslLink="main.xsl"
                        MissingAssembly="Cannot import this Web Part." HelpMode="Modeless" ListUrl=""
                        ID="xlvwpSharedDocs" ConnectionID="00000000-0000-0000-0000-000000000000"
                        AllowZoneChange="True" TitleUrl="/Shared Documents" ManualRefresh="False" __MarkupType="vsattributemarkup"
                        __WebPartId="{EB1B4BDC-CFA1-4BC1-8DAB-1C3DE3486C92}" __AllowXSLTEditing="true"
                        __designer:CustomXsl="fldtypes_Ratings.xsl" WebPart="true" Height="" Width="">
                        <ParameterBindings>
			<ParameterBinding Name="dvt_sortdir" Location="Postback;Connection"/>
			<ParameterBinding Name="dvt_sortfield" Location="Postback;Connection"/>
			<ParameterBinding Name="dvt_startposition" Location="Postback" DefaultValue=""/>
			<ParameterBinding Name="dvt_firstrow" Location="Postback;Connection"/>
			<ParameterBinding Name="OpenMenuKeyAccessible" Location="Resource(wss,OpenMenuKeyAccessible)" />
			<ParameterBinding Name="open_menu" Location="Resource(wss,open_menu)" />
			<ParameterBinding Name="select_deselect_all" Location="Resource(wss,select_deselect_all)" />
			<ParameterBinding Name="idPresEnabled" Location="Resource(wss,idPresEnabled)" />
			<ParameterBinding Name="NoAnnouncements" Location="Resource(wss,noitemsinview_doclibrary)" />
			<ParameterBinding Name="NoAnnouncementsHowTo" Location="Resource(wss,noitemsinview_doclibrary_howto2)" />
                        </ParameterBindings>
                        <XmlDefinition>
			<View Name="{EB1B4BDC-CFA1-4BC1-8DAB-1C3DE3486C92}" MobileView="TRUE" Type="HTML" Hidden="TRUE" DisplayName="" Url="/SitePages/Test webparts.aspx" Level="1" BaseViewID="1" ContentTypeID="0x" ImageUrl="/_layouts/images/dlicon.png">
				<Query>
					<OrderBy>
						<FieldRef Name="FileLeafRef"/>
					</OrderBy>
				</Query>
				<ViewFields>
					<FieldRef Name="DocIcon"/>
					<FieldRef Name="LinkFilename"/>
					<FieldRef Name="Modified"/>
					<FieldRef Name="Editor"/>
				</ViewFields>
				<RowLimit Paged="TRUE">30</RowLimit>
				<Toolbar Type="Freeform"/>
			</View>
                        </XmlDefinition>
                        <DataFields>
                        </DataFields>
                    </WebPartPages:XsltListViewWebPart>
                </ZoneTemplate>
            </WebPartPages:WebPartZone>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" runat="server" ContentPlaceHolderID="PlaceHolderLeftNavBar">
</asp:Content>
