<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Prospect Detail View - Emails" Language="C#" AutoEventWireup="true"
    CodeBehind="ProspectDetailEmailTab.aspx.cs" Inherits="Prospect_ProspectDetailEmailTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.contextMenu.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.contextMenu.js" type="text/javascript"></script>
    <title>Client Detail - Emails</title>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        Array.prototype.remove = function (s) {
            var nIndex = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i] == s) {
                    nIndex = i;
                    break;
                }
            }
            if (nIndex != -1) {
                this.splice(nIndex, 1);
                return true;
            }
            else
                return false;
        }

        function CheckAllClicked(me, areaID, hiAllIDs, hiSelectedIDs) {
            var bCheck = $(me).attr('checked');
            if (bCheck) {
                // copy all ids to selected id holder
                $('#' + hiSelectedIDs).val($('#' + hiAllIDs).val());
            }
            else
                $('#' + hiSelectedIDs).val('');
            $('input:checkbox', $('#' + areaID) + '.CheckBoxColumn').each(function () { $(this).attr('checked', bCheck); });
        }

        function CheckBoxClicked(me, ckAllID, hiAllIDs, hiSelectedIDs, id) {
            var sAllIDs = $('#' + hiAllIDs).val();
            var sSelectedIDs = $('#' + hiSelectedIDs).val();
            var allIDs = new Array();
            var selectedIDs = new Array();
            if (sAllIDs.length > 0)
                allIDs = sAllIDs.split(',');

            if (sSelectedIDs.length > 0)
                selectedIDs = sSelectedIDs.split(',');

            if ($(me).attr('checked'))
                selectedIDs.push(id);
            else
                selectedIDs.remove(id);

            // set the CheckAll check box checked status
            // $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);

            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');
        }
    </script>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        var bId = -1;
        var fId = -1;
        
        var sHasSend = "<%=sHasSendRight %>";
        $(document).ready(function () {
            var startDate = $("#" + '<%=tbSentStart.ClientID %>');
            var endDate = $("#" + '<%=tbSentEnd.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");


            if (sHasSend == "0") {
                DisableLink("aSendEmail");
            }
        });

        function SelectedCount() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (null == sIds || sIds.length == 0)
                return 0;
            var arrIds = sIds.split(",");
            return arrIds.length;
        }
// ]]>
    </script>
    <script type="text/javascript">

        $(document).ready(function () {

            // left click context menu
            $("#aSendEmail").contextMenu({ menu: 'divSendEmailMenu', leftButton: true, onShowMenu: function () { $("#divSendEmailMenu").css({ top: 65, left: 15 }).fadeIn(-1); } }, function (action, el, pos) { contextMenuWork(action, el, pos); });
        });
        
        function ShowGlobalPopup(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divGlobalPopup").attr("title", Title);

            $("#ifrGlobalPopup").attr("src", "");
            $("#ifrGlobalPopup").attr("src", iFrameSrc);
            $("#ifrGlobalPopup").width(iFrameWidth);
            $("#ifrGlobalPopup").height(iFrameHeight);

            // show modal
            $("#divGlobalPopup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) {
                    $("#divGlobalPopup").dialog("destroy");
                    $("#ifrGlobalPopup").attr("src", "about:blank");
                }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseGlobalPopup() {

            $("#divGlobalPopup").dialog("close");           
          }

        function aSendEmail_onclick() {

            var ProspectID = GetQueryString1("ProspectID");

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/EmailSendPopup.aspx?sid=" + RadomStr + "&ProspectID=" + ProspectID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            window.parent.parent.parent.ShowGlobalPopup("Send Email", 605, 530, 630, 570, iFrameSrc);
        }

        function aDetail_onclick() {

            if (SelectedCount() == 0) {
                alert("No record has been selected.");
                return;
            }
            else if (SelectedCount() > 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            var EmailLogID = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            ShowDialog_EmailDetail(EmailLogID)
        }

        function ShowDialog_EmailDetail(EmailLogID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Prospect/EmailDetailPopup.aspx?sid=" + RadomStr + "&EmailLogID=" + EmailLogID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";
            var BaseWidth = 550
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;
            var BaseHeight = 750;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Email Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function contextMenuWork(action, el, pos) {

            if (action == "ProspectEmail") {

                aSendEmail_onclick();
            }
            else {

                var LoanID = action;
                //alert("LoanID: " + LoanID);

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                var iFrameSrc = "../LoanDetails/EmailSendPopup.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

                window.parent.parent.parent.ShowGlobalPopup("Send Email", 605, 530, 630, 570, iFrameSrc);
            }
        }

        function aReply_onclick() {

            if (SelectedCount() == 0) {
                alert("No record has been selected.");
                return;
            }
            else if (SelectedCount() > 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            var EmailLogID = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            //alert(EmailLogID);

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "EmailReply.aspx?sid=" + RadomStr + "&EmailLogID=" + EmailLogID + "&Action=Reply&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 640
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 700;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.parent.ShowGlobalPopup("Reply Message", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }


        //CR64
        function aEmailLogAttachments_onclick() {

            if (SelectedCount() == 0) {
                alert("No record has been selected.");
                return;
            }
            else if (SelectedCount() > 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            var EmailLogID = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            ShowDialog_EmailLogAttachments(EmailLogID)
        }

        function ShowDialog_EmailLogAttachments(EmailLogID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Prospect/EmailLogAttachmentsPopup.aspx?sid=" + RadomStr + "&EmailLogID=" + EmailLogID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";
            var BaseWidth = 340
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;
            var BaseHeight = 400;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Email Log Attachments Popup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        //END CR64

    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="aspnetForm">
        <div style="padding-left: 10px; padding-right: 10px;">
            <div id="divFilter" style="margin-top: 10px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 330px;">
                            <asp:DropDownList ID="ddlEmailTypeFilter" runat="server" DataValueField="EmailTypeID" DataTextField="EmailType" Width="300px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 35px;">Sent:</td>
                        <td>
                            <asp:TextBox ID="tbSentStart" runat="server" CssClass="DateField"></asp:TextBox>
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:TextBox ID="tbSentEnd" runat="server" CssClass="DateField"></asp:TextBox>
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                            </asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divToolBar" style="margin-top: 13px;">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td>
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aSendEmail" href="" onclick="return false;">Send Email</a><span>|</span></li>
                                <li><a id="aDetail" href="javascript:aDetail_onclick()">Detail</a><span>|</span></li>
                                <li><a id="aReply" href="javascript:aReply_onclick()">Reply</li>
                            </ul>
                        </td>
                        <td style="text-align: right;">
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                                OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                                FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                                ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                                LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divDivision" class="ColorGrid" style="margin-top: 3px;">
            <asp:GridView ID="gridList" runat="server" DataKeyNames="EmailLogId,EmailBody" EmptyDataText="There is no email in database."
                AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound" OnPreRender="gridList_PreRender"
                CellPadding="3" CssClass="GrayGrid" GridLines="None" OnSorting="gridList_Sorting"
                AllowSorting="true">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <asp:CheckBox ID="ckbAll" runat="server" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="ckbSelect" runat="server" />
                        </ItemTemplate>
                        <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                        <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                    </asp:TemplateField>
                    <asp:BoundField DataField="LastSent" SortExpression="LastSent" HeaderText="Sent" />
                    <asp:BoundField DataField="EmailType" SortExpression="EmailType" HeaderText="Type" />
                    <asp:BoundField DataField="LoanFile" SortExpression="LoanFile" HeaderText="Loan File" />
                    <asp:TemplateField HeaderText="Subject" SortExpression="Subject">
                        <ItemTemplate>
                            <a href="javascript:ShowDialog_EmailDetail('<%# Eval("EmailLogId") %>')"><%# Eval("Subject") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FromUserName" SortExpression="FromUserName" HeaderText="From" />
                    <asp:BoundField DataField="ToUserName" SortExpression="ToUserName" HeaderText="To" />
                    <asp:TemplateField HeaderText="Body">
                        <ItemTemplate>
                            <a href="#" onclick="ShowDialog_EmailDetail('<%# Eval("EmailLogId") %>'); return false;">
                                <asp:Literal ID="litBody" runat="server"></asp:Literal></a>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Attachments" SortExpression="Attachments">
                        <ItemTemplate>
                            <%# (Eval("Attachments") != null && Eval("Attachments").ToString()!="") ? "<a href=\"javascript:ShowDialog_EmailLogAttachments('" + Eval("EmailLogId") + "')\"><img src=\"../images/fj.jpg\" alt=\"Attachments\" /></a>" : ""%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">
                &nbsp;</div>
            <asp:HiddenField ID="hiAllIds" runat="server" />
            <asp:HiddenField ID="hiCheckedIds" runat="server" />
        </div>
    </div>
    <div id="divGlobalPopup" title="Global Popup" style="display: none;">
        <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="400px" height="300px">
        </iframe>
    </div>
    <ul id="divSendEmailMenu" class="contextMenu" style="width: 250px;">
        <li><a href="#ProspectEmail">Client Email</a></li>
        <asp:Literal ID="ltrContextMenuItems" runat="server"></asp:Literal>
    </ul>
    </form>
</body>
</html>
