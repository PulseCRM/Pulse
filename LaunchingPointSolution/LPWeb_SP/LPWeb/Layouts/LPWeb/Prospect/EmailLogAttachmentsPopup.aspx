<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailLogAttachmentsPopup.aspx.cs" Inherits="Prospect_EmailLogAttachmentsPopup" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Email Detail Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/featuredcontentglider2.css" rel="stylesheet" type="text/css" />
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
    <script src="../js/featuredcontentglider2.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 340px; border: solid 0px blue;">
        
        <div id="divEmailLogs" class="" style="border: solid 0px green; height: 400px;">

                    <div class="" style="width: 340px; border: solid 0px red; overflow: auto;">

                        <div style="margin-top: 10px;">
                           

                            <div id="divDivision" class="ColorGrid" style="margin-top: 3px; padding:5px">

                                 <table>
                                    <tr>
                                        <td style="width: 180px;">Borrower: <%= this.GetBorrowerName()%></td>
                                        <td style="width: 155px; text-align:right;">Sent: <%=Sentlast %></td>
                                    </tr>
                                </table>
                                <br />
                                <table>
                                    <tr>
                                        <td colspan="2">Attachments:</td>
                                    </tr>
                                </table>
                                <div style=" height:300px; overflow:auto;">
                                <asp:GridView ID="gridList" runat="server" DataKeyNames="EmailLogId" EmptyDataText="There is no Attachments in database."
                                    AutoGenerateColumns="False"
                                    CellPadding="3" CssClass="GrayGrid" GridLines="None" Width="320" >
                                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                    <AlternatingRowStyle CssClass="EvenRow" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                            <ItemTemplate>
                                                <a href="<%=System.Web.HttpContext.Current.Request.RawUrl%>&fileName=<%# Eval("Name")%>.<%# Eval("FileType")%>" target="_blank" ><%# Eval("Name")%>.<%# Eval("FileType")%></a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Size" SortExpression="FileSize" ItemStyle-HorizontalAlign="Right" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <%# this.GetSizeStr( Eval("FileSize"))%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="GridPaddingBottom">
                                    &nbsp;</div>
                                <asp:HiddenField ID="hiAllIds" runat="server" />
                                <asp:HiddenField ID="hiCheckedIds" runat="server" />
                                </div>
                                <div style=" display:none;"> <iframe id="ifrdownload" name="ifrdownload" src="about:blank"></iframe><%-- target="ifrdownload"--%> </div>
                            </div>
                        </div>

                    </div>
            
        </div>

    </div>

    </form>
</body>
</html>