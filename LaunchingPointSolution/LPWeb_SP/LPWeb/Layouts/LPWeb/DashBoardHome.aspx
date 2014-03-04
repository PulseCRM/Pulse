<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Dashboard Home for User" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Home.master"
    AutoEventWireup="true" Inherits="DashBoardHome" CodeBehind="DashBoardHome.aspx.cs" %>

<%@ Register src="~/_controltemplates/LPWeb/HomeAlertWebPart/HomeAlertWebPartUserControl.ascx" tagname="AlertWebPart" tagprefix="uc1" %>

<%@ Register TagPrefix="WpNs0" Namespace="Microsoft.SharePoint.Portal.WebControls" Assembly="Microsoft.SharePoint.Portal, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="js/jquery.datepick.js" type="text/javascript"></script>
    <script src="js/jquery.base64.js" type="text/javascript"></script>
    <script src="js/urlparser.js" type="text/javascript"></script>
    <script src="js/date.js" type="text/javascript"></script>
    <script src="js/jquery.formatCurrency.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        $(document).ready(function () {

            InitFilter();

            // calendar
            $('#ctl00_ctl00_PlaceHolderMain_MainArea_txtFromDate').datepick({
                onSelect: txtFromDate_onselect
            });
            $('#ctl00_ctl00_PlaceHolderMain_MainArea_txtToDate').datepick({
                onSelect: txtToDate_onselect
            });

            // add change event
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegions").change(ddlRegions_onchange);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivisions").change(ddlDivisions_onchange);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranches").change(ddlBranches_onchange);
            $(".chkAmount").click(chkAmount_onclick);
            $("#ddlWorkflowType").change(ddlWorkflowType_onchange);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_AlertWebPart1_ddlDueDateFilter").change(ddlDueDateFilter_onchange);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStageFilter").change(ddlStageFilter_onchange);

            CalcTotalAmount();

        });

        function InitFilter() {

            // Region
            var Region = GetQueryString1("Region");
            if (Region != "") {
                var RegionID = $.base64Decode(Region);
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegions").val(parseInt(RegionID));
            }

            // Division
            var Division = GetQueryString1("Division");
            if (Division != "") {
                var DivisionID = $.base64Decode(Division);
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivisions").val(parseInt(DivisionID));
            }

            // Branch
            var Branch = GetQueryString1("Branch");
            if (Branch != "") {
                var BranchID = $.base64Decode(Branch);
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranches").val(parseInt(BranchID));
            }

            // Date Type
            var DateType = GetQueryString1("DateType");
            if (DateType != "") {
                var DateType_Dcode = $.base64Decode(DateType);
                $("#ddlDateFilter").val(DateType_Dcode);
            }

            // date range
            var FromDate = GetQueryString1("FromDate");
            if (FromDate != "") {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFromDate").val($.base64Decode(FromDate));
            }
            var ToDate = GetQueryString1("ToDate");
            if (ToDate != "") {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtToDate").val($.base64Decode(ToDate));
            }

            // Workflow Type
            var WorkflowType = GetQueryString1("WorkflowType");
            if (WorkflowType != "") {
                var WorkflowType_Dcode = $.base64Decode(WorkflowType);
                if (WorkflowType_Dcode.indexOf("Processing") != -1) {

                    $("#ddlWorkflowType").val("Processing");
                }
                else if (WorkflowType_Dcode.indexOf("Prospect") != -1) {

                    $("#ddlWorkflowType").val("Prospect");
                }
                else if (WorkflowType_Dcode.indexOf("Archived") != -1) {

                    $("#ddlWorkflowType").val("Archived");
                }
            }

            // DueDate
            var DueDate = GetQueryString1("DueDate");
            if (DueDate != "") {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_AlertWebPart1_ddlDueDateFilter").val(DueDate);
            }

            // StageFilter
            var StageFilter = GetQueryString1("StageFilter");
            if (StageFilter != "") {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStageFilter").val(StageFilter);

//                if (StageFilter == "LastCompletedStages") {

//                    $("#tbLoanAnalysis a").removeAttr("href");
//                    $("#tbLoanAnalysis a").css("text-decoration", "none");
//                }
            }
        }

        function CalcTotalAmount() {

            var Total = 0.00;
            var TotalCounts = 0;

            $("#tbLoanAnalysis :checkbox:checked").each(function (i) {

                var AmountText = $(this).attr("title");
                var Amount = new Number(AmountText);
                Total = Total + Amount;


                var CountsText = $(this).attr("LoanCounts");
                var Counts = new Number(CountsText);
                TotalCounts = TotalCounts + Counts;
            });

            $("#spanTotal").text("$" + Total.toString());

            $('#spanTotal').formatCurrency();

            $("#spanTotalCounts").text(TotalCounts.toString());
            //$('#spanTotalCounts').formatCurrency();

        }

        function ddlRegions_onchange() {

            BuildQueryString();
        }

        function ddlDivisions_onchange() {

            BuildQueryString();
        }

        function ddlBranches_onchange() {

            BuildQueryString();
        }

        function txtFromDate_onselect(date) {

            BuildQueryString();
        }

        function txtToDate_onselect(date) {

            BuildQueryString();
        }

        function BuildQueryString() {

            var QueryString = "";

            // region
            var SelRegionID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlRegions").val();
            if (SelRegionID != "-1") {

                QueryString += "&Region=" + $.base64Encode(SelRegionID);
            }

            // division
            var SelDivisionID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivisions").val();
            if (SelDivisionID != "-1") {

                QueryString += "&Division=" + $.base64Encode(SelDivisionID);
            }

            // branch
            var SelBranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlBranches").val();
            if (SelBranchID != "-1") {

                QueryString += "&Branch=" + $.base64Encode(SelBranchID);
            }

            // date type
            var DateType = $("#ddlDateFilter").val();
            if (DateType != "") {

                QueryString += "&DateType=" + $.base64Encode(DateType);
            }

            //#region from date/to date

            var FromDate = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFromDate").val();
            var ToDate = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtToDate").val();

            if (FromDate != "") {
//                if (isDate(FromDate, "MM/dd/yyyy") == false) {
//                    alert("please enter a valid date，e.g." + formatDate(new Date(), 'MM/dd/yyyy') + ".");
//                    return;
//                }
                if (DateType == "") {

                    alert("Please select date type.");
                    return;
                }
                QueryString += "&FromDate=" + $.base64Encode(FromDate);
            }

            if (ToDate != "") {
//                if (isDate(ToDate, "MM/dd/yyyy") == false) {
//                    alert("please enter a valid date，e.g." + formatDate(new Date(), 'MM/dd/yyyy') + ".");
//                    return;
//                }
                if (DateType == "") {

                    alert("Please select date type.");
                    return;
                }
                QueryString += "&ToDate=" + $.base64Encode(ToDate);
            }

            //#endregion

            // workflow type
            var WorkflowType = $("#ddlWorkflowType").val();
            QueryString += "&WorkflowType=" + $.base64Encode(WorkflowType);

            // DueDate
            var DueDate = $("#ctl00_ctl00_PlaceHolderMain_MainArea_AlertWebPart1_ddlDueDateFilter").val();
            if (DueDate != "") {

                QueryString += "&DueDate=" + DueDate;
            }

            // StageFilter
            var StageFilter = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStageFilter").val();
            if (StageFilter != "") {

                QueryString += "&StageFilter=" + StageFilter;
            }

            if (QueryString != "") {

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);

                window.location.href = window.location.pathname + "?sid=" + RadomStr + QueryString;
            }
            else {

                window.location.href = window.location.pathname
            }
        }

        function chkAmount_onclick() {

            CalcTotalAmount();
        }

        function GoToLoanDetails(FileID) {

            window.location.href = "LoanDetails/LoanDetails.aspx?FromPage=" + encodeURIComponent(window.location.href) + "&fieldid=" + FileID + "&fieldids=" + FileID;
        }

        function ddlWorkflowType_onchange() {

            BuildQueryString();
        }

        function ddlDueDateFilter_onchange() {

            BuildQueryString();
        }

        function ddlStageFilter_onchange() {

            BuildQueryString();
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#tbLoanAnalysis tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#tbLoanAnalysis tr td :checkbox").attr("checked", "");
            }
            chkAmount_onclick();
        }
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" Runat="Server">
    <table style="width: 100%;">
        <tr>
            <td style="width: 360px; vertical-align: top;">
                
                <uc1:AlertWebPart ID="AlertWebPart1" runat="server" />

                <div id="divEmailInbox" runat="server" class="Widget" style="width: 350px; margin-bottom: 9px;">
                    <div class="Widget_Header">
                        <div>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="wTitle">Email Inbox</td>
                                    <td><a href="<%= ConfigurationManager.AppSettings["EmailInboxUrl"] %>" target="_blank"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="Widget_Body" style="overflow: auto; padding-left: 12px; padding-right: 12px;">
                        <asp:PlaceHolder ID="phInbox" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
                
                <div id="divComCal" runat="server" class="Widget" style="width: 350px; margin-bottom: 9px;">
                    <div class="Widget_Header">
                        <div>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="wTitle">Company  Calendar</td>
                                    <td><a href="<%=strComCalListUrl %>" target="_self"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="Widget_Body" style="padding-left: 12px; padding-right: 12px;">
                        <asp:PlaceHolder runat="server" ID="phComCal"></asp:PlaceHolder>
                    </div>
                </div>

                <div id="divRates" runat="server" class="Widget" style="width: 350px;">
                    <div class="Widget_Header">
                        <div>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="wTitle">Rates  Summary</td>
                                    <td><a href="<%=strRatesListUrl %>" target="_self"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="Widget_Body" style="padding-left: 12px; padding-right: 12px;">
                        <asp:PlaceHolder runat="server" ID="phRates"></asp:PlaceHolder>
                    </div>
                </div>

                <div id="divQuickLead" runat="server" class="Widget" style="width: 350px;">
                    <div class="Widget_Header">
                        <div>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="wTitle">Quick Lead</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="Widget_Body" style="padding-left: 12px; padding-right: 12px; padding-top:5px;">
                        
                        <iframe frameborder="0" scrolling="no" width="335px" height="500px" src="QuickLeadForm.aspx"></iframe>
                    
                    </div>
                </div>

            </td>
            <td style="vertical-align: top;">
                <div class="Widget" style="width: 560px; margin-bottom: 9px;">
                    <div class="Widget_Header">
                        <div>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="wTitle">Pipeline Summary</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="Widget_Body" style="padding-top: 8px; padding-left: 12px; padding-right: 12px; vertical-align: top;">
                        <div id="divFilters" runat="server">
                            
                            <div>
                                <select id="ddlWorkflowType" style="width: 150px">
                                    <option value="Prospect">Leads</option>
                                    <option value="Processing" selected="selected">Active Loans</option>
                                    <option value="Archived">Archived Loans</option>
                                </select>
                            </div>
                            
                            <table cellpadding="0" cellspacing="0" style="margin-top: 6px;">
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
                                
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="margin-top: 5px;">
                                <tr>
                                    <td>
                                        <select id="ddlDateFilter" style="width: 150px;">
                                            <option value="CloseDate">Close Date</option>
                                            <option value="OpenDate">Open Date</option>
                                        </select>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:TextBox ID="txtFromDate" runat="server" Width="140px" CssClass="iTextBox"></asp:TextBox>
                                    </td>
                                    <td style="padding-left: 10px;">
                                        <asp:TextBox ID="txtToDate" runat="server" Width="140px" CssClass="iTextBox"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <table cellpadding="0" cellspacing="0" border="0" style="margin-top: 8px;">
                            <tr>
                                <td style="width: 250px; vertical-align: top;">

                                    <div id="divPipelineAnalysis" runat="server">
                                        
                                        <table>
                                        	<tr>
                                        		<td>
                                        			<input type="checkbox" checked="checked" id="SelectAll" onclick="CheckAll(this)" /><label for="SelectAll"> Select All</label>
                                        		</td>
                                        		<td>
                                                    <select id="ddlStageFilter" runat="server">
                                                        <option value="CurrentStages">Current Stages</option>
                                                        <option value="LastCompletedStages">Last Completed Stages</option>
                                                    </select>
                                        		</td>
                                        	</tr>	
                                        </table>

                                        <table id="tbLoanAnalysis" cellpadding="0" cellspacing="0" style="width: 100%; border: solid 1px #f7f8fa;">
                                            <asp:Repeater ID="rptLoanAnalysis" runat="server">
                                                <ItemTemplate>
                                                    <tr style="background-color: #f7f8fa; height: 22px;">
                                                        <td style="width: 30px; padding-left: 8px;">
                                                            <input id="chkStage" type="checkbox" myStageAlias="<%# Eval("StageAlias") %>" title="<%# Eval("Amount") %>" LoanCounts="<%# Eval("LoanCounts") %>"   class="chkAmount" checked />
                                                        </td>
                                                        <td style="width: 110px;"><a id="a<%# Eval("StageAlias") %>" href="<%# Eval("Href") %>"><%# Eval("StageAlias") %></a></td>
                                                        <td style="width: 110px; text-align:center;"><%# Eval("LoanCounts")%></td>
                                                        <td class="tdAmount">$<%# Convert.ToDecimal(Eval("Amount")).ToString("n")%></td>
                                                    </tr>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <tr style="height: 22px;">
                                                        <td style="width: 30px; padding-left: 8px;">
                                                            <input id="chkStage" type="checkbox" myStageAlias="<%# Eval("StageAlias") %>" title="<%# Eval("Amount") %>" LoanCounts="<%# Eval("LoanCounts") %>"   class="chkAmount" checked />
                                                        </td>
                                                        <td style="width: 110px;"><a id="a<%# Eval("StageAlias") %>" href="<%# Eval("Href") %>"><%# Eval("StageAlias") %></a></td>
                                                        <td style="width: 110px; text-align:center;"><%# Eval("LoanCounts")%></td>
                                                        <td class="tdAmount">$<%# Convert.ToDecimal(Eval("Amount")).ToString("n")%></td>
                                                    </tr>
                                                </AlternatingItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                        <div style="border: solid 1px #f7f8fa; border-top: none;">
                                            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr style="height: 22px;">
                                                    <td style="width: 36px; padding-left: 3px;">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 110px; font-weight: bold;">
                                                        Total:
                                                    </td>
                                                    <td style="font-weight: bold;width: 50px;">
                                                        <span id="spanTotalCounts">0</span>
                                                    </td>
                                                    <td style="font-weight: bold;">
                                                        <span id="spanTotal">$00.00</span>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                    
                                </td>
                                <td style="vertical-align: top; width: 280px; padding-left: 5px;">
                                    <div id="divUserGoals" runat="server">
                                        <iframe id="ifUserGoals" src="AnyChart/UserGoals.aspx" frameborder="0" scrolling="no" width="280px" height="260px"></iframe>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="vertical-align: top;">
                            <div id="divCalendar" runat="server" class="Widget" style="width: 275px;">
                                <div class="Widget_Header">
                                    <div>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="wTitle">Calendar Summary</td>
                                                <td><a href="<%= ConfigurationManager.AppSettings["MyCalendarUrl"] %>" target="_blank"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="Widget_Body" style="overflow: auto; padding-left: 12px; padding-right: 12px;">
                                    <asp:PlaceHolder ID="phMyCalendar" runat="server"></asp:PlaceHolder>
                                </div>
                            </div>
                        </td>
                        <td style="vertical-align: top; padding-left: 8px;">
                            <div id="divCompanyAnn" runat="server" class="Widget" style="width: 275px;">
                                <div class="Widget_Header">
                                    <div>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td class="wTitle">Company  Announcements</td>
                                                <td><a href="<%=strComAnnListUrl %>" target="_self"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                                <div class="Widget_Body" style="overflow: auto; padding-left: 12px; padding-right: 12px;">
                                    <asp:PlaceHolder runat="server" ID="phComAnn"></asp:PlaceHolder>
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
