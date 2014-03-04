<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailTemplateAddParent.aspx.cs" Inherits="Settings_EmailTemplateAddParent" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            var sid = GetQueryString1("sid");

            var iFrameSrc = "EmailTemplateAdd.aspx?sid=" + sid;

            $("#ifrEmailTemplateSetup").attr("src", iFrameSrc);
        });

        //#region show/close waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog() {

            $.unblockUI();
        }

        //#endregion
    
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    
    <br />
    <div>
        <iframe id="ifrEmailTemplateSetup" frameborder="0" scrolling="no" width="1000px" height="1060px">
        </iframe>
    </div>
    
    <div id="divWaiting" style="display: none; padding: 5px;">
	    <table style="margin-left: auto; margin-right: auto;">
		    <tr>
			    <td>
				    <img id="imgWaiting" src="../images/waiting.gif" />
			    </td>
			    <td style="padding-left: 5px;">
				    <label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
			    </td>
		    </tr>
	    </table>
    </div>

</asp:Content>