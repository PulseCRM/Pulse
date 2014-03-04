<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>  
<%@ Page Title="Workflow Template Setup" Language="C#" AutoEventWireup="true" CodeBehind="WorkflowTemplateAdd2.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.WorkflowTemplateAdd2" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            AddValidators();

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtDesc").maxlength(500);

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_chkEnabled").attr("disabled", "true");

            // set parent.iframe.height
            var xx = $("#divContainer").height() + 10;
            //alert(xx);
            if (xx > 700) {

                $(window.parent.document).find("#ifrWflStageSetup").height(xx);
            }
        });

        // add jQuery Validators
        function AddValidators() {

            $("#aspnetForm").validate({

                rules: {
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtWorkflowTemplate: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlWorkflowType: {
                        required: true
                    }
                },
                messages: {
                    ctl00$ctl00$PlaceHolderMain$MainArea$txtWorkflowTemplate: {
                        required: "<div>Please enter Workflow Template.</div>"
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$ddlWorkflowType: {
                        required: "<div>Please select Workflow Type.</div>"
                    }
                }
            });
        }

        //#region Add/Remove Stage

        function aAddStage_onclick() {

            var SelWorkflowType = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlWorkflowType").val();
            if (SelWorkflowType == "") {

                alert("Please select Workflow Type at first.");
                return;
            }

            var TrCount = $("#gridStageList tr").length;

            // add th
            if (TrCount == 1) {

                // clear tr
                $("#gridStageList").empty();

                // add th
                $("#gridStageList").append($("#gridStageList1 tr").eq(0).clone());
            }



            // next index
            var NowIndex = new Number($("#hdnCounter").val());
            var NextIndex = NowIndex + 1;
            //alert("NextIndex: "+NextIndex);

            // clone tr
            var TrCopy = $("#gridStageList1 tr").eq(1).clone(true);

            //#region Add Tr

            // ddlSeq
            var last_seq_value = "0";
            if ($("#gridStageList tr td #ddlSeq:last").length > 0) {

                last_seq_value = $("#gridStageList tr td #ddlSeq:last").val();
            }
            var xSeq = new Number(last_seq_value) + 10;
            if (xSeq > 999) {

                xSeq = 999;
            }
            TrCopy.find("#ddlSeq").val(xSeq);

            //#region ddlStage

            var ddlStage_NewID = "ddlStage" + NextIndex;

            var ddlStageTemp = "";
            var ddlStageCode = "";

            if (SelWorkflowType == "Processing") {

                ddlStageTemp = $.trim($("#hdnStageProcessing").text());
                ddlStageCode = ddlStageTemp.replace("ctl00_ctl00_PlaceHolderMain_MainArea_ddlStageProcessing", ddlStage_NewID);
                ddlStageCode = ddlStageCode.replace("ctl00$ctl00$PlaceHolderMain$MainArea$ddlStageProcessing", ddlStage_NewID);
            }
            else {

                ddlStageTemp = $.trim($("#hdnStageProspect").text());
                ddlStageCode = ddlStageTemp.replace("ctl00_ctl00_PlaceHolderMain_MainArea_ddlStageProspect", ddlStage_NewID);
                ddlStageCode = ddlStageCode.replace("ctl00$ctl00$PlaceHolderMain$MainArea$ddlStageProspect", ddlStage_NewID);
            }

            //alert(ddlStageTemp);
            //alert(ddlStageCode);

            TrCopy.find("#ddlStage").replaceWith(ddlStageCode);

            //#endregion

            //#region txtDisplayedAs

            var txtDisplayedAs_NewID = "txtDisplayedAs" + NextIndex;
            var txtDisplayedAsTemp = $.trim($("#hdnDisplayAsTemp").val());
            var txtDisplayedAsCode = txtDisplayedAsTemp.replace(/txtDisplayedAs/g, txtDisplayedAs_NewID);

            //alert(txtDisplayedAsTemp);
            //alert(txtDisplayedAsCode);

            TrCopy.find("#txtDisplayedAs").replaceWith(txtDisplayedAsCode);

            //#endregion

            //#region txtDaysFromEstCloseDate

            var txtDaysFromEstCloseDate_NewID = "txtDaysFromEstCloseDate" + NextIndex;
            var txtDaysFromEstCloseDateTemp = $.trim($("#hdnDaysFromEstCloseDateTemp").val());

            var txtDaysFromEstCloseDateCode = txtDaysFromEstCloseDateTemp.replace(/txtDaysFromEstCloseDate/g, txtDaysFromEstCloseDate_NewID);

            //alert(txtDaysFromEstCloseDateTemp);
            //alert(txtDaysFromEstCloseDateCode);

            TrCopy.find("#txtDaysFromEstCloseDate").replaceWith(txtDaysFromEstCloseDateCode);

            //#endregion

            //#region txtDaysAfterCreationDate

            var txtDaysAfterCreationDate_NewID = "txtDaysAfterCreationDate" + NextIndex;
            var txtDaysAfterCreationDateTemp = $.trim($("#hdnDaysAfterCreationDateTemp").val());
            var txtDaysAfterCreationDateCode = txtDaysAfterCreationDateTemp.replace(/txtDaysAfterCreationDate/g, txtDaysAfterCreationDate_NewID);

            //alert(txtDaysAfterCreationDateTemp);
            //alert(txtDaysAfterCreationDateCode);

            TrCopy.find("#txtDaysAfterCreationDate").replaceWith(txtDaysAfterCreationDateCode);

            //#endregion

            // append tr
            $("#gridStageList").append(TrCopy);

            // set counter
            $("#hdnCounter").val(NextIndex);

            // disable Workflow Type
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlWorkflowType").attr("disabled", "true");

            // disable Calc. Method
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCalcDueDateMethod").attr("disabled", "true");

            //#region add validate

            $("#" + ddlStage_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please select Stage.</div>"
                }
            });

            $("#" + txtDisplayedAs_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please enter Displayed As.</div>"
                }
            });
            //$("#" + txtDisplayedAs_NewID).alphanumeric({ ichars: "$" });

            // add event
            $("#" + ddlStage_NewID).change(function () {

                var SelStageID = $(this).val();
                var StageAlias = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStageTemplateList").find("option[value='" + SelStageID + "']").text();
                $("#" + txtDisplayedAs_NewID).val(StageAlias);
            });

            var CalcMethod = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCalcDueDateMethod").val();
            if (CalcMethod == "Est Close Date") {    //bug 559 改为由WorkflowType控制, 20110519 Rocky重新修改，改回根据Calculation method
                //if (SelWorkflowType == "Processing"){

                $("#" + txtDaysFromEstCloseDate_NewID).rules("add", {
                    required: true,
                    range: [-365, 365],
                    messages: {
                        required: "<div>Required.</div>",
                        range: "<div>-365 to 365.</div>"
                    }
                });

                // readonly
                $("#" + txtDaysFromEstCloseDate_NewID).attr("readonly", "");
                $("#" + txtDaysAfterCreationDate_NewID).attr("readonly", "true");
            }
            else {

                $("#" + txtDaysAfterCreationDate_NewID).rules("add", {
                    required: true,
                    range: [-365, 365],
                    messages: {
                        required: "<div>Required.</div>",
                        range: "<div>-365 to 365.</div>"
                    }
                });

                // readonly
                $("#" + txtDaysFromEstCloseDate_NewID).attr("readonly", "true");
                $("#" + txtDaysAfterCreationDate_NewID).attr("readonly", "");
            }

            // only number
            $("#" + txtDaysFromEstCloseDate_NewID).OnlyInt();
            $("#" + txtDaysAfterCreationDate_NewID).OnlyInt();

            //#endregion

            //#endregion
        }

        function aRemoveStage_onclick() {

            var SelectedCount = $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No stage was selected.");
                return;
            }

            var Result = confirm("Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if (SelectedCount == $("#gridStageList tr:not(:first)").length) {

                $("#gridStageList").empty();
                $("#gridStageList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no stage, please add.</td></tr>");

                // enable Workflow Type
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlWorkflowType").attr("disabled", "");

                // enable Workflow Type
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCalcDueDateMethod").attr("disabled", "");
            }
            else {

                $("#gridStageList tr:not(:first) td :checkbox[id='chkChecked']:checked").parent().parent().remove();
            }

            // reset sequence
            //ResetSeq();
        }

        function ResetSeq() {

            for (var i = 1; i < $("#gridStageList tr").length; i++) {

                var xSeq = i * 10;
                if (xSeq > 999) {

                    xSeq = 999;
                }
                $("#gridStageList tr").eq(i).find(":input[id='ddlSeq']").val(xSeq);
            }
        }

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridStageList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridStageList tr td :checkbox").attr("checked", "");
            }
        }

        //#endregion

        function aCreateStage_onclick() {

//            var result = confirm("Your changes have not been saved, if you leave this page now, you will lose the changes you have made so far. You probably want to save the changes first.\r\n\r\nDo you still want to continue and lose your changes?");
//            if (result == false) {

//                return;
            //            }
            alert("Please save this workflow template at first.");
            return;
            CreateStage();  //Create Stage
        }

        function CreateStage() {

            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrStageAdd").attr("src", "StageTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&stageid=0&sid=" + radomStr);
            $("#divAddStage").dialog({
                height: 300,
                width: 730,
                title: "Stage template setup",
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            $("body>div[role=dialog]").appendTo("#aspnetForm");
            return false;

        }

        function ClosePopupStage() {

            $("#divAddStage").dialog("close");
        }

        function aUpdateStage_onclick() {

            alert("Please save this workflow template at first.");
            return;
        }

        //#region show/close waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $("body").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        //#endregion

        //#region before save

        function DoValidate() {

            // call validate
            var IsValid = $("#aspnetForm").valid();
            if (IsValid == false) {

                return;
            }

            //#region validate stage list - sequence duplication

            var IsDuplicated = false;
            $("#gridStageList tr td :input[id='ddlSeq']").each(function (i) {

                var ThisSeq = $(this).val();

                for (var j = i + 1; j < $("#gridStageList tr td :input[id='ddlSeq']").length; j++) {

                    var OtherSeq = $("#gridStageList tr td :input[id='ddlSeq']").eq(j).val();

                    if (ThisSeq == OtherSeq) {

                        IsDuplicated = true;
                        return;
                    }
                }
            });

            if (IsDuplicated == true) {

                alert("The Sequence in stage list can not be duplicated.");
                return;
            }

            //#endregion

            //#region validate stage list - stage id

            IsDuplicated = false;
            $("#gridStageList tr td :input[id^='ddlStage']").each(function (i) {

                var ThisID = $(this).val();

                for (var j = i + 1; j < $("#gridStageList tr td :input[id^='ddlStage']").length; j++) {

                    var OtherID = $("#gridStageList tr td :input[id^='ddlStage']").eq(j).val();

                    if (ThisID == OtherID) {

                        IsDuplicated = true;
                        return;
                    }
                }
            });

            if (IsDuplicated == true) {

                alert("The Stage Name in stage list can not be duplicated.");
                return;
            }

            //#endregion

            CheckDuplication();
        }

        function CheckDuplication() {

            //#region check duplication of workflow template name

            var WorkflowTemplateName = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_txtWorkflowTemplate").val());

            // show waiting dialog
            ShowWaitingDialog("Checking duplicates...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("CheckWflTempNameDuplicate.ashx?sid=" + Radom + "&WflTempID=0&WflTempName=" + encodeURIComponent(WorkflowTemplateName), AfterCheckDuplication);

            //#endregion
        }

        function AfterCheckDuplication(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    $("body").unblock();
                    return;
                }

                if (data.IsDuplicated == "true") {

                    $("body").unblock();
                    alert("The workflow template name was used by others, please change another one.");
                    return;
                }

                //$('body').unblock();

                //#region check default

                var IsDefault = $("#ctl00_ctl00_PlaceHolderMain_MainArea_chkDefault").attr("checked");
                if (IsDefault == true) {

                    CheckDefault();
                }
                else {

                    CallPostBack();
                }

                //#endregion

            }, 2000);
        }

        function CheckDefault() {

            // show waiting dialog
            ShowWaitingDialog("Checking defaults...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var WorkflowType = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlWorkflowType").val();
            //alert(WorkflowType);

            // Ajax
            $.getJSON("GetDefaultWflTempCount.ashx?sid=" + Radom + "&WorkflowType=" + encodeURIComponent(WorkflowType), AfterCheckDefault);

        }

        function AfterCheckDefault(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    $("body").unblock();
                    return;
                }

                $("body").unblock();

                var CountNum = new Number(data.DefaultCount);
                //alert(CountNum);
                if (CountNum > 0) {

                    var WorkflowType = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlWorkflowType").val();
                    var result = confirm("More than one default Workflow Template has been configured for Workflow Type " + WorkflowType + ". Do you want to make this one the default Worklfow Template for Workflow Type " + WorkflowType + "?");
                    if (result == false) {

                        return;
                    }
                }

                CallPostBack();

            }, 2000);
        }

        function CallPostBack() {

            //#region Sequences

            var Sequences = "";
            $("#gridStageList tr td :input[id='ddlSeq']").each(function (i) {

                var ThisSeq = $(this).val();

                if (Sequences == "") {

                    Sequences = ThisSeq;
                }
                else {

                    Sequences += "," + ThisSeq;
                }
            });

            //alert(Sequences);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSequences").val(Sequences);

            //#endregion

            //#region StageIDs

            var StageIDs = "";
            $("#gridStageList tr td :input[id^='ddlStage']").each(function (i) {

                var value = $(this).val();

                if (StageIDs == "") {

                    StageIDs = value;
                }
                else {

                    StageIDs += "," + value;
                }
            });

            //alert(StageIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnStageIDs").val(StageIDs);

            //#endregion

            //#region Stage Names

            var StageNames = "";
            $("#gridStageList tr td :input[id^='ddlStage']").each(function (i) {

                var value = $(this).val();
                var StageName = $(this).find("option[value='" + value + "']").text();

                if (StageNames == "") {

                    StageNames = "[$" + StageName + "$]";
                }
                else {

                    StageNames += ",[$" + StageName + "$]";
                }
            });

            //alert(StageNames);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnStageNames").val(StageNames);

            //#endregion

            //#region StageEnableds

            var StageEnableds = "";
            $("#gridStageList tr td :checkbox[id='chkStageEnabled']").each(function (i) {

                var CheckedValue = $(this).attr("checked");

                if (i == 0) {

                    StageEnableds = CheckedValue;
                }
                else {

                    StageEnableds += "," + CheckedValue;
                }
            });

            //alert("StageEnableds: " + StageEnableds);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnEnableds").val(StageEnableds);

            //#endregion

            //#region Days from Est Close Date

            var DaysFromEstCloseDates = "";
            $("#gridStageList tr td :input[id^='txtDaysFromEstCloseDate']").each(function (i) {

                var value = $(this).val();
                if (value == "") {

                    value = "null";
                }

                if (DaysFromEstCloseDates == "") {

                    DaysFromEstCloseDates = value;
                }
                else {

                    DaysFromEstCloseDates += "," + value;
                }
            });

            //alert(DaysFromEstCloseDates);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnDaysFromEstCloseDates").val(DaysFromEstCloseDates);

            //#endregion

            //#region Days after Creation Date

            var DaysAfterCreationDates = "";
            $("#gridStageList tr td :input[id^='txtDaysAfterCreationDate']").each(function (i) {

                var value = $(this).val();
                if (value == "") {

                    value = "null";
                }

                if (DaysAfterCreationDates == "") {

                    DaysAfterCreationDates = value;
                }
                else {

                    DaysAfterCreationDates += "," + value;
                }
            });

            //alert(DaysAfterCreationDates);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnDaysAfterCreationDates").val(DaysAfterCreationDates);

            //#endregion

            // submit form
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlWorkflowType").attr("disabled", "");
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCalcDueDateMethod").attr("disabled", "");
            __doPostBack("ctl00$ctl00$PlaceHolderMain$MainArea$btnSave", "");
        }

        //#endregion

// ]]>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    
    <div id="divContainer" style="width: 985px; height:auto!important; min-height: 700px;">
        <input id="btnGoToList" type="button" value="Back to Workflow Template List" onclick="javascript:window.location.href='WorkflowTemplateList.aspx'" class="Btn-250" />
        <br />
        <br />
        <div id="divWorkflowDetails">
            <table>
                <tr>
                    <td style="width: 110px;">Workflow Template</td>
                    <td style="width: 350px;">
                        <asp:TextBox ID="txtWorkflowTemplate" runat="server" Width="300px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 85px;">Workflow Type</td>
                    <td style="width: 170px;">
                        <asp:DropDownList ID="ddlWorkflowType" runat="server" Width="110px">
                            <asp:ListItem Value="">-- select --</asp:ListItem>
                            <asp:ListItem>Processing</asp:ListItem>
                            <asp:ListItem>Prospect</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
                    </td>
                    <td><label for="chkEnabled">Enabled</label></td>
                    
                    <td style="padding-left:15px;"><asp:CheckBox ID="chkDefault" runat="server" /></td>
                     <td><label for="chkDefault">Default</label></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">Description</td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Width="750px" Height="50px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="margin-top: 2px;">
                <tr>
                    <td style="width: 110px;">Calculate Due Dates<br>based on</td>
                    <td style="width: 170px; vertical-align: top;">
                        <asp:DropDownList ID="ddlCalcDueDateMethod" runat="server" Width="120px">
                            <asp:ListItem>Est Close Date</asp:ListItem>
                            <asp:ListItem>Creation Date</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 175px;">
                        <table cellpadding="0" cellspacing="0" style="position: relative; top: -5px;">
                            <tr>
                                
                            </tr>
                        </table>
                        
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="DoValidate(); return false;" onclick="btnSave_Click" />
                        
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-top: 20px;">
            <div id="divToolBar">
                <ul class="ToolStrip" style="margin-left: 0px;">
                    <li><a id="aAdd" href="javascript:aAddStage_onclick()">Add Stage</a><span>|</span></li>
                    <li><a id="aRemove" href="javascript:aRemoveStage_onclick()">Remove Stage</a><span>|</span></li>
                    <li><a id="aCreate" href="javascript:aCreateStage_onclick()">Create Stage</a><span>|</span></li>
                    <li><a id="aUpdate" href="javascript:aUpdateStage_onclick()">Update Stage</a></li>
                </ul>
            </div>
            <div>
            <div id="divStageList" class="ColorGrid" style="margin-top: 5px;">
                <div>
                    <table id="gridStageList" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                        <tr class="EmptyDataRow" align="center">
                            <td colspan="2">
                                There is no stage, please add.
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
            </div>
            <div id="divStageList1" class="ColorGrid" style="margin-top: 5px; display: none;">
                <div>
                    <table id="gridStageList1" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                        <tr>
	                        <th class="CheckBoxHeader" scope="col"><input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" /></th>
	                        <th scope="col">Sequence</th>
	                        <th scope="col">Stage</th>
	                        <th scope="col">Displayed As</th>
                            <th scope="col">Enabled</th>
	                        <th scope="col">Days from<br />Est Close</th>
	                        <th scope="col">Days after<br />Creation date</th>
                            <th scope="col">Tasks</th>
                        </tr>
                        <tr>
	                        <td class="CheckBoxColumn">
                                <input id="chkChecked" type="checkbox" />
                            </td>
	                        <td style="width: 60px;">
                                <select id="ddlSeq" style="width: 50px;">
                                    <asp:Literal ID="ltrSeqOptions" runat="server"></asp:Literal>
                                </select>
                            </td>
                            <td>
                                <select id="ddlStage" style="width: 245px;">
                                </select>
                            </td>
	                        <td style="width: 230px;">
                                <input id="txtDisplayedAs" type="text" maxlength="50" readonly style="width: 165px;" />
                            </td>
	                        <td style="width: 50px; text-align: center;">
                                <input id="chkStageEnabled" type="checkbox" checked />
                            </td>
	                        <td style="width: 100px;">
                                <input id="txtDaysFromEstCloseDate" type="text" maxlength="4" style="width: 60px;" />
                            </td>
                            <td style="width: 100px;">
                                <input id="txtDaysAfterCreationDate" type="text" maxlength="4" style="width: 60px;" />
                            </td>
                            <td style="width: 40px; text-align: center">
                                <a id="aTaskCount" href="javascript:aUpdateStage_onclick()">0</a>
                            </td>
                        </tr>
                        
                    </table>
                </div>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
    </div>

    <div id="divCodeTemplates" style="display: none;">
        <dl>
            <dd>
                <textarea id="hdnStageProcessing" cols="20" rows="2">
                    <asp:DropDownList ID="ddlStageProcessing" runat="server" DataValueField="TemplStageId" DataTextField="Name" Width="245px"></asp:DropDownList>
                </textarea>
            </dd>
            <dd>
                <textarea id="hdnStageProspect" cols="20" rows="2">
                    <asp:DropDownList ID="ddlStageProspect" runat="server" DataValueField="TemplStageId" DataTextField="Name" Width="245px"></asp:DropDownList>
                </textarea>
            </dd>
            <dd><input id="hdnDisplayAsTemp" type="text" value="<input id='txtDisplayedAs' name='txtDisplayedAs' type='text' maxlength='50' readonly style='width: 165px;' />" /></dd>
            <dd><input id="hdnDaysFromEstCloseDateTemp" type="text" value="<input id='txtDaysFromEstCloseDate' name='txtDaysFromEstCloseDate' type='text' maxlength='4' style='width: 60px;' />" /></dd>
            <dd><input id="hdnDaysAfterCreationDateTemp" type="text" value="<input id='txtDaysAfterCreationDate' name='txtDaysAfterCreationDate' type='text' maxlength='4' style='width: 60px;' />" /></dd>
        </dl>
    </div>

    <div id="divHiddenFields" style="display: none;">
        <input id="hdnCounter" type="hidden" value="0" />
        <asp:TextBox ID="hdnSequences" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnStageIDs" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnStageNames" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnEnableds" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnDaysFromEstCloseDates" runat="server"></asp:TextBox>
        <asp:TextBox ID="hdnDaysAfterCreationDates" runat="server"></asp:TextBox>
        <asp:DropDownList ID="ddlStageTemplateList" runat="server" DataValueField="TemplStageId" DataTextField="Alias"></asp:DropDownList>
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

    <div id="divAddStage" title="Create a new stage template" style="display: none;">
        <iframe id="ifrStageAdd" frameborder="0" scrolling="no" width="700px" height="260px">
        </iframe>
    </div>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
</asp:Content>