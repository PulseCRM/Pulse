<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Loan Detail View" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master"
    AutoEventWireup="true" Inherits="LoanDetails_LoanDetails" CodeBehind="LoanDetails.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/featuredcontentglider.js" type="text/javascript"></script>
    <link href="../css/featuredcontentglider.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">

        // div index of glider
        var idivIndex = 0;

        jQuery(document).ready(function () {
            var iIndex = 0;
            var iValue = 0;
            var iCurrentID = jQuery("#<%= this.hfID.ClientID %>").val();
            var iIDs = jQuery("#<%= this.hfIDs.ClientID %>").val();
            var sTab = GetQueryString1("tab");
            var arrIDs = iIDs.split(",");
            if (iCurrentID == 0 || iCurrentID == "") {
                jQuery("#aPrev").hide();
                jQuery("#aNext").hide();
            }

            jQuery.each(arrIDs, function (i, n) {
                //alert(iIDs+"-->"+n);
                if (iCurrentID == n) {
                    iIndex = i;
                }
            });

            var tmpIndex = iIndex;

            jQuery(".glidecontent iframe").each(function (i, frame) {
                //tmpIndex = iIndex; 

                var iframeid = $(this).attr("id");

                if (i == 0) {
                    //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + iCurrentID + "&tab=" + sTab);
                    window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + iCurrentID + "&tab=" + sTab);
                }
                if (i == 1) {
                    iIndex = iIndex + 1;
                    CheckIndex(iIndex);
                    window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab);

                    iIndex = tmpIndex;
                }
                if (i == 2) {
                    iIndex = iIndex - 1;
                    CheckIndex(iIndex);
                    window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab);

                    iIndex = tmpIndex;
                }
            });


            function CheckIndex(i) {
                if (i == arrIDs.length) {
                    iIndex = 0;
                }
                if (i == -1) {
                    iIndex = arrIDs.length - 1;
                }
            };

            function CheckDivIndex(i) {
                if (i > 2) {
                    idivIndex = 0;
                }
                if (i < 0) {
                    idivIndex = 2;
                }
            };

            function AttatchFrame(i) {

                var sTab1 = GetQueryString1("tab");
                if (i == 1) {
                    tmpIndex = iIndex;
                    jQuery(".glidecontent iframe").each(function (i, frame) {

                        var iframeid = $(this).attr("id");

                        if (i == 0) {
                            iIndex = iIndex - 1;
                            CheckIndex(iIndex);
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);

                            iIndex = tmpIndex;
                        }
                        if (i == 2) {
                            iIndex = iIndex + 1;
                            CheckIndex(iIndex);
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);

                            iIndex = tmpIndex;
                        }
                    });
                }
                else if (i == 0) {
                    tmpIndex = iIndex;
                    jQuery(".glidecontent iframe").each(function (i, frame) {

                        var iframeid = $(this).attr("id");

                        if (i == 1) {
                            iIndex = iIndex + 1;
                            CheckIndex(iIndex);
                            window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);

                            iIndex = tmpIndex;
                        }
                        if (i == 2) {
                            iIndex = iIndex - 1;
                            CheckIndex(iIndex);
                            window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);

                            iIndex = tmpIndex;
                        }

                    });
                }
                else if (i == 2) {
                    tmpIndex = iIndex;
                    jQuery(".glidecontent iframe").each(function (i, frame) {

                        var iframeid = $(this).attr("id");

                        if (i == 0) {
                            iIndex = iIndex + 1;
                            CheckIndex(iIndex);
                            window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);

                            iIndex = tmpIndex;
                        }

                        if (i == 1) {
                            iIndex = iIndex - 1;
                            CheckIndex(iIndex);
                            window.document.getElementById(iframeid).contentWindow.location.replace("LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1);

                            iIndex = tmpIndex;
                        }
                    });
                }
            };

            jQuery("#p-select a").each(function () {
                jQuery(this).click(function () {
                    if (jQuery(this).attr("class") == "next") {
                        iIndex = iIndex + 1;
                        CheckIndex(iIndex);
                        idivIndex = idivIndex + 1;
                        CheckDivIndex(idivIndex)
                        //iValue = iValue + 1;
                    }
                    else {
                        iIndex = iIndex - 1;
                        CheckIndex(iIndex);
                        idivIndex = idivIndex - 1;
                        CheckDivIndex(idivIndex)
                        //iValue = iValue - 1;
                    };
                    //alert(idivIndex);
                    AttatchFrame(idivIndex);
                    jQuery("#<%= this.hfID.ClientID %>").val(arrIDs[iIndex]);
                });
            });

            featuredcontentglider.init({
                gliderid: "canadaprovinces", //ID of main glider container
                contentclass: "glidecontent", //Shared CSS class name of each glider content
                togglerid: "p-select", //ID of toggler container
                remotecontent: "", //Get gliding contents from external file on server? "filename" or "" to disable
                selected: 0, //Default selected content index (0=1st)
                persiststate: false, //Remember last content shown within browser session (true/false)?
                speed: 500, //Glide animation duration (in milliseconds)
                direction: "rightleft", //set direction of glide: "updown", "downup", "leftright", or "rightleft"
                autorotate: false, //Auto rotate contents (true/false)?
                autorotateconfig: [3000, 2] //if auto rotate enabled, set [milliseconds_btw_rotations, cycles_before_stopping]
            });

            function Cancel() {
                window.location.href = jQuery("#<%= this.hfPageFrom.ClientID %>").val();
                return false;
            }

            jQuery("#aBack").click(function () {
                Cancel();
                return false;
            });
        }); 

    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            DrawTab();
        });

        // go to prospect details
        function LoanViewToProspectDetails() {

            var CurrentFileID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_hfID").val();
            //alert("CurrentFileID: " + CurrentFileID);

            var HrefEncode = $.base64Encode(window.location.href);

            window.location.href = "../Prospect/LoanViewToProspectView.aspx?FromPage=" + HrefEncode + "&FileID=" + CurrentFileID;
        }

        // go to lead details
        function LoanViewToProspectLoanDetails() {

            var CurrentFileID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_hfID").val();
            //alert("CurrentFileID: " + CurrentFileID);

            var HrefEncode = $.base64Encode(window.location.href);

            window.location.href = "LoanViewToProspectLoanView.aspx?FromPage=" + HrefEncode + "&FileID=" + CurrentFileID;
        }

        function RefreshLoanDetailInfo() {

            //alert(idivIndex);
            if ($.browser.msie == true) {   // ie

                window.document.frames[idivIndex].document.frames["ifrLoanInfo"].location.reload();
            }
            else {   // firefox

                var CurrentIFrameID = $(".glidecontent iframe").eq(idivIndex).attr("id");
                //alert(iframeid);
                window.document.getElementById(CurrentIFrameID).contentDocument.getElementById('ifrLoanInfo').contentWindow.location.reload();
            }
        }

        function RefreshContactsTab() {
            //SetTab('LoanContactsTab.aspx', 7);
            //alert(idivIndex);
            if ($.browser.msie == true) {   // ie

                window.document.frames[idivIndex].document.frames["tabFrame"].location.reload();
            }
            else {   // firefox

                var CurrentIFrameID = $(".glidecontent iframe").eq(idivIndex).attr("id");
                //alert(CurrentIFrameID);
                //window.document.getElementById(CurrentIFrameID).contentDocument.getElementById('tabFrame').contentWindow.location.reload();
                window.document.getElementById(CurrentIFrameID).contentDocument.getElementById('tabFrame').src = window.document.getElementById(CurrentIFrameID).contentDocument.getElementById('tabFrame').src;
            }
        }

        function RefreshConditionsTab() {
            
            if ($.browser.msie == true) {   // ie

                window.document.frames[idivIndex].document.frames["tabFrame"].document.frames["tabFrame"].location.reload();
            }
            else {   // firefox

                var CurrentIFrameID = $(".glidecontent iframe").eq(idivIndex).attr("id");
                window.document.getElementById(CurrentIFrameID).contentDocument.getElementById('tabFrame').contentDocument.getElementById('tabFrame').src = window.document.getElementById(CurrentIFrameID).contentDocument.getElementById('tabFrame').contentDocument.getElementById('tabFrame').src;
            }
        }



        function ShowWaitingDialog3(WaitingMsg) {

            $("#WaitingMsg3").text(WaitingMsg);
            $.blockUI({ message: $('#divWaiting3'), css: { width: '450px', padding: '7px'} });
        }

        function CloseWaitingDialog3() {

            $.unblockUI();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div class="JTab" style="margin-top: 5px; border: solid 0px red; width: 1102px; margin-bottom: 20px;">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <ul>
                            <li><a href="javascript: LoanViewToProspectDetails()"><span>Client View</span></a></li>
                            <li><a href="javascript: LoanViewToProspectLoanDetails()"><span>Lead View</span></a></li>
							<li id="current"><a href="javascript:window.location.href=window.location.href"><span>Loan View</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <div id="TabBody">
            <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
            <div id="TabLine2" class="TabRightLine">&nbsp;</div>
            <div class="TabContent" style="padding: 10px;">
						    <div id="p-select" class="glidecontenttoggler" style="width: 1087px;">
						        <table cellpadding="0" cellspacing="0">
						            <tr>
						                <td style="padding-left: 15px;">
						                    <a id="aPrev" class="prev">
						                        <img src="../images/loan/Arrow-Left.gif" alt="" />
						                    </a>
						                </td>
						                <td style="padding-left: 7px;">
						                    <a id="aNext" class="next">
						                        <img src="../images/loan/ArrowRight.gif" alt="" />
						                    </a>
						                </td>
						                <td style="padding-left: 10px;">
						                    <a id="aBack" href="../Pipeline/PipelineSummary.aspx">Back to List </a>
						                </td>
						            </tr>
						        </table>
						    </div>
						    <div id="canadaprovinces" class="glidecontentwrapper" style="margin-top: 5px; width: 1087px; height: 1050px; border: solid 0px red;">
						        <div class="glidecontent" style="border: none; width: 1090px;">
						            <iframe id="ifrGlider1" frameborder="0" style="border: solid 0px blue; height: 1045px; width: 1085px;"></iframe>
						        </div>
						        <div class="glidecontent" style="border: none; width: 1090px;">
						            <iframe id="ifrGlider2" frameborder="0" style="border: solid 0px blue; height: 1045px; width: 1085px;"></iframe>
						        </div>
						        <div class="glidecontent" style="border: none; width: 1090px;">
						            <iframe id="ifrGlider3" frameborder="0" style="border: solid 0px blue; height: 1045px; width: 1085px;"></iframe>
						        </div> 
						    </div>
						</div>
        </div>
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
    <asp:HiddenField ID="hfIDs" runat="server" />
    <asp:HiddenField ID="hfID" runat="server" />
    <asp:HiddenField ID="hfPageFrom" runat="server" />
</asp:Content>