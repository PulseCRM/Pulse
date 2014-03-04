<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSkinEdit.aspx.cs" Inherits="Settings_EmailSkinEdit" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.cleditor.js" type="text/javascript"></script>
    
    <script language="javascript" type="text/javascript">

        var HtmlEditor;

        $(document).ready(function () {

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtDesc").maxlength(500);

            InitHtmlEditor();
        });

        // init cleditor
        function InitHtmlEditor() {

            if (HtmlEditor == undefined) {

                HtmlEditor = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHtmlBody").cleditor({ width: 805, height: 420, bodyStyle: "font:11px Arial", docType: '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">' });
                HtmlEditor[0].focus();
            }
        }

        function BeforeSave() {

            var EmailSkinName = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_txtEmailSkinName").val());
            if (EmailSkinName == "") {

                alert("Please enter Email Skin Name.");
                return false;
            }

            return true;
        }

        function chkDefault_onclick(checkbox) {

            if (checkbox.checked == true) {

                var RadomNum = Math.random();
                var sid = RadomNum.toString().substr(2);
                $.getJSON("EmailSkinCheckDefaultAjax.aspx?sid=" + sid, function (data) {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        checkbox.checked = false;
                        return;
                    }

                    if (data.hasDefault == "true") {

                        var result = confirm("Another Email Skin has been set as the default Email Skin. You can have only one default Email Skin in Pulse. \r\nDo you want to make this one the default Email Skin instead?");
                        if (result == false) {

                            checkbox.checked = false;
                        }
                    }
                });
            }
        }

        function aInsert_onclick() {

            HtmlEditor[0].focus();
            HtmlEditor[0].execCommand("inserthtml", "<@EmailBody@>", null, null);
        }

        function btnCreate_onclick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            window.location.href = "EmailSkinAdd.aspx?sid=" + sid;
        }

        function BeforeDelete() {

            var result = confirm("Deleting the Email Skin may affect the emails that have been associated with this email skin. \r\nAre you sure you want to delete the selected Email Skin?");
            if (result == false) {

                return false;
            }

            return true;
        }

        function chkEnabled_onclick(checkbox) {

            if (checkbox.checked == false) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_chkDefault").removeAttr("checked");
            }
        }
    
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">

    <div id="divEmailSkinContainer" style="margin-top:10px; height:700px;">
		
		<div style="margin-top:10px;">
			<input id="btnGoBack" type="button" value="Back to Email Skin List" onclick="javascript:window.location.href='EmailSkinList.aspx'" class="Btn-140" />
		</div>
		
        <table style="margin-top:10px;">
            <tr>
                <td style="width:90px;">Email Skin Name:</td>
                <td style="width:530px;">
                    <asp:TextBox ID="txtEmailSkinName" runat="server" Width="450" MaxLength="255"></asp:TextBox>
                </td>
                <td>
                    <asp:CheckBox ID="chkEnabled" runat="server" Text=" Enabled" Checked="true" onclick="chkEnabled_onclick(this)" />
                </td>
                <td style="padding-left:30px;">
                    <asp:CheckBox ID="chkDefault" runat="server" Text=" Default" onclick="chkDefault_onclick(this)" />
                </td>
            </tr>
        </table>
        
        <table>
            <tr>
                <td style="width:90px;">Description:</td>
                <td>
                    <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Width="700" Height="100"></asp:TextBox>
                </td>
            </tr>
        </table>
        
        

        <table style="margin-top:10px;">
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return BeforeSave()" OnClick="btnSave_Click" CssClass="Btn-66" />
                </td>
                <td>
                	<input id="btnCreate" type="button" value="Create" onclick="btnCreate_onclick()" class="Btn-66" />
                </td>
                <td>
                	<asp:Button ID="btnClone" runat="server" Text="Clone" OnClick="btnClone_Click" CssClass="Btn-66" />
                </td>
                <td>
                	<asp:Button ID="btnDelete" runat="server" Text="Delete" OnClientClick="return BeforeDelete()" OnClick="btnDelete_Click" CssClass="Btn-66" />
                </td>
            </tr>
        </table>
        
        <div style="margin-top:20px;">
        
        	<ul class="ToolStrip" style="margin-left:0px;font-weight:bold;">
        		<li><a id="aInsert" href="javascript:aInsert_onclick()">Insert Email Body Tag</a></li>
            </ul>
        	<asp:TextBox ID="txtHtmlBody" runat="server" TextMode="MultiLine"></asp:TextBox>
        
        </div>
    
    </div>

</asp:Content>

