<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSendCompletionPopup.aspx.cs" Inherits="LoanDetails_EmailSendCompletionPopup" %>

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

    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#gridRecipientList tr td:contains('No emaill address found for the selected recipient(s).')").css("color", "red");

            $("#cbAll").change(function () {

                if ($(this).attr("checked") == null || $(this).attr("checked")==false) {
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

        function btnAttachments_onclick(id) {

//            var EmailTemplateID = $("select[cid='ddlEmailTemplate_" + id + "']").val();   //GetQueryString1("EmailTemplateID");
//            if (EmailTemplateID == "0") {

//                alert("Please select a email template.");
//                return false;
//            }
//            var LoanID = GetQueryString1("LoanID");

            
  
            return false;
        }

        function BeforeSend() {

           
            /*
            var Sender = $("#lbSender").text();
            if (Sender == "There is no sender.") {

                alert("Could not find the From email address.");
                return false;
            }

            if ($("#gridRecipientList tr").length == 1) {

                alert("No emaill address found for the selected recipient(s).");
                return false;
            }

            if ($("#gridRecipientList tr td:contains('There is no emaill address')").length == $("#gridRecipientList tr").length - 1) {

                alert("No emaill address found for the selected recipient(s).");
                return false;
            }

            

            var ToEmails = "";
            $("#gridRecipientList tr").each(function () {

                var Email = $(this).text();
                if (ToEmails == "") {

                    ToEmails = Email;
                }
                else {

                    ToEmails += "$" + Email;
                }
            });

            $("#txtToEmails").val(ToEmails);

            */
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

            //alert(EmailTemplateID);
            //alert(checkedList.size());
            //return false;

            

            var url_bg = "EmailSendConmpletion_Background.aspx?sid=" + Radom + "&LoanID=" + LoanID + "&TaskID=" + TaskID + "&EmailTemplateID=" + EmailTemplateID + "&Token=" + token;
            ///$.getJSON(url, sendok);
            //"EmailSendConmpletion_Background.aspx?sid=893256634244153&LoanID=294&TaskID=3487&EmailTemplateID=28,36",
            $.ajax({
                type: "GET",
                url: url_bg,
                processData: false,
                data: "{}",
                cache: false,
                dataType:"json",
                success: sendok,

                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    $('#bodydiv').unblock();
                    alert(textStatus);

                }
            });


//                function (data) {

//                    if (data.ExecResult == "Failed") {
//                        $('#divContainer').unblock();
//                        alert(data.ErrorMsg);
//                        CloseWaitingDialog("Error!");
//                        return;
//                    }

//                    //alert("Sent completion email successfully.");

//                    window.parent.CloseDialog_SendCompletionEmail();
//                }
//            );


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

            CloseThisDialog();
        }

        function CloseThisDialog() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
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


        function ShowGlobalPopup(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divGlobalPopup").attr("title", Title);

            $("#ifrGlobalPopup").attr("src", "");
            $("#ifrGlobalPopup").attr("src", iFrameSrc);
            $("#ifrGlobalPopup").width(iFrameWidth);
            $("#ifrGlobalPopup").height(iFrameHeight);

            // show modal
            $("#divGlobalPopup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) {
                    $("#divGlobalPopup").dialog("destroy");
                    $("#ifrGlobalPopup").attr("src", "about:blank");
                }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
        }

        function CloseGlobalPopup() {

            $("#divGlobalPopup").dialog("close");
            bindAttachmentsList();
        }

        function AddAttachments(templEmailId,isListTemplEmailAttach) {

             var TemplEmailId =templEmailId==null?<%=iEmailTemplateID %>:templEmailId; //$("#ddlEmailTemplateList").val();
            var RadomNum = Math.random();
            if(isListTemplEmailAttach == null || isListTemplEmailAttach == undefined)
            {
                isListTemplEmailAttach = false;
            }
            ShowGlobalPopup("Add Email Attachments", 500, 300, 530, 350, "AddEmailAttachmentsPopup.aspx?r=" + RadomNum + "&Token=" + token +"&isListTemplEmailAttach=" + isListTemplEmailAttach + "&TemplEmailId=" + TemplEmailId + "&CloseDialogCodes=window.parent.CloseGlobalPopup();");

        }

// ]]>
    </script>
</head>
<body>
<div id="bodydiv" style=" margin:0px; height:405px;" >
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 600px;">
    <div id="OldInfo_notUse" style=" display:none;">
        <table cellpadding="0" cellspacing="0">
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
        
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                    <input id="btnSend1" value="Send" class="Btn-66" type="button" onclick="BeforeSend();" />
                        <asp:Button ID="Button2" runat="server" Text="Send" CssClass="Btn-66" Visible="false" OnClientClick="return BeforeSend()" onclick="btnSend_Click" />
                    </td>
                    <td style="padding-left: 8px; display:none;">
                        <input id="btnPreview1" type="button" value="Preview" class="Btn-66"/> <%--onclick="btnPreview_onclick()" --%>
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel1" type="button" value="Cancel" class="Btn-66" onclick="CloseThisDialog()" />
                    </td>
                </tr>
            </table>
        </div>
        


        <script>
            $(function () {

                $("#divAttachmentsList").css("height", "150px");

                bindAttachmentsList();

                $("#ddlEmailTemplateList").click(function () { bindAttachmentsList() });

                $("#AddAttachments").live("click", function () {

                    //alert("AddAttachments");
                    AddAttachments();
                });


                $("#RemoveAttachments").live("click", function () {

                    if ($("input[cid='attachments']:checked").size() == 0) {

                        alert("Please select a Attachments first.");
                        return;
                    }
                    var idList = "";
                    $("input[cid='attachments']:checked").each(function (i, n) {

                        if (i == 0) {
                            idList += $(n).attr("attachId");

                        } else {
                            idList += "," + $(n).attr("attachId");
                        }

                    });
                    var TemplEmailId = <%=iEmailTemplateID %>; // $("#ddlEmailTemplateList").val();
                    var RadomNum = Math.random();
                    $.get("EmailAttachmentsList.aspx?op=Remove&TemplEmailId=" + TemplEmailId + "&Token=" + token + "&r=" + RadomNum + "&AttachId=" + idList, function (data) {

                        if (data == "1") {
                            bindAttachmentsList();
                        }
                    });

                     $("#hidToken").val(token); //init
                });

            });
            var token = Math.random(); //"token_test_123"; //

            function bindAttachmentsList() {
                var RadomNum = Math.random();
                var TemplEmailId = <%=iEmailTemplateID %>; // $("#ddlEmailTemplateList").val();

                $.get("EmailAttachmentsList.aspx?TemplEmailId=" + TemplEmailId + "&Token=" + token + "&r=" + RadomNum, function (data) {

                    //alert($(data).find("#AttachmentsTable").html());

                    $("#AttachmentsSection").html(
                        $(data).find("#AttachmentsTable").html()
                    );
                });

            }

        </script>
        <asp:HiddenField ID="hidToken" runat="server" />
        <div id="AttachmentsSection">
        <!--ajax Get from EmailAttachmentsList.aspx  -->
            

        </div>


        <%--<div id="divEmailAttachments" class="ColorGrid" style="margin-top: 3px; width: 500px;">
            
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width:80px;">Attachments:</td>
                    <td style="padding-left: 8px; width="400px;">
                        <ul class="ToolStrip">
                            <li>
                                <asp:LinkButton ID="btnAddAtt" runat="server" Text="Add Attachments" OnClick="btnAddAtt_Click"></asp:LinkButton><span>|</span></li>
                            <li>
                                <asp:LinkButton ID="btnRemoveAtt" runat="server" Text="Remove Attachments" OnClick="btnRemoveAtt_Click"></asp:LinkButton></li>
                        </ul>
                    </td>
                    <td style="padding-left: 8px;">
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gridAttachment" runat="server"  EmptyDataText="There is no Email attachments." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="20" />
                        <ItemTemplate><input cid="cbMeAtt" type="checkbox" value="<%# Eval("EmailLogId ")%>" /></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField  >
                        <ItemTemplate>
                        <a href="javascript:btnAttachments_onclick('<%# Eval("EmailLogId")%>'); void(0);" cid="<%# Eval("EmailLogId")%>" ><%# Eval("Name")%></a> 
                        </ItemTemplate>
                    </asp:TemplateField>                   
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>--%>

        <div id="divLoanTask_CompletionEmails" class="ColorGrid" style="margin-top: 3px; width: 570px;">
            
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
                            <asp:DropDownList ID="ddlEmailTemplate" DataTextField="Name" DataValueField="TemplEmailId" runat="server" Width="300"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="100" />
                        <ItemTemplate>
                           <a href="javascript:btnPreview_onclick('<%# Eval("TaskCompletionEmailId")%>'); void(0);" cid="<%# Eval("TaskCompletionEmailId")%>" >Preview</a> 
                        </ItemTemplate>    
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Width="200" />
                        <ItemTemplate>
                           <a href="javascript:AddAttachments('<%# Eval("TemplEmailId")%>',true); void(0);" cid="<%# Eval("TaskCompletionEmailId")%>" >Attachments</a> 
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
                    <input id="btnSend2" value="Send" class="Btn-66" type="button" onclick="BeforeSend();" />
                        <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="Btn-66" Visible="false" OnClientClick="return BeforeSend()" onclick="btnSend_Click" />
                    </td>
                    <td style="padding-left: 8px; display:none;">
                        <input id="btnPreview" type="button" value="Preview" class="Btn-66"/> <%--onclick="btnPreview_onclick()" --%>
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="CloseThisDialog()" />
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

    <div id="divGlobalPopup" title="Global Popup" style="display: none;">
        <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="100px" height="100px">
        </iframe>
    </div>

</div>
</body>
</html>
