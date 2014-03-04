<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectDetailInfo.aspx.cs" Inherits="ProspectDetailInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        function btnModify_onclick() {

            var ContactID = GetQueryString1("ContactID");
            //gdc CR50
            var isLock = CheckPointFileStatus(ContactID,
               function () {

                   var RadomNum = Math.random();
                   var sid = RadomNum.toString().substr(2);

                   window.parent.parent.location.href = "LeadEdit.aspx?sid=" + sid + "&ContactId=" + ContactID;
               });
        }

        function ShowDialog_SendEmail() {

            var ProspectID = GetQueryString1("ContactID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "../LoanDetails/EmailSendPopup.aspx?sid=" + RadomStr + "&ProspectID=" + ProspectID + "&CloseDialogCodes=window.parent.CloseGlobalPopupPipeline()";

            window.parent.parent.ShowGlobalPopup("Send Email", 605, 530, 630, 570, iFrameSrc);
        }

        function ShowDialog_LinkLoan() {

            var ProspectID = GetQueryString1("ContactID");
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "LinkLoanDetails.aspx?sid=" + RadomStr + "&ProspectID=" + ProspectID + "&CloseDialogCodes=window.parent.CloseGlobalPopupPipeline()&RefreshCodes=window.parent.RefreshLoanDetailInfo()";

            window.parent.parent.ShowGlobalPopup("Link Loan", 775, 460, 800, 500, iFrameSrc);
        }

        function BeforeDelete() {

            var result = confirm("Deleting the prospect record will also delete the prospect and the history information. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }

            return true;
        }

        function UpdatePoint() {

            var ProspectID = GetQueryString1("ContactID");

            var isLock = CheckPointFileStatus(ProspectID,
               function () {

                   //window.parent.ShowWaitingDialog1("Please wait...");
                   $.blockUI({ message: "Please wait..." });
                   var RadomNum = Math.random();
                   var Radom = RadomNum.toString().substr(2);
                   $.getJSON("UpdateBorrower_Background.aspx?sid=" + Radom + "&ContactID=" + ProspectID, function (data) {

                       if (data.ExecResult == "Failed") {

                           alert(data.ErrorMsg);
                       }
                       else {

                           alert("The Point file(s) has been updated successfully.");
                       }
                       //window.parent.CloseWaitingDialog1();
                       $.unblockUI();
                       window.location.href = window.location.href;

                    });



                   //            $.getJSON("UpdateBorrower_Background.aspx?ContactID=" + ProspectID, function (json) {
                   //                if (json.ExecResult == 'Success') {
                   //                    alert("The Point file(s) has been updated successfully.");
                   //                }
                   //                else {
                   //                    alert(json.ErrorMsg);
                   //                }
                   //                window.parent.CloseWaitingDialog1();
                   //            });

               });
            return false;
        }
        

        // CR50 Start CheckPointFileStatus
        function CheckPointFileStatus(ContactID, Fun) {
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);
            var isAsync = false;
            if (Fun != null) {
                isAsync = true;
            }
            //var FileId = GetQueryString1("FileID");
            var res = false;
            $.blockUI({ message: "Please wait..." });
            $.ajax({
                url: "../CheckPointFileStatus_BG_JSON.aspx?sid=" + Radom + "&ContactID=" + ContactID,
                async: isAsync,
                cache: false,
                dataType: "json",
                success: function (data) {

                    if (data.ExecResult == "Failed") {
                        //$('#divDetail').unblock();
                        alert(data.ErrorMsg);
                        //return false;
                        res = false;
                    }
                    else {
                        //return true;
                        res = true;
                        Fun();
                    }
                    $.unblockUI();
                }
            });
            return res;
        }

        //End CheckPointFileStatus
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server" style="height:245px; width:1000px">
    <div id="divContainer" style="border: solid 0px green; padding-left: 10px;">
        <table>
        	<tr>
                <td style="width: 250px;">
                    <asp:Label ID="Label3" runat="server" Font-Size="Larger" Text="Client:" Style="color: #003399;
                        font-size: Larger; font-weight: bold;"></asp:Label>
                    <asp:Label ID="lbClient" runat="server" Font-Size="Larger" Text=" " Style="color: #003399;
                        font-size: Larger; font-weight: bold;"></asp:Label>
                </td>
                
                <td>
                    <input id="btnModify" runat="server" type="button" value="Modify" class="Btn-66" onclick="btnModify_onclick()" />&nbsp;
                    <input id="btnSendEmail" runat="server" type="button" value="Send Email" class="Btn-91" onclick="ShowDialog_SendEmail()" />&nbsp;
                    <input id="btnLinkLoan" runat="server" type="button" value="Link Loan" class="Btn-91" onclick="ShowDialog_LinkLoan()" />&nbsp;
                    <asp:Button ID="btnSyncNow" runat="server" Text="Sync Now" CssClass="Btn-91" OnClick="btnSyncNow_Click"/>&nbsp;
                    <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDelete();" OnClick="btnDelete_Click" />
                    <input id="btnUpdatePoint"  type="button" value="Update Point" class="Btn-91" onclick="return UpdatePoint();" />&nbsp;
                    <asp:Button ID="btnSaveAsVCard" runat="server" Text="Save as vCard" CssClass="Btn-115" OnClick="btnSaveAsVCard_Click"/>
                </td>
            </tr>
        </table>
        <table style="margin-top: 5px;">
            
            <tr>
                <td style="width: 250px;">
                    <asp:Label ID="Label1" runat="server" Text="Nikename:" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lbNikeName" runat="server" Text=" " Font-Bold="true"></asp:Label>
                </td>
                <td style="width: 500px;">
                    <asp:Label ID="Label2" runat="server" Text="Status:" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lbStatus" runat="server" Text=" " Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    DOB:
                    <asp:Label ID="lbDOB" runat="server" Text=" "></asp:Label>
                </td>
                
                <td>
                    Title:
                    <asp:Label ID="lbTitle" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Generation Code:
                    <asp:Label ID="lbGenCode" runat="server" Text=" "></asp:Label>
                </td>
                
                <td>
                    Preferred Contact:
                    <asp:Label ID="lbPreferredContact" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    SSN:
                    <asp:Label ID="lbSSN" runat="server" Text=" "></asp:Label>
                </td>
                
                <td>
                    Home Phone:
                    <asp:Label ID="lbHomePhone" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Credit Ranking:
                    <asp:Label ID="lbCreditRanking" runat="server" Text=" "></asp:Label>
                </td>
                
                <td>
                    Cell Phone:
                    <asp:Label ID="lbCellPhone" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Experian/FICO score:
                    <asp:Label ID="lbExperScore" runat="server" Text=" "></asp:Label>
                </td>
                <td>
                    Business Phone:
                    <asp:Label ID="lbBusinessPhone" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    TransUnion/Emprica score:
                    <asp:Label ID="lbTranScore" runat="server" Text=" "></asp:Label>
                </td>
                <td>
                    Fax:
                    <asp:Label ID="lbFax" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Equifax/BEACON score:
                    <asp:Label ID="lbEquifax" runat="server" Text=" "></asp:Label>
                </td>
                <td>
                    Email:
                    <asp:Label ID="lbEmail" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Lead Source:
                    <asp:Label ID="lbLeadSource" runat="server" Text=" "></asp:Label>
                </td>
                <td rowspan="2">
                    <table style="margin-left: -3px; padding-left: 0px;">
                        <tr>
                            <td>
                                Address:
                            </td>
                            <td>
                                <asp:Label ID="lbAddress" runat="server" Text=" "></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Label ID="lbAddress1" runat="server" Text=" "></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    Loan Officer:
                    <asp:Label ID="lbLoanOfficer" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    Referral:
                    <asp:Label ID="lbReferral" runat="server" Text=" "></asp:Label>
                </td> 
            </tr>
            <tr>
                <td>
                    Created on:
                    <asp:Label ID="lbCreatedOn" runat="server" Text=" "></asp:Label>
                </td>
                <td>
                    Last Modified:
                    <asp:Label ID="lbLastModified" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Created By:
                    <asp:Label ID="lbCreatedBy" runat="server" Text=" "></asp:Label>
                </td>
                <td>
                    Modified By:
                    <asp:Label ID="lbModifiedBy" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
