<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="LPWeb.Settings.StageTemplateSetup" CodeBehind="StageTemplateSetup.aspx.cs" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <title>Stage template setup</title>
    <script language="javascript" type="text/javascript">
        var tbxDaysFromEstCloseClientId = "#<%= tbxDaysFromEstClose.ClientID %>";
        var tbxDaysAfterCreationDateClientId = "#<%= tbxDaysAfterCreationDate.ClientID %>";
        var tbxSequenceClientId = "#<%= tbxSequence.ClientID %>";
        var tbxPointStageNameFieldClientId = "#<%= tbxPointStageNameField.ClientID %>";
        var tbxPointStageDateFieldClientId = "#<%= tbxPointStageDateField.ClientID %>";
        var ddlTypeClientId = "#<%= ddlType.ClientID %>";
        var hdnStageIDClientId = "#<%= hdnStageID.ClientID %>";
        var hdnMaxIDType1ClientId = "#<%= hdnMaxIDType1.ClientID %>";
        var hdnMaxIDType2ClientId = "#<%= hdnMaxIDType2.ClientID %>";

        $(document).ready(function () {

            $(tbxDaysFromEstCloseClientId).focusout(function () {
                if ($(tbxDaysFromEstCloseClientId).val() == "" || $(tbxDaysFromEstCloseClientId).val() == "0") {
                }
                else {
                    $(tbxDaysAfterCreationDateClientId).val("");
                }
            });

            $(tbxDaysAfterCreationDateClientId).focusout(function () {
                if ($(tbxDaysAfterCreationDateClientId).val() == "") {
                }
                else {
                    $(tbxDaysFromEstCloseClientId).val("");
                }
            });

            $(ddlTypeClientId).change(function () {
                if ($(hdnStageIDClientId).val() == "" || $(hdnStageIDClientId).val() == "0") {

                    if ($(ddlTypeClientId).val() == "") {
                        $(tbxSequenceClientId).val("0");
                    }
                    else if ($(ddlTypeClientId).val() == "Processing") {
                        $(tbxSequenceClientId).val($(hdnMaxIDType1ClientId).val());
                    }
                    else if ($(ddlTypeClientId).val() == "Prospect") {
                        $(tbxSequenceClientId).val($(hdnMaxIDType2ClientId).val());
                    }
                }
            });

            $(tbxDaysFromEstCloseClientId).OnlyInt();
            $(tbxDaysAfterCreationDateClientId).OnlyInt();
            $(tbxSequenceClientId).onlypressnum();
            $(tbxPointStageNameFieldClientId).onlypressnum();
            $(tbxPointStageDateFieldClientId).onlypressnum();


        })
        function CheckInput() {
            var tbxStageNameClientId = "#<%= tbxStageName.ClientID %>";
            if ($(tbxStageNameClientId).val() == "") {
                alert("Please enter the stage name.");
                return false;
            }

            if ($(ddlTypeClientId).val() == "") {
                alert("Please select a workflow type.");
                return false;
            }

            if ($(tbxSequenceClientId).val() == "" || $(tbxSequenceClientId).val() == "0") {
                alert("Please enter the sequence value.");
                return false;
            }

            if ($(tbxPointStageNameFieldClientId).val() != "" && ($(tbxPointStageNameFieldClientId).val() < 1 || $(tbxPointStageNameFieldClientId).val() > 30000)) {
                alert("A valid Point Field ID should be 1 - 30000.");
                return false;
            }

            if ($(tbxPointStageDateFieldClientId).val() != "" && ($(tbxPointStageDateFieldClientId).val() < 1 || $(tbxPointStageDateFieldClientId).val() > 30000)) {
                alert("A valid Point Field ID should be 1 - 30000.");
                return false;
            }

            return true;
        }

    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdnStageID" runat="server" />
    <asp:HiddenField ID="hdnMaxIDType1" runat="server" />
    <asp:HiddenField ID="hdnMaxIDType2" runat="server" />
    <div id="divContent" class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0" width="670px">
                <tr>
                    <td style="width:120px;padding-top: 9px;">
                        Stage Name:
                    </td>
                    <td style="padding-left: 15px;padding-top: 9px; width:300px;" colspan="2">
                        <asp:TextBox ID="tbxStageName" runat="server" Width="295px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="padding-left: 15px; padding-top:15px; text-align:right; width:220px;">
                        <asp:CheckBox ID="chkEnabled" runat="server" Text="    Enabled"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Displayed as:
                    </td>
                    <td style="padding-left: 15px; padding-top: 9px;" colspan="2">
                        <asp:TextBox ID="tbxAlias" runat="server" Width="295px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="text-align:right; padding-top: 9px;">
                        Workflow Type: &nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlType" runat="server" DataTextField="TypeName" DataValueField="TypeId" Width="90px" AutoPostBack="true" EnableViewState="true">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Sequence:
                    </td>
                    <td style="padding-left: 15px; padding-top: 9px; width:90px">
                        <asp:TextBox ID="tbxSequence" runat="server" Width="80px" MaxLength="2"></asp:TextBox>
                    </td>
                    <td style="padding-top: 9px; width:210px; text-align:right;">
                        Type:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tbxType" runat="server" Width="80px" Enabled="false"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Days From Est Close:
                    </td>
                    <td style="padding-left: 15px; padding-top: 9px;" colspan="2">
                        <asp:TextBox ID="tbxDaysFromEstClose" runat="server" Width="80px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td colspan="3" style="padding-top: 9px; text-align:right;">
                        Days After Creation Date:&nbsp;&nbsp;&nbsp;
                        <asp:TextBox ID="tbxDaysAfterCreationDate" runat="server" Width="80px" MaxLength="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Point Stage Name field:
                    </td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                        <asp:TextBox ID="tbxPointStageNameField" runat="server" Width="80px" MaxLength="5"></asp:TextBox>
                    </td>
                    <td style="padding-top: 9px; text-align:right;">
                        Point Stage Date field:&nbsp;&nbsp;&nbsp;<asp:TextBox ID="tbxPointStageDateField" runat="server" Width="80px" MaxLength="5"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
    </div>
    <div style="margin-top: 20px; padding-left:15px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClientClick="return CheckInput();" OnClick="btnSave_Click"/>
                </td>
                <td style="padding-left: 8px;">
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="window.parent.ClosePopupStage();" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>