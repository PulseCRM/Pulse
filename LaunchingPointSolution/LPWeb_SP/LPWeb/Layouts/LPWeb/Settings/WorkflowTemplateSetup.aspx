<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowTemplateSetup.aspx.cs"
    EnableViewState="true" Title="Workflow Template Setup" Inherits="LPWeb.Settings.WorkflowTemplateSetup"
    MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <style>
        a.TemplateTask:link, :visited, :active
        {
            color: #818892;
        }
        a.TemplateTask:hover
        {
            color: Blue;
        }
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        var ddlAlphabetsClientId = "#<%= ddlAlphabets.ClientID %>";
        var gvTaskListClientId = "#<%= gvTaskList.ClientID %>";
        var hdnTmpIDsClientId = "#<%= hdnTmpIDs.ClientID %>";
        var hdnTmpIDClientId = "#<%= hdnTmpID.ClientID %>";
        var ddlStageClientId = "#<%= ddlStage.ClientID %>";

        $(document).ready(function () {

            // add onchange event 
            $(ddlAlphabetsClientId).change(function () {
                var tmid = $(hdnTmpIDClientId).val();
                if (tmid != "" && tmid != "0") {
                    var SelectedAlphabet = $(ddlAlphabetsClientId).val();
                    var SelectedStage = $(ddlStageClientId).val();
                    window.location.href = "WorkflowTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&Alphabet=" + SelectedAlphabet + "&StageID=" + SelectedStage + "&WflTemplId=" + tmid;
                }
            });

            // add onchange event 
            $(ddlStageClientId).change(function () {
                var tmid = $(hdnTmpIDClientId).val();
                if (tmid != "" && tmid != "0") {
                    var SelectedAlphabet = $(ddlAlphabetsClientId).val();
                    var SelectedStage = $(ddlStageClientId).val();
                    window.location.href = "WorkflowTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&Alphabet=" + SelectedAlphabet + "&StageID=" + SelectedStage + "&WflTemplId=" + tmid;
                }
            });

            $("a.TemplateTask").each(function () {
                $(this).click(function () {
                    var selctedItems = new Array();
                    $(gvTaskListClientId + " :checkbox:gt(0)").each(function () {
                        var item = $(this);
                        selctedItems.push(item.attr("tag"));
                    });

                    var tmid = $(hdnTmpIDClientId).val();
                    if (tmid == "0" || tmid == "") {
                        return;
                    }
                    //                  var stageid = 0;
                    //                    if (stageid == "0" || stageid == "") {
                    //                        return;
                    //                    }
                    var stageid = $(this).attr("WflStageId");
                    var taskid = $(this).attr("tag");
                    var radomNum = Math.random();
                    var radomStr = radomNum.toString().substr(2);
                    $("#ifrTaskAdd").attr("src", "WorkflowTaskTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&taskid=" + taskid + "&templateid=" + tmid + "&stageid=" + stageid + "&sid=" + radomStr);
                    $("#divAddTask").dialog({
                        height: 400,
                        width: 880,
                        title: ' Update a Workflow Task',
                        modal: true,
                        resizable: false
                    });

                    $(".ui-dialog").css("border", "solid 3px #aaaaaa")

                });
            });
        });

        function CheckSelected() {

            if ($(gvTaskListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }
            else {
                BeforeSave();
                return true;
            }
        }

        function BeforeSave() {

            var TmpIDs = "";
            $(gvTaskListClientId + " tr td :checkbox[checked=true]").each(function (i) {

                var WflTemplId = $(this).attr("tag");
                if (i == 0) {
                    TmpIDs = WflTemplId;
                }
                else {
                    TmpIDs += "," + WflTemplId;
                }
            });

            $(hdnTmpIDsClientId).val(TmpIDs);

            return true;
        }

        function CheckInput() {
            var txbTemplateNameClientId = "#<%= txbTemplateName.ClientID %>";
            if ($(txbTemplateNameClientId).val() == "") {
                alert("Please enter the workflow template name.");
                return false;
            }

            var txbEstCloseClientId = "#<%= txbEstClose.ClientID %>";
            if ($(txbEstCloseClientId).val() == "") {
                alert("Please enter the stage target completion days from Est Close.");
                return false;
            }

            return true;
        }

        // check/decheck all
        function CheckAll(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

        // show popup for enter Branch name
        function EnterTemplateName() {
            // clear Template name
            $("#txbTemplateName1").val("")

            // show modal
            $("#divNewTemplateName").dialog({
                height: 160,
                width: 390,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
            $("body>div[role=dialog]").appendTo("#aspnetForm");

        }

        function ClosePopupTemplateName() {

            // close modal
            $("#divNewTemplateName").dialog("close");
        }

        function BeforeCloneTemplateName() {

            var txbTemplateNameClientId = "#<%= txbTemplateName.ClientID %>";
            var txbTemplateName1ClientId = "#<%= txbTemplateName1.ClientID %>";
            $(txbTemplateNameClientId).val($(txbTemplateName1ClientId).val());
            $("#divNewTemplateName").dialog("close");
            // close modal

        }

        function PageCancel() {
            var hdnPageFromClientId = "#<%= hdnPageFrom.ClientID %>";

            window.location.href = "<%= FromURL %>";
        }

        function ClosePopupTask() {

            // close modal
            $("#divAddTask").dialog("close");

        }

        function CreateTask() {
            var tmid = $(hdnTmpIDClientId).val();
            var stageid = $(ddlStageClientId).val();
            if (tmid == "0" || tmid == "") {
                return;
            }
            //            if (stageid == "0" || stageid == "") {
            //                return;
            //                alert(2);
            //            }

            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrTaskAdd").attr("src", "WorkflowTaskTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&taskid=0&templateid=" + tmid + "&stageid=" + stageid + "&sid=" + radomStr);
            //$("#ifrTaskAdd").attr("src", "WorkflowTaskTemplateSetup.aspx");
            $("#divAddTask").dialog({
                height: 400,
                width: 880,
                title: "Workflow Task Setup",
                modal: true,
                resizable: false
            });

            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            $("body>div[role=dialog]").appendTo("#aspnetForm");
        }

        function UpdateTask() {

            if ($(gvTaskListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one record.");
                return;
            }
            if ($(gvTaskListClientId + " tr td :checkbox[checked=true]").length > 1) {
                alert("You can select only one record.");
                return;
            }
            BeforeSave();
            var tmid = $(hdnTmpIDClientId).val();
            var stageid = $(ddlStageClientId).val();
            if (tmid == "0" || tmid == "") {
                return;
            }
            //            if (stageid == "0" || stageid == "") {
            //                return;
            //            }
            var taskid = $(hdnTmpIDsClientId).val();
            if (taskid == "0" || taskid == "") {
                return;
            }

            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrTaskAdd").attr("src", "WorkflowTaskTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&taskid=" + taskid + "&templateid=" + tmid + "&stageid=" + stageid + "&sid=" + radomStr);
            $("#divAddTask").dialog({
                height: 400,
                width: 880,
                title: "Update Workflow Task",
                modal: true,
                resizable: false
            });

            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
            $("body>div[role=dialog]").appendTo("#aspnetForm");

            return false;
        }

        function BeforeDeleteTask() {

            BeforeSave();

            if ($(gvTaskListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }

            return confirm('Are you sure you want to delete the task template(s)?');

        }

        function BeforeDisableTask() {

            BeforeSave();

            if ($(gvTaskListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }

            return confirm('Are you sure you want to disable the task template(s)?');

        }
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div class="Heading" style="padding-left: 0px;">
        Workflow Template Setup</div>
    <div class="SplitLine">
    </div>
    <div style="padding-top: 9px;">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 730px;">
            <tr style="height: 1px;">
                <td style="width: 100px;">
                </td>
                <td style="width: 60px;">
                </td>
                <td style="width: 155px;">
                </td>
                <td style="width: 315px;">
                </td>
                <td style="width: 100px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-left: 0px;">
                    <asp:Label ID="Label1" runat="server" Text="Workflow Template Name:"></asp:Label>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txbTemplateName" runat="server" MaxLength="50" Width="355px"></asp:TextBox>
                </td>
                <td align="right" style="padding-right: 9px;">
                    <asp:CheckBox ID="cbEnabled" Text=" Enabled" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 9px;">
                    <asp:Label ID="Label2" runat="server" Text="Description:"></asp:Label>
                </td>
                <td colspan="3" style="padding-top: 9px;">
                    <asp:TextBox ID="txbEescription" runat="server" Height="60px" TextMode="MultiLine"
                        MaxLength="500" Width="593px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="width: 100px; padding-top: 9px;">
                    <asp:Label ID="Label3" runat="server" Text="Stage:"></asp:Label>
                </td>
                <td colspan="3" style="padding-top: 9px;">
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlStage" EnableViewState="true" runat="server" Height="21px"
                                    Width="100px">
                                </asp:DropDownList>
                            </td>
                            <td align="right" style="padding-right: 8px;">
                                <asp:Label ID="Label4" runat="server" Text="Stage target completion days from Est Close :"></asp:Label>
                                <asp:TextBox ID="txbEstClose" runat="server" Width="20px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="divToolBar" style="margin-top: 13px;">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 760px;">
            <tr>
                <td style="width: 40px;">
                    <asp:DropDownList ID="ddlAlphabets" runat="server">
                        <asp:ListItem Value=""></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td style="width: 300px;">
                    <ul class="ToolStrip">
                        <li>
                            <asp:LinkButton ID="btnCreateTask" runat="server" OnClientClick=" CreateTask();return false; "
                                Text="Create"></asp:LinkButton>
                            <span>|</span> </li>
                        <li>
                            <asp:LinkButton ID="btnDisable" runat="server" OnClientClick="return BeforeDisableTask(); "
                                Text="Disable" OnClick="btnDisable_Click"></asp:LinkButton><span>|</span>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnDelete" runat="server" OnClientClick="return BeforeDeleteTask(); "
                                Text="Delete" OnClick="btnDelete_Click"></asp:LinkButton><span>|</span>
                        </li>
                        <li>
                            <asp:LinkButton ID="btnUpdateTask" runat="server" OnClientClick=" UpdateTask();return false; "
                                Text="Update"></asp:LinkButton>
                        </li>
                    </ul>
                </td>
                <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                        UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                        NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                        CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                    </webdiyer:AspNetPager>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDivision" class="ColorGrid" style="width: 760px; margin-top: 5px;">
        <asp:GridView ID="gvTaskList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
            Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
            OnSorting="gvTaskList_Sorting" CellPadding="3" GridLines="None">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <input type="checkbox" onclick="CheckAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("TemplTaskId")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task" SortExpression="Name" ItemStyle-Wrap="false"
                    ItemStyle-Width="120">
                    <ItemTemplate>
                        <a href="" class="TemplateTask" tag='<%# Eval("TemplTaskId")%>' wflstageid='<%# Eval("WflStageId")%>'
                            onclick="return false;">
                            <%# Eval("Name")%></a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Prerequisite Task" SortExpression="PrerequisiteTaskName"
                    ItemStyle-Wrap="false" ItemStyle-Width="120">
                    <ItemTemplate>
                        <%# Eval("PrerequisiteTaskName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Days Due After Prerequisite" SortExpression="DaysDueAfterPrerequisite"
                    ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# Eval("DaysDueAfterPrerequisite")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Days Due From COE" SortExpression="DaysDueFromCoe"
                    ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                    <ItemTemplate>
                        <%# Eval("DaysDueFromCoe")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Stage Name" SortExpression="StageName" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("StageName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled" ItemStyle-Wrap="false">
                    <ItemTemplate>
                        <%# Eval("Enabled")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="margin-top: 20px;">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return CheckInput()"
                        CssClass="Btn-66" OnClick="btnSave_Click" />
                </td>
                <td style="padding-left: 8px;">
                    <input id="Button2" type="button" value="Clone" class="Btn-66" onclick="EnterTemplateName()" />
                </td>
                <td style="padding-left: 8px;">
                    <asp:Button ID="btnDeleteTemplate" runat="server" Text="Delete" class="Btn-66" OnClientClick="return confirm('Are you sure you want to continue?');"
                        OnClick="btnDeleteTemplate_Click" />
                </td>
                <td style="padding-left: 8px;">
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="PageCancel()" />
                </td>
            </tr>
        </table>
    </div>
    <asp:HiddenField ID="hdnTmpIDs" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hdnTmpID" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hdnPageFrom" EnableViewState="true" runat="server" />
    <div id="divNewTemplateName" title="Enter Workflow Template Name" style="display: none;
        margin: 10px;">
        <table>
            <tr>
                <td style="width: 180px;">
                    Workflow Template Name:
                </td>
                <td>
                    <asp:TextBox ID="txbTemplateName1" runat="server" MaxLength="50" Style="width: 200px;"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <div style="text-align: center;">
            <asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="Btn-66" OnClientClick="BeforeCloneTemplateName()"
                OnClick="btnCreate_Click" />
            &nbsp;&nbsp;
            <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="ClosePopupTemplateName()" />
        </div>
        <div id="divMsg" style="margin-top: 10px; text-align: center; display: none;">
            <img id="imgWaiting" src='../images/waiting.gif' style='position: relative; top: 2px;' />
            <label id="lbMsg" style='font-weight: bold;'>
                Please wait...</label>
        </div>
    </div>
    <div id="divAddTask" title="Create a new Workflow Task" style="display: none;">
        <iframe id="ifrTaskAdd" frameborder="0" scrolling="no" width="850px" height="360px">
        </iframe>
    </div>
</asp:Content>
