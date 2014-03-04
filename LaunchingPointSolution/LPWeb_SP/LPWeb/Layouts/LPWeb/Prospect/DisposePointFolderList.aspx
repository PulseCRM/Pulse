<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DisposePointFolderList.aspx.cs" Inherits="Prospect_DisposePointFolderList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <base target="_self" />
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

                $("#gridPointFolderList").width(582);
            }
        });

        function CheckOne(CheckBox) {

            var IsChecked = CheckBox.checked;

            $("#gridPointFolderList tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }
        }

        function BeforeSumbit() {

            if ($("#gridPointFolderList tr td :checkbox:checked").length == 0) {

                alert("Please select a Point Folder.");
                return false;
            }

            var PointFolderID = $("#gridPointFolderList tr td :checkbox:checked").eq(0).attr("myFolderID");
            //alert(PointFolderID);

            $("#hdnTargetPointFolderID").val(PointFolderID);

            return true;
        }

        function Cancel() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
            
        }

        function ShowDialog_WorkflowTemplateSelection() {

            var LoanID = 335;
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Prospect/DisposeWorkflowTemplateList.aspx?sid=" + RadomStr + "&LoanID=" + LoanID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 601
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.CloseGlobalPopup();
            window.parent.ShowGlobalPopup("Workflow Template Selection", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowSubmitLoanPopup() {

            var LoanID = "<%=iLoanID %>";
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Prospect/SubmitLoanPopup.aspx?sid=" + RadomStr + "&loanID=" + LoanID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth =450
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 420;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;
            if ($.browser.msie == true) {
                window.parent.CloseGlobalPopup();
            }
            window.parent.ShowGlobalPopup("Submit Loan Popup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            return false;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="Container" style="width: 601px; height: 600px; border: solid 0px red;">
        
        <div style="padding-top: 5px;">
            <asp:Button ID="btnYes" runat="server" Text="Yes" CssClass="Btn-66" OnClientClick="return BeforeSumbit();" onclick="btnYes_Click" />&nbsp;
            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="Cancel()" />
        </div>
        
        <div id="divGridContainer" style="height: 550px; overflow: auto; margin-top: 5px; border-bottom: solid 1px #e4e7ef;">
            <div id="divPointFolderList" class="ColorGrid">
                <asp:GridView ID="gridPointFolderList" runat="server" EmptyDataText="There is no point folder for selection." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" onclick="CheckOne(this)" myFolderID="<%# Eval("FolderID") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Folder Name" />
                        <asp:BoundField DataField="Path" HeaderText="Path" ItemStyle-Width="310px" />
                        <asp:BoundField DataField="StatusName" HeaderText="Loan Status" ItemStyle-Width="80px" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        
    </div>
    <asp:HiddenField ID="hdnTargetPointFolderID" runat="server" />
    </form>
    <!-- 20110901 -->
</body>
</html>