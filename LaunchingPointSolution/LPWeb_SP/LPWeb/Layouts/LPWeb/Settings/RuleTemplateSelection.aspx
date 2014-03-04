<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RuleTemplateSelection.aspx.cs" Inherits="Settings_RuleTemplateSelection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rule Selection Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function CheckOne(CheckBox) {

            var IsChecked = CheckBox.checked;

            $("#gridRuleList tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }
        }

        function btnSelect_onclick() {

            if ($("#gridRuleList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("Please select a rule.");
                return;
            }

            var RuleID = $("#gridRuleList tr:not(:first) td :checkbox:checked").attr("myRuleID");
            var RuleName = $("#gridRuleList tr:not(:first) td :checkbox:checked").attr("myRuleName");

            //alert(RuleID);
            //alert(RuleName);

            window.parent.SetRuleIDAndName(RuleID, RuleName);

            Cancel();
        }

        function Cancel() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 400px;">
        <div id="divGridPanel" style="width: 400px; height: 450px; overflow: auto; border-bottom: solid 1px #e4e7ef;">
            <div id="divRuleList" class="ColorGrid" style="margin-top: 5px;">
                <asp:GridView ID="gridRuleList" runat="server" EmptyDataText="There is no rule." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <ItemTemplate>
                                <input id="chkSelected" type="checkbox" myRuleID="<%# Eval("RuleId")%>" myRuleName="<%# LPWeb.Common.Encrypter.Base64Encode(Eval("Name").ToString()) %>" onclick="CheckOne(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Rule" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        <div id="divButtons" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <input id="btnSelect" type="button" value="Select" class="Btn-66" onclick="btnSelect_onclick()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="Cancel()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>