<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectPartnerBranch.aspx.cs" Inherits="Contact_SelectPartnerBranch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Partner Branch</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {


        });

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridPartnerBranchList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridPartnerBranchList tr td :checkbox").attr("checked", "");
            }
        }

        function BeforeSubmit() {

            if ($("#gridPartnerBranchList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("Please select a partner branch.");
                return false;
            }

            var SelectedBranchIDs = "";
            $("#gridPartnerBranchList tr:not(:first) td :checkbox:checked").each(function (i) {

                var BranchID = $(this).attr("BranchID");
                //alert(BranchID);
                if (i == 0) {

                    SelectedBranchIDs = BranchID;
                }
                else {

                    SelectedBranchIDs += "," + BranchID;
                }
            });

            //alert(SelectedBranchIDs);

            $("#hdnSelectedBranchIDs").val(SelectedBranchIDs);

            return true;
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="PopupContainer">
        <div id="divButtons">
            <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="Btn-66" OnClientClick="return BeforeSubmit();" onclick="btnSelect_Click" />&nbsp;
            <asp:Button ID="btnRemove" runat="server" Text="Remove" CssClass="Btn-66" OnClientClick="return BeforeSubmit();" onclick="btnRemove_Click" />&nbsp;
            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
        </div>
        <div id="divGridContainer" style="margin-top: 5px;">
            <div id="divPartnerBranchList" class="ColorGrid">
                <asp:GridView ID="gridPartnerBranchList" runat="server" EmptyDataText="There is no partner branch." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" BranchID="<%# Eval("ContactBranchId") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Branch" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        <asp:HiddenField ID="hdnSelectedBranchIDs" runat="server" />
    </div>
    </form>
</body>
</html>
