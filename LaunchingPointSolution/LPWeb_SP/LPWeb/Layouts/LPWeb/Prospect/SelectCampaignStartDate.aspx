<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectCampaignStartDate.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Prospect.SelectCampaignStartDate" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var campaignDate = $("#tbxStartDate");
            campaignDate.datepick();
        });
        function returnSelectedDate() {
            var sReturn = $("#tbxStartDate").val();
            if (sReturn == "") {
                alert("Please enter start date.");
                return false;
            }
            // close popup window
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            if ($.browser.msie == true) {
                eval(CloseDialogCodes);
            }
            // return value
            var sRecieveDataCodes = GetQueryString1("RecieveDataCodes");
            sRecieveDataCodes = sRecieveDataCodes.replace("returnValue", sReturn);
            eval(sRecieveDataCodes);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <div style="text-align: center;">
            Campaign start date:&nbsp;&nbsp;<input type="text" id="tbxStartDate" cssclass="DateField"
                width="70px" />
            <br />
            <br />
            <input id="btnOK" type="button" value="Yes" class="Btn-66" onclick="returnSelectedDate()" />&nbsp;&nbsp;
        </div>
    </form>
</body>
</html>