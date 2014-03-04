<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmploymentTab.aspx.cs"
    Inherits="EmploymentTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script type="text/javascript">

        var gridId = "#<%=gridList.ClientID %>";

        $(document).ready(function () {

        });

        function getSelectedItems() {
            var selctedItems = new Array();
            $("#<%=gridList.ClientID %> tr td.CheckBoxColumn :checkbox").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            return selctedItems;
        }

        function ShowEmploymentDetailsAdd() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "EmploymentDetailPopup.aspx?sid=" + RadomStr + "&Action=Add&ContactID=" + $("#<%=ddlContacts.ClientID %>").val() + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 720;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 480;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.ShowGlobalPopup("Prospect Employment Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function PopupDetailUpdate() {
            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("Please select one row.");
                return;
            }

            if (checkedIds == null || checkedIds.length != 1) {
                alert("Only one record can be selected for this operation.");
                return;
            }

            var EmployID = checkedIds.pop();
            var ContactID= $("#<%=ddlContacts.ClientID %>").val();
            ShowEmploymentDetailsUpdate(EmployID,ContactID);
            
        }

        function ShowEmploymentDetailsUpdate(EmployID,ContactID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "EmploymentDetailPopup.aspx?sid=" + RadomStr + "&Action=Update&EmployID=" + EmployID + "&ContactID=" + ContactID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 720;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 480;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.ShowGlobalPopup("Prospect Employment Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        // check/decheck all
        function CheckAll(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td.CheckBoxColumn :checkbox").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td.CheckBoxColumn :checkbox").attr("checked", "");
            }
        }
        // do sth. before saving
        function BeforeDelete() {

            var checkedIds = getSelectedItems();
            if (checkedIds.length == 0) {
                alert("Please select one or more row(s).");
                return false;
            }

            var result = confirm(" This operation is not reversible. Are you sure you want to continue? ");

            if (result == false) {

                return false;
            }
            $("#hdnEmplIds").val(checkedIds.join(",")); 

            return true;
        }

    </script>
</head>
<body style="width: 700px">
    <form id="form1" runat="server">
    <asp:HiddenField ID="hfdContactId" runat="server" />
    <asp:HiddenField ID="hfdFileID" runat="server" />
    <asp:HiddenField ID="hdnEmplIds" runat="server" />
    <div id="divFilters" style="margin-top: 20px;">
        <table>
            <tr>
                <td style="width: 510px;">
                    <asp:DropDownList ID="ddlContacts" runat="server" DataValueField="ContactId" DataTextField="ContactsName"
                        Width="200px" OnSelectedIndexChanged="ddlContacts_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="All Contacts" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 900px;">
                <tr>
                    <td style="width: 300px;">
                        <div id="div1">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="btnNew" runat="server" style="cursor: hand" onclick="ShowEmploymentDetailsAdd()">
                                    Add</a><span>|</span></li>
                                <li><a id="btnUpdate" runat="server" style="cursor: hand" onclick="PopupDetailUpdate()">
                                    Update</a><span>|</span></li>
                                <li>
                                    <asp:LinkButton ID="btnRemove" runat="server" Text="Delete" OnClientClick="return BeforeDelete();" OnClick="btnRemove_Click"></asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </td>
                    <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
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
    <div id="divGrid" class="ColorGrid" style="width: 1000px; margin-top: 5px;">
        <asp:GridView ID="gridList" runat="server"
            EmptyDataText="There is no data information." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None"
            OnSorting="gridList_Sorting" AllowSorting="true">
            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
            <AlternatingRowStyle CssClass="EvenRow" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                    <HeaderTemplate>
                        <input type="checkbox" onclick="CheckAll(this)" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <input type="checkbox" tag='<%# Eval("EmplId") %>' />
                    </ItemTemplate>
                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Employer " SortExpression="CompanyName"  
                    HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:ShowEmploymentDetailsUpdate(<%# Eval("EmplId")%>,<%# Eval("ContactId")%>)">
                            <%# Eval("CompanyName")%></a>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Position " SortExpression="Position"  
                    HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <a href="#" onclick="javascript:ShowEmploymentDetailsUpdate(<%# Eval("EmplId")%>,<%# Eval("ContactId")%>)">
                            <%# Eval("Position")%></a>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="From " SortExpression="StartMonth"  
                    HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("StartMonth")%>/<%# Eval("StartYear")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="To " SortExpression="EndMonth"  
                    HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Eval("EndMonth")%>/<%# Eval("EndYear")%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <asp:BoundField DataField="Phone" SortExpression="Phone" HeaderText="Phone" HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="CurrentMY" SortExpression="CurrentMY" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAuto" runat="server" Checked='<%# Eval("CurrentMY")%>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Self-Employed" SortExpression="SelfEmployed" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70px">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkAuto" runat="server" Checked='<%# Eval("SelfEmployed")%>' Enabled="false" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <div class="GridPaddingBottom">
            &nbsp;</div>
    </div>
    </form>
</body>
</html>
