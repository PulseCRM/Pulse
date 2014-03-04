<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Alert List" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master"
    AutoEventWireup="true" Inherits="Pipeline_AlertList" CodeBehind="AlertList.aspx.cs"
    EnableEventValidation="false" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
    <script language="javascript" type="text/javascript">
// <![CDATA[
        var gridId = "#divAlertList";
        function CheckAll(chked) {
            CheckAllByTag("tag", "alertList", chked);
            getSelectedItems();
        }

        function CheckAllByTag(tagName, tagValue, chked) {
            $("input[type=checkbox][" + tagName + "=" + tagValue + "]").attr("checked", chked);
        }

        function GoToLoanDetails(FileID, LoanStatus) {

            var allFileIds = getGridAllItemIds();
            if (LoanStatus == "Processing") {
                window.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=<%= FromURL %>&fieldid=" + FileID + "&fieldids=" + allFileIds;
            } else {
                window.location.href = "../Prospect/ProspectLoanDetails.aspx?FromPage=<%= FromURL %>&FileId=" + FileID + "&FileIds=" + allFileIds;
            }
        }

        $().ready(function () {
            $(gridId + " :checkbox:gt(0)").each(function () {
                $(this).unbind("click").click(function () {
                    //if ($(this).attr("checked") == false) {
                    //    checkAll.attr("checked", false);
                    // }
                    getSelectedItems();
                });
            });

            $("#btnDetails").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }

                var items = $("#<%=hfDeleteItems.ClientID %>").val().split(",");
                if (items.length > 1) {
                    alert("Only one record can be selected for this operation..");
                    return false;
                }
                //                fieldid=2&fieldids=1,2,3,4,5,6

                var selctedItems = new Array();
                $(gridId + " :checkbox:gt(0)").each(function () {
                    var item = $(this);
                    selctedItems.push(item.attr("fileId"));
                });

                GoToLoanDetails($("#<%=hfDeleteItems.ClientID %>").val()); //
                /*
                window.location.href = '../LoanDetails/LoanDetails.aspx?FromPage=< FromURL %>&fieldid=' + $("#<=hfDeleteItems.ClientID %>").val() + "&fieldids=" + selctedItems.join(",");
                */
            });

            $(".alert img[src$='Unknown.png']").each(function () {
                $(this).hide();
            });

            $("#<%= btnExport.ClientID %>").click(function () {
                if ($("#<%=hfSelAlertId.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }
                else{
                    return true;
                }
            });
        });


        function getSelectedItems() {
            var selctedItems = new Array();
            var selIDs=new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("fileId"));
                    selIDs.push(item.attr("value"));
                }
            });
            $("#<%=hfDeleteItems.ClientID %>").val(selctedItems.join(','));
            $("#<%=hfSelAlertId.ClientID %>").val(selIDs.join(','));
        }

        function getGridAllItemIds() {
            var selctedItems1 = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                selctedItems1.push(item.attr("fileId"));
            });

            return selctedItems1.join(',');
        }

        function GridSorting(sortColumn) {
            $("#<%= hfdSortField.ClientID %>").val(sortColumn);
            var $hfdDir = $("#<%= hfdDirection.ClientID %>");
            if ($hfdDir.val() == 'ASC') {
                $hfdDir.val('DESC');
            }
            else {
                $hfdDir.val('ASC');
            }
            $("#<%= btnSort.ClientID %>").trigger("click");
        }

        function PopupAlertDetail(iconImage, fileId, typeName, LoanTaskId, LoanAlertId) {
            if (typeName == 'Rate Lock')// rate lock
            {
                $("#frmDetail").attr("src", "PopupAlertDetail.aspx?icon=" + iconImage + "&fileId=" + fileId).show();
                $("#frmDetail").dialog({
                    bgiframe: true,
                    modal: true,
                    title: "Rate Lock Alert",
                    width: 580,
                    height: 420,
                    close: function (event, ui) { $(this).dialog('destroy') },
                    open: function (event, ui) { $(this).css("width", "100%") }
                });
            }
            else if (typeName == 'Task Alert')//task alert
            {
                var dialog = $("#taskAlertDetail");
                dialog.attr("src", "TaskAlertDetail.aspx?fileID=" + fileId + "&LoanTaskId=" + LoanTaskId );
                dialog.dialog({
                    bgiframe: true,
                    modal: true,
                    title: "Task Alert Detail",
                    width: 580,
                    height: 420,
                    close: function (event, ui) { $(this).dialog('destroy') },
                    open: function (event, ui) { $(this).css("width", "100%") }
                });
            }
            else if (typeName == 'Rule Alert')//Rule alert
            {
                var dialog = $("#ruleAlertDetail");
                dialog.attr("src", "../LoanDetails/RuleAlertPopup.aspx?LoanID=" + fileId + "&AlertID=" + LoanAlertId);
                dialog.dialog({
                    bgiframe: true,
                    modal: true,
                    title: "Rule Alert Detail",
                    width: 690,
                    height: 600,
                    close: function (event, ui) { $(this).dialog('destroy') },
                    open: function (event, ui) { $(this).css("width", "100%") }
                });
            }
        }

        function closeDialog(bRefresh) {
            $("#taskAlertDetail").dialog('destroy');
            if (bRefresh === true)
            {
                <%=this.ClientScript.GetPostBackEventReference(this.lbtnEmpty, null) %>;
            }
        }

        function closeDialogR() {
            $("#frmDetail").dialog('destroy');
        }

        function CloseDialog_RuleAlert() {
            $("#ruleAlertDetail").dialog('destroy');
        }

       
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle">
        Summary Alert View</div>
    <div class="SplitLine">
    </div>
    <iframe id="taskAlertDetail" frameborder="0" style="display:none;border:none;padding:0;margin:0;"></iframe>
    <iframe id="ruleAlertDetail" frameborder="0" style="display: none; border: none; padding: 0; margin: 0;"></iframe>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlRegion" runat="server" DataTextField="Name" DataValueField="RegionId"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                            <asp:ListItem Text="All Regions" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlDivison" runat="server" DataTextField="Name" DataValueField="DivisionId"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlDivison_SelectedIndexChanged">
                            <asp:ListItem Text="All Divisions" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="Name" DataValueField="BranchId">
                            <asp:ListItem Text="All Branches" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlTasks" runat="server">
                            <asp:ListItem Text="All Due Dates" Value=""></asp:ListItem>
                            <asp:ListItem Text="Overdue" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Overdue + Due Today" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Due Today" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Due Tomorrow" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Due in 7 Days" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlAlerts" runat="server">
                            <asp:ListItem Text="All Owners" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="My Alerts" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                     <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlAlertType" runat="server">
                            <asp:ListItem Text="All Alert Types" Value="" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Rate Lock Alert" Value="Rate Lock"></asp:ListItem>
                            <asp:ListItem Text="Rule Alert" Value="Rule Alert"></asp:ListItem>
                            <asp:ListItem Text="Task Alert" Value="Task Alert"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 775px">
                <tr>
                    <td style="width: 40px;">
                        <asp:DropDownList ID="ddlAlphabets" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlphabets_SelectedIndexChanged">
                            <asp:ListItem Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 300px;">
                        <ul class="ToolStrip">
                            <%-- <li><a id="aCreate" href="#">Setup</a><span>|</span></li>--%>
                            <li><a id="btnDetails" href="#">Details</a></li><span>|</span>
                            <%-- <li><a id="aDelete" href="#">Sync</a><span>|</span></li>
                            <li><a id="aUpdate" href="#">Remove</a></li>--%>
                            <li><asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export"></asp:LinkButton></li>
                        </ul>
                    </td>
                    <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            OnPageChanged="AspNetPager1_PageChanged">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divAlertList" class="Widget" style="width: 790px; margin-top: 5px;">
        <div class="Widget_Grid_Header">
            <div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 22px;">
                            <input id="cbxGridAll" type="checkbox" onclick="CheckAll(this.checked)" tag="alertList"
                                style="position: relative; left: -1px;" />
                        </td>
                        <td style="width: 260px;">
                            <a href="javascript:GridSorting('Desc')">Alert Description</a>
                        </td>
                        <td style="width: 120px;">
                            <a href="javascript:GridSorting('Username')">Assign User</a>
                        </td>
                        <td style="width: 100px;">
                            <a href="javascript:GridSorting('AlertType')">Type</a>
                        </td>
                        <td style="width: 110px;">
                            <a href="javascript:GridSorting('DueDate')">Due Date</a>
                        </td>
                        <td>
                            <a href="javascript:GridSorting('BorrowerName')">Borrower</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="Widget_Body">
            <asp:Button ID="btnSort" runat="server" Text="Button" Style="display: none" OnClick="btnSort_Click" />
            <asp:HiddenField ID="hfdSortField" runat="server" />
            <asp:HiddenField ID="hfdDirection" runat="server" Value="ASC" />
            <asp:Repeater ID="rptGrid" runat="server">
                <ItemTemplate>
                    <div class="GridRow24_Odd" style="">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                            <tr>
                                <td style="width: 23px;">
                                    <input id="Checkbox2" type="checkbox" tag="alertList" value="<%# Eval("LoanAlertId") %>"
                                        fileid='<%# Eval("FileId") %>' onchange='getSelectedItems()' />
                                </td>
                                <td style="width: 260px; border-right: solid 1px #e4e7ef;">
                                    <span class="alert">
                                        <img id="imgRateLockIcon" src='../images/loan/<%# Eval("DisplayIcon")%>' width="16" style="cursor:pointer"
                                            height="16" onclick="PopupAlertDetail('<%# Eval("DisplayIcon")%>','<%# Eval("FileId") %>','<%# Eval("AlertType")%>','<%# Eval("LoanTaskId")%>','<%# Eval("LoanAlertId")%>')" /></span>
                                    <%# Eval("Desc")%>
                                </td>
                                <td style="width: 120px; border-right: solid 1px #e4e7ef;">
                                    <%# Eval("Username")%>
                                </td>
                                <td style="width: 100px; border-right: solid 1px #e4e7ef;">
                                    <span id='spanType_<%# Eval("FileId")%>'><%# Eval("AlertType")%></span>
                                </td>
                                <td style="width: 110px; border-right: solid 1px #e4e7ef;">
                                    <%# DateTime.Parse(Eval("DueDate").ToString()).ToShortDateString()%>
                                </td>
                                <td>
                                    <a href="javascript:GoToLoanDetails('<%# Eval("FileId")%>', '<%# Eval("LoanStatus")%>')">
                                        <%# Eval("BorrowerName")%></a>
                                </td>
                            </tr>
                        </table>
                        <div class="DashedBorder">
                            &nbsp;</div>
                    </div>
                </ItemTemplate>
                <AlternatingItemTemplate>
                    <div class="GridRow24_Even" style="">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                            <tr>
                                <td style="width: 23px;">
                                    <input id="Checkbox2" type="checkbox" tag="alertList" value="<%# Eval("LoanAlertId") %>"
                                        fileid='<%# Eval("FileId") %>' onchange='getSelectedItems()' />
                                </td>
                                <td style="width: 260px; border-right: solid 1px #e4e7ef;">
                                    <span class="alert">
                                        <img id="imgRateLockIcon" src='../images/loan/<%# Eval("DisplayIcon")%>' width="16" style="cursor:pointer"
                                            height="16" onclick="PopupAlertDetail('<%# Eval("DisplayIcon")%>','<%# Eval("FileId") %>','<%# Eval("AlertType")%>','<%# Eval("LoanTaskId")%>','<%# Eval("LoanAlertId")%>')" /></span>
                                    <%# Eval("Desc")%>
                                </td>
                                <td style="width: 120px; border-right: solid 1px #e4e7ef;">
                                    <%# Eval("Username")%>
                                </td>
                                <td style="width: 100px; border-right: solid 1px #e4e7ef;">
                                     <span id='spanType_<%# Eval("FileId")%>'><%# Eval("AlertType")%></span>
                                </td>
                                <td style="width: 110px; border-right: solid 1px #e4e7ef;">
                                    <%# DateTime.Parse(Eval("DueDate").ToString()).ToShortDateString()%>
                                </td>
                                <td>
                                    <a href="javascript:GoToLoanDetails('<%# Eval("FileId")%>', '<%# Eval("LoanStatus")%>')">
                                        <%# Eval("BorrowerName")%></a>
                                </td>
                            </tr>
                        </table>
                        <div class="DashedBorder">
                            &nbsp;</div>
                    </div>
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
    </div>
    <asp:HiddenField ID="hfDeleteItems" runat="server" />
    <asp:HiddenField ID="hfSelAlertId" runat="server" />
    <iframe id="frmDetail" frameborder="no" style="display:none;border:none;padding:0;margin:0;"></iframe>
    <asp:LinkButton ID="lbtnEmpty" runat="server" OnClick="lbtnEmpty_Click"></asp:LinkButton>
</asp:Content>
