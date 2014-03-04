<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompanyReport.aspx.cs" Inherits="Settings_CompanyReport" 
MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .TabContent input.Btn-66, input.Btn-91
        {
            margin-right: 8px;
        }
        
        .TabContent table td
        {
            padding-top: 9px;
        }
    </style>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();
            // add event
            $("#<%=ddlRole.ClientID %>").change(ddlRole_onchange);
        });

        function ddlRole_onchange() {
            var SelRole = $("#<%=ddlRole.ClientID %>").val();
            if (SelRole != "0") {

               $("#<%=tbSenderEmail.ClientID %>").attr("disabled", "true");
               $("#<%=tbSenderName.ClientID %>").attr("disabled", "true");
            }
            else {
                $("#<%=tbSenderEmail.ClientID %>").attr("disabled", "");
                $("#<%=tbSenderName.ClientID %>").attr("disabled", "");
            }
        }
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleNe" class="Heading">Company Report</div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">&nbsp;</td>
                    <td>
                        <div id="tabs10">
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


                                <li  id="current"><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine" style="width: 242px">&nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">&nbsp;</div>
                <div class="TabContent">
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 623px;">
                               For weekly report,  send every &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlDOW" runat="server" Width="100">
                                                <asp:ListItem Text="Monday" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Tuesday" Value="2"></asp:ListItem>
                                                <asp:ListItem Text=" Wednesday" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Thursday" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="Friday" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Saturday" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="Sunday" Value="7"></asp:ListItem>
                                            </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                     <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 623px;">
                               Send report at &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlTOD" runat="server" Width="80">
                                                <asp:ListItem Text="00:00" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="01:00" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="02:00" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="03:00" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="04:00" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="05:00" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="06:00" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="07:00" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="08:00" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="09:00" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="10:00" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="11:00" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="12:00" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="13:00" Value="13"></asp:ListItem>
                                                <asp:ListItem Text="14:00" Value="14"></asp:ListItem>
                                                <asp:ListItem Text="15:00" Value="15"></asp:ListItem>
                                                <asp:ListItem Text="16:00" Value="16"></asp:ListItem>
                                                <asp:ListItem Text="17:00" Value="17"></asp:ListItem>
                                                <asp:ListItem Text="18:00" Value="18"></asp:ListItem>
                                                <asp:ListItem Text="19:00" Value="19"></asp:ListItem>
                                                <asp:ListItem Text="20:00" Value="20"></asp:ListItem>
                                                <asp:ListItem Text="21:00" Value="21"></asp:ListItem>
                                                <asp:ListItem Text="22:00" Value="22"></asp:ListItem>
                                                <asp:ListItem Text="23:00" Value="23"></asp:ListItem>
                                            </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                     <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 623px;">
                               Sender Role &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlRole" runat="server" Width="150" onchange="">
                                                <asp:ListItem Text="– select –" Value="0" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                     <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 400px;">
                              Or sender  email address &nbsp;&nbsp;<asp:TextBox ID="tbSenderEmail" runat="server" Text="" Width="220px" MaxLength="255"></asp:TextBox>
                                <%--<asp:RequiredFieldValidator ID="rfvSenderEmail" runat="server" ErrorMessage="*"
                                    ControlToValidate="tbSenderEmail"></asp:RequiredFieldValidator>--%>
                                <asp:RegularExpressionValidator ID="revSenderEmail" runat="server" ErrorMessage="*"
                                    ControlToValidate="tbSenderEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </td>
                            <td style="width:210px;">
                               Sender  name &nbsp;&nbsp;<asp:TextBox ID="tbSenderName" runat="server" Text="" Width="120px" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" /><asp:Button
                                    ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="divContainer"></div>
    </div>
</asp:Content>
