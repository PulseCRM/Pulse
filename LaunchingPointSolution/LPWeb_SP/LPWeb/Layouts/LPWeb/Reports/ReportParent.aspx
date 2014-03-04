<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Title="Goals Reports" Language="C#" AutoEventWireup="true" CodeBehind="ReportParent.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Reports.ReportParent" MasterPageFile="~/_layouts/LPWeb/MasterPage/Home.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        $(document).ready(function () {
            InitFilter();
        });

        function InitFilter() {
            // ReportTypeID
            var ReportTypeID = GetQueryString1("ReportTypeID");
            if (ReportTypeID != "") {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlReportType").val(ReportTypeID);

                SetIFrameSrc(ReportTypeID);
            }

            // Region
            var Region = GetQueryString1("Region");
            if (Region != "") {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegions").val(Region);
            }

            // Division
            var Division = GetQueryString1("Division");
            if (Division != "") {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivisions").val(Division);
            }

            // Branch
            var Branch = GetQueryString1("Branch");
            if (Branch != "") {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranches").val(Branch);
            }
        }

        function BuildQueryString() {
            var ReportTypeID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlReportType").val();
            var QueryString = "?ReportTypeID=" + ReportTypeID;

            // region
            var SelRegionID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegions").val();
            if (SelRegionID != undefined && SelRegionID != "-1") {
                QueryString += "&Region=" + SelRegionID;
            }

            // division
            var SelDivisionID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivisions").val();
            if (SelDivisionID != undefined && SelDivisionID != "-1") {
                QueryString += "&Division=" + SelDivisionID;
            }

            // branch
            var SelBranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranches").val();
            if (SelBranchID != undefined && SelBranchID != "-1") {
                QueryString += "&Branch=" + SelBranchID;
            }
            return QueryString;
        }

        function btnDisplay_onclick() {
            var ReportTypeID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlReportType").val();
            window.location.href = window.location.pathname + "?ReportTypeID=" + ReportTypeID;
        }

        function btnFilter_onclick() {
            var ReportTypeID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlReportType").val();
            SetIFrameSrc(ReportTypeID);
            //window.location.href = window.location.pathname + BuildQueryString();
        }

        function SetIFrameSrc(ReportTypeID) {
            var Src = "";
            if (ReportTypeID == "1") {
                Src = "ReportRegionProduction.aspx";
            }
            else if (ReportTypeID == "2") {
                Src = "ReportDivisionProduction.aspx";
            }
            else if (ReportTypeID == "3") {
                Src = "ReportBranchProduction.aspx";
            }
            else if (ReportTypeID == "4") {
                Src = "ReportUserProduction.aspx";
            }
            else if (ReportTypeID == "5") {
                Src = "ReportPointPipeline.aspx";
            }

            var QueryString = "";

            // region
            var SelRegionID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegions").val();
            if (SelRegionID == undefined || SelRegionID == "-1") {
                QueryString += "&Region=-1";
            }
            else {
                QueryString += "&Region=" + SelRegionID;
            }

            // division
            var SelDivisionID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivisions").val();
            if (SelDivisionID == undefined || SelDivisionID == "-1") {
                QueryString += "&Division=-1";
            }
            else {
                QueryString += "&Division=" + SelDivisionID;
            }

            // branch
            var SelBranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranches").val();
            if (SelBranchID == undefined || SelBranchID == "-1") {
                QueryString += "&Branch=-1";
            }
            else {
                QueryString += "&Branch=" + SelBranchID;
            }

            if (QueryString != "") {
                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString();
                $("#ifReportList").attr("src", Src + "?sid=" + RadomStr + QueryString);
            }
            else {
                $("#ifReportList").attr("src", Src);
            }
        }
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" Runat="Server">
    <div id="divReportType">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>Report</td>
                <td style="padding-left: 10px;">
                    <asp:DropDownList ID="ddlReportType" runat="server" Width="300px">
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 10px;">
                    <input id="btnDisplay" type="button" value="Display" class="Btn-66" onclick="btnDisplay_onclick()" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divFilters" style="margin-top: 10px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlRegions" runat="server" DataValueField="RegionID" DataTextField="Name" Width="150px">
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 10px;">
                    <asp:DropDownList ID="ddlDivisions" runat="server" DataValueField="DivisionID" DataTextField="Name" Width="150px">
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 10px;">
                    <asp:DropDownList ID="ddlBranches" runat="server" DataValueField="BranchID" DataTextField="Name" Width="150px">
                    </asp:DropDownList>
                </td>
                <td style="padding-left: 10px;">
                    <input id="btnFilter" runat="server" type="button" value="Filter" class="Btn-66" onclick="btnFilter_onclick()" />
                </td>           
            </tr>
        </table>                   
    </div>
    
    <div id="divReportList" style="margin-top: 10px;">
        <iframe id="ifReportList" src="about:blank" frameborder="0" scrolling="no" width="100%" height="100%"></iframe>
    </div>
</asp:Content>

