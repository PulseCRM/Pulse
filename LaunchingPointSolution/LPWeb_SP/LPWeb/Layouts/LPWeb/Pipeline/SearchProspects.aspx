<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchProspects.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Pipeline.SearchProspects" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.tinysort.min.js" type="text/javascript"></script>
    <title>Search Prospects</title>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
       
        function CheckBoxClicked(me, ckAllID, hiAllIDs, hiSelectedIDs, id) {
            var sAllIDs = $('#' + hiAllIDs).val();
            var sSelectedIDs = $('#' + hiSelectedIDs).val();
            var allIDs = new Array();
            var selectedIDs = new Array();
            if (sAllIDs.length > 0)
                allIDs = sAllIDs.split(',');

            if (sSelectedIDs.length > 0)
                selectedIDs = sSelectedIDs.split(',');

            if ($(me).attr('checked'))
                selectedIDs.push(id);
            else
                selectedIDs.remove(id);

            // set the CheckAll check box checked status
            $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);
            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');
        }
    </script>
    <script type="text/javascript">
        (function ($) {
            $.fn.outerHTML = function (s) {
                return (s)
			? this.before(s).remove()
			: $('<p>').append(this.eq(0).clone()).html();
            }
        })(jQuery);

        $(document).ready(function () {

           
        });

        function DoSearch() {

            var loanOfficer =$("#" + '<%=ddlLoanOfficer.ClientID %>').val();
            var refCode = $("#" + '<%=tbRefCode.ClientID %>').val();
            var status = $("#" + '<%=ddlStatus.ClientID %>').val();
            var leadSource =$("#" + '<%=tbLeadSource.ClientID %>').val();
            var lastName =$("#" + '<%=tbLastName.ClientID %>').val();
            var address = $("#" + '<%=tbAddress.ClientID %>').val();
            var city = $("#" + '<%=tbCity.ClientID %>').val();
            var state = $("#" + '<%=ddlState.ClientID %>').val();
            var zip = $("#" + '<%=tbZip.ClientID %>').val();


            window.parent.GetSearchHiddenValue(loanOfficer, refCode, status, leadSource, lastName, address, city, state, zip);
            window.parent.closeSearchPop();
        }

       
    </script>
    
    <script type="text/javascript">
        function initEmailTpltSetupWin() {
            $('#dialogEmailTplt').dialog({
                modal: false,
                autoOpen: false,
                title: 'Email Template Setup',
                width: 835,
                height: 750,
                resizable: false,
                close: clearEmailTpltSetupWin
            });
        }

        function showEmailTpltSetupWin(mode, id) {
//            var f = document.getElementById('ifrEdit');
//            if (null == mode || "" == mode)
//                mode = "0";
//            if (null == id)
//                id = "";
//            f.src = "EmailTemplateEdit.aspx?mode=" + mode + "&EmailTemplateID=" + id + "&t=" + Math.random().toString();
            //            $('#dialogEmailTplt').dialog('open');

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "../Settings/EmailTemplateEditParent.aspx?sid=" + sid + "&EmailTemplateID=" + EmailTemplateID;
        }

        function clearEmailTpltSetupWin() {
            var f = document.getElementById('ifrEdit');
            f.src = "about:blank";
        }

        function CloseDialog_AddRule() {
            $("#dialogRule").dialog("close");
        }
        function CloseDialog_EditRule() {
            $("#dialogRule").dialog("close");
        }
        function CloseDialog_EditEmailTemplate() {
            $("#dialogEmailTplt").dialog("close");
        }
        function clearBtnClicked() {
            $("#" + '<%=ddlLoanOfficer.ClientID %>').val("0");
            $("#" + '<%=tbRefCode.ClientID %>').val("");
            $("#" + '<%=ddlStatus.ClientID %>').val("");
            $("#" + '<%=tbLeadSource.ClientID %>').val("");
            $("#" + '<%=tbLastName.ClientID %>').val("");
            $("#" + '<%=tbAddress.ClientID %>').val("");
            $("#" + '<%=tbCity.ClientID %>').val("");
            $("#" + '<%=ddlState.ClientID %>').val("");
            $("#" + '<%=tbZip.ClientID %>').val("");
        }
    </script>
    <script type="text/javascript">

        function closeBox(isRefresh, bReset) {
            if (bReset === false)
                bReset = false;
            else
                bReset = true;
            self.parent.closeSetupWin(isRefresh, bReset);
            return false;
        }
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="aspnetForm">
        <div class="Heading">
            Search Client</div>
         <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-left: 15px;">
                            <asp:Button ID="btnSearch" runat="server" Text="Search" class="Btn-66" OnClientClick="DoSearch()"/>
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnClear" runat="server" Text="Clear" class="Btn-66" OnClientClick="clearBtnClicked()" />
                        </td>
                    </tr>
                </table>
            </div>
        <div class="DetailsContainer">
            <div>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="white-space: nowrap;">
                            Loan Officer:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlLoanOfficer" runat="server"  Width="100px"></asp:DropDownList>
                        </td>
                        <td style="white-space: nowrap;">
                           Reference Code:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:TextBox ID="tbRefCode" runat="server" class="iTextBox" Style="width: 100px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                         <td style="white-space: nowrap;">
                            &nbsp;&nbsp;Status:
                        </td>
                        <td style="padding-left: 15px;" >
                            <asp:DropDownList ID="ddlStatus" runat="server"  Width="110px">
                                <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Active" Value="Active" ></asp:ListItem>
                                <asp:ListItem Text="Bad" Value="Bad"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                <asp:ListItem Text="Converted" Value="Converted"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                            Lead Source:
                        </td>
                        <td style="padding-left: 15px;" colspan="3">
                            <asp:TextBox ID="tbLeadSource" runat="server" class="iTextBox" Style="width: 360px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="white-space: nowrap;">
                           &nbsp;&nbsp;Last Name:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:TextBox ID="tbLastName" runat="server" class="iTextBox" Style="width: 100px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style=" white-space: nowrap;">
                            Address:
                        </td>
                        <td style="padding-left: 15px; padding-top: 9px;" colspan="5">
                            <asp:TextBox ID="tbAddress" runat="server"  Width="360px"  class="iTextBox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="white-space: nowrap;">
                           City:
                        </td>
                        <td style="padding-left: 15px;" >
                            <asp:TextBox ID="tbCity" runat="server" class="iTextBox" Style="width: 152px;"></asp:TextBox>
                        </td>
                        <td style="white-space: nowrap;">
                            &nbsp;&nbsp;State:
                        </td>
                         <td style="padding-left: 15px;">
                           <asp:DropDownList ID="ddlState" runat="server" Width="110px" >
                                <asp:ListItem Text="-Select-" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                                <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                                <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                                <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                                <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                                <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                                <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                                <asp:ListItem Text="DC" Value="DC"></asp:ListItem>
                                <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                                <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                                <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                                <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                                <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                                <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                                <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                                <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                                <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                                <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                                <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                                <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                                <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                                <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                                <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                                <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                                <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                                <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                                <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                                <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                                <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                                <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                                <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                                <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                                <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                                <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                                <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                                <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                                <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                                <asp:ListItem Text="PR" Value="PR"></asp:ListItem>
                                <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                                <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                                <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                                <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                                <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                                <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                                <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                                <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                                <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                                <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                                <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="white-space: nowrap;">
                             &nbsp;&nbsp;Zip:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:TextBox ID="tbZip" runat="server" class="iTextBox" Style="width: 100px;"
                                MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hiReferenced" runat="server" Value="0" />
                <asp:HiddenField ID="hiCloned" runat="server" Value="0" />
                 <asp:HiddenField ID="hiAllIds" runat="server" />
                <asp:HiddenField ID="hiCheckedIds" runat="server" />
                <asp:HiddenField ID="hiCreatedId" runat="server" />
                <asp:HiddenField ID="hiCurrentData" runat="server" />
            </div>
           
           
            <div class="DashedBorder" style="margin-top: 15px;">
                &nbsp;</div>
           
        </div>
    </div>
    <div style="display: none;">
        <div id="dialog1" title="Rule Selection">
            <iframe id="iframeRS" name="iframeRS" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="dialogRule" title="Rule Setup">
            <iframe id="ifrRuleEdit" name="ifrRuleEdit" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="dialogEmailTplt" title="Rule Setup">
            <iframe id="ifrEdit" name="ifrEdit" frameborder="0" width="100%" height="100%"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
