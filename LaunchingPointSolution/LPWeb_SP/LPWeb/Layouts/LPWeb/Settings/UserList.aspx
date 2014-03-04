<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="User List View" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="LPWeb.Settings.UserList" CodeBehind="UserList.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        Array.prototype.remove = function (b) {
            var index = -1;
            for(var i = 0; i < this.length; i++)
            {
                if (this[i] == b)
                {
                    index = i;
                    break;
                }
            }
            if (index >= 0) {
                this.splice(index, 1);
                return true;
            }
            return false;
        }; 

        var nSign = 0;  // indicate whether non-LO selected

        $(document).ready(function () {
            initUserSetupWin();
            initUserGoalsSetupWin();
            initReassignUserWin();
        });

        var bRefreshList = false;
        function initUserSetupWin()
        {
            $('#dialogUserSetup').dialog({
                modal: true,
                autoOpen: false,
                title: 'User Setup',
                width: 880,
                height: 680,
			    resizable: false,
                close: clearUserSetupWin
            });
        }
        function showUserSetupWin(mode, uid)
        {
            var f = document.getElementById('iframeUS');
            if (null == mode || "" == mode)
                mode = "0";
            if (null == uid)
                uid = "";
            f.src = "UserSetup.aspx?mode=" + mode + "&uid=" + uid + "&t=" + Math.random().toString();
            $('#dialogUserSetup').dialog('open');
        }
        function closeUserSetupWin(bRefresh, bReset)
        {
            $('#dialogUserSetup').dialog('close');
            if (bRefresh === true)
                RefreshList(bReset);
        }
        function clearUserSetupWin(bR)
        {
            var f = document.getElementById('iframeUS');
            f.src="about:blank";
        }

        function RefreshList(bR) {
            if (bR === true)
                <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>
            else
                <%=this.ClientScript.GetPostBackEventReference(this.lbtnRefreshList, null) %>
        }
        function onUpdateBtnClick()
        {
            var sSelIDs = $("#" + '<%=hiSelectedIds.ClientID %>').val();
            var arrIds = new Array();
            if (sSelIDs.length > 0)
                arrIds = sSelIDs.split(',');

            if (arrIds.length == 0)
                alert("No record has been selected.");
            else if(arrIds.length == 1)
                showUserSetupWin("1", arrIds[0]);
            else
                alert("Only one record can be selected for this operation.");
        }
// ]]>
    </script>
    <script type="text/javascript">
// <![CDATA[
        // check/decheck all
        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#" + '<%=gridUserList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
                $("#" + '<%=hiSelectedIds.ClientID %>').val($("#" + '<%=hiAllIds.ClientID %>').val());
                // set these variables for Set User Goals page
                $("#" + '<%=hiSelectedLOIds.ClientID %>').val($("#" + '<%=hiAllLOIds.ClientID %>').val());
                nSign = $("#" + '<%=hiNonLOUserIds.ClientID %>').val();
            }
            else {
                $("#" + '<%=gridUserList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
                $("#" + '<%=hiSelectedIds.ClientID %>').val("");
                // set these variables for Set User Goals page
                $("#" + '<%=hiSelectedLOIds.ClientID %>').val("");
                nSign = 0;
            }
        }
        function IsRowSelected(args) {
            if ($("#" + '<%=gridUserList.ClientID %>' + " tr td :checkbox[checked=true]").length == 0) {
                ShowMsg("noRowSelected");
            }
            else {
                // give confirm info
                if ("disable" == args) {
                    return confirm('Are you sure you want to disable the selected user accounts?');
                }
                else if ("delete" == args) {
                    return confirm('Are you sure you want to delete the selected user accounts?');
                }
            }
            return false;
        }
        function ShowMsg(args) {
            if ("noRowSelected" == args)
                alert("No record has been selected.");
            else if ("noUserSelected" == args)
                alert("No record has been selected.");
        }
// ]]>
    </script>
    <script type="text/javascript">       
// <![CDATA[
        function onUserSelected(bIsLO, bIsSelected, sUID) {
            var sSelIDs = $("#" + '<%=hiSelectedIds.ClientID %>').val();
            var sSelLOIDs = $("#" + '<%=hiSelectedLOIds.ClientID %>').val();

            var arrSelectedUId = new Array();
            var arrSelectedLOID = new Array();

            if (sSelIDs.length > 0)
                arrSelectedUId = sSelIDs.split(',');
            if (sSelLOIDs.length > 0)
                arrSelectedLOID = sSelLOIDs.split(',');

            // Start: set sign for update
            if (bIsSelected)
                arrSelectedUId.push(sUID);
            else
                arrSelectedUId.remove(sUID);

            if (arrSelectedUId.length > 0)
                $("#" + '<%=hiSelectedIds.ClientID %>').val(arrSelectedUId);
            else
                $("#" + '<%=hiSelectedIds.ClientID %>').val('');
            // End: set sign for update

            // Start: set sign for usergoles setup
            if (bIsLO) {
                if (bIsSelected)
                    arrSelectedLOID.push(sUID);
                else
                    arrSelectedLOID.remove(sUID);
            }
            else {
                if (bIsSelected)
                    nSign += 1;
                else
                    nSign -= 1;
            }
            if (arrSelectedLOID.length > 0)
                $("#" + '<%=hiSelectedLOIds.ClientID %>').val(arrSelectedLOID);
            else
                $("#" + '<%=hiSelectedLOIds.ClientID %>').val('');
            // End: set sign for usergoles setup
        }
        function onUserGoalsClick() {
            var sSelLOIDs = $("#" + '<%=hiSelectedLOIds.ClientID %>').val();
            var arrLOIds = new Array();
            if (sSelLOIDs.length > 0)
                arrLOIds = sSelLOIDs.split(',');

            if (arrLOIds.length <= 0 && nSign <= 0) {
                // if no row selected give alert message
                alert("No record has been selected.");
                return false;
            }
            else {
                // check user's role 
                // if not loan officer, give alert message and return false
                // else show User Goals Setup window
                if (nSign > 0) {
                    alert("Only Loan Officers can be selected for this operation.");
                    return false;
                }
                else {
                    showUserGoalsSetupWin(arrLOIds);
                }
            }
            return false;
        }
        function initUserGoalsSetupWin() {
            $('#dialogUserGoalsSetup').dialog({
                modal: true,
                autoOpen: false,
                title: 'User Goals Setup',
                width: 950,
                height: 620,
                resizable: false,
                close: clearUserGoalsSetupWin
            });
        }
        function showUserGoalsSetupWin(allIDs) {
            var f = document.getElementById('iframeUG');
            f.src = "UserGoalsSetup.aspx?ids=" + allIDs + "&t=" + Math.random().toString();
            $('#dialogUserGoalsSetup').dialog('open');
        }
        function clearUserGoalsSetupWin(bR) {
            var f = document.getElementById('iframeUG');
            f.src = "about:blank";
        }
        function closeUserGoalsSetupWin() {
            $('#dialogUserGoalsSetup').dialog('close');
        }
// ]]>
    </script>
    <script type="text/javascript">
        function UserInfo(sID, sLoanCount, sContactCount) {
            this.ID = sID;
            this.LoanCount = sLoanCount;
            this.ContactCount = sContactCount;
        }
        function GetUserInfoById(sId) {
            var sUserInfo = $("#" + "<%=hiUserInfo.ClientID %>").val();
            var arrUserInfo = sUserInfo.split(";");
            for (var i = 0; i < arrUserInfo.length; i++) {
                var arrTemp = arrUserInfo[i].split(":");
                if (arrTemp.length == 3 && arrTemp[0] == sId) {
                    return new UserInfo(arrTemp[0], arrTemp[1], arrTemp[2]);
                }
            }
            return null;
        }
        function onDeleteBtnClicked() {
            var nLength = $("#" + '<%=gridUserList.ClientID %>' + " tr td :checkbox[checked=true]").length;
            if (nLength == 0) {
                alert("No record has been selected.");
                return false;
            }
            else if (nLength > 1) {
                alert("Only one record can be selected for this operation.");
                return false;
            }
            else {
                if (confirm('Are you sure you want to delete the selected user account?')) {
                    var id =  $("#" + '<%=hiSelectedIds.ClientID %>').val();
                    var userInfo = GetUserInfoById(id);

                    var loanCount = null;
                    try
                    {
                        loanCount = new Number(userInfo.LoanCount);
                    }
                    catch (e)
                    {
                        loanCount = 0;
                    }
                    var contactCount = null;
                    try
                    {
                        contactCount = new Number(userInfo.ContactCount);
                    }
                    catch (e)
                    {
                        contactCount = 0;
                    }
                    if (loanCount > 0 || contactCount > 0)
                    {
                        showReassignUserWin();
                        return false;
                    }
                    else
                        return true;
                }
                else
                    return false;
            }
        }
        function initReassignUserWin() {
            $('#dialog3').dialog({
                modal: true,
                autoOpen: false,
                title: 'User Selection',
                width: 600,
                height: 530,
                resizable: false,
                close: clearReassignUserWin
            });
        }
        function showReassignUserWin() {
            var f = document.getElementById('iframeReassignUser');
            var id =  $("#" + '<%=hiSelectedIds.ClientID %>').val();
            f.src = "UserForReassignSelection.aspx?CloseDialogCodes=window.parent.closeAndGetReturnValue('returnValue')&uid=" + id + "&t=" + Math.random().toString();
            $('#dialog3').dialog('open');
        }
        function clearReassignUserWin() {
            var f = document.getElementById('iframeReassignUser');
            f.src = "about:blank";
        }
        function closeAndGetReturnValue(rtValue) {
            if ("returnValue" == rtValue)
                return;
            if (rtValue.length > 0) {
                $("#" + "<%=hiReassignUserId.ClientID %>").val(rtValue);
                $('#dialog3').dialog('close');
                // callback 
                <%=this.ClientScript.GetPostBackEventReference(this.lbtnDelete, null) %>
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle">
        User List View</div>
    <div class="SplitLine">
    </div>
    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>
            <div style="padding-left: 10px; padding-right: 10px;">
                <div id="divFilter" style="margin-top: 10px;">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:DropDownList ID="ddlRegion" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                    <asp:ListItem Value="">All Regions</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 15px;">
                                <asp:DropDownList ID="ddlDivision" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                    <asp:ListItem Value="">All Divisions</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 15px;">
                                <asp:DropDownList ID="ddlBranch" runat="server" AutoPostBack="true">
                                    <asp:ListItem Value="">All Branches</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left: 15px;">
                                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="Btn-66" OnClick="btnFilter_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
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
                                   <%-- <li>
                                        <asp:LinkButton ID="lbtnCreate" runat="server" Text="Create" OnClientClick="showUserSetupWin('0', ''); return false;"></asp:LinkButton><span>|</span></li>--%>
                                    <li>
                                        <asp:LinkButton ID="lbtnDisable" runat="server" OnClientClick="return IsRowSelected('disable');"
                                            OnClick="lbtnDisable_Click">Disable</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnDelete" runat="server" OnClientClick="return onDeleteBtnClicked();"
                                            OnClick="lbtnDelete_Click">Delete</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnUpdate" runat="server" OnClientClick="onUpdateBtnClick(); return false;">Update</asp:LinkButton><span>|</span></li>
                                    <li>
                                        <asp:LinkButton ID="lbtnSetGoals" runat="server" OnClientClick="onUserGoalsClick(); return false;">Set Goals</asp:LinkButton></li>
                                </ul>
                            </td>
                            <td style="text-align: right;">
                                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                                    OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false" FirstPageText="<<" LastPageText=">>"
                                    NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                    CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                                </webdiyer:AspNetPager>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div id="div1" class="ColorGrid" style="margin-top: 5px;">
                <asp:GridView ID="gridUserList" runat="server" DataKeyNames="UserId,RoleId,Username,FirstName,LastName,UserLoanCount,UserContactCount,EmailAddress"
                    EmptyDataText="There is no user in database." AutoGenerateColumns="False" OnRowDataBound="gridUserList_RowDataBound"
                    OnPreRender="gridUserList_PreRender" CellPadding="3" CssClass="GrayGrid" GridLines="None" OnSorting="gridUserList_Sorting" AllowSorting="true">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" />
                            </ItemTemplate>
                            <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                            <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                        </asp:TemplateField>
                        <asp:BoundField DataField="LastName" SortExpression="LastName" HeaderText="Last Name" />
                        <asp:BoundField DataField="FirstName" SortExpression="FirstName" HeaderText="First Name" />
                        <asp:TemplateField HeaderText="User Name" SortExpression="UserName">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("UserName") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lbtnUserName" runat="server" Text='<%# Bind("UserName") %>' OnClientClick='<%#string.Format("showUserSetupWin(\"1\", \"{0}\"); return false;", Eval("UserId")) %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoleName" SortExpression="RoleName" HeaderText="Assigned Role">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Enabled" SortExpression="UserEnabled">
                            <ItemTemplate>
                                <asp:Label ID="lblEnabled" runat="server" Text='<%# Eval("UserEnabled").ToString().ToLower() == "true" ? "Yes" : "No" %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch">
                            <ItemTemplate>
                                <asp:Label ID="lblBranch" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hiPrefix" runat="server" />
                <div class="GridPaddingBottom">
                    &nbsp;</div>
            </div>
            <asp:HiddenField ID="hiAllIds" runat="server" />
            <asp:HiddenField ID="hiSelectedIds" runat="server" />
            <asp:HiddenField ID="hiAllLOIds" runat="server" />
            <asp:HiddenField ID="hiSelectedLOIds" runat="server" />
            <asp:HiddenField ID="hiNonLOUserIds" runat="server" />
            <asp:HiddenField ID="hiUserInfo" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="display: none;">
        <div id="dialogUserSetup">
            <iframe id="iframeUS" name="iframeUS" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="dialogUserGoalsSetup">
            <iframe id="iframeUG" name="iframeUG" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <div id="dialog3" title="User Selection">
            <iframe id="iframeReassignUser" name="iframeReassignUser" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
        <asp:LinkButton ID="lbtnRefreshList" runat="server" OnClick="lbtnRefreshList_Click"></asp:LinkButton>
        <asp:HiddenField ID="hiReassignUserId" runat="server" />
    </div>
</asp:Content>
