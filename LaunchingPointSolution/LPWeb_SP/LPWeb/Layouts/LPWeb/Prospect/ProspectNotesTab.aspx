<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectNotesTab.aspx.cs" Inherits="LPWeb.Prospect.ProspectNotesTab"
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
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>


    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.progressbar.min.js"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
   
    <script type="text/javascript">
        var hdnCreateNotes = "#<%= hdnCreateNotes.ClientID %>";

        $(document).ready(function () {
            var startDate = $("#" + '<%=tbSentStart.ClientID %>');
            var endDate = $("#" + '<%=tbSentEnd.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");

            if ($(hdnCreateNotes).val() == "0") {
                // $("#btnNew").removeAttr('href');
                DisableLink("btnNew");
            }

            $("#btnNew").contextMenu("menu1", {
                menuStyle: {
                    listStyle: 'none',
                    padding: '1px',
                    margin: '0px',
                    backgroundColor: '#fff',
                    border: '1px solid #999',
                    width: '250px'
                },
                itemStyle: {
                    margin: '0px',
                    color: '#000',
                    display: 'block',
                    cursor: 'default',
                    padding: '3px',
                    border: '1px solid #fff',
                    backgroundColor: 'transparent'
                },
                itemHoverStyle: {
                    border: '1px solid #0a246a',
                    backgroundColor: '#b6bdd2'
                },
                onContextMenu: function (e) {
                    return true;
                },
                onShowMenu: function (e, menu) {

                    return menu;
                }
            });
        });



        function PopupAddNoteWindow(isEdit, noteId, form) {

            // show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var FileID = $("#<%= hfdContactId.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();

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
                        var fileId = $("#<%= hfdContactId.ClientID %>").val();
                        var showTitle = "Add Prospect Note";
                        var sSrc = "";
                        if (form == "0") {
                            sSrc = "../LoanDetails/LoanNoteAdd.aspx?fileId=" + fileId;
                        }
                        else {
                            sSrc = "ProspectNoteAdd.aspx?contactId=" + fileId;
                        }

                        if (isEdit) {
                            showTitle = "Prospect Note Detail";
                            $("#ifrLoanNoteAdd").attr("src", sSrc + "&noteId=" + noteId).show();
                        }
                        else {
                            $("#ifrLoanNoteAdd").attr("src", "ProspectNoteAdd.aspx?contactId=" + fileId).show();
                        }
                        $("#divAddNote").dialog({
                            bgiframe: true,
                            modal: true,
                            title: showTitle,
                            height: 500,
                            width: 555,
                            close: function (event, ui) { $(this).dialog('destroy') },
                            open: function (event, ui) { $(this).css("width", "100%") }
                        });
                    }
                }
            });

        }

        function PopupAddLoanNoteWindow(status, fileId) {

            var ContactID = $("#<%= hfdContactId.ClientID %>").val();

            if (status == "ActiveLoan") {

                // show waiting
                window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

                var RadomNum = Math.random();
                var sid = RadomNum.toString().substr(2);

                $.getJSON("../CheckPointFileStatus_BG_JSON.aspx?sid=" + sid + "&ContactID=" + ContactID, function (data) {

                    window.parent.parent.parent.CloseWaitingDialog3();

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
                            ShowPopupAddLoanNoteWindow(status, ContactID);

                        }
                    }
                });
            }
            else {

                ShowPopupAddLoanNoteWindow(status, ContactID);
            }

        }

        function ShowPopupAddLoanNoteWindow(status, fileId) {

            //            var fileId = $("#<%= hfdContactId.ClientID %>").val();
            $("#ifrLoanNoteAdd").attr("src", "../LoanDetails/LoanNoteAdd.aspx?fileId=" + fileId).show();
            var showTitle = "Add " + status + " Note";
            $("#divAddNote").dialog({
                bgiframe: true,
                modal: true,
                title: showTitle,
                height: 500,
                width: 555,
                close: function (event, ui) { $(this).dialog('destroy') },
                open: function (event, ui) { $(this).css("width", "100%") }
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

        
    </script>
</head>
<body style="width: 700px">
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfdContactId" runat="server" />
    <div id="divAddNote" title="Add Loan Note" style="display: none;">
        <iframe id="ifrLoanNoteAdd" frameborder="0" scrolling="no" width="580px" height="500px">
        </iframe>
    </div>
     <div id="divFilters" style="margin-top: 10px;">
        <table>
            <tr>
                <td style="width: 310px;">
                    <asp:DropDownList ID="ddlNoteType" runat="server" DataValueField="ContactId" DataTextField="NoteTypeName" Width="300px">
                    </asp:DropDownList>
                </td>
                <td style="width: 65px;">Start Date:</td>
                <td >
                    <asp:TextBox ID="tbSentStart" runat="server" CssClass="DateField"></asp:TextBox>
                </td>
                <td style="width: 65px;">End Date:</td>
                <td >
                    <asp:TextBox ID="tbSentEnd" runat="server" CssClass="DateField"></asp:TextBox>
                </td>
                <td style="padding-left: 15px;">
                     <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 900px;">
                <tr>
                    <td style="width: 300px;">
                        <div id="div1">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="btnNew" runat="server" href="javascript:PopupAddNoteWindow(false);">Add</a></li>
                            </ul>
                             <div id="menu1" class="contextMenu" style="display: none;">
                                <ul>
                                    <li id="0"><a href="javascript:PopupAddNoteWindow(false,'1','1');"><span>Prospect Note</span></a></li>
                                    <%=sNoteType %>
                                </ul>
                            </div>
                        </div>
                        
                    </td>
                    <td style="letter-spacing: 1px; text-align: right;">
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
    <div id="divGrid" class="ColorGrid" style="width: 900px; margin-top: 5px;">
        <asp:GridView ID="gvNoteList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
            OnSorting="gvNoteList_Sorting" CellPadding="3" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderText="Time" SortExpression="Created" ItemStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Width="100">
                    <ItemTemplate>
                        <%# Eval("Created", "{0:MM/dd/yyyy HH:mm}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type" SortExpression="LoanType" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("LoanType")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Loan File" SortExpression="LoanFile" HeaderStyle-Width="200" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("LoanFile")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Sender" SortExpression="SenderName" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("SenderName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Note" SortExpression="Note" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200"
                    ItemStyle-Wrap="false">
                    <ItemTemplate>
                       <a href="javascript:PopupAddNoteWindow(true,'<%# Eval("NoteId") %>','<%# Eval("form") %>')"><%# Eval("Note").ToString().Length > 100 ? Server.HtmlEncode(Eval("Note").ToString()).Substring(0, 80) + "..." : Server.HtmlEncode(Eval("Note").ToString())%></a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">&nbsp;</div>
    </div>
    <asp:HiddenField ID="hdnCreateNotes" runat="server" />
    </form>
</body>
</html>
