<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSetup.aspx.cs" Inherits="LPWeb.Settings.UserSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <title>User Setup</title>
    <style type="text/css">
        .validSign
        {
            padding-top: 4px;
            color: Red;
            display: none;
        }
    </style>
    <script type="text/javascript">
    
        $(document).ready(function () {
            DrawTab();
            
            $('#addLoanRep').click(function () {
                var f = document.getElementById('iframeUL');
                f.src="UserLoanRepSelection.aspx?currIds="+$("#"+'<%=hiCurrLoanRep.ClientID %>').val() + "&uid=" + '<%=string.Format("{0}", UserId) %>' + "&t=" + Math.random().toString();
                $('#dialog1').dialog('open');
                return false;
            });
            $('#addGroup').click(function () {
                var f = document.getElementById('iframeGroup');
                f.src="GroupSelection.aspx?currIds="+$("#" + '<%=hiCurrGroup.ClientID %>').val() + "&t=" + Math.random().toString();
                $('#dialog2').dialog('open');
                return false;
            });
            
            initCfmBox('dialog1', 'User Loan Rep Selection', '', 500, 450, function(){
                if (window.frames.iframeUL && window.frames.iframeUL.returnFn) {
                    window.frames.iframeUL.returnFn();
                }
            });
            initCfmBox('dialog2', 'Group Selection', '', 500, 450, function(){
                if (window.frames.iframeGroup && window.frames.iframeGroup.returnFn) {
                    window.frames.iframeGroup.returnFn();
                }
            });
            initReassignUserWin();
        });

        function getLoanRepSelectionReturn(sReturn)
        {
            $("#"+'<%=hiLoanRep.ClientID %>').val(sReturn);
            $('#dialog1').dialog('close');
            <%=this.ClientScript.GetPostBackEventReference(this.lbtnLoanRepSelected, null) %>
            clearUserSetupWin('dialog1');
        }

        function getGroupSelectionReturn(sReturn)
        {
            $("#"+'<%=hiGroup.ClientID %>').val(sReturn);
            $('#dialog2').dialog('close');
            <%=this.ClientScript.GetPostBackEventReference(this.lbtnGroupSelected, null) %>
            clearUserSetupWin('dialog2');
        }

        function initCfmBox(id, title, msg, w, h, callBack)
        {
            $('#'+id).dialog({
                modal: false,
                autoOpen: false,
                width: w,
                height: h,
			    resizable: false,
                title: title,
                buttons:{
                    Yes: function(){
                        callBack();
                        $(this).dialog("close");
                    },
                    Cancel: function(){
                        $(this).dialog("close");
                        clearUserSetupWin(id);
                    }
                },
                open: function(){                    
                    $('.ui-dialog-buttonset').css('float', 'none');
                    $('.ui-dialog-buttonset').css('text-align', 'center');
                    $('.ui-dialog-buttonpane').find('button').addClass('Btn-66');
                }
            });
        }
        function clearUserSetupWin(id)
        {
            var f = "";
            if ("dialog1" == id)
                f = document.getElementById('iframeUL');
            else if ("dialog2" == id)
                f = document.getElementById('iframeGroup');
            f.src="about:blank";
        }

        function closeBox(isRefresh, bReset) {
            if (bReset === false)
                bReset = false;
            else
                bReset = true;
            self.parent.closeUserSetupWin(isRefresh, bReset);
            return false;
        }

        function ShowMsg(args, msg, isClose, isRefresh, isReset) {
            if ("invalidInput" == args)
                alert("Invalid input!");
            else if ("saveSuccss" == args)
                alert("Saved!");
            else if ("userNameExists" == args)
                alert("Duplicate Username!");
            else if ("userEmailExists" == args)
                alert("Duplicate User Email Address!");
            else if ("userPicInvalid" == args)
                alert(msg);
            else if ("noRowSelected" == args)
                alert("Please select one or more row.");
            else if ("noUserSelected" == args)
                alert("Please select one user.");
            if (isClose)
                closeBox(isRefresh, isReset);
        }

    </script>
    <script type="text/javascript">
<!--
        function resizeImage(imgID) {
            // set image size, width
            var w = $("#" + imgID).width();
            var h = $("#" + imgID).height();
            if (w / h > 120 / 150) {
                if (w < 120)
                    return;
                $("#" + imgID).jScale({ w: "120px" });
            }
            else {
                if (h < 150)
                    return;
                $("#" + imgID).jScale({ h: "150px" });
            }
        }
//-->  
    </script>
    <script type="text/javascript">
// <![CDATA[
        // check/decheck all
        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#" + '<%=gridLoanRepMapping.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#" + '<%=gridLoanRepMapping.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
            }
        }
        function CheckAll2(CheckBox) {
            if (CheckBox.checked) {
                $("#" + '<%=gvGroup.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#" + '<%=gvGroup.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
            }
        }
        function IsRowSelected(sign, args) {
            var gvID = "";
            if (0 == sign)
                gvID = '<%=gridLoanRepMapping.ClientID %>';
            else if (1 == sign)
                gvID = '<%=gvGroup.ClientID %>';
            if ($("#" + gvID + " tr td :checkbox[checked=true]").length == 0) {
                ShowMsg("noRowSelected");
            }
            else {
                // give confirm info
                if ("loanrep" == args) {
                    return confirm('Are you sure you want to delete the selected loan reps?');
                }
                else if ("group" == args) {
                    return confirm('Are you sure you want to delete the selected groups?');
                }
            }
            return false;
        }

        function onSaveBtnClick() {
            if ($("#" + "<%=trPwd.ClientID %>").is(":visible")) {
                var sPwd = $("#" + "<%=tbPWD.ClientID %>").val();
                var sPwdRe = $("#" + "<%=tbPwdRe.ClientID %>").val();
                if (sPwd.length <= 0) {
                    $("#spanPwd").show();
                    $("#spanPwdRe").show();
                    alert("Please ensure that the password and confirmation match exactly.");
                    return false;
                }
                else if (sPwd != sPwdRe) {
                    $("#spanPwd").show();
                    $("#spanPwdRe").show();
                    alert("Please ensure that the password and confirmation match exactly.");
                    return false;
                }
                else {
                    $("#spanPwd").hide();
                    $("#spanPwdRe").hide();
                    return true;
                }
            }
            return true;
        }
        function changePWDBtnClicked(me) {
            $(me).hide();
            $("#" + "<%=lbtnCancelPwd.ClientID %>").show();
            $("#" + "<%=trPwd.ClientID %>").show();
        }
        function cancelPWDChange(me) {
            $(me).hide();
            $("#" + "<%=lbtnChangePwd.ClientID %>").show();
            $("#" + "<%=trPwd.ClientID %>").hide();

            $("#" + "<%=tbPWD.ClientID %>").val("");
            $("#" + "<%=tbPwdRe.ClientID %>").val("");
        }
// ]]>
    </script>
    <script type="text/javascript">
        function onDeleteBtnClicked() {
            if (confirm('Are you sure you want to delete the current user account?')) {
                var loanCount = null;
                try
                {
                    loanCount = new Number($("#" + "<%=hiUserLoanCount.ClientID %>").val());
                }
                catch (e)
                {
                    loanCount = 0;
                }
                var ucCount = null;
                try
                {
                    ucCount = new Number($("#" + "<%=hiUserContactCount.ClientID %>").val());
                }
                catch (e)
                {
                    ucCount = 0;
                }

                if (loanCount > 0 || ucCount > 0)
                    showReassignUserWin();
                else
                    return true;
            }
            return false;
        }
        function initReassignUserWin() {
            $('#dialog3').dialog({
                modal: false,
                autoOpen: false,
                title: 'User Selection',
                width: 600,
                height: 530,
                resizable: false,
                close: clearReassignUserWin
            });
        }
        function showReassignUserWin() {
            var f = document.getElementById('iframeReassignUser');
            var id = "<%=UserId.GetValueOrDefault(0) %>";
            f.src = "UserForReassignSelection.aspx?CloseDialogCodes=window.parent.closeAndGetReturnValue('returnValue')&uid=" + id + "&t=" + Math.random().toString();
            $('#dialog3').dialog('open');
        }
        function clearReassignUserWin() {
            var f = document.getElementById('iframeReassignUser');
            f.src = "about:blank";
        }
        function closeAndGetReturnValue(rtValue) {
            if ("returnValue" == rtValue)
                return;
            if (rtValue.length > 0) {
                $("#" + "<%=hiReassignUserId.ClientID %>").val(rtValue);
                $('#dialog3').dialog('close');
                // callback 
                <%=this.ClientScript.GetPostBackEventReference(this.btnDelete, null) %>
            }
        }
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="JTab" style="margin-top:10px;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <ul>
                            <li id="current"><a href="UserSetup.aspx?mode=<%= Mode %>&uid=<%=UserId %>&t=<% =Random %>"><span>General Settings</span></a></li>
                            <li><a href="UserLeadRouting.aspx?uid=<%=UserId %>&t=<%= Random %>"><span>Lead Routing</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <div id="TabBody">
            <div id="TabLine1" class="TabLeftLine" style="width: 242px">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">
                    &nbsp;</div>
                <div class="TabContent">
                    
                    <div id="aspnetForm">
        
                        <div class="DetailsContainer">

                            <div style="margin-top: 0px; margin-bottom:5px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click"
                                                OnClientClick="if(!onSaveBtnClick()){return false;};" />
                                        </td>
                                        <td style="padding-left: 8px;">
                                            <asp:Button ID="btnClone" runat="server" Text="Clone" class="Btn-66" OnClick="btnClone_Click" />
                                        </td>
                                        <td style="padding-left: 8px;">
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" class="Btn-66" OnClientClick="return onDeleteBtnClicked();"
                                                OnClick="btnDelete_Click" />
                                            <asp:HiddenField ID="hiReassignUserId" runat="server" />
                                            <asp:HiddenField ID="hiUserLoanCount" runat="server" />
                                            <asp:HiddenField ID="hiUserContactCount" runat="server" />
                                        </td>
                                        <td style="padding-left: 8px;">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="Btn-66" OnClientClick="return closeBox(false);" />
                                        </td>
                                    </tr>
                                </table>
                            </div>

                            <div>
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td style="white-space: nowrap;">
                                            Username prefix
                                        </td>
                                        <td style="padding-left: 15px;">
                                            <asp:Label ID="lbPrefix" runat="server"></asp:Label>
                                        </td>
                                        <td colspan="2" style="padding-left: 39px;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        Enabled
                                                    </td>
                                                    <td style="padding-left: 8px;">
                                                        <asp:CheckBox ID="ckbEnabled" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="padding-left: 39px; vertical-align: top;" rowspan="5">
                                            <div style="padding: 1px; border: 1px solid Gray; height: 150px; width: 120px; overflow: hidden;">
                                                <asp:Image ID="imgUserPic" runat="server" AlternateText="User Picture" /></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 9px;">
                                            Username
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="tbUserName" runat="server" class="iTextBox" Style="width: 177px;"
                                                MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="tbUserName"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="padding-left: 39px; padding-top: 9px; white-space: nowrap;">
                                            User Email Address
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="tbEmail" runat="server" class="iTextBox" Style="width: 177px;" MaxLength="255"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="tbEmail"
                                                ValidationExpression="^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$" ErrorMessage="*"
                                                ForeColor="Red"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 9px; white-space: nowrap;">
                                            User First Name
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="tbFirstName" runat="server" class="iTextBox" Style="width: 177px;"
                                                MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="tbFirstName"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="padding-left: 39px; padding-top: 9px;">
                                            User Picture File
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:FileUpload ID="fuPicture" runat="server" class="iFileUpload" Style="width: 177px;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top: 9px;">
                                            User Last Name
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="tbLastName" runat="server" class="iTextBox" Style="width: 177px;"
                                                MaxLength="50"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="tbLastName"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="padding-left: 39px; padding-top: 9px;">
                                            Role
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:DropDownList ID="ddlRole" runat="server" Width="187px">
                                                <asp:ListItem Text="Loan officer" Value="Loan officer"></asp:ListItem>
                                                <asp:ListItem Text="Loan officer assistant" Value="Loan officer assistant"></asp:ListItem>
                                                <asp:ListItem Text="Processor" Value="Processor"></asp:ListItem>
                                                <asp:ListItem Text="Underwriter" Value="Underwriter"></asp:ListItem>
                                                <asp:ListItem Text="Closer" Value="Closer"></asp:ListItem>
                                                <asp:ListItem Text="Doc Prep" Value="Doc Prep"></asp:ListItem>
                                                <asp:ListItem Text="Shipper" Value="Shipper"></asp:ListItem>
                                                <asp:ListItem Text="Manager" Value="Manager"></asp:ListItem>
                                                <asp:ListItem Text="Executive" Value="Executive"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvRole" runat="server" ControlToValidate="ddlRole"
                                                ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr id="trPwd" style="display: none;" runat="server">
                                        <td style="padding-top: 9px;">
                                            Password
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="tbPWD" runat="server" TextMode="Password" class="iTextBox" Style="width: 177px;"
                                                MaxLength="50"></asp:TextBox>
                                            <span id="spanPwd" class="validSign">*</span>
                                        </td>
                                        <td style="padding-left: 39px; padding-top: 9px; white-space: nowrap;">
                                            Re-type new password
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="tbPwdRe" runat="server" TextMode="Password" class="iTextBox" Style="width: 177px;"
                                                MaxLength="50"></asp:TextBox>
                                            <span id="spanPwdRe" class="validSign">*</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" style="padding-top: 9px;">
                                            <asp:LinkButton ID="lbtnChangePwd" runat="server" OnClientClick="changePWDBtnClicked(this); return false;">Change password</asp:LinkButton>
                                            <asp:LinkButton ID="lbtnCancelPwd" runat="server" OnClientClick="cancelPWDChange(this); return false;">Cancel</asp:LinkButton>
                                        </td>
                                    </tr>


                                    <tr>
                                        <td>
                                            Phone                    
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="txbPhone" Width ="100px" MaxLength="20" runat="server"></asp:TextBox>
                                        </td>

                                        <td style="padding-left: 39px; padding-top: 9px;">
                                            Cell                    
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="txbCell" Width ="100px" MaxLength="20" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Fax
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="txbFax"  Width ="100px" MaxLength="20"  runat="server"></asp:TextBox>
                                        </td>
                        
                                        <td style="padding-left: 39px; padding-top: 9px;">
                                            Exchange Password                    
                                        </td>
                                        <td style="padding-left: 15px; padding-top: 9px;">
                                            <asp:TextBox ID="txbExchangePassword" Width ="100px" MaxLength="50" runat="server" TextMode="Password"></asp:TextBox>
                                        </td>
                                    </tr>

                                </table>
                            </div>


                            <div style =" width:800px;">
            
                                <fieldset>
                                <legend>Compensation</legend>
                
                                <table>
                                    <tr>
                                        <td>Loan Officer Compensation:</td>
                                        <td><asp:TextBox ID="txbLoanOfficerCompenstation" runat="server"></asp:TextBox>%</td>
                                        <td>Division Manager Compensation:</td>
                                        <td><asp:TextBox ID="txbDivisionManagerCompensation" runat="server"></asp:TextBox>%</td>
                                    </tr>
                                    <tr>
                                        <td>Branch Manager Compensation:</td>
                                        <td><asp:TextBox ID="txbBranchManagerCompensation" runat="server"></asp:TextBox>%</td>
                                        <td>Regional Manager Compensation:</td>
                                        <td><asp:TextBox ID="txbRegionalManagerCompensation" runat="server"></asp:TextBox>%</td>
                                    </tr>
                                </table>
                    
                    
                                </fieldset>

                            </div>

                            <div class="DashedBorder" style="margin-top: 15px;">
                                &nbsp;</div>
                            <div style="margin-top: 9px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                        </td>
                                        <td style="text-decoration: underline; padding-left: 15px;">
                                            Loan Rep Mapping
                                        </td>
                                        <td>
                                        </td>
                                        <td style="text-decoration: underline; padding-left: 15px;">
                                            Group Access
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; padding-top: 9px;">
                                            Point Loan Rep Mapping
                                        </td>
                                        <td style="padding-left: 15px; vertical-align: top;">
                                            <asp:UpdatePanel ID="upLoanRepMapping" runat="server">
                                                <ContentTemplate>
                                                    <div id="div1" class="ColorGrid" style="margin-top: 5px; width: 200px;">
                                                        <asp:GridView ID="gridLoanRepMapping" runat="server" DataKeyNames="NameId,Name" EmptyDataText="There is no record."
                                                            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                                                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                                            <AlternatingRowStyle CssClass="EvenRow" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                                                    <HeaderTemplate>
                                                                        <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ckbItem" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="NameId" HeaderText="ID" />
                                                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#E4E7EF" />
                                                        </asp:GridView>
                                                        <asp:XmlDataSource ID="xdsLoanRep" runat="server" XPath="//LoanRep" EnableCaching="false">
                                                        </asp:XmlDataSource>
                                                        <input type="hidden" id="hiCurrLoanRep" runat="server" />
                                                        <input type="hidden" id="hiLoanRep" runat="server" />
                                                        <asp:LinkButton ID="lbtnLoanRepSelected" runat="server" OnClick="lbtnLoanRepSelected_Click"
                                                            CausesValidation="false"></asp:LinkButton>
                                                        <div class="GridPaddingBottom">
                                                            &nbsp;</div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div style="margin-top: 5px;">
                                                <a id="addLoanRep" href="#" class="Link-Btn">Add Row</a><span style="padding-left: 8px;
                                                    padding-right: 6px;">|</span>
                                                <asp:LinkButton ID="lbtnRemoveLoanRep" runat="server" OnClientClick="return IsRowSelected(0, 'loanrep');"
                                                    OnClick="lbtnRemoveLoanRep_Click" CssClass="Link-Btn">Remove</asp:LinkButton>
                                            </div>
                                        </td>
                                        <td style="padding-left: 39px; vertical-align: top; padding-top: 9px;">
                                            Group Memberships
                                        </td>
                                        <td style="padding-left: 15px; vertical-align: top;">
                                            <asp:UpdatePanel ID="upGroup" runat="server">
                                                <ContentTemplate>
                                                    <div id="div2" class="ColorGrid" style="margin-top: 5px; width: 200px;">
                                                        <asp:GridView ID="gvGroup" runat="server" DataKeyNames="GroupId,GroupName" EmptyDataText="There is no record."
                                                            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                                                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                                            <AlternatingRowStyle CssClass="EvenRow" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                                                    <HeaderTemplate>
                                                                        <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="ckbItem" runat="server" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="GroupId" HeaderText="ID" />
                                                                <asp:BoundField DataField="GroupName" HeaderText="Name" />
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="#E4E7EF" />
                                                        </asp:GridView>
                                                        <asp:XmlDataSource ID="xdsGroup" runat="server" XPath="//Group" EnableCaching="false">
                                                        </asp:XmlDataSource>
                                                        <input type="hidden" id="hiCurrGroup" runat="server" />
                                                        <input type="hidden" id="hiGroup" runat="server" />
                                                        <asp:LinkButton ID="lbtnGroupSelected" runat="server" OnClick="lbtnGroupSelected_Click"
                                                            CausesValidation="false"></asp:LinkButton>
                                                        <div class="GridPaddingBottom">
                                                            &nbsp;</div>
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <div style="margin-top: 5px;">
                                                <a id="addGroup" href="#" class="Link-Btn">Add Row</a><span style="padding-left: 8px;
                                                    padding-right: 6px;">|</span>
                                                <asp:LinkButton ID="lbtnRemoveGroup" runat="server" OnClientClick="return IsRowSelected(1, 'group');"
                                                    OnClick="lbtnRemoveGroup_Click" CssClass="Link-Btn">Remove</asp:LinkButton>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
            
                        </div>
                    </div>
                    <div style="display: none;">
                        <div id="dialog1" title="User Loan Rep Selection">
                            <iframe id="iframeUL" name="iframeUL" frameborder="0" width="100%" height="100%">
                            </iframe>
                        </div>
                        <div id="dialog2" title="Group Selection">
                            <iframe id="iframeGroup" name="iframeGroup" frameborder="0" width="100%" height="100%">
                            </iframe>
                        </div>
                        <div id="dialog3" title="User Selection">
                            <iframe id="iframeReassignUser" name="iframeReassignUser" frameborder="0" width="100%" height="100%">
                            </iframe>
                        </div>
                    </div>
                </div>
        </div>
    </div>
    </form>
</body>
</html>
