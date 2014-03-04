<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Page Language="C#" DynamicMasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master"
    AutoEventWireup="true" Inherits="PartnerContactDetailReferralstab" CodeBehind="PartnerContactDetailReferralstab.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
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
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <title>Contact Detail View - Referrals</title>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        var gridId = "#<%=gridList.ClientID %>";
        $(document).ready(function () {
            var startDate = $("#" + '<%=tbSentStart.ClientID %>');
            var endDate = $("#" + '<%=tbSentEnd.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");

            //var checkItems=
            var checkAll = $(gridId + " :checkbox:eq(0)");
            checkAll.click(function () {
                var allStatus = checkAll.attr("checked");
                $(gridId + " :checkbox:gt(0)").each(function () {
                    $(this).attr("checked", allStatus);
                });
                getSelectedItems();
            });
            //
            $(gridId + " :checkbox:gt(0)").each(function () {
                $(this).unbind("click").click(function () {
                    if ($(this).attr("checked") == false) {
                        checkAll.attr("checked", false);
                    }
                    getSelectedItems();
                });
            });
        });
         
// ]]>
    </script>
    <script type="text/javascript">


        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gridList.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        function GoToProspectDetail(ContactID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var c = "ContactID=" + ContactID + "&ContactIDs=" + ContactID;
            var e = $.base64Encode(c);

            window.parent.location.href = "../Prospect/ProspectDetailView.aspx?PageFrom=" + encodeURIComponent(window.parent.location) + "&e=" + e;
            //            window.location.href = "../Prospect/ProspectDetailView.aspx?sid=" + RadomStr + "&ContactID=" + ContactID;

        }

        function GoToLoanDetail(ContactID) {

            window.parent.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=" + encodeURIComponent(window.parent.location) + "&ContactID=" + ContactID;

        }

        function aDetail_onclick() {

            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("No record has been selected.");
                return;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            GoToProspectDetail(checkedIds.pop());
        }

        function GoToLoanDetails(FileIDs) {

            if (FileIDs == "") {

                alert("There is no loan that the partner contact has as referral.");
                return;
            }

            var FirstFileID = "";
            var Pos = FileIDs.indexOf(",");
            if (Pos == -1) {

                FirstFileID = FileIDs;
            }
            else {

                FirstFileID = FileIDs.substr(0, Pos);
            }
            //alert(FirstFileID);

            window.parent.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=" + encodeURIComponent(window.parent.location.href) + "&fieldid=" + FirstFileID + "&fieldids=" + FileIDs;
        }
 
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <div id="aspnetForm">
        <div style="padding-left: 10px; padding-right: 10px;">
            <div id="divFilter" style="margin-top: 10px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 50px;">
                            Created:&nbsp;&nbsp;
                        </td>
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
                                <li>
                                    <input id="btnDetail" type="button" value="Detail" class="Btn-66" onclick="aDetail_onclick()" />&nbsp;
                                </li>
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
        <div id="divDivision" class="ColorGrid" style="margin-top: 3px; width: 600px;">
            <asp:GridView ID="gridList" runat="server" EmptyDataText="There is no data in database."
                AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None"
                OnSorting="gridList_Sorting" AllowSorting="true">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input type="checkbox" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" tag='<%# Eval("ContactID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Created" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="left">
                        <ItemTemplate>
                            <%# Eval("Created", "{0:d}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Client" SortExpression="ClientName" ItemStyle-Width="350px">
                        <ItemTemplate>
                            <a href="javascript:GoToProspectDetail('<%# Eval("ContactID") %>')">
                                <%# Eval("ContactFullName")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Loans" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="50px">
                        <ItemTemplate>
                            <a href="javascript:GoToLoanDetail('<%# Eval("ContactID") %>')">
                                <%# Eval("LoanCount")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Total Referral $" SortExpression="TotalReferral" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <a href="javascript:GoToLoanDetails('<%# Eval("TotalReferralFileIDs")%>')"><%# Eval("TotalReferral") == DBNull.Value ? string.Empty : Convert.ToDecimal(Eval("TotalReferral")).ToString("c0") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Total Referral Funded $" SortExpression="TotalReferralFunded" ItemStyle-Width="120px" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <a href="javascript:GoToLoanDetails('<%# Eval("TotalReferralFundedFileIDs")%>')"><%# Eval("TotalReferralFunded") == DBNull.Value ? string.Empty : Convert.ToDecimal(Eval("TotalReferralFunded")).ToString("c0") %></a>
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
    </form>
</body>
</html>
