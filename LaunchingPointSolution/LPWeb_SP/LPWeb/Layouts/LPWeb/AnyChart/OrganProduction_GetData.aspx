<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrganProduction_GetData.aspx.cs" Inherits="LPWeb.AnyChart.OrganProduction_GetData" ContentType="text/xml" %>

<anychart>
<margin all="0" />
<settings>
    <animation enabled="True"/>
</settings>
<charts>
<chart plot_type="CategorizedVertical">
	<data_plot_settings enable_3d_mode="true">
		<bar_series shape_type="Cylinder">
			<tooltip_settings enabled="True">
				<format>{%Value}{numDecimals:0}k</format>
			</tooltip_settings>
		</bar_series>
	</data_plot_settings>
	
	<data palette="default">
		<%= this.sOrganPoints %>
    </data>
	<chart_settings>
		<title enabled="true">
          <text>Organizational Production</text>
        </title>
		<axes>
			<y_axis>
				<title enabled="false" />
				<labels>
					<format>${%Value}{numDecimals:0}k</format>
				</labels>				
				<scale mode="Overlay" maximum="<%= this.iOverlay %>"/>				
			</y_axis>
			<x_axis>
				<title enabled="false" />
                <labels rotation="90" />
			</x_axis>
		</axes>
        <chart_background>
            <border enabled="True" thickness="1" type="solid" color="#e4e7ef" />
            <corners type="Rounded" all="5" />
        </chart_background>
	</chart_settings>	
</chart>
</charts>
</anychart>
