<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QuickLeadForm.aspx.cs"
    Inherits="QuickLeadForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quick Lead Form</title>
    <link href="css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-autocomplete-input
        {
            width: 220px;
        }
        .ui-autocomplete-loading
        {
            background: white url('images/loading.gif') right center no-repeat;
        }
        .ui-icon-triangle-1-sx
        {
            background-position: -64px -20px;
        }
        .ui-autocomplete
        {
            max-height: 100px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            padding-right: 20px;
        }
        /* IE 6 doesn't support max-height
	 * we use height instead, but this forces the menu to always be this tall
	 */
        * html .ui-autocomplete
        {
            height: 430px;
        }
    </style>

    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/urlparser.js" type="text/javascript"></script>
    <script src="js/common.js" type="text/javascript"></script>
    <script src="js/jquery.base64.js" type="text/javascript"></script>
    <script src="js/jquery.cleditor.js" type="text/javascript"></script>

    <script src="js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="js/jquery.validate.js" type="text/javascript"></script>
    <script src="js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <script src="js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.tabs.min.js" type="text/javascript"></script>
    <script src="js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="js/jquery.datepick.js" type="text/javascript"></script>
    <script src="js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="js/jquery.formatCurrency.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[


        $(document).ready(function () {

            AddValidators();

            $("#txtBirthday").datepick({ yearRange: "1910:2012" });
            $("#txtDueDate").datepick();

            $("#txtPhone").mask("(999) 999-9999");
            $("#txtSSN").mask("999-99-9999");
            $("#txtBirthday").mask("99/99/9999");
            $("#txtDueDate").mask("99/99/9999");
            $("#txtDueTime").mask("99:99");
            $("#txtLoanAmount").numeric({ allow: "," });
            $("#txtLoanAmount").blur(function () { FormatMoney(this); });


            var tTime = document.getElementById("<%=txtDueTime.ClientID%>");

            var aTime = '<%=day()%>';

            //tTime.value = aTime;

            //alert(a);


            //            var dateTime = new Date();


            //                        var hh = dateTime.getHours() + 2;
            //                        var mm = dateTime.getMinutes();

            //                        if (hh >= 24) {
            //                            hh = "0";

            //                        }
            //                        if (hh < 10) {
            //                            hh = "0" + hh;
            //                        }
            //                        if (mm < 10) {
            //                            tTime.value = hh + ":" + "0" + mm;
            //                        }
            //                        else {
            //                            tTime.value = hh + ":" + mm;
            //                        }



            InitAutoComplete();

            // add event
            $("#ddlType").change(ddlType_onchange);
        });


        function FormatMoney(textbox) {

            // format
            $(textbox).formatCurrency();

            // substring
            var m0 = $(textbox).val();
            var m1 = m0.replace("$", "");
            var m2 = m1.replace(".00", "");

            var maxlength = $(textbox).attr("maxlength");
            var maxnum = new Number(maxlength);

            if (m2.length > maxnum) {

                var substr = m2.substring(0, maxnum);

                var m3 = substr.replace(/,/g, "");

                $(textbox).val(m3);

                $(textbox).formatCurrency();
                var m4 = $(textbox).val();
                var m5 = m4.replace("$", "");
                var m6 = m5.replace(".00", "");
                $(textbox).val(m6);
            }
            else {

                $(textbox).val(m2);
            }
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
                    txtPhone: {
                        required: true
                    }
                    //                    txtReminderUser: {
                    //                        required: true
                    //                    }
                },
                messages: {

                    txtFirstName: {
                        required: "*"
                    },
                    txtLastName: {
                        required: "*"
                    },
                    txtPhone: {
                        required: "*"
                    }
                    //                    txtReminderUser: {
                    //                        required: "*"
                    //                    }
                }
            });
        }

        function InitAutoComplete() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $("#txtReminderUser").autocomplete({

                source: "GetUserData_Ajax.aspx?sid=" + sid,
                minLength: 1,
                search: function (event, ui) {

                    $("#hdnReminderUserID").val("");
                },
                select: function (event, ui) {

                    $("#hdnReminderUserID").val(ui.item.id);
                }
            });

            $("#txtReminderUser").blur(function () {

                if ($("#hdnReminderUserID").val() == "") {

                    $("#txtReminderUser").val("");
                }
            });


        }

        function ddlType_onchange() {

            var Type = $("#ddlType").val();
            if (Type.indexOf("Phone") == -1) {	// email

                $("#txtPhone").rules("remove", "required");
                $("#txtEmail").rules("add", {
                    required: true,
                    email: true,
                    messages: { required: "*", email: "<span title='Invalid email address.'>x</span>" }
                });
            }
            else {

                $("#txtEmail").rules("remove", "required");
                $("#txtPhone").rules("add", {
                    required: true,
                    messages: { required: "*" }
                });
            }
        }

        function chkReminder_onclick(checkbox) {

            if (checkbox.checked == true) {

                $("#txtReminderUser").removeAttr("disabled");
                $("#txtReminderUser").rules("add", {
                    required: true,
                    messages: { required: "*" }
                });
                $("#ddlReminderInterval").removeAttr("disabled");
            }
            else {

                $("#txtReminderUser").attr("disabled", "true");
                $("#ddlReminderInterval").attr("disabled", "true");
                $("#txtReminderUser").rules("remove", "required");
            }
        }

        function radTaskList_onclick() {

            $("#ddlTaskList").removeAttr("disabled");
            $("#txtTaskName").val("");
            $("#txtTaskName").attr("disabled", "true");
            $("#txtTaskName").rules("remove", "required");
        }

        function radTaskName_onclicik() {

            $("#ddlTaskList").attr("disabled", "true");
            $("#txtTaskName").removeAttr("disabled");
            $("#txtTaskName").rules("add", {
                required: true,
                messages: { required: "*" }
            });
        }

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $.blockUI({ message: $('#divWaiting'), css: { width: '125px'} });
            return true;
        }

        function CloseWaitingDialog() {

            $.unblockUI();
        }

        function showWaiting() {
            var isValid = $("#form1").validate().form();
            if (isValid == false) {
                return false;
            }
            ShowWaitingDialog("Please wait...");
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 100%; border: solid 0px red; margin-top: 5px;">
        <div id="divButtons">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return showWaiting()" OnClick="btnSave_Click" />&nbsp;
            <asp:Button ID="btnSaveAndGoToLeadDetail" runat="server" Text="Save And Go to Lead Detail"
                CssClass="Btn-140" OnClick="btnSaveAndGoToLeadDetail_Click" />&nbsp;
            <input id="btnClear" type="reset" value="Clear" class="Btn-66" />
        </div>
        <div id="divContactInfo" style="margin-top: 10px;">
            <table>
                <tr>
                    <td style="width: 100px;">
                        Lead Source:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLeadSource" runat="server" DataValueField="LeadSourceID" DataTextField="LeadSource"
                            Width="184px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Loan Officer:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLoanOfficer" runat="server" Width="184px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100px;">
                        First Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Last Name:
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="180px" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Phone:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhone" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" Width="184px">
                            <asp:ListItem>Cell Phone</asp:ListItem>
                            <asp:ListItem>Email</asp:ListItem>
                            <asp:ListItem>Home Phone</asp:ListItem>
                            <asp:ListItem>Work Phone</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Date of Birth:
                    </td>
                    <td>
                        <asp:TextBox ID="txtBirthday" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        SSN:
                    </td>
                    <td>
                        <asp:TextBox ID="txtSSN" runat="server" Width="180px" MaxLength="11"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Purpose:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPurpose" runat="server" Width="184px">
                            <asp:ListItem Selected="True">- select -</asp:ListItem>
                            <asp:ListItem>Purchase</asp:ListItem>
                            <asp:ListItem>No Cash-Out Refinance</asp:ListItem>
                            <asp:ListItem>Cash-Out Refinance</asp:ListItem>
                            <asp:ListItem>Construction</asp:ListItem>
                            <asp:ListItem>Construction - Perm</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Loan Amount:
                    </td>
                    <td>
                        $<asp:TextBox ID="txtLoanAmount" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Workflow:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlWorkflow" runat="server" Width="184px" DataTextField="Name"
                            DataValueField="WflTemplId">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Marketing<br />
                        enrollment:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMarketing" runat="server" Width="184px" DataTextField="Name"
                            DataValueField="LID">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divTaskInfo" style="margin-top: 15px;">
            <table>
                <tr>
                    <td rowspan="2" style="width: 80px;">
                        Task Name:
                    </td>
                    <td style="width: 15px;">
                        <input id="radTaskList" runat="server" type="radio" name="task-name-source" checked
                            onclick="radTaskList_onclick()" />
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTaskList" runat="server" Width="184px" DataTextField="TaskName"
                            DataValueField="TaskName">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <%--<td>Task Name:</td>--%>
                    <td>
                        <input id="radTaskName" runat="server" type="radio" name="task-name-source" onclick="radTaskName_onclicik()" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtTaskName" runat="server" Width="180px" Enabled="false" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Due Date:
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtDueDate" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Due Time:
                    </td>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <asp:TextBox ID="txtDueTime" runat="server" Width="180px" Visible="false"></asp:TextBox>
                        <asp:DropDownList ID="ddlDueTime_hour" runat="server">
                            <asp:ListItem Value="00">00am</asp:ListItem>
                            <asp:ListItem Value="01">01am</asp:ListItem>
                            <asp:ListItem Value="02">02am</asp:ListItem>
                            <asp:ListItem Value="03">03am</asp:ListItem>
                            <asp:ListItem Value="04">04am</asp:ListItem>
                            <asp:ListItem Value="05">05am</asp:ListItem>
                            <asp:ListItem Value="06">06am</asp:ListItem>
                            <asp:ListItem Value="07">07am</asp:ListItem>
                            <asp:ListItem Value="08">08am</asp:ListItem>
                            <asp:ListItem Value="09">09am</asp:ListItem>
                            <asp:ListItem Value="10">10am</asp:ListItem>
                            <asp:ListItem Value="11">11am</asp:ListItem>
                            <asp:ListItem Value="12">12am</asp:ListItem>
                            <asp:ListItem Value="13">13pm</asp:ListItem>
                            <asp:ListItem Value="14">14pm</asp:ListItem>
                            <asp:ListItem Value="15">15pm</asp:ListItem>
                            <asp:ListItem Value="16">16pm</asp:ListItem>
                            <asp:ListItem Value="17">17pm</asp:ListItem>
                            <asp:ListItem Value="18">18pm</asp:ListItem>
                            <asp:ListItem Value="19">19pm</asp:ListItem>
                            <asp:ListItem Value="20">20pm</asp:ListItem>
                            <asp:ListItem Value="21">21pm</asp:ListItem>
                            <asp:ListItem Value="22">22pm</asp:ListItem>
                            <asp:ListItem Value="23">23pm</asp:ListItem>
                        </asp:DropDownList>
                        <% ddlDueTime_hour.SelectedValue = DateTime.Now.Hour.ToString(); %>
                        <asp:DropDownList ID="ddlDueTime_min" runat="server">
                            <asp:ListItem>00</asp:ListItem>
                            <asp:ListItem>05</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>20</asp:ListItem>
                            <asp:ListItem>25</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>35</asp:ListItem>
                            <asp:ListItem>40</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>50</asp:ListItem>
                            <asp:ListItem>55</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Reminder:
                    </td>
                    <td style="vertical-align: top; padding-top: 0px;">
                        <input id="chkReminder" runat="server" type="checkbox" checked onclick="chkReminder_onclick(this)" />
                    </td>
                    <td>
                        <asp:TextBox ID="txtReminderUser" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        <asp:HiddenField ID="hdnReminderUserID" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlReminderInterval" runat="server" Width="184px">
                            <asp:ListItem>5 minutes</asp:ListItem>
                            <asp:ListItem>10 minutes</asp:ListItem>
                            <asp:ListItem>15 minutes</asp:ListItem>
                            <asp:ListItem>30 minutes</asp:ListItem>
                            <asp:ListItem>45 minutes</asp:ListItem>
                            <asp:ListItem>1 hours </asp:ListItem>
                            <asp:ListItem>2 hours</asp:ListItem>
                            <asp:ListItem>3 hours</asp:ListItem>
                            <asp:ListItem>4 hours</asp:ListItem>
                            <asp:ListItem>5 hours</asp:ListItem>
                            <asp:ListItem>6 hours</asp:ListItem>
                            <asp:ListItem>7 hours</asp:ListItem>
                            <asp:ListItem>8 hours</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divWaiting" style="display: none; padding: 2px;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting" src="images/waiting.gif" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
				</td>
			</tr>
		</table>
	</div>
    </form>
</body>
</html>
