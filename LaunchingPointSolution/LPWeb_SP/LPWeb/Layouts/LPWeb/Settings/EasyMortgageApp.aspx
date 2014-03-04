<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EasyMortgageApp.aspx.cs" 
Inherits="Settings_EasyMortgageApp" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryFileTree.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.thickbox.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.thickbox.js" type="text/javascript"></script>
    <script src="../js/jqueryFileTree.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var contextFile;
        $(document).ready(function () {
            DrawTab();
            $("input.thickbox").each(function () {
                flt = $(this).attr("filter");
                container = $(this).attr("container");
                initDir = $(this).attr("initDir");
                if (initDir == "") {
                    initDir = "/";
                }
                $('#' + container).fileTree({ root: initDir, script: 'FileTree.ashx', filter: flt }, function (file) {
                    fileHandller(file);
                });
                $(this).click(function () {
                    contextFile = $(this).prev("input");
                });
            });
        });
        function fileHandller(file) {
            if (contextFile != null) {
                contextFile.val(file);
            }
            tb_remove();
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
        .TabContent table td
        {
            padding-top: 9px;
        }
        a.Btn-91:link, a.Btn-91:visited, a.Btn-91:active
        {
            color: #77787B;
            font-size: 11px;
            text-decoration: none;
        }
        a.Btn-91:hover
        {
            color: #77787B;
            text-decoration: none;
            font-size: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        Company Setup</div>
    <div id="iniFile" style="display: none;">
    </div>
    <div id="mapFile" style="display: none;">
    </div>
    <div id="crdxFile" style="display: none;">
    </div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                <li id="current"><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
                                <%--<li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
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

                    <div class="subJTab">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td style="width: 10px;">
                                    &nbsp;
                                </td>
                                <td>
                                    <div id="tabsubs10">
                                        <ul>
                                            <li><a href="CompanyPoint.aspx"><span>Point</span></a></li>
                                            <li id="currentsub"><a href="EasyMortgageApp.aspx"><span>Easy Mortgage App</span></a></li>
                                            <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
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
                            <div class="TabContent">



                    <table style=" margin:10px;">
                        <tr>
                        <td colspan="4">
                              <asp:CheckBox ID="ckEnable" runat="server" Text=" Enable sync of Active Loans to Easy Mortgage App" /></td>
                        </tr>
                        <tr>
                            <td>
                                Send data every  
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txbSyncIntervalHours" runat="server" value="24" Width="40"></asp:TextBox> hours
                            </td>
                        </tr>


                        <tr>
                            <td>
                                Post data URL
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtURL" runat="server" Width="310"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Client ID
                            </td>
                            <td>
                                <asp:TextBox ID="txtClientID" runat="server" Width="190"></asp:TextBox>
                            </td>

                            <td>
                                Security Token
                            </td>
                            <td>
                                <asp:TextBox ID="txtSecurityToken" runat="server" Width="240" Enabled="false"></asp:TextBox>
                            </td>

                        </tr>
                        
                        <tr style="height: 20px;">
                            <td colspan="4">
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" />
                            </td>
                        </tr>
                    </table>



                            </div>
                    </div>
                    </div>


                </div>
            </div>
        </div>
    </div>
</asp:Content>