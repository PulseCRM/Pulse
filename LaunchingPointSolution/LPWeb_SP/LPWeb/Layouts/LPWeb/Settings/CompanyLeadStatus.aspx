<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyLeadStatus.aspx.cs"
    Inherits="Settings_CompanyLeadStatus" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var gridId = "#<%=gvLeadStatus.ClientID %>";
        $(document).ready(function () {
            DrawTab();

            $("#<%= gvLeadStatus.ClientID %>").tablesorter({

                headers: {
                    0: { sorter: false }
                },
                widgets: ['zebra']
            });
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

            $("#<%= btnRemove.ClientID %>").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("Please select item to delete.");
                    return false;
                }
                else {
                    return confirm("Delete selected item(s)?");
                }
            });
        });


        function getSelectedItems() {
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            $("#<%=hfDeleteItems.ClientID %>").val(selctedItems.join(","));
        }
    </script>
    <style>
        .TabContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabContent input[type="text"], select, input[type="file"]
        {
            margin-left: 15px;
            margin-right: 15px;
        }
        #divUserList td
        {
            margin-left: 0px;
            margin-right: 0px;
            padding: 0;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        Company Setup</div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
             <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs11">
                            <ul>
                                <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
                                <%--<li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <ul>
                            <li><a href="CompanyTaskPickList.aspx"><span>Task Pick List</span></a></li>
                            <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>
                            <li id="current"><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine" style="width: 242px">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">
                    &nbsp;</div>
                <div class="TabContent">
                    <table>
                        <tr>
                            <td width="70px">
                                Lead Status
                            </td>
                            <td width="*">
                                <asp:TextBox ID="txtLeadSource" runat="server" Text="" Width="320" MaxLength="150"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvLeadSource" runat="server" ErrorMessage="*" ControlToValidate="txtLeadSource"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td height="20" colspan="2" style="padding-top: 9px; padding-bottom: 9px;">
                                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="Btn-66" OnClick="btnAdd_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="divDivision" class="ColorGrid" style="width: 760px; margin-top: 5px;">
                                    <asp:GridView ID="gvLeadStatus" runat="server" CssClass="GrayGrid tablesorter" AutoGenerateColumns="false"
                                        EmptyDataText="There is no data in database." CellPadding="3" GridLines="None">
                                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="EvenRow" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn"
                                                HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                <HeaderTemplate>
                                                    <input type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="checkbox" tag='<%# Eval("LeadStatusId") %>' />
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <input type="checkbox" tag='<%# Eval("LeadStatusId") %>' />
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lead Status" HeaderStyle-CssClass="CheckBoxHeader"
                                                HeaderStyle-Width="600px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%# Eval("LeadStatusName")%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <%# Eval("LeadStatusName")%>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Enabled" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <itemtemplate><%# (Boolean.Parse(Eval("Enabled").ToString())) ? "Yes" : "No"%></itemtemplate>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <itemtemplate><%# (Boolean.Parse(Eval("Enabled").ToString())) ? "Yes" : "No"%></itemtemplate>
                                                </AlternatingItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td height="20px" colspan="2">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td height="20" colspan="2">
                                <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="Btn-66" OnClick="btnRemove_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnDisable" runat="server" Text="Disable" CssClass="Btn-66" OnClick="btnDisable_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnEnable" runat="server" Text="Enable" CssClass="Btn-66" OnClick="btnEnable_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" Visible="false"
                                    CausesValidation="true" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel"
                                        CssClass="Btn-66" OnClick="btnCancel_Click" CausesValidation="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfDeleteItems" runat="server" />
</asp:Content>
