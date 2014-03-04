<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailTemplateAttachmentAdd.aspx.cs" Inherits="Settings_EmailTemplateAttachmentAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Add Email Attachment</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            //alert($.browser.version);

            // add event
            $("#FileUpload1").change(FileUpload1_onchange);

        });

        function FileUpload1_onchange() {

            var FilePath = $("#FileUpload1").val();
            var FileName = FilePath.substring(FilePath.lastIndexOf('\\') + 1);

            var IsValid = ValidateImageExt(FileName);
            if (IsValid == false) {

                alert("Please select a valid file.");

                $("#FileUpload1").val("");
                $("#FileUpload1").replaceWith("<input type='file' name='FileUpload1' id='FileUpload1' style='width:330px;' />")
                $("#FileUpload1").change(FileUpload1_onchange);

                $("#txtAttchName").val("")
                $("#ddlFileType").val("--")

                return;
            }

            var Ext = FileName.substr(FileName.lastIndexOf('.') + 1);
            //alert(Ext);
            var FileNameNoExt = FileName.replace("." + Ext, "");
            //alert(FileNameNoExt);

            $("#txtAttchName").val(FileNameNoExt);

            Ext = Ext.toUpperCase();
            if (Ext == "DOC" || Ext == "DOCX") {

                $("#ddlFileType").val("Word");
            }
            else if (Ext == "XLS" || Ext == "XLSX") {

                $("#ddlFileType").val("XLS");
            }
            else if (Ext == "JPG" || Ext == "JPGE") {

                $("#ddlFileType").val("JPEG");
            }
            else {

                $("#ddlFileType").val(Ext.toUpperCase());
            }
        }

        function ValidateImageExt(sImagePath) {

            var ext = sImagePath.substr(sImagePath.lastIndexOf('.') + 1);
            //alert(ext);

            var AllowExt = "gif|jpg|jpeg|pdf|png|doc|docx|xls|xlsx|zip";
            var ExtOK = false;
            var ArrayExt;
            if (AllowExt.indexOf('|') != -1) {
                ArrayExt = AllowExt.split('|');
                for (i = 0; i < ArrayExt.length; i++) {
                    if (ext.toLowerCase() == ArrayExt[i]) {
                        ExtOK = true;
                        break;
                    }
                }
            }
            else {
                ArrayExt = AllowExt;
                if (ext.toLowerCase() == ArrayExt) {
                    ExtOK = true;
                }
            }
            return ExtOK;
        }

        function BeforeSave() {

            var FilePath = $("#FileUpload1").val();
            if (FilePath == "") {

                alert("Please select target File.");
                return false;
            }

            var AttachName = $.trim($("#txtAttchName").val());
            if (AttachName == "") {

                alert("Please enter Attachment Name.");
                return false;
            }



            return true;
        }

        function btnCancel_onclick() {

            window.top.CloseDialog();
        }

// ]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        
        <table cellpadding="3" cellspacing="3">
            <tr>
                <td style="text-align: right;">Email Template:</td>
                <td>
                    <asp:DropDownList ID="ddlEmailTemplate" runat="server" DataValueField="TemplEmailId" DataTextField="Name" Width="260px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">Attachment Name:</td>
                <td>
                    <asp:TextBox ID="txtAttchName" runat="server" Width="255px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">File:</td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" Width="330px" />
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">Type:</td>
                <td>
                    <asp:DropDownList ID="ddlFileType" runat="server" Width="150px">
                        <asp:ListItem>--</asp:ListItem>
                        <asp:ListItem>GIF</asp:ListItem>
                        <asp:ListItem>JPEG</asp:ListItem>
                        <asp:ListItem>PDF</asp:ListItem>
                        <asp:ListItem>PNG</asp:ListItem>
                        <asp:ListItem>Word</asp:ListItem>
                        <asp:ListItem>XLS</asp:ListItem>
                        <asp:ListItem>ZIP</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        
        <div style="margin-top: 20px;">
            <table>
                <tr>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave();" OnClick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnSaveAnother" runat="server" Text="Save and Add Another" CssClass="Btn-140" OnClientClick="return BeforeSave();" OnClick="btnSaveAnother_Click" />
                    </td>
                    <td>
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                    </td>
                </tr>
            </table>
        </div>

    </div>
    </form>

</body>
</html>