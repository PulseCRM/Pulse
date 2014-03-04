<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" AutoEventWireup="true" Inherits="LPWeb.Settings.WorkflowTaskTemplateSetup"
    CodeBehind="WorkflowTaskTemplateSetup.aspx.cs" %>

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
    <title>Workflow task template setup</title>
    <script language="javascript" type="text/javascript">
        var tbxDueDaysByDateClientId = "#<%= tbxDueDaysByDate.ClientID %>";
        var tbxDueDaysByTaskClientId = "#<%= tbxDueDaysByTask.ClientID %>";
        var tbxDaysDueAfterCreationDateClientId = "#<%= tbxDaysDueAfterCreationDate.ClientID %>";
        var ddlPrerequisiteTaskClientId = "#<%= ddlPrerequisiteTask.ClientID %>";
        var hdnIsReferencedClientId = "#<%= hdnIsReferenced.ClientID %>";
        var hdnTaskIDClientId = "#<%= hdnTaskID.ClientID %>";
        var hdnCustomTemplateClientId = "#<%= hdnCustomTemplate.ClientID %>";

        $(document).ready(function () {

            if ($(hdnCustomTemplateClientId).val() == "False") {
                $("#Button2").attr("disabled", "disabled");
            }
            $(tbxDueDaysByDateClientId).focusout(function () {
                if ($(tbxDueDaysByDateClientId).val() == "" || $(tbxDueDaysByDateClientId).val() == "0") {
                }
                else {
                    $(tbxDueDaysByTaskClientId).val("");
                    $(ddlPrerequisiteTaskClientId).val(0);
                }
            });
            $(tbxDaysDueAfterCreationDateClientId).focusout(function () {
                if ($(tbxDaysDueAfterCreationDateClientId).val() == "" || $(tbxDaysDueAfterCreationDateClientId).val() == "0") {
                }
                else {
                    $(tbxDueDaysByTaskClientId).val("");
                    $(ddlPrerequisiteTaskClientId).val(0);
                }
            });

            $(tbxDueDaysByTaskClientId).focusout(function () {
                if ($(tbxDueDaysByTaskClientId).val() == "") {
                }
                else {
                    $(tbxDueDaysByDateClientId).val("");
                    $(tbxDaysDueAfterCreationDateClientId).val("");
                }
            });
            $(ddlPrerequisiteTaskClientId).change(function () {
                if ($(ddlPrerequisiteTaskClientId).val() == "") {
                }
                else {
                    $(tbxDueDaysByDateClientId).val("");
                    $(tbxDaysDueAfterCreationDateClientId).val("");
                }
            });

            $(tbxDueDaysByDateClientId).OnlyInt();
            $(tbxDueDaysByTaskClientId).OnlyInt();
            $(tbxDaysDueAfterCreationDateClientId).OnlyInt();
        })
        function CheckInput() {
            var tbxTaskNameClientId = "#<%= tbxTaskName.ClientID %>";
            if ($(tbxTaskNameClientId).val() == "") {
                alert("Please enter the task name.");
                return false;
            }

            var ddlStageClientId = "#<%= ddlStage.ClientID %>";
            if ($(ddlStageClientId).val() == "") {
                alert("Please select a stage.");
                return false;
            }
            return true;
        }

        function DeleteTask() {

            if ($(hdnIsReferencedClientId).val() == "true") {
                return confrim("The Workflow Task has been referenced by loan tasks. Deleting this Workflow Task will remove the references and is not reversible. Are you sure you want to continue?");

            }
            else {
                return confirm('Are you sure you want to delete the task template?');
            }
        }

        // show popup for enter Branch name
        function EnterTaskName() {
            // clear Template name
            $("#txbTaskName1").val("")

            // show modal
            $("#divNewTaskName").dialog({
                height: 160,
                width: 390,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
            $("body>div[role=dialog]").appendTo("#form1");

        }
        function ClosePopupTaskName() {

            // close modal
            $("#divNewTaskName").dialog("close");
        }

        function BeforeCloneTaskName() {

            var tbxTaskNameClientId = "#<%= tbxTaskName.ClientID %>";
            var txbTaskName1ClientId = "#<%= txbTaskName1.ClientID %>";
            $(tbxTaskNameClientId).val($(txbTaskName1ClientId).val());
            $(hdnTaskIDClientId).val("0");
            $("#divNewTaskName").dialog("close");
            // close modal

        }
        function btnPreviewWarningEmail_OnClick() {

            var ddlWarningEmailClientId = "#<%= ddlWarningEmail.ClientID%>";
            if ($(ddlWarningEmailClientId).val() == "" || $(ddlWarningEmailClientId).val() == "0") {
                alert("Please select a email template.");
                return false;
            }


            var EmailTemplateID = $(ddlWarningEmailClientId).val()

            OpenWindow("EmailTemplatePreview.aspx?id=" + EmailTemplateID, "_PreviewEmailTemplate4", 760, 700, "no", "center");

            return true;


        }
        function btnPreviewOverdueEmail_OnClick() {

            var ddlOverdueEmailClientId = "#<%= ddlOverdueEmail.ClientID%>";
            if ($(ddlOverdueEmailClientId).val() == "" || $(ddlOverdueEmailClientId).val() == "0") {
                alert("Please select a email template.");
                return false;
            }

            var EmailTemplateID = $(ddlOverdueEmailClientId).val()

            OpenWindow("EmailTemplatePreview.aspx?id=" + EmailTemplateID, "_PreviewEmailTemplate4", 760, 700, "no", "center");

            return true;


        }
        function btnPreviewCompletionEmail_OnClick() {

            if ($(ddlCompletionEmailClientId).val() == "" || $(ddlCompletionEmailClientId).val() == "0") {
                alert("Please select a email template.");
                return false;
            }

            var EmailTemplateID = $(ddlCompletionEmailClientId).val()

            OpenWindow("EmailTemplatePreview.aspx?id=" + EmailTemplateID, "_PreviewEmailTemplate4", 760, 700, "no", "center");

            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="hdnIsDependTask" runat="server" />
    <asp:HiddenField ID="hdnIsReferenced" runat="server" />
    <asp:HiddenField ID="hdnTaskID" runat="server" />
    <asp:HiddenField ID="hdnCustomTemplate" runat="server" />
    <div style="border: solid 0px red; height: 448px; overflow: auto;">
    <div id="divNewTaskName" title="Enter Workflow Template Name" style="display: none; margin: 10px;">
        <table>
            <tr>
                <td style="width: 180px;">
                    Task Name:
                </td>
                <td>
                    <asp:TextBox ID="txbTaskName1" runat="server" MaxLength="255" Style="width: 200px;"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div style="text-align: center;">
            <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="Btn-66" OnClientClick="BeforeCloneTaskName()"
                OnClick="btnCreate_Click" />
            &nbsp;&nbsp;
            <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="ClosePopupTaskName()" />
        </div>
        <div id="divMsg" style="margin-top: 10px; text-align: center; display: none;">
            <img id="imgWaiting" src='../images/waiting.gif' style='position: relative; top: 2px;' />
            <label id="lbMsg" style='font-weight: bold;'>Please wait...</label>
        </div>
    </div>
    <div id="divContent" style="margin-top: 0px;">
        <div>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Workflow Template:
                    </td>
                    <td style="width: 310px;">
                        <asp:TextBox ID="tbxTemplateName" runat="server" Width="290px" ReadOnly="true" Enabled="false"></asp:TextBox>
                    </td>
                    <td style="width: 40px;">
                        Stage:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStage" runat="server" DataTextField="Name" DataValueField="WflStageId"
                            Width="230px" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged" AutoPostBack="true"
                            EnableViewState="true">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Task Name:
                    </td>
                    <td style="width: 310px;">
                        <asp:TextBox ID="tbxTaskName" runat="server" Width="290px" MaxLength="255"></asp:TextBox>
                    </td>
                    <td style="width: 80px;">
                        <asp:CheckBox ID="chkEnable" runat="server" Text="Enabled"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkExternalViewing" runat="server" Text="External Viewing" Checked="true"></asp:CheckBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Description:
                    </td>
                    <td style="width: 310px;">
                        <asp:TextBox ID="tbxDescription" runat="server" Width="576px" TextMode="MultiLine"
                            Height="30px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Default Owner:
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList ID="ddlOwner" runat="server" DataTextField="RoleName" DataValueField="RoleId"
                            Width="290px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 80px;">
                        Days due from Est Close Date:
                    </td>
                    <td style="width: 60px;">
                        <asp:TextBox ID="tbxDueDaysByDate" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td style="width: 80px;">
                        Days due After Creation Date:
                    </td>
                    <td>
                        <asp:TextBox ID="tbxDaysDueAfterCreationDate" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Prerequisite Task:
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList ID="ddlPrerequisiteTask" runat="server" DataTextField="TaskName"
                            DataValueField="TaskId" Width="290px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 228px;">
                        Days due after Prerequisite Task Completed:
                    </td>
                    <td>
                        <asp:TextBox ID="tbxDueDaysByTask" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Warning Email:
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList ID="ddlWarningEmail" runat="server" DataTextField="Name" DataValueField="TemplEmailId"
                            Width="290px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input id="btnPreviewWarningEmail" type="button" value="Preview" class="Btn-66" onclick="btnPreviewWarningEmail_OnClick()" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Overdue Email:
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList ID="ddlOverdueEmail" runat="server" DataTextField="Name" DataValueField="TemplEmailId"
                            Width="290px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input id="btnPreviewOverdueEmail" type="button" value="Preview" class="Btn-66" onclick="btnPreviewOverdueEmail_OnClick()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divCompletionEmailList1" class="ColorGrid" style="width: 550px; margin-top: 10px; display: normal;">
        <div>
            <table id="gridCompletionEmailList1" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                <tr>
                  <th class="CheckBoxHeader" scope="col"><input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" /></th>
                  <th scope="col">Email Template</th>
                    <th scope="col">Enabled</th>
                    <th scope="col"></th>
                </tr>
                <tr>
                    <td class="CheckBoxColumn">
                        <input id="chkChecked" type="checkbox" />
                    </td>
                    <td>
                        <select id="Select1" style="width: 350px;">
                        </select>
                    </td>
                    <td style="width: 50px; text-align: center;">
                        <input id="chkStageEnabled" type="checkbox" checked />
                    </td>
                    <td style="width: 50px; text-align: center">
                        <a id="aTaskCount" href="javascript:aUpdateStage_onclick()" style="text-decoration: underline;">Preview</a>
                    </td>
                </tr>
                
            </table>
        </div>
        <div class="GridPaddingBottom">&nbsp;</div>
    </div>
    <div style="margin-top: 10px; padding-left:15px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClientClick="return CheckInput()"
                        OnClick="btnSave_Click" />
                </td>
                <td style="padding-left: 8px;">
                    <input id="Button2" type="button" value="Clone" class="Btn-66" onclick="EnterTaskName()"/>
                </td>
                <td style="padding-left: 8px;">
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" class="Btn-66" OnClientClick="return DeleteTask()"
                        OnClick="btnDelete_Click" />
                </td>
                <td style="padding-left: 8px;">
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="window.parent.ClosePopupTask();parent.window.location.href=parent.window.location.href;" />
                </td>
            </tr>
        </table>
    </div>
    </div>
    </form>
</body>
</html>
