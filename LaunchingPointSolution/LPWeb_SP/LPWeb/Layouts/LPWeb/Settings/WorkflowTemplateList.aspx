<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WorkflowTemplateList.aspx.cs"
    Title="Workflow Template List" Inherits="LPWeb.Layouts.LPWeb.Settings.WorkflowTemplateList"
    MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
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
        var gvWorkFolwListClientId = "#<%= gvWorkFolwList.ClientID %>";
        var hdnTmpIDsClientId = "#<%= hdnTmpIDs.ClientID %>";
        var hdnWorkflowTempl = "#<%= hdnWorkflowTempl.ClientID %>";

        $(document).ready(function () {

            DrawTab();
            // add onchange event 
            $(ddlAlphabetsClientId).change(function () {
                var SelectedAlphabet = $(ddlAlphabetsClientId).val();
                window.location.href = "WorkflowTemplateList.aspx?Alphabet=" + SelectedAlphabet;
            });



            if ($(hdnWorkflowTempl).val().indexOf('1') == -1) {
                $("#aCreate").removeAttr('href');
            }
            if ($(hdnWorkflowTempl).val().indexOf('2') == -1) {
                $("#aUpdate").removeAttr('href');
            }
        });

        function CheckSelected() {

            if ($(gvWorkFolwListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }
            else {
                BeforeSave();
                return confirm('Are you sure you want to continue?');
            }
        }

        function BeforeSave() {

            var TmpIDs = "";
            $(gvWorkFolwListClientId + " tr td :checkbox[checked=true]").each(function (i) {

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

        function CreateTemplate() {

            window.location.href = 'WorkflowTemplateSetup.aspx?FromPage=<%= FromURL %>';

        }

        function UpdateTemplate() {

            if ($(gvWorkFolwListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one record.");
                return;
            }
            if ($(gvWorkFolwListClientId + " tr td :checkbox[checked=true]").length > 1) {
                alert("You can select only one record.");
                return;
            }
            BeforeSave();
            window.location.href = 'WorkflowTemplateSetup.aspx?FromPage=<%= FromURL %>&WflTemplId=' + $(hdnTmpIDsClientId).val();

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
// ]]>
    </script>
    <style>
        .TabContent input.Btn-66
        {
            margin-right: 8px;
        }
        .TabContent input[type="text"], select, input[type="file"]
        {
            margin-left: 15px;
            margin-right: 15px;
        }
        #divUserList td
        {
            margin-left: 0px;
            margin-right: 0px;
            padding: 0;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        //#region Create/Update Workflow Template

        function ShowDialog_AddWorkflowTemplate() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "WorkflowTemplateAdd2.aspx?sid=" + sid;

            window.location.href = iFrameSrc;
            //ShowDialog("Workflow Template Setup", 987, 600, 1002, 640, iFrameSrc);
        }

        function ShowDialog_EditWorkflowTemplate() {

            var SelectedCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gvWorkFolwList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No workflow template was selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one workflow template can be selected.");
                return;
            }

            var WorkflowTemplateID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gvWorkFolwList tr:not(:first) td :checkbox:checked").attr("tag");
            //alert(WorkflowTemplateID);

            UpdateWorkflowTemplate(WorkflowTemplateID);
        }

        function UpdateWorkflowTemplate(WorkflowTemplateID) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "WorkflowTemplateEdit2.aspx?sid=" + sid + "&WorkflowTemplateID=" + WorkflowTemplateID;

            window.location.href = iFrameSrc;
            //ShowDialog("Workflow Template Setup", 987, 600, 1002, 640, iFrameSrc);

        }

        //#endregion

        //#region Show Dialog

        function ShowDialog(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divWflTempSetup").attr("title", Title);

            $("#ifrWflTempSetup").attr("src", "");
            $("#ifrWflTempSetup").attr("src", iFrameSrc);
            $("#ifrWflTempSetup").width(iFrameWidth);
            $("#ifrWflTempSetup").height(iFrameHeight);

            // show modal
            $("#divWflTempSetup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) { $("#divWflTempSetup").dialog("destroy"); }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog() {

            $("#divWflTempSetup").dialog("close");
        }

        //#endregion
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divCompanyTabs" style="margin-top: 10px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="WorkflowTemplateList.aspx"><span>Workflow Template List</span></a></li>
                                <li><a href="StageTemplateList.aspx"><span>Stage List</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody" style="margin-bottom:10px;">
                <div id="TabLine1" class="TabLeftLine">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine">
                    &nbsp;</div>
                <div class="TabContent">
                    <div id="divFilter" style="margin-top: 10px;">
                        <table cellpadding="0" cellspacing="0" style="padding-left: 15px;">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="ckbAutoProcessing" runat="server" />
                                </td>
                                <td style="padding-left: 4px;">
                                    Auto Apply Default Processing Workflow
                                </td>
                                <td style="padding-left: 15px;">
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-top: 10px;">
                                    <asp:CheckBox ID="ckbAutoProspecting" runat="server" />
                                </td>
                                <td style="padding-left: 4px; padding-top: 10px;">
                                    Auto Apply Default Prospecting Workflow
                                </td>
                                <td style="padding-left: 15px; padding-top: 10px;">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divToolBar" style="margin-top: 13px;">
                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                            <tr>
                                <td style="width: 40px;">
                                    <asp:DropDownList ID="ddlAlphabets" runat="server">
                                        <asp:ListItem Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td style="width: 300px;">
                                    <ul class="ToolStrip">
                                        <li id="liCreate"><a id="aCreate" href="javascript:ShowDialog_AddWorkflowTemplate()">
                                            Create</a><span>|</span> </li>
                                        <li id="liDisable">
                                            <asp:LinkButton ID="btnDisable" runat="server" OnClientClick="return CheckSelected(); "
                                                Text="Disable" OnClick="btnDisable_Click"></asp:LinkButton><span>|</span>
                                        </li>
                                        <li id="liEnable">
                                            <asp:LinkButton ID="btnEnable" runat="server" OnClientClick="return CheckSelected(); "
                                                Text="Enable" OnClick="btnEnable_Click"></asp:LinkButton><span>|</span>
                                        </li>
                                        <li id="liDelete">
                                            <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick="return CheckSelected();"
                                                OnClick="btnDelete_Click"></asp:LinkButton><span>|</span> </li>
                                        <li id="liUpdate"><a id="aUpdate" href="javascript:ShowDialog_EditWorkflowTemplate()">
                                            Update</a></li>
                                    </ul>
                                </td>
                                <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                                    <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                                        UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                        NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                        CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                                    </webdiyer:AspNetPager>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divDivision" class="ColorGrid" style="margin-top: 5px; margin-bottom:10px;">
                        <asp:GridView ID="gvWorkFolwList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                            Width="100%" AllowSorting="true" EmptyDataText="There is no workflow template."
                            OnSorting="gvWorkFolwList_Sorting" CellPadding="3" GridLines="None">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate>
                                        <input type="checkbox" onclick="CheckAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="checkbox" tag='<%# Eval("WflTemplId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" SortExpression="Name" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <a href="javascript:UpdateWorkflowTemplate('<%# Eval("WflTemplId") %>')" class="TemplateTask"
                                            tag='<%# Eval("WflTemplId") %>'>
                                            <%# Eval("Name")%></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description" SortExpression="Desc" ItemStyle-Width="350">
                                    <ItemTemplate>
                                        <dd class="TextEllipsis" style="width: 340px; margin: 0px;" title="<%# Eval("Desc")%>">
                                            <%# Eval("Desc")%></dd>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Workflow Type" SortExpression="WorkflowType" ItemStyle-Wrap="true"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <%# Eval("WorkflowType")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stages" SortExpression="Stages" ItemStyle-Wrap="true"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <%# Eval("Stages")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type" SortExpression="Custom" ItemStyle-Wrap="true"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                                    <ItemTemplate>
                                        <%# Eval("Custom_Cov")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Default" SortExpression="Default" ItemStyle-Wrap="false"
                                    ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <%# Eval("Default_Cov")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled" ItemStyle-Wrap="false"
                                    ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <%# Eval("Enabled_Cov")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">
                            &nbsp;</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTmpIDs" runat="server" />
    <asp:HiddenField ID="hdnWorkflowTempl" runat="server" />
    <div id="divWflTempSetup" title="Workflow Template Setup" style="display: none;">
        <iframe id="ifrWflTempSetup" frameborder="0" scrolling="no" width="987px" height="700px">
        </iframe>
    </div>
</asp:Content>