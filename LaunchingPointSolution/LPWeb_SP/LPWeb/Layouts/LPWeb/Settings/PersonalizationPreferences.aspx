<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalizationPreferences.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Settings.PersonalizationPreferences" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    
    <style type="text/css">
        .TabContent table td
        {
            padding-top: 9px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();
        });
    </script>
    <script type="text/javascript">
// <![CDATA[
        var nPCSChecked = 0;
        function PCSSelected(ckb) {
            //alert(nPCSChecked);

            if (ckb.checked) {
               
                //alert(nPCSChecked);
                if (nPCSChecked >=12) {
                    alert("You can not pick up more than 12 selections.");
                    ckb.checked = false;
                    
                    return false;
                }
                else {
                    
                    nPCSChecked++;
                    return true;
                }
            }
            else {
                
                nPCSChecked--;
                return true;
            }
        }
        var nPPsCSChecked = 0;
        function PPsCSSelected(ckb) {
            if (ckb.checked) {
                if (nPPsCSChecked >=6) {
                    alert("You can not pick up more than six selections.");
                    ckb.checked = false;
                    return false;
                }
                else {
                    nPPsCSChecked++;
                    return true;
                }
            }
            else {
                nPPsCSChecked--;
                return true;
            }
        }
        var nPVPCSChecked = 0;
        function PVPCSelected(ckb) {
            if (ckb.checked) {
                if (nPVPCSChecked >= 6) {
                    alert("You can not pick up more than six selections.");
                    ckb.checked = false;
                    return false;
                }
                else {
                    nPVPCSChecked++;
                    return true;
                }
            }
            else {
                nPVPCSChecked--;
                return true;
            }
        }
        var allChartCkbCtlIds = new Array('<%=ckbPipelineChart.ClientID %>', '<%=ckbSalesBreakdownChart.ClientID %>',
        '<%=ckbOrgProductionChart.ClientID %>', '<%=ckbOrgProductSaleBreakdownChart.ClientID %>');
        var needCheckAllChart = true;
        var nHPChecked = 0;
        function HPSelected(ckb) {
            if (ckb.checked) {
                if (needCheckAllChart) {
                    var index = Array.indexOf(allChartCkbCtlIds, ckb.id);
                    if (!isNaN(index) && index != -1) {
                        for (var i = 0; i < allChartCkbCtlIds.length; i++) {
                            if (ckb.id != allChartCkbCtlIds[i] && $("#" + allChartCkbCtlIds[i]).attr("checked")) {
                                $("#" + allChartCkbCtlIds[i]).attr("checked", false);
                                nHPChecked--;
                            }
                        }
                    }
                }
                if (nHPChecked >= 6) {
                    alert("You can not pick up more than six selections.");
                    ckb.checked = false;
                    return false;
                }
                else {
                    nHPChecked++;
                    return true;
                }
            }
            else {
                nHPChecked--;
                return true;
            }
        }

        function btnManage_onclick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "../ManagePipelineViewsPopup.aspx?sid=" + sid

            var BaseWidth = 450;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.ShowGlobalPopup("Manage Pipeline Views", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }



        function CheckboxCheckChange() {

            //alert('11');


            var tt = document.getElementById("<%=cbxRemindTaskDue.ClientID%>");
            var t = document.getElementById("<%=txtReminderTime.ClientID%>");


            //alert(objcbx);
            if (!tt.checked) {
                //取消时
                
                t.disabled = true;
                var t1 = document.getElementById("<%=txtReminderTime.ClientID%>");
                t1.value = "15";
               

            }
            else {
               
                t.disabled = false;
            }



            return false;
        }

        function BeforeSave() {

            var tt = document.getElementById("<%=cbxRemindTaskDue.ClientID%>").checked;

            //alert(tt);
            if (tt) {
                //选中之后 判断是否为空              

                var t = document.getElementById("<%=txtReminderTime.ClientID%>").value;
                //alert(t);

                if (t != null && typeof (t) != "undefined" && t != "") {
                    if (/^[0-9]*[1-9][0-9]*$/.test(t)) {//这个正则表达式为整数                         
                        if (t < 5 || t >= 32767) {
                            alert("Enter 5 to 32767 between!");

                            return false;
                        }
                    }
                    else {
                        alert("Reminder Time is a positive integer!");

                        return false;
                    }
                }
                else {
                    alert("Reminder Time is null");
                    return false;
                }


            }
           



            return true;

        }


//        //duanlijun
//        function btnModifytest_onclick() {


//            //LoanID 该值对应的是[LoanTasks] 的FileId字段
//            var iFrameSrc = "../LoanDetails/TaskReminderPopup.aspx?LoanID=114";

//            var BaseWidth = 650;
//            var iFrameWidth = BaseWidth + 2;
//            var divWidth = iFrameWidth + 25;

//            var BaseHeight = 450;
//            var iFrameHeight = BaseHeight + 2;
//            var divHeight = iFrameHeight + 40;

//            window.parent.ShowGlobalPopup("Task Reminder Popup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
//        }

       
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        User Personalization</div>
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
                                <li><a href="PersonalizationSettings.aspx"><span>Settings</span></a></li>
                                <li id="current"><a href="PersonalizationPreferences.aspx"><span>Preferences</span></a></li>
                                <%--<li><a href="PersonalizationMarketing.aspx"><span>Marketing</span></a></li>--%>
                                <li><a href="PersonalizationLoansViewtTab.aspx"><span>Pipeline View</span></a></li>
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
                    <div>
                        <table cellpadding="0" cellspacing="0" style="margin-top: 15px;">
                            <tr>
                                <td style="width: 100px;">
                                    Loans per page
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlLoanPerPage" runat="server" Width="170px">
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="padding-left: 35px;">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click"  OnClientClick="return BeforeSave();" />
                                </td>
                                <td style="padding-left: 25px;" >
                                    <input id="btnManage" type="button" value="Manage My Pipeline Views" class="Btn-250" onclick="btnManage_onclick()" />
                                </td>

                               <%--  <td>
                                   <input id="Button1" runat="server" type="button" value="Log Lead Task Popup" class="Btn-66"
                                       onclick="btnModifytest_onclick()" />
                                   </td>--%>
                            </tr>
                        </table>
                        
                    </div>
                    <br />
                    <asp:CheckBox ID="cbxShowTasksInLSR" runat="server" Text=" Show pending task in the LSR that is sent to me" />

                     <br />
                      <br />
                     
                    <asp:CheckBox ID="cbxRemindTaskDue" runat="server" Text=" Display Task Reminder Popup" OnClick="CheckboxCheckChange()" />
                    <asp:TextBox ID="txtReminderTime" runat="server" Enabled="false" MaxLength="5"></asp:TextBox>&nbsp;minutes before due

                     <br />
                      <br />
                     
                     Show the Lead Tasks in the pick list in 
                     <asp:DropDownList ID="ddlSortTaskPickList" runat="server" Width="250px">
                            <asp:ListItem Selected="true" Value="A">alphabetical order</asp:ListItem>
                            <asp:ListItem Value="S">sequence</asp:ListItem>
                           
                        </asp:DropDownList>
                    <div style=" display:none;">
                    <div id="divModuleName1" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                        Loans View Pipeline Column Selections (pick up to 12)</div>
                    <div class="DashedBorder" style="margin-top: 8px;">
                        &nbsp;</div>
                    <div style="margin-top: 3px;">
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbBranch" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Branch
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbAmount" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;"> 
                                    Amount
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbPercentComplete" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Percent complete
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbLoanOfficer" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Loan Officer
                                </td>


                            </tr>


                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbPointFolder" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Point folder
                                </td>
                                
                                <td>
                                    <asp:CheckBox ID="ckbLien" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Lien
                                </td>

                                 <td>
                                    <asp:CheckBox ID="ckbTaskCount" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Task count
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbAssistant" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Loan Officer Assistant
                                </td>
                               
                            </tr>

                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbFilename" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Filename
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbRate" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Rate
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbAlerts" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Alerts
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbCloser" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Closer
                                </td>

                            </tr>
                            <tr>

                                <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="ckbLender" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Lender
                                </td>

                                <td>
                                    <asp:CheckBox ID="chkLastComplStage" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Last Completed Stage
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbDocPrep" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Doc Prep
                                </td>

                            </tr>

                            <tr>
                               
                                 <td></td>
                                <td></td>


                                <td>
                                    <asp:CheckBox ID="ckbLockExpirDate" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Lock expiration date
                                </td>

                                <td>
                                    <asp:CheckBox ID="chkLastStageComlDate" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Last Completed Stage Date
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbShipper" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Shipper
                                </td>
                                
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="cbxPurpose" runat="server" onclick="PCSSelected(this)" /> 
                                </td>
                                <td style="padding-left: 4px;">
                                    Purpose
                                </td>


                                <td>
                                    <asp:CheckBox ID="ckbStage" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Current Stage
                                </td>

                                <td>
                                    <asp:CheckBox ID="ckbProcessor" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Processor
                                </td>

                            </tr>
                            <tr>
                                
                                 <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="cbxLoanProgram" runat="server" onclick="PCSSelected(this)" /> 
                                </td>
                                <td style="padding-left: 4px;">
                                    Loan Program
                                </td>


                                 <td></td>
                                <td></td>
                               

                                <td>
                                    <asp:CheckBox ID="cbxJrProcessor" runat="server" onclick="PCSSelected(this)" /> 
                                </td>
                                <td style="padding-left: 4px;">
                                    Jr Processor
                                </td>
                                
                            </tr>
                            <tr>
                                
                                <td></td>
                                <td></td>

                                <td>
                                    <asp:CheckBox ID="ckbEstimatedClose" runat="server" onclick="PCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Estimated Close
                                </td>


                                <td></td>
                                <td></td>
                                
                                
                                <td></td>
                                <td></td>
                                
                            </tr>

                            <tr>
                                
                                
                                <td>
                                    
                                </td>
                                <td style="padding-left: 4px;">
                                    
                                </td>
                                
                            </tr>
                        </table>
                        <br />
                         Default Loans Pipeline View: <asp:DropDownList ID="ddlDefaultLoansPV" runat="server" DataTextField="ViewName" DataValueField="UserPipelineViewID"><asp:ListItem Selected="True" Text="-- select --" Value="0"></asp:ListItem> </asp:DropDownList>
                    </div>
                    </div>
                    <div id="divModuleName2" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                        Client View Pipeline Column Selections (pick up to six)</div>
                    <div class="DashedBorder" style="margin-top: 8px;">
                        &nbsp;</div>
                    <div style="margin-top: 3px;">
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbpvBranch" runat="server" onclick="PVPCSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Branch
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbpvCreate" runat="server" onclick="PVPCSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Date Created
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbpvLeadSource" runat="server" onclick="PVPCSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Lead Source
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbpvLoanOfficer" runat="server" onclick="PVPCSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Loan Officer
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbpvRefCode" runat="server" onclick="PVPCSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Reference Code
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbpvReferral" runat="server" onclick="PVPCSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Referral
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbpvPartner" runat="server" onclick="PVPCSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Partner(Referral Company)
                                </td>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                        <br />
                         Default Clients Pipeline View: <asp:DropDownList ID="ddlDefaultClientsPV" runat="server" DataTextField="ViewName" DataValueField="UserPipelineViewID"><asp:ListItem Selected="True" Text="-- select --" Value="0"></asp:ListItem> </asp:DropDownList>
                    </div>

                    <br /><br />
                    <hr />

                    <div id="divModuleName3" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                        Leads View Pipeline Column Selections (pick up to six)</div>
                    <div class="DashedBorder" style="margin-top: 8px;">
                        &nbsp;</div>
                    <div style="margin-top: 3px;">
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckblvAmount" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Amount
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvBranch" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Branch
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvLeadSource" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Lead Source
                                </td>
                                <td>
                                    <asp:CheckBox ID="chklvLastStageComplDate" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Last Stage Compl Date
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckblvLien" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Lien
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvLoanOfficer" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Loan Officer
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvLoanProgram" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Loan Program
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckblvPointFilename" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Point Filename
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvRanking" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Ranking
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvRate" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Rate
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckblvRefCode" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Reference Code
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvEstClose" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Est Close
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvProgress" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Percent complete
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckblvReferral" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Referral
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckblvPartner" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Partner(Referral Company)
                                </td>
                                <td>
                                    <asp:CheckBox ID="chklvLastComplStage" runat="server" onclick="PPsCSSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Last Completed Stage
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                        <br />
                         Default Leads Pipeline View: <asp:DropDownList ID="ddlDefaultLeadsPV" runat="server" DataTextField="ViewName" DataValueField="UserPipelineViewID"><asp:ListItem Selected="True" Text="-- select --" Value="0"></asp:ListItem> </asp:DropDownList>
                    </div>

                    <br /><br />
                    <hr />

                    <div id="div1" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">
                        Homepage Selections (pick up to six)</div>
                    <div class="DashedBorder" style="margin-top: 8px;">
                        &nbsp;</div>
                    <div style="margin-top: 3px;">
                        <table cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbCompanyCalendar" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 443px;">
                                    Company calendar
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbRatesSummary" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px; width: 210px;">
                                    Rates summary
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbPipelineChart" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Pipeline chart
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbGoalsChart" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Goals chart
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbSalesBreakdownChart" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Pipeline summary with sales breakdown chart
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbOverdueTasks" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Overdue tasks and alert summary
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbOrgProductionChart" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Pipeline summary with organizational production chart
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbAnnouncements" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Company announcements
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbOrgProductSaleBreakdownChart" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Pipeline summary with organizational production chart & sales breakdown chart
                                </td>
                                <td>
                                    <asp:CheckBox ID="ckbExchangeInbox" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Exchange inbox
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbExchangeCalendar" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Exchange calendar
                                </td> 

                                  <td>
                                    <asp:CheckBox ID="ckbQuickleadform" runat="server" onclick="HPSelected(this)" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Quick Lead Form
                                </td> 

                               
                            </tr>
                       
                       </table>
                       </div>
                       <div>
                      <br />
                      <table>
                         <tr>
                            <td style="width:460px;">
                            Show : <asp:DropDownList ID="ddlDashboardLastCompletedStages" runat="server">
                       <asp:ListItem Selected="True" Text="Current Stages" Value="0"></asp:ListItem> 
                       <asp:ListItem Text="Last Completed Stages" Value="1"></asp:ListItem> 
                       </asp:DropDownList> in my Active Loans Pipeline Summary Chart
                    
                       </td>
                       <td>
                                    Alerts Filter
                               
                                    <asp:DropDownList ID="ddlAlertsFilter" runat="server">
                                        <asp:ListItem Value="1"  Text="Overdue" Selected="True"></asp:ListItem>
                                        <asp:ListItem Value="7" Text="Overdue + Today" ></asp:ListItem>
                                        <asp:ListItem Value="2" Text="Due Today" ></asp:ListItem>
                                        <asp:ListItem Value="3" Text="Due Tomorrow" ></asp:ListItem>
                                        <asp:ListItem Value="4" Text="Due in the next 7 days" ></asp:ListItem>
                                        <asp:ListItem Value="5" Text="Due in the next 2 weeks" ></asp:ListItem>
                                        <asp:ListItem Value="6" Text="Due in the next month" ></asp:ListItem>
                                    </asp:DropDownList>
                              </td>
                           </tr>        
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
