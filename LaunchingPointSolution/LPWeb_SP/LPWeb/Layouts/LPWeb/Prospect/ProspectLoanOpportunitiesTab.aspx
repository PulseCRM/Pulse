<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectLoanOpportunitiesTab.aspx.cs"
    Inherits="ProspectLoanOpportunitiesTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script type="text/javascript">
        var FromPage = "";

        $(document).ready(function () {

            var sHasCreate = "<%=sHasCreateRight %>";
            if (sHasCreate == "0") {
                DisableLink("btnNew");
            }
        });

        function DialogLoanDetailEditClose() {
            $("#divLoanDetailEdit").dialog('destroy');
        }

        function DialogLoanDetailViewClose() {
            $("#divLoanDetailView").dialog('destroy');
        }

        function LoanOpportunityNew() {
            var RadomStr = Math.random().toString().substr(2);
            var ContactID = $("#<%= hfdContactID.ClientID %>").val();

            var iFrameSrc = "LoanDetailEdit.aspx?sid=" + RadomStr + "&ContactID=" + ContactID + "&CloseDialogCodes=window.parent.CloseGlobalPopupPipeline()&RefreshCodes=window.parent.location = window.parent.location"; //

            var BaseWidth = 800
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.parent.ShowGlobalPopup("Loan Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function LoanOpportunityUpdate() {
            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("No record has been selected.");
                return;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            var RadomStr = Math.random().toString().substr(2);
            FromPage = "Update";

            var ContactID = $("#<%= hfdContactID.ClientID %>").val();

            var iFrameSrc = "LoanDetailEdit.aspx?sid=" + RadomStr + "&FileID=" + checkedIds.pop() + "&ContactID=" + ContactID + "&CloseDialogCodes=window.parent.CloseGlobalPopupPipeline()&RefreshCodes=window.parent.location.href=window.parent.location.href"; //

            var BaseWidth = 740
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.parent.ShowGlobalPopup("Loan Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function LoanOpportunityView() {
            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("No record has been selected.");
                return;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            GoToProspectLoanDetails(checkedIds.pop());
        }

        function GoToProspectLoanDetails(CurrentFileID) {

            var FileIDs = "";
            $(jQuery("#<%= this.gvLoans.ClientID %>").val() + " tr td a[id='aBorrower']").each(function (i) {

                if (i == 0) {

                    FileIDs = $(this).attr("myFileID");
                }
                else {

                    FileIDs += "," + $(this).attr("myFileID");
                }
            });

            //alert(FileIDs);

            var HrefEncode = encodeURIComponent(window.parent.parent.parent.location.href);
            window.parent.parent.parent.location.href = "ProspectLoanDetails.aspx?FromPage=" + HrefEncode + "&FileID=" + CurrentFileID + "&FileIDs=" + FileIDs;

        }

        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gvLoans.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        // check/decheck all
        function CheckAll(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 700px;">
                <tr>
                    <td>
                        <div id="div2" style="margin-left: 10px; margin-bottom:10px;">
                            <asp:DropDownList ID="ddlActiveLeads" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlActiveLeads_SelectedIndexChanged">
                                <asp:ListItem Text="Active Leads" Value="0"></asp:ListItem>
                                <asp:ListItem Text="All Leads" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Archived Leads" Value="2"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="div1" style="margin-left: 10px;">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li>
                                    <input id="btnDetail" type="button" value="Detail" class="Btn-66" onclick="LoanOpportunityView()" />
                                    &nbsp; </li>
                                <li>
                                    <input id="btnNew" type="button" value="New" class="Btn-66" onclick="LoanOpportunityNew()" />
                                    &nbsp; </li>
                                <li>
                                    <input id="btnView" type="button" value="Modify" class="Btn-66" onclick="LoanOpportunityUpdate()" />
                                    &nbsp; </li>
                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
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
    <div id="divGrid" class="ColorGrid" style="width: 900px; margin-top: 5px; margin-left: 10px;">
        <asp:GridView ID="gvLoans" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
            OnSorting="gvLoans_Sorting" CellPadding="3" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <input type="checkbox" onclick="CheckAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("FileID") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProspectLoanStatus" SortExpression="ProspectLoanStatus"
                    HeaderStyle-Width="80" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("ProspectLoanStatus") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lien" SortExpression="LienPosition">
                    <ItemTemplate>
                        <%# Eval("LienPosition")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Purpose" SortExpression="Purpose" ItemStyle-Wrap="false"
                    ItemStyle-Width="100">
                    <ItemTemplate>
                        <%# Eval("Purpose")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Amount" SortExpression="LoanAmount" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("LoanAmount", "{0:c0}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rate" SortExpression="Rate" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("Rate", "{0:N4}")%>%
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Property" SortExpression="PropertyAddr" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("PropertyAddr")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loan Type" SortExpression="LoanType" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("LoanType")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Point File " SortExpression="Name" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <a id="aBorrower" href="javascript:GoToProspectLoanDetails('<%# Eval("FileID") %>')"
                            myfileid="<%# Eval("FileID") %>">
                            <%# Eval("Name")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Loan Program" SortExpression="Program" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("Program").ToString() == "0" ? "" : Eval("Program").ToString()%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
        <asp:HiddenField ID="hfdContactID" runat="server" />
    </div>
    </form>
</body>
</html>
