<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Role Setup" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="Settings_RoleSetup" CodeBehind="RoleSetup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .td-center
        {   
            text-align: center;
        }
        .td-center2
        {
            width: 50px;
            text-align: center;
        }
        
<%--        .rptList tr td { padding:20px; margin:10px }--%>
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <script type="text/javascript">
        $().ready(function () {
            $("span[tag='pip'] input").each(function () {
                $(this).bind("click", { obj: this }, checkHandler);
            });
        });

        function checkHandler(event) {
            if (event.data.obj.checked) {
                $("span[tag='pip'] input").each(function () {
                    $(this).not(event.data.obj).attr("checked", false);
                });
            }
        }

        function checkAllContact() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_checkAllContact").attr("checked") == true) {
                $("#tblContact tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#tblContact tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function checkAllTask() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_checkAllTask").attr("checked") == true) {
                $("#tblTask tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#tblTask tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function checkAllLoan() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_checkAllLoan").attr("checked") == true) {
                $("#tblLoan tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#tblLoan tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function checkAllProspect() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_checkAllProspect").attr("checked") == true) {
                $("#tblProspect tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#tblProspect tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function checkAllContactMgr() {
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_checkAllContactMgr").attr("checked") == true) {
                $("#tblContactMgr tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#tblContactMgr tr td input[type=checkbox]").attr("checked", "");
            }
        }
        function checkAllMarketing() {
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_chkMarketingSltAll").attr("checked") == true) {
                $("#tblMarketing tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#tblMarketing tr td input[type=checkbox]").attr("checked", "");
            }
        }

    </script>
    <div class="Heading">
        Role Setup</div>
    <div class="SplitLine">
    </div>
    <div class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Role Name
                    </td>
                    <td colspan="2" style="padding-left: 10px;">
                        <asp:DropDownList ID="ddlRoleName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRoleName_SelectedIndexChanged"
                            Width="200px">
                            <%--<asp:ListItem Text="Loan officer" Value="Loan officer"></asp:ListItem>
                            <asp:ListItem Text="Loan officer assistant" Value="Loan officer assistant"></asp:ListItem>
                            <asp:ListItem Text="Processor" Value="Processor"></asp:ListItem>
                            <asp:ListItem Text="Underwriter" Value="Underwriter"></asp:ListItem>
                            <asp:ListItem Text="Closer" Value="Closer"></asp:ListItem>
                            <asp:ListItem Text="Doc Prep" Value="Doc Prep"></asp:ListItem>
                            <asp:ListItem Text="Shipper" Value="Shipper"></asp:ListItem>
                            <asp:ListItem Text="Manager" Value="Manager"></asp:ListItem>
                            <asp:ListItem Text="Executive" Value="Executive"></asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Access company setup screens
                    </td>
                    <td colspan="2" style="padding-top: 9px; padding-left: 10px;">
                        <asp:CheckBox ID="cbxAccCompany" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Access loan setup screens
                    </td>
                    <td colspan="2" style="padding-top: 9px; padding-left: 10px;">
                        <asp:CheckBox ID="cbxAccLoan" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Access rights
                    </td>
                    <td colspan="1" style="padding-top: 9px; padding-left: 8px;">
                        <asp:RadioButtonList ID="rblAccRights" runat="server" RepeatDirection="Horizontal"
                            Width="310px">
                            <asp:ListItem Text="&nbsp;&nbsp;All loans" Value="All loans"></asp:ListItem>
                            <asp:ListItem Text="&nbsp;&nbsp;Assigned loans" Value="Assigned loans"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td style="padding-top: 9px; padding-left: 8px;">
                        <asp:CheckBox ID="cbxUnassignedLeads" runat="server" Text=" Unassigned Leads" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Customize user homepage
                    </td>
                    <td colspan="2" style="padding-top: 9px; padding-left: 10px;">
                        <asp:CheckBox ID="cbxCusUserHomePage" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Set Production Goals
                    </td>
                    <td colspan="2" style="padding-top: 9px; padding-left: 8px;">
                        <asp:RadioButtonList ID="rblSetProductionGoals" runat="server" RepeatDirection="Horizontal"
                            Width="218px">
                            <asp:ListItem Text="&nbsp;&nbsp;All user's goals" Value="0"></asp:ListItem>
                            <asp:ListItem Text="&nbsp;&nbsp;Own goals" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Access reports
                    </td>
                    <td colspan="2" style="padding-top: 9px; padding-left: 10px;">
                        <asp:CheckBox ID="cbxAccessReports" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <%--<div class="DashedBorder" style="margin-top:15px; border-top:solid; border-top-width:thin;">
            &nbsp;</div>--%>
        <table id="tblTitles">
            <tr>
                <td width="430px">
                    <div id="divModuleName" class="ModuleTitle" style="padding-left: 0px; padding-top: 8px;">
                        Task & Alert Rights&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="checkAllTask" runat="server"
                            onclick="javascript:checkAllTask()" />&nbsp;select all</div>
                </td>
                <td>
                    <div id="div1" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                        Loan Management Rights&nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="checkAllLoan" runat="server"
                            onclick="javascript:checkAllLoan()" />&nbsp;select all</div>
                </td>
            </tr>
        </table>
        <div class="DashedBorder" style="margin-top: 8px;">
            &nbsp;</div>
        <table id="tblCotent">
            <tr>
                <td>
                    <div style="margin-top: 3px;">
                        <table cellpadding="2" cellspacing="0" id="tblTask">
                            <tr>
                                <td>
                                </td>
                                <td class="td-center2" style="padding-left: 15x;">
                                    Create
                                </td>
                                <td class="td-center2">
                                    Modify
                                </td>
                                <td class="td-center2">
                                    Delete
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Workflow Templates
                                </td>
                                <td class="td-center2" style="padding-left: 15px;">
                                    <asp:CheckBox ID="cbxWfCreate" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxWfModify" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxWfDel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Loan Tasks
                                </td>
                                <td class="td-center2" style="padding-left: 15px;">
                                    <asp:CheckBox ID="cbxLoanCreate" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxLoanModify" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxLoanDel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Alert Rules
                                </td>
                                <td class="td-center2" style="padding-left: 15px;">
                                    <asp:CheckBox ID="cbxAlertRulesCreate" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxAlertRulesModify" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxAlertRulesDel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Alert Rule Groups
                                </td>
                                <td class="td-center2" style="padding-left: 15px;">
                                    <asp:CheckBox ID="cbxAlertRuleTempCreate" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxAlertRuleTempModify" runat="server" />
                                </td>
                                <td class="td-center2">
                                    <asp:CheckBox ID="cbxAlertRuleTempDel" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Mark Tasks Complete
                                </td>
                                <td colspan="3" style="padding-left: 33px;">
                                    <%-- <asp:RadioButtonList ID="rblMarkTasks" runat="server" RepeatDirection="Horizontal"
                                        Width="100px">
                                        <asp:ListItem Text="All tasks" Value="All tasks"></asp:ListItem>
                                        <asp:ListItem Text="Assigned tasks" Value="Assigned tasks"></asp:ListItem>
                                    </asp:RadioButtonList>--%>
                                    <table width="237px">
                                        <tbody>
                                            <tr>
                                                <td width="10px">
                                                    <asp:RadioButton ID="rbMarkTasks_All" runat="server" GroupName="markTasks" />
                                                </td>
                                                <td style="padding-left: 2px;">
                                                    All tasks
                                                </td>
                                                <td align="right" width="40px">
                                                    <asp:RadioButton ID="rbMarkTasks_Ass" runat="server" GroupName="markTasks" />
                                                </td>
                                                <td style="padding-left: 2px;">
                                                    Assigned tasks
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Assign tasks
                                </td>
                                <td colspan="3" style="padding-left: 36px;">
                                    <asp:CheckBox ID="cbxAssignTasks" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                <td valign="top" style="width: 231px" align="left">
                    <div style="margin-top: 3px; padding-left: 62px;">
                        <table cellpadding="2" cellspacing="0" id="tblLoan">
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="cbxImportLoans" runat="server" />
                                </td>
                                <td style="padding-left: 2px;">
                                    Import loans from Point
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:CheckBox ID="cbxRemoveLoans" runat="server" />
                                </td>
                                <td style="padding-left: 2px;">
                                    Remove loans from pipeline
                                </td>
                            </tr>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="cbxLoanReassignment" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Loan reassignment
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="cbxApplyWf" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Apply workflow
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="cbxApplyAlertRule" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Apply alert rule
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="cbxSendMail" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Send email
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="cbxCreateNotes" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Create notes
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="cbxSendLSR" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Send LSR
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="cbxExportPipelines" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Export Pipelines
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:CheckBox ID="chkUpdateCondition" runat="server" />
                </td>
                <td style="padding-left: 2px;">
                    Change Condition status
                </td>
            </tr>
        </table>
    </div>
    </td> </tr> </table>
    <div class="DashedBorder" style="margin-top: 15px; border-top:solid; border-top-width:thin;">
        &nbsp;</div>
        
    <table id="Table2">
        <tr>
            <td>
                <div id="div7" class="ModuleTitle" style="padding-left: 0px; padding-top: 6px;">
                    Lock and Profitability &nbsp;&nbsp;&nbsp;&nbsp;</div>
            </td>
            
            <td align="left" style="padding-top: 15px;">
                <asp:CheckBox ID="cbxViewLockInfo" runat="server" />
            </td>
            <td style="padding-left: 2px;padding-top: 15px;">
                View Lock Info &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td align="left" style="padding-top: 15px;">
                <asp:CheckBox ID="cbxLockRate" runat="server" />
            </td>
            <td style="padding-left: 2px;padding-top: 15px;">
                Lock Rate &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td align="left" style="padding-top: 15px;">
                <asp:CheckBox ID="cbxExtendRateLock" runat="server" />
            </td>
            <td style="padding-left: 2px;padding-top: 15px;">
                ExtendRateLock
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td align="left" style="padding-top: 15px;">
                <asp:CheckBox ID="cbxAccessProfitability" runat="server" />
            </td>
            <td style="padding-left: 2px;padding-top: 15px;">
                Access Profitability &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td align="left" style="padding-top: 15px;">
                <asp:CheckBox ID="cbxViewCompensation" runat="server" />
            </td>
            <td style="padding-left: 2px;padding-top: 15px;">
                View Compensation &nbsp;&nbsp;&nbsp;&nbsp;
            </td>
            <td align="left" style="padding-top: 15px;">
            </td>
            <td style="padding-left: 2px;padding-top: 15px;">
            </td>
        </tr>
    </table>

    <div class="DashedBorder" style="margin-top: 15px; border-top:solid; border-top-width:thin;">
        &nbsp;</div>
    <table id="Table3">
        <tr>
            <td>
                <div id="div4" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                    Prospect Management Rights &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="checkAllProspect"
                        runat="server" onclick="javascript:checkAllProspect()" />&nbsp;select all</div>
            </td>
        </tr>
    </table>
    <div class="DashedBorder" style="margin-top: 8px;">
        &nbsp;</div>
    <table id="Table4">
        <tr>
            <td>
                <div style="margin-top: 3px;">
                    <table cellpadding="2" cellspacing="0" id="tblProspect">
                        <tr>
                            <td rowspan="5" style="width: 135px">
                                Prospect Contact Record
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectCreate" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Modify
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectDelete" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectView" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectSearch" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Search
                            </td>

                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectExport" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Export
                            </td>

                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectDispose" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Dispose
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectImport" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Import Point (Sync)
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectUpdatePoint" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Update Point
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectAssign" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Assign
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectMerge" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Merge
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLinkLoans" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Link Loans
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectViewNotes" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View Notes
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectAddNotes" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Add Notes
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectViewEmails" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                ViewEmails
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectSendEmails" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Send Emails
                            </td>
                        </tr>
                        <tr>
                            <td colspan="13" style="height: 10px">
                                <div class="DashedBorder" style="margin: 8px;">&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="5">
                                Prospect Loan
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanCreate" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Modify
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanDelete" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanView" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanSearch" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Search
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanDispose" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Dispose
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanSync" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Sync
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanViewNotes" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View Notes
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanAddNotes" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Add Notes
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanViewEmails" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                ViewEmails
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxProspectLoanSendEmails" runat="server" />
                            </td>
                            <td style="padding-left: 4px;" colspan="10">
                                Send Emails
                            </td>
                        </tr>
                        <tr>
                            <td colspan="13" style="height: 4px">
                                <div class="DashedBorder" style="margin: 8px;">&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="6">
                                Conditions
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxConditionsCreate" runat="server" Enabled="false" />
                            </td>
                            <td style="padding-left: 4px;">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxConditionsModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Modify
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxConditionsDel" runat="server" Enabled="false" />
                            </td>
                            <td style="padding-left: 4px;">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxConditionsClear" runat="server" Enabled="false" />
                            </td>
                            <td style="padding-left: 4px;">
                                Clear
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxConditionsAssign" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Assign
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxEnableLSR" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Enable LSR Viewing for external contacts
                            </td>
                        </tr>
                    </table>

                </div>
            </td>
        </tr>
    </table>
    <%--<div class="DashedBorder" style="margin-top: 15px;border-top:solid; border-top-width:thin; ">
        &nbsp;</div>
    <table id="tbMarketingSelectAll">
        <tr>
            <td>
                <div id="div3" class="ModuleTitle" style="padding-left: 0px; padding-top: 8px;">
                    Marketing Campaign Rights &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chkMarketingSltAll"
                        runat="server" onclick="javascript:checkAllMarketing()" />&nbsp;select all</div>
            </td>
        </tr>
    </table>
    <div class="DashedBorder" style="margin-top: 8px;">
        &nbsp;</div>
    <table id="tbMarketing">
        <tr>
            <td>
                <div style="margin-top: 3px;">
                    <table cellpadding="2" cellspacing="0" id="tblMarketing">
                        <tr>
                            <td rowspan="6" style="width: 135px">
                                Marketing Campaigns
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="chkAddMarketing" runat="server" />
                            </td>
                            <td style="padding-left: 4px; width: 68px;">
                                Add
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="chkRemoveMarketing" runat="server" />
                            </td>
                            <td style="padding-left: 4px; width: 68px;">
                                Remove
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="chkVewMarketing" runat="server" />
                            </td>
                            <td style="padding-left: 4px; width: 68px;">
                                View
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>--%>
    <div class="DashedBorder" style="margin-top: 8px;border-top:solid; border-top-width:thin;">
        &nbsp;</div>
    <table id="Table1">
        <tr>
            <td>
                <div id="div6" class="ModuleTitle" style="padding-left: 0px; padding-top: 8px;">
                    MailChimp Rights &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbxAccessAllMailChimpList"
                        runat="server" Text="" />&nbsp;&nbsp;Access all MailChimp Lists</div>
            </td>
        </tr>
    </table>
    <div class="DashedBorder" style="margin-top: 15px; border-top:solid; border-top-width:thin;">
        &nbsp;</div>
    <table id="Table5">
        <tr>
            <td>
                <div id="div5" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                    Contact Management Rights &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="checkAllContactMgr"
                        runat="server" onclick="javascript:checkAllContactMgr()" />&nbsp;select all</div>
            </td>
        </tr>
    </table>
    <div class="DashedBorder" style="margin-top: 8px;">
        &nbsp;</div>
    <table id="Table6">
        <tr>
            <td>
                <div style="margin-top: 3px;">
                    <table cellpadding="2" cellspacing="0" id="tblContactMgr">
                        <tr>
                            <td style="padding-top: 9px;">
                                Access All Contacts
                            </td>
                            <td colspan="11" style="padding-left: 4px;">
                                <asp:CheckBox ID="cbxAccessAllContacts" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="12" style="height: 4px">
                                <div class="DashedBorder" style="margin: 8px; ">&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3" style="width: 135px">
                                Contact Records
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxContactView" runat="server" />
                            </td>
                            <td style="padding-left: 4px; width: 68px">
                                View
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxContactCreate" runat="server" />
                            </td>
                            <td style="padding-left: 4px; width: 68px">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxContactModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px; width: 68px">
                                Modify
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxContactDelete" runat="server" />
                            </td>
                            <td style="padding-left: 4px; width: 68px">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxContactReassign" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Assign
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxContactMerge" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Merge
                            </td>

                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxContactExport" runat="server" />
                            </td>
                            <td style="padding-left: 4px;" colspan="7">
                                Export
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                                <div class="DashedBorder" style="margin: 8px;">&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3" style="width: 135px">
                                Contact Company
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTCompanyView" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTCompanyCreate" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTCompanyModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Modify
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTCompanyDelete" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTCompanyDisable" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Disable
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTCompanyAddBranches" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Add Branches
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTCompanyRemoveBranches" runat="server" />
                            </td>
                            <td style="padding-left: 4px;" colspan="7">
                                Remove Branches
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            <div class="DashedBorder" style="margin: 8px;">&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="3">
                                Contact Branch
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTBranchView" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTBranchCreate" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTBranchModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Modify
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTBranchDelete" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTBranchDisable" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Disable
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTBranchAddContacts" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Add Contacts
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTBranchRemoveContacts" runat="server" />
                            </td>
                            <td style="padding-left: 4px;" colspan="7">
                                Remove Contacts
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            <div class="DashedBorder" style="margin: 8px; ">&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="1">
                                Service Type
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTServiceView" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTServiceCreate" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTServiceModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Enable
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTServiceDelete" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTServiceDisable" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Disable
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            <div class="DashedBorder" style="margin: 8px;">&nbsp;</div>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                Contact Role
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTRoleView" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                View
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTRoleCreate" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Create
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTRoleModify" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Enable
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTRoleDelete" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Delete
                            </td>
                            <td style="padding-left: 4px; width: 10px">
                                <asp:CheckBox ID="cbxCTRoleDisable" runat="server" />
                            </td>
                            <td style="padding-left: 4px;">
                                Disable
                            </td>
                        </tr>
                        <tr>
                            <td colspan="11" style="height: 4px">
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <div class="DashedBorder" style="margin-top: 8px; border-top:solid; border-top-width:thin;">
        &nbsp;</div>
    <table>
        <tr>
            <td>
                <div id="div2" style="padding-left: 0px; padding-top: 15px; width: 136px">
                    <label>
                        <b>Homepage Selections</b></label>
                    <br />
                    (pick up to six)</div>
            </td>
            <td>
                <div style="margin-top: 3px;">
                    <table cellpadding="2" cellspacing="0">
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbxCompanyCalendar" runat="server" />
                            </td>
                            <td style="padding-left: 2px;" width="283px">
                                Company calendar
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxRatesSummary" runat="server" />
                            </td>
                            <td style="padding-left: 2px;">
                                Rates summary
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbxPipelineChart" runat="server" Text="" tag="pip" />
                            </td>
                            <td style="padding-left: 2px;">
                                Pipeline chart
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxGoalsChart" runat="server" Text="" />
                            </td>
                            <td style="padding-left: 2px;">
                                Goals chart
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbxPipelineSummaryWithSales" runat="server" Text="" tag="pip" />
                            </td>
                            <td style="padding-left: 2px;">
                                Pipeline summary with sales breakdown chart
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxOverdueTasks" runat="server" Text="" />
                            </td>
                            <td style="padding-left: 2px;">
                                Overdue tasks and alert summary
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbxPipelineSummaryWithOrg" runat="server" Text="" tag="pip" />
                            </td>
                            <td style="padding-left: 2px;">
                                Pipeline summary with organizational production chart
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxCompanyAnn" runat="server" Text="" />
                            </td>
                            <td style="padding-left: 2px;">
                                Company announcements
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" colspan="2">
                                <asp:CheckBox ID="cbxPipelineSummaryWithOrgAndSales" runat="server" Text="" tag="pip" />&nbsp;Pipeline
                                summary with organizational production chart
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;& sales breakdown chart
                            </td>
                            <td>
                                <asp:CheckBox ID="cbxExchangeInbox" runat="server" />
                            </td>
                            <td style="padding-left: 2px;">
                                Exchange inbox
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:CheckBox ID="cbxExchangeCalendar" runat="server" />
                            </td>
                            <td style="padding-left: 2px;">
                                Exchange calendar
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <div style="margin-top: 20px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" class="Btn-66" />
                </td>
                <td style="padding-left: 8px;">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"
                        class="Btn-66" />
                </td>
            </tr>
        </table>
    </div>
    </div>
    <div id="divContent" style="margin-top: 15px;">
    </div>
</asp:Content>
