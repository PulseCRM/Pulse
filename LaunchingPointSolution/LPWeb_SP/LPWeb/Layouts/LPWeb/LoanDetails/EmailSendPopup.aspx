<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSendPopup.aspx.cs" Inherits="LoanDetails_EmailSendPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Send Email</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/jquery.cleditor.js" type="text/javascript"></script>

    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
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

        var HtmlEditor;

        $(document).ready(function () {

            InitHtmlEditor();
            EnableSubjectBody();

            // add event
            $("#chkUserEmailTemplate").click(EnableSubjectBody);
        });

        // init cleditor
        function InitHtmlEditor() {

            if (HtmlEditor == undefined && $("#txtBody").is(":visible") == true) {

                HtmlEditor = $("#txtBody").cleditor({
                    width: 500,
                    height: 200,
                    bodyStyle: "font:11px Arial",
                    docType: '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">'
                });
            }
        }

        function EnableSubjectBody() {

            if (HtmlEditor == undefined) {

                return;
            }
            
            var UserEmailTemplate = $("#chkUserEmailTemplate").attr("checked");
            if (UserEmailTemplate == true) {

                $("#chkAppendMyPic").attr("checked", "");
                $("#chkAppendMyPic").attr("disabled", "true");

                $("#ddlEmailTemplateList").attr("disabled", "");

                $("#txtSubject").val("");
                $("#txtSubject").attr("disabled", "true");

                HtmlEditor[0].clear();

                HtmlEditor[0].disable(true);

            }
            else {

                $("#chkAppendMyPic").attr("disabled", "");

                $("#ddlEmailTemplateList").attr("disabled", "true");
                $("#txtSubject").attr("disabled", "");

                HtmlEditor[0].disable(false);
            }
        }

        function btnAdd_onclick() {

            var SelUserID = $("#ddlToList").val();
            var SelFullName = $("#ddlToList_FullName option[value='" + SelUserID + "']").text();
            var SelEmail = $("#ddlToList_Email option[value='" + SelUserID + "']").text();

            if (SelEmail == "") {

                alert("The email address of selected user/contact is empty, so it's ignored.");
                return;
            }

            if ($("#gridRecipientList tr").length == 1) {

                // clear tr
                $("#gridRecipientList").empty();

                // add th
                $("#gridRecipientList").append("<tr><th scope='col'>Name</th><th scope='col'>Email</th></tr>");
            }

            // add tr
            $("#gridRecipientList").append("<tr><td style='width: 200px;'>" + SelFullName + "</td><td><span class='EmailAddress' ToID='" + SelUserID + "'>" + SelEmail + "</span></td></tr>");
        }

        function btnPreview_onclick() {

            var UseEmailTemplate = $("#chkUserEmailTemplate").attr("checked");

            var LoanID = GetQueryString1("LoanID");
            var ProspectID = GetQueryString1("ProspectID");
            var ProspectAlertID = GetQueryString1("ProspectAlertID");

            if (UseEmailTemplate == true) {

                var EmailTemplateID = $("#ddlEmailTemplateList").val();

                if (LoanID != "") {

                    OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&LoanID=" + LoanID + "&AppendMyPic=0", "_SendMail5", 760, 580, "no", "center");
                }
                else if (ProspectID != "") {

                    OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&ProspectID=" + ProspectID + "&AppendMyPic=0", "_SendMail5", 760, 580, "no", "center");
                }
                else if (ProspectAlertID != "") {

                    OpenWindow("EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&ProspectAlertID=" + ProspectAlertID + "&AppendMyPic=0", "_SendMail5", 760, 580, "no", "center");
                }
            }
            else {

                var TextBody = $.trim($("#txtBody").val());
                if (TextBody == "") {

                    alert("Please enter email body.");
                    return false;
                }
                var TextBody_Encode = $.base64Encode(TextBody);
                var AppendMyPic = $("#chkAppendMyPic").attr("checked") == true ? "1" : "0";

                if (LoanID != "") {

                    OpenWindow("EmailPreview.aspx?UseEmailTemplate=0&EmailTemplateID=0&TextBody=" + encodeURIComponent(TextBody_Encode) + "&LoanID=" + LoanID + "&AppendMyPic=" + AppendMyPic, "_SendMail5", 760, 580, "no", "center");
                }
                else if (ProspectID != "") {

                    OpenWindow("EmailPreview.aspx?UseEmailTemplate=0&EmailTemplateID=0&TextBody=" + encodeURIComponent(TextBody_Encode) + "&ProspectID=" + ProspectID + "&AppendMyPic=" + AppendMyPic, "_SendMail5", 760, 580, "no", "center");
                }
                else if (ProspectAlertID != "") {

                    OpenWindow("EmailPreview.aspx?UseEmailTemplate=0&EmailTemplateID=0&TextBody=" + encodeURIComponent(TextBody_Encode) + "&ProspectAlertID=" + ProspectAlertID + "&AppendMyPic=" + AppendMyPic, "_SendMail5", 760, 580, "no", "center");
                }
            }

            return true;
        }

        function BeforeSend() {

            var UseEmailTemplate = $("#chkUserEmailTemplate").attr("checked");
            if (UseEmailTemplate == false) {

                var Subject = $.trim($("#txtSubject").val());
                if (Subject == "") {

                    alert("Please enter email subject.");
                    return false;
                }

                var TextBody = $.trim($("#txtBody").val());
                if (TextBody == "") {

                    alert("Please enter email body.");
                    return false;
                }
            }

            if ($("#gridRecipientList span[class='EmailAddress']").length == 0) {

                alert("Please add email recipient(s).");
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

            var ToIDs = "";
            $("#gridRecipientList span[class='EmailAddress']").each(function () {

                var ToID = $(this).attr("ToID");
                if (ToIDs == "") {

                    ToIDs = ToID;
                }
                else {

                    ToIDs += "$" + ToID;
                }
            });

            $("#hdnToIDs").val(ToIDs);

            return true;
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            //alert(CloseDialogCodes);

            if (CloseDialogCodes != "") {

                eval(CloseDialogCodes);
            }
            else {

                window.parent.CloseDialog_SendEmail();
            }
        }



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

        function AddAttachments() {

            var TemplEmailId = $("#ddlEmailTemplateList").val();
            var RadomNum = Math.random();

            ShowGlobalPopup("Add Email Attachments", 500, 300, 530, 350, "AddEmailAttachmentsPopup.aspx?r=" + RadomNum + "&TemplEmailId=" + TemplEmailId + "&Token=" + token + "&CloseDialogCodes=window.parent.CloseGlobalPopup();");


        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 600px;">
        <table>
            <tr>
                <td style="width: 150px;">
                    <asp:CheckBox ID="chkUserEmailTemplate" runat="server" Text=" Use Email Template" Checked="true" />
                </td>
                <td style="width: 90px;">
                    <asp:CheckBox ID="chkCCMe" runat="server" Text=" CC me" />
                </td>
                <td>
                    <asp:CheckBox ID="chkAppendMyPic" runat="server" Text=" Append my picture and signature at the bottom" />
                </td>
            </tr>
        </table>
        <table style="margin-top: 5px;">
            <tr>
                <td style="width: 80px;">Email Template:</td>
                <td>
                    <asp:DropDownList ID="ddlEmailTemplateList" runat="server" DataValueField="TemplEmailId" DataTextField="Name" Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table style="margin-top: 5px;">
            <tr>
                <td style="width: 80px;">Subject</td>
                <td>
                    <asp:TextBox ID="txtSubject" runat="server" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">Body</td>
                <td style="padding-top: 5px;">
                    <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Height="190px"></asp:TextBox>
                </td>
            </tr>
        </table>

        <script>
            $(function () {
                
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
                    var TemplEmailId = $("#ddlEmailTemplateList").val();
                    var RadomNum = Math.random();

                    $.get("EmailAttachmentsList.aspx?op=Remove&TemplEmailId=" + TemplEmailId + "&Token=" + token + "&r=" + RadomNum + "&AttachId=" + idList, function (data) {

                        if (data == "1") {
                            bindAttachmentsList();
                        }
                    });


                });


                $("#hidToken").val(token); //init

            });

            var token = Math.random(); //"token_test_123"; //
            
            function bindAttachmentsList() {
                var RadomNum = Math.random();
                var TemplEmailId = $("#ddlEmailTemplateList").val();

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


        <table style="margin-top: 0px;">
            <tr>
                <td style="width: 80px;">To:</td>
                <td>
                    <asp:DropDownList ID="ddlToList" runat="server" Width="300px" DataValueField="UserId" DataTextField="RoleAndFullName">
                    </asp:DropDownList>
                    <div style="display: none;">
                        <asp:DropDownList ID="ddlToList_FullName" runat="server" Width="200px" DataValueField="UserId" DataTextField="FullName">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlToList_Email" runat="server" Width="200px" DataValueField="UserId" DataTextField="Email">
                        </asp:DropDownList>
                    </div>
                </td>
                <td>
                    <input id="btnAdd" type="button" value="Add" class="Btn-66" onclick="btnAdd_onclick()" />
                </td>
            </tr>
        </table>
        <div id="divRecipientList" class="ColorGrid" style="margin-top: 2px;">
            <div style=" height:60px; overflow:auto;">
                <table class="GrayGrid" cellspacing="0" cellpadding="3" id="gridRecipientList" style="border-collapse: collapse;">
                    <tr class="EmptyDataRow" align="center">
                        <td colspan="2">
                            Please select email and add to recipient list.
                        </td>
                    </tr>
                </table>
            </div>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <div style="margin-top: 2px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSend" runat="server" Text="Send" CssClass="Btn-66" OnClientClick="return BeforeSend();" onclick="btnSend_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnPreview" type="button" value="Preview" class="Btn-66" onclick="btnPreview_onclick()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divHiddenFields" style="display: none;">
            <asp:HiddenField ID="hdnToIDs" runat="server" />
            <asp:HiddenField ID="hdnUseEWS" runat="server" />
        </div>
    </div>


        <div id="divGlobalPopup" title="Global Popup" style="display: none;">
        <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="100px" height="100px">
        </iframe>
        </div>
    </div>
    </form>
</body>
</html>