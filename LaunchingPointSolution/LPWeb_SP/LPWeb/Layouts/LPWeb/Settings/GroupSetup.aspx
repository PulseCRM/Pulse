<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Group Setup" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" CodeBehind="GroupSetup.aspx.cs" Inherits="LPWeb.Settings.GroupSetup" %>

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
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            // add onchange event
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlGroupList").change(ddlGroupList_onchange);

            // set max-length
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtGroupDesc").maxlength(500);
        });

        // onchange for group name list
        function ddlGroupList_onchange() {

            var SelectedGroupID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlGroupList").val();
            window.location.href = "GroupSetup.aspx?GroupID=" + SelectedGroupID;
        }

        // check/decheck all
        function CheckAll2(CheckBox) {

            var GridName = $(CheckBox).parent().parent().parent().parent().attr("id");
            if (GridName == "ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList") {
                if (CheckBox.checked) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox").attr("checked", "true");
                }
                else {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox").attr("checked", "");
                }
            }
            else {

                if (CheckBox.checked) {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox").attr("checked", "true");
                }
                else {

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox").attr("checked", "");
                }
            }
        }

        // add member
        function AddMember() {

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
        function RemoveMember() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more group members.");
                return;
            }
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox[checked=true]").parent().parent().remove();
        }

        // add user
        function btnSelectAdd_onclick() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserSelectionList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more group members.");
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
        function btnSelectCancel_onclick() {

            // close modal
            $("#dialog-modal").dialog("close");
        }

        // do sth. before saving
        function BeforeSave() {

            var GroupMemberIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridUserList tr td :checkbox").each(function (i) {

                var UserID = $(this).attr("title");
//                alert("i:" + i);
                if (i == 0) {
                    GroupMemberIDs = UserID;
                }
                else {
                    GroupMemberIDs += "," + UserID;
                }
//                alert("GroupMemberIDs:" + GroupMemberIDs);
            });
//            alert(GroupMemberIDs);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnGroupMemberIDs").val(GroupMemberIDs);

            return true;
        }

        // show popup for enter group name
        function EnterGroupName() {

            // clear group name
            $("#txtGroupName").val("")

            // show modal
            $("#divNewGroupName").dialog({
                height: 160,
                width: 380,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        // create group
        function CreateGroup() {

            var GroupName = $.trim($("#txtGroupName").val());
            if (GroupName == "") {

                alert("Please enter Group Name.");
                return;
            }
            else {

                if (GroupName.length < 2) {

                    alert("Please enter more than 2 characters for Group Name.");
                    return;
                }

                var Regex = /^[0-9a-zA-Z_\-\.\s]{2,50}$/;
                var bIsValid = Regex.test(GroupName);
                if (bIsValid == false) {
                    alert("Use only letters[a-z or A-Z], numbers[0-9], dash and white space.");
                    return;
                }
            }

            // show waiting message
            $("#divMsg").show();

            $.ajax({
                type: "POST",
                url: "SettingsWebService.aspx/CreateGroup",
                data: '{sGroupName:"' + GroupName + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: CreateGroup_Callback,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert("Failed to create the user group."); $("#divNewGroupName").dialog("close"); }
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
            setTimeout("$('#lbMsg').text('Created user group successfully.');", 2000);

            // close modal dialog
            setTimeout("$('#divNewGroupName').dialog('close');window.location.href='GroupSetup.aspx?GroupID=" + data.d + "'", 4000);
        }

        function ClosePopupEnterGroupName() {

            // close modal
            $("#divNewGroupName").dialog("close");
        }
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div class="Heading">
        Group Setup</div>
    <div class="SplitLine">
    </div>
    <div class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 87px;">
                        Group Name
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlGroupList" runat="server" DataValueField="GroupID" DataTextField="GroupName"
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 39px;">
                        <lable><asp:CheckBox ID="chkEnabled" runat="server"></asp:CheckBox> Enabled</lable>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Group Description
                    </td>
                    <td style="padding-left: 15px; padding-top: 9px;">
                        <asp:TextBox ID="txtGroupDesc" runat="server" TextMode="MultiLine" Width="354px"
                            Height="44px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divTitleAndBtn" style="width: 400px; margin-top: 20px;">
            <table>
                <tr>
                    <td style="font-weight: bold;">
                        Group Members
                    </td>
                    <td style="padding-left: 30px; text-align: right;">
                        <a href="javascript:AddMember()" class="Link-Btn">Add Member</a><span style="padding-left: 8px;
                            padding-right: 6px;">|</span> <a href="javascript:RemoveMember()" class="Link-Btn">Remove</a>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divUserList" class="ColorGrid" style="margin-top: 3px; width: 400px;">
            <asp:GridView ID="gridUserList" runat="server" DataKeyNames="UserId" EmptyDataText="There is no members in this group."
                CellPadding="3" CssClass="GrayGrid" GridLines="None" AutoGenerateColumns="false">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="Checkbox2" type="checkbox" title="<%# Eval("UserID") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">
                &nbsp;</div>
        </div>
        <asp:HiddenField ID="hdnGroupMemberIDs" runat="server" />
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()"
                            OnClick="btnSave_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="Button2" type="button" value="New" class="Btn-66" onclick="EnterGroupName()" />
                    </td>
                    <td style="padding-left: 8px;">
                        <input id="Button3" type="button" value="Cancel" class="Btn-66" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="dialog-modal" title="Group Member Selection" style="display: none;">
            <br />
            <div style="width: 418px; height: 370px; overflow: auto;">
                <div id="divUserSelectionList" class="ColorGrid" style="width: 400px;">
                    <asp:GridView ID="gridUserSelectionList" runat="server" DataKeyNames="UserId" EmptyDataText="There is no user for selection."
                        CellPadding="3" CssClass="GrayGrid" GridLines="None" AutoGenerateColumns="false">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                <HeaderTemplate>
                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this)" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input id="Checkbox2" type="checkbox" title="<%# Eval("UserID") %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                            <asp:BoundField DataField="UserName" HeaderText="User Name" />
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
                <input id="btnSelectAdd" type="button" value="Add" class="Btn-66" onclick="return btnSelectAdd_onclick()" />&nbsp;&nbsp;
                <input id="btnSelectCancel" type="button" value="Cancel" class="Btn-66" onclick="return btnSelectCancel_onclick()" />
            </div>
        </div>
        <div id="divNewGroupName" title="Enter Group Name" style="display: none; margin: 10px;">
            <table>
                <tr>
                    <td style="width: 90px;">
                        Group Name:
                    </td>
                    <td>
                        <input id="txtGroupName" type="text" maxlength="50" style="width: 200px;" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="text-align: center;">
                <input id="btnCreate" type="button" value="Create" class="Btn-66" onclick="CreateGroup()" />&nbsp;&nbsp;
                <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="ClosePopupEnterGroupName()" />
            </div>
            <div id="divMsg" style="margin-top: 10px; text-align: center; display: none;">
                <img id="imgWaiting" src='../images/waiting.gif' style='position: relative; top: 2px;' />
                <label id="lbMsg" style='font-weight: bold;'>
                    Creating group...</label>
            </div>
        </div>
    </div>
</asp:Content>
