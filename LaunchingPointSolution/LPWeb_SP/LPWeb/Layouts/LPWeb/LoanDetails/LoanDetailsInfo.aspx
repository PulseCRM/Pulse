<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailsInfo.aspx.cs" Inherits="LoanDetails_LoanDetailsInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.tabs.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/loan_details_progress.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery-ui-1.8.5.custom.min.js"></script>
	<script type="text/javascript" src="../js/jquery.wt-scroller.js"></script>
    <script type="text/javascript" src="../js/loan_details_progress.js"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>

    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

          function initCfmBox(id, title, msg, w, h, callBack) {
            $('#' + id).dialog({
                modal: true,
                autoOpen: false,
                width: w,
                height: h,
                resizable: false,
                title: title,
                buttons: {
                    Yes: function () {
                        callBack();
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                        clearFolderSelWin(id);
                    }
                },
                open: function () {
                    $('.ui-dialog-buttonset').css('float', 'none');
                    $('.ui-dialog-buttonset').css('text-align', 'center');
                    $('.ui-dialog-buttonpane').find('button').addClass('Btn-66');
                }
            });
          }

         initCfmBox('dialog1', 'Point Folder Selection', '', 500, 450, function () {
                if (window.frames.ifrPopup && window.frames.ifrPopup.returnFn) {
                    window.frames.ifrPopup.returnFn();
                }
            });
        });

        function onMoveClick() {

            // show waiting
            window.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");
            
            var iLoanID = "<%=iLoanID %>";
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // check point file locked
            $.getJSON("CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + iLoanID, function(data){
            
                window.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {
                    
                    alert(data.ErrorMsg);
                    return false;
                }
                else {
                    
                    // if locked
                    if(data.ErrorMsg != ""){
                    
                        alert(data.ErrorMsg);
                        return false;
                    }
                    else{
                    
                    	// continue
			            var sBranchID = "<%=sBranchID %>";
			            var sStatus = "<%=sStatus %>";
			            var iFrameSrc ="../Pipeline/PointFolderSelectionForDetails.aspx?fid=" + iLoanID + "&tls=" + sStatus + "&bid=" + sBranchID + "&t=" + Math.random().toString() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";
			            window.parent.parent.ShowGlobalPopup("Point Folder Selection", '100%','100%', 500, 500, iFrameSrc);
                    }
                }
            });
        }

        function CloseGlobalPopup() {

            $("#divGlobalPopup").dialog("close");
        }

        function RefreshLoanDetailInfo() {
//        alert("RefreshLoanDetailInfo()");
//            alert(idivIndex);
//            $("#divGlobalPopup").dialog("close");
            if ($.browser.msie == true) {   // ie

                window.document.frames[idivIndex].document.frames["ifrLoanInfo"].location.reload();
            }
            else {   // firefox

                var CurrentIFrameID = $(".glidecontent iframe").eq(idivIndex).attr("id");
                //alert(iframeid);
                window.document.getElementById(CurrentIFrameID).contentDocument.getElementById('ifrLoanInfo').contentWindow.location.reload();
            }
        }

        function getFolderSelectionReturn(sReturn)
        {
            $("#"+'<%=hiSelectedFolderId.ClientID %>').val(sReturn);
            $('#dialog1').dialog('close');
            <%=this.ClientScript.GetPostBackEventReference(this.btnRefresh, null) %>
            clearFolderSelWin('dialog1');
        }

        function clearFolderSelWin(id) {
            var f = "";
            if ("dialog1" == id)
                f = document.getElementById('iframePF');
            f.src = "about:blank";
        }

        // CR38
        function btnLockRate_onclick(){
        
            var bShowLockRatePopup = $("#<%= hdnShowLockRatePopup.ClientID %>").val();
            if(bShowLockRatePopup != "true"){ 
                    alert("You do not have the privilege to view lock info, lock rate or extend rate lock.");
                    return false;
            }
            window.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");


            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var FileId = "<%= iLoanID %>";

            $.getJSON("CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileId, function(data)
            {
            
                window.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") 
                {
                    
                    alert(data.ErrorMsg);
                    return false;
                }
                else 
                {
                    
                    // if locked
                    if(data.ErrorMsg != "")
                    {
                    
                        alert(data.ErrorMsg);
                        var iFrameSrc = "LockRatePopup.aspx?FileId="+FileId+"&sid="+sid+"&locked=1" + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 620;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Lock Rate", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

                        return false;
                    }
                    else
                    {


            var iFrameSrc = "LockRatePopup.aspx?FileId="+FileId+"&sid="+sid;

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 620;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Lock Rate", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            }
                }
            });
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="border: solid 0px green; padding-left: 10px;">
        <table cellpadding="0" cellspacing="0" style="margin-left: 10px;">
            <tr>
                <td><h4 id="hProspectName" runat="server" style="margin: 0px; color: #5880b3">Loan Detail - Doe, John E</h4></td>
                <td style="padding-left: 40px;">
                    <div id="divLoanDetailsProgress" class="container">
                        <div class="wt-scroller">
        	                <div class="prev-btn"></div>          
        	                <div class="slides">
            	                <ul>
                                    <asp:Repeater ID="rptStageProgressItems" runat="server">
                                        <ItemTemplate>
                                            <li title="<%# this.GetStageToolTip(Eval("StageAlias").ToString(), Eval("Completed").ToString()) %>">
                                                <a><img src="../images/loan_details_progress/<%# Eval("StageImage") %>" alt=""/></a>
                                                <p><%# this.GetSpan_StageAliasCompleteDate(Eval("StageAlias").ToString(), Eval("StageImage").ToString(), Eval("Completed").ToString())%></p>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </ul>
        	                </div>          	
     		                <div class="next-btn"></div>
        	                <div class="lower-panel"></div>
		                </div>   
                    </div>
                </td>
            </tr>
        </table>

        <table style="margin-top: 5px;">
            <tr>
                <td>
                    <asp:Button ID="btnSyncNow" runat="server" Text="Sync Now" CssClass="Btn-91" OnClick="btnSyncNow_Click" />
                </td>
                 <td>
                    <input id="btnMove" type="button" value="Move File" class="Btn-91" onclick="onMoveClick();"  />
                </td>
                <td>
                    <input id="btnLockRate" type="button" value="Lock Rate" class="Btn-91" onclick="btnLockRate_onclick();"  />
                </td>
            </tr>
        </table>

        <table cellpadding="2" cellspacing="3" style="margin-top: 10px; margin-left: 6px;" border="0">
            <tr>
                <td style="width: 320px;">Status: <asp:Label ID="lbStatus" runat="server" Text="Label"></asp:Label></td>
                <td style="width: 8px;">&nbsp;</td>
                <td>Loan Amount: $<asp:Label ID="lbLoanAmount" runat="server" Text="Label"></asp:Label></td>
                <td>Funding Date:<asp:Label ID="lbFundingDate" runat="server" Text=""></asp:Label> </td>
            </tr>
            <tr>
                <td>Borrower: <asp:Label ID="lbBorrower" runat="server" Text="Label"></asp:Label></td>
                <td></td>
                <td>Interest Rate: <asp:Label ID="lbInterestRate" runat="server" Text="Label"></asp:Label>%</td>
                <td>Note Date:<asp:Label ID="lbNotDate" runat="server" Text=""></asp:Label> </td>
            </tr>
            <tr>
                <td>Coborrower: <asp:Label ID="lbCoborrower" runat="server" Text="Label"></asp:Label></td>
                <td></td>
                <td>Lien Position: <asp:Label ID="lbLienPosition" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>Loan Officer: <asp:Label ID="lbLoanOfficer" runat="server" Text="Label"></asp:Label></td>
                <td></td>
                <td>Purpose: <asp:Label ID="lbPurpose" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>Property: <asp:Label ID="lbProperty" runat="server" Text="Label"></asp:Label></td>
                <td></td>
                <td>Loan Program: <asp:Label ID="lbLoanProgram" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>County: <asp:Label ID="lbCountry" runat="server" Text="Label"></asp:Label></td>
                <td></td>
                <td>Down Payment: <asp:Label ID="lbDownPayment" runat="server" Text="Label"></asp:Label>%</td>
            </tr>
            <tr>
                <td>Point File: <asp:Label ID="lbPointFile" runat="server" Text="Label"></asp:Label></td>
                <td></td>
                <td>Estimated Close Date: <asp:Label ID="lbEstCloseDate" runat="server" Text="Label"></asp:Label></td>
            </tr>
            <tr>
                <td>LOS Loan Officer: <asp:Label ID="lbLOSLoanOfficer" runat="server" Text=""></asp:Label></td>
                <td><asp:Image ID="imgRateLock" runat="server" /></td>
                <td>
                    Rate Lock Expiration: <asp:Label ID="lbRateLock" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
	    
    </div>
    <ul id="divDisposeMenu" class="contextMenu">
        <asp:Literal ID="ltrMenuItems" runat="server"></asp:Literal>
    </ul>
    <asp:HiddenField ID="hdnActiveLoan" runat="server" />
    <asp:HiddenField ID="hiSelectedFolderId" runat="server" />
    <asp:HiddenField ID="hdnShowLockRatePopup" runat="server" />
     <div id="dialog1" title="Point Folder Selection">
            <iframe id="iframePF" name="iframePF" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
    <div style="display:none" >
        <asp:Button ID="btnRefresh" OnClick="btnRefresh_Click" runat="server" />
    </div>
    </form>
</body>
</html>
