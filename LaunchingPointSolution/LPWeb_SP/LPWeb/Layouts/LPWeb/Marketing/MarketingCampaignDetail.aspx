<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarketingCampaignDetail.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Marketing.MarketingCampaignDetail"%>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
     <script src="../js/jquery.js" type="text/javascript"></script>

    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />

    <script src="../js/featuredcontentglider.js" type="text/javascript"></script>

    <link href="../css/featuredcontentglider.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <style>
         html {     overflow:-moz-scrollbars-vertical; }

    </style>

    <script type="text/javascript">

        // div index of glider
        var idivIndex = 0;

        jQuery(document).ready(function () {
            var iIndex = 0;
            var iValue = 0;
            var iCurrentID = jQuery("#<%= this.hfID.ClientID %>").val();
            var iIDs = jQuery("#<%= this.hfIDs.ClientID %>").val();
            var sTab = jQuery("#<%= this.hfDefaultTab.ClientID %>").val();
            var iTabIdx = jQuery("#<%= this.hfTabIndex.ClientID %>").val();
            var arrIDs = iIDs.split(",");
            //            alert(iIDs);
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

            //            if ($.browser.msie == true) {

            //            }
            //            else {   // not 

            //                jQuery("#divGlider1").css("overflow", "auto");
            //                jQuery("#divGlider2").css("overflow", "auto")
            //                jQuery("#divGlider3").css("overflow", "auto"); ;
            //            }


            var tmpIndex = iIndex;

            jQuery(".glidecontent iframe").each(function (i, frame) {
                //tmpIndex = iIndex; 

                var iframeid = $(this).attr("id");

                if (i == 0) {
                    //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + iCurrentID + "&tab=" + sTab + "&tabIndex=" + iTabIdx);
                    window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + iCurrentID + "&tab=" + sTab + "&tabIndex=" + iTabIdx);
                }
                if (i == 1) {
                    iIndex = iIndex + 1;
                    CheckIndex(iIndex);
                    //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab + "&tabIndex=" + iTabIdx);
                    window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab + "&tabIndex=" + iTabIdx);

                    iIndex = tmpIndex;
                }
                if (i == 2) {
                    iIndex = iIndex - 1;
                    CheckIndex(iIndex);
                    //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab + "&tabIndex=" + iTabIdx);
                    window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab + "&tabIndex=" + iTabIdx);

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

                var sTab1 = jQuery("#<%= this.hfDefaultTab.ClientID %>").val();
                var iTabIdx1 = jQuery("#<%= this.hfTabIndex.ClientID %>").val();
                window.parent.MarkRow(arrIDs[iIndex]);
                if (i == 1) {
                    tmpIndex = iIndex;
                    jQuery(".glidecontent iframe").each(function (i, frame) {

                        var iframeid = $(this).attr("id");

                        if (i == 0) {
                            iIndex = iIndex - 1;
                            CheckIndex(iIndex);
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);

                            iIndex = tmpIndex;
                        }
                        if (i == 2) {
                            iIndex = iIndex + 1;
                            CheckIndex(iIndex);
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);

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
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);

                            iIndex = tmpIndex;
                        }
                        if (i == 2) {
                            iIndex = iIndex - 1;
                            CheckIndex(iIndex);
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);

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
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);

                            iIndex = tmpIndex;
                        }

                        if (i == 1) {
                            iIndex = iIndex - 1;
                            CheckIndex(iIndex);
                            //jQuery(frame).attr("src", "LoanDetail.aspx?FileID=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);
                            window.document.getElementById(iframeid).contentWindow.location.replace("MarketingCampaignDetails.aspx?campaignId=" + arrIDs[iIndex] + "&tab=" + sTab1 + "&tabIndex=" + iTabIdx1);

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
                    //                    alert(idivIndex);
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

        });
        var getFFVersion = navigator.userAgent.substring(navigator.userAgent.indexOf("Firefox")).split("/")[1];
        var FFextraHeight = getFFVersion >= 0.1 ? 16 : 0;

        function dyniframesize(iframename) {
            var pTar = null;
            if (document.getElementById) {
                pTar = window.document.getElementById(iframename);
            }
            else {
                eval('pTar = ' + iframename + ';');
            }
            if (pTar && !window.opera) {
                if (pTar.contentDocument && pTar.contentDocument.body.offsetHeight) {
                    $("#" + iframename).height(pTar.contentDocument.body.offsetHeight + FFextraHeight + 25);
                    //pTar.style.height = pTar.contentDocument.body.offsetHeight+FFextraHeight+25;
                }
                else if (pTar.Document && pTar.Document.body.scrollHeight) {
                    $("#" + iframename).height(pTar.Document.body.scrollHeight);
                    //pTar.style.height = pTar.Document.body.scrollHeight;
                }
            }
        } 
    </script>

</head>
<body>
    <form id="Form1" runat="server">
        <div id="p-select" class="glidecontenttoggler" style="width: 680px;">
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
                </tr>
            </table>
        </div>
        <div id="canadaprovinces" class="glidecontentwrapper" style="margin-top: 5px; width: 680px;
            height: 355px; border: solid 0px red;">
            <div id="divGlider1" class="glidecontent" style="border: none; width: 100%; overflow: auto;">
                <iframe id="ifrGlider1" frameborder="0" style="border: solid 0px blue; height: 325px;
                    width: 650px;" ></iframe>
            </div>
            <div id="divGlider2" class="glidecontent" style="border: none; width: 100%; overflow: auto;">
                <iframe id="ifrGlider2" frameborder="0" style="border: solid 0px blue; height: 325px;
                    width: 650px;" ></iframe>
            </div>
            <div id="divGlider3" class="glidecontent" style="border: none; width: 100%; overflow: auto;">
                <iframe id="ifrGlider3" frameborder="0" style="border: solid 0px blue; height: 325px;
                    width: 650px;" ></iframe>
            </div>
        </div>
        <div runat="server" id="divHid">
            <asp:HiddenField ID="hfIDs" runat="server" />
            <asp:HiddenField ID="hfID" runat="server" />
            <asp:HiddenField ID="hfPageFrom" runat="server" />
            <asp:HiddenField ID="hfDefaultTab" runat="server" />
            <asp:HiddenField ID="hfTabIndex" runat="server" />
        </div>
    </form>
</body>
</html>