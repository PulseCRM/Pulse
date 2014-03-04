<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManagePipelineViewsPopup.aspx.cs" Inherits="ManagePipelineViewsPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manage Pipeline Views Popup</title>
    <link href="css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="css/style.grid.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    
    <script src="js/jquery.js" type="text/javascript"></script>
    <script src="js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            InitFilters();

        });

        function InitFilters() {

            // PipelineType
            var PipelineType = GetQueryString1("PipelineType");
            if (PipelineType != "") {
                $("#ddlPipleineType").val(PipelineType);
            }

            // Enable
            var Enable = GetQueryString1("Enable");
            if (Enable != "") {

                $("#ddlEnabled").val(Enable);
            }

            // OrderByField
            var OrderByField = GetQueryString1("OrderByField");
            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByField != "") {

                if (OrderByField == "PipelineType") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag1").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag1").text("▼");
                    }
                }
                else if (OrderByField == "ViewName") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag2").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag2").text("▼");
                    }
                }
                else if (OrderByField == "DefaultText") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag3").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag3").text("▼");
                    }
                }
                else if (OrderByField == "Enabled") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag4").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag4").text("▼");
                    }
                }
            }
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridUserPipelineViewList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridUserPipelineViewList tr td :checkbox").removeAttr("checked");
            }
        }



        function BuildQueryStrings() {

            var PipelineType = $("#ddlPipleineType").val();
            var Enable = $("#ddlEnabled").val();

            // 参数字符串
            var sQueryStrings = "";

            // PipelineType
            if (PipelineType != "All") {

                sQueryStrings += "&PipelineType=" + PipelineType;
            }

            // Enable
            if (Enable != "All") {

                sQueryStrings += "&Enable=" + Enable;
            }

            return sQueryStrings;
        }

        function btnDisplay_onclick() {

            var sQueryStrings = BuildQueryStrings();

            if (sQueryStrings == "") {

                window.location.href = window.location.pathname;
            }
            else {

                var RadomNum = Math.random();
                var sid = RadomNum.toString().substr(2);
                sQueryStrings = "?sid=" + sid + sQueryStrings;
                window.location.href = window.location.pathname + sQueryStrings;
            }
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
            sQueryStrings = "?sid=" + sid + sQueryStrings;
            window.location.href = window.location.pathname + sQueryStrings;
        }

        function aEnable_onclick() {

            var SelCount = $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("Please select pipeline view record(s).");
                return;
            }

            var PipelineViewIDs = "";
            $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").each(function () {

                var UserPipelineViewID = $(this).attr("UserPipelineViewID");
                if (PipelineViewIDs == "") {

                    PipelineViewIDs = UserPipelineViewID;
                }
                else {

                    PipelineViewIDs += "," + UserPipelineViewID;
                }
            });

            // ajax
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            $.getJSON("ManagePipelineViewPopup_ActionAjax.aspx?sid=" + sid + "&Action=Enable&PipelineViewIDs=" + PipelineViewIDs, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Enable pipeline view(s) successfully.");

                    window.parent.location.href = window.parent.location.href;
                }

            });
        }

        function aDisable_onclick() {

            var SelCount = $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("Please select pipeline view record(s).");
                return;
            }

            var PipelineViewIDs = "";
            $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").each(function () {

                var UserPipelineViewID = $(this).attr("UserPipelineViewID");
                if (PipelineViewIDs == "") {

                    PipelineViewIDs = UserPipelineViewID;
                }
                else {

                    PipelineViewIDs += "," + UserPipelineViewID;
                }
            });

            // ajax
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            $.getJSON("ManagePipelineViewPopup_ActionAjax.aspx?sid=" + sid + "&Action=Disable&PipelineViewIDs=" + PipelineViewIDs, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Disable pipeline view(s) successfully.");

                    window.parent.location.href = window.parent.location.href;
                }

            });
        }

        function aMakeDefault_onclick() {

            var SelCount = $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("Please select a pipeline view record.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one record can be selected.");
                return;
            }

            if ($("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked[EnabledText='No']").length == 1) {

                alert("The selected Pipeline View is disabled. Please enable the view first.");
                return;
            }

            var PipelineViewIDs = "";
            $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").each(function () {

                var UserPipelineViewID = $(this).attr("UserPipelineViewID");


                if (PipelineViewIDs == "") {

                    PipelineViewIDs = UserPipelineViewID;
                }
                else {

                    PipelineViewIDs += "," + UserPipelineViewID;
                }
            });

            // ajax
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            $.getJSON("ManagePipelineViewPopup_ActionAjax.aspx?sid=" + sid + "&Action=MakeDefault&PipelineViewIDs=" + PipelineViewIDs, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Make pipeline view(s) default successfully.");

                    window.parent.location.href = window.parent.location.href;
                }

            });
        }

        function aDelete_onclick() {

            var SelCount = $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("Please select pipeline view record(s).");
                return;
            }

            var result = confirm("Are you sure to continue?");
            if (result == false) {

                return;
            }

            var PipelineViewIDs = "";
            $("#gridUserPipelineViewList tr:not(:first) td :checkbox:checked").each(function () {

                var UserPipelineViewID = $(this).attr("UserPipelineViewID");
                if (PipelineViewIDs == "") {

                    PipelineViewIDs = UserPipelineViewID;
                }
                else {

                    PipelineViewIDs += "," + UserPipelineViewID;
                }
            });

            // ajax
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            $.getJSON("ManagePipelineViewPopup_ActionAjax.aspx?sid=" + sid + "&Action=Delete&PipelineViewIDs=" + PipelineViewIDs, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                }
                else {

                    alert("Delete pipeline view(s) successfully.");

                    window.parent.location.href = window.parent.location.href;
                }

            });
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width:450px; height:500px; border:solid 0px red; overflow:auto;">
        
        <div id="divFilters" style="margin-left:10px;">
            
            <table>
                <tr>
                    <td style="width:30px;">Pipeline:</td>
                    <td style="width:100px;">
                        <select id="ddlPipleineType" style="width: 80px; border: solid 1px #ccc;">
                            <option value="All">All</option>
                            <option value="Clients">Clients</option>
                            <option value="Leads">Leads</option>
                            <option value="Loans">Loans</option>
                        </select>
                    </td>
                    <td style="width:40px;">Enabled:</td>
                    <td style="width:100px;">
                        <select id="ddlEnabled" style="width: 80px; border: solid 1px #ccc;">
                            <option value="All">All</option>
                            <option value="Yes">Yes</option>
                            <option value="No">No</option>
                        </select>
                    </td>
                    <td>
                        <input id="btnDisplay" type="button" value="Display" onclick="btnDisplay_onclick()" class="Btn-66" />
                    </td>
                </tr>
            </table>

        </div>

        <div id="divToolBar" style="margin-top:10px;">
            <ul class="ToolStrip">
                <li><a id="aEnable" href="javascript:aEnable_onclick()">Enable</a><span>|</span></li>
                <li><a id="aDisable" href="javascript:aDisable_onclick()">Disable</a><span>|</span></li>
                <li><a id="aMakeDefault" href="javascript:aMakeDefault_onclick()">Make Default</a><span>|</span></li>
                <li><a id="aDelete" href="javascript:aDelete_onclick()">Delete</a></li>
            </ul>
        </div>

        <div id="divUserPipelineViewList" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridUserPipelineViewList" runat="server" EmptyDataText="There is no user pipeline view." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkSelected" type="checkbox" UserPipelineViewID="<%# Eval("UserPipelineViewID")%>" EnabledText="<%# Eval("EnabledText")%>" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('PipelineType')" style="text-decoration: underline;">Pipeline</a><span id="spOrderByFlag1" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("PipelineType")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('ViewName')" style="text-decoration: underline;">View Name</a><span id="spOrderByFlag2" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("ViewName")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('DefaultText')" style="text-decoration: underline;">Default</a><span id="spOrderByFlag3" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("DefaultText")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-Width="70px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('Enabled')" style="text-decoration: underline;">Enabled</a><span id="spOrderByFlag4" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("EnabledText")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>

    </div>
    </form>
</body>
</html>