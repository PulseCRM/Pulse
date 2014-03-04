<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerContactDetailInfo.aspx.cs"
    Inherits="PartnerContactDetailInfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" /> 
    <script src="../js/urlparser.js" type="text/javascript"></script>   
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {


        });

        function UpdateContact() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&ContactID=" + $("#<%=hdnContactID.ClientID %>").val() + "&CloseDialogCodes=window.parent.parent.CloseGlobalPopup()";

            var BaseWidth = 700;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        //#region Bug840 neo

        function btnDelete_onclick() {

            var result = confirm("Deleting a partner contact will also delete the history information.\r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return;
            }

            window.parent.ShowWaitingDialog("Checking if partner contact's being referenced...");

            var ContactID = GetQueryString1("ContactID");

            // Ajax - check whether contact is ref. or not
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);
            $.getJSON("CheckContactRef.ashx?sid=" + Radom + "&ContactID=" + ContactID, AfterCheckContactRef);
        }

        function AfterCheckContactRef(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    window.parent.CloseWaitingDialog();
                    return;
                }

                if (data.IsRef == "true") {

                    alert("The current partner contact has been assigned roles in the loans. You will need to assign another partner contact to replace him/her in the loans.");

                    window.parent.CloseWaitingDialog()

                    ShowDialog_AssignContactBeforeDelete();
                }
                else {

                    DeleteContact();
                }

            }, 2000);
        }

        function ShowDialog_AssignContactBeforeDelete() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var DelContactID = GetQueryString1("ContactID");

            var iFrameSrc = "AssignContactBeforeDelete.aspx?DelContactID=" + DelContactID + "&sid=" + RadomStr;

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 510;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.ShowGlobalPopup("Search and Assign Contact", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function DeleteContact() {

            window.parent.ShowWaitingDialog("Deleting...");

            var DelContactID = GetQueryString1("ContactID");
            $.getJSON("DeletePartnerContact_Background.aspx?DelContactID=" + DelContactID, AfterDeleteContact);
        }

        function AfterDeleteContact(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    window.parent.CloseWaitingDialog()
                    return;
                }

                alert("Deleted selected contact successfully.");

                window.parent.location.href = window.parent.location.href;

            }, 2000);
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
                    <h4 id="hProspectName" runat="server" style="margin: 0px; color: #5880b3">
                        Partner Contact Detail</h4>
                </td>
            </tr>
        </table>
        <table style="margin-top: 5px;">
            <tr>
                <td>
                    <input id="btnUpdate" type="button"  class="Btn-91" onclick="window.parent.ShowDialog_UpdateContact();" value="Update" />
                </td>
                <td style="margin-left: 15px;">
                    <input id="btnDelete" type="button" value="Delete" onclick="btnDelete_onclick()" class="Btn-66" />
                </td>
                <td style="margin-left: 15px;">
                    <asp:Button ID="btnSaveAsVCard" runat="server" Text="Save as vCard" CssClass="Btn-115" OnClick="btnSaveAsVCard_Click"/>
                </td>
            </tr>
        </table>
        <table cellpadding="2" cellspacing="3" style="margin-top: 10px; margin-left: 6px;"
            border="0">
            <tr>
                <td style="width: 320px;">
                    Name:
                    <asp:Label ID="lbName" runat="server" Text=""></asp:Label>
                </td>
                <td style="width: 8px;">
                    &nbsp;
                </td>
                <td>
                    Enabled:<asp:Label ID="lbEnabled" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Service Type:
                    <asp:Label ID="lbServiceType" runat="server" Text=""></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Company:
                    <asp:Label ID="lbCompany" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Bussiness Phone:
                    <asp:Label ID="lbBussinessPhone" runat="server" Text=""></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Branch:
                    <asp:Label ID="lbBranch" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Cell Phone:
                    <asp:Label ID="lbCellPhone" runat="server" Text=""></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Phone:
                    <asp:Label ID="lbPhone" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Fax:
                    <asp:Label ID="lbFax" runat="server" Text=""></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Fax:
                    <asp:Label ID="lbFax1" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Email:
                    <asp:Label ID="lbEmail" runat="server" Text=""></asp:Label>
                </td>
                <td>
                </td>
                <td>
                    Address:
                    <asp:Label ID="lbAddress" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Total Referral Amount: <a id="aTotalReferral" runat="server" href="" style="text-decoration: underline;"></a></td>
                <td></td>
                <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lbCity" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td>Total Referral Funded: <asp:Label ID="lbTotalReferralFunded" runat="server" Text="$1,097,200"></asp:Label></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>Win Ratio: <asp:Label ID="lbWinRatio" runat="server" Text="100%"></asp:Label></td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </div>
    
    <asp:HiddenField ID="hdnContactID" runat="server" />
    </form>
</body>
</html>
