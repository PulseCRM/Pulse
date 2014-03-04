<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectMailChimpListPopup.aspx.cs" Inherits="Prospect_SelectMailChimpListPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Select MailChimp List Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function btnSelect_onclick() {

            var LID = $.trim($("#txtLID").val());
            //alert("LID: " + LID);

            var GetIDsFunction = GetQueryString1("GetIDsFunction");

            var InvokeStr = GetIDsFunction + "('" + LID + "')";
            //alert(InvokeStr);

            btnCancel_onclick();

            eval(InvokeStr);
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            //alert(CloseDialogCodes);

            eval(CloseDialogCodes);
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input id="txtLID" type="text" value="132ADCBA-89C2-4AD6-920F-31B8B3C25972" />
        <input id="btnSelect" type="button" value="Select" onclick="return btnSelect_onclick()" />
        <input id="btnCancel" type="button" value="Cancel" onclick="return btnCancel_onclick()" />
    </div>
    </form>
</body>
</html>
