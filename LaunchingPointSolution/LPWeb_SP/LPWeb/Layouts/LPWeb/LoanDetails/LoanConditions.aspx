<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanConditions.aspx.cs" Inherits="LoanConditions" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.treeview.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />


    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <script type="text/javascript" src="../js/jqcontextmenu.js"></script>
    <script type="text/javascript">
        var gridId = "#<%=gridList.ClientID %>";
        $(document).ready(function () {
            var checkAll = $(gridId + " :checkbox:eq(0)");
            checkAll.click(function () {
                var allStatus = checkAll.attr("checked");
                $(gridId + " :checkbox:gt(0)").each(function () {
                    $(this).attr("checked", allStatus);
                });
                //getSelectedItems();

            });

        });
    </script>

    <script language="javascript" type="text/javascript">
// <![CDATA[

        function ShowAddNotePopup() {

            var checked_count = $(gridId + " tr:not(:first) td :checkbox:checked").length;
            //alert(checked_count);
            if (checked_count == 0) {

                alert("Please select a condition first.");
                return;
            }
            else if (checked_count > 1) {

                alert("Please select only one condition.");
                return;
            }

            var ConditionID = $(gridId + " tr:not(:first) td :checkbox:checked").parent().attr("title");
            //alert(ConditionID);
            var FileId = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "LoanConditionAddNote.aspx?sid=" + sid + "&FileId=" + FileId + "&ConditionID=" + ConditionID;

            var BaseWidth = 730;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 320;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.top.ShowGlobalPopup("Add Condition Note", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowNoteDetailPopup(ConditionID) {

            //alert(ConditionID);
            var FileId = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var iFrameSrc = "LoanConditionNoteDetail.aspx?sid=" + sid + "&FileId=" + FileId + "&ConditionID=" + ConditionID;

            var BaseWidth = 730;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 400;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.top.ShowGlobalPopup("Condition Note Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function CheckPointFile() {
            var checked_count = $(gridId + " tr:not(:first) td :checkbox:checked").length;

            if (checked_count == 0) {

                alert("Please select the condition(s) first.");
                return;
            }
            //show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");
            var FileId = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            //Check Pint File Status
            $.getJSON("../LoanDetails/CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + FileId, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();
                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {
                    if (data.ErrorMsg != "") {
                        //If the response indicates that the file is locked, Close the AJAX waiting popup and display the response’s hdr.StatusInfo
                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {
                        MarkReceivedClick();
                    }
                }
            });

        }


        function MarkReceivedClick() {

            //show waiting
            window.parent.parent.parent.ShowWaitingDialog3("Changing the selected conditions status...");
            var selctedItems = new Array();

            $(gridId + "  tr:not(:first) td :checkbox:checked").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.parent().attr("title"));
                }
            });
            var ConditionID = selctedItems.join(",");
            //var ConditionID = $(gridId + " tr:not(:first) td :checkbox:checked").parent().attr("title");
            var FileId = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("../LoanDetails/LoanConditionMarkAsReceivedAjax.aspx?sid=" + sid + "&FileId=" + FileId + "&ConditionId=" + ConditionID, function (data) {

                window.parent.parent.parent.CloseWaitingDialog3();
                if (data.ExecResult == "Failed") {
                    alert(data.ErrorMsg);
                    return false;
                }
                else {
                    if (data.ErrorMsg != "") {
                        //If the response indicates an error or failure, close the AJAX waiting popup, display the error;   
                        alert(data.ErrorMsg);
                        return false;
                    }
                    else {
                        //Refresh Condition Tab
                        window.location.href = window.location.href;
//                        window.parent.parent.parent.idivIndex = 1;
//                        window.parent.parent.parent.RefreshConditionsTab();
                        return;
                    }
                }
            });
       }

// ]]>
    </script>

</head>
<body>
    <form  id="form1" runat="server">
    <div>
    <h4>Conditions List</h4>

    <div >
        <asp:DropDownList ID="ddlAllStatuses" runat="server">
        <asp:ListItem Text="All Statuses" Value=""></asp:ListItem>
        <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
        <asp:ListItem Text="Received" Value="Received"></asp:ListItem>
        <asp:ListItem Text="Submitted" Value="Submitted"></asp:ListItem>
        <asp:ListItem Text="Cleared" Value="Cleared"></asp:ListItem>
        </asp:DropDownList>

        <asp:DropDownList ID="ddlType" runat="server">
        </asp:DropDownList>

        <asp:DropDownList ID="ddlAllDueDates" runat="server">
        <asp:ListItem Text="All Due Dates" Value="0"></asp:ListItem>
        <asp:ListItem Text="Overdue" Value="1"></asp:ListItem>
        <asp:ListItem Text="Overdue + Due Today" Value="2"></asp:ListItem>
        <asp:ListItem Text="Due Today" Value="3"></asp:ListItem>
        <asp:ListItem Text="Due Tomorrow" Value="4"></asp:ListItem>
        <asp:ListItem Text="Due in 7 days" Value="5"></asp:ListItem>
        </asp:DropDownList> 

        <asp:Button ID="btnFilter" runat="server" class="Btn-66" Text="Filter" OnClick="btnFilter_OnClick" />

        <br /><br />
        <div style="width:980px" >
            <div style=" width:500px; float:left; text-align:left;" >
                <asp:LinkButton ID="linkEnableExV" runat="server" OnClick="linkEnableExV_OnClick">Enable External Viewing </asp:LinkButton>&nbsp;&nbsp; | &nbsp;&nbsp; <asp:LinkButton ID="linkDisableExV" OnClick="linkDisableExV_OnClick" runat="server"> Disable External Viewing</asp:LinkButton>
                &nbsp;&nbsp; | &nbsp;&nbsp; <a href="javascript:ShowAddNotePopup()">Add Note</a>&nbsp;&nbsp; | &nbsp;&nbsp; <a id="btnMarkReceived" runat="server" href="javascript:CheckPointFile();">Mark as Received</a>
            </div>
            <div style=" width:500px; float:right; text-align:right;">
                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                    OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                    FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                    ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                    LayoutType="Table">
                </webdiyer:AspNetPager>
            </div>
        </div>
        <div style=" clear:both;" ></div>

        <div style="height:485px; overflow: auto;">
        <div id="divDivision" class="ColorGrid" style="width: 980px; margin-top: 5px;" >
            <asp:GridView ID="gridList" runat="server" DataKeyNames="FileId"
                EmptyDataText="There is no record in database." AutoGenerateColumns="False"  AllowSorting="false" CellPadding="3" CssClass="GrayGrid" GridLines="None" OnRowDataBound="gridList_RowDataBound"><%-- 
                OnPreRender="gridList_PreRender" 
                OnSorting="gridList_Sorting"--%>
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
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Condition" HeaderText="Condition" >
                        <ItemTemplate>
                            <asp:Image ID="imgicon" runat="server" Width="16px" Height="16px" />
                            <asp:Literal ID="litcondName" runat="server"></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="CondType" SortExpression="CondType" HeaderText="Type" />
                    <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                    <asp:BoundField DataField="Cleared" HeaderText="Cleared"  DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                    <asp:BoundField DataField="Submitted" HeaderText="Submitted" DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                    <asp:BoundField DataField="Received" HeaderText="Received"  DataFormatString="{0:MM/dd/yyyy}" ItemStyle-Width="70px" HeaderStyle-Width="70px" />
                    <asp:TemplateField HeaderText="External" ItemStyle-Width="50px" HeaderStyle-Width="50px">
                        <ItemTemplate>
                            <%# (Eval("ExternalViewing") == DBNull.Value || Convert.ToBoolean(Eval("ExternalViewing")) == false) ? "No" : "Yes" %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Note" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <a title="<%# Eval("Note") %>" href="javascript:ShowNoteDetailPopup('<%# Eval("LoanCondId") %>')"><%# Eval("Note").ToString().Substring(0, Eval("Note").ToString().Length > 80 ? 80 : Eval("Note").ToString().Length) + (Eval("Note").ToString().Length > 80 ? "..." : "")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        </div>
        
            <asp:HiddenField ID="hiAllIds" runat="server" />
            <asp:HiddenField ID="hiCheckedIds" runat="server" />
            <asp:HiddenField ID="hiLoanInfo" runat="server" />
            <asp:HiddenField ID="hiSelectedDisposal" runat="server" />
            <asp:HiddenField ID="hiSelectedFolderId" runat="server" />

        </div>
    </div>
    </form>
</body>
</html>