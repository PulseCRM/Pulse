<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleSelection.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Settings.RuleSelection" %>

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
    <title>Rule Selection</title>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        // for grid checkbox
        Array.prototype.remove = function (s) {
            var nIndex = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i] == s) {
                    nIndex = i;
                    break;
                }
            }
            if (nIndex != -1) {
                this.splice(nIndex, 1);
                return true;
            }
            else
                return false;
        }

        function CheckAllClicked(me, areaID, hiAllIDs, hiSelectedIDs, shiAllData) {
            var bCheck = $(me).attr('checked');
            if (bCheck) {
                // copy all ids to selected id holder
                $('#' + hiSelectedIDs).val($('#' + hiAllIDs).val());
                $("#" + '<%=hiCheckedData.ClientID %>').val($('#' + shiAllData).val());
            }
            else {
                $('#' + hiSelectedIDs).val('');
                $("#" + '<%=hiCheckedData.ClientID %>').val("<table />");
            }
            $('input:checkbox', $('#' + areaID) + '.CheckBoxColumn').each(function () { $(this).attr('checked', bCheck); });
        }

        function CheckBoxClicked(me, ckAllID, hiAllIDs, hiSelectedIDs, id, sName, sTpltId, sTpltName) {
            var sAllIDs = $('#' + hiAllIDs).val();
            var sSelectedIDs = $('#' + hiSelectedIDs).val();
            var allIDs = new Array();
            var selectedIDs = new Array();
            if (sAllIDs.length > 0)
                allIDs = sAllIDs.split(',');

            if (sSelectedIDs.length > 0)
                selectedIDs = sSelectedIDs.split(',');

            if ($(me).attr('checked')) {
                selectedIDs.push(id);
                AddRuleItem(id, sName, sTpltId, sTpltName);
            }
            else {
                selectedIDs.remove(id);
                RemoveRuleItem(id);
            }

            // set the CheckAll check box checked status
            // $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);

            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');

            // delegate function
            if (typeof (onRowChecked) == "function")
                onRowChecked();
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

        function AddRuleItem(sId, sName, sAlertEmailTpltId, sAlertEmailTpltName) {
            var sDom = decodeDataXml($("#" + '<%=hiCheckedData.ClientID %>').val());
            if (sDom.length <= 0)
                sDom = "<table />";
            var theDom = $(sDom);
            var item = $("tr[RuleId='" + sId + "']", theDom);
            if (item.length > 0) return;
            var sNewItem = "<tr RuleId='{0}' Name='{1}' AlertEmailTemplId='{2}' AlertEmailTpltName='{3}' />";
            sNewItem = sNewItem.replace("{0}", sId);
            sNewItem = sNewItem.replace("{1}", sName);
            sNewItem = sNewItem.replace("{2}", sAlertEmailTpltId);
            sNewItem = sNewItem.replace("{3}", sAlertEmailTpltName);
            $(sNewItem).appendTo(theDom);
            $("#" + '<%=hiCheckedData.ClientID %>').val(encodeDataXml(theDom.outerHTML()));
        }

        function RemoveRuleItem(sId) {
            var theDom = $(decodeDataXml($("#" + '<%=hiCheckedData.ClientID %>').val()));
            $("tr[RuleId='" + sId + "']", theDom).each(function () { $(this).remove(); });
            $("#" + '<%=hiCheckedData.ClientID %>').val(encodeDataXml(theDom.outerHTML()));
        }

        function encodeDataXml(sXml) {
            return sXml.replace(/</g, "\u0001");
        }
        function decodeDataXml(sXml) {
            return sXml.replace(/\u0001/g, "<");
        }

        // call back
        function returnFn()
        {
            if(window.parent && window.parent.getRuleSelectionReturn)
                window.parent.getRuleSelectionReturn($("#" + '<%=hiCheckedData.ClientID %>').val());
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="div1" class="ColorGrid" style="margin-top: 5px;">
        <asp:GridView ID="gridList" runat="server" DataKeyNames="RuleId,Name,AlertEmailTemplId,AlertEmailTpltName" EmptyDataText="There is no record in database."
            AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound" OnPreRender="gridList_PreRender"
            OnSorting="gridList_Sorting" CellPadding="3" CssClass="GrayGrid" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <asp:CheckBox ID="ckbAll" runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="ckbSelect" runat="server" />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="Rule" />
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
        <asp:HiddenField ID="hiAllIds" runat="server" />
        <asp:HiddenField ID="hiCheckedIds" runat="server" />
        <asp:HiddenField ID="hiCheckedData" runat="server" />
        <asp:HiddenField ID="hiAllData" runat="server" />
    </div>
    </form>
</body>
</html>
