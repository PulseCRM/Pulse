<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="Home" Codebehind="Home.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home - Launching Point</title>
    <link href="css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="css/style.custom.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div id="PageContainer">
        <div id="Header">

            <div id="WelcomeMsg">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>Welcome <a>[John Smith]</a></td>
                        <td style="width: 50px;"><a>My Site</a></td>
                        <td style="width: 13px; text-align: center;">|</td>
                        <td><a>My Links</a></td>
                    </tr>
                </table>
            </div>

            <div id="LogoContainer">
                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="width: 695px;"><img src="images/navigator/logo.jpg" alt="logo" /></td>
                        <td style="width: 200px;"><input id="txtSearchContent" type="text" value="Search" /></td>
                        <td><img src="images/Magnifier.gif" alt="magnifier" /></td>
                    </tr>
                </table>
            </div>

            <div id="NavContainer">
                <table id="tNavTab" cellpadding="0" cellspacing="0" style="width: 100%;" border="0">
                    <tr>
                        <td style="width: 4px; height: 41px; background: url('images/navigator/NavTab-Left.jpg') no-repeat left top;">&nbsp;</td>
                        <td style="height: 41px; background: url('images/navigator/NavTab-Mid.gif') repeat left top;">
                            <table id="tNavTabItem" cellpadding="0" cellspacing="0" border="0" style="">
                                <tr>
                                    <td style="width: 10px;">&nbsp;</td>
                                    <td class="SelectedTab" style="width: 61px; padding-left: 11px;"><a>Home</a></td>
                                    <td style="width: 105px;"><a>My Documents</a></td>
                                    <td style="width: 130px;"><a>Shared Documents</a></td>
                                    <td style="width: 87px;"><a>My Pileline</a></td>
                                    <td style="width: 87px;"><a>Email Inbox</a></td>
                                    <td style="width: 92px;"><a>My Calendar</a></td>
                                    <td style="width: 133px;"><a>Company Calendar</a></td>
                                    <td><a>Rates</a></td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 4px; height: 41px; background: url('images/navigator/NavTab-Right.gif') no-repeat left top;">&nbsp;</td>
                    </tr>
                </table>
            </div>

            <div id="SiteMapContainer">
                <div style="float: left;">MOSS > Site/library Title</div>
                <div style="text-align: right;">Welcome to the ABC Mortgage Company Portal!</div>
            </div>
           
        </div>
        
        <div id="PageContent">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 360px; vertical-align: top;">
                        <div id="divEmailInbox" class="Widget" style="width: 350px;">
                            <div class="Widget_Header">
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="wTitle">Email Inbox</td>
                                            <td><a href="http://www.sina.com" target="_blank"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="Widget_Body" style="padding-top: 7px;">
                    
                                <div class="DashedRow25_Odd" style="padding-left: 9px; padding-right: 9px;">
                                    <table class="tEmailInboxRow" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 27px;">
                                                <img src="images/Email-New.gif" />
                                            </td>
                                            <td class="TextEllipsis" style="width: 98px; font-weight: bold;"><a>John Doe</a></td>
                                            <td><a class="aEmailSubject TextEllipsis" href="http://www.sina.com">Please call today so we can discuss aaa bbbbb…</a></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="DashedRow25_Odd" style="padding-left: 9px; padding-right: 9px;">
                                    <table class="tEmailInboxRow" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 27px;">
                                                <img src="images/Email-Read.gif" />
                                            </td>
                                            <td class="TextEllipsis" style="width: 98px;">Suzie Smith</td>
                                            <td><a class="aEmailSubject TextEllipsis" href="http://www.sina.com">Jones Loan Docs</a></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="DashedRow25_Odd" style="padding-left: 9px; padding-right: 9px;">
                                    <table class="tEmailInboxRow" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 27px;">
                                                <a><img src="images/Email-Read.gif" /></a>
                                            </td>
                                            <td class="TextEllipsis" style="width: 98px;">Fred Jones</td>
                                            <td><a class="aEmailSubject TextEllipsis" href="http://www.sina.com">Where are we on my closing?</a></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="DashedRow25_Odd" style="padding-left: 9px; padding-right: 9px;">
                                    <table class="tEmailInboxRow" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 27px;">
                                                <img src="images/Email-Read.gif" />
                                            </td>
                                            <td class="TextEllipsis" style="width: 98px;">Magan Casper</td>
                                            <td><a class="aEmailSubject TextEllipsis" href="http://www.sina.com">I'm trying to determine what loan...</a></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="DashedRow25_Odd" style="padding-left: 9px; padding-right: 9px;">
                                    <table class="tEmailInboxRow" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 27px;">
                                                <img src="images/Email-Read.gif" />
                                            </td>
                                            <td class="TextEllipsis" style="width: 98px;">Frank Hardy</td>
                                            <td><a class="aEmailSubject TextEllipsis" href="http://www.sina.com">Thanks for lunch today</a></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="DashedRow25_Odd" style="padding-left: 9px; padding-right: 9px;">
                                    <table class="tEmailInboxRow" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width: 27px;">
                                                <img src="images/Email-Read.gif" />
                                            </td>
                                            <td class="TextEllipsis" style="width: 98px;">Jeff Johnson</td>
                                            <td><a class="aEmailSubject TextEllipsis" href="http://www.sina.com">Please call today so we can discuss...</a></td>
                                        </tr>
                                    </table>
                                </div>
                    
                            </div>
                        </div>
                        
                        <div id="divCalendar" class="Widget" style="width: 350px; margin-top: 10px;">
                            <div class="Widget_Header">
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="wTitle">Calendar Summary</td>
                                            <td><a href="http://www.sohu.com" target="_blank"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                        </tr>
                                    </table>
                                </div>
                            </div>

                            <div class="Widget_Body">
                                
                                <div class="DashedRow25_Odd" style="padding-left: 15px; padding-right: 15px; text-align: center; font-weight: bold;">
                                    3-31-2010
                                </div>

                                <div class="DashedRow25_Even" style="padding-left: 15px; padding-right: 15px;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td style="width: 74px;">9:00 am</td>
                                            <td><a class="TextEllipsis" style="width: 250px">Meet with John Doe asdfasdf adfsadfas afdsadfasdf adsfasdf</a></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="DashedRow25_Odd" style="padding-left: 15px; padding-right: 15px;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td style="width: 74px;">10:00 am</td>
                                            <td><a class="TextEllipsis" style="width: 250px">Review closing docs with Suzie</a></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="DashedRow25_Even" style="padding-left: 15px; padding-right: 15px;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td style="width: 74px;">9:00 am</td>
                                            <td><a class="TextEllipsis" style="width: 250px">Meet with John Doe asdfasdf adfsadfas afdsadfasdf adsfasdf</a></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="DashedRow25_Odd" style="padding-left: 15px; padding-right: 15px;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td style="width: 74px;">10:00 am</td>
                                            <td><a class="TextEllipsis" style="width: 250px">Review closing docs with Suzie</a></td>
                                        </tr>
                                    </table>
                                </div>

                                <div class="DashedRow25_Even" style="padding-left: 15px; padding-right: 15px;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td style="width: 74px;">9:00 am</td>
                                            <td><a class="TextEllipsis" style="width: 250px">Meet with John Doe asdfasdf adfsadfas afdsadfasdf adsfasdf</a></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="DashedRow25_Odd" style="padding-left: 15px; padding-right: 15px;">
                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                        <tr>
                                            <td style="width: 74px;">10:00 am</td>
                                            <td><a class="TextEllipsis" style="width: 250px">Review closing docs with Suzie</a></td>
                                        </tr>
                                    </table>
                                </div>

                            </div>
                        </div>

                    </td>
                    <td style="vertical-align: top;">
                        <div id="divPipeline" class="Widget" style="width: 560px;">
                            <div class="Widget_Header">
                                <div>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="wTitle">Pipeline Summary</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                            <div class="Widget_Body" style="padding-top: 12px;">
                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 235px; vertical-align: top; padding-left: 15px;">
                                            <div style="color: #9e4666; border-bottom: solid 1px #c79896; width: 213px;">Quarterly Revenue Goal ($1,000)</div>
                                            <div style="margin-top: 5px;">
                                                <img src="images/Dashboard.gif" />
                                            </div>
                                            
                                        </td>
                                        <td style="vertical-align: top; padding-right: 9px;">
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 105px;">
                                                        <select id="Select1" style="width: 80px;">
                                                            <option></option>
                                                        </select>
                                                    </td>
                                                    <td style="width: 105px;">
                                                        <select id="Select2" style="width: 80px;">
                                                            <option></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <select id="Select3" style="width: 80px;">
                                                            <option></option>
                                                        </select>
                                                    </td>
                                                </tr>
                                            </table>
                                            
                                            <table cellpadding="0" cellspacing="0" style="margin-top: 14px;">
                                                <tr>
                                                    <td style="width: 105px;">
                                                        <select id="Select4" style="width: 80px;">
                                                            <option></option>
                                                        </select>
                                                    </td>
                                                    <td style="width: 105px;">
                                                        <select id="Select5" style="width: 80px;">
                                                            <option></option>
                                                        </select>
                                                    </td>
                                                    <td>
                                                        <select id="Select6" style="width: 80px;">
                                                            <option></option>
                                                        </select>
                                                    </td>
                                                </tr>
                                            </table>
                                            

                                            <div style="margin-top: 8px;">
                                                <table cellpadding="0" cellspacing="0" style="width: 100%; border: solid 1px #f7f8fa;">
                                                    <tr style="background-color: #f7f8fa; height: 22px;">
                                                        <td style="width: 35px; padding-left: 3px;">
                                                            <input id="Checkbox1" type="checkbox" />
                                                        </td>
                                                        <td style="width: 90px;">Opened</td>
                                                        <td>$315,000.00</td>
                                                    </tr>
                                                    <tr style="height: 22px;">
                                                        <td style="width: 35px; padding-left: 3px;">
                                                            <input id="Checkbox2" type="checkbox" />
                                                        </td>
                                                        <td style="width: 90px;">Opened</td>
                                                        <td>$315,000.00</td>
                                                    </tr>

                                                    <tr style="background-color: #f7f8fa; height: 22px;">
                                                        <td style="width: 35px; padding-left: 3px;">
                                                            <input id="Checkbox3" type="checkbox" />
                                                        </td>
                                                        <td style="width: 90px;">Opened</td>
                                                        <td>$315,000.00</td>
                                                    </tr>
                                                    <tr style="height: 22px;">
                                                        <td style="width: 35px; padding-left: 3px;">
                                                            <input id="Checkbox4" type="checkbox" />
                                                        </td>
                                                        <td style="width: 90px;">Opened</td>
                                                        <td>$315,000.00</td>
                                                    </tr>

                                                    <tr style="background-color: #f7f8fa; height: 22px;">
                                                        <td style="width: 35px; padding-left: 3px;">
                                                            <input id="Checkbox5" type="checkbox" />
                                                        </td>
                                                        <td style="width: 90px;">Opened</td>
                                                        <td>$315,000.00</td>
                                                    </tr>
                                                    <tr style="height: 22px;">
                                                        <td style="width: 35px; padding-left: 3px;">
                                                            <input id="Checkbox6" type="checkbox" />
                                                        </td>
                                                        <td style="width: 90px;">Opened</td>
                                                        <td>$315,000.00</td>
                                                    </tr>

                                                    
                                                    
                                                </table>
                                                <div style="border: solid 1px #f7f8fa; border-top: none;">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                                        <tr style="height: 22px;">
                                                            <td style="width: 35px; padding-left: 3px;">&nbsp;</td>
                                                            <td style="width: 90px; font-weight: bold;">Total:</td>
                                                            <td style="font-weight: bold;">$1,972,000.00</td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                        <table cellpadding="0" cellspacing="0" style="margin-top: 9px;">
                            <tr>
                                <td style="width: 285px;">
                                    <div id="divCompanyAnn" class="Widget" style="width: 275px;">
                                        <div class="Widget_Header">
                                            <div>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="wTitle">Company  Announcements</td>
                                                        <td><a href="http://www.sina.com" target="_blank"><img src="images/Widget-More.gif" alt="more" /></a></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="Widget_Body" style="padding-top: 15px; padding-left: 15px; height: 96px;">
                                            
                                            <ul style="list-style: none; padding: 0px; line-height: 24px;">
                                                <li class="TextEllipsis" style="width: 240px;">Bageis in the conference room aaaaaaaaaaaaaaaaaaaaaaaaaaaaa</li>
                                                <li class="TextEllipsis" style="width: 240px;">Company mandatory meeting tomorrow at Eddie V's</li>
                                                <li class="TextEllipsis" style="width: 240px;">Branch manager lunch Friday Flemmings</li>
                                                
                                            </ul>
                                            
                                            
                                        </div>
                                    </div>
                                </td>
                                <td style="vertical-align: top;">
                                    <div id="divAlerts" class="Widget" style="width: 275px;">
                                        <div class="Widget_Header_Orange">
                                            <div>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td class="wTitle">Alerts</td>
                                                        <td><a href="http://www.sina.com" target="_blank"><img src="images/Widget-More-Orange.gif" alt="more" /></a></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="Widget_Body" style="padding-top: 3px; height: 96px; padding-left: 13px; padding-right: 8px;">
                                            <table cellpadding="1" cellspacing="0">
                                                <tr>
                                                    <td style="width: 97px;">John Doe</td>
                                                    <td style="color: #b4bccb;">Please explain the plan to get my rate approved</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 97px;">Suzie Johnston</td>
                                                    <td style="color: #b4bccb;">Please explain the plan to get my rate approved</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 97px;">John Doe</td>
                                                    <td style="color: #b4bccb;">Please explain the plan to get my rate approved</td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="Footer">
            
        </div>
    </div>
    </form>
</body>
</html>
