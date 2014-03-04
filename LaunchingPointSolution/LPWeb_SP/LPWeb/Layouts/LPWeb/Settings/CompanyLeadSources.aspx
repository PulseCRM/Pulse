<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyLeadSources.aspx.cs" Inherits="Settings_CompanyLeadSources" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        var gridId = "#<%=gvLeadSourceses.ClientID %>";
        $(document).ready(function () {
            DrawTab();

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

        function getSelectedItems() {
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });

            $("#<%=hfdChkIds.ClientID %>").val(selctedItems.join(","));
        }

        
        function delConfirm() {
            var hfdids = $("#<%=hfdChkIds.ClientID%>").val();

            if (hfdids  == "") {
                alert('Please select at least one record to delete.');
                return false;
            }
            ret = window.confirm("This operation is not reversible and will remove all the associated lead routing mechanisms for the selected lead source(s). Are you sure you want to delete the selected records?")

            return ret;
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
                            <li id="current"><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>
                            <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>
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
                            <td height="20" colspan="2" style="padding-top: 9px; padding-bottom: 9px;">
                                <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click">Add</asp:LinkButton> &nbsp;&nbsp;|&nbsp;&nbsp;
                                <asp:LinkButton ID="lbtnDel" runat="server" OnClientClick="if ( ! delConfirm()) return false;" OnClick="lbtnDel_Click">Delete</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="divDivision" class="ColorGrid" style="width: 760px; margin-top: 5px;">
                                        <asp:HiddenField runat="server" ID="hfdChkIds"/>
                                       <asp:GridView ID="gvLeadSourceses" runat="server" CssClass="GrayGrid" AutoGenerateColumns="False"
                                        EmptyDataText="There is no data in database." CellPadding="3" 
                                        DataKeyNames="LeadSourceID" GridLines="None"
                                         OnRowEditing="gvLeadSourceses_RowEditing"
                                         OnRowDataBound="gvLeadSourceses_RowDataBound"
                                         OnRowUpdating="gvLeadSourceses_RowUpdating">
                                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                        <AlternatingRowStyle CssClass="EvenRow" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn" HeaderStyle-Width="60px" ItemStyle-Width="60px">
                                                <HeaderTemplate>
                                                    <input type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="checkbox" tag='<%# Eval("LeadSourceID") %>'  />
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <input type="checkbox" tag='<%# Eval("LeadSourceID") %>' />
                                                </AlternatingItemTemplate>
                                               <EditItemTemplate>
                                                   <input type="checkbox" />
                                                   
                                               </EditItemTemplate>


                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Lead Source" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%# Eval("LeadSource") %>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <%# Eval("LeadSource") %>
                                                </AlternatingItemTemplate>
                                                <EditItemTemplate>
                                                      <table width="250px">
												        <tr>
												            <td><asp:TextBox runat="server" ID="txtLeadSource" Text='<%# Eval("LeadSource") %>'></asp:TextBox></td>
                                                            <td width="80px"><asp:RequiredFieldValidator ID="rfLeadSource" ControlToValidate="txtLeadSource" runat="server" ErrorMessage="required"></asp:RequiredFieldValidator></td>
												        </tr>
												     </table>
                                                 </EditItemTemplate>
												 <FooterTemplate>
												     <table width="250px">
												        <tr>
												            <td><asp:TextBox ID="tbxLeadSource" Runat="server"></asp:TextBox></td>
                                                            <td width="80px"><asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="AddLeadSource" ControlToValidate="tbxLeadSource" runat="server" ErrorMessage="required"></asp:RequiredFieldValidator></td>
												        </tr>
												     </table>
                								 </FooterTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Default User to Route Leads to" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <%# Eval("DefaultUser")%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <%# Eval("DefaultUser")%>
                                                </AlternatingItemTemplate>
                                                 <EditItemTemplate>
                                                      <asp:HiddenField ID="hfDefaultUser" runat="server" Value='<%# Eval("DefaultUserId") %>' />
                                                      <asp:DropDownList ID ="ddlDefaultUser" runat="server" ></asp:DropDownList> 
                                                 </EditItemTemplate>                   
												          <FooterTemplate>
														  <asp:DropDownList ID="ddlUsers" runat="server">
														   </asp:DropDownList>
														    </FooterTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Default" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%# Eval("DefaultString")%>
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <%# Eval("DefaultString")%>
                                                </AlternatingItemTemplate>
                                                 <EditItemTemplate>
                                                      <asp:CheckBox ID="cbxDefaultStr" runat="server" Checked="True" />Yes
                                                 </EditItemTemplate>
<FooterTemplate>
                    <asp:CheckBox ID="cbxDefaultStr" runat="server" Checked="True" />Yes
                </FooterTemplate>

                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" HeaderStyle-CssClass="CheckBoxHeader" HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" 
                                                    CommandName ="Edit" CommandArgument='<%# Eval("LeadSourceID")%>'  />
                                                </ItemTemplate>
                                                <AlternatingItemTemplate>
                                                    <asp:Button ID="btnEdit" Text="Edit" runat="server" 
                                                    CommandName ="Edit" CommandArgument='<%# Eval("LeadSourceID")%>'  />
                                                </AlternatingItemTemplate>
                                                 <EditItemTemplate>
                                                     <asp:Button ID="btnEdit" Text="Save" runat="server" 
                                                    CommandName ="Update" CommandArgument='<%# Eval("LeadSourceID")%>'  />
                                                 </EditItemTemplate>
<FooterTemplate>
                    <asp:Button ID="btnAddSave" Text="Save" runat="server" OnClick="btnAddSave_Click" ValidationGroup="AddLeadSource"/>
                                
                </FooterTemplate>

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
                               
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
   
</asp:Content>
