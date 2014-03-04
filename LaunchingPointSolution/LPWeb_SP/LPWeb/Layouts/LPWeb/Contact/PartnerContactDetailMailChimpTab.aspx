<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerContactDetailMailChimpTab.aspx.cs" Inherits="Contact_PartnerContactDetailMailChimpTab" %>

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
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            DrawTab();
        });

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridContactMailCampaignList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridContactMailCampaignList tr td :checkbox").attr("checked", "");
            }
        }

        function aSubscribe_onclick() {

            var ContactID = GetQueryString1("ContactID");

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var p = "";
            if ($.browser.msie == true) {   // ie

                p = "window.parent.document.frames['tabFrame'].SubscribeMailChimp";
            }
            else {   // firefox

                p = "window.parent.document.getElementById('tabFrame').contentWindow.SubscribeMailChimp";
            }

            var iFrameSrc = "../LoanDetails/SelectMailChimpListPopup.aspx?sid=" + RadomStr + "&ContactIDs=" + ContactID + "&GetIDsFunction=" + p + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 630;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.ShowGlobalPopup("Select MailChimp List", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function SubscribeMailChimp(LID) {

            window.parent.CloseGlobalPopup();
            window.parent.ShowWaitingDialog3("Please wait...");
            
            var ContactID = GetQueryString1("ContactID");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // check exist
            $.getJSON("../Prospect/ProspectMailChimp_CheckExist_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactID + "&LID=" + LID, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        window.parent.CloseWaitingDialog3();
                        return;
                    }

                    var RadomNum2 = Math.random();
                    var Radom2 = RadomNum2.toString().substr(2);

                    // Ajax
                    $.getJSON("../Prospect/ProspectMailChimp_Subscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactID + "&LID=" + LID, AfterSubscribeMailChimp);

                }, 2000);
            });
        }

        function AfterSubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Completed.");

                    window.location.href = window.location.href;
                }

                window.parent.CloseWaitingDialog3();

            }, 2000);
        }

        function aUnsubscribe_onclick() {

            var SelCount = $("#gridContactMailCampaignList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("Please select a record.");
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

            var ContactID = GetQueryString1("ContactID");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.parent.ShowWaitingDialog3("Please wait...");

            // Ajax
            $.getJSON("../Prospect/ProspectMailChimp_Unsubscribe_Ajax.aspx?sid=" + Radom + "&ContactIDs=" + ContactID + "&LIDs=" + LIDs, AfterUnsubscribeMailChimp);
        }

        function AfterUnsubscribeMailChimp(data) {

            setTimeout(function () {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Completed.");

                    window.location.href = window.location.href;
                }

                window.parent.CloseWaitingDialog3();

            }, 2000);
        }

    </script>

    
</head>
<body>
    <form id="form1" runat="server">
    <div id="divTabContent">
        <div id="divToolBar">
              <table cellpadding="0" cellspacing="0" border="0" style="width: 900px;">
                    <tr>
                        <td style="width: 500px;">
                            <ul class="ToolStrip">
                                <li><a id="aSubscribe" href="javascript:aSubscribe_onclick();">Subscribe</a><span>|</span></li>
                                <li><a id="aUnsubscribe" href="javascript:aUnsubscribe_onclick();">Unsubscribe</a></li>
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
    </form>
</body>
</html>