<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StageTemplateList.aspx.cs"
    Title="Stage Template List" Inherits="LPWeb.Layouts.LPWeb.Settings.StageTemplateList"
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
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <style>
        a.TemplateStage:link, :visited, :active
        {
            color: #818892;
        }
        a.TemplateStage:hover
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
        var gvStageListClientId = "#<%= gvStageList.ClientID %>";
        var hdnTmpIDsClientId = "#<%= hdnTmpIDs.ClientID %>";
        var hdnStageTemplate = "#<%= hdnStageTemplate.ClientID %>";
        var ddlWorkflowTypeClientId = "#<%= ddlWorkflowType.ClientID %>";

        $(document).ready(function () {

            DrawTab();

            InitWorkflowType();

            $("#aCreate").click(CreateStage);
            $("#aUpdate").click(UpdateStage);


            if ($(hdnStageTemplate).val().indexOf('1') == -1) {
                $("#aCreate").removeAttr('href');
            }
            if ($(hdnStageTemplate).val().indexOf('2') == -1) {
                $("#aUpdate").removeAttr('href');
            }

            $("a.TemplateStage").each(function () {
                $(this).click(function () {
                    var selctedItems = new Array();
                    $(gvStageListClientId + " :checkbox:gt(0)").each(function () {
                        var item = $(this);
                        selctedItems.push(item.attr("tag"));
                    });

                    var stageid = $(this).attr("tag");
                    var radomNum = Math.random();
                    var radomStr = radomNum.toString().substr(2);
                    $("#ifrStageAdd").attr("src", "StageTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&stageid=" + stageid + "&sid=" + radomStr);
                    $("#divAddStage").dialog({
                        height: 400,
                        width: 730,
                        title: ' Update a stage template',
                        modal: true,
                        resizable: false
                    });

                    $(".ui-dialog").css("border", "solid 3px #aaaaaa")

                });
            });
        });

        function CheckSelected() {

            if ($(gvStageListClientId + " tr td :checkbox[checked=true]").length == 0) {
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
            $(gvStageListClientId + " tr td :checkbox[checked=true]").each(function (i) {

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

        function ClosePopupStage() {

            // close modal
            $("#divAddStage").dialog("close");

        }

        function CreateStage() {

            var WorkflowType = $(ddlWorkflowTypeClientId).val();
            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrStageAdd").attr("src", "StageTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&stageid=0&sid=" + radomStr + "&WorkflowType=" + WorkflowType);
            $("#divAddStage").dialog({
                height: 400,
                width: 730,
                title: "Stage template setup",
                modal: true,
                resizable: false
            });

            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            $("body>div[role=dialog]").appendTo("#aspnetForm");

            return false;
        }

        function UpdateStage() {

            if ($(gvStageListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one record.");
                return;
            }
            if ($(gvStageListClientId + " tr td :checkbox[checked=true]").length > 1) {
                alert("You can select only one record.");
                return;
            }
            BeforeSave();

            var stageid = $(hdnTmpIDsClientId).val();
            if (stageid == "0" || stageid == "") {
                return;
            }

            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrStageAdd").attr("src", "StageTemplateSetup.aspx?FromPage=" + encodeURIComponent("<%= FromURL %>") + "&stageid=" + stageid + "&sid=" + radomStr);
            $("#divAddStage").dialog({
                height: 400,
                width: 730,
                title: "Update a stage template",
                modal: true,
                resizable: false
            });

            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
            $("body>div[role=dialog]").appendTo("#aspnetForm");

            return false;
        }

        function BeforeDeleteStage() {

            BeforeSave();

            if ($(gvStageListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }

            var bRe = "true";
            $(gvStageListClientId + " tr td :checkbox[checked=true]").each(function (i) {
                var Custom = $(this).attr("custom");
                if (Custom != 'True') {
                    var StageName = $(this).attr("stagename");
                    alert("The selected <" + StageName + "> is a standard stage. You cannot delete a standard stage.");
                    bRe = "false";
                    return false;
                }

            });
            if (bRe != "true") {
                return false;
            }

            return confirm('This operation is not reversible. Are you sure you want to continue?');

        }

        function BeforeDisableStage() {

            BeforeSave();

            if ($(gvStageListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }
            var bRe = "true";
            $(gvStageListClientId + " tr td :checkbox[checked=true]").each(function (i) {
                var Custom = $(this).attr("custom");
                if (Custom != 'True') {
                    var StageName = $(this).attr("stagename");
                    alert("The selected <" + StageName + "> is a standard stage. You cannot disable a standard stage.");
                    bRe = "false";
                    return false;
                }

            });
            if (bRe != "true") {
                return false;
            }

            return confirm('Are you sure you want to disable the stage template(s)?');

        }

        function aApply_onclick() {

            var result = confirm("This will apply all the Stage \"Display As\" to the loan stages and remove the orphan loan stages that do not associate with any workflow stage. Are you sure you want to continue?");
            if (result == false) {

                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Please wait...");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("ApplytoLoanStages.ashx?sid=" + sid, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        $("body").unblock();

                        return;
                    }

                    $("body").unblock();

                }, 2000);
            });
        }

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $("body").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        function InitWorkflowType() {

            // WorkflowType
            var WorkflowType = GetQueryString1("WorkflowType");
            if (WorkflowType != "") {

                $(ddlWorkflowTypeClientId).val(WorkflowType);
            }

            // OrderByField
            var OrderByField = GetQueryString1("OrderByField");
            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByField == "") {

                $("#spOrderByFlag5").text("▼");
            }
            else {

                if (OrderByField == "Name") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag1").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag1").text("▼");
                    }
                }
                else if (OrderByField == "Alias") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag2").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag2").text("▼");
                    }
                }
                else if (OrderByField == "WorkflowTypeName") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag3").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag3").text("▼");
                    }
                }
                else if (OrderByField == "CustomName") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag4").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag4").text("▼");
                    }
                }
                else if (OrderByField == "SequenceNumber") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag5").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag5").text("▼");
                    }
                }
                else if (OrderByField == "EnabledName") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag6").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag6").text("▼");
                    }
                }
            }
        }

        function ddlWorkflowType_onchange() {

            var sQueryStrings = BuildQueryStrings();

            if (sQueryStrings == "") {

                window.location.href = "StageTemplateList.aspx";
            }
            else {

                var radomNum = Math.random();
                var sid = radomNum.toString().substr(2);
                window.location.href = "StageTemplateList.aspx?sid=" + sid + sQueryStrings;
            }
        }

        function BuildQueryStrings() {

            // 参数字符串
            var sQueryStrings = "";

            // WorkflowType
            var WorkflowType = $(ddlWorkflowTypeClientId).val();
            if (WorkflowType != "") {

                sQueryStrings += "&WorkflowType=" + encodeURIComponent(WorkflowType);
            }

            // PageIndex
            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "") {

                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            return sQueryStrings;
        }

        function OrderBy(field) {

            var sQueryStrings = BuildQueryStrings();

            // OrderByField
            sQueryStrings += "&OrderByField=" + field;

            // OrderByType
            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByType == "") {

                sQueryStrings += "&OrderByType=0";
            }
            else if (OrderByType == "0") {

                sQueryStrings += "&OrderByType=1";
            }
            else if (OrderByType == "1") {

                sQueryStrings += "&OrderByType=0";
            }

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "StageTemplateList.aspx?sid=" + sid + sQueryStrings;
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
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
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
                                <li><a href="WorkflowTemplateList.aspx"><span>Workflow Template List</span></a></li>
                                <li id="current"><a href="StageTemplateList.aspx"><span>Stage List</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
               <div id="TabLine1" class="TabLeftLine" style="width: 232px">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 444px">
                    &nbsp;</div>
                <div class="TabContent"> 
                
                <div id="divToolBar" style="margin-top: 10px;">
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 770px;">
                        <tr>
                            <td style="width: 150px;">
                                <asp:DropDownList ID="ddlWorkflowType" runat="server" Width="120px" onchange="ddlWorkflowType_onchange()">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 400px;">
                                <ul class="ToolStrip">
                                    <li  id="liCreate"><a id="aCreate" href="#">Create</a><span>|</span> </li>
                                    <li  id="liDisable">
                                        <asp:LinkButton ID="btnDisable" runat="server" OnClientClick="return BeforeDisableStage();"
                                            Text="Disable" OnClick="btnDisable_Click"></asp:LinkButton><span>|</span>
                                    </li>
                                    <li id="liDelete">
                                        <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick="return BeforeDeleteStage();"
                                            OnClick="btnDelete_Click"></asp:LinkButton><span>|</span> </li>
                                    <li id="liUpdate"><a id="aUpdate" href="#">Update</a><span>|</span></li>
                                    <li><a id="aApply" href="javascript:aApply_onclick()">Apply to Loan Stages</a></li>
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
                <div id="divStageTemplate" class="ColorGrid" style="width: 760px; margin-top: 5px; margin-left:9px;">
                    <asp:GridView ID="gvStageList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                        Width="100%" AllowSorting="true" EmptyDataText="There is no data in database." CellPadding="3" GridLines="None">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                <HeaderTemplate>
                                    <input type="checkbox" onclick="CheckAll(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="checkbox" tag='<%# Eval("TemplStageId") %>' custom='<%# Eval("Custom")%>' stagename='<%# Eval("Name")%>'/>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            <asp:TemplateField HeaderStyle-Width="120px" ItemStyle-Width="120px">
		                        <HeaderTemplate>
		                            <a href="javascript:OrderBy('Name')" style="text-decoration: underline;">Stage Name</a><span id="spOrderByFlag1" style="margin-left: 3px;"></span>
		                        </HeaderTemplate>
		                        <ItemTemplate>
		                            <a href="" class="TemplateStage" tag='<%# Eval("TemplStageId")%>' onclick="return false;"><%# Eval("Name")%></a>
		                        </ItemTemplate>
		                    </asp:TemplateField>
		                    
		                    
		                    <asp:TemplateField HeaderStyle-Width="120px" ItemStyle-Width="120px">
		                        <HeaderTemplate>
		                            <a href="javascript:OrderBy('Alias')" style="text-decoration: underline;">Display as</a><span id="spOrderByFlag2" style="margin-left: 3px;"></span>
		                        </HeaderTemplate>
		                        <ItemTemplate>
		                            <%# Eval("Alias")%>
		                        </ItemTemplate>
		                    </asp:TemplateField>
		                    
		                    <asp:TemplateField HeaderStyle-Width="120px" ItemStyle-Width="120px">
		                        <HeaderTemplate>
		                            <a href="javascript:OrderBy('WorkflowTypeName')" style="text-decoration: underline;">Workflow Type</a><span id="spOrderByFlag3" style="margin-left: 3px;"></span>
		                        </HeaderTemplate>
		                        <ItemTemplate>
		                            <%# Eval("WorkflowTypeName")%>
		                        </ItemTemplate>
		                    </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-Width="90px" ItemStyle-Width="90px">
		                        <HeaderTemplate>
		                            <a href="javascript:OrderBy('CustomName')" style="text-decoration: underline;">Type</a><span id="spOrderByFlag4" style="margin-left: 3px;"></span>
		                        </HeaderTemplate>
		                        <ItemTemplate>
		                            <%# Eval("CustomName")%>
		                        </ItemTemplate>
		                    </asp:TemplateField>
		                    
		                    <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-Width="80px">
		                        <HeaderTemplate>
		                            <a href="javascript:OrderBy('SequenceNumber')" style="text-decoration: underline;">Sequence</a><span id="spOrderByFlag5" style="margin-left: 3px;"></span>
		                        </HeaderTemplate>
		                        <ItemTemplate>
		                            <%# Eval("SequenceNumber")%>
		                        </ItemTemplate>
		                    </asp:TemplateField>
		                    
		                    <asp:TemplateField HeaderStyle-Width="90px" ItemStyle-Width="90px">
		                        <HeaderTemplate>
		                            <a href="javascript:OrderBy('EnabledName')" style="text-decoration: underline;">Enabled</a><span id="spOrderByFlag6" style="margin-left: 3px;"></span>
		                        </HeaderTemplate>
		                        <ItemTemplate>
		                            <%# Eval("EnabledName")%>
		                        </ItemTemplate>
		                    </asp:TemplateField>
                        </Columns>
                        
                    </asp:GridView>
                    <div class="GridPaddingBottom">&nbsp;</div>
                </div>
                <br />
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hdnTmpIDs" runat="server" />
    <asp:HiddenField ID="hdnStageTemplate" runat="server" />
    <div id="divAddStage" title="Create a new stage template" style="display: none;">
        <iframe id="ifrStageAdd" frameborder="0" scrolling="no" width="700px" height="360px">
        </iframe>
    </div>
    <div id="divWaiting" style="display: none; padding: 5px;">
	    <table style="margin-left: auto; margin-right: auto;">
		    <tr>
			    <td>
				    <img id="imgWaiting" src="../images/waiting.gif" />
			    </td>
			    <td style="padding-left: 5px;">
				    <label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
			    </td>
		    </tr>
	    </table>
    </div>
</asp:Content>