<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanMarketingEventContent.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.LoanDetails.LoanMarketingEventContent"  %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>

  
</head>
<body>
    <form id="form1" runat="server">
    <center >
    <div id="divContainer" style="border: solid 0px green; padding-left: 10px;">

        <table style="width:570px; height:500px" >
<%--             <tr>
                <td  width="100%"  style="padding-left: 10px;">
                    <b><asp:Label ID="lbTitle" runat="server" ></asp:Label></b>
                </td>
            </tr>--%>
            <tr style="padding-top:20px;">
                <td  width="100%" style="height:100%" align="left" valign="top">
                    <asp:Label ID="lbContent" runat="server" ></asp:Label>
                    <%--<iframe runat="server" id="ifmEmail" visible="false" height="100%" width="100%"></iframe>
                    <asp:Image ID="imgMailing" runat="server"  Width="360px" Height="360px" Visible="false" />--%>
                </td>
            </tr>
        </table>
     </div>  
     </center> 
    </form>
</body>
</html>