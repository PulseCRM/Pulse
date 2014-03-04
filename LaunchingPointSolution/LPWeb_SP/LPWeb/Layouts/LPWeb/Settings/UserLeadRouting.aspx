<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserLeadRouting.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.UserLeadRouting" 

DynamicMasterPageFile="~masterurl/default.master" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head>
        <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
        <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
        <script src="../js/common.js" type="text/javascript"> </script>
        <script src="../js/jquery.js" type="text/javascript"> </script>
        <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
        <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
        <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
        <script src="../js/urlparser.js" type="text/javascript"> </script>
        <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"> </script>
        <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"> </script>
        <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"> </script>
        <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"> </script>
        <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"> </script>
        <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"> </script>
        <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"> </script>
        <script src="../js/jquery.jscale.js" type="text/javascript"> </script>
        <script src="../js/jquery.tab.js" type="text/javascript"> </script>
        <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
        <title>User Lead Routing </title>
        <script type="text/javascript">
            $(document).ready(function () {
                DrawTab();
                $("#cbxAcceptRouteLeads").change(toggleCheckListState);
                toggleCheckListState();
                checkAllList();
                $("#tbMaxDay").keyup(function () {
                    $(this).val($(this).val().replace(/[^0-9]/g, ''));
                }).bind("paste", function () {  //CTR+V事件处理  
                    $(this).val($(this).val().replace(/[^0-9]/g, ''));
                }).css("ime-mode", "disabled"); //CSS设置输入法不可用  
            });

            function toggleCheckListState() {
                if ($("#cbxAcceptRouteLeads").is(":checked")) {
                    $(".tblChecklist :checkbox").each(function () {
                        $(this).attr('disabled', false);
                        $(this).closest('span').removeAttr('disabled');
                        $(this).closest('table').removeAttr('disabled');
                    });
                    $(".tblChecklist :checkbox").removeAttr("disabled");
                    $("#tbMaxDay").removeAttr("disabled");
                } else {
                    $("#tbMaxDay").val("0");
                    $(".tblChecklist :checkbox").attr("disabled", "disabled");
                    $("#tbMaxDay").attr("disabled", "disabled");
                }
            }

            function checkAllList() {
                var list = $(".tblChecklist :checkbox").change(function () {
                    var $this = $(this),
                        chkVal = $this.is(":checked") ? "checked" : "";

                    if ($.trim($this.prev().html().toLowerCase()) == "all") {
                        $this.parents("table:first").find(":checkbox").attr("checked", chkVal);
                    } else {
                        var allChk = true;
                        $this.parents("table:first").find(":checkbox:gt(0)").each(function () {
                            if (!$(this).is(":checked")) {
                                allChk = false;
                                return false;
                            }
                        });
                        var v = allChk ? "checked" : "";
                        $this.parents("table:first").find(":checkbox").first().attr("checked", v);
                    }
                });
            }

            function delConfirm() {

                var ret = window.confirm("This will delete all the Lead Routing Settings for the selected user. Are you sure you want to continue?");

                return ret;
            }

            function BeforeSave() {
                if ($("#cbxAcceptRouteLeads").is(":checked")) {
                    if (!$("#tbMaxDay").val() || parseInt($("#tbMaxDay").val()) <= 0) {
                        alert('"Max Leads per Day" must be greater than zero');
                        return false;
                    }
                }

                ShowWaitingDialog("Waiting...");
                return true;
            }

            function ShowWaitingDialog(WaitingMsg) {
                $("#WaitingMsg").text(WaitingMsg);
                $.blockUI({ message: $('#divWaiting'), css: { width: '225px'} });
                return true;
            }

            function CloseWaitingDialog() {
                $.unblockUI();
            }

            function showWaiting() {
                var isValid = $("#form1").validate().form();
                if (isValid == false) {
                    return false;
                }

            }
        </script>
        <style>
            /*.tblChecklist { padding-left: 100px; }*/

            .tblChecklist td { padding-left: 50px;text-align: right; }
            .tblChecklist td label { margin-right:5px; }
            table{
                font-size:11px;
            }
        </style>
    </head>   
    <body style="margin: 0; padding: 0;">
        <form id="form1" runat="server">
           
            <div class="JTab" style="margin-top: 10px;">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="width: 10px;">&nbsp;</td>
                        <td>
                            <div id="tabs10">
                                <ul>
                                    <li ><a href="UserSetup.aspx?mode=1&uid=<%= UserId %>&t=<% = Random %>"><span>General Settings</span></a></li>
                                    <li id="current"><a href="UserLeadRouting.aspx?uid=<%= UserId %>&t=<% = Random %>"><span>Lead Routing</span></a></li>
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
                        <div id="aspnetForm">
                            <table>
                                <tr>
                                    <td>
                                        User Name:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbUserName" class="iTextBox" Style="width: 150px;" Enabled="False"/>
                                    </td>
                                    <td>
                                        First Name:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbFirstName" class="iTextBox" Style="width: 150px;" Enabled="False"/>
                                    </td>
                                    <td>
                                        Last Name:
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="tbLastName" class="iTextBox" Style="width: 150px;" Enabled="False"/>
                                    </td>
                                </tr>  
                            </table>
                            
                            <table>
                                <tr>
                                    <td width="120px">
                                        <asp:Button ID="btnSave" runat="server" Text="Save Lead Routing" class="Btn-250" OnClick="btnSave_Click" OnClientClick="return BeforeSave()"/>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnDelete" runat="server" OnClientClick="if ( ! delConfirm()) return false;" Text="Delete Lead Routing" class="Btn-250" OnClick="btnDelete_Click"  />
                                    </td>
                                </tr>
                            </table>
                               
                            <div class="DashedBorder" style="">
                                &nbsp;</div>
                            <div>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="border-right-style: groove; border-right-width: 2px; padding-left: 50px; width: 50%;">
                                            <asp:CheckBox ID="cbxAcceptRouteLeads" runat="server" Text="Accept Routed Leads" onClick="toggleCheckListState()"  />
                                        </td>
                                        <td style="padding-left: 50px;">
                                            Max Leads per Day
                                            <asp:TextBox runat="server" ID="tbMaxDay" Text="0" Width="30px"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="DashedBorder">
                                &nbsp;</div>
                            <div>
                                <table>
                                    <tr>
                                        <td><b>Lead Sources:</b></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList TextAlign="Left" runat="server" ID="cblLeadSources" CssClass="tblChecklist" RepeatColumns="4" RepeatDirection="Vertical"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="DashedBorder" style="margin-top: 8px;">
                                &nbsp;</div>
                            <div>
                                <table>
                                    <tr>
                                        <td><b>Licensed States:</b></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBoxList TextAlign="Left" runat="server" ID="cblLicensedStates" CssClass="tblChecklist" RepeatColumns="5" RepeatDirection="Vertical"/>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="DashedBorder" style="margin-top: 8px;">
                                &nbsp;</div>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width:50%;border-right-style: groove; border-right-width: 2px;">
                                            <table style="width:100%">
                                                <tr><td><b>Purpose:</b></td>
                                                </tr>
                                                <tr><td> 
                                                    <asp:CheckBoxList TextAlign="Left" runat="server" ID="cblPurposes" CssClass="tblChecklist" RepeatColumns="2" RepeatDirection="Vertical"/> 
                                                </td></tr>
                                            </table>
                                    </td>
                                     <td style="vertical-align: top;">
                                            <table style="width:100%">
                                                <tr><td><b>Type:</b></td>
                                                </tr>
                                                <tr><td> 
                                                    <asp:CheckBoxList TextAlign="Left" runat="server" ID="cblTypes" CssClass="tblChecklist" RepeatColumns="2" RepeatDirection="Vertical"/>
                                                </td></tr>
                                            </table>
                                    </td>
                                  
                                </tr>
                               
                           </table>
                        </div>
                    </div>
                </div>
             
            </div>
            <div id="divWaiting" style="display: none; padding: 2px;">
                <table style="margin-left: auto; margin-right: auto;">
                <tr>
                    <td>
                        <img id="imgWaiting" src="../images/waiting.gif" />
                    </td>
                    <td style="padding-left: 5px;">
                        <label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
                    </td>
                </tr>
                </table>
            </div>
        </form>
    </body>
</html>
