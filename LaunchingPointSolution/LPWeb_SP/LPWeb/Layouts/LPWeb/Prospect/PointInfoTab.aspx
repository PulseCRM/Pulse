<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PointInfoTab.aspx.cs" Inherits="PointInfoTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.thickbox.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
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
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>

    <style>
        div.opt
        {
            margin-top: 20px;
        }
        div.opt input
        {
            margin-right: 10px;
        }
        form
        {
            padding-left: 10px;
        }
        .detail
        {
            margin-top: 20px;
        }
        .detail td
        {
            line-height: 20px;
            white-space: nowrap;
        }
    </style>
    <script type="text/javascript">
        $().ready(function () {
            //$("#btnCancel").click(CloseCurrentWindow);

            $("#btnCreatePointFile").click(function () {
                ShowWaitingDialog("Please wait...");
                var RadomNum = Math.random();
                var Radom = RadomNum.toString().substr(2);

                var LoanID = GetQueryString1("FileID");

                var op = "CreatePointFile";
                $.getJSON("PointInfoTab_BG.aspx?sid=" + Radom + "&op=" + op + "&LoanID=" + LoanID + otherUrl(), function (data) {

                    if (data.ExecResult == "Failed") {
                        $('#divDetail').unblock();
                        alert(data.ErrorMsg);
                        //RefreshPage();
                        return false;
                    }
                    else {
                        RefreshPage();
                        return false;
                    }

                });

                return false;
            });

            $("#btnUpdatePoint").click(function () {

                // show waiting
                window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

                var FileID = GetQueryString1("FileID");
                var RadomNum = Math.random();
                var sid = RadomNum.toString().substr(2);

                $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileID, function (data) {

                    window.parent.parent.parent.CloseWaitingDialog3();

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {

                        // if locked
                        if (data.ErrorMsg != "") {

                            alert(data.ErrorMsg);
                            return false;
                        }
                        else {

                            // continue
                            ShowWaitingDialog("Updating point file...");
                            sid = RadomNum.toString().substr(2);

                            var op = "updatepoint";
                            $.getJSON("PointInfoTab_BG.aspx?sid=" + sid + "&op=" + op + "&LoanID=" + FileID + otherUrl(), function (data) {

                                window.parent.parent.parent.CloseWaitingDialog3();

                                if (data.ExecResult == "Failed") {

                                    alert(data.ErrorMsg);

                                    return false;
                                }
                                else {
                                    RefreshPage();
                                    return false;
                                }

                            });

                            return false;
                        }
                    }
                });
            });

            $("#btnSyncNow").click(function () {

                ShowWaitingDialog("Please wait...");
                var RadomNum = Math.random();
                var Radom = RadomNum.toString().substr(2);

                var LoanID = GetQueryString1("FileID");
                var op = "syncnow";
                $.getJSON("PointInfoTab_BG.aspx?sid=" + Radom + "&op=" + op + "&LoanID=" + LoanID + otherUrl(), function (data) {

                    if (data.ExecResult == "Failed") {
                        $('#divDetail').unblock();
                        alert(data.ErrorMsg);
                        //RefreshPage();
                        return false;
                    }
                    else {
                        RefreshPage();
                        return false;
                    }

                });

                return false;

            });

        });

        function otherUrl() {

            var q = "&BranchId=" + $.trim($("#ddlBranch").val());
            q += "&LoanOfficerId=" + $.trim($("#ddlLoanOfficer").val());
            q += "&FolderId=" + $.trim($("#ddlFolderName").val());
            q += "&filename=" + $.trim($("#txbFilename").val());

            return q;
        }


        //#region Show/Close Waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divDetail").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {
            var Msg = $('#hdnErrMsg').val();

            if (Msg.length <= 0)
                Msg = SuccessMsg;
            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(Msg);
            $('#aClose').show();

            //$('#divDetail').unblock();
        }

        function CloseDialog() {
            $('#divDetail').unblock();
            return false;
        }

        //#endregion


        function RefreshPage() {
            window.location.href = window.location.href;
        }

    </script>
</head>
<body>
    <form id="form1" runat="server" style=" margin:0px;">
    <div id="divDetail" style=" height:540px; margin-left:0px;">
        <table class="detail">
            <tr>
                <td style="padding-right: 50px;">
                    Branch:
                </td>
                <td>
                   <asp:DropDownList ID="ddlBranch" runat="server" Enabled="false" Width="440" DataTextField ="Name" DataValueField ="BranchId" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_OnChanged">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                     Loan Officer:
                </td>
                <td>
                <asp:DropDownList ID="ddlLoanOfficer" runat="server" Enabled="false" Width="440">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Filename:                    
                </td>
                <td>
                <asp:TextBox ID="txbFilename" runat="server" Width="440"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Folder Name:
                </td>
                <td>
                    <asp:DropDownList ID="ddlFolderName" runat="server" Width="440">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Last Sync:
                </td>
                <td>
                <asp:TextBox ID="txbLastSync" runat="server" ReadOnly="true" Width="440"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Path:
                </td>
                <td>
                    <asp:TextBox ID="txbPath" runat="server" ReadOnly="true" Width="800"></asp:TextBox>
                </td>
            </tr>
        </table>
    
    <asp:HiddenField ID="hfFileID" runat="server" />
    <br />
    <div id="divButtons" class="opt">
        <input id="btnCreatePointFile" type="button" value="Create Point File" class="Btn-140" runat="server" />
        <input id="btnUpdatePoint" type="button" value="Update Point" class="Btn-140"  runat="server" />
        <input id="btnSyncNow" type="button" value="Sync Now" class="Btn-140" runat="server" />

    </div>

    </div>
    </form>

    <div style=" display:none;">
    <input id="hdnErrMsg" type="hidden" value="" />
    <div id="divWaiting" style="display: none; padding: 5px;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
                </td>
                <td style="padding-left: 5px;">
                    <label id="WaitingMsg" style="color: #818892; font-weight: bold;">
                    </label>
                    &nbsp;&nbsp; <a id="aClose" href="javascript:CloseDialog();return false;" style="font-weight: bold;
                        color: #6182c1;">[Close]</a>
                </td>
            </tr>
        </table>
    </div>

    </div>
</body>
</html>
