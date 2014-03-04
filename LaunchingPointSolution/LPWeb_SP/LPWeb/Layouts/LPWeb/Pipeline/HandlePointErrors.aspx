<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>

<%@ Page Language="C#" AutoEventWireup="true" Inherits="LPWeb.Layouts.LPWeb.Pipeline.HandlePointErrors"
    CodeBehind="HandlePointErrors.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
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
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <title>Handle Point Errors</title>
    <script type="text/javascript">
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

        var arrAllErrorIds = new Array();   // set by code behind, hold all alert id on current page
        var arrSelectedID = new Array();

        $(document).ready(function () {
            initDetailWin();
        });

        function initDetailWin() {
            $('#dlgDetail').dialog({
                //modal: true,
                autoOpen: false,
                title: 'Point Import Alert Detail',
                width: 640,
                height: 520,
                resizable: false,
                close: clearDetailWin
            });
        }

        function showDetailWin(eid) {
            var f = document.getElementById('iframePE');
            if (null == eid)
                eid = "";
            var src = $("."+eid.toString()).attr("src");
            f.src = "PopupPointImportAlert.aspx?hisId=" + eid +"&icon="+ src.substring(src.lastIndexOf("/") + 1)+ "&t=" + Math.random().toString();
            $('#dlgDetail').dialog('open');
        }

        function closeImportErrorDialog(bRefresh) {
            $("#dlgDetail").dialog('close');
            if (bRefresh === true)
                RefreshList();
        }

        function closeDetailWin(bRefresh)
        {
            $('#dlgDetail').dialog('close');
            if (bRefresh === true)
                RefreshList();
        }

        function clearDetailWin() {
            var f = document.getElementById('iframePE');
            f.src = "about:blank";
        }

        function RefreshList() {
            <%=this.ClientScript.GetPostBackEventReference(this.btnFilter, null) %>
        }

        function onDetailBtnClick() {
            if (arrSelectedID.length == 0)
                alert("No record has been selected.");
            else if (arrSelectedID.length == 1)
                showDetailWin(arrSelectedID[0]);
            else
                alert("Only one record can be selected for this operation.");
        }

        function onRowChecked(bIsSelected, sID) {
            if (bIsSelected)
                arrSelectedID.push(sID);
            else
                arrSelectedID.remove(sID);
        }
    </script>
    <script type="text/javascript">
// <![CDATA[
        // check/decheck all
        function CheckAll(CheckBox) {
            if (CheckBox.checked) {
                $("#" + '<%=gridErrors.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
                arrSelectedID = arrAllErrorIds;
            }
            else {
                $("#" + '<%=gridErrors.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
                arrSelectedID = new Array();
            }
        }
        function IsRowSelected(args) {
            if ($("#" + '<%=gridErrors.ClientID %>' + " tr td :checkbox[checked=true]").length == 0) {
                alert("No record has been selected.");
                return false;
            }
            else {
                return confirm('This operation is not reversible. Are you sure you want to continue?');
            }
        }
// ]]>
    </script>
</head>
<body style="margin: 0; padding: 0;">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="aspnetForm">
        <div class="Heading">
            Handle Point Errors</div>
        <div class="SplitLine">
        </div>
        <div class="DetailsContainer">
            <asp:UpdatePanel ID="updatePanel" runat="server">
                <ContentTemplate>
                    <div style="padding-left: 10px; padding-right: 10px;">
                        <div id="divFilter" style="margin-top: 10px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlRegion" runat="server" DataTextField="Name" DataValueField="RegionId"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlRegion_SelectedIndexChanged">
                                            <asp:ListItem Value="0">All Regions</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="padding-left: 15px;">
                                        <asp:DropDownList ID="ddlDivision" runat="server" DataTextField="Name" DataValueField="DivisionId"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlDivision_SelectedIndexChanged">
                                            <asp:ListItem Value="0">All Divisions</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="padding-left: 15px;">
                                        <asp:DropDownList ID="ddlBranch" runat="server" DataTextField="Name" DataValueField="BranchId">
                                            <asp:ListItem Value="0">All Branches</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id="div2" style="margin-top: 13px;">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlErrorType" runat="server">
                                            <asp:ListItem Value="1">All errors and warnings</asp:ListItem>
                                            <asp:ListItem Value="2">All errors</asp:ListItem>
                                            <asp:ListItem Value="3">All warnings</asp:ListItem>
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
                                            <li>
                                                <asp:LinkButton ID="lbtnDetail" runat="server" OnClientClick="onDetailBtnClick(); return false;">Detail</asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" OnClientClick="return IsRowSelected();"
                                                    OnClick="lbtnDelete_Click">Delete</asp:LinkButton></li>
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
                    </div>
                    <div id="div1" class="ColorGrid" style="margin-top: 5px;">
                        <asp:GridView ID="gridErrors" runat="server" DataKeyNames="HistoryId" EmptyDataText="There is no data in database."
                            AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None"
                            AllowSorting="true" OnSorting="gridErrors_Sorting" OnRowDataBound="gridErrors_RowDataBound"
                            OnPreRender="gridErrors_PreRender">
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
                                <asp:BoundField DataField="Borrower" SortExpression="Borrower" HeaderText="Borrower" />
                                <asp:TemplateField SortExpression="Severity" HeaderText="Severity">
                                    <ItemTemplate>
                                        <asp:Image ID="imgAlert" class='<%# Eval("HistoryId")%>' runat="server" ToolTip='<%# Bind("Severity") %>' Height="16"
                                            Width="16" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Error" HeaderText="Error Message">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnError" runat="server" Text='<%# Bind("Error") %>' OnClientClick='<%#string.Format("showDetailWin(\"{0}\"); return false;", Eval("HistoryId")) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PointFile" SortExpression="PointFile" HeaderText="Point File">
                                    <HeaderStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">
                            &nbsp;</div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div style="display: none;">
        <div id="dlgDetail" title="Point Import Alert Detail">
            <iframe id="iframePE" name="iframePE" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
    </div>
    </form>
</body>
</html>
