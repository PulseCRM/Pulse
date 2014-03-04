<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Division Setup" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="True" Inherits="Settings_DivisionSetup" CodeBehind="DivisionSetup.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            // add onchange event
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").change(ddlDivision_onchange);
        });

        function ddlDivision_onchange() {

            var SelectedGroupID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlDivision").val();
            window.location.href = "DivisionSetup.aspx?DivisionID=" + SelectedGroupID;
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

        function AddMember() {

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchSelectionList tr td input[type=checkbox]").attr("checked", "");

            // check exist members
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td input[type=checkbox][title]").each(function (i) {
                var BranchID = $(this).attr("title");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchSelectionList tr td input[type=checkbox][title=" + BranchID + "]").attr("checked", "true");
            });

            // show modal dialog
            $("#divPopupBranchMemberSelection").dialog({
                height: 480,
                width: 440,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")

        }

        function RemoveMember() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox[checked=true]").parent().parent().remove();
        }

        // add branch
        function btnSelectAdd_onclick() {
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchSelectionList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }

            // no data, add columns
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr th").length == 0) {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:first").remove();
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchSelectionList tr:first").clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList");
            }

            // clear user list
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr:not(:first)").remove();

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchSelectionList tr td :checkbox[checked=true]").parent().parent().clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList");

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "");


            // close modal
            $("#divPopupBranchMemberSelection").dialog("close");
        }

        function btnSelectCancel_onclick() {

            $("#divPopupBranchMemberSelection").dialog("close");
        }

        // add member
        function AddExecutiveMember() {

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox").attr("checked", "");

            // check exist members
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[title]").each(function (i) {
                var UserID = $(this).attr("title");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox[title=" + UserID + "]").attr("checked", "true");
            });

            // show modal dialog
            $("#dialog-modal").dialog({
                height: 480,
                width: 440,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // remove member
        function RemoveExecutiveMember() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[checked=true]").parent().parent().remove();
        }

        // add user
        function btnExecutiveSelectAdd_onclick() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }

            // no data, add columns
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr th").length == 0) {
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr:first").remove();
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr:first").clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList");

            }

            // clear user list
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr:not(:first)").remove();

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox[checked=true]").parent().parent().clone().appendTo("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList");

            // uncheck
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox").attr("checked", "");

            // close modal
            $("#dialog-modal").dialog("close");
        }

        // popup cancel
        function btnExecutiveSelectCancel_onclick() {

            // close modal
            $("#dialog-modal").dialog("close");
        }

        function BeforeSave() {

            var BranchMemberIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").each(function (i) {

                var BranchID = $(this).attr("title");
                if (i == 0) {
                    BranchMemberIDs = BranchID;
                }
                else {
                    BranchMemberIDs += "," + BranchID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnBranchMemberIDs").val(BranchMemberIDs);

            if (BranchMemberIDs == "") {
                var rv = confirm("There is no subordinate branch defined. Are you sure?");
                if (!rv) {
                    return false;
                }
            }

            var Managers = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox").each(function (i) {

                var UserID = $(this).attr("title");
                if (i == 0) {
                    Managers = UserID;
                }
                else {
                    Managers += "," + UserID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnExecutiveIDs").val(Managers);
            return true;
        }

        // show popup for enter group name
        function EnterDivisionName() {

            // clear division name
            $("#txbDivisionName").val("")

            // show modal
            $("#divDivisionName").dialog({
                height: 160,
                width: 380,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // create group
        function CreateDivision() {

            var DivisionName = $.trim($("#txbDivisionName").val());
            if (DivisionName == "") {

                alert("Please enter Division Name.");
                return;
            }
            else {

                if (DivisionName.length < 2) {

                    alert("Please enter more than 2 characters for group name.");
                    return;
                }

                var Regex = /^[0-9a-zA-Z_\-\.\s]{2,50}$/;
                var bIsValid = Regex.test(DivisionName);
                if (bIsValid == false) {
                    alert("Use only letters[a-z or A-Z], numbers[0-9], dash and white space.");
                    return;
                }
            }

            // show waiting message
            $("#divMsg").show();

            $.ajax({
                type: "POST",
                url: "SettingsWebService.aspx/CreateDivision",
                data: '{sDivisionName:"' + DivisionName + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: CreateGroup_Callback,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert("Failed to create division."); $("#divDivisionName").dialog("close"); }
            });
        }

        // callback of create group
        function CreateGroup_Callback(data) {

            if (data.d != "" && isID(data.d) == false) {
                $("#divMsg").hide();    // hide waiting message
                alert(data.d);
                return;
            }

            // show success message
            setTimeout("$('#lbMsg').text('Create division successfully.');", 2000);

            // close modal dialog
            setTimeout("$('#divDivisionName').dialog('close');window.location.href='DivisionSetup.aspx?DivisionID=" + data.d + "'", 4000);

        }

        function ClosePopupDivisionName() {

            // close modal
            $("#divDivisionName").dialog("close");
        } 

// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div class="Heading">
        Division Setup</div>
    <div class="SplitLine">
    </div>
    <div class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 82px;">
                        Division Name
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlDivision" Width="177px" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 62px;">
                        Enabled
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:CheckBox ID="ckbEnabled" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-top: 9px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 82px;">
                        Division Description
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="txbDescription" runat="server" Height="42px" Rows="2" TextMode="MultiLine"
                            Width="466px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Group Access
                    </td>
                    <td style="padding-top: 9px; padding-left: 15px;">
                        <asp:DropDownList ID="ddlGroupAccess" Width="177px" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="DashedBorder" style="margin-top: 15px;">
            &nbsp;</div>
        <div style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-left: 15px; vertical-align: top;">
                        <div id="divToolBar" style="margin-top: 13px;">
                            <ul class="ToolStrip">
                                <li><a id="aAdd" href="javascript:AddMember()">Add Branch</a><span>|</span></li>
                                <li><a id="aRemove" href="javascript:RemoveMember()">Remove</a></li>
                            </ul>
                        </div>
                        <div id="divUserList" class="ColorGrid" style="margin-top: 2px; width: 302px;">
                            <asp:GridView ID="gridBranchList" runat="server" DataKeyNames="BranchID" EmptyDataText="There is no data to display."
                                CellPadding="3" CssClass="GrayGrid" GridLines="None" AutoGenerateColumns="false">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="Checkbox2" type="checkbox" title="<%# Eval("BranchID") %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Branch Name" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                        </div>
                    </td>
                    <td style="padding-left: 15px; vertical-align: top;">
                        <div id="div1" style="margin-top: 13px;">
                            <ul class="ToolStrip">
                                <li><a id="a1" href="javascript:AddExecutiveMember()">Add Executive</a><span>|</span></li>
                                <li><a id="a2" href="javascript:RemoveExecutiveMember()">Remove</a></li>
                            </ul>
                        </div>
                        <div id="div2" class="ColorGrid" style="margin-top: 2px; width: 300px;">
                            <asp:GridView ID="gridUserList" runat="server" DataKeyNames="UserId" EmptyDataText="There is no data to display."
                                CellPadding="3" CssClass="GrayGrid" GridLines="None" AutoGenerateColumns="false">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="Checkbox2" type="checkbox" title="<%# Eval("UserID") %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                    <asp:BoundField DataField="FullName" HeaderText="Full name" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return BeforeSave()"
                            CssClass="Btn-66" OnClick="btnSave_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnSaveAs" runat="server" OnClientClick="EnterDivisionName(); return false;"
                            Text="New" CssClass="Btn-66" />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divDivisionName" title="Enter Division Name" style="display: none; cursor: default;
        margin: 10px;">
        <table>
            <tr>
                <td style="width: 90px;">
                    <font style="font-weight: bold; color: #818892;">Division Name:</font>
                </td>
                <td>
                    <input id="txbDivisionName" type="text" maxlength="50" style="width: 200px;" />
                </td>
            </tr>
        </table>
        <br />
        <div style="text-align: center;">
            <input id="btnCreate" type="button" value="Create" class="Btn-66" onclick="CreateDivision()" />&nbsp;&nbsp;
            <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="ClosePopupDivisionName(); return false;" />
        </div>
        <div id="divMsg" style="margin-top: 10px; text-align: center; display: none;">
            <img id="imgWaiting" src='../images/waiting.gif' style='position: relative; top: 2px;' />
            <label id="lbMsg" style='font-weight: bold;'>
                Please wait...</label>
        </div>
    </div>
    <asp:HiddenField ID="hdnBranchMemberIDs" runat="server" />
    <asp:HiddenField ID="hdnExecutiveIDs" runat="server" />
    <div id="divPopupBranchMemberSelection" title="Branch Member Selection" style="display: none;
        cursor: default; margin: 20px;">
        <div class="ModuleTitle" style="color: #818892;">
            Branch Members Selection</div>
        <div id="divx" style="width: 318px; height: 300px; overflow: auto; margin-top: 10px;">
            <div id="divBranchSelectionList" class="ColorGrid" style="width: 300px;">
                <asp:GridView ID="gridBranchSelectionList" runat="server" DataKeyNames="BranchID"
                    EmptyDataText="There is no data to display." Width="300px" CellPadding="3" CssClass="GrayGrid"
                    GridLines="None" AutoGenerateColumns="false">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Checkbox2" type="checkbox" title="<%# Eval("BranchID") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Name" HeaderText="Branch Name" ItemStyle-HorizontalAlign="Left"
                            HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="#818892" ItemStyle-ForeColor="#818892" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">
                    &nbsp;</div>
            </div>
        </div>
        <div style="margin-top: 15px;">
            <input id="btnSelectAdd" type="button" value="Add" class="Btn-66" onclick="return btnSelectAdd_onclick()" />&nbsp;&nbsp;
            <input id="btnSelectCancel" type="button" value="Cancel" class="Btn-66" onclick="return btnSelectCancel_onclick()" />
        </div>
    </div>
    <div id="dialog-modal" title="Division Executive Selection" style="display: none;">
        <br />
        <div style="width: 418px; height: 370px; overflow: auto;">
            <div id="divUserSelectionList" class="ColorGrid" style="width: 400px;">
                <asp:GridView ID="gridUserSelectionList" runat="server" DataKeyNames="UserId" EmptyDataText="There is no data to display."
                    CellPadding="3" CssClass="GrayGrid" GridLines="None" AutoGenerateColumns="false">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Checkbox2" type="checkbox" title="<%# Eval("UserID") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                        <asp:BoundField DataField="FullName" HeaderText="Full name" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">
                    &nbsp;</div>
            </div>
        </div>
        <div class="SplitLine" style="position: absolute; left: -8px; top: 380px; width: 446px;">
            &nbsp;
        </div>
        <div style="margin-top: 20px; text-align: center;">
            <input id="Button1" type="button" value="Add" class="Btn-66" onclick="return btnExecutiveSelectAdd_onclick()" />&nbsp;&nbsp;
            <input id="Button2" type="button" value="Cancel" class="Btn-66" onclick="return btnExecutiveSelectCancel_onclick()" />
        </div>
    </div>
</asp:Content>
