<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanNoteList.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.LoanDetails.LoanNoteList"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css" runat="server" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>

    <script type="text/javascript">
        var hdnCreateNotes = "#<%= hdnCreateNotes.ClientID %>";

        $(document).ready(function () {

            DrawTab();

            if ($(hdnCreateNotes).val() == "0") {
                $("#btnNew").removeAttr('href');
            }
            // if not active loan, disable all
            var IsActiveLoan = $("#hdnActiveLoan").val();
            if (IsActiveLoan == "False") {
                DisableAll();
            }
        });
        function DisableAll() {
            $(".ToolStrip a").each(function () {
                $(this).attr("disabled", "true");
                $(this).removeAttr("href");
                $(this).css("text-decoration", "none");
            });
        }

        function PopupAddNoteWindow(isEdit, noteId) {

            // show waiting
            window.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var fileId = $("#<%= hfdFileId.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + fileId, function (data) {

                window.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // continue
                        var showTitle = "Add Loan Note";
                        if (isEdit) {
                            showTitle = "Loan Note Detail";
                            $("#ifrLoanNoteAdd").attr("src", "LoanNoteAdd.aspx?fileId=" + fileId + "&noteId=" + noteId).show();
                        }
                        else {
                            $("#ifrLoanNoteAdd").attr("src", "LoanNoteAdd.aspx?fileId=" + fileId).show();
                        }

                        $("#ifrLoanNoteAdd").dialog({
                            bgiframe: true,
                            modal: true,
                            title: showTitle,
                            height: 480,
                            width: 525,
                            close: function (event, ui) { $(this).dialog('destroy') },
                            open: function (event, ui) { $(this).css("width", "100%") }
                        });
                    }
                }
            });

        }

        function ClosePopupWindow() {
            ClosePopupWindowDirectly();
            //location.reload();
            //refresh the page
            window.location.href = window.location.href;
        }

        function ClosePopupWindowDirectly() {
            $("#ifrLoanNoteAdd").dialog("destroy");
        }

        //#region CR48 fake tab navigation

        function aTasks_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsTask.aspx?sid=" + sid + "&LoanID=" + LoanID + "&ref=" + sRef;
        }

        function aAlerts_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsAlertTab.aspx?sid=" + sid + "&LoanID=" + LoanID + "&ref=" + sRef;
        }

        function aEmails_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../Prospect/LoanDetailEmailTab.aspx?from=2&itemid=" + LoanID + "&sid=" + sid + "&ref=" + sRef;

        }

        function aNotes_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanNoteList.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }

        function aActivityHistory_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/Activity.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }


        //#endregion

    </script>

</head>
<body>
    <form id="form1" runat="server">

    <div class="JTab" style="margin-top:10px; width: 1020px; margin-left: 15px;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <ul>
                            <li><a id="aTasks" href="javascript:aTasks_onclick()"><span>Tasks</span></a></li>
                            <li><a id="aAlerts" href="javascript:aAlerts_onclick()"><span>Rule Alerts</span></a></li>
                            <li><a id="aEmails" href="javascript:aEmails_onclick()"><span>Emails</span></a></li>
                            <li id="current"><a id="aNotes" href="javascript:aNotes_onclick()"><span>Notes</span></a></li>
                            <li><a id="aActivityHistory" href="javascript:aActivityHistory_onclick()"><span>Activity History</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <div id="TabBody" style="margin-bottom:10px; padding-bottom:10px;">
            <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
            <div id="TabLine2" class="TabRightLine">&nbsp;</div>
            <div class="TabContent" style="padding: 15px;">

                <asp:HiddenField ID="hfdFileId" runat="server" />
                <div id="divAddNote" title="Add Loan Note" style="display: none;">
                    <iframe id="ifrLoanNoteAdd" frameborder="0" scrolling="no" width="580px" height="290px">
                    </iframe>
                </div>
                <div style="padding-left: 10px; padding-right: 10px;">
                    <div id="divToolBar">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 700px;">
                            <tr>
                                <td style="width: 300px;">
                                    <div id="div1">
                                        <ul class="ToolStrip" style="margin-left: 0px;">
                                            <li><a id="btnNew" runat="server" href="javascript:PopupAddNoteWindow(false);">Add</a></li>
                                        </ul>
                                    </div>
                                </td>
                                <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
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
                <div id="divGrid" class="ColorGrid">
                    <asp:GridView ID="gvNoteList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                        Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                        OnSorting="gvNoteList_Sorting" CellPadding="3" GridLines="None" OnRowDataBound="gvNoteList_OnRowDataBound">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderText="Time" SortExpression="Created" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                                ItemStyle-Width="100">
                                <ItemTemplate>
                                    <%# Eval("Created", "{0:MM/dd/yyyy HH:mm}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sender" SortExpression="Sender" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%# Eval("Sender")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Note" SortExpression="Note" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false">
                                <ItemTemplate>
                                   <a href="javascript:PopupAddNoteWindow(true,'<%# Eval("NoteId") %>')"><%# Eval("Note").ToString().Length > 100 ? Server.HtmlEncode(Eval("Note").ToString()).Substring(0, 80) + "..." : Server.HtmlEncode(Eval("Note").ToString())%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="External" SortExpression="ExternalViewing" HeaderStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbExternalViewing" runat="server" AutoPostBack="true" OnCheckedChanged="cbExternalViewing_OnCheckedChanged"   />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Condition" DataField="CondName" HeaderStyle-Width="370px" ItemStyle-Width="370px" />
                        </Columns>
                    </asp:GridView>
                    <div class="GridPaddingBottom">&nbsp;</div>
                </div>

            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnCreateNotes" runat="server" />
    <asp:HiddenField ID="hdnActiveLoan" runat="server" />
    </form>
</body>
</html>
