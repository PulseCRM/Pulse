<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisposeWorkflowTemplateList.aspx.cs" Inherits="Prospect_DisposeWorkflowTemplateList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            // set scroll bar
            if ($.browser.msie == true && $.browser.version == 7.0 && $("#divGridContainer").get(0).scrollHeight > $("#divGridContainer").height()) {

                $("#gridWorkflowTemplateList").width(582);
            }
        });

        function CheckOne(CheckBox) {

            var IsChecked = CheckBox.checked;

            $("#gridWorkflowTemplateList tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }
        }

        function BeforeSumbit() {

            if ($("#gridWorkflowTemplateList tr td :checkbox:checked").length == 0) {

                alert("Please select a Workflow Template.");
                return false;
            }

            var WorkflowTemplateID = $("#gridWorkflowTemplateList tr td :checkbox:checked").eq(0).attr("myWorkflowTemplateID");
            //alert(WorkflowTemplateID);

            $("#hdnTargetWorkflowTemplateID").val(WorkflowTemplateID);

            return true;
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
    <div id="Container" style="width: 601px; height: 600px; border: solid 0px red;">
        
        <div style="padding-top: 5px;">
            <asp:Button ID="btnApply" runat="server" Text="Apply" CssClass="Btn-66" OnClientClick="return BeforeSumbit();" onclick="btnApply_Click" />&nbsp;
            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="Cancel()" />
        </div>
        
        <div id="divGridContainer" style="height: 550px; overflow: auto; margin-top: 5px;">
            <div id="divWorkflowTemplateList" class="ColorGrid">
                <asp:GridView ID="gridWorkflowTemplateList" runat="server" EmptyDataText="There is no point folder for selection." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" onclick="CheckOne(this)" myWorkflowTemplateID="<%# Eval("WflTemplId") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Workflow Template" />
                        <asp:BoundField DataField="Desc" HeaderText="Desc." ItemStyle-Width="330px" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        
    </div>
    <asp:HiddenField ID="hdnTargetWorkflowTemplateID" runat="server" />
    </form>
</body>
</html>
