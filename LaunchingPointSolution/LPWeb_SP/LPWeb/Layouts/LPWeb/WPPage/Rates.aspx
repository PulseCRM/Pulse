<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rates.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.Rates"
    DynamicMasterPageFile="~masterurl/default.master" %>--%>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" CodeBehind="Rates.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.WPPage.Rates,LPWeb,Version=1.0.0.0,Culture=neutral,PublicKeyToken=a2c3274f2ef313f2"
    AutoEventWireup="true" meta:webpartpageexpansion="full" meta:progid="SharePoint.WebPartPage.Document"
    MasterPageFile="~/_layouts/LPWeb/MasterPage/Super.master" %>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="PlaceHolderMain">
    <div id="PageContent">
        <div>
            <WebPartPages:WebPartZone runat="server" Title="loc:FullPage" ID="FullPage" FrameType="TitleBarOnly"
                AllowCustomization="false" AllowLayoutChange="false" AllowPersonalization="false">
                <ZoneTemplate>
                    <WebPartPages:XsltListViewWebPart runat="server" Description="" ListDisplayName=""
                        PartOrder="2" HelpLink="" AllowRemove="True" IsVisible="True" AllowHide="True"
                        UseSQLDataSourcePaging="True" ExportControlledProperties="False" IsIncludedFilter=""
                        DataSourceID="" Title="Rate Sheets" ViewFlag="8388621" NoDefaultStyle="" AllowConnect="True"
                        FrameState="Normal" CatalogIconImageUrl="/_layouts/images/itdl.png" PageSize="-1"
                        PartImageLarge="/_layouts/images/itdl.png" AsyncRefresh="False" Dir="Default"
                        DetailLink="/Rate Sheets" ShowWithSampleData="False" ListId="323e589c-df69-4fa2-ad91-854bb0759ae3"
                        ListName="{323E589C-DF69-4FA2-AD91-854BB0759AE3}" FrameType="Default" PartImageSmall=""
                        IsIncluded="True" SuppressWebPartChrome="False" AllowEdit="True" ViewGuid="{ADF26022-EDED-4F16-9098-B2C480649BDC}"
                        AutoRefresh="False" AutoRefreshInterval="60" AllowMinimize="True" WebId="00000000-0000-0000-0000-000000000000"
                        ViewContentTypeId="0x" InitialAsyncDataFetch="False" GhostedXslLink="main.xsl"
                        MissingAssembly="Cannot import this Web Part." HelpMode="Modeless" ListUrl=""
                        ID="xlvwpRates" ConnectionID="00000000-0000-0000-0000-000000000000" AllowZoneChange="True"
                        TitleUrl="/Rate Sheets" ManualRefresh="False" __MarkupType="vsattributemarkup"
                        __WebPartId="{ADF26022-EDED-4F16-9098-B2C480649BDC}" __AllowXSLTEditing="true"
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
			<View Name="{ADF26022-EDED-4F16-9098-B2C480649BDC}" MobileView="TRUE" Type="HTML" Hidden="TRUE" DisplayName="" Url="/SitePages/Test webparts.aspx" Level="1" BaseViewID="1" ContentTypeID="0x" ImageUrl="/_layouts/images/dlicon.png">
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
