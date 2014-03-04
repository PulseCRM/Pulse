<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailDetailPopup.aspx.cs" Inherits="Contact_EmailDetailPopup"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Email Detail Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/featuredcontentglider2.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
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
                selected: 0, //Default selected content index (0=1st)
                persiststate: false, //Remember last content shown within browser session (true/false)?
                speed: 1000, //Glide animation duration (in milliseconds)
                direction: "leftright", //set direction of glide: "updown", "downup", "leftright", or "rightleft"
                autorotate: false, //Auto rotate contents (true/false)?
                autorotateconfig: [3000, 2] //if auto rotate enabled, set [milliseconds_btw_rotations, cycles_before_stopping]
            });

            jQuery(".glidecontent").height(720);
        })
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
                        </div>
        
                        <div style="margin-top: 10px;">
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
                            <%# Encoding.UTF8.GetString((byte[])Eval("EmailBody")) %>
                        </div>

                    </div>
                </ItemTemplate>
            </asp:Repeater>
            
        </div>

    </div>
    </form>
</body>
</html>