<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailDetailPopup.aspx.cs" Inherits="Prospect_EmailDetailPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Email Detail Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/featuredcontentglider2.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>   
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/featuredcontentglider2.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        jQuery(document).ready(function () {

            featuredcontentglider.init({
                gliderid: "divEmailLogs", //ID of main glider container
                contentclass: "glidecontent", //Shared CSS class name of each glider content
                togglerid: "divPrevNextBtn", //ID of toggler container
                remotecontent: "", //Get gliding contents from external file on server? "filename" or "" to disable
                selected: '<%=nCurrentEmailLogIndex %>', //Default selected content index (0=1st)
                persiststate: false, //Remember last content shown within browser session (true/false)?
                speed: 1000, //Glide animation duration (in milliseconds)
                direction: "leftright", //set direction of glide: "updown", "downup", "leftright", or "rightleft"
                autorotate: false, //Auto rotate contents (true/false)?
                autorotateconfig: [3000, 2] //if auto rotate enabled, set [milliseconds_btw_rotations, cycles_before_stopping]
            });

            jQuery(".glidecontent").height(720);
        });

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
                title: Title,
                modal: true,
                resizable: false,
                close: function (event, ui) {
                    $("#divGlobalPopup").dialog("destroy");
                    $("#ifrGlobalPopup").attr("src", "about:blank");
                }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseGlobalPopup() {

            $("#divGlobalPopup").dialog("close");            
            window.parent.CloseGlobalPopup();

        }

        function CloseGlobalPopupRefresh() {

            $("#divGlobalPopup").dialog("close");
            window.parent.CloseGlobalPopup();
            window.parent.location.href = window.parent.location.href;           
        }

        function btnReply_onclick(EmailLogID, Action) {

        //    alert(EmailLogID);

        //    window.parent.CloseGlobalPopup();

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Prospect/EmailReply.aspx?sid=" + RadomStr + "&EmailLogID=" + EmailLogID + "&Action=" + Action + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";
          
            var BaseWidth = 640
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 700;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;
            // Lon :  The following line should changed to window.ShowGlobalPopup("Reply Message", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc); 
             window.parent.ShowGlobalPopup("Reply Message", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            //ShowGlobalPopup("Reply Message", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

         }


         function ShowEmailLogAttachmentsPopup(EmailLogID) {

             var RadomNum = Math.random();
             var RadomStr = RadomNum.toString().substr(2);
             var iFrameSrc = "../Prospect/EmailLogAttachmentsPopup.aspx?sid=" + RadomStr + "&EmailLogID=" + EmailLogID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

             var BaseWidth = 340
             var iFrameWidth = BaseWidth + 2;
             var divWidth = iFrameWidth + 25;

             var BaseHeight = 400;
             var iFrameHeight = BaseHeight + 2;
             var divHeight = iFrameHeight + 40;
             window.parent.ShowGlobalPopup("Email Log Attachments Popup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

         }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 550px; border: solid 0px blue;">
        
        

        <div id="divPrevNextBtn" class="glidecontenttoggler">
            
            <table>
                <tr>
                    <td style="width: 100px;"><h4 style="margin: 0px; color: #5880b3">Email Detail</h4></td>
                    <td>
                        <a href="#" class="prev"><img alt="" src="../images/ico_16_l.gif" /></a> 
                    </td>
                    <td>
                        <a href="#" class="next"><img alt="" src="../images/ico_16_r.gif" /></a>
                    </td>
                </tr>
            </table>
        </div>
        
        

        <div id="divEmailLogs" class="glidecontentwrapper" style="border: solid 0px green; height: 723px;">

            <asp:Repeater ID="rptEmailLogList" runat="server">
                <ItemTemplate>
                    <div class="glidecontent" style="width: 548px; border: solid 0px red; overflow: auto;">

                        <div style="margin-top: 10px;">
                            <table>
                                <tr>
                                    <td style="width: 250px;">From User: <%# this.GetFromUserFullName(Eval("FromUser").ToString())%></td>
                                    <td>From Email: <%# this.Server.HtmlEncode(Eval("FromEmail").ToString()) %></td>
                                </tr>
                            </table>
                            <table style="margin-top: 3px;">
                                <tr>
                                    <td>Subject: <%# Eval("Subject")%></td>
                                </tr>
                            </table>
                            <table style="margin-top: 3px;">
                                <tr>
                                    <td style="width: 250px;">Sent: <%# Convert.ToDateTime(Eval("LastSent").ToString()).ToString("dd/MM/yyyy HH:mm") %></td>
                                    <td>Status: <%# this.GetEmailStatus(Eval("Success").ToString()) %></td>
                                </tr>
                            </table>
                            <table style="margin-top: 3px; <%# Eval("Success").ToString() == "False" ? "" : "display: none;" %>">
                                <tr>
                                    <td>Error detail:</td>
                                    <td>
                                        <textarea id="txtErrorDetail" cols="20" rows="2" style="height: 30px;"><%# Eval("Error")%></textarea>
                                    </td>
                                </tr>
                            </table>
                            <table style="margin-top: 3px;">
                                <tr>
                                    <td colspan="2">Attachments:</td>
                                </tr>
                                    <%=this.GetEmailLogAttachmentsList()%>
                            </table>
                            <div style=" display:none;"> <iframe id="ifrdownload" name="ifrdownload" src="about:blank"></iframe> </div>
                        </div>

                        <table style="margin-top: 15px;">
                            <tr>
                                <td>
                                    <input id="btnReplyAll" type="button" value="Reply All" class="Btn-91" onclick="btnReply_onclick('<%# Eval("EmailLogId")%>','ReplyAll')" />
                                </td>
                               <td>
                                    <input id="btnReply" type="button" value="Reply" class="Btn-66" onclick="btnReply_onclick('<%# Eval("EmailLogId")%>','Reply')" />
                               </td>
                            </tr>
                        </table>
        
                        <div style="margin-top: 20px;">
                            <div>To:</div>
                            <div id="divToList" class="ColorGrid" style="margin-top: 5px;">
                                <div>
                                    <table id="gridToList" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                                        
                                        <%# this.GetToListRows(Eval("ToUser").ToString(), Eval("ToContact").ToString(), Eval("ToEmail").ToString())%>
                                    </table>
                                </div>
                                <div class="GridPaddingBottom">&nbsp;</div>
                            </div>
                        </div>

                        <div style="margin-top: 10px;">
                            <div>CC:</div>
                            <div id="divCCList" class="ColorGrid" style="margin-top: 5px;">
                                <div>
                                    <table id="gridCCList" class="GrayGrid" cellspacing="0" cellpadding="4" style="border-collapse: collapse;">
                                        <%# this.GetToListRows(Eval("CCUser").ToString(), Eval("CCContact").ToString(), Eval("CCEmail").ToString())%>
                                    </table>
                                </div>
                                <div class="GridPaddingBottom">&nbsp;</div>
                            </div>
                        </div>

                        <div style="height: 300px; margin-top: 20px; padding: 10px; overflow: auto; border: solid 1px #e4e7ef;">
                            <%# Eval("EmailBody").ToString() == string.Empty ? string.Empty : Encoding.UTF8.GetString((byte[])Eval("EmailBody")) %>
                        </div>

                    </div>
                </ItemTemplate>
            </asp:Repeater>
            
        </div>

    </div>
    <div id="divGlobalPopup" title="Global Popup" style="display: none;">
        <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="400px" height="300px">
        </iframe>
    </div>
    </form>
</body>
</html>