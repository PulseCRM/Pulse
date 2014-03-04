<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SubmitLoanPopup.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Prospect.SubmitLoanPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <base target="_self" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.treeview.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.treeview.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {


        });

        function BeforeSumbit() {
            ShowWaitingDialog("Submitting the loan…");
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = "<%=iLoanID %>";
            var Program = $("#" + '<%=ddlProgram.ClientID %>').val();
            var Type = $("#" + '<%=ddlType.ClientID %>').val();
            // Ajax
            $.getJSON("SubmitLoanPopup_Background.aspx?sid=" + Radom + "&loanID=" + LoanID + "&program=" + Program + "&type=" + Type, AfterTaskComplete);
            return false;
        }


        function AfterTaskComplete(data) {

            if (data.ExecResult == "Failed") {
                $('#divContainer').unblock();
                alert(data.ErrorMsg);
                RefreshPage();
                return;
            }
            $("#hdnErrMsg").val(data.ErrorMsg);
            CloseLoan(data);
        }

        function CloseLoan(data) {
            setTimeout("CloseWaitingDialog('Submitting the loan successfully.')", 500);
        }

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '400px'} });
        }


        function CloseWaitingDialog(SuccessMsg) {
            var Msg = $('#hdnErrMsg').val();

            if (Msg.length <= 0)
                Msg = SuccessMsg;
            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(Msg);
            $('#aClose').show();
        }

        function RefreshPage() {

            $("#divWaiting").dialog("close");
            window.parent.CloseGlobalPopup();
        }

        function ShowDialog_WorkflowTemplateSelection() {

            var LoanID = 335;
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Prospect/DisposeWorkflowTemplateList.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 601
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.CloseGlobalPopup();
            window.parent.ShowGlobalPopup("Workflow Template Selection", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 480px; height: 420px; border: solid 0px red;">
        
            <table >
                <tr>
                    <td style="margin-left:5px" colspan="3">
                        <b>Submit Loan Application</b>
                    </td>
                </tr>
                <tr>
                    <td style="margin-left:5px" colspan="3">
                       &nbsp;
                    </td>
                </tr>
                <tr>
                     <td style="width:20px; height:28px" >
                       &nbsp;
                    </td>
                    <td style="margin-left:5px" >
                        Loan Program:
                    </td>
                    <td style="margin-left:5px">
                        <asp:DropDownList ID="ddlProgram" runat="server" style=" width:150px;" ></asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                     <td style="width:20px;height:28px" >
                       &nbsp;
                    </td>
                    <td style="margin-left:5px" >
                        Originator Type:
                    </td>
                    <td style="margin-left:5px">
                        <asp:DropDownList ID="ddlType" runat="server" style=" width:150px;" >
                            <asp:ListItem Text="Branch" Value="Branch" Selected="True" ></asp:ListItem>
                            <asp:ListItem Text="Lender(B)" Value="Lender(B)" ></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
         <div style="padding-top: 5px;margin-left:25px">
            <asp:Button ID="btnYes" runat="server" Text="Submit" CssClass="Btn-66" OnClientClick="return BeforeSumbit();" onclick="btnYes_Click" />
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
    </div>
    <asp:HiddenField ID="hdFileID" runat="server" />
     <input id="hdnErrMsg" runat="server" type="text" style="display: none;"/>
    </form>
</body>
</html>
