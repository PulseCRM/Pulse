<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowStageSetup2.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.WorkflowStageSetup2" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var sid = GetQueryString1("sid");
            var WflStageID = GetQueryString1("WflStageID");
            var WorkflowTemplateID = GetQueryString1("WorkflowTemplateID");
            var from = GetQueryString1("from");

            var iFrameSrc = "WorkflowStageSetup.aspx?sid=" + sid + "&WflStageID=" + WflStageID + "&WorkflowTemplateID=" + WorkflowTemplateID + "&from=" + encodeURIComponent(from);

            $("#ifrWflStageSetup").attr("src", iFrameSrc);
        });
    
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    
    <div>
        <iframe id="ifrWflStageSetup" frameborder="0" scrolling="no" width="1000px" height="700px">
        </iframe>
    </div>

</asp:Content>