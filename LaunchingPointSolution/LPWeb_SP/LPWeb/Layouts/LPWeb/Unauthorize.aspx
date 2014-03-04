<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Title="You have no privilege to access this page." Language="C#" AutoEventWireup="true" CodeBehind="Unauthorize.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Unauthorize" MasterPageFile="~/_layouts/LPWeb/MasterPage/Home.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" Runat="Server">
    <div>
        <div style="width: 561px; height: 164px; background: url('images/unauthorize.png') no-repeat left top; margin-left: auto; margin-right: auto; margin-top: 100px;">
            <div style="margin-left: 180px;">
                <div style="padding-top: 55px;">
                    <label id="lbMsg" runat="server" style="font-size: 12px; font-weight: bold;"></label>
                </div>
                <div style="margin-top: 5px;">
                    <a id="lnkBack" runat="server">[Go Back]</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
