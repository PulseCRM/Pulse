<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Settings - Company MCT" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" AutoEventWireup="true" CodeBehind="CompanyMCT.aspx.cs" Inherits="Settings_CompanyMCT" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>


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
// <![CDATA[

        $(document).ready(function () {

            DrawTab();

            $("#aspnetForm").validate({

                rules: {
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbClientID: {
                        required: true
                    },
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbMCTPostURL: {
                        url: true
                    }
                },
                messages: {
                    ctl00$ctl00$PlaceHolderMain$MainArea$tbClientID: {
                        required: "<div>Please enter MCT Client ID.</div>"
                    }
                }
            });
            EnablePostData_CheckChange();
        });




        function BeforeSave() {

            var sURL = $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbMCTPostURL").val();
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbMCTPostURL").attr("value",$.trim(sURL));


            // call validate
            var IsValid = $("#aspnetForm").valid();
            if (IsValid == false) {

                return false;
            }

            return true;
        }

        function EnablePostData_CheckChange() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_chkEnablePostData").attr("checked") == true) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbMCTPostURL").removeAttr("disabled");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlActiveLoanInterval").removeAttr("disabled");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlArchivedLoanDisposeMonth").removeAttr("disabled");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlArchivedLoanInterval").removeAttr("disabled");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_tbMCTPostURL").attr("disabled", "true");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlActiveLoanInterval").attr("disabled", "true");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlArchivedLoanDisposeMonth").attr("disabled", "true");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlArchivedLoanInterval").attr("disabled", "true");

            }
        }


// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer">
        <div id="divModuleName" class="Heading">Company Setup - MCT</div>
        <div style="margin-top: 15px;">
            <div class="JTab">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 10px;">&nbsp;</td>
                        <td>
                            <div id="tabs10">
<%--                                <ul>
                                    <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                    <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                    <li id="current"><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                    <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                    <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                    <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                                    <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
                                    <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
                                </ul>--%>
                                
                                <ul>
                                    <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                    <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                    <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                    <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                    <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                    <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                                   <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                    <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
    <%--                                <li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                                   <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                    <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                    <li id="current"><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                    <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="TabBody">
                    <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
                    <div id="TabLine2" class="TabRightLine">&nbsp;</div>
                    <div class="TabContent">

<%--                        <div class="subJTab">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td style="width: 10px;">
                                    &nbsp;
                                </td>
                                <td>
                                    <div id="tabsubs10">
                                        <ul>
                                            <li><a href="CompanyPoint.aspx"><span>Point</span></a></li>
                                            <li><a href="EasyMortgageApp.aspx"><span>Easy Mortgage App</span></a></li>
                                            <li id="currentsub"><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                        </ul>
                                    </div>
                                </td>
                            </tr>
                        </table>
                        <div id="TabsubBody">
                            <div id="TabsubLine1" class="TabLeftLine" style="width: 242px">
                                &nbsp;</div>
                            <div id="TabsubLine2" class="TabRightLine" style="width: 434px">
                                &nbsp;</div>
                            <div class="TabContent">--%>


                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 350px;">
                               MCT Client ID: &nbsp;&nbsp;&nbsp;<asp:TextBox ID="tbClientID" runat="server" Text="" Width="200px" MaxLength="255"></asp:TextBox>
                            </td>
                            <td style="width: 273px;">
                               <asp:CheckBox ID="chkEnablePostData" runat="server" Text=" Enabled scheduled sending of pipeline data" onclick="javascript:EnablePostData_CheckChange()"/>
                            </td>
                        </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 623px;">
                               MCT Post URL: &nbsp;&nbsp;<asp:TextBox ID="tbMCTPostURL" runat="server" Text="" Width="500px" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 450px;">
                               Send active loan data every &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlActiveLoanInterval" runat="server" Width="200">
                                                <asp:ListItem Text=" 30 minutes" Value="30" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text=" 45 minutes" Value="45"></asp:ListItem>
                                                <asp:ListItem Text=" 60 minutes" Value="60"></asp:ListItem>
                                                <asp:ListItem Text=" 90 minutes" Value="90"></asp:ListItem>
                                                <asp:ListItem Text=" 120 minutes" Value="120"></asp:ListItem>
                                            </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 600px;">
                               Send archive loans that are &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlArchivedLoanDisposeMonth" runat="server" Width="150">
                                                <asp:ListItem Text=" 1 month" Value="1"></asp:ListItem>
                                                <asp:ListItem Text=" 2 months" Value="2"></asp:ListItem>
                                                <asp:ListItem Text=" 3 months" Value="3" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text=" 4 months" Value="4"></asp:ListItem>
                                                <asp:ListItem Text=" 5 months" Value="5"></asp:ListItem>
                                                <asp:ListItem Text=" 6 months" Value="6"></asp:ListItem>
                                            </asp:DropDownList> &nbsp;&nbsp;old using Canceled, Closed, Denied and Sold Dates.
                            </td>
                        </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 450px;">
                               Send archived loan data every &nbsp;&nbsp;
                                <asp:DropDownList ID="ddlArchivedLoanInterval" runat="server" Width="150">
                                                <asp:ListItem Text=" 24 hours" Value="24" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text=" 48 hours" Value="48"></asp:ListItem>
                                                <asp:ListItem Text=" 36 hours" Value="36"></asp:ListItem>
                                            </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    
                    <table>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave();" OnClick="btnSave_Click" /><asp:Button
                                    ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" />
                            </td>
                        </tr>
                    </table>



                            </div>
<%--                        </div>
                        </div>


                    </div>--%>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
