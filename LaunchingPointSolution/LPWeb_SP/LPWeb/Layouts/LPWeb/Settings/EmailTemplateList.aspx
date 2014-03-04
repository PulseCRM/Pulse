<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Email Template List" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" CodeBehind="EmailTemplateList.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.EmailTemplateList" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="head" runat="server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            DrawTab();

        });

        // check/decheck all
//        function CheckAll(CheckBox) {
//            CheckAllClicked(CheckBox);
//            if (CheckBox.checked) {
//                $("#" + '<%=gridList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
//            }
//            else {
//                $("#" + '<%=gridList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
//            }
        //        }

        function CheckAll(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function IsRowSelected(args) {
            if ($("#" + '<%=gridList.ClientID %>' + " tr td :checkbox[checked=true]").length == 0) {
                alert("No record has been selected.");
            }
            else {
                // give confirm info
                if ("disable" == args) {
                    return confirm('Are you sure you want to disable the selected email template(s)?');
                }
                else if ("enable" == args) {
                    return confirm('Are you sure you want to enable the selected email template(s)?');
                }
                else if ("delete" == args) {
                    return confirm('This operation is not reversible. Are you sure you want to continue?');
                }
            }
            return false;
        }

        //#region neo

        //#region Create/Update Email Template

        function ShowDialog_AddEmailTemplate() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "EmailTemplateAddParent.aspx?sid=" + sid;
        }

        function ShowDialog_EditEmailTemplate() {

            var SelectedCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No Email Template was selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one Email Template can be selected.");
                return;
            }

            var EmailTemplateID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr:not(:first) td :checkbox:checked").parent().attr("title");
            //alert(EmailTemplateID);

            UpdateEmailTemplate(EmailTemplateID);
        }

        function UpdateEmailTemplate(EmailTemplateID) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "EmailTemplateEditParent.aspx?sid=" + sid + "&EmailTemplateID=" + EmailTemplateID;
        }

        //#endregion

        //#region Show Dialog

        function ShowDialog(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divEmailTemplateSetup").attr("title", Title);

            $("#ifrEmailTemplateSetup").attr("src", "");
            $("#ifrEmailTemplateSetup").attr("src", iFrameSrc);
            $("#ifrEmailTemplateSetup").width(iFrameWidth);
            $("#ifrEmailTemplateSetup").height(iFrameHeight);

            // show modal
            $("#divEmailTemplateSetup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) { $("#divEmailTemplateSetup").dialog("destroy"); }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog() {

            $("#divEmailTemplateSetup").dialog("close");
        }

        //#endregion

        //#endregion

// ]]>
    </script>
    <script type="text/javascript">
        Array.prototype.remove = function (s) {
            var nIndex = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i] == s) {
                    nIndex = i;
                    break;
                }
            }
            if (nIndex != -1) {
                this.splice(nIndex, 1);
                return true;
            }
            else
                return false;
        }

        //function CheckAllClicked(me, areaID, hiAllIDs, hiSelectedIDs)
        function CheckAllClicked(me) {
            var areaID = '<%=gridList.ClientID %>';
            var hiAllIDs = '<%=hiAllIds.ClientID %>';
            var hiSelectedIDs = '<%=hiCheckedIds.ClientID %>';
            var bCheck = $(me).attr('checked');
            if (bCheck) {
                // copy all ids to selected id holder
                $('#' + hiSelectedIDs).val($('#' + hiAllIDs).val());
            }
            else
                $('#' + hiSelectedIDs).val('');
            $('input:checkbox', $('#' + areaID) + '.CheckBoxColumn').each(function () { $(this).attr('checked', bCheck); });
        }

        //function CheckBoxClicked(me, ckAllID, hiAllIDs, hiSelectedIDs, id)
        function CheckBoxClicked(me, hiAllIDs, hiSelectedIDs, id) {
            var ckAllID = 'Checkbox1';
            var sAllIDs = $('#' + hiAllIDs).val();
            var sSelectedIDs = $('#' + hiSelectedIDs).val();
            var allIDs = new Array();
            var selectedIDs = new Array();
            if (sAllIDs.length > 0)
                allIDs = sAllIDs.split(',');

            if (sSelectedIDs.length > 0)
                selectedIDs = sSelectedIDs.split(',');

            if ($(me).attr('checked'))
                selectedIDs.push(id);
            else
                selectedIDs.remove(id);

            // set the CheckAll check box checked status
            // $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);

            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');
        }
    </script>
    <script type="text/javascript">

        function deleteBtnClicked() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (sIds.length <= 0) {
                alert("No record has been selected.");
                return false;
            }
            else {
                var arrIds = sIds.split(",");
                for (var n = 0; n < arrIds.length; n++) {
                    if (isRuleReferenced(arrIds[n]))
                        return confirm('The selected record(s) has been referenced by others.'
                    + 'Deleting the records will cause the references to be removed. Are you sure you want to continue? ');
                }
                return confirm('This operation is not reversible. Are you sure you want to continue?');
            }
        }

        function isRuleReferenced(sRid) {
            // check reference
            var sRef = $("#" + "<%=hiReferenced.ClientID %>").val();
            var arrRef = sRef.split(";");
            for (var i = 0; i < arrRef.length; i++) {
                var arrTemp = arrRef[i].split(":");
                if (arrTemp.length == 2 && arrTemp[0] == sRid) {
                    var nCount = new Number(arrTemp[1]);
                    if (!isNaN(nCount))
                        if (nCount > 0)
                            return true;
                }
            }
            return false;
        }

        function GoToEmailSkinSetup(skinid) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            window.location.href = "EmailSkinEdit.aspx?sid=" + sid + "&EmailSkinID=" + skinid;
        }

        //#region Clone EmailTemplate

        function aClone_onclick() {

            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("No Email Template has been selected.");
                return;
            }
            else if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr:not(:first) td :checkbox:checked").length > 1) {

                alert("Only one Email Template can be selected.");
                return;
            }

            // show waiting dialog
            ShowWaitingDialog("Cloning Email Template...");

            // selected EmailTemplate ids
            var EmailTemplateID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr:not(:first) td :checkbox:checked").parent().attr("title");
            //alert(EmailTemplateID);

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("CloneEmailTemplateAjax.aspx?sid=" + sid + "&EmailTemplateID=" + EmailTemplateID, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        $("body").unblock();
                        return;
                    }

                    $("body").unblock();

                    window.location.href = window.location.href;

                }, 2000);

            });
        }

        //#endregion

        //#region show/close waiting

        function ShowWaitingDialog(WaitingMsg) {

            $("#WaitingMsg").text(WaitingMsg);
            $("body").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

        //#endregion

    </script>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="MainArea" runat="server">
    
    <div class="JTab" style="margin-top:10px;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <ul>
                            <li id="current"><a href="EmailTemplateList.aspx"><span>Email Templates</span></a></li>
                            <li><a href="EmailSkinList.aspx"><span>Email Skins</span></a></li>
                            
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <div id="TabBody" style="margin-bottom:10px; padding-bottom:10px;">
            <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
            <div id="TabLine2" class="TabRightLine">&nbsp;</div>
            <div class="TabContent">
                    
                <asp:UpdatePanel ID="updatePanel" runat="server">
                    <ContentTemplate>
                        <div style="padding-left: 10px; padding-right: 10px;">
                            <div id="divToolBar" style="margin-top: 13px;">
                                <table cellpadding="0" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 40px;">
                                            <asp:DropDownList ID="ddlAlphabet" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlAlphabet_SelectedIndexChanged">
                                                <asp:ListItem Value=""></asp:ListItem>
                                                <asp:ListItem Value="A">A</asp:ListItem>
                                                <asp:ListItem Value="B">B</asp:ListItem>
                                                <asp:ListItem Value="C">C</asp:ListItem>
                                                <asp:ListItem Value="D">D</asp:ListItem>
                                                <asp:ListItem Value="E">E</asp:ListItem>
                                                <asp:ListItem Value="F">F</asp:ListItem>
                                                <asp:ListItem Value="G">G</asp:ListItem>
                                                <asp:ListItem Value="H">H</asp:ListItem>
                                                <asp:ListItem Value="I">I</asp:ListItem>
                                                <asp:ListItem Value="J">J</asp:ListItem>
                                                <asp:ListItem Value="K">K</asp:ListItem>
                                                <asp:ListItem Value="L">L</asp:ListItem>
                                                <asp:ListItem Value="M">M</asp:ListItem>
                                                <asp:ListItem Value="N">N</asp:ListItem>
                                                <asp:ListItem Value="O">O</asp:ListItem>
                                                <asp:ListItem Value="P">P</asp:ListItem>
                                                <asp:ListItem Value="Q">Q</asp:ListItem>
                                                <asp:ListItem Value="R">R</asp:ListItem>
                                                <asp:ListItem Value="S">S</asp:ListItem>
                                                <asp:ListItem Value="T">T</asp:ListItem>
                                                <asp:ListItem Value="U">U</asp:ListItem>
                                                <asp:ListItem Value="V">V</asp:ListItem>
                                                <asp:ListItem Value="W">W</asp:ListItem>
                                                <asp:ListItem Value="X">X</asp:ListItem>
                                                <asp:ListItem Value="Y">Y</asp:ListItem>
                                                <asp:ListItem Value="Z">Z</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 350px;">
                                            <ul class="ToolStrip">
                                                <li><a href="javascript:ShowDialog_AddEmailTemplate()">Create</a><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="lbtnDisable" runat="server" OnClientClick="return IsRowSelected('disable');"
                                                        OnClick="lbtnDisable_Click">Disable</asp:LinkButton><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="lbtnEnable" runat="server" OnClientClick="return IsRowSelected('enable');"
                                                        OnClick="lbtnEnable_Click">Enable</asp:LinkButton><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" OnClientClick="return deleteBtnClicked();"
                                                        OnClick="lbtnDelete_Click">Delete</asp:LinkButton><span>|</span></li>
                                                <li><a href="javascript:ShowDialog_EditEmailTemplate()">Update</a><span>|</span></li>
                                                <li><a id="aClone" href="javascript:aClone_onclick()">Clone</a></li>
                                            </ul>
                                        </td>
                                        <td style="text-align: right;">
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
                            <div style="display: none;">
                                <asp:LinkButton ID="lbtnEmpty" runat="server" OnClick="lbtnEmpty_Click"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnEmpty2" runat="server" OnClick="lbtnEmpty2_Click"></asp:LinkButton>
                            </div>
                        </div>
                        <div id="div1" class="ColorGrid" style="margin-top: 5px;">
                            <asp:GridView ID="gridList" runat="server" DataKeyNames="TemplEmailId" EmptyDataText="There is no email template in database."
                                AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound" CellPadding="3" OnPreRender="gridList_PreRender"
                                CssClass="GrayGrid" GridLines="None" OnSorting="gridList_Sorting" AllowSorting="true">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <AlternatingRowStyle CssClass="EvenRow" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />

                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" ToolTip='<%# Eval("TemplEmailId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                        <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email Template Name" SortExpression="Name">
                                        <ItemTemplate>
                                            <a href="javascript:UpdateEmailTemplate('<%# Eval("TemplEmailId") %>')"><%# Eval("Name") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="Desc" ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Desc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email Skin" SortExpression="EmailSkinName"  ItemStyle-Width="150px">
                                        <ItemTemplate>
                                            <a href="javascript:GoToEmailSkinSetup('<%# Eval("EmailSkinId") %>')"><%# Eval("EmailSkinName") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled"  ItemStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnabled" runat="server" Text='<%# Eval("Enabled").ToString().ToLower() == "true" ? "Yes" : "No" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">
                                &nbsp;</div>
                            <asp:HiddenField ID="hiAllIds" runat="server" />
                            <asp:HiddenField ID="hiCheckedIds" runat="server" />
                            <asp:HiddenField ID="hiReferenced" runat="server" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="divEmailTemplateSetup" title="Email Template Setup" style="display: none;">
                    <iframe id="ifrEmailTemplateSetup" frameborder="0" scrolling="no" width="820px" height="740px"></iframe>
                </div>
    
            </div>
        </div>
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