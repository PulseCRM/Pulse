<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesBreakdown_GetData.aspx.cs" Inherits="LPWeb.AnyChart.SalesBreakdown_GetData" ContentType="text/xml" %>

<anychart>
  <margin all="0" />
  <settings>
    <animation enabled="false"/>
  </settings>
  <charts>
    <chart plot_type="Pie">
      <data_plot_settings enable_3d_mode="true">
        <pie_series>
          <tooltip_settings enabled="true">
            <format>{%Name}: ${%Value}{numDecimals:0}k | {%YPercentOfSeries}%</format>
            <font family="Arial" />
          </tooltip_settings>
          <label_settings enabled="true">
            <background enabled="false"/>
            <position anchor="Center" valign="Center" halign="Center" padding="20"/>
            <font color="White" family="Arial">
              <effects>
                <drop_shadow enabled="true" distance="2" opacity="0.6" blur_x="2" blur_y="2"/>
              </effects>
            </font>
            <format>${%YValue}{numDecimals:0}k</format>
          </label_settings>
        </pie_series>
      </data_plot_settings>
      <data>
        <series name="Series 1" type="Pie">
          <%= this.sPiePointText %>
        </series>
      </data>
      <chart_settings>
        <title enabled="true" padding="15">
          <text>Sales Breakdown</text>
          <font family="Arial" />
        </title>
        <chart_background>
            <border enabled="True" thickness="1" type="solid" color="#e4e7ef" />
            <corners type="Rounded" all="5" />
        </chart_background>
        <legend enabled="true" position="Bottom" align="Left" ignore_auto_item="true" rows_padding="0">
          <format>{%Icon} {%Name}</format>
          <font family="Arial" />
          <title enabled="false" />
          <columns_separator enabled="false" />
          <rows_separator enabled="false" />
          <icon height="6" width="15"></icon>
          <items>
            <item source="Points"/>
          </items>
        </legend>
      </chart_settings>
    </chart>
  </charts>
</anychart>