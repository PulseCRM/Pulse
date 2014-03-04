<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerContactDetailNotesTab.aspx.cs"
 Inherits="Contact_PartnerContactDetailNotesTab"  %>
 <%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

 <!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css"
        runat="server" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script type="text/javascript">
        var hdnCreateNotes = "#<%= hdnCreateNotes.ClientID %>";

        $(document).ready(function () {
            if ($(hdnCreateNotes).val() == "0") {
                // $("#btnNew").removeAttr('href');
                DisableLink("btnNew");
            }
        });

        function PopupAddNoteWindow(isEdit, noteId) {
            var fileId = $("#<%= hfdContactId.ClientID %>").val();
            var showTitle = "Add Contact Note";
            if (isEdit) {
                showTitle = "Contact Note Detail";
                $("#ifrLoanNoteAdd").attr("src", "PartnerContactDetailNoteDetail.aspx?contactId=" + fileId + "&noteId=" + noteId).show();
            }
            else {
                $("#ifrLoanNoteAdd").attr("src", "PartnerContactDetailNoteDetail.aspx?contactId=" + fileId).show();
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
        <iframe id="ifrLoanNoteAdd" frameborder="0" scrolling="no" width="580px" height="290px">
        </iframe>
    </div>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divToolBar" style="margin-top: 13px;">
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
    <div id="divGrid" class="ColorGrid" style="width: 700px; margin-top: 5px;">
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
                <asp:TemplateField HeaderText="Sender" SortExpression="CreaterName" HeaderStyle-Width="100" HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("CreaterName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Note" SortExpression="Note" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-Wrap="false">
                    <ItemTemplate>
                       <a href="javascript:PopupAddNoteWindow(true,'<%# Eval("ContactNoteId") %>')"><%# Eval("Note").ToString().Length > 50 ? Server.HtmlEncode(Eval("Note").ToString()).Substring(0, 50) + "..." : Server.HtmlEncode(Eval("Note").ToString())%></a>
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
