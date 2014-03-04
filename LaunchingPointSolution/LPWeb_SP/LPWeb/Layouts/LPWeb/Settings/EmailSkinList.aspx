<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" 
Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailSkinList.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Settings.EmailSkinList" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"  %>

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
        function CheckAll(CheckBox) {
            //CheckAllClicked(CheckBox);
            if (CheckBox.checked) {
                $("#" + '<%=gridList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#" + '<%=gridList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function IsRowSelected(args) {
            var checkedTotal =$("#" + '<%=gridList.ClientID %>' + " tr td :checkbox[checked=true]").length;
            if (checkedTotal == 0) {
                alert("No record has been selected.");
                return false;
            }

            if ("delete" == args) {
                return confirm('If you delete the Email Skin(s), the email templates that are referencing the Email Skins will be affected.Are you sure you want to delete the selected Email Skin(s)?');
            }
            else if ("clone" == args && checkedTotal > 1) {
                alert("Only one Email Skin can be selected.");
                return false;
            }
            
            return true;
        }

        function ShowDialog_EditEmailTemplateSkin() {

            var SelectedCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No Email Skin was selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one Email Skin can be selected.");
                return;
            }

            var EmailTemplateSkinID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridList tr:not(:first) td :checkbox:checked").parent().attr("title");
            //alert(EmailTemplateID);

            UpdateEmailTemplateSkin(EmailTemplateSkinID);
        }

        function UpdateEmailTemplateSkin(EmailTemplateSkinID) {

            location.href = "EmailSkinEdit.aspx?EmailSkinID=" + EmailTemplateSkinID;
        }

        function EmailTemplateSkinUse(EmailTemplateSkinID) {

            location.href = "EmailTemplateList.aspx?EmailSkinID=" + EmailTemplateSkinID;

        }

        
// ]]>
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
                            <li><a href="EmailTemplateList.aspx"><span>Email Templates</span></a></li>
                            <li id="current"><a href="EmailSkinList.aspx"><span>Email Skins</span></a></li>
                            
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
                                                <li><a href="EmailSkinAdd.aspx">Create</a><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="lbtnDisable" runat="server" OnClientClick="return IsRowSelected('disable');"
                                                        OnClick="lbtnDisable_Click">Disable</asp:LinkButton><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="lbtnDelete" runat="server" OnClientClick="return IsRowSelected('delete');"
                                                        OnClick="lbtnDelete_Click">Delete</asp:LinkButton><span>|</span></li>
                                                <li><a href="javascript:ShowDialog_EditEmailTemplateSkin()">Update</a><span>|</span></li>
                                                <li>
                                                    <asp:LinkButton ID="lbtnClone" runat="server"  OnClientClick="return IsRowSelected('clone');"
                                                        OnClick="lbtnClone_Click">Clone</asp:LinkButton></li>
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
                            <asp:GridView ID="gridList" runat="server" DataKeyNames="EmailSkinId" EmptyDataText="There is no email skin in database."
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
                                            <asp:CheckBox ID="ckbSelected" runat="server" EnableViewState="true" ToolTip='<%# Eval("EmailSkinId") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                        <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Email Skin Name" SortExpression="Name">
                                        <ItemTemplate>
                                            <a href="javascript:UpdateEmailTemplateSkin('<%# Eval("EmailSkinId") %>')"><%# Eval("Name")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="Desc" ItemStyle-Width="300">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDesc" runat="server" Text='<%# Bind("Desc") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled">
                                        <ItemTemplate>
                                            <asp:Label ID="lblEnabled" runat="server" Text='<%# Eval("Enabled").ToString().ToLower() == "true" ? "Yes" : "No" %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="# Email Templ" SortExpression="EmailTemplTotal"  ItemStyle-Width="250" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <a href="javascript:EmailTemplateSkinUse('<%# Eval("EmailSkinId") %>')"><%# Eval("EmailTemplTotal")%></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Default" SortExpression="Default">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDefault" runat="server" Text='<%# Eval("Default").ToString().ToLower() == "true" ? "Yes" : "No" %>'></asp:Label>
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

                
            </div>
        </div>
    </div>
</asp:Content>