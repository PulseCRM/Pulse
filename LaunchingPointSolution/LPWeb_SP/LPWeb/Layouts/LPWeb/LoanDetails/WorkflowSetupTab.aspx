<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowSetupTab.aspx.cs" Inherits="LoanDetails_WorkflowSetupTab" EnableEventValidation="false"%>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">

        function BeforeApplyWflTemp() {

            var result = confirm("Do you want to apply the selected Workflow Template now and regenerate the workflow for the loan?");
            if (result == false) {

                return false;
            }
            
            return true;
        }

        function TriggerEvent(objId) {
            $("#" + objId).trigger("click");
        }

        function CloseDialog_AddTask(){
        
            // close modal
            $("#divAddTask").dialog("close");
        }
        function CloseDialog_EditTask() {

            // close modal
            $("#divEditTask").dialog("close");
        }
        function DialogClose() {
            $("#ifrLoanTaskAdd").dialog('destroy');
        }

        function btnCreateTask_onclick() {

            var FileId = $("#<%= hfdFileId.ClientID %>").val();
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/TaskCreate.aspx?sid=" + sid + "&LoanID=" + FileId + "&CloseDialogCodes=window.parent.RefreshPage()";

            var BaseWidth = 610;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 400;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            //window.parent.parent.parent.ShowGlobalPopup("Loan Task Details", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
            ShowPopup("Loan Task Details", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function btnEditTask_onclick() {

            var SelectedCount = $("#gvWfGrid tr td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one task can be selected.");
                return;
            }

            var FileId = $("#<%= hfdFileId.ClientID %>").val();
            var TaskID = $("#gvWfGrid tr td :checkbox:checked").attr("myTaskID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "../LoanDetails/TaskEdit.aspx?sid=" + sid + "&LoanID=" + FileId + "&TaskID=" + TaskID + "&CloseDialogCodes=window.parent.RefreshPage()";

            var BaseWidth = 610;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 400;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowPopup("Loan Task Details", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }


        function LoanTaskDelete() {
            //            if (obj.disabled)
            //                return;


            var SelectedCount = $("#gvWfGrid tr td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No task has been selected.");
                return;
            }

            var Result = confirm("This operation will not be reversible. Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Deleting selected task(s)...");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            var TaskIDs = GetSelectedTaskIDs();
            var LoginUserID = $("#hdnLoginUserID").val();
            var LoanID = $("#<%= hfdFileId.ClientID %>").val();

            // Ajax
            $.getJSON("LoanTaskDelete_Background.aspx?sid=" + Radom + "&TaskIDs=" + encodeURIComponent(TaskIDs) + "&LoginUserID=" + LoginUserID + "&LoanID=" + LoanID, AfterTaskDelete);

        }

        function GetSelectedTaskIDs() {

            var TaskIDs = "";
            $("#gvWfGrid tr td :checkbox:checked").each(function (i) {

                var TaskID = $(this).attr("myTaskID");
                if (i == 0) {

                    TaskIDs = TaskID;
                }
                else {

                    TaskIDs += "," + TaskID;
                }
            });

            return TaskIDs;
        }

        function AfterTaskDelete(data) {

            if (data.ExecResult == "Failed") {
                $.unblockUI();
                alert(data.ErrorMsg);
                return;
            }

            setTimeout("CloseWaitingDialog('Delete selected task(s) successfully.')", 2000);
        }

        function CloseWaitingDialog(SuccessMsg) {

            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(SuccessMsg);
            $('#aClose').show();
        }

        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gvWfGrid.ClientID %> :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        function ShowWaitingDialog(WaitingMsg) {

            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();
            $.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function ShowPopup(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divPopup").attr("title", Title);

            $("#ifrPopup").attr("src", iFrameSrc);
            $("#ifrPopup").width(iFrameWidth);
            $("#ifrPopup").height(iFrameHeight);

            // show modal
            $("#divPopup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) {
                    $("#divPopup").dialog("destroy");
                    $("#ifrPopup").attr("src", "");
                }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function ClosePopup() {
            

        }

        function RefreshPage() {
            var rBaseParent = window.parent;

            if (window.parent.document.location.href.indexOf("LeadSetupTab.aspx") != -1) {
                rBaseParent = window.parent.parent;
            }

            if ($.browser.msie == true) {   // ie
                rBaseParent.document.frames("ifrLoanInfo").location.reload();
            }
            else {   // firefox

                rBaseParent.document.getElementById('ifrLoanInfo').contentWindow.location.reload();
            }

            // refresh current page
            window.location.href = window.location.href;
        }
    </script>
</head>
<body style="width: 100%">
    <form id="form1" runat="server">
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 750px;">
                <tr>
                    <td colspan="2">
                        Workflow Template: <asp:Label runat="server" ID="lblWflTemplApplied" /> &nbsp;&nbsp; Last Applied On:<asp:Label runat="server" ID="lblWflAppliedDate" />
                        &nbsp;&nbsp;&nbsp;&nbsp;Applied By: &nbsp;&nbsp;<asp:Label runat="server" ID="lblWflAppliedBy" />
                    </td>
                </tr> 
                <tr><td>&nbsp;</td></td></tr>
                <tr>
                    <td colspan="2">
                        Workflow Template:&nbsp;&nbsp;<asp:DropDownList ID="ddlWfTemps" runat="server"></asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnApplyWfl" runat="server" Text="Apply Workflow" CssClass="Btn-140" OnClientClick="return BeforeApplyWflTemp()" OnClick="btnApplyWfl_Click" />
                    </td>
                </tr>
                <tr><td>&nbsp;</td></td>
                <tr>
                    <td style="width: 400px;">

                    <div id="div1">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="btnNew" runat="server" href="javascript:btnCreateTask_onclick();">Add Custom Task</a><span>|</span></li>
                                <li><a id="btnUpdate" runat="server" href="javascript:btnEditTask_onclick();">Update Custom Task</a><span>|</span></li>
                                <li><a id="btnRemoveWfl" runat="server" href="javascript:LoanTaskDelete();">Delete Custom Task</a></li>
                            </ul>
                        </div>
                    </td>
                    <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                            FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                            ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                            LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divGrid" class="ColorGrid" style="width: 98%; margin-top: 5px;">
        <asp:GridView ID="gvWfGrid" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
            OnSorting="gvWfGrid_Sorting" CellPadding="3" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <input type="checkbox" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("LoanTaskId") %>' myTaskID="<%# Eval("LoanTaskId")%>"/>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Custom Task" SortExpression="Name" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stage" SortExpression="StageName" HeaderStyle-Width="100px">
                    <ItemTemplate>
                        <%# Eval("StageName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Due Date" SortExpression="Due" ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false" ItemStyle-Width="60px">
                    <ItemTemplate>
                        <%# Eval("Due", "{0:d}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Owner" SortExpression="OwnerName" HeaderStyle-Width="120px"
                    ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("OwnerName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prerequisite Task" SortExpression="PreName" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("PreName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Days Due After Prereq" SortExpression="DaysDueAfterPrerequisite" ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false" HeaderStyle-Width="120px">
                    <ItemTemplate>
                        <%# Eval("DaysDueAfterPrerequisite")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Days From Est Close" SortExpression="DaysDueFromEstClose" ItemStyle-HorizontalAlign="Right"
                    ItemStyle-Wrap="false" HeaderStyle-Width="120px">
                    <ItemTemplate>
                        <%# Eval("DaysDueFromEstClose")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">&nbsp;</div>
    </div>
    
    <div id="divWaiting" style="display: none; padding: 5px;">
	    <table style="margin-left: auto; margin-right: auto;">
		    <tr>
			    <td>
				    <img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
			    </td>
			    <td style="padding-left: 5px;">
				    <label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>&nbsp;&nbsp;
				    <a id="aClose" href="javascript:window.location.href=window.location.href" style="font-weight: bold; color: #6182c1;">[Close]</a>
			    </td>
		    </tr>
	    </table>
	</div>
    <asp:HiddenField ID="hfdFileId" runat="server" />
    <input id="hdnLoginUserID" runat="server" type="text" style="display: none;" />
    </form>

    <div id="divPopup" title="Popup" style="display: none;">
        <iframe id="ifrPopup" frameborder="0" scrolling="no" width="400px" height="300px">
        </iframe>
    </div>
</body>
</html>
