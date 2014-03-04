<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportLeads.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Pipeline.ImportLeads"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.tinysort.min.js" type="text/javascript"></script>
    <title>Import Leads</title>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
  
    <script type="text/javascript">
      

        function DoImport() {

            var loanOfficer = $("#" + '<%=ddlLoanOfficer.ClientID %>').val();
            var refCode = $("#" + '<%=tbRefCode.ClientID %>').val();
            var leadSource = $("#" + '<%=ddlLeadSource.ClientID %>').val();
            var FileUpload = $("#" + '<%=FileUpload.ClientID %>').val();
            var Branch = $("#" + '<%=ddlBranch.ClientID %>').val();
            if (Branch == "-1") {
                alert("Please select a Branch.");
                return false;
            }
            if (FileUpload == "") {
                alert("Please select a file to import.");
                return false;
            }
            else {
                var extension = new String(FileUpload.substring(FileUpload.lastIndexOf(".") + 1, FileUpload.length))
                if (extension != "xls" && extension != "xlsx" && extension != "csv") {
                    alert("The file type is not supported. Pulse supports .XLS, .XLSX and .CSV files.");
                    return false;
                }
            }
            return true;
        }

        function closeBtnClicked() {
            window.parent.closeImportPop();
        }
    </script>
    
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="aspnetForm" runat="server" >
        <div class="Heading">
            Import Leads</div>
         <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-left: 15px;">
                            <asp:Button ID="btnImport" runat="server" Text="Import" class="Btn-66" OnClientClick="return DoImport()" OnClick="btnImport_OnClick"/>
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="Btn-66"  OnClientClick="closeBtnClicked()" />
                        </td>
                    </tr>
                </table>
            </div>
        <div class="DetailsContainer">
            <div>
                <table cellpadding="0" cellspacing="0" border="0" style=" width:630px">
                    <tr>
                        <td style="white-space: nowrap; width:150px">
                            Lead Source:
                        </td>
                        <td style="padding-left: 15px; width:170px">
                            <asp:DropDownList ID="ddlLeadSource" runat="server" DataTextField="LeadSource" DataValueField="LeadSourceID"  Width="150px"></asp:DropDownList>
                        </td>
                        <td style="padding-left: 15px;width:150px">
                           Reference Code:
                        </td>
                        <td style="padding-left: 15px; width:170px">
                            <asp:TextBox ID="tbRefCode" runat="server" class="iTextBox" Style="width: 150px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;width:150px">
                            Branch:
                        </td>
                        <td style="padding-left: 15px; width:170px" >
                            <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="Name" DataValueField="BranchId"  Width="150px" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_OnSelectedIndexChanged"></asp:DropDownList>
                        </td>
                         <td style="padding-left: 15px;width:150px">
                            Loan Officer:
                        </td>
                        <td style="padding-left: 15px; width:170px">
                            <asp:DropDownList ID="ddlLoanOfficer" runat="server" DataTextField="LeadSource" DataValueField="LeadSourceID"  Width="160px">
                                <asp:ListItem Text ="- select a Loan Officer -" Value="-1" ></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style=" white-space: nowrap;width:150px">
                            File to import:
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="3">
                             <asp:FileUpload ID="FileUpload" runat="server" Width="495px" />
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hiReferenced" runat="server" Value="0" />
                <asp:HiddenField ID="hiCloned" runat="server" Value="0" />
                 <asp:HiddenField ID="hiAllIds" runat="server" />
                <asp:HiddenField ID="hiCheckedIds" runat="server" />
                <asp:HiddenField ID="hiCreatedId" runat="server" />
                <asp:HiddenField ID="hiCurrentData" runat="server" />
            </div>
           
           
            <div class="DashedBorder" style="margin-top: 15px;">
                &nbsp;</div>
           
        </div>
    </div>
    <div id="divError" runat="server" >
          <div id="Div1" class="TabLeftLine">&nbsp;</div>
         <div id="Div2" class="TabRightLine">&nbsp;</div>
         Unable to import the following rows,please check the file to import.    <asp:Button ID="btnReturn" runat="server" Text="Cancel" class="Btn-66" OnClientClick="closeBtnClicked()"   />
         <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
         <div id="TabLine2" class="TabRightLine">&nbsp;</div>
         <div id="divDivision" class="ColorGrid" align="center" style="width: 700px; margin-top: 5px;">
         <asp:GridView ID="gvErrorView" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="700px" AllowSorting="true" EmptyDataText="There is no data in database."
            CellPadding="3" GridLines="None" DataKeyNames="RowNo" >
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderText="Row #" ItemStyle-Wrap="false" 
                    ItemStyle-Width="40px">
                    <ItemTemplate>
                        <%# Eval("RowNo")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Borrower Name"  ItemStyle-Wrap="false"
                    ItemStyle-Width="170px">
                    <ItemTemplate>
                        <%# Eval("BorrowerName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                    <asp:TemplateField HeaderText="Columns Infomation"  ItemStyle-Wrap="false"
                    ItemStyle-Width="500px">
                    <ItemTemplate>
                       <%# Eval("ColumnsInfo")%>
                    </ItemTemplate>
                    <ItemStyle Width="495px" Wrap="true" HorizontalAlign="Left"  />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
    </div>
    <div style="display: none;">
        <div id="dialog1" title="Rule Selection">
            <iframe id="iframeRS" name="iframeRS" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="dialogRule" title="Rule Setup">
            <iframe id="ifrRuleEdit" name="ifrRuleEdit" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="dialogEmailTplt" title="Rule Setup">
            <iframe id="ifrEdit" name="ifrEdit" frameborder="0" width="100%" height="100%"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
