<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailsAlertTab.aspx.cs" Inherits="LoanDetails_LoanDetailsAlertTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Loan Details - Alerts</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.effects.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.effects.slide.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

// <![CDATA[

        $(document).ready(function () {

            DrawTab();
            
            // init filter
            InitFilter();

            // init grid
            InitGrid();

            // lock
            LockAlertDetails();

            // event
            $("#ddlEmailSelector").change(ddlEmailSelector_onchange);
        });

        function btnFilter_onclick() {

            var QueryString = "";
            var LoanID = GetQueryString1("LoanID");
            QueryString = "ref=loan&LoanID=" + LoanID;

            // Filter
            var Filter = $("#ddlFilter").val();
            if (Filter != "All") {

                QueryString += "&Filter=" + Filter;
            }

            window.location.href = window.location.pathname + "?" + QueryString;
        }

        function InitFilter() {

            // Filter
            var Filter = GetQueryString1("Filter");
            if (Filter != "") {
                $("#ddlFilter").val(Filter);
            }
        }

        function InitGrid() {

            $("#gridAlertList tr:not(:first)").mousedown(function () {

                $("#gridAlertList").find("tr").attr("class", "NormalRow");

                $(this).attr("class", "FocusedRow");

                var MyTextArea = $(this).find("textarea");
                ShowAlertDetails(MyTextArea, "left");

            });
        }

        function ShowAlertDetails(MyTextArea, Direction) {

            $("#divRightContent").show();

            //#region show alert details

            // status
            var myStatus = MyTextArea.attr("myStatus");
            $("#lbStatus").text(myStatus);

            // Acknowledged by
            if (myStatus == "") {

                $("#tbAlertInfo1").hide();

                $("#lbOnTitle").text("Created On:");

                var CreatedDate = MyTextArea.attr("myDateCreated");
                $("#lbOnDate").text(CreatedDate);
            }
            else if (myStatus == "Acknowledged") {

                $("#tbAlertInfo1").show();

                $("#lbByTitle").text("Acknowledged By:");

                var AcknowledgedByName = MyTextArea.attr("myAcknowledgedByName");
                $("#lbByName").text(AcknowledgedByName);

                $("#lbOnTitle").text("Acknowledged On:");

                var AcknowledgedDate = MyTextArea.attr("myAcknowledgedDate");
                $("#lbOnDate").text(AcknowledgedDate);
            }
            else if (myStatus == "Accepted") {

                $("#tbAlertInfo1").show();

                $("#lbByTitle").text("Accepted By:");

                var AcceptedByName = MyTextArea.attr("myAcceptedByName");
                $("#lbByName").text(AcceptedByName);

                $("#lbOnTitle").text("Accepted On:");

                var AcceptedDate = MyTextArea.attr("myAcceptedDate");
                $("#lbOnDate").text(AcceptedDate);
            }
            else if (myStatus == "Declined") {

                $("#tbAlertInfo1").show();

                $("#lbByTitle").text("Declined By:");

                var DeclinedByName = MyTextArea.attr("myDeclinedByName");
                $("#lbByName").text(DeclinedByName);

                $("#lbOnTitle").text("Declined On:");

                var DeclinedDate = MyTextArea.attr("myDeclinedDate");
                $("#lbOnDate").text(DeclinedDate);
            }
            else if (myStatus == "Dismissed") {

                $("#tbAlertInfo1").show();

                $("#lbByTitle").text("Dismissed By:");

                var DismissedByName = MyTextArea.attr("myDismissedByName");
                $("#lbByName").text(DismissedByName);

                $("#lbOnTitle").text("Dismissed On:");

                var DismissedDate = MyTextArea.attr("myDismissedDate");
                $("#lbOnDate").text(DismissedDate);
            }

            // due date
            var DueDate = MyTextArea.attr("myDueDate");
            $("#lbDueDate").text(DueDate);

            // alert icon
            var AlertIconFileName = MyTextArea.attr("myAlertIconFileName");
            if (AlertIconFileName == "") {

                $("#imgAlertIcon").hide();
            }
            else {

                $("#imgAlertIcon").attr("src", "../images/alert/" + AlertIconFileName);
                $("#imgAlertIcon").show();
            }

            // desc
            var myDesc = MyTextArea.attr("myDesc");
            var myDesc_Decode = $.base64Decode(myDesc);
            $("#lbAlertDesc").text(myDesc_Decode);

            // email content
            $("#ddlEmailSelector").val("Alert Email Content");
            ShowEmailContent(MyTextArea, "Alert Email Content")

            //#endregion

            //#region slide

            if (Direction == "left") {

                SlideLeft();
            }
            else {

                SlideRight();
            }

            //#endregion

            //#region enable/disable link buttons

            if (myStatus == "") {

                EnableLink("aAcknowledge", "javascript:aAcknowledge_onclick()");
                EnableLink("aDismiss", "javascript:aDismiss_onclick()");
                EnableLink("aAccept", "javascript:aAccept_onclick()");
                EnableLink("aDecline", "javascript:aDecline_onclick()");
            }
            else if (myStatus == "Acknowledged") {

                DisableLink("aAcknowledge");
                DisableLink("aDismiss");

                EnableLink("aAccept", "javascript:aAccept_onclick()");
                EnableLink("aDecline", "javascript:aDecline_onclick()");
            }
            else {

                DisableLink("aAcknowledge");
                DisableLink("aDismiss");
                DisableLink("aAccept");
                DisableLink("aDecline");
            }

            //#endregion
        }

        //#region slide effect

        function SlideLeft() {
            // most effect types need no options passed by default
            var options = { direction: 'right' };

            // run the effect
            $("#divAlertDetails").show("slide", options, 1000, callback);
        };

        function SlideRight() {
            // most effect types need no options passed by default
            var options = { direction: 'left' };

            // run the effect
            $("#divAlertDetails").show("slide", options, 1000, callback);
        };

        //callback function to bring a hidden box back
        function callback() {


        };

        function LockAlertDetails() {

            $("#divRightContent").hide();
//            $("#tbAlertInfo1").hide();
//            $("#ddlEmailSelector").attr("disabled", true);
//            $('#divRightContent').block({ message: null, overlayCSS: { backgroundColor: '#bcbdbe', opacity: 0.2} });
        }

        //#endregion

        //#region Last/Next button

        function aLast_onclick() {

            var FocusedIndex = $("#gridAlertList tr[class='FocusedRow']").index();

            var NextIndex = 0;
            if (FocusedIndex == 1) {

                NextIndex = $("#gridAlertList tr").length - 1;
            }
            else {

                NextIndex = FocusedIndex - 1
            }

            FocusAlertRow(NextIndex, "right");
        }

        function aNext_onclick() {

            var FocusedIndex = $("#gridAlertList tr[class='FocusedRow']").index();

            var NextIndex = 0;
            if (FocusedIndex == ($("#gridAlertList tr").length - 1)) {

                NextIndex = 1;
            }
            else {

                NextIndex = FocusedIndex + 1
            }

            FocusAlertRow(NextIndex, "left");
        }

        function FocusAlertRow(index, direction) {

            $("#gridAlertList").find("tr").attr("class", "NormalRow");
            $("#gridAlertList tr").eq(index).attr("class", "FocusedRow");
            var MyTextArea = $("#gridAlertList tr").eq(index).find("textarea");
            ShowAlertDetails(MyTextArea, direction);
        }

        //#endregion

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {

            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(SuccessMsg);
            $('#aClose').show();
        }

        //#region button click

        function aAcknowledge_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Acknowledging Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = $("#gridAlertList tr[class='FocusedRow']").find("textarea").attr("myAlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Acknowledge&AlertID=" + AlertID, AfterAcknowledge);
        }

        function AfterAcknowledge(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Acknowledged successfully.')", 2000);
        }

        function aDismiss_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Dismissing Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = $("#gridAlertList tr[class='FocusedRow']").find("textarea").attr("myAlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Dismiss&AlertID=" + AlertID, AfterDismiss);
        }

        function AfterDismiss(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Dismissed alert successfully.')", 2000);
        }

        function aAccept_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Accepting Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = $("#gridAlertList tr[class='FocusedRow']").find("textarea").attr("myAlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Accept&AlertID=" + AlertID, AfterAccept);
        }

        function AfterAccept(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Accepted alert successfully.')", 2000);
        }

        function aDecline_onclick() {

            // show waiting dialog
            ShowWaitingDialog("Declining Alert...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var AlertID = $("#gridAlertList tr[class='FocusedRow']").find("textarea").attr("myAlertID");
            var LoginUserID = $("#hdnLoginUserID").val();

            // Ajax
            $.getJSON("AlertAction_Background.aspx?sid=" + Radom + "&Action=Decline&AlertID=" + AlertID, AfterDecline);
        }

        function AfterDecline(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Declined alert successfully.')", 2000);
        }

        //#endregion

        //#region show email content

        function ddlEmailSelector_onchange(EmailType) {

            var MyTextArea = $("#gridAlertList tr[class='FocusedRow']").find("textarea");
            var EmailType = $("#ddlEmailSelector").val();
            ShowEmailContent(MyTextArea, EmailType);
        }

        function ShowEmailContent(MyTextArea, EmailType) {

            var AlertEmail = "";
            if (EmailType == "Alert Email Content") {

                AlertEmail = MyTextArea.attr("myAlertEmail");
            }
            else {

                AlertEmail = MyTextArea.attr("myRecomEmail");
            }

            var AlertEmail_Decode = $.base64Decode(AlertEmail);
            $("#divEmailContent").html(AlertEmail_Decode);
        }

        //#endregion

        function RefreshPage() {

            if ($.browser.msie == true) {   // ie

                window.parent.document.frames("ifrLoanInfo").location.reload();
            }
            else {   // firefox

                window.parent.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
            }
            
            // refresh current page
            window.location.href = window.location.href;
        }

        //#region CR48 fake tab navigation

        function aTasks_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsTask.aspx?sid=" + sid + "&LoanID=" + LoanID + "&ref=" + sRef;
        }

        function aAlerts_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsAlertTab.aspx?sid=" + sid + "&LoanID=" + LoanID + "&ref=" + sRef;
        }

        function aEmails_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../Prospect/LoanDetailEmailTab.aspx?from=2&itemid=" + LoanID + "&sid=" + sid + "&ref=" + sRef;

        }

        function aNotes_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanNoteList.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }

        function aActivityHistory_onclick() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/Activity.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }


        //#endregion

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">

    <div class="JTab" style="margin-top:10px;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                         <%
                             bool isOnlytab = false;

                             //string rUrl = Request.UrlReferrer.AbsoluteUri;

                             //if (rUrl.Contains("LoanDetail.aspx"))
                             //{
                             //    isOnlytab = true;
                             //}
                             if (this.Request.QueryString["ref"] != null && this.Request.QueryString["ref"].ToString() == "loan")
                             {
                                 isOnlytab = true;
                             }
                         %>
                        <ul>
                            <%if (!isOnlytab)
                              { %>
                            <li sid="otherTab"><a id="aTasks" href="javascript:aTasks_onclick()"><span>Tasks</span></a></li>
                            <%} %>
                            
                            <li id="current"><a id="aAlerts" href="javascript:aAlerts_onclick()"><span>Rule Alerts</span></a></li>
                            
                            <%if (!isOnlytab)
                              { %>
                            <li sid="otherTab"><a id="aEmails" href="javascript:aEmails_onclick()"><span>Emails</span></a></li>
                            <li sid="otherTab"><a id="aNotes" href="javascript:aNotes_onclick()"><span>Notes</span></a></li>
                            <li sid="otherTab"><a id="aActivityHistory" href="javascript:aActivityHistory_onclick()">
                                <span>Activity History</span></a></li>
                            <%} %>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <div id="TabBody" style="margin-bottom:10px; padding-bottom:10px;">
            <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
            <div id="TabLine2" class="TabRightLine">&nbsp;</div>
            <div class="TabContent">


    <div id="divContainer" style="width: 980px; height: 605px; border: solid 0px red; overflow: auto;">
        <table cellpadding="0" cellspacing="0" style="width: 100%">
            <tr>
                <td style="vertical-align: top; width: 450px; padding-right: 15px;">
                	<div id="divLeftAlertList" style="border: solid 1px #e4e7ef; padding: 5px 15px 5px 15px; height: 509px;">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <select id="ddlFilter">
                                        <option>All</option>
                                        <option>Pending</option>
                                        <option>Acknowledged</option>
                                        <option>Dismissed</option>
                                        <option>Accepted</option>
                                        <option>Declined</option>
                                    </select>
                                </td>
                                <td style="padding-left: 10px;">
                                    <input id="btnFilter" type="button" value="Filter" class="Btn-66" onclick="btnFilter_onclick()" />
                                </td>
                            </tr>
                        </table>
                        <div id="divAlertList" class="ColorGrid" style="margin-top: 10px;">
                            <asp:GridView ID="gridAlertList" runat="server" DataSourceID="AlertSqlDataSource" EmptyDataText="There is no alert in current loan." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundField DataField="DateCreated" HeaderText="Created" ItemStyle-Width="60px" HtmlEncode="false" DataFormatString="{0:MM/dd/yyyy}" />
                                    <asp:TemplateField HeaderText="Alert Desc" >
                                        <ItemTemplate>
                                            <%# this.PrintImage(Eval("AlertIconFileName").ToString())%> <span style="position: relative; top: -3px;"><%# Eval("Desc") %></span>
                                            <textarea id="hdnAlertInfo" style="display: none;" cols="20" rows="2" myAlertIconFileName="<%# Eval("AlertIconFileName") %>" myAlertID="<%# Eval("LoanAlertId") %>" myLoanID="<%# Eval("FileId") %>" 
                                            myDesc="<%# this.EncodeText(Eval("Desc").ToString()) %>" myDueDate="<%# this.FormatDateTime(Eval("DueDate").ToString()) %>" 
                                            myClearedBy="<%# Eval("ClearedBy") %>" myClearedDate="<%# this.FormatDateTime(Eval("Cleared").ToString()) %>" 
                                            myAcknowlegeReq="<%# Eval("AcknowlegeReq") %>" myAcknowledgedBy="<%# Eval("AcknowledgedBy") %>" 
                                            myAcknowledgedDate="<%# this.FormatDateTime(Eval("Acknowledged").ToString()) %>" myLoanRuleId="<%# Eval("LoanRuleId") %>" 
                                            myOwnerId="<%# Eval("OwnerId") %>" myLoanTaskId="<%# Eval("LoanAlertId") %>" myAlertType="<%# Eval("AlertType") %>" 
                                            myDateCreated="<%# this.FormatDateTime(Eval("DateCreated").ToString()) %>" myStatus="<%# Eval("Status") %>" 
                                            myAcceptedDate="<%# this.FormatDateTime(Eval("Accepted").ToString()) %>" myDeclinedDate="<%# this.FormatDateTime(Eval("Declined").ToString()) %>" 
                                            myDismissedDate="<%# this.FormatDateTime(Eval("Dismissed").ToString()) %>" myAcceptedBy="<%# Eval("AcceptedBy") %>" myDeclinedBy="<%# Eval("DeclinedBy") %>" 
                                            myDismissedBy="<%# Eval("DismissedBy") %>" myAlertEmail="<%# this.EncodeText(Eval("AlertEmail").ToString()) %>" 
                                            myRecomEmail="<%# this.EncodeText(Eval("RecomEmail").ToString()) %>"
                                            myAcknowledgedByName="<%# Eval("AcknowledgedByName") %>" myAcceptedByName="<%# Eval("AcceptedByName") %>"
                                            myDeclinedByName="<%# Eval("DeclinedByName") %>" myDismissedByName="<%# Eval("DismissedByName") %>"></textarea>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="75px" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        <asp:SqlDataSource ID="AlertSqlDataSource" runat="server" DataSourceMode="DataReader">
                        </asp:SqlDataSource>
                    </div>
                </td>
                <td style="vertical-align: top; border-left: solid 0px #e4e7ef;">
                    <div id="divRightContent" style=" width:600px;">
                        <table id="divLastNext" cellpadding="0" cellspacing="0" style="margin-left: 10px; margin-top: 5px;">
                            <tr>
                                <td>
                                    <a id="aLast" href="javascript:aLast_onclick();"><img alt="" src="../images/ico_16_l.gif" /></a>
                                </td>
                                <td style="padding-left: 10px;">
                                    <a id="aNext" href="javascript:aNext_onclick();"><img alt="" src="../images/ico_16_r.gif" /></a>
                                </td>
                            </tr>
                        </table>
                        <div id="divAlertDetails" style="margin-top: 5px; border: solid 2px #e4e7ef; padding: 5px; padding-left: 10px; padding-bottom: 10px; padding-right: 10px; height: 473px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div id="divModuleName" class="ModuleTitle" style="margin-top: 0px; margin-left: 5px;">Alert Details</div>
                                    </td>
                                    <td style="padding-left: 15px;">
                                        <ul class="ToolStrip" style="margin-left: 0px;">
                                            <li><a id="aAcknowledge" href="javascript:aAcknowledge_onclick()">Acknowledge</a><span>|</span></li>
                                            <li><a id="aDismiss" href="javascript:aDismiss_onclick()">Dismiss</a><span>|</span></li>
                                            <li><a id="aAccept" href="javascript:aAccept_onclick()">Accept</a><span>|</span></li>
                                            <li><a id="aDecline" href="javascript:aDecline_onclick()">Decline</a></li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                            <div class="SplitLine" style="margin-top: 2px;"></div>
                            <table id="tbAlertInfo1" cellpadding="0" cellspacing="0" style="margin-top: 10px;">
                                <tr>
                                    <td>Status:</td>
                                    <td style="padding-left: 10px;">
                                        <span id="lbStatus"></span>
                                    </td>
                                    <td style="padding-left: 50px;"><span id="lbByTitle">By:</span></td>
                                    <td style="padding-left: 10px;"><span id="lbByName"></span></td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
                                <tr>
                                    <td><span id="lbOnTitle">On:</span></td>
                                    <td style="padding-left: 10px;"><span id="lbOnDate"></span></td>
                                    <td style="padding-left: 40px;">Due Date:</td>
                                    <td style="padding-left: 10px;"><span id="lbDueDate"></span></td>
                                </tr>
                            </table>
                            <table cellpadding="0" cellspacing="0" style="margin-top: 15px;">
                                <tr>
                                    <td style="width: 65px;">View Email:</td>
                                    <td>
                                        <select id="ddlEmailSelector">
                                            <option>Alert Email Content</option>
                                            <option>Recommended Email Content</option>
                                        </select>
                                    </td>
                            
                                </tr>
                            </table>
                            <div style="margin-top: 15px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <img id="imgAlertIcon" alt="AlertIcon" src="../images/alert/Alert-Gray.gif" />
                                        </td>
                                        <td style="padding-left: 8px;">
                                            <span id="lbAlertDesc" style="position: relative; top: -1px;">&lt;Alert Description&gt;</span></td>
                                    </tr>
                                </table>
                            </div>
                            <div id="divEmailContent" style="height: 300px; margin-top: 3px; padding: 10px; overflow: auto; border: solid 1px #e4e7ef;">
                                
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>

            </div>
        </div>
    </div>

    <div id="divWaiting" style="display: none; padding: 5px;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>&nbsp;&nbsp;
					<a id="aClose" href="javascript:RefreshPage()" style="font-weight: bold; color: #6182c1;">[Close]</a>
				</td>
			</tr>
		</table>
	</div>
    <input id="hdnLoginUserID" runat="server" type="text" style="display: none;" />
    <input id="hdnParentForm" runat="server" type="text" style="display: none;" />
    </form>
</body>
</html>
