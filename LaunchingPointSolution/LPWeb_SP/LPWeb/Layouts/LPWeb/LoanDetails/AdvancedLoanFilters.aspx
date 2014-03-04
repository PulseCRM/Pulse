<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Advanced Loan Filters" Language="C#" AutoEventWireup="true" CodeBehind="AdvancedLoanFilters.aspx.cs"
    Inherits="LoanDetails_AdvancedLoanFilters" MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        .td-width-1
        {
            width: 90px;
        }
        .td-width-2
        {
            width: 270px;
        }
        .td-width-3
        {
            width: 120px;
            text-align: right;
        }
        .ddl-width
        {
            width: 250px;
        }
        .date-field
        {
            width: 100px;
        }
    </style>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.cleditor.js" type="text/javascript"></script>
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.multiselect.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">


        $(document).ready(function () {

            $("input:text[disabled]").css("background-color", "#f9f9f9");

            $("select").multiselect({

                selectedList: 4
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegion").multiselect({

                close: function () {

                    //#region ddlRegion close

                    // get selected checkboxes
                    var checkboxes = $(this).multiselect("getChecked");

                    var region_ids = GetRegionIDs(checkboxes);

                    // get division
                    GetJSON_Division(region_ids);

                    // get Branch
                    GetJSON_Branch(region_ids, "0");

                    // get PointFolder
                    GetJSON_PointFolder(region_ids, "0", "0");

                    // get LoanOfficer
                    GetJSON_LoanRoles2(region_ids, "0", "0", "LoanOfficer", "ddlLoanOfficer");

                    // get LoanOfficerAssistant
                    GetJSON_LoanRoles(region_ids, "0", "0", "LoanOfficerAssistant", "ddlLoanOfficerAssistant");

                    // get Processor
                    GetJSON_LoanRoles2(region_ids, "0", "0", "Processor", "ddlProcessor");

                    // get JrProcessor
                    GetJSON_LoanRoles(region_ids, "0", "0", "JrProcessor", "ddlJrProcessor");

                    // get DocPre
                    GetJSON_LoanRoles(region_ids, "0", "0", "DocPre", "ddlDocPre");

                    // get Shipper
                    GetJSON_LoanRoles(region_ids, "0", "0", "Shipper", "ddlShipper");

                    // get Closer
                    GetJSON_LoanRoles(region_ids, "0", "0", "Closer", "ddlCloser");

                    // get Underwriter
                    GetJSON_LoanRoles2(region_ids, "0", "0", "Underwriter", "ddlUnderwriter");
                }

                //#endregion
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").multiselect({

                close: function () {

                    //#region ddlDivision close

                    // get ddlRegion selected checkboxes
                    var checkboxes1 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegion").multiselect("getChecked");
                    var region_ids = GetRegionIDs(checkboxes1);

                    // get ddlDivision selected checkboxes
                    var checkboxes = $(this).multiselect("getChecked");
                    var division_ids = GetRegionIDs(checkboxes);

                    // get Branch
                    GetJSON_Branch(region_ids, division_ids);

                    // get PointFolder
                    GetJSON_PointFolder(region_ids, division_ids, "0");

                    // get LoanOfficer
                    GetJSON_LoanRoles2(region_ids, division_ids, "0", "LoanOfficer", "ddlLoanOfficer");

                    // get LoanOfficerAssistant
                    GetJSON_LoanRoles(region_ids, division_ids, "0", "LoanOfficerAssistant", "ddlLoanOfficerAssistant");

                    // get Processor
                    GetJSON_LoanRoles2(region_ids, division_ids, "0", "Processor", "ddlProcessor");

                    // get JrProcessor
                    GetJSON_LoanRoles(region_ids, division_ids, "0", "JrProcessor", "ddlJrProcessor");

                    // get DocPre
                    GetJSON_LoanRoles(region_ids, division_ids, "0", "DocPre", "ddlDocPre");

                    // get Shipper
                    GetJSON_LoanRoles(region_ids, division_ids, "0", "Shipper", "ddlShipper");

                    // get Closer
                    GetJSON_LoanRoles(region_ids, division_ids, "0", "Closer", "ddlCloser");

                    // get Underwriter
                    GetJSON_LoanRoles2(region_ids, division_ids, "0", "Underwriter", "ddlUnderwriter");
                }

                //#endregion
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranch").multiselect({

                close: function () {

                    //#region ddlBranch close

                    // get ddlRegion selected checkboxes
                    var checkboxes1 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegion").multiselect("getChecked");
                    var region_ids = GetRegionIDs(checkboxes1);

                    // get ddlDivision selected checkboxes
                    var checkboxes = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").multiselect("getChecked");
                    var division_ids = GetRegionIDs(checkboxes);

                    // get ddlBranch selected checkboxes
                    var checkboxes2 = $(this).multiselect("getChecked");
                    var branch_ids = GetRegionIDs(checkboxes2);

                    // get PointFolder
                    GetJSON_PointFolder(region_ids, division_ids, branch_ids);

                    // get LoanOfficer
                    GetJSON_LoanRoles2(region_ids, division_ids, branch_ids, "LoanOfficer", "ddlLoanOfficer");

                    // get LoanOfficerAssistant
                    GetJSON_LoanRoles(region_ids, division_ids, branch_ids, "LoanOfficerAssistant", "ddlLoanOfficerAssistant");

                    // get Processor
                    GetJSON_LoanRoles2(region_ids, division_ids, branch_ids, "Processor", "ddlProcessor");

                    // get JrProcessor
                    GetJSON_LoanRoles(region_ids, division_ids, branch_ids, "JrProcessor", "ddlJrProcessor");

                    // get DocPre
                    GetJSON_LoanRoles(region_ids, division_ids, branch_ids, "DocPre", "ddlDocPre");

                    // get Shipper
                    GetJSON_LoanRoles(region_ids, division_ids, branch_ids, "Shipper", "ddlShipper");

                    // get Closer
                    GetJSON_LoanRoles(region_ids, division_ids, branch_ids, "Closer", "ddlCloser");

                    // get Underwriter
                    GetJSON_LoanRoles2(region_ids, division_ids, branch_ids, "Underwriter", "ddlUnderwriter");
                }

                //#endregion
            });

            InitValueInput();

            $(".date-field").datepick();

            AddLicensesRow();
        });

        function GetRegionIDs(checkboxes) {

            var region_ids = "";

            if (checkboxes.length == 0) {

                region_ids = "-1";
            }
            else {

                // check 0->ALL
                for (var i = 0; i < checkboxes.length; i++) {

                    if (checkboxes[i].value == "0") {

                        region_ids = "0";
                        break;
                    }
                }

                // if no All
                if (region_ids == "") {

                    for (var i = 0; i < checkboxes.length; i++) {

                        if (i == "0") {

                            region_ids = checkboxes[i].value;
                        }
                        else {

                            region_ids += "," + checkboxes[i].value;
                        }
                    }
                }
            }

            //alert(region_ids);
            return region_ids;
        }

        function GetNames(checkboxes) {

            var Names = "";

            if (checkboxes.length == 0) {

                return "";
            }
            else {

                // check ALL
                for (var i = 0; i < checkboxes.length; i++) {

                    if (checkboxes[i].value == "ALL") {

                        return "";
                    }
                }

                // if no All
                if (Names == "") {

                    for (var i = 0; i < checkboxes.length; i++) {

                        if (i == "0") {

                            Names = checkboxes[i].value;
                        }
                        else {

                            Names += "$$" + checkboxes[i].value;
                        }
                    }
                }
            }

            //alert(Names);
            return Names;
        }

        function GetJSON_Division(region_ids) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // get division
            $.getJSON("GetUserDivisionAjax.aspx?sid=" + sid + "&RegionIDs=" + region_ids, function (data) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").empty();

                if (data.length > 0) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").append("<option value='0'>ALL</option>");
                }

                for (var j = 0; j < data.length; j++) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").append("<option value='" + data[j].DivisionID + "'>" + data[j].Name + "</option>");
                }


                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").multiselect("refresh");
            });
        }

        function GetJSON_Branch(region_ids, division_ids) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // get Branch
            $.getJSON("GetUserBranchAjax.aspx?sid=" + sid + "&RegionIDs=" + region_ids + "&DivisionIDs=" + division_ids, function (data) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranch").empty();

                if (data.length > 0) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranch").append("<option value='0'>ALL</option>");
                }

                for (var j = 0; j < data.length; j++) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranch").append("<option value='" + data[j].BranchID + "'>" + data[j].Name + "</option>");
                }


                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranch").multiselect("refresh");
            });
        }

        function GetJSON_PointFolder(region_ids, division_ids, branch_ids) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // get PointFolder
            $.getJSON("GetPointFolderAjax.aspx?sid=" + sid + "&RegionIDs=" + region_ids + "&DivisionIDs=" + division_ids + "&BranchIDs=" + branch_ids, function (data) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlFolder").empty();

                if (data.length > 0) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlFolder").append("<option value='0'>ALL</option>");
                }

                for (var j = 0; j < data.length; j++) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlFolder").append("<option value='" + data[j].FolderID + "'>" + data[j].Name + "</option>");
                }


                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlFolder").multiselect("refresh");
            });
        }

        function GetJSON_LoanRoles(region_ids, division_ids, branch_ids, role, dropdown_id) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("GetLoanRolesAjax.aspx?sid=" + sid + "&RegionIDs=" + region_ids + "&DivisionIDs=" + division_ids + "&BranchIDs=" + branch_ids + "&Role=" + role, function (data) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).empty();

                if (data.length > 0) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).append("<option value='ALL'>ALL</option>");
                }

                for (var j = 0; j < data.length; j++) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).append("<option value='" + data[j].Name + "'>" + data[j].Name + "</option>");
                }


                $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).multiselect("refresh");
            });
        }

        function GetJSON_LoanRoles2(region_ids, division_ids, branch_ids, role, dropdown_id) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("GetLoanRolesAjax.aspx?sid=" + sid + "&RegionIDs=" + region_ids + "&DivisionIDs=" + division_ids + "&BranchIDs=" + branch_ids + "&Role=" + role, function (data) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).empty();

                if (data.length > 0) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).append("<option value='0'>ALL</option>");
                }

                for (var j = 0; j < data.length; j++) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).append("<option value='" + data[j].ID + "'>" + data[j].Name + "</option>");
                }


                $("#ctl00_ctl00_PlaceHolderMain_MainArea_" + dropdown_id).multiselect("refresh");
            });
        }

        function ExpandTreeNode_Organizations() {

            var IsHidden = $("#divFiltersOrganizations").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeOrganizations").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divFiltersOrganizations").show();
            }
            else {

                $("#imgNodeOrganizations").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divFiltersOrganizations").hide();
            }
        }

        function ExpandTreeNode_UsersBorrower() {

            var IsHidden = $("#divFiltersUsersBorrower").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeUsersBorrower").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divFiltersUsersBorrower").show();
            }
            else {

                $("#imgNodeUsersBorrower").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divFiltersUsersBorrower").hide();
            }
        }

        function ExpandTreeNode_Lender() {

            var IsHidden = $("#divFiltersLender").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeLender").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divFiltersLender").show();
            }
            else {

                $("#imgNodeLender").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divFiltersLender").hide();
            }
        }

        function ExpandTreeNode_Stage() {

            var IsHidden = $("#divFiltersStage").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeLender").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divFiltersStage").show();
            }
            else {

                $("#imgNodeStage").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divFiltersStage").hide();
            }
        }

        function ExpandTreeNode_PointFields() {

            var IsHidden = $("#divFiltersPointFields").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodePointFields").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divFiltersPointFields").show();
            }
            else {

                $("#imgNodePointFields").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divFiltersPointFields").hide();
            }
        }

        function btnFilter_onclick() {

            var q = "";

            // get ddlRegion selected checkboxes
            var checkboxes0 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegion").multiselect("getChecked");
            var region_ids = GetRegionIDs(checkboxes0);
            if (region_ids != "-1" && region_ids != "0") {

                q += "&RegionIDs=" + region_ids;
            }

            // get ddlDivision selected checkboxes
            var checkboxes1 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").multiselect("getChecked");
            var division_ids = GetRegionIDs(checkboxes1);
            if (division_ids != "-1" && division_ids != "0") {

                q += "&DivisionIDs=" + division_ids;
            }

            // get ddlBranch selected checkboxes
            var checkboxes2 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranch").multiselect("getChecked");
            var branch_ids = GetRegionIDs(checkboxes2);
            if (branch_ids != "-1" && branch_ids != "0") {

                q += "&BranchIDs=" + branch_ids;
            }

            // get ddlFolder selected checkboxes
            var checkboxes3 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlFolder").multiselect("getChecked");
            var folder_ids = GetRegionIDs(checkboxes3);
            if (folder_ids != "-1" && folder_ids != "0") {

                q += "&FolderIDs=" + folder_ids;
            }

            // get ddlLoanOfficer selected checkboxes
            var checkboxes4 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLoanOfficer").multiselect("getChecked");
            var loanofficer_ids = GetRegionIDs(checkboxes4);
            if (loanofficer_ids != "-1" && loanofficer_ids != "0") {

                q += "&LoanOfficerIDs=" + loanofficer_ids;
            }

            // get ddlLoanOfficerAssistant selected checkboxes
            var checkboxes5 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLoanOfficerAssistant").multiselect("getChecked");
            var loanofficerassitant_names = GetNames(checkboxes5);
            if (loanofficerassitant_names != "") {

                q += "&LoanOfficerAssistant=" + encodeURIComponent(loanofficerassitant_names);
            }

            // get ddlProcessor selected checkboxes
            var checkboxes6 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlProcessor").multiselect("getChecked");
            var processor_ids = GetRegionIDs(checkboxes6);
            if (processor_ids != "-1" && processor_ids != "0") {

                q += "&ProcessorIDs=" + processor_ids;
            }


            // get ddlJrProcessor selected checkboxes
            var checkboxes7 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlJrProcessor").multiselect("getChecked");
            var jrprocessor_names = GetNames(checkboxes7);
            if (jrprocessor_names != "") {

                q += "&JrProcessor=" + encodeURIComponent(jrprocessor_names);
            }

            // get ddlDocPrep selected checkboxes
            var checkboxes8 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDocPrep").multiselect("getChecked");
            var docprep_names = GetNames(checkboxes8);
            if (docprep_names != "") {

                q += "&DocPrep=" + encodeURIComponent(docprep_names);
            }

            // get ddlShipper selected checkboxes
            var checkboxes9 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlShipper").multiselect("getChecked");
            var shipper_names = GetNames(checkboxes9);
            if (shipper_names != "") {

                q += "&Shipper=" + encodeURIComponent(shipper_names);
            }

            // get ddlCloser selected checkboxes
            var checkboxes10 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCloser").multiselect("getChecked");
            var closer_names = GetNames(checkboxes10);
            if (closer_names != "") {

                q += "&Closer=" + encodeURIComponent(closer_names);
            }

            // get ddlUnderwriter selected checkboxes
            var checkboxes11 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlUnderwriter").multiselect("getChecked");
            var underwriter_ids = GetRegionIDs(checkboxes11);
            if (underwriter_ids != "-1" && underwriter_ids != "0") {

                q += "&UnderwriterIDs=" + underwriter_ids;
            }

            // txtBorrowerLastName
            var BorrowerLastName = $.trim($("#ctl00_ctl00_PlaceHolderMain_MainArea_txtBorrowerLastName").val());
            if (BorrowerLastName != "") {

                q += "&BorrowerLastName=" + encodeURIComponent(BorrowerLastName);
            }

            // get ddlLender selected checkboxes
            var checkboxes12 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLender").multiselect("getChecked");
            var lender_ids = GetRegionIDs(checkboxes12);
            if (lender_ids != "-1" && lender_ids != "0") {

                q += "&LenderIDs=" + lender_ids;
            }

            // ddlProgram
            var checkboxes13 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlProgram").multiselect("getChecked");
            var program_values = GetNames(checkboxes13);
            if (program_values != "") {

                q += "&Program=" + encodeURIComponent(program_values);
            }

            // get ddlPartner selected checkboxes
            var checkboxes14 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlPartner").multiselect("getChecked");
            var partner_ids = GetRegionIDs(checkboxes14);
            if (partner_ids != "-1" && partner_ids != "0") {

                q += "&PartnerIDs=" + partner_ids;
            }

            // get ddlLeadSource selected checkboxes
            var checkboxes15 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLeadSource").multiselect("getChecked");
            var leadsource_names = GetNames(checkboxes15);
            if (leadsource_names != "") {

                q += "&LeadSource=" + encodeURIComponent(leadsource_names);
            }

            // get ddlReferral selected checkboxes
            var checkboxes16 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlReferral").multiselect("getChecked");
            var referral_ids = GetRegionIDs(checkboxes16);
            if (referral_ids != "-1" && referral_ids != "0") {

                q += "&ReferralIDs=" + referral_ids;
            }

            // get ddlPurpose selected checkboxes
            var checkboxes17 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlPurpose").multiselect("getChecked");
            var purpose_names = GetNames(checkboxes17);
            if (purpose_names != "") {

                q += "&Purpose=" + encodeURIComponent(purpose_names);
            }

            // get ddlCurrentStage selected checkboxes
            var checkboxes18 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCurrentStage").multiselect("getChecked");
            var currentstage_ids = GetRegionIDs(checkboxes18);
            if (currentstage_ids != "-1" && currentstage_ids != "0") {

                q += "&CurrentStageIDs=" + currentstage_ids;
            }

            // get ddlLoanStatus selected checkboxes
            var checkboxes19 = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlLoanStatus").multiselect("getChecked");
            var loanstatus_names = GetNames(checkboxes19);
            if (loanstatus_names != "") {

                q += "&LoanStatus=" + encodeURIComponent(loanstatus_names);
            }

            // txtStageCompletionStart
            var StageCompletionStart = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtStageCompletionStart").val();
            if (StageCompletionStart != "") {

                if (isDate(StageCompletionStart, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&StageCompletionStart=" + encodeURIComponent(StageCompletionStart);
            }

            // txtStageCompletionEnd
            var StageCompletionEnd = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtStageCompletionEnd").val();
            if (StageCompletionEnd != "") {

                if (isDate(StageCompletionEnd, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&StageCompletionEnd=" + encodeURIComponent(StageCompletionEnd);
            }

            // txtEstimatedCloseStart
            var EstimatedCloseStart = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtEstimatedCloseStart").val();
            if (EstimatedCloseStart != "") {

                if (isDate(EstimatedCloseStart, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&EstimatedCloseStart=" + encodeURIComponent(EstimatedCloseStart);
            }

            // txtEstimatedCloseEnd
            var EstimatedCloseEnd = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtEstimatedCloseEnd").val();
            if (EstimatedCloseEnd != "") {

                if (isDate(EstimatedCloseEnd, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&EstimatedCloseEnd=" + encodeURIComponent(EstimatedCloseEnd);
            }

            // txtLockExpirationStart
            var LockExpirationStart = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLockExpirationStart").val();
            if (LockExpirationStart != "") {

                if (isDate(LockExpirationStart, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&LockExpirationStart=" + encodeURIComponent(LockExpirationStart);
            }

            // txtLockExpirationEnd
            var LockExpirationEnd = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLockExpirationEnd").val();
            if (LockExpirationEnd != "") {

                if (isDate(LockExpirationEnd, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&LockExpirationEnd=" + encodeURIComponent(LockExpirationEnd);
            }

            // txtCreationDateStart
            var CreationDateStart = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtCreationDateStart").val();
            if (CreationDateStart != "") {

                if (isDate(CreationDateStart, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&CreationDateStart=" + encodeURIComponent(CreationDateStart);
            }

            // txtCreationDateEnd
            var CreationDateEnd = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtCreationDateEnd").val();
            if (CreationDateEnd != "") {

                if (isDate(CreationDateEnd, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    return;
                }

                q += "&CreationDateEnd=" + encodeURIComponent(CreationDateEnd);
            }

            // tbFiltersPointFields
            var PointFieldIDs = "";
            var PointFieldValues = "";
            var DataTypeNames = "";

            var Return = "";
            $("#tbFiltersPointFields tr td input:checkbox:checked").each(function () {


                var PointFieldID = $(this).attr("PointFieldID");
                var DataTypeName = $(this).attr("DataTypeName");

                if (DataTypeName == "String" || DataTypeName == "Yes/No") {

                    var PointFieldValue = "";
                    if (DataTypeName == "String") {

                        PointFieldValue = $.trim($(this).parent().parent().find("input:text[id='txtValue']").val());
                    }
                    else { // Yes/No

                        PointFieldValue = $(this).parent().parent().find("#ddlValue").val();
                    }

                    if (PointFieldValue != "") {

                        if (PointFieldIDs == "") {

                            PointFieldIDs = PointFieldID;
                            DataTypeNames = DataTypeName;
                            PointFieldValues = PointFieldValue;

                        }
                        else {

                            PointFieldIDs += "," + PointFieldID;
                            DataTypeNames += "," + DataTypeName;
                            PointFieldValues += "$$" + PointFieldValue;

                        }
                    }
                }
                else if (DataTypeName == "Numeric" || DataTypeName == "Date") {

                    var StartValueNum = "";
                    var EndValueNum = "";
                    if (DataTypeName == "Numeric") {

                        StartValueNum = $.trim($(this).parent().parent().find("input:text[id='txtStartValueNum']").val());
                        EndValueNum = $.trim($(this).parent().parent().find("input:text[id='txtEndValueNum']").val());

                        //alert(StartValueNum);
                        //alert(EndValueNum);

                        if (StartValueNum != "" && EndValueNum != "") {

                            var StartValueNum1 = new Number(StartValueNum);
                            var EndValueNum1 = new Number(EndValueNum);
                            if (StartValueNum1 > EndValueNum1) {

                                alert("Start Value should be smaller than End Value.");

                                Return = "true";
                            }
                        }
                    }
                    else { // Date

                        StartValueNum = $.trim($(this).parent().parent().find("input:text[id^='txtStartValueDate']").val());
                        EndValueNum = $.trim($(this).parent().parent().find("input:text[id^='txtEndValueDate']").val());
                        //alert("StartValueNum: " + StartValueNum + "||EndValueNum: " + EndValueNum);

                        var result = compareDates(StartValueNum, "MM/dd/yyyy", EndValueNum, "MM/dd/yyyy");
                        if (result == 1) {

                            alert("Start Date should be smaller than End Date.");

                            Return = "true";
                        }
                    }


                    if (StartValueNum != "" || EndValueNum != "") {

                        if (PointFieldIDs == "") {

                            PointFieldIDs = PointFieldID;
                            DataTypeNames = DataTypeName;

                            if (StartValueNum != "" && EndValueNum != "") {

                                PointFieldValues = StartValueNum + ";" + EndValueNum;
                            }
                            else if (StartValueNum != "" && EndValueNum == "") {

                                PointFieldValues = StartValueNum + ";null";
                            }
                            else if (StartValueNum == "" && EndValueNum != "") {

                                PointFieldValues = "null;" + EndValueNum;
                            }
                        }
                        else {

                            PointFieldIDs += "," + PointFieldID;
                            DataTypeNames += "," + DataTypeName;

                            if (StartValueNum != "" && EndValueNum != "") {

                                PointFieldValues += "$$" + StartValueNum + ";" + EndValueNum;
                            }
                            else if (StartValueNum != "" && EndValueNum == "") {

                                PointFieldValues += "$$" + StartValueNum + ";null";
                            }
                            else if (StartValueNum == "" && EndValueNum != "") {

                                PointFieldValues += "$$" + "null;" + EndValueNum;
                            }
                        }
                    }
                }
            });

            //            alert(PointFieldIDs);
            //            alert(PointFieldValues);
            //            alert(DataTypeNames);
            //            return;

            if (Return == "true") {

                return;
            }
            if (PointFieldIDs != "" && PointFieldValues != "" && DataTypeNames != "") {

                q += "&PointFieldIDs=" + encodeURIComponent(PointFieldIDs) + "&PointFieldValues=" + encodeURIComponent(PointFieldValues) + "&DataTypeNames=" + encodeURIComponent(DataTypeNames);
            }

            var sTaskNameString = "";
            var sDueDateString = "";
            var sCompDateString = "";
            var SetReturn = "";
            $("#gridLicensesList tr td input:text[id='hidMark']").each(function () {
                var TaskName = $(this).parent().parent().find("input:text[cid='TaskName']");
                var TaskNameValue = $.trim(TaskName.val());
                //                if (TaskNameValue == "") {
                //                    alert("Please enter Task Name.");
                //                    SetReturn ="true";
                //                }
                var Exact = $(this).parent().parent().find("input:radio[cid='Exact']");
                var Partial = $(this).parent().parent().find("input:radio[cid='Partial']");

                if (Exact.attr("checked") == true) {
                    if (TaskNameValue != "") {
                        sTaskNameString += "LoanTasks.Name='" + TaskNameValue + "',"
                    }
                }
                if (Partial.attr("checked") == true) {
                    if (TaskNameValue != "") {
                        sTaskNameString += "LoanTasks.Name like '%" + TaskNameValue + "%',"
                    }
                }

                var DueDateStart = $(this).parent().parent().find("input:text[cid='DueDateStart']");
                var DueDateStartValue = $.trim(DueDateStart.val());
                var DueDateEnd = $(this).parent().parent().find("input:text[cid='DueDateEnd']");
                var DueDateEndValue = $.trim(DueDateEnd.val());

                sDueDateString += DueDateStartValue + "|" + DueDateEndValue + ",";


                var CompDateStart = $(this).parent().parent().find("input:text[cid='CompDateStart']");
                var CompDateStartValue = $.trim(CompDateStart.val());
                var CompDateEnd = $(this).parent().parent().find("input:text[cid='CompDateEnd']");
                var CompDateEndValue = $.trim(CompDateEnd.val());

                sCompDateString += CompDateStartValue + "|" + CompDateEndValue + ",";
            });

            if (SetReturn == "true") {
                return;
            }
            q += "&TaskNames=" + $.base64Encode(sTaskNameString) + "&DueDates=" + $.base64Encode(sDueDateString) + "&CompDates=" + $.base64Encode(sCompDateString);
          
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var url = "../Pipeline/ProcessingPipelineSummary.aspx?filter=AdvacedLoanFilters&sid=" + sid + q;

            //alert(url);
            //return;

            window.location.href = url;

        }

        function Reset() {

            window.location.href = window.location.href;
        }

        function InitValueInput() {

            $("#tbFiltersPointFields tr td input:hidden[id=hdnValueInput]").each(function () {

                var DataTypeName = $(this).val();
                //alert(DataTypeName);

                if (DataTypeName == "Numeric") {

                    $(this).replaceWith("<input id='txtStartValueNum' type='text' value='' style='width: 100px' />&nbsp;&nbsp;&nbsp;&nbsp;<input id='txtEndValueNum' type='text' value='' style='width: 100px' />");

                    $("#txtStartValueNum").numeric({ allow: "." });
                    $("#txtEndValueNum").numeric({ allow: "." });
                }
                else if (DataTypeName == "Yes/No") {

                    $(this).replaceWith("<select id='ddlValue' style='width: 102px'><option></option><option>Yes</option><option>No</option></select>");
                }
                else if (DataTypeName == "Date") {

                    var PointFieldID = $(this).parent().parent().find(":checkbox[id='chkChecked']").attr("PointFieldID");
                    //alert(PointFieldID);

                    $(this).replaceWith("<input id='txtStartValueDate" + PointFieldID + "' type='text' value='' class='date-field' />&nbsp;&nbsp;&nbsp;&nbsp;<input id='txtEndValueDate" + PointFieldID + "' type='text' value='' class='date-field' />");

                }
                else {

                    $(this).replaceWith("<input id='txtValue' type='text' value='' style='width: 370px' />");
                }
            });
        }

        //Licenses
        var hdnCountNum = "<%= hdnCountNum.ClientID%>";

        function AddLicensesRow() {

           // var TrCopy = $("#gridLicensesListTMP tr").eq(0).clone(true);
            var num= $("#" + hdnCountNum).val();
            var i = num + 1;

            var TrCopyNew = "<tr><td class='CheckBoxColumn'><span title=''><input id='chkSelected" + i + "' type='checkbox' uid='cbLicenses' name='ckbSelected' /></span></td><td style='width: 160px;'><input name='txbTaskName" + i + "' cid='TaskName' id='txbTaskName" + i + "' type='text' style='width: 150px;' /> </td><td style='width: 150px;'><input name='MatchType" + i + "'  cid='Exact'  id='MatchType" + i + "_Exact'  type='radio' value='Exact' style='width:20px;' >Exact</input><input name='MatchType" + i + "' cid='Partial' id='MatchType" + i + "_Partial' type='radio' value='Partial' style='width:20px;' checked='true'>Partial</input></td> <td style='width: 240px;'><input name='tbDueDateStart" + i + "' cid='DueDateStart' id='tbDueDateStart" + i + "' type='text' class=\"date-field\" style='width: 90px;' />&nbsp;&nbsp;&nbsp;&nbsp;<input name='tbDueDateEnd" + i + "' cid='DueDateEnd' id='tbDueDateEnd" + i + "' type='text' class=\"date-field\" style='width: 90px;' /></td><td style='width: 240px;'><input name='tbCompDateStart" + i + "' cid='CompDateStart' id='tbCompDateStart" + i + "' type='text' class=\"date-field\" style='width: 90px;' />&nbsp;&nbsp;&nbsp;&nbsp;<input name='tbCompDateEnd" + i + "' cid='CompDateEnd' id='tbCompDateEnd" + i + "' type='text'  class=\"date-field\" style='width: 90px;' /></td><td style='display:none;'><input type='text' id='hidMark' /></td></tr>";

            $("#gridLicensesList").append(TrCopyNew);
            $("#" + hdnCountNum).val(i);
            $(".date-field").datepick();
        }

        function RemoveLicensesRow() {

            $("#gridLicensesList input[uid='cbLicenses']:checked").parent().parent().parent().remove();
            $("#gridLicensesList tr th input[type=checkbox]").attr("checked", "");
        }

        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#gridLicensesList tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#gridLicensesList tr td input[type=checkbox]").attr("checked", "");
            }
        }

        var hidLicenseNumberListID = "<%= hidLicenseNumberList.ClientID%>";

        function SaveAll_Licenses() {

            var LicenseNumberList = $("#gridLicensesList input[uid='LicenseNumber']");
            var str = "";

            LicenseNumberList.each(function (i) {

                str = str + $(this).val() + ",";
            });



            $("#" + hidLicenseNumberListID).val(str);

            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="AdvFilterContainer" class="margin-top-10 margin-bottom-20">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 150px; font-weight: bold; font-size: 12px;">
                    Advanced Loan Filters
                </td>
                <td>
                    <input id="btnFilter" type="button" value="Filter" class="Btn-66" onclick="btnFilter_onclick()" />
                    &nbsp;&nbsp;
                    <input id="btnReset" type="reset" value="Reset" class="Btn-66" onclick="Reset()" />
                </td>
            </tr>
        </table>
        <div id="divContainerOrganizations" style="margin-top: 20px; padding-bottom: 15px;
            border-bottom: solid 1px #d6d9e0;">
            <div id="divTileOrganizations" runat="server" style="font-weight: bold;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodeOrganizations" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;"
                                onclick="ExpandTreeNode_Organizations()" />
                        </td>
                        <td style="font-weight: bold;">
                            Organizations
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divFiltersOrganizations" style="margin-left: 20px; margin-top: 10px;">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td class="td-width-1">
                            Region:
                        </td>
                        <td class="td-width-2">
                            <asp:DropDownList ID="ddlRegion" runat="server" DataValueField="RegionID" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Division:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDivision" runat="server" DataValueField="DivisionID" DataTextField="Name"
                                CssClass="ddl-width">
                                <asp:ListItem Value="">ALL</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Branch:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" DataValueField="BranchID" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Folder:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFolder" runat="server" DataValueField="FolderID" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divContainerUsersBorrower" style="margin-top: 15px; padding-bottom: 15px;
            border-bottom: solid 1px #d6d9e0;">
            <div id="divTileUsersBorrower" runat="server" style="font-weight: bold;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodeUsersBorrower" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;"
                                onclick="ExpandTreeNode_UsersBorrower()" />
                        </td>
                        <td style="font-weight: bold;">
                            Users and Borrower
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divFiltersUsersBorrower" style="margin-left: 20px; margin-top: 10px;">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td class="td-width-1">
                            Loan Officer:
                        </td>
                        <td class="td-width-2">
                            <asp:DropDownList ID="ddlLoanOfficer" runat="server" DataValueField="ID" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Loan Officer Assistant:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLoanOfficerAssistant" DataValueField="Name" DataTextField="Name"
                                runat="server" CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Processor:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProcessor" runat="server" DataValueField="ID" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Jr Processor:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlJrProcessor" runat="server" DataValueField="Name" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Doc Prep:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlDocPrep" runat="server" DataValueField="Name" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Shipper:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlShipper" runat="server" DataValueField="Name" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Closer:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCloser" runat="server" DataValueField="Name" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Underwriter:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUnderwriter" runat="server" DataValueField="ID" DataTextField="Name"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Borrower<br />
                            Last Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtBorrowerLastName" runat="server" Width="240"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divContainerLender" style="margin-top: 15px; padding-bottom: 15px; border-bottom: solid 1px #d6d9e0;">
            <div id="divTileLender" runat="server" style="font-weight: bold;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodeLender" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;"
                                onclick="ExpandTreeNode_Lender()" />
                        </td>
                        <td style="font-weight: bold;">
                            Lender, Referral and Loan Info
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divFiltersLender" style="margin-left: 20px; margin-top: 10px;">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td class="td-width-1">
                            Lender:
                        </td>
                        <td class="td-width-2">
                            <asp:DropDownList ID="ddlLender" runat="server" DataValueField="LenderId" DataTextField="Lender"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Program:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlProgram" runat="server" DataValueField="Program" DataTextField="Program"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Partner:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPartner" runat="server" DataValueField="PartnerId" DataTextField="Partner"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Lead Source:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLeadSource" runat="server" DataValueField="LeadSource" DataTextField="LeadSource"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Referral:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlReferral" runat="server" DataValueField="ReferralId" DataTextField="Referral"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Purpose:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlPurpose" runat="server" DataValueField="Purpose" DataTextField="Purpose"
                                CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divContainerStage" style="margin-top: 15px; padding-bottom: 15px; border-bottom: solid 1px #d6d9e0;">
            <div id="divTileStage" runat="server" style="font-weight: bold;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodeStage" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;"
                                onclick="ExpandTreeNode_Stage()" />
                        </td>
                        <td style="font-weight: bold;">
                            Stage and Status Dates
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divFiltersStage" style="margin-left: 20px; margin-top: 10px;">
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td style="text-align: center; font-weight: bold;">
                            Start
                        </td>
                        <td style="text-align: center; font-weight: bold;">
                            End
                        </td>
                    </tr>
                    <tr>
                        <td class="td-width-1">
                            Current Stage:
                        </td>
                        <td class="td-width-2">
                            <asp:DropDownList ID="ddlCurrentStage" runat="server" DataValueField="TemplStageId"
                                DataTextField="Alias" CssClass="ddl-width">
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Stage Completion:
                        </td>
                        <td>
                            <asp:TextBox ID="txtStageCompletionStart" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtStageCompletionEnd" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Loan Status:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlLoanStatus" runat="server" CssClass="ddl-width">
                                <asp:ListItem Value="ALL">ALL</asp:ListItem>
                                <asp:ListItem Value="Active">Active</asp:ListItem>
                                <asp:ListItem Value="Canceled">Canceled</asp:ListItem>
                                <asp:ListItem Value="Closed">Closed</asp:ListItem>
                                <asp:ListItem Value="Denied">Denied</asp:ListItem>
                                <%--CR063 <asp:ListItem Value="Suspended">Suspended</asp:ListItem>--%>
                            </asp:DropDownList>
                        </td>
                        <td class="td-width-3">
                            Estimated Close:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstimatedCloseStart" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEstimatedCloseEnd" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td-width-3">
                            Lock Expiration:
                        </td>
                        <td>
                            <asp:TextBox ID="txtLockExpirationStart" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLockExpirationEnd" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td class="td-width-3">
                            Creation Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreationDateStart" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreationDateEnd" runat="server" CssClass="date-field"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divTaskNames" style="margin-top: 15px; padding-bottom: 15px; border-bottom: solid 1px #d6d9e0;">
            <div id="divTileTaskNames" runat="server" style="font-weight: bold;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodeTaskNames" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;"
                                onclick="ExpandTreeNode_Stage()" />
                        </td>
                        <td style="font-weight: bold;">
                            Task Names
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divFiltersTaskNames" class="" style="width: 200px; margin-left: 10px; margin-top: 10px;">
                <div style="margin-left: 25px;">
                    <a href="javascript:void(0);" onclick="AddLicensesRow();">Add Row</a> &nbsp;&nbsp;|&nbsp;&nbsp;
                    <a href="javascript:void(0);" onclick="RemoveLicensesRow();">Remove Row</a></div>
                <div style="height:10px;" ></div>
                <table id="gridLicensesList" class="GrayGrid" cellspacing="0" cellpadding="3" border="0" style="border-collapse: collapse;
                   width:830px;">
                    <tr>
                        <th class="CheckBoxHeader" scope="col">
                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                        </th>
                        <th scope="col" style="width: 160px;" align="left">
                            Task Name
                        </th>
                        <th scope="col" style="width: 150px;" align="left">
                            Match Type
                        </th>
                        <th scope="col" style="width: 240px;" align="left">
                            Due Date (Start/End)
                        </th>
                        <th scope="col" style="width: 240px;" align="left">
                            Completion Date (Start/End)
                        </th>
                    </tr>
                </table>
               
                <asp:HiddenField ID="hidLicenseNumberList" runat="server" Value="" />
                <asp:HiddenField ID="hdnCountNum" runat="server" Value="0" />
                <div class="GridPaddingBottom">
                    &nbsp;</div>
                <table class="GrayGrid" cellspacing="0" cellpadding="3" border="0" id="gridLicensesListTMP"
                    style="border-collapse: collapse; display: none; width:820px;">
                    <tr>
                        <td class="CheckBoxColumn">
                            <span title="">
                                <input type="checkbox" uid="cbLicenses" name="ckbSelected" /></span>
                        </td>
                        <td style="width: 180px;">
                            <input name="txbTaskName" type="text" style="width: 150px;" />
                        </td>
                        <td style="width: 150px;">
                            <input name="MatchType" type="radio" value="Exact" >Exact</input>
                            <input name="MatchType" type="radio" value="Partial" >Partial</input>
                        </td>
                        <td style="width: 230px;">
                            <input name="tbDueDateStart" type="text" class="date-field" style="width: 90px;" />
                            <input name="tbDueDateEnd" type="text" class="date-field" style="width: 90px;" />
                        </td>
                        <td style="width: 230px;">
                            <input name="tbCompDateStart" type="text" class="date-field" style="width: 90px;" />
                            <input name="tbCompDateEnd" type="text"  class="date-field" style="width: 90px;" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divContainerPointFields" style="margin-top: 15px; padding-bottom: 15px;
            border-bottom: solid 1px #d6d9e0;">
            <div id="divTilePointFields" runat="server" style="font-weight: bold;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodePointFields" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;"
                                onclick="ExpandTreeNode_PointFields()" />
                        </td>
                        <td style="font-weight: bold;">
                            Point Fields
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divFiltersPointFields" style="margin-left: 20px; margin-top: 10px;">
                <table id="tbFiltersPointFields" cellpadding="3" cellspacing="3">
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td style="font-weight: bold;">
                            Column Heading
                        </td>
                        <td style="font-weight: bold;">
                            Data Type
                        </td>
                        <td style="font-weight: bold;">
                            Search value(s) (multiple values can be entered with ";")
                        </td>
                    </tr>
                    <asp:Repeater ID="rptHeading" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <input id="chkChecked" type="checkbox" pointfieldid="<%# Eval("PointFieldId")%>"
                                        datatypename="<%# Eval("DataTypeName") %>" />
                                </td>
                                <td>
                                    <input id="txtPointField" type="text" value="<%# Eval("Heading") %>" disabled style="width: 370px;" />
                                </td>
                                <td>
                                    <input id="txtDataType" type="text" value="<%# Eval("DataTypeName") %>" disabled
                                        style="width: 80px;" />
                                </td>
                                <td>
                                    <input id="hdnValueInput" type="hidden" value="<%# Eval("DataTypeName") %>" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
        <div class="margin-top-20">
            <input id="btnFilter2" type="button" value="Filter" class="Btn-66" onclick="btnFilter_onclick()" />
            &nbsp;&nbsp;
            <input id="btnReset2" type="reset" value="Reset" class="Btn-66" onclick="Reset()" />
        </div>
    </div>
</asp:Content>
