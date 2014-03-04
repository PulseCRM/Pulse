<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Regions Setup" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="Settings_RegionSetup" CodeBehind="RegionSetup.aspx.cs" %>

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
        var gridSelectionClientId = "#<%= gridDivisionSelectionList.ClientID %>";
        var gridExeSelectionClientId = "#gridExecutiveSelectionList";
        var gridClientId = "#<%= gridDivisionList.ClientID %>";
        var gridExeClientId = "#<%= gridExecutivesList.ClientID %>";

        var hdnDivMemberIDs = "#<%= hdnDivisionMemberIDs.ClientID %>";
        var hdnExeMemberIDs = "#<%= hdnExecutivesMembers.ClientID %>";

        var dialogDivi = "#dialog-modal";
        var dialogExe = "#dialog-modal1";

        $(document).ready(function () {
            // set max-length
            $("#<%= tbxDescription.ClientID %>").maxlength(500);
        });

        // check/decheck all
        function CheckAll2(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function AddMemberExecutives(divObj) {
            var showSelectionGrid = gridSelectionClientId;
            var showGrid = gridClientId;
            var showDialog = "#dialog-modal";

            if (divObj != null) {
                showSelectionGrid = gridExeSelectionClientId;
                showGrid = gridExeClientId;
                showDialog = "#" + divObj;

                var gId = $("#<%= ddlGroupAccess.ClientID %>").val();
                var rId = $("#<%= hfdRegionId.ClientID %>").val();
                $("#frmSelection").attr("src", "RegionSetupExecutiveSelection.aspx?groupid=" + gId + "&regionid=" + rId + "&nocash=" + parseInt(Math.random() * 100));
            }

            $(showDialog).dialog({
                height: 500,
                width: 480,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CheckExistMembers() {
            // uncheck

            $("#frmSelection").contents().find("#gridExecutiveSelectionList tr td :checkbox").attr("checked", "")
            //            // check exist members
            $(gridExeClientId + " tr td :checkbox[title]").each(function (i) {
                var UserID = $(this).attr("title");
                $("#frmSelection").contents().find("#gridExecutiveSelectionList tr td :checkbox[title=" + UserID + "]").attr("checked", "true");
            });
        }

        function AddMember(divObj) {
            var showSelectionGrid = gridSelectionClientId;
            var showGrid = gridClientId;
            var showDialog = "#dialog-modal";

            if (divObj != null) {
                showSelectionGrid = gridExeSelectionClientId;
                showGrid = gridExeClientId;
                showDialog = "#" + divObj;

                var gId = $("#<%= ddlGroupAccess.ClientID %>").val();
                var rId = $("#<%= hfdRegionId.ClientID %>").val();
                $("#frmSelection").attr("src", "RegionSetupExecutiveSelection.aspx?groupid=" + gId + "&regionid=" + rId + "&nocash=" + parseInt(Math.random() * 100));
            }

            // uncheck
            $(showSelectionGrid + " tr td :checkbox").attr("checked", "");

            // check exist members
            $(showGrid + " tr td :checkbox[title]").each(function (i) {
                var UserID = $(this).attr("title");
                $(showSelectionGrid + " tr td :checkbox[title=" + UserID + "]").attr("checked", "true");
            });

            $(showDialog).dialog({
                height: 500,
                width: 440,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function RemoveMember(divObj) {

            var operGrid = gridClientId;
            var operObj = "division";
            if (divObj != null) {
                operGrid = gridExeClientId;
                operObj = "executive";
            }

            if ($(operGrid + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more " + operObj + " members.");
                return;
            }
            $(operGrid + " tr td :checkbox[checked=true]").parent().parent().remove();
        }

        // add user
        function btnSelectAddExecutives_onclick(divObj) {

            var gridSelId = gridSelectionClientId;
            var gridId = gridClientId;
            var dialog = dialogDivi;
            if (divObj != null) {
                gridSelId = gridExeSelectionClientId;
                gridId = gridExeClientId;
                dialog = dialogExe;
            }

            var $frm = $("#frmSelection").contents();
            if ($frm.find(gridSelId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more group members.");
                return;
            }

            // no data, add columns
            if ($(gridId + " tr th").length == 0) {
                $(gridId + " tr:first").remove();
                $frm.find(gridSelId + " tr:first").clone().appendTo(gridId);
            }

            // clear user list
            $(gridId + " tr:not(:first)").remove();

            $frm.find(gridSelId + " tr td :checkbox[checked=true]").parent().parent().clone().appendTo(gridId + "");

            // uncheck
            $(gridId + " tr td :checkbox").attr("checked", "");

            // close modal
            $(dialog).dialog("close");
        }

        // add user
        function btnSelectAdd_onclick(divObj) {

            var gridSelId = gridSelectionClientId;
            var gridId = gridClientId;
            var dialog = dialogDivi;
            if (divObj != null) {
                gridSelId = gridExeSelectionClientId;
                gridId = gridExeClientId;
                dialog = dialogExe;
            }

            if ($(gridSelId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more group members.");
                return;
            }

            // no data, add columns
            if ($(gridId + " tr th").length == 0) {
                $(gridId + " tr:first").remove();
                $(gridSelId + " tr:first").clone().appendTo(gridId);
            }

            // clear user list
            $(gridId + " tr:not(:first)").remove();

            $(gridSelId + " tr td :checkbox[checked=true]").parent().parent().clone().appendTo(gridId + "");

            // uncheck
            $(gridId + " tr td :checkbox").attr("checked", "");

            // close modal
            $(dialog).dialog("close");
        }

        function btnSelectCancel_onclick(divObj) {
            var closeDialog = dialogDivi;
            if (divObj != null) {
                closeDialog = dialogExe;
            }
            // close modal
            $(closeDialog).dialog("close");
        }


        // create Region
        function CreateRegion() {
            var regionName = $.trim($("#txbRegionName").val());
            if (regionName == "") {
                alert("Please enter Region Name.");
                return;
            }
            else {

                if (regionName.length < 2) {

                    alert("Please enter more than 2 characters for region name.");
                    return;
                }

                var Regex = /^[0-9a-zA-Z_\-\.\s]{2,50}$/;
                var bIsValid = Regex.test(regionName);
                if (bIsValid == false) {
                    alert("Use only letters[a-z or A-Z], numbers[0-9], dash and white space.");
                    return;
                }
            }

            $("#divMsg").show();

            $.ajax({
                type: "POST",
                url: "SettingsWebService.aspx/CreateRegion",
                data: '{sRegionName:"' + regionName + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: CreateGroup_Callback,
                error: function (XMLHttpRequest, textStatus, errorThrown) { alert("Failed to create region."); $("#divRegionName").dialog("close"); }
            });
        }

        // callback of create region
        function CreateGroup_Callback(data) {

            if (data.d != "" && isID(data.d) == false) {
                $("#divMsg").hide();
                alert(data.d);
                return;
            }

            setTimeout("$('#lbMsg').text('Create region successfully.');", 2000);

            // after 3 seconds
            setTimeout("$('#divRegionName').dialog('close');window.location.href='RegionSetup.aspx?RegionID=" + data.d + "'", 4000);
        }

        function ShowRegionNamePanel() {

            // clear group name
            $("#txbRegionName").val("")

            // show modal
            $("#divRegionName").dialog({
                height: 200,
                width: 380,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseRegionNamePanel() {
            // close modal
            $("#divRegionName").dialog("close");
        }

        // do sth. before saving
        function BeforeSave() {
            if ($("#<%= ddlGroupAccess.ClientID %> option").val() == "0") {
                alert("Please select a group!");
                return false;
            }

            //设置division的ids
            $(hdnDivMemberIDs).val(GetSelectedMemberIds(gridClientId));

            if ($(hdnDivMemberIDs).val() == "") {
                var rv = confirm("There is no subordinate division defined. Are you sure?");
                if (!rv) {
                    return false;
                }
            }

            //设置executive的ids
            $(hdnExeMemberIDs).val(GetSelectedMemberIds(gridExeClientId));
            return true;
        }

        //generate memberids by grid
        function GetSelectedMemberIds(gridId) {

            var memberIds = "";
            $(gridId + " tr td :checkbox").each(function (i) {

                var itemId = $(this).attr("title");
                if (i == 0) {
                    memberIds = itemId;
                }
                else {
                    memberIds += "," + itemId;
                }
            });
            return memberIds;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <asp:HiddenField ID="hfdRegionId" runat="server" />
    <div class="Heading">
        Regions Setup</div>
    <div class="SplitLine">
    </div>
    <div class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 84px;">
                        Region Name
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:DropDownList ID="ddlRegions" runat="server" DataValueField="RegionId" DataTextField="Name"
                            AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlRegions_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 62px;">
                        Enabled
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:CheckBox ID="cbxEnabled" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="padding-top: 9px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 84px;">
                        Description
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="tbxDescription" runat="server" TextMode="MultiLine" Width="354px"
                            Height="44px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top: 9px;">
                        Group Access
                    </td>
                    <td style="padding-top: 9px; padding-left: 15px;">
                        <asp:DropDownList ID="ddlGroupAccess" runat="server" DataValueField="GroupId" DataTextField="GroupName"
                            Width="200px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div class="DashedBorder" style="margin-top: 5px;">
            &nbsp;</div>
        <div style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-left: 15px; vertical-align: top;">
                        <div id="divToolBar" style="margin-top: 13px;">
                            <ul class="ToolStrip">
                                <li><a id="aAdd" href="javascript:AddMember()">Add Divisions</a><span>|</span></li>
                                <li><a id="aRemove" href="javascript:RemoveMember()">Remove</a></li>
                            </ul>
                        </div>
                        <div id="divDivision" class="ColorGrid" style="margin-top: 2px; width: 302px;">
                            <asp:GridView ID="gridDivisionList" runat="server" AutoGenerateColumns="false" DataKeyNames="DivisionId"
                                EmptyDataText="There is no data to display." CellPadding="3" CssClass="GrayGrid"
                                GridLines="None">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this,'#<%= gridDivisionList.ClientID %>')" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="Checkbox2" type="checkbox" title="<%# Eval("DivisionId") %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            ID
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((GridViewRow)Container).RowIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Name" HeaderText="Division Name" />
                                    <asp:BoundField DataField="Desc" HeaderText="Description" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                        </div>
                        <asp:HiddenField ID="hdnDivisionMemberIDs" runat="server" />
                    </td>
                    <td style="padding-left: 15px; vertical-align: top;">
                        <div id="div1" style="margin-top: 13px;">
                            <ul class="ToolStrip">
                                <li><a id="a1" href="javascript:AddMemberExecutives('dialog-modal1')">Add Executives</a><span>|</span></li>
                                <li><a id="a2" href="javascript:RemoveMember('dialog-modal1')">Remove</a></li>
                            </ul>
                        </div>
                        <div id="div2" class="ColorGrid" style="margin-top: 2px; width: 300px;">
                            <asp:GridView ID="gridExecutivesList" runat="server" AutoGenerateColumns="false"
                                DataKeyNames="UserId" EmptyDataText="There is no data to display." CellPadding="3"
                                CssClass="GrayGrid" GridLines="None">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this,'#<%= gridExecutivesList.ClientID %>')" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="Checkbox2" type="checkbox" title="<%# Eval("UserID") %>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            ID
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# ((GridViewRow)Container).RowIndex + 1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            Full Name
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# Eval("LastName") %>.<%# Eval("FirstName") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                        </div>
                        <asp:HiddenField ID="hdnExecutivesMembers" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <!--Buttons-->
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()"
                            OnClick="btnSave_Click" />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnSaveAs" runat="server" OnClientClick="ShowRegionNamePanel(); return false;"
                            Text="New" CssClass="Btn-66" />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divRegionName" title="Enter Region Name" style="display: none; margin: 10px;">
            <table>
                <tr>
                    <td style="width: 90px;">
                        Region Name:
                    </td>
                    <td>
                        <input id="txbRegionName" type="text" maxlength="50" style="width: 200px;" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="text-align: center;">
                <input id="btnCreate" type="button" value="Create" class="Btn-66" onclick="CreateRegion()" />&nbsp;&nbsp;
                <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="CloseRegionNamePanel();" />
            </div>
            <div id="divMsg" style="margin-top: 10px; text-align: center; display: none;">
                <img id="imgWaiting" src='../images/waiting.gif' style='position: relative; top: 2px;' />
                <label id="lbMsg" style='font-weight: bold;'>
                    Please wait...</label>
            </div>
        </div>
        <div id="divPopupDivisionMemberSelection" style="display: none; cursor: default;
            margin: 20px;">
            <div class="ModuleTitle" style="color: #818892;">
                Division Members Selection</div>
            <div id="divx" style="width: 468px; height: 300px; overflow: auto; margin-top: 10px;">
            </div>
        </div>
        <div id="dialog-modal" title="Division Member Selection" style="display: none;">
            <br />
            <div style="width: 418px; height: 370px; overflow: auto;">
                <div id="divDivisionSelectionList" class="ColorGrid" style="width: 400px;">
                    <asp:GridView ID="gridDivisionSelectionList" runat="server" DataKeyNames="DivisionID"
                        AutoGenerateColumns="false" EmptyDataText="There is no division for selection."
                        CellPadding="3" CssClass="GrayGrid" GridLines="None">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                <HeaderTemplate>
                                    <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this,'#<%= gridDivisionSelectionList.ClientID %>')" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input id="Checkbox2" type="checkbox" title="<%# Eval("DivisionID") %>" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                <HeaderTemplate>
                                    ID
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((GridViewRow)Container).RowIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Name" HeaderText="Divison Name" />
                            <asp:BoundField DataField="Desc" HeaderText="Description" />
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
        <div id="divExecutiveSelectionList" style="display: none; cursor: default; margin: 20px;">
            <div class="ModuleTitle" style="color: #818892;">
                Executives Members Selection</div>
            <div id="div4" style="width: 468px; height: 300px; overflow: auto; margin-top: 10px;">
            </div>
        </div>
        <div id="dialog-modal1" title="Executive Members Selection" style="display: none;">
            <br />
            <%--            <div id="div6" class="ColorGrid" style="width: 450px;">
                <asp:GridView ID="gridExecutiveSelectionList" runat="server" DataKeyNames="UserId"
                    AutoGenerateColumns="false" EmptyDataText="There is no executive for selection.">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll2(this,'#<%= gridExecutiveSelectionList.ClientID %>')" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Checkbox2" type="checkbox" title="<%# Eval("UserId") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                ID
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# ((GridViewRow)Container).RowIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Full Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("LastName") %>.<%# Eval("FirstName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="UserName" HeaderText="User Name" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">
                    &nbsp;</div>
                    </div>--%>
            <iframe id="frmSelection" scrolling="yes" style="width: 418px; height: 370px" frameborder="no"
                border="0"></iframe>
            <div class="SplitLine" style="position: absolute; left: -8px; top: 380px; width: 446px;">
                &nbsp;
            </div>
            <div style="margin-top: 20px; text-align: center;">
                <input id="Button1" type="button" value="Add" class="Btn-66" onclick="return btnSelectAddExecutives_onclick('dialog-modal1')" />&nbsp;&nbsp;
                <input id="Button2" type="button" value="Cancel" class="Btn-66" onclick="return btnSelectCancel_onclick('dialog-modal1')" />
            </div>
        </div>
    </div>
</asp:Content>
