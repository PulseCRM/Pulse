<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdvancedSearch.aspx.cs" Inherits="Pipeline_AdvancedSearch"  MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        //#region declare id

        var ddlState = "<%= this.ddlState.ClientID %>";
        var ddlLoanOfficer = "<%= this.ddlLoanOfficer.ClientID %>";
        var ddlProcessor = "<%= this.ddlProcessor.ClientID %>";
        var ddlRegion = "<%= this.ddlRegion.ClientID %>";
        var ddlDivision = "<%= this.ddlDivision.ClientID %>";
        var ddlBranch = "<%= this.ddlBranch.ClientID %>";
        var ddlServiceType = "<%= this.ddlServiceType.ClientID %>";

        var divSearchResultContainer = "<%= this.divSearchResultContainer.ClientID %>";

        //#endregion

        $(document).ready(function () {

            InitSearchInput();

            // add event
            $("#" + ddlRegion).change(ddlRegion_onchange);
            $("#" + ddlDivision).change(ddlDivision_onchange);
        });

        //#region expand tree node

        function ExpandTreeNode_SearchCriteria() {

            var IsHidden = $("#divSearchCriteriaContainer").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeSearchCriteria").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divSearchCriteriaContainer").show();
            }
            else {

                $("#imgNodeSearchCriteria").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divSearchCriteriaContainer").hide();
            }
        }

        function ExpandTreeNode_SearchResults() {

            var IsHidden = $("#" + divSearchResultContainer).is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeSearchResults").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#" + divSearchResultContainer).show();
            }
            else {

                $("#imgNodeSearchResults").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#" + divSearchResultContainer).hide();
            }
        }

        function ExpandTreeNode_ActiveLoans() {

            var IsHidden = $("#divPagerAndGrid_ActiveLoans").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeActiveLoans").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divPagerAndGrid_ActiveLoans").show();
            }
            else {

                $("#imgNodeActiveLoans").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divPagerAndGrid_ActiveLoans").hide();
            }
        }

        function ExpandTreeNode_Opportunities() {

            var IsHidden = $("#divPagerAndGrid_Opportunities").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeOpportunities").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divPagerAndGrid_Opportunities").show();
            }
            else {

                $("#imgNodeOpportunities").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divPagerAndGrid_Opportunities").hide();
            }
        }

        function ExpandTreeNode_ArchivedLoans() {

            var IsHidden = $("#divPagerAndGrid_ArchivedLoans").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeArchivedLoans").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divPagerAndGrid_ArchivedLoans").show();
            }
            else {

                $("#imgNodeArchivedLoans").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divPagerAndGrid_ArchivedLoans").hide();
            }
        }

        function ExpandTreeNode_Clients() {

            var IsHidden = $("#divPagerAndGrid_Clients").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodeClients").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divPagerAndGrid_Clients").show();
            }
            else {

                $("#imgNodeClients").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divPagerAndGrid_Clients").hide();
            }
        }

        function ExpandTreeNode_Partners() {

            var IsHidden = $("#divPagerAndGrid_Partners").is(":hidden");
            //alert(IsHidden);
            if (IsHidden == true) {

                $("#imgNodePartners").attr("src", "../images/CompanyOverview/accordion.gif")
                $("#divPagerAndGrid_Partners").show();
            }
            else {

                $("#imgNodePartners").attr("src", "../images/CompanyOverview/expansion.gif")
                $("#divPagerAndGrid_Partners").hide();
            }
        }

        //#endregion

        function InitSearchInput() {

            //#region checkbox

            var SearchLoans = GetQueryString1("SearchLoans");
            if (SearchLoans != "") {
                $("#chkSearchLoans").attr("checked", eval(SearchLoans));
            }

            var SearchOpportunities = GetQueryString1("SearchOpportunities");
            if (SearchOpportunities != "") {
                $("#chkSearchOpportunities").attr("checked", eval(SearchOpportunities));
            }

            var SearchArchivedLoans = GetQueryString1("SearchArchivedLoans");
            if (SearchArchivedLoans != "") {
                $("#chkSearchArchivedLoans").attr("checked", eval(SearchArchivedLoans));
            }

            var SearchClients = GetQueryString1("SearchClients");
            if (SearchClients != "") {
                $("#chkSearchClients").attr("checked", eval(SearchClients));
            }

            var SearchPartners = GetQueryString1("SearchPartners");
            if (SearchPartners != "") {
                $("#chkSearchPartners").attr("checked", eval(SearchPartners));
            }


            //#endregion

            //#region text box

            // Name
            var Name = GetQueryString1("Name");
            if (Name != "") {
                $("#txtName").val(Name);
            }

            // Company
            var Company = GetQueryString1("Company");
            if (Company != "") {
                $("#txtCompany").val(Company);
            }

            // Address
            var Address = GetQueryString1("Address");
            if (Address != "") {
                $("#txtAddress").val(Address);
            }

            // City
            var City = GetQueryString1("City");
            if (City != "") {
                $("#txtCity").val(City);
            }

            // State
            var State = GetQueryString1("State");
            if (State != "") {
                $("#" + ddlState).val(State);
            }

            // Email
            var Email = GetQueryString1("Email");
            if (Email != "") {
                $("#txtEmail").val(Email);
            }

            // Phone
            var Phone = GetQueryString1("Phone");
            if (Phone != "") {
                $("#txtPhone").val(Phone);
            }

            // Loan Number
            var LoanNumber = GetQueryString1("LoanNumber");
            if (LoanNumber != "") {
                $("#txtLoanNumber").val(LoanNumber);
            }

            // Filename
            var Filename = GetQueryString1("Filename");
            if (Filename != "") {
                $("#txtFilename").val(Filename);
            }

            //#endregion

            //#region dropdown list

            // Loan Officer
            var LoanOfficer = GetQueryString1("LoanOfficer");
            if (LoanOfficer != "") {
                $("#" + ddlLoanOfficer).val(LoanOfficer);
            }

            // Processor
            var Processor = GetQueryString1("Processor");
            if (Processor != "") {
                $("#" + ddlProcessor).val(Processor);
            }

            // Region
            var Region = GetQueryString1("Region");
            if (Region != "") {
                $("#" + ddlRegion).val(Region);
            }

            // Division
            var Division = GetQueryString1("Division");
            if (Division != "") {
                $("#" + ddlDivision).val(Division);
            }

            // Branch
            var Branch = GetQueryString1("Branch");
            if (Branch != "") {
                $("#" + ddlBranch).val(Branch);
            }

            // Service Type
            var ServiceType = GetQueryString1("ServiceType");
            if (ServiceType != "") {
                $("#" + ddlServiceType).val(ServiceType);
            }

            //#endregion
        }

        function btnSearch_onclick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "?sid=" + sid;

            //#region search options (checkbox)

            var SearchLoans = $("#chkSearchLoans").attr("checked");
            var SearchOpportunities = $("#chkSearchOpportunities").attr("checked");
            var SearchArchivedLoans = $("#chkSearchArchivedLoans").attr("checked");
            var SearchClients = $("#chkSearchClients").attr("checked");
            var SearchPartners = $("#chkSearchPartners").attr("checked");

            if (SearchLoans == false
                && SearchOpportunities == false
                 && SearchArchivedLoans == false
                  && SearchClients == false
                   && SearchPartners == false) {

                alert("You must check one option at lest.");
                return;
            }

            sQueryStrings += "&SearchLoans=" + SearchLoans + "&SearchOpportunities=" + SearchOpportunities + "&SearchArchivedLoans=" + SearchArchivedLoans + "&SearchClients=" + SearchClients + "&SearchPartners=" + SearchPartners;

            //#endregion

            //#region textbox

            // Name
            var Name = $.trim($("#txtName").val());
            if (Name != "") {
                sQueryStrings += "&Name=" + encodeURIComponent(Name);
            }

            // Company
            var Company = $.trim($("#txtCompany").val());
            if (Company != "") {
                sQueryStrings += "&Company=" + encodeURIComponent(Company);
            }

            // Address
            var Address = $.trim($("#txtAddress").val());
            if (Address != "") {
                sQueryStrings += "&Address=" + encodeURIComponent(Address);
            }

            // City
            var City = $.trim($("#txtCity").val());
            if (City != "") {
                sQueryStrings += "&City=" + encodeURIComponent(City);
            }

            // State
            var State = $("#" + ddlState).val();
            if (State != "") {
                sQueryStrings += "&State=" + encodeURIComponent(State);
            }

            // Email
            var Email = $.trim($("#txtEmail").val());
            if (Email != "") {
                sQueryStrings += "&Email=" + encodeURIComponent(Email);
            }

            // Phone
            var Phone = $.trim($("#txtPhone").val());
            if (Phone != "") {
                sQueryStrings += "&Phone=" + encodeURIComponent(Phone);
            }

            // Loan Number
            var LoanNumber = $.trim($("#txtLoanNumber").val());
            if (LoanNumber != "") {
                sQueryStrings += "&LoanNumber=" + encodeURIComponent(LoanNumber);
            }

            // Filename
            var Filename = $.trim($("#txtFilename").val());
            if (Filename != "") {
                sQueryStrings += "&Filename=" + encodeURIComponent(Filename);
            }

            //#endregion

            //#region dropdown list

            // Loan Officer
            var LoanOfficer = $("#" + ddlLoanOfficer).val();
            if (LoanOfficer != "" && LoanOfficer != null) {
                sQueryStrings += "&LoanOfficer=" + encodeURIComponent(LoanOfficer);
            }

            // Processor
            var Processor = $("#" + ddlProcessor).val();
            if (Processor != "" && Processor != null) {
                sQueryStrings += "&Processor=" + encodeURIComponent(Processor);
            }

            // Region
            var Region = $("#" + ddlRegion).val();
            if (Region != "" && Region != null) {
                sQueryStrings += "&Region=" + encodeURIComponent(Region);
            }

            // Division
            var Division = $("#" + ddlDivision).val();
            if (Division != "" && Division != null) {
                sQueryStrings += "&Division=" + encodeURIComponent(Division);
            }

            // Branch
            var Branch = $("#" + ddlBranch).val();
            if (Branch != "" && Branch != null) {
                sQueryStrings += "&Branch=" + encodeURIComponent(Branch);
            }

            // Service Type
            var ServiceType = $("#" + ddlServiceType).val();
            if (ServiceType != "" && ServiceType != null) {
                sQueryStrings += "&ServiceType=" + encodeURIComponent(ServiceType);
            }

            //#endregion

            //#region PageIndex

            var PageIndex1 = GetQueryString1("PageIndex1");
            if (PageIndex1 != "" && PageIndex1 != "1") {
                sQueryStrings += "&PageIndex1=" + PageIndex1;
            }

            var PageIndex2 = GetQueryString1("PageIndex2");
            if (PageIndex2 != "" && PageIndex2 != "1") {
                sQueryStrings += "&PageIndex2=" + PageIndex2;
            }

            var PageIndex3 = GetQueryString1("PageIndex3");
            if (PageIndex3 != "" && PageIndex3 != "1") {
                sQueryStrings += "&PageIndex3=" + PageIndex3;
            }

            var PageIndex4 = GetQueryString1("PageIndex4");
            if (PageIndex4 != "" && PageIndex4 != "1") {
                sQueryStrings += "&PageIndex4=" + PageIndex4;
            }

            var PageIndex5 = GetQueryString1("PageIndex5");
            if (PageIndex5 != "" && PageIndex5 != "1") {
                sQueryStrings += "&PageIndex5=" + PageIndex5;
            }

            //#endregion

            //alert(sQueryStrings);

            if (sQueryStrings == "") {

                window.location.href = window.location.href;
            }
            else {

                window.location.href = window.location.pathname + sQueryStrings;
            }
        }

        function btnClear_onclick() {

            window.location.href = window.location.pathname;
        }

        //#region Region→Division→Branch

        function ddlRegion_onchange() {

            $("#" + ddlDivision).val('');
            $("#" + ddlDivision).attr('disabled', 'true');

            $("#" + ddlBranch).val('');
            $("#" + ddlBranch).attr('disabled', 'true');

            var RegionID = $("#" + ddlRegion).val();

            GetDivisions(RegionID);
        }

        function GetDivisions(RegionID) {

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("GetDivisionOptions.ashx?sid=" + Radom + "&RegionID=" + RegionID, AfterGetDivisions);
        }

        function AfterGetDivisions(data) {

            if (data.ExecResult == "Failed") {

                alert(data.ErrorMsg);
                return;
            }

            $("#" + ddlDivision).empty();
            $("#" + ddlDivision).append(data.Options);
            $("#" + ddlDivision).attr('disabled', '');

            var RegionID = $("#" + ddlRegion).val();

            GetBranchesByRegion(RegionID);
        }

        function GetBranchesByRegion(RegionID) {

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("GetBranchOptions.ashx?sid=" + Radom + "&RegionID=" + RegionID, AfterBranchesByRegion);
        }

        function AfterBranchesByRegion(data) {

            if (data.ExecResult == "Failed") {

                alert(data.ErrorMsg);
                return;
            }

            $("#" + ddlBranch).empty();
            $("#" + ddlBranch).append(data.Options);
            $("#" + ddlBranch).attr('disabled', '');
        }

        function ddlDivision_onchange() {

            $("#" + ddlBranch).val('');

            var DivisionID = $("#" + ddlDivision).val();

            GetBranches(DivisionID);
        }

        function GetBranches(DivisionID) {

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("GetBranchOptions.ashx?sid=" + Radom + "&DivisionID=" + DivisionID, AfterGetBranches);
        }

        function AfterGetBranches(data) {

            if (data.ExecResult == "Failed") {

                alert(data.ErrorMsg);
                return;
            }

            $("#" + ddlBranch).empty();
            $("#" + ddlBranch).append(data.Options);
            $("#" + ddlBranch).attr('disabled', '');
        }

        //#endregion

        //#region Name Link

        function GoToLoanDetails(FileID) {

            var FileIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridActiveLoanList tr td a").each(function (i) {

                var FileID = $(this).attr("myFileID");
                if (i == 0) {

                    FileIDs = FileID;
                }
                else {

                    FileIDs += "," + FileID;
                }
            });

            window.location.href = '../LoanDetails/LoanDetails.aspx?FromPage=' + encodeURIComponent(window.location.href) + '&fieldid=' + FileID + '&fieldids=' + FileIDs;
        }

        function GoToProspectLoanDetails(FileID) {

            var FileIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridOpportunityList tr td a").each(function (i) {

                var FileID = $(this).attr("myFileID");
                if (i == 0) {

                    FileIDs = FileID;
                }
                else {

                    FileIDs += "," + FileID;
                }
            });

            window.location.href = '../Prospect/ProspectLoanDetails.aspx?FromPage=' + encodeURIComponent(window.location.href) + '&FileID=' + FileID + '&FileIDs=' + FileIDs;
        }

        function GoToArchivedLoanDetails(FileID) {

            var FileIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridArchivedLoanList tr td a").each(function (i) {

                var FileID = $(this).attr("myFileID");
                if (i == 0) {

                    FileIDs = FileID;
                }
                else {

                    FileIDs += "," + FileID;
                }
            });

            window.location.href = '../LoanDetails/LoanDetails.aspx?FromPage=' + encodeURIComponent(window.location.href) + '&fieldid=' + FileID + '&fieldids=' + FileIDs;
        }

        //#endregion

        function GoToPartnerCompanySetup(CompanyID) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "../Contact/PartnerCompanyEdit.aspx?sid=" + sid + "&CompanyID=" + CompanyID;
        }

        function GoToPartnerContactDetails(ContactID) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "../Contact/PartnerContactDetailView.aspx?sid=" + sid + "&ContactID=" + ContactID;
        }

        function GoToProspectDetails(ProspectID) {

            var c = "ContactID=" + ProspectID + "&ContactIDs=" + ProspectID;
            var e = $.base64Encode(c);

            window.location.href = "../Prospect/ProspectDetailView.aspx?PageFrom=" + encodeURIComponent(window.location.href) + "&e=" + e;
        }
// ]]>
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer">
        <div id="divModuleName" class="Heading" style="padding-left: 0px;">Advanced Search</div>
        <div class="SplitLine"></div>
        <div id="divSearchCriteria" style="margin-top: 20px; margin-left: 20px;">
            <div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodeSearchCriteria" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;" onclick="ExpandTreeNode_SearchCriteria()" />
                        </td>
                        <td style="font-weight: bold;">Search Criteria</td>
                    </tr>
                </table>
            </div>
            <div id="divSearchCriteriaContainer" style="margin-top: 5px; border: solid 1px #d6d9e0; padding: 10px; width: 800px;">
                <table>
                    <tr>
                        <td>
                            <input id="chkSearchLoans" type="checkbox" checked /><label for="chkSearchLoans"> Search Loans</label>
                        </td>
                        <td style="padding-left: 20px;">
                            <input id="chkSearchOpportunities" type="checkbox" checked /><label for="chkSearchOpportunities"> Search Leads</label>
                        </td>
                        <td style="padding-left: 20px;">
                            <input id="chkSearchArchivedLoans" type="checkbox" checked /><label for="chkSearchArchivedLoans"> Search Archived Loans</label>
                        </td>
                        <td style="padding-left: 20px;">
                            <input id="chkSearchClients" type="checkbox" checked /><label for="chkSearchClients"> Search Clients</label>
                        </td>
                        <td style="padding-left: 20px;">
                            <input id="chkSearchPartners" type="checkbox" checked /><label for="chkSearchPartners"> Search Partners</label>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 10px;">
                    <tr>
                        <td style="width: 80px;">Name:</td>
                        <td style="width: 250px;">
                            <input id="txtName" type="text" style="width: 200px;" />
                        </td>
                        <td style="width: 80px;">Company:</td>
                        <td>
                            <input id="txtCompany" type="text" style="width: 200px;" />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 80px;">Address:</td>
                        <td style="width: 250px;">
                            <input id="txtAddress" type="text" style="width: 200px;" />
                        </td>
                        <td style="width: 80px;">City:</td>
                        <td style="width: 125px;">
                            <input id="txtCity" type="text" style="width: 105px;" />
                        </td>
                        <td>State:</td>
                        <td>
                            <asp:DropDownList ID="ddlState" runat="server">
                                <asp:ListItem Text="All" Value=""></asp:ListItem>
                                <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                                <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                                <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                                <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                                <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                                <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                                <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                                <asp:ListItem Text="DC" Value="DC"></asp:ListItem>
                                <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                                <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                                <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                                <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                                <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                                <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                                <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                                <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                                <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                                <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                                <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                                <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                                <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                                <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                                <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                                <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                                <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                                <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                                <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                                <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                                <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                                <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                                <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                                <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                                <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                                <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                                <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                                <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                                <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                                <asp:ListItem Text="PR" Value="PR"></asp:ListItem>
                                <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                                <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                                <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                                <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                                <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                                <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                                <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                                <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                                <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                                <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                                <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td style="width: 80px;">Email:</td>
                        <td style="width: 250px;">
                            <input id="txtEmail" type="text" style="width: 200px;" />
                        </td>
                        <td>Phone:</td>
                        <td>
                            <input id="txtPhone" type="text" style="width: 200px;" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 80px;">Loan Number:</td>
                        <td>
                            <input id="txtLoanNumber" type="text" style="width: 200px;" />
                        </td>
                        <td>Filename:</td>
                        <td>
                            <input id="txtFilename" type="text" style="width: 200px;" />
                        </td>
                    </tr>
                    <tr>
                        <td>Loan Officer:</td>
                        <td>
                            <asp:DropDownList ID="ddlLoanOfficer" runat="server" DataValueField="UserId" DataTextField="FullName" Width="203px">
                            </asp:DropDownList>
                        </td>
                        <td>Processor:</td>
                        <td>
                            <asp:DropDownList ID="ddlProcessor" runat="server" DataValueField="UserId" DataTextField="FullName" Width="203px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Region:</td>
                        <td>
                            <asp:DropDownList ID="ddlRegion" runat="server" DataValueField="RegionId" DataTextField="Name" Width="203px">
                            </asp:DropDownList>
                        </td>
                        <td>Division:</td>
                        <td>
                            <asp:DropDownList ID="ddlDivision" runat="server" DataValueField="DivisionId" DataTextField="Name" Width="203px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Branch:</td>
                        <td>
                            <asp:DropDownList ID="ddlBranch" runat="server" DataValueField="BranchId" DataTextField="Name" Width="203px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 80px;">Service Type:</td>
                        <td>
                            <asp:DropDownList ID="ddlServiceType" runat="server" DataValueField="ServiceTypeId" DataTextField="Name" Width="203px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <table style="margin-top: 10px;">
                    <tr>
                        <td>
                            <input id="btnSearch" type="button" value="Search" class="Btn-66" onclick="btnSearch_onclick()" />
                        </td>
                        <td>
                            <input id="btnClear" type="button" value="Clear" class="Btn-66" onclick="btnClear_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        
        <div id="divSearchResult" style="margin-top: 20px;  margin-left: 20px;">
            <div id="divSearchResultTile" runat="server" style="font-weight: bold;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 20px;">
                            <img id="imgNodeSearchResults" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;" onclick="ExpandTreeNode_SearchResults()" />
                        </td>
                        <td style="font-weight: bold;">Search Results</td>
                    </tr>
                </table>
            </div>
            <div id="divSearchResultContainer" runat="server" style="margin-left: 20px; width: 800px;">
                <div id="divActiveLoansContainer" runat="server" style="margin-top: 20px;">
                    <table>
                        <tr>
                            <td style="width: 20px;">
                                <img id="imgNodeActiveLoans" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;" onclick="ExpandTreeNode_ActiveLoans()" />
                            </td>
                            <td style="width: 80px;">Active Loans</td>
                            <td>
                                <a id="aPipeline_ActiveLoans" runat="server" style="text-decoration: underline; color: blue;">Pipeline</a>
                            </td>
                        </tr>
                    </table>
                    <div id="divPagerAndGrid_ActiveLoans">
                        <div style="text-align: right; padding-right: 5px;">
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex1" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </div>
                        <div id="divActiveLoanList" class="ColorGrid" style="margin-top: 5px;">
                            <asp:GridView ID="gridActiveLoanList" runat="server" DataSourceID="ActiveLoanSqlDataSource" EmptyDataText="There is no loan by criteria." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Borrower" SortExpression="FullName">
                                        <ItemTemplate>
                                            <a href="javascript:GoToLoanDetails('<%# Eval("FileId")%>')" myFileID="<%# Eval("FileId")%>"><%# Eval("FullName")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="LoanAmount" HeaderText="Amount" SortExpression="LoanAmount" ItemStyle-Width="120px" DataFormatString="{0:c}" />
                                    <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" ItemStyle-Width="100px" DataFormatString="{0:n3}" />
                                    <asp:BoundField DataField="LienPosition" HeaderText="Lien" SortExpression="LienPosition" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Program" HeaderText="Program" SortExpression="Program" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="CurrentStage" HeaderText="Stage" SortExpression="CurrentStage" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        <asp:SqlDataSource ID="ActiveLoanSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
                            <SelectParameters>
                                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="FullName" />
                                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select c.LastName +', '+ c.FirstName + case when ISNULL(c.MiddleName, '') != '' then ' '+ c.MiddleName else '' end as FullName, a.*,b.ContactId from Loans as a inner join LoanContacts as b on a.FileId = b.FileId inner join Contacts as c on b.ContactId = c.ContactId where b.ContactRoleId=1 and a.Status='Processing') as t" />
                                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="10" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>

                <div id="divOpportunitiesContainer" runat="server" style="margin-top: 20px;">
                    <table>
                        <tr>
                            <td style="width: 20px;">
                                <img id="imgNodeOpportunities" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;" onclick="ExpandTreeNode_Opportunities()" />
                            </td>
                            <td style="width: 80px;">Leads</td>
                            <td>
                                <a id="aPipeline_Opportunities" runat="server" style="text-decoration: underline; color: blue;">Pipeline</a>
                            </td>
                        </tr>
                    </table>
                    <div id="divPagerAndGrid_Opportunities">
                        <div style="text-align: right; padding-right: 5px;">
                            <webdiyer:AspNetPager ID="AspNetPager2" runat="server" PageSize="10" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex2" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </div>
                        <div id="divOpportunityList" class="ColorGrid" style="margin-top: 5px;">
                            <asp:GridView ID="gridOpportunityList" runat="server" DataSourceID="OpportunitySqlDataSource" EmptyDataText="There is no loan by criteria." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Borrower" SortExpression="FullName">
                                        <ItemTemplate>
                                            <a href="javascript:GoToProspectLoanDetails('<%# Eval("FileId")%>')" myFileID="<%# Eval("FileId")%>"><%# Eval("FullName")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="LoanAmount" HeaderText="Amount" SortExpression="LoanAmount" ItemStyle-Width="120px" DataFormatString="{0:c}" />
                                    <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" ItemStyle-Width="100px" DataFormatString="{0:n3}" />
                                    <asp:BoundField DataField="LienPosition" HeaderText="Lien" SortExpression="LienPosition" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Program" HeaderText="Program" SortExpression="Program" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="CurrentStage" HeaderText="Stage" SortExpression="CurrentStage" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        <asp:SqlDataSource ID="OpportunitySqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
                            <SelectParameters>
                                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="FullName" />
                                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select c.LastName +', '+ c.FirstName + case when ISNULL(c.MiddleName, '') != '' then ' '+ c.MiddleName else '' end as FullName, a.*,b.ContactId from Loans as a inner join LoanContacts as b on a.FileId = b.FileId inner join Contacts as c on b.ContactId = c.ContactId where b.ContactRoleId=1 and a.Status='Prospect') as t" />
                                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                                <asp:ControlParameter ControlID="AspNetPager2" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                                <asp:ControlParameter ControlID="AspNetPager2" DefaultValue="10" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>

                <div id="divArchivedLoansContainer" runat="server" style="margin-top: 20px;">
                    <table>
                        <tr>
                            <td style="width: 20px;">
                                <img id="imgNodeArchivedLoans" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;" onclick="ExpandTreeNode_ArchivedLoans()" />
                            </td>
                            <td style="width: 80px;">Archived Loans</td>
                            <td>
                                <a id="aPipeline_ArchivedLoans" runat="server" style="text-decoration: underline; color: blue;">Pipeline</a>
                            </td>
                        </tr>
                    </table>
                    <div id="divPagerAndGrid_ArchivedLoans">
                        <div style="text-align: right; padding-right: 5px;">
                            <webdiyer:AspNetPager ID="AspNetPager3" runat="server" PageSize="10" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex3" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </div>
                        <div id="divArchivedLoanList" class="ColorGrid" style="margin-top: 5px;">
                            <asp:GridView ID="gridArchivedLoanList" runat="server" DataSourceID="ArchivedLoanSqlDataSource" EmptyDataText="There is no loan by criteria." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Borrower" SortExpression="FullName">
                                        <ItemTemplate>
                                            <a href="javascript:GoToArchivedLoanDetails('<%# Eval("FileId")%>')" myFileID="<%# Eval("FileId")%>"><%# Eval("FullName")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="LoanAmount" HeaderText="Amount" SortExpression="LoanAmount" ItemStyle-Width="120px" DataFormatString="{0:c}" />
                                    <asp:BoundField DataField="Rate" HeaderText="Rate" SortExpression="Rate" ItemStyle-Width="100px" DataFormatString="{0:n3}" />
                                    <asp:BoundField DataField="LienPosition" HeaderText="Lien" SortExpression="LienPosition" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Program" HeaderText="Program" SortExpression="Program" ItemStyle-Width="150px" />
                                    <asp:BoundField DataField="CurrentStage" HeaderText="Stage" SortExpression="CurrentStage" ItemStyle-Width="100px" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        <asp:SqlDataSource ID="ArchivedLoanSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
                            <SelectParameters>
                                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="FullName" />
                                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select c.LastName +', '+ c.FirstName + case when ISNULL(c.MiddleName, '') != '' then ' '+ c.MiddleName else '' end as FullName, a.*,b.ContactId from Loans as a inner join LoanContacts as b on a.FileId = b.FileId inner join Contacts as c on b.ContactId = c.ContactId where b.ContactRoleId=1 and a.Status<>'Processing' and a.Status<>'Prospect') as t" />
                                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                                <asp:ControlParameter ControlID="AspNetPager3" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                                <asp:ControlParameter ControlID="AspNetPager3" DefaultValue="10" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>

                <div id="divClientsContainer" runat="server" style="margin-top: 20px;">
                    <table>
                        <tr>
                            <td style="width: 20px;">
                                <img id="imgNodeClients" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;" onclick="ExpandTreeNode_Clients()" />
                            </td>
                            <td style="width: 80px;">Clients</td>
                        </tr>
                    </table>
                    <div id="divPagerAndGrid_Clients">
                        <div style="text-align: right; padding-right: 5px;">
                            <webdiyer:AspNetPager ID="AspNetPager4" runat="server" PageSize="10" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex4" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </div>
                        <div id="divClientList" class="ColorGrid" style="margin-top: 5px;">
                            <asp:GridView ID="gridClientList" runat="server" EmptyDataText="There is no client by criteria." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Name" SortExpression="FullName" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <a href="javascript:GoToProspectDetails('<%# Eval("ContactId")%>')" ContactID="<%# Eval("ContactId")%>"><%# Eval("FullName")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DOB" HeaderText="DOB" SortExpression="DOB" ItemStyle-Width="100px" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" />
                                    <asp:BoundField DataField="HomePhone" HeaderText="Home Phone" SortExpression="HomePhone" ItemStyle-Width="120px" />
                                    <asp:BoundField DataField="CellPhone" HeaderText="Cell Phone" SortExpression="CellPhone" ItemStyle-Width="120px" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        
                    </div>
                </div>

                <div id="divPartnersContainer" runat="server" style="margin-top: 20px;">
                    <table>
                        <tr>
                            <td style="width: 20px;">
                                <img id="imgNodePartners" src="../images/CompanyOverview/accordion.gif" style="cursor: pointer;" style="cursor: pointer;" onclick="ExpandTreeNode_Partners()" />
                            </td>
                            <td style="width: 80px;">Partners</td>
                        </tr>
                    </table>
                    <div id="divPagerAndGrid_Partners">
                        <div style="text-align: right; padding-right: 5px;">
                            <webdiyer:AspNetPager ID="AspNetPager5" runat="server" PageSize="10" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex5" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </div>
                        <div id="divPartnerList" class="ColorGrid" style="margin-top: 5px;">
                            <asp:GridView ID="gridPartnerList" runat="server" DataSourceID="PartnerSqlDataSource" EmptyDataText="There is no Partner by criteria." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Company" SortExpression="CompanyName">
                                        <ItemTemplate>
                                            <a href="javascript:GoToPartnerCompanySetup('<%# Eval("ContactCompanyId2")%>')" CompanyID="<%# Eval("ContactCompanyId2")%>"><%# Eval("CompanyName")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ServiceType" HeaderText="Service Type" SortExpression="ServiceType" ItemStyle-Width="100px" />
                                    <asp:TemplateField HeaderText="Name" SortExpression="FullName" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <a href="javascript:GoToPartnerContactDetails('<%# Eval("ContactId")%>')" ContactID="<%# Eval("ContactId")%>"><%# Eval("FullName")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Email" HeaderText="Email" SortExpression="Email" ItemStyle-Width="200px" />
                                    <asp:BoundField DataField="BusinessPhone" HeaderText="Business Phone" SortExpression="BusinessPhone" ItemStyle-Width="120px" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        <asp:SqlDataSource ID="PartnerSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
                            <SelectParameters>
                                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="CompanyName" />
                                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                                <asp:Parameter Name="DbTable" Type="String" DefaultValue="" />
                                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                                <asp:ControlParameter ControlID="AspNetPager5" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                                <asp:ControlParameter ControlID="AspNetPager5" DefaultValue="10" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>