<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSendCompletionWindowOpen.aspx.cs" Inherits="LoanDetails_EmailSendCompletionWindowOpen" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send Completion Email</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        // <![CDATA[

        $(document).ready(function () {

            $("#gridRecipientList tr td:contains('No emaill address found for the selected recipient(s).')").css("color", "red");

            $("#cbAll").change(function () {

                if ($(this).attr("checked") == null || $(this).attr("checked") == false) {
                    $("input[cid='cbMe']").attr("checked", "");
                }
                else {
                    $("input[cid='cbMe']").attr("checked", "checked");
                }

            });
        });

        function btnPreview_onclick(id) {

            var EmailTemplateID = $("select[cid='ddlEmailTemplate_" + id + "']").val();   //GetQueryString1("EmailTemplateID");
            if (EmailTemplateID == "0") {

                alert("Please select a email template.");
                return false;
            }
            var LoanID = GetQueryString1("LoanID");

            OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_Preview7", 760, 580, "no", "center");

            return false;
        }

        function BeforeSend() {

            //Now,sending mail.

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var LoanID = GetQueryString1("LoanID");
            var TaskID = GetQueryString1("TaskID");
            var EmailTemplateID = "";

            var checkedList = $("input[cid='cbMe']:checked");

            if (checkedList.size() == 0) {
                alert("Please select one or more templates!");
                return false;
            }

            //#region Use EWS

            var UseEWS = $("#hdnUseEWS").val();
            if (UseEWS == "True") {

                var result = confirm("In order to send the email, you need to have your password recorded in the database. Please go to Settings-->My Account, enter your password and save it. \r\n\r\nIf you send the email without authentication now, your Outlook Sent Items will be out of sync with Pulse. Do you want to continue to send the email now?");

                if (result == false) {

                    return false;
                }
            }

            //#endregion

            $.each(checkedList, function (i, cb) {

                var Id = $(cb).val();
                var emailTmpId = $("select[cid='ddlEmailTemplate_" + Id + "']").val();
                //alert(emailTmpId);
                //return;
                if (i == 0) {
                    EmailTemplateID = emailTmpId;
                }
                else {
                    EmailTemplateID = EmailTemplateID + "," + emailTmpId;
                }
            });

            ShowWaitingDialog("Sending Email...");

            var url_bg = "EmailSendConmpletion_Background.aspx?sid=" + Radom + "&LoanID=" + LoanID + "&TaskID=" + TaskID + "&EmailTemplateID=" + EmailTemplateID;

            $.ajax({
                type: "GET",
                url: url_bg,
                processData: false,
                data: "{}",
                cache: false,
                dataType: "json",
                success: sendok,

                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#bodydiv').unblock();
                    alert(textStatus);

                }
            });

            //always false  not postback
            return false;
        }

        function sendok(data) {
            if (data.ExecResult == "Failed") {
                $('#bodydiv').unblock();
                alert(data.ErrorMsg);
                //CloseWaitingDialog(data.ErrorMsg);
                return;
            }

            alert("Sent completion email successfully.");

            //window.parent.CloseDialog_SendCompletionEmail();
            window.close();
        }

        //#region Show/Close Waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#bodydiv").block({ message: $('#divWaiting'), css: { width: '590px'} });
        }

        function CloseWaitingDialog(SuccessMsg) {
            var Msg = $('#hdnErrMsg').val();

            if (Msg.length <= 0)
                Msg = SuccessMsg;
            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(Msg);
            $('#aClose').show();
        }

        //#endregion

// ]]>
    </script>
</head>
<body>
<div id="bodydiv">
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 600px; height:405px;" class="AlignCenter">
        <div id="divModuleName" class="ModuleTitle">Send Completion Email</div>
        <div class="SplitLine" style=" margin-bottom:10px;"></div>

        <div id="oldInfo" style=" display:none;" >
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
            <tr>
                <td style="width: 90px;">Email Template:</td>
                <td>
                    <asp:Label ID="lbEmailTemplate" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            
        </table>
        <table cellpadding="0" cellspacing="0" style="margin-top: 10px;">
            <tr>
                <td style="width: 90px;">Sender:</td>
                <td>
                    <asp:Label ID="lbSender" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
        <div style="margin-top: 15px;">Recipients:</div>
        <div id="divRecipientList" class="ColorGrid" style="margin-top: 3px; width: 590px;">
            
            <asp:GridView ID="gridRecipientList" runat="server" EmptyDataText="There is no recipient." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:BoundField DataField="RecipientType" HeaderText="Type" ItemStyle-Width="80px" />
                    <asp:BoundField DataField="FullName" HeaderText="Name" ItemStyle-Width="170px" />
                    <asp:BoundField DataField="Email" HeaderText="Email" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        </div>


        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="width: 300px;">Borrower: <asp:Label ID="lbBorrower" runat="server" Text=""></asp:Label></td>
                <td style="width: 300px;">Task: <asp:Label ID="lbTask" runat="server" Text=""></asp:Label></td>
            </tr>
        </table>
        <br />
        <div id="divLoanTask_CompletionEmails" class="ColorGrid" style="margin-top: 3px; width: 500px; ">
            
            <asp:GridView ID="gridCompletetionEmails" runat="server" OnRowDataBound="gridCompletetionEmails_RowDataBound" EmptyDataText="There is no Completion Emails." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="20" />
                        <HeaderTemplate><input id="cbAll" type="checkbox" /></HeaderTemplate>
                        <ItemTemplate><input cid="cbMe" type="checkbox" value="<%# Eval("TaskCompletionEmailId")%>" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  >
                        <HeaderTemplate>Email Template</HeaderTemplate>
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlEmailTemplate" DataTextField="Name" DataValueField="TemplEmailId" runat="server" Width="380"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="70" />
                        <ItemTemplate>
                           <a href="javascript:btnPreview_onclick('<%# Eval("TaskCompletionEmailId")%>'); void(0);" cid="<%# Eval("TaskCompletionEmailId")%>" >Preview</a> 
                        </ItemTemplate>    
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>

        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="Btn-66" OnClientClick="return BeforeSend()" onclick="btnSend_Click" />
                    </td>
                    <td style="padding-left: 8px; display:none;">
                        <input id="btnPreview" type="button" value="Preview" class="Btn-66" onclick="btnPreview_onclick()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="window.close();" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hdnUseEWS" runat="server" />
    </div>

    <div id="divWaiting" style="display: none; padding: 5px;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
                </td>
                <td style="padding-left: 5px;">
                    <label id="WaitingMsg" style="color: #818892; font-weight: bold;">
                    </label>
                    &nbsp;&nbsp; <a id="aClose" href="javascript:RefreshPage()" style="font-weight: bold;
                        color: #6182c1;">[Close]</a>
                </td>
            </tr>
        </table>
    </div>

    </form>
</div>
</body>
</html>
