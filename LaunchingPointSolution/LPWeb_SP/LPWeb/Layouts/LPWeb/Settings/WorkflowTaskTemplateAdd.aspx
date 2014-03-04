<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowTaskTemplateAdd.aspx.cs" Inherits="Settings_WorkflowTaskTemplateAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
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
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#ddlStage").change(ddlStage_onchange);

            $("#tbxDescription").maxlength(500);

            $("#tbxDueDaysByDate").OnlyInt();
            $("#tbxDueDaysByTask").OnlyInt();
            $("#tbxDaysDueAfterCreationDate").OnlyInt();
            $("#txtSequence").OnlyInt();

            $("#tbxDueDaysByDate").focusout(function () {
                if ($("#tbxDueDaysByDate").val() == "" || $("#tbxDueDaysByDate").val() == "0") {
                }
                else {
                    $("#tbxDueDaysByTask").val("");
                    $("#ddlPrerequisiteTask").val(0);
                }
            });
            $("#tbxDaysDueAfterCreationDate").focusout(function () {
                if ($("#tbxDaysDueAfterCreationDate").val() == "" || $("#tbxDaysDueAfterCreationDate").val() == "0") {
                }
                else {
                    $("#tbxDueDaysByTask").val("");
                    $("#ddlPrerequisiteTask").val(0);
                }
            });
            $("#tbxDueDaysByTask").focusout(function () {
                if ($("#tbxDueDaysByTask").val() == "") {
                }
                else {
                    $("#tbxDueDaysByDate").val("");
                    $("#tbxDaysDueAfterCreationDate").val("");
                }
            });
            $("#ddlPrerequisiteTask").change(function () {
                if ($("#ddlPrerequisiteTask").val() == "") {
                }
                else {
                    $("#tbxDueDaysByDate").val("");
                    $("#tbxDaysDueAfterCreationDate").val("");
                }
            });

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflStageSetup").height(xx);
            }
        });

        function aAddEmail_onclick() {

            var TrCount = $("#gridCompletionEmailList tr").length;

            // add th
            if (TrCount == 1) {

                // clear tr
                $("#gridCompletionEmailList").empty();

                // add th
                $("#gridCompletionEmailList").append($("#gridCompletionEmailList1 tr").eq(0).clone());
            }

            // clone tr
            var TrCopy = $("#gridCompletionEmailList1 tr").eq(1).clone(true);

            //#region Add Tr

            // next index
            var NowIndex = new Number($("#hdnCounter").val());
            var NextIndex = NowIndex + 1;

            // ddlCmpltEmailTmplt
            var ddlCmpltEmailTmplt_NewID = "ddlCmpltEmailTmplt" + NextIndex;
            var ddlCmpltEmailTmpltTemp = $.trim($("#hdnCmpltEmailTmpltTemp").val());
            //alert(ddlCmpltEmailTmpltTemp);
            var ddlCmpltEmailTmpltCode = ddlCmpltEmailTmpltTemp.replace(/CmpltEmailTmpltText/g, ddlCmpltEmailTmplt_NewID);
            //alert(ddlCmpltEmailTmpltCode);
            TrCopy.find("#ddlCmpltEmailTmplt").replaceWith(ddlCmpltEmailTmpltCode);
            TrCopy.find("#" + ddlCmpltEmailTmplt_NewID).change(ddlCmpltEmailTmplt_onchange);

            // aPreview
            var SelEmailTemplateID = TrCopy.find("#" + ddlCmpltEmailTmplt_NewID).val();
            //alert(SelEmailTemplateID);
            TrCopy.find("#aPreview").attr("href", "javascript: PreviewCompletionEmail(" + SelEmailTemplateID + ");");

            // append tr
            $("#gridCompletionEmailList").append(TrCopy);

            // set counter
            $("#hdnCounter").val(NextIndex);

            //#endregion

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflStageSetup").height(xx);
            }
        }

        function aRemoveEmail_onclick() {

            var SelectedCount = $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No completion email template was selected.");
                return;
            }

            var Result = confirm("This will delete the selected email template(s) for this workflow task. Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if (SelectedCount == $("#gridCompletionEmailList tr:not(:first)").length) {

                $("#gridCompletionEmailList").empty();
                $("#gridCompletionEmailList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no completion email template, please add.</td></tr>");

            }
            else {

                $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']:checked").parent().parent().remove();
            }

            // reset sequence
            //            ResetSeq();

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflStageSetup").height(xx);
            }
            else {

                $(window.parent.document).find("#ifrWflStageSetup").height(700);
            }
        }

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']").attr("checked", "true");
            }
            else {

                $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']").attr("checked", "");
            }
        }

        function aEnable_onclick() {

            var SelectedCount = $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No completion email template was selected.");
                return;
            }

            $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']:checked").parent().parent().find("#chkEnabled").attr("checked", "true");
        }

        function aDisable_onclick() {

            var SelectedCount = $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No completion email template was selected.");
                return;
            }

            $("#gridCompletionEmailList tr:not(:first) td :checkbox[id='chkChecked']:checked").parent().parent().find("#chkEnabled").attr("checked", "");
        }

        function ddlCmpltEmailTmplt_onchange() {

            //alert($(this).val());

            var SelEmailTemplateID = $(this).val();
            $(this).parent().parent().find("#aPreview").attr("href", "javascript: PreviewCompletionEmail(" + SelEmailTemplateID + ");");
        }

        function btnPreviewWarningEmail_onclick() {

            var EmailTemplateID = $("#ddlWarningEmail").val();
            //alert(EmailTemplateID);

            if (EmailTemplateID == "0") {
                alert("Please select an email template.");
                return;
            }

            OpenWindow("EmailTemplatePreview.aspx?id=" + EmailTemplateID, "_Preview11", 760, 700, "no", "center");
        }

        function btnPreviewOverdueEmail_onclick() {

            var EmailTemplateID = $("#ddlOverdueEmail").val();
            //alert(EmailTemplateID);

            if (EmailTemplateID == "0") {
                alert("Please select an email template.");
                return;
            }

            OpenWindow("EmailTemplatePreview.aspx?id=" + EmailTemplateID, "_Preview11", 760, 700, "no", "center");
        }

        function PreviewCompletionEmail(id) {

            //alert(id);
            OpenWindow("EmailTemplatePreview.aspx?id=" + id, "_Preview10", 760, 700, "no", "center");
        }

        function ddlStage_onchange() {

            var TemplateID = GetQueryString1("TemplateID");
            var StageID = $("#ddlStage").val();

            if (StageID == "0") {

                window.location.href = "WorkflowTaskTemplateAdd.aspx?TemplateID=" + TemplateID;
            }
            else {

                window.location.href = "WorkflowTaskTemplateAdd.aspx?TemplateID=" + TemplateID + "&StageID=" + StageID;
            }

        }

        function BeforeSave() {

            // required
            var TaskName = $.trim($("#tbxTaskName").val());
            if (TaskName == "") {

                alert("Please enter the task name.");
                return false;
            }

            var StageID = $("#ddlStage").val();
            if (StageID == "0") {

                alert("Please enter select the stage.");
                return false;
            }

            var str = $("#txtDaysDueAfterPrevStage").val();

            if (str != null && typeof (str) != "undefined" && str != "") {
                if (str < -32768 || str > 32767) {
                    alert("Beyond the scope of content!");
                    $("#txtDaysDueAfterPrevStage").focus();
                    return false;
                }
                else {
                    var ex = /^(\+|-)?\d+$/;
                    if (ex.test(str) || str < 0) {
//                        return true;
                    } else {
                        alert("Not a number!");
                        $("#txtDaysDueAfterPrevStage").focus();
                        return false;
                    }
                }
            }


              

            //#region validate email template id duplicate

            var duplicated = false;
            $("#gridCompletionEmailList tr td :input[id^='ddlCmpltEmailTmplt']").each(function (i) {

                var ThisID = $(this).val();

                for (var j = i + 1; j < $("#gridCompletionEmailList tr td :input[^id='ddlCmpltEmailTmplt']").length; j++) {

                    var OtherID = $("#gridCompletionEmailList tr td :input[id^='ddlCmpltEmailTmplt']").eq(j).val();

                    if (ThisID == OtherID) {

                        duplicated = true;
                        alert("The email template id in completion email list can not be duplicated.");
                    }
                }
            });

            if (duplicated == true) {

                return false;
            }

            //#endregion

            //#region EmailTemplateIDs

            var EmailTemplateIDs = "";
            $("#gridCompletionEmailList tr td :input[id^='ddlCmpltEmailTmplt']").each(function (i) {

                var value = $(this).val();

                if (EmailTemplateIDs == "") {

                    EmailTemplateIDs = value;
                }
                else {

                    EmailTemplateIDs += "," + value;
                }
            });

            //alert(EmailTemplateIDs);
            $("#hdnEmailTemplateIDs").val(EmailTemplateIDs);

            //#endregion

            //#region EnabledList

            var EnabledList = "";
            $("#gridCompletionEmailList tr td :input[id='chkEnabled']").each(function (i) {

                var value = $(this).attr("checked");
                if (value == "") {

                    value = "false";
                }

                if (EnabledList == "") {

                    EnabledList = value;
                }
                else {

                    EnabledList += "," + value;
                }
            });

            //alert(EnabledList);
            $("#hdnEnabledList").val(EnabledList);

            //#endregion

            return true;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="border: solid 0px red;">
        
        <div id="btnButtons" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" 
                            OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnClone" type="button" value="Clone" class="Btn-66" disabled />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" class="Btn-66" disabled />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="window.parent.location.href= window.parent.location.href;" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="divDetails" style="margin-top: 10px;">
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
                        <asp:DropDownList ID="ddlStage" runat="server" DataTextField="Name" DataValueField="WflStageId" Width="230px">
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
                        <asp:CheckBox ID="chkEnable" runat="server" Text="Enabled" Checked="true"></asp:CheckBox>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkExternalViewing" runat="server" Text="External Viewing" Checked="true"></asp:CheckBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Sequence:
                    </td>
                    <td style="width: 310px;">
                        <asp:TextBox ID="txtSequence" runat="server" Width="100px" MaxLength="3" Value="1"></asp:TextBox>
                    </td>
                    
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Description:
                    </td>
                    <td style="width: 310px;">
                        <asp:TextBox ID="tbxDescription" runat="server" Width="576px" TextMode="MultiLine" Height="30px"></asp:TextBox>
                    </td>
                </tr>
            </table>
             <table>
                <tr>
                     <td >
                        Days due from Est Close Date:
                   
                        <asp:TextBox ID="tbxDueDaysByDate" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>
                    <td >
                        Days due After Creation Date:
                    </td>
                    <td>
                        <asp:TextBox ID="tbxDaysDueAfterCreationDate" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td >
                        Days due after Completion Date of the Previous Stage:
                    </td>
                    <td >
                        <asp:TextBox ID="txtDaysDueAfterPrevStage" runat="server" Width="100px" MaxLength="5"></asp:TextBox>
                    </td>
                    
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Default Owner:
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList ID="ddlOwner" runat="server" DataTextField="Name" DataValueField="RoleId" Width="290px">
                        </asp:DropDownList>
                    </td>
                   <%-- <td style="width: 80px;">
                        Days due from Est Close Date:
                    </td>
                    <td style="width: 60px;">
                        <asp:TextBox ID="tbxDueDaysByDate" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>--%>
                   <%-- <td style="width: 80px;">
                        Days due After Creation Date:
                    </td>
                    <td>
                        <asp:TextBox ID="tbxDaysDueAfterCreationDate" runat="server" Width="30px" MaxLength="3"></asp:TextBox>
                    </td>--%>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Prerequisite Task:
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList ID="ddlPrerequisiteTask" runat="server" DataTextField="Name" DataValueField="TemplTaskId" Width="290px">
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
                        <asp:DropDownList ID="ddlWarningEmail" runat="server" DataTextField="Name" DataValueField="TemplEmailId" Width="290px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input id="btnPreviewWarningEmail" type="button" value="Preview" class="Btn-66" onclick="btnPreviewWarningEmail_onclick()" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 105px;">
                        Overdue Email:
                    </td>
                    <td style="width: 310px;">
                        <asp:DropDownList ID="ddlOverdueEmail" runat="server" DataTextField="Name" DataValueField="TemplEmailId" Width="290px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input id="btnPreviewOverdueEmail" type="button" value="Preview" class="Btn-66" onclick="btnPreviewOverdueEmail_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divCmpltEmailList">
            <div id="divToolBar" style="margin-top: 10px; ">
                <ul class="ToolStrip" style="margin-left: 0px;">
                    <li><a id="aAdd" href="javascript:aAddEmail_onclick()">Add Completion Email</a><span>|</span></li>
                    <li><a id="aRemove" href="javascript:aRemoveEmail_onclick()">Remove Completion Email</a><span>|</span></li>
                    <li><a id="aEnable" href="javascript:aEnable_onclick()">Enable</a><span>|</span></li>
                    <li><a id="aDisable" href="javascript:aDisable_onclick()">Disable</a></li>
                </ul>
            </div>
            <div id="divCompletionEmailList" class="ColorGrid" style="width: 550px; margin-top: 3px;">
                <table id="gridCompletionEmailList" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                    <tr class="EmptyDataRow" align="center">
                        <td colspan="2">
                            There is no completion email template, please add.
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divCompletionEmailList1" class="ColorGrid" style="width: 550px; margin-top: 3px; display: none;">
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
                                <select id="ddlCmpltEmailTmplt"style="width:350px;">
                                </select>
                            </td>
                            <td style="width: 50px; text-align: center;">
                                <input id="chkEnabled" type="checkbox" checked />
                            </td>
                            <td style="width: 50px; text-align: center">
                                <a id="aPreview" href="" style="text-decoration: underline;">Preview</a>
                            </td>
                        </tr>
                        
                    </table>
                </div>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        
        
    </div>
    <div id="divHiddenFields" style="display: none;">
        <input id="hdnCounter" type="hidden" value="0" />
        <asp:HiddenField ID="hdnEmailTemplateIDs" runat="server" />
        <asp:HiddenField ID="hdnEnabledList" runat="server" />
        <textarea id="hdnCmpltEmailTmpltTemp">
            <asp:DropDownList ID="CmpltEmailTmpltText" runat="server" DataTextField="Name" DataValueField="TemplEmailId" Width="350px">
            </asp:DropDownList>
        </textarea>
    </div>
    
    </form>
</body>
</html>