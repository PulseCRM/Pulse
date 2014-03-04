<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSendPopup.aspx.cs" Inherits="Contact_EmailSendPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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

  
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>

  
   
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
            var ProspectID = GetQueryString1("ContactID");


            if (UseEmailTemplate == true) {

                var EmailTemplateID = $("#ddlEmailTemplateList").val();

                if (ProspectID != "") {
                    OpenPreview("../LoanDetails/EmailPreview.aspx?UseEmailTemplate=1&EmailTemplateID=" + EmailTemplateID + "&TextBody=&ProspectID=" + ProspectID + "&AppendMyPic=0"); //, "_SendMail5", 760, 580, "no", "center"
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

                if (ProspectID != "") {
                    OpenPreview("../LoanDetails/EmailPreview.aspx?UseEmailTemplate=0&EmailTemplateID=0&TextBody=" + encodeURIComponent(TextBody_Encode) + "&ProspectID=" + ProspectID + "&AppendMyPic=" + AppendMyPic); //, "_SendMail5", 760, 580, "no", "center"
                }
            }

            return false;
        }

        function OpenPreview(src) {

            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrPreview").attr("src", src);
            $("#divPreview").dialog({
                height: 450,
                width: 550,
                title: "Preview Email",
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
//            $("body>div[role=dialog]").appendTo("#aspnetForm");
            return false;
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
        <table style="margin-top: 10px;">
            <tr>
                <td style="width: 80px;">Email Template:</td>
                <td>
                    <asp:DropDownList ID="ddlEmailTemplateList" runat="server" DataValueField="TemplEmailId" DataTextField="Name" Width="300px">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
        <table style="margin-top: 10px;">
            <tr>
                <td style="width: 80px;">Subject</td>
                <td>
                    <asp:TextBox ID="txtSubject" runat="server" Width="500px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">Body</td>
                <td style="padding-top: 5px;">
                    <asp:TextBox ID="txtBody" runat="server" TextMode="MultiLine" Height="200px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table style="margin-top: 10px;">
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
        <div id="divRecipientList" class="ColorGrid" style="margin-top: 8px;">
            <div>
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
        <div style="margin-top: 10px;">
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
        <div id="divPreview" title="Preview Email" style="display: none;">
        <iframe id="ifrPreview" frameborder="0" scrolling="no" width="100%" height="100%">
        </iframe>
    </div>
    </form>
</body>
</html>