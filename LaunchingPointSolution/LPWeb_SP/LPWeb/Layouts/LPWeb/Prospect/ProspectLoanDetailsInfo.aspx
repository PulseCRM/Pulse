<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectLoanDetailsInfo.aspx.cs"
    Inherits="Prospect_ProspectLoanDetailsInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/loan_details_progress.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />

    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqcontextmenu.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .task-square{background-color:#f8f8f8; border: solid 1px #ccc; padding:0px 6px 10px 8px; color:#555; width:250px;}
    .task-square p{margin:0px;margin-top:7px;}
    .task-square.yellow{background-color:yellow; }
    .clear {
	  clear: both;
	  display: block;
	  overflow: hidden;
	  visibility: hidden;
	  width: 0;
	  height: 0;
	}
    </style>
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script type="text/javascript" src="../js/jquery-ui-1.8.5.custom.min.js"></script>
    <script type="text/javascript" src="../js/jquery.wt-scroller.js"></script>
    <script type="text/javascript" src="../js/loan_details_progress.js"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jqcontextmenu.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>

    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            AddValidators();

            $("#txtBirthday").datepick({ yearRange: "1910:2012" });

            $("#txtCellPhone").mask("(999) 999-9999");
            $("#txtHomePhone").mask("(999) 999-9999");
            $("#txtWorkPhone").mask("(999) 999-9999");
            $("#txtBirthday").mask("99/99/9999");

            $("#txtAmount").numeric({ allow: "," });
            $("#txtRate").numeric({ allow: "." });

            $('#btnChangeStatus').addcontextmenu('divChangeStatusMenu');
            $('#btnPointSync').addcontextmenu('divPointSyncMenu');

        });

        function ShowDialog_SendEmail() {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/EmailSendPopup.aspx?sid=" + sid + "&LoanID=" + LoanID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 600;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Send Email", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        //#region CR48 neo

        function btnModify_onclick() {

            var FileId = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.parent.parent.location.href = "LeadEdit.aspx?sid=" + sid + "&LoanId=" + FileId;
        }

        function btnEdit_onclick() {

            // show waiting
            window.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                window.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        $("#btnEdit").hide();
                        $("#btnSave").show();
                        $("#tbLeadForm").show()
                        $("#tbLeadInfo").css("margin-top", "10px");
                        $("#tbLeadInfo").hide();
                    }
                }
            });

        }

        function AddValidators() {

            $("#form1").validate({

                rules: {

                    txtFirstName: {
                        required: true
                    },
                    txtLastName: {
                        required: true
                    },
                    txtEmail: {
                        email: true
                    }
                },
                messages: {

                    txtFirstName: {
                        required: "*"
                    },
                    txtLastName: {
                        required: "*"
                    },
                    txtEmail: {
                        email: "x"
                    }
                }
            });
        }

        function BeforeSave() {

            // call validate
            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return;
            }
            var Email = $.trim($("#txtEmail").val());
            var CellPhone = $.trim($("#txtCellPhone").val());
            var HomePhone = $.trim($("#txtHomePhone").val());
            var WorkPhone = $.trim($("#txtWorkPhone").val());
            if (Email == "" && CellPhone == "" && HomePhone == "" && WorkPhone == "") {

                alert(" One of Email, CellPhone, HomePhone and WorkPhone is required.");
                return false;
            }

            return true;
        }

        function btnLogTask_onclick() {

            var FileId = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "LogLeadTaskPopup.aspx?sid=" + sid + "&FileId=" + FileId + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 420;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 280;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Log Lead Task", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function btnCreateTask_onclick() {

            var FileId = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/TaskCreate.aspx?sid=" + sid + "&LoanID=" + FileId + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 610;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 400;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Loan Task Details", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        //#region referral source

        function aSelectContact_onlick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "../Contact/SearchContactsPopup.aspx?sid=" + sid + "&CloseDialogCodes=window.parent.CloseGlobalPopup()&from=ProspectLoanDetailInfo";

            var BaseWidth = 600
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 240;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Search Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function GetSearchCondition(sWhere) {

            window.parent.parent.CloseGlobalPopup();
            ShowDialog_SelectContact(sWhere);
        }

        function GetSearchCondition_TabPage(sWhere) {

            window.parent.parent.CloseGlobalPopup();
            ShowDialog_SelectContact_TabPage(sWhere);
        }

        function ShowDialog_SelectContact(sWhere) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var stype = "ProspectLoanDetailInfo";
            var iFrameSrc = "../Contact/SelectContactsPopup.aspx?sid=" + sid + "&pagesize=15&sCon=" + sWhere + "&type=" + stype + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 750
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 550;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Select Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowDialog_SelectContact_TabPage(sWhere) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var stype = "TabPage";
            var iFrameSrc = "../Contact/SelectContactsPopup.aspx?sid=" + sid + "&pagesize=15&sCon=" + sWhere + "&type=" + stype + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 750
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 550;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Select Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowReferral(s) {

            window.parent.parent.CloseGlobalPopup();
            var ss = s.split(':');

            $("#txtReferralSource").val(ss[1]);
            $("#hdnReferralID").val(ss[0]);
        }

        //#endregion

        function ChangeStatus(status) {

            // show waiting
            window.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileId = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileId, function (data) {

                window.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        var HasPointFile = $("#hdnHasPointFile").val();
//                        if (HasPointFile == "False") {

//                            alert("Can not change status, because here is no point file.");
//                            return;
//                        }

                        var BranchID = $("#hdnBranchID").val();

                        sid = RadomNum.toString().substr(2);
                        var iFrameSrc = "DisposePointFolderList.aspx?sid=" + sid + "&Action=" + status + "&LoanID=" + FileId + "&BranchID=" + BranchID + "&detail=1&CloseDialogCodes=window.parent.CloseGlobalPopup()";

                        var BaseWidth = 601
                        var iFrameWidth = BaseWidth + 2;
                        var divWidth = iFrameWidth + 25;

                        var BaseHeight = 600;
                        var iFrameHeight = BaseHeight + 2;
                        var divHeight = iFrameHeight + 40;

                        window.parent.parent.ShowGlobalPopup("Dispose Point Folder List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
                    }
                }
            });

        }

        //#region make complete

        function btnMakeComplete_onclick() {

            var TaskID = $("#hdnNextTaskID").val();
            if (TaskID == "") {

                alert("There is no task to be completed.");
                return;
            }

            //#region complete others' task

            var TaskOwnerID = $("#hdnNextTaskOwnerID").val();
            var LoginUserID = $("#hdnLoginUserID").val();
            var CompleteOtherTask = $("#hdnCompleteOtherTask").val();
            if (CompleteOtherTask == "False") {

                if ((LoginUserID != TaskOwnerID) && (TaskOwnerID)) {

                    alert("You do not have the privilege to complete tasks that are not assigned to you.");
                    return;
                }
            }

            //#endregion

            CompleteTask(TaskID);
        }

        function CompleteTask(TaskID) {

            // show waiting dialog
            window.parent.parent.ShowWaitingDialog3("Completing task...");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var LoanID = GetQueryString1("LoanID");

            // Ajax
            $.getJSON("../LoanDetails/LoanTaskComplete_Background.aspx?sid=" + sid + "&TaskID=" + TaskID + "&LoanID=" + LoanID, AfterCompleteTask);
        }

        function AfterCompleteTask(data) {

            if (data.ExecResult == "Failed") {

                alert(data.ErrorMsg);
                return;
            }

            // success
            window.parent.parent.CloseWaitingDialog3();
            alert("Completed task successfully.");

            // refresh
            window.parent.RefreshTopAndBottomIframe();

            // send completion email
            if (isID(data.EmailTemplateID) == true) {

                ShowDialog_SendCompletionEmail(data.EmailTemplateID, data.TaskID);
            }
        }

        function ShowDialog_SendCompletionEmail(EmailTemplateID, TaskID) {

            var LoanID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/EmailSendCompletionPopup.aspx?sid=" + sid + "&LoanID=" + LoanID + "&EmailTemplateID=" + EmailTemplateID + "&TaskID=" + TaskID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 380;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Send Completion Email", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        //#endregion

        function BeforeExport() {

            // show waiting
            window.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileID = GetQueryString1("LoanID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                window.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        var HasPointFile = $("#hdnHasPointFile").val();
                        //alert(HasPointFile);
                        if (HasPointFile == "False") {

                            alert("Please select a Point Folder at first in Setup Tab -> Point Info Tab.");
                            window.parent.aSetup_onclick(4);
                            return;
                        }

                        __doPostBack("lnkExport", "");
                    }
                }
            });

        }

        //#endregion

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="border: solid 0px green; padding-left: 10px;">
        <table cellpadding="0" cellspacing="0" style="margin-left: 10px;">
            <tr>
                <td>
                    <h4 id="hProspectName" runat="server" style="margin: 0px; color: #587ec6"></h4>
                </td>
                <td style="padding-left: 40px;">
                    <div id="divLoanDetailsProgress" class="container">
                        <div class="wt-scroller">
                            <div class="prev-btn">
                            </div>
                            <div class="slides">
                                <ul>
                                    <asp:Repeater ID="rptStageProgressItems" runat="server">
                                        <ItemTemplate>
                                            <li title="<%# this.GetStageToolTip(Eval("StageAlias").ToString(), Eval("Completed").ToString()) %>">
                                                <a>
                                                    <img src="../images/loan_details_progress/<%# Eval("StageImage") %>" alt="" /></a>
                                                <p>
                                                    <%# this.GetSpan_StageAliasCompleteDate(Eval("StageAlias").ToString(), Eval("StageImage").ToString(), Eval("Completed").ToString())%></p>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
                            </div>
                            <div class="next-btn">
                            </div>
                            <div class="lower-panel">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
        <table style="margin-top: 5px;">
            <tr>
                <%--<td>
                    <input id="btnModify" type="button" value="Modify" class="Btn-66" onclick="btnModify_onclick()" />
                </td>--%>
            	<td>
            		<input id="btnEdit" type="button" value="Edit" class="Btn-66"  onclick="btnEdit_onclick()" />
            		<asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" OnClick="btnSave_Click" style="display:none;" />
            	</td>
                <td>
                    <input id="btnSendEmail" runat="server" type="button" value="Send Email" class="Btn-91" onclick="ShowDialog_SendEmail()" />
                </td>
                <td>
                    <input id="btnLogTask" runat="server" type="button" value="Log Task" class="Btn-91" onclick="btnLogTask_onclick()" />
                </td>
                <td>
                    <input id="btnCreateTask" runat="server" type="button" value="Create Task" class="Btn-91" onclick="btnCreateTask_onclick()" />
                </td>
                <td>
                    <input id="btnChangeStatus" runat="server" type="button" value="Change Status" class="Btn-91" />
                </td>
                <td>
                    <asp:Button ID="btnvCardExport" runat="server" Text="vCard Export" CssClass="Btn-91" OnClick="btnvCardExport_Click" />
                </td>
                <td>
                    <input id="btnPointSync" runat="server" type="button" value="Point Sync" class="Btn-91" />
                </td>
            </tr>
        </table>
        <div  id="divLeadFormContainer" style="margin-top:10px; float:left;">
	        <table  id="tbLeadForm" cellpadding="0" cellspacing="0" style="display:none;">
	        	<tr>
	        		<td style="width:350px;">
	        			<table cellpadding="3" cellspacing="3">
				        	<tr>
				        		<td>First Name:</td>
				        		<td>
				        			<asp:TextBox ID="txtFirstName" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
				        		</td>
				        	</tr>
				        	<tr>
				        		<td>Last Name:</td>
				        		<td>
				        			<asp:TextBox ID="txtLastName" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
				        		</td>
				        	</tr>
				        	<tr>
				        		<td>Email:</td>
				        		<td>
				        			<asp:TextBox ID="txtEmail" runat="server" Width="180px" MaxLength="255"></asp:TextBox>
				        		</td>
				        	</tr>
				        	<tr>
			                    <td>Cell Phone:</td>
			                    <td>
			                        <asp:TextBox ID="txtCellPhone" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			                    </td>
			                </tr>
			                <tr>
			                    <td>Home Phone:</td>
			                    <td>
			                        <asp:TextBox ID="txtHomePhone" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			                    </td>
			                </tr>
			                <tr>
			                    <td>Work Phone:</td>
			                    <td>
			                        <asp:TextBox ID="txtWorkPhone" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			                    </td>
			                </tr>
			                <tr>
			                    <td>Date of Birth:</td>
			                    <td>
			                        <asp:TextBox ID="txtBirthday" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
			                    </td>
			                </tr>
				        </table>
	        		</td>
	        		<td style="width:350px;">
	        			<table cellpadding="3" cellspacing="3">
				        	<tr>
				        		<td>Lead Source:</td>
				        		<td>
				        			<asp:DropDownList ID="ddlLeadSource" runat="server" DataValueField="LeadSource" DataTextField="LeadSource" Width="184px">
	                                </asp:DropDownList>
				        		</td>
				        	</tr>
				        	<tr>
				        		<td>Referral Source:</td>
				        		<td>
				        			<asp:TextBox ID="txtReferralSource" runat="server" Width="180px" MaxLength="300"></asp:TextBox>
				        			<a id="aSelectContact" href="javascript:aSelectContact_onlick()"> [Select]</a>
	                                <asp:HiddenField ID="hdnReferralID" runat="server" />
				        		</td>
				        	</tr>
				        	<tr>
				        		<td>Purpose:</td>
				        		<td>
				        			<asp:DropDownList ID="ddlPurpose" runat="server" Height="18px" Width="184px">
	                                    <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
	                                    <asp:ListItem Text="Purchase" Value="Purchase"></asp:ListItem>
	                                    <asp:ListItem Text="No Cash-Out Refinance" Value="No Cash-Out Refinance"></asp:ListItem>
	                                    <asp:ListItem Text="Cash-Out Refinance" Value="Cash-Out Refinance"></asp:ListItem>
	                                    <asp:ListItem Text="Construction" Value="Construction"></asp:ListItem>
	                                    <asp:ListItem Text="Construction-Perm" Value="Construction-Perm"></asp:ListItem>
	                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
	                                </asp:DropDownList>
				        		</td>
				        	</tr>
				        	<tr>
			                    <td>Type:</td>
			                    <td>
			                        <asp:DropDownList ID="ddlType" runat="server" Width="184px">
	                                    <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
	                                    <asp:ListItem>Conv</asp:ListItem>
	                                    <asp:ListItem>FHA</asp:ListItem>
	                                    <asp:ListItem>VA</asp:ListItem>
	                                    <asp:ListItem>USDA/RH</asp:ListItem>
	                                    <asp:ListItem>Other</asp:ListItem>
				                    </asp:DropDownList>
			                    </td>
			                </tr>
			                <tr>
			                    <td>Program:</td>
			                    <td>
			                        <asp:DropDownList ID="ddlProgram" runat="server" DataValueField="LoanProgram" DataTextField="LoanProgram" Width="184px">
				                    </asp:DropDownList>
			                    </td>
			                </tr>
			             	</table>
			             	<table cellpadding="0" cellspacing="0" border="0">
			                <tr>
			                    <td style="width:79px; padding-left:7px;">Amount:</td>
			                    <td>$&nbsp;</td>
			                    <td>
			                        <asp:TextBox ID="txtAmount" runat="server" Width="180px" MaxLength="15"></asp:TextBox>
			                    </td>
			                </tr>
			              </table>
			              <table cellpadding="0" cellspacing="0" border="0">
			                <tr>
			                    <td style="width:75px; padding-left:7px; padding-top:6px;">Rate:</td>
			                    <td style="padding-top:6px;">%&nbsp;</td>
			                    <td style="padding-top:6px;">
			                        <asp:TextBox ID="txtRate" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
			                    </td>
			                </tr>
				        </table>
	        		</td>
	        	</tr>	
	        </table>
	        
	        <table id="tbLeadInfo" cellpadding="0" cellspacing="0" style="margin-top:10px;">
        	<tr>
        		<td style="width:350px;">
        			<table cellpadding="4" cellspacing="4">
			        	<tr>
			        		<td>First Name:</td>
			        		<td>
			        			<asp:Label ID="lbFirstName" runat="server" Text=""></asp:Label>
			        		</td>
			        	</tr>
			        	<tr>
			        		<td>Last Name:</td>
			        		<td>
			        			<asp:Label ID="lbLastName" runat="server" Text=""></asp:Label>
			        		</td>
			        	</tr>
			        	<tr>
			        		<td>Email:</td>
			        		<td>
			        			<asp:Label ID="lbEmail" runat="server" Text=""></asp:Label>
			        		</td>
			        	</tr>
			        	<tr>
		                    <td>Cell Phone:</td>
		                    <td>
		                        <asp:Label ID="lbCellPhone" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
		                <tr>
		                    <td>Home Phone:</td>
		                    <td>
		                        <asp:Label ID="lbHomePhone" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
		                <tr>
		                    <td>Work Phone:</td>
		                    <td>
		                        <asp:Label ID="lbWorkPhone" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
		                <tr>
		                    <td>Date of Birth:</td>
		                    <td>
		                        <asp:Label ID="lbBirthday" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
			        </table>
        		</td>
        		<td style="width:350px; vertical-align: top;">
        			<table cellpadding="3" cellspacing="3">
			        	<tr>
			        		<td>Lead Source:</td>
			        		<td>
			        			<asp:Label ID="lbLeadSource" runat="server" Text=""></asp:Label>
			        		</td>
			        	</tr>
			        	<tr>
			        		<td>Referral Source:</td>
			        		<td>
			        			<asp:Label ID="lbReferralSource" runat="server" Text=""></asp:Label>
			        		</td>
			        	</tr>
			        	<tr>
			        		<td>Purpose:</td>
			        		<td>
			        			<asp:Label ID="lbPurpose" runat="server" Text=""></asp:Label>
			        		</td>
			        	</tr>
			        	<tr>
		                    <td>Type:</td>
		                    <td>
		                        <asp:Label ID="lbType" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
		                <tr>
		                    <td>Program:</td>
		                    <td>
		                        <asp:Label ID="lbProgram" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
		             	</table>
		             	<table cellpadding="0" cellspacing="0" border="0">
		                <tr>
		                    <td style="width:79px; padding-left:7px;">Amount:</td>
		                    <td>$&nbsp;</td>
		                    <td>
		                        <asp:Label ID="lbAmount" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
		              </table>
		              <table cellpadding="0" cellspacing="0" border="0">
		                <tr>
		                    <td style="width:75px; padding-left:7px; padding-top:6px;">Rate:</td>
		                    <td style="padding-top:6px;">&nbsp;</td>
		                    <td style="padding-top:6px;">
		                        <asp:Label ID="lbRate" runat="server" Text=""></asp:Label>
		                    </td>
		                </tr>
			        </table>
        		</td>
        		
        	</tr>	
        </table>
        </div>
        <div id="divActions" runat="server" style="margin-top: 20px;">
        	<table cellpadding="3" cellspacing="3" border="0">
		        
                <tr id="trLastAction" runat="server">
		        	<td style="vertical-align:top; width:70px; padding-top:7px;">Last Action:</td>
		        	<td>
		        		<div class="task-square">
		        			<p>
                                <asp:Literal ID="ltrLastTaskName" runat="server"></asp:Literal>
                            </p>
			        		<p>
                                <asp:Literal ID="ltrLastDueDate" runat="server"></asp:Literal>
                            </p>
			        		<p>
                                <asp:Literal ID="ltrLastDueTime" runat="server"></asp:Literal>
                            </p>
		        		</div>
		        	</td>
		        </tr>
		        	
		        <tr id="trNextAction" runat="server">
		        	<td style="vertical-align:top; width:70px; padding-top:7px;">Next  Action:</td>
		        	<td>
		        		<div class="task-square yellow">
		        			<p>
                                <asp:Literal ID="ltrNextTaskName" runat="server"></asp:Literal>
                                
                                <asp:HiddenField ID="hdnNextTaskID" runat="server" />
                                <asp:HiddenField ID="hdnNextTaskOwnerID" runat="server" />
                                <asp:HiddenField ID="hdnLoginUserID" runat="server" />
                                <asp:HiddenField ID="hdnCompleteOtherTask" runat="server" />
                                <asp:HiddenField ID="hdnLoanStatus" runat="server" />
                            </p>
			        		<p>
                                <asp:Literal ID="ltrNextDueDate" runat="server"></asp:Literal>
                            </p>
			        		<p>
                                <asp:Literal ID="ltrNextDueTime" runat="server"></asp:Literal>
                            </p>
		        		</div>
                        <div style="text-align:center; margin-top:5px;">
                            <input id="btnMakeComplete" runat="server" type="button" value="Mark Complete" onclick="btnMakeComplete_onclick()" class="Btn-115" />
				        	&nbsp;&nbsp;
				        	<asp:Button ID="btniCalendarExport" runat="server" Text="iCalendar Export" CssClass="Btn-115" OnClick="btniCalendarExport_Click" />
                        </div>
		        	</td>
		        </tr>
		        	
		    </table>
        </div>

        <div class="clear"></div>
        


        <ul id="divChangeStatusMenu" class="jqcontextmenu">
	        <asp:Literal ID="ltrChangeStatusMenuItems" runat="server"></asp:Literal>
	    </ul>

        <ul id="divPointSyncMenu" class="jqcontextmenu">
	        <li>
                <asp:LinkButton ID="lnkImport" runat="server" OnClick="lnkImport_Click">Import</asp:LinkButton>
            </li>
            <li>
                <a id="aExport" runat="server" href="javascript:BeforeExport();">Export</a>
                <asp:LinkButton ID="lnkExport" runat="server" OnClick="lnkExport_Click" style="display:none;">Export</asp:LinkButton>
            </li>
	    </ul>
    </div>

    <asp:HiddenField ID="hdnHasPointFile" runat="server" />
    <asp:HiddenField ID="hdnBranchID" runat="server" />
    
    </form>
</body>
</html>
