<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGoals_GetData.aspx.cs" Inherits="LPWeb.AnyChart.UserGoals_GetData" ContentType="text/xml" %>
<anychart>
  <margin all="0"/>
  <settings>
    <animation enabled="true"/>
  </settings>
  <gauges>
    <gauge>
	  <label enabled="true">
        <position placement_mode="ByPoint" x="93" y="90" />
		<format>$<%=sHighRangeEnd%>k</format>
      </label>
      <chart_settings>
        <chart_background>
            <border enabled="True" thickness="1" type="solid" color="#e4e7ef" />
            <inside_margin bottom="0" />
            <corners type="Rounded" all="5" />
        </chart_background>
        <title padding="0">
          <text><%=sChartTitle%></text>
        </title>
      </chart_settings>
      <circular>
        <axis radius="40" start_angle="85" sweep_angle="190" size="2">
          <labels align="Outside" padding="1" show_last="false">
            <format>${%Value}{numDecimals:0}k</format>
          </labels>
		  <scale minimum="0" maximum="<%=sHighRangeEnd%>" major_interval="<%=sScaleInterval%>" />
          <scale_bar enabled="true">
            <fill color="silver" />
          </scale_bar>
          <major_tickmark enabled="true" align="Center" length="4" width="1" padding="0">
            <fill color="#292929" />
          </major_tickmark>
          <minor_tickmark enabled="false"/>
          <color_ranges>
            <color_range start="0" end="<%=sLowRangeEnd%>" align="Inside" start_size="60" end_size="60" padding="6" color="Red">
              <border enabled="true" color="Black" opacity="0.4"/>
              <label enabled="true" align="Inside" padding="34">
                <format>Low</format>
                <position valign="Center" halign="Center"/>
                <font bold="false" size="11"/>
              </label>
              <fill opacity="0.6"/>
            </color_range>
            <color_range start="<%=sLowRangeEnd%>" end="<%=sMediumRangeEnd%>" align="Inside" start_size="60" end_size="60" padding="6" color="Yellow">
              <border enabled="true" color="Black" opacity="0.4"/>
              <label enabled="true" align="Inside" padding="34">
                <format>Medium</format>
                <position valign="Center" halign="Center"/>
                <font bold="false" size="11"/>
              </label>
              <fill opacity="0.6"/>
            </color_range>
            <color_range start="<%=sMediumRangeEnd%>" end="<%=sHighRangeEnd%>" align="Inside" start_size="60" end_size="60" padding="6" color="Green">
              <border enabled="true" color="Black" opacity="0.4"/>
              <label enabled="true" align="Inside" padding="34">
                <format>High</format>
                <position valign="Center" halign="Center"/>
                <font bold="false" size="11"/>
              </label>
              <fill opacity="0.6"/>
            </color_range>
          </color_ranges>
        </axis>
        <frame enabled="false">
        </frame>
        <pointers>
          <pointer value="<%=sRealAmount%>">
            <label enabled="true">
              <position placement_mode="ByPoint" x="50" y="4" />
              <format>Amount: ${%Value}{numDecimals:0}k</format>
              <background enabled="false" />
			  <font family="Arial" />
            </label>
            <needle_pointer_style thickness="7" point_thickness="5" point_radius="3">
              <fill color="Rgb(230,230,230)"/>
              <border color="Black" opacity="0.7"/>
              <effects enabled="true">
                <bevel enabled="true" distance="2" shadow_opacity="0.6" highlight_opacity="0.6"/>
                <drop_shadow enabled="true" distance="1" blur_x="1" blur_y="1" opacity="0.4"/>
              </effects>
              <cap>
                <background>
                  <fill type="Gradient">
                    <gradient type="Linear" angle="45">
                      <key color="#D3D3D3"/>
                      <key color="#6F6F6F"/>
                    </gradient>
                  </fill>
                  <border color="Black" opacity="0.9"/>
                </background>
                <effects enabled="true">
                  <bevel enabled="true" distance="2" shadow_opacity="0.6" highlight_opacity="0.6"/>
                  <drop_shadow enabled="true" distance="1.5" blur_x="2" blur_y="2" opacity="0.4"/>
                </effects>
              </cap>
            </needle_pointer_style>
            <animation enabled="true" start_time="0" duration="0.5" interpolation_type="Bounce"/>
          </pointer>
        </pointers>
      </circular>
    </gauge>
  </gauges>
</anychart>