<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Title="Partner Contact Detail" Language="C#"  AutoEventWireup="true" CodeBehind="PartnerContactDetailView.aspx.cs"
 Inherits="PartnerContactDetailView" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master"
    %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {
            DrawTab();

            //set the default display tab page
            if ($("#<%= hfDefaultTab.ClientID %>").val() != "") {
                SetTab($("#<%= hfDefaultTab.ClientID %>").val() + ".aspx", $("#<%= hfTabIndex.ClientID %>").val());
            } else {
                SetTab('PartnerContactDetailNotesTab.aspx', 0);
            }
        });

        function SetTab(src, i) {
            //            var ContactID = $("#<%= hfContactID.ClientID %>").val();
            //            if (i == 1) {
            //                // Start: add by peter
            //                var RadomNum = Math.random();
            //                var RadomStr = RadomNum.toString().substr(2);
            //                $("#tabFrame").attr("src", "../Prospect/ProspectDetailEmailTab.aspx?from=3&itemid=" + ContactID + "&sid=" + RadomStr);
            //                // End : add by peter

            //            }
            //            else {
            $("#tabFrame").attr("src", SetSrc(src));
            //            }

            $("#tabs10 #current").removeAttr("id");
            $("#tabs10 ul li").eq(i).attr("id", "current");
            DrawTab();
        }

        function SetSrc(src) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var ContactID = jQuery("#<%= this.hfContactID.ClientID %>").val();
            src = src + "?sid=" + RadomStr + "&ContactID=" + ContactID;

            return src
        }

        function GoToPartnerContactsPage() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerContactsPage.aspx?sid=" + RadomStr;
        }

        function ShowDialog_UpdateContact() {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var iFrameSrc = "PartnerContactsSetupPopup.aspx?sid=" + RadomStr + "&ContactID=" + $("#<%=hfContactID.ClientID %>").val() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 820;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 680;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Contact Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);

        }

        //#region show/close waiting

        function ShowWaitingDialog3(WaitingMsg) {

            $("#WaitingMsg3").text(WaitingMsg);
            $.blockUI({ message: $('#divWaiting3'), css: { width: '450px', padding: '7px'} });
        }

        function CloseWaitingDialog3() {

            $.unblockUI();
        }

        //#endregion
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div style="width: 1060px; border: solid 1px #e4e7ef; padding: 10px; padding-top: 10px;">
        <div id="content">
            <iframe id="ifrLoanInfo" frameborder="0" scrolling="no" style="width: 1056px; height: 230px; border: solid 0px red;" src="PartnerContactDetailInfo.aspx?ContactID=<%= this.sContactID %>"></iframe>
        </div>
        <div class="JTab" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="" onclick="SetTab('PartnerContactDetailNotesTab.aspx',0);return false;"><span>Notes</span></a></li>
                                <li><a href="" onclick="SetTab('PartnerContactDetailEmailTab.aspx',1);return false;"><span>Emails</span></a></li>
                                <li><a href="" onclick="SetTab('PartnerContactLoans.aspx',2);return false;"><span>Loans</span></a></li>
                                <li><a href="" onclick="SetTab('PartnerContactDetailReferralstab.aspx',3);return false;"><span>Referrals</span></a></li>
                                <li><a href="" onclick="SetTab('ParnerContactActivityTab.aspx',4);return false;"><span>Activity History</span></a></li>
                                <li><a href="" onclick="SetTab('PartnerContactDetailMailChimpTab.aspx',5);return false;"><span>Marketing</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine">
                    &nbsp;</div>
                <div class="TabContent">
                    <iframe id="tabFrame" frameborder="0" style="border: solid 0px blue; height: 610px;
                        width: 1035px;"></iframe>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfDefaultTab" runat="server" />
        <asp:HiddenField ID="hfTabIndex" runat="server" />
    </div>
    <div id="divWaiting3" style="display: none;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting3" src="../images/waiting.gif" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg3" style="color: #818892; font-weight: bold;"></label>
				</td>
			</tr>
		</table>
	</div>
    <asp:HiddenField ID="hfContactID" runat="server" />
    <asp:HiddenField ID="hfIDs" runat="server" />
    <asp:HiddenField ID="hfID" runat="server" />
    <asp:HiddenField ID="hfPageFrom" runat="server" />
</asp:Content>