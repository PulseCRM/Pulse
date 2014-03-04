<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailMailChimpTab.aspx.cs" Inherits="LoanDetails_LoanDetailMailChimpTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Marketing - Mail Chimp</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            DrawTab();

            // left click context menu
            $("#aSubscribe").contextMenu({ menu: 'divSubscribeMenu', leftButton: true }, function (action, el, pos) { contextMenuWork1(action, el, pos); });

            $("#aUnsubscribe").contextMenu({ menu: 'divUnsubscribeMenu', leftButton: true }, function (action, el, pos) { contextMenuWork2(action, el, pos); });

        });

        // left click context menu
        function contextMenuWork1(action, el, pos) {

            switch (action) {
                case "Both":
                    {
                        aSubscribe_onclick("Both");
                        break;
                    }
                case "Borrower":
                    {
                        aSubscribe_onclick("Borrower");
                        break;
                    }

                case "CoBorrower":
                    {
                        aSubscribe_onclick("CoBorrower");
                        break;
                    }
            }
        }

        function contextMenuWork2(action, el, pos) {

            switch (action) {
                case "Both":
                    {
                        aUnsubscribe_onclick("Both");
                        break;
                    }
                case "Borrower":
                    {
                        aUnsubscribe_onclick("Borrower");
                        break;
                    }

                case "CoBorrower":
                    {
                        aUnsubscribe_onclick("CoBorrower");
                        break;
                    }
            }
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridContactMailCampaignList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridContactMailCampaignList tr td :checkbox").attr("checked", "");
            }
        }

        var ProspectIDs = "";
        function aSubscribe_onclick(action) {

            var BorrowerID = $("#hdnBorrowerID").val();
            var CoBorrowerID = $("#hdnCoBorrowerID").val();

            if (BorrowerID == "" && CoBorrowerID == "") {

                alert("There is no borrower and co-borrower in this loan.");
                return;
            }

            if (action == "Both") {

                if (BorrowerID != "" && CoBorrowerID != "") {

                    ProspectIDs = BorrowerID + "," + CoBorrowerID;
                }
                else if (BorrowerID != "" && CoBorrowerID == "") {

                    ProspectIDs = BorrowerID;
                }
                else if (BorrowerID == "" && CoBorrowerID != "") {

                    ProspectIDs = CoBorrowerID;
                }
            }
            else if (action == "Borrower") {

                if (BorrowerID == "") {

                    alert("There is no borrower in this loan.");
                    return;
                }

                ProspectIDs = BorrowerID;
            }
            else if (action == "CoBorrower") {

                if (CoBorrowerID == "") {

                    alert("There is no co-borrower in this loan.");
                    return;
                }

                ProspectIDs = CoBorrowerID;
            }

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var p = "";
            if ($.browser.msie == true) {   // ie

                p = "window.parent.document.frames[window.parent.idivIndex].document.frames['tabFrame'].SubscribeMailChimp";
            }
            else {   // firefox

                p = "window.parent.document.getElementById(window.parent.document.getElementsByTagName('iframe')[window.parent.idivIndex].id).contentDocument.getElementById('tabFrame').contentWindow.SubscribeMailChimp";
            }

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + BorrowerID + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function SubscribeMailChimp(LID) {

            window.parent.parent.CloseGlobalPopup();
            window.parent.parent.ShowWaitingDialog3("Please wait...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // check exist
            $.getJSON("../Prospect/ProspectMailChimp_CheckExist_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ProspectIDs + "&LID=" + LID, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        window.parent.parent.CloseWaitingDialog3();
                        return;
                    }

                    var RadomNum2 = Math.random();
                    var Radom2 = RadomNum2.toString().substr(2);

                    // Ajax
                    $.getJSON("../Prospect/ProspectMailChimp_Subscribe_Ajax.aspx?sid=" + Radom2 + "&ContactIDs=" + ProspectIDs + "&LID=" + LID, AfterSubscribeMailChimp);

                }, 2000);
            });
        }

        function AfterSubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Subscribe successfully.");

                    window.location.href = window.location.href;
                }
                window.parent.parent.CloseWaitingDialog3();

            }, 2000);
        }

        function aUnsubscribe_onclick(action) {

            var BorrowerID = $("#hdnBorrowerID").val();
            var CoBorrowerID = $("#hdnCoBorrowerID").val();

            if (BorrowerID == "" && CoBorrowerID == "") {

                alert("There is no borrower and co-borrower in this loan.");
                return;
            }

            if (action == "Both") {

                if (BorrowerID != "" && CoBorrowerID != "") {

                    ProspectIDs = BorrowerID + "," + CoBorrowerID;
                }
                else if (BorrowerID != "" && CoBorrowerID == "") {

                    ProspectIDs = BorrowerID;
                }
                else if (BorrowerID == "" && CoBorrowerID != "") {

                    ProspectIDs = CoBorrowerID;
                }
            }
            else if (action == "Borrower") {

                if (BorrowerID == "") {

                    alert("There is no borrower in this loan.");
                    return;
                }

                ProspectIDs = BorrowerID;
            }
            else if (action == "CoBorrower") {

                if (CoBorrowerID == "") {

                    alert("There is no co-borrower in this loan.");
                    return;
                }

                ProspectIDs = CoBorrowerID;
            }

            var SelCount = $("#gridContactMailCampaignList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("Please select ContactMailCampaign record(s).");
                return;
            }

            var LIDs = "";
            $("#gridContactMailCampaignList tr:not(:first) td :checkbox:checked").each(function () {

                var LID = $(this).attr("LID");
                if (LIDs == "") {

                    LIDs = LID;
                }
                else {

                    LIDs += "," + LID;
                }
            });

            //alert(LIDs);

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.parent.parent.ShowWaitingDialog3("Please wait...");

            // Ajax
            $.getJSON("../Prospect/ProspectMailChimp_Unsubscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ProspectIDs + "&LIDs=" + LIDs, AfterUnsubscribeMailChimp);
        }

        function AfterUnsubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Unsubscribe successfully.");

                    window.location.href = window.location.href;
                }
                window.parent.parent.CloseWaitingDialog3();

            }, 2000);
        }

        function GoToLeadstar() {

            var FileID = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            //alert(window.parent.location.pathname);

            if (window.parent.location.pathname.indexOf("ProspectLoanDetailsChild.aspx") != -1) {

                window.location.href = "../Prospect/ProspectDetailMarketingTab.aspx?sid=" + Radom + "&FileID=" + FileID;
            }
            else {

                window.location.href = "../LoanDetails/LoanMarketingTab.aspx?sid=" + Radom + "&FileID=" + FileID;
            }
        }

    </script>

    
</head>
<body>
    <form id="form1" runat="server">
    <div id="divTabContent">
        <div class="JTab" style="margin-top: 5px; border: solid 0px red; width: 1000px; margin-bottom: 20px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">&nbsp;</td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <%--<li><a href="javascript:GoToLeadstar()"><span>Leadstar</span></a></li>--%>
                                <li id="current"><a href="javascript:window.location.href=window.location.href"><span>Mail Chimp</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
                <div id="TabLine2" class="TabRightLine">&nbsp;</div>
                <div class="TabContent" style="padding: 10px;">
                    <div id="divToolBar">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 900px;">
                            <tr>
                                <td style="width: 500px;">
                                    <ul class="ToolStrip">
                                        <li><a id="aSubscribe" href="" onclick="return false;">Subscribe</a><span>|</span></li>
                                        <li><a id="aUnsubscribe" href="" onclick="return false;">Unsubscribe</a></li>
                                    </ul>
                                </td>
                                <td style="text-align: right;">
                                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                                        UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                        NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                        CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                                    </webdiyer:AspNetPager>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divContactMailCampaignList" class="ColorGrid" style="margin-top: 5px;">
                        <asp:GridView ID="gridContactMailCampaignList" runat="server" EmptyDataText="There is no contact mail campaign." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate>
                                        <input id="chkAll" type="checkbox" onclick="CheckAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="chkSelected" type="checkbox" LID="<%# Eval("LID")%>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="MailChimpCampaignName" HeaderText="Campaign" />
                                <asp:BoundField DataField="MailChimpListName" HeaderText="List" />
                                <asp:BoundField DataField="Result" HeaderText="Result" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Added" HeaderText="Added" DataFormatString="{0:MM/dd/yy}" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="AddedByName" HeaderText="Added By" ItemStyle-Width="200px" />
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">&nbsp;</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <ul id="divSubscribeMenu" class="contextMenu">
        <li><a href="#Both">Both</a></li>
        <li><a href="#Borrower">Borrower</a></li>
        <li><a href="#CoBorrower">CoBorrower</a></li>
    </ul>
    <ul id="divUnsubscribeMenu" class="contextMenu">
        <li><a href="#Both">Both</a></li>
        <li><a href="#Borrower">Borrower</a></li>
        <li><a href="#CoBorrower">CoBorrower</a></li>
    </ul>
    <asp:HiddenField ID="hdnBorrowerID" runat="server" />
    <asp:HiddenField ID="hdnCoBorrowerID" runat="server" />
    </form>
</body>
</html>