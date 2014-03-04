<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerCompanyList.aspx.cs" Inherits="Contact_PartnerCompanyList" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <!CDATA[

        var sHasCreate = "<%=sHasCreate %>";
        var sHasModify = "<%=sHasModify %>";
        var sHasAddBranch = "<%=sHasAddBranch %>";
        var sHasRemoveBranch = "<%=sHasRemoveBranch %>";
        $(document).ready(function () {

            InitSearchInput();

            if (sHasCreate == "0") {
                DisableLink("aCreate");
            }
            if (sHasModify == "0") {
                DisableLink("aRemove");
            }
            if (sHasAddBranch == "0") {
                DisableLink("aAddBranch");
            }
            if (sHasRemoveBranch == "0") {
                DisableLink("aRemoveBranch");
            }
        });

        function InitSearchInput() {

            // Alphabet
            var Alphabet = GetQueryString1("Alphabet");
            if (Alphabet != "") {

                $("#ddlAlphabet").val(Alphabet);
            }

            // ServiceType
            var ServiceType = GetQueryString1("ServiceType");
            if (ServiceType != "") {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlServiceType").val(ServiceType);
            }
        }

        function btnFilter_onclick() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "";

            // Alphabet
            var Alphabet = $("#ddlAlphabet").val();
            if (Alphabet != "") {

                sQueryStrings += "&Alphabet=" + encodeURIComponent(Alphabet);
            }

            // ServiceType
            var ServiceType = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlServiceType").val();
            if (ServiceType != "0") {

                sQueryStrings += "&ServiceType=" + encodeURIComponent(ServiceType);
            }

            // PageIndex
            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "" && PageIndex != "1") {

                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            if (sQueryStrings == "") {

                window.location.href = window.location.pathname;
            }
            else {

                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr td :checkbox").attr("checked", "");
            }
        }

        function aCreate_onclick() {

            window.location.href = "PartnerCompanyAdd.aspx";
        }

        function aUpdate_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No contact branch was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one contact branch can be selected.");
                return;
            }

            var CompanyID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").attr("CompanyID");

            GoToUpdateContactCompany(CompanyID);
        }

        function GoToUpdateContactCompany(CompanyID) {

            if (sHasModify != "1") {

                alert("You have no privilege to do this operation.");
                return;
            }
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerCompanyEdit.aspx?sid=" + RadomStr + "&CompanyID=" + CompanyID;
        }

        function BeforeDelete() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner company was selected.");
                return false;
            }
            else if (SelCount > 1) {

                alert("Only one partner company can be selected.");
                return false;
            }
            
            var result = confirm("Deleting a partner company will also delete its branches and contacts. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }
            
            var SelCompanyID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").attr("CompanyID");

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedCompanyID").val(SelCompanyID);

            return true;
        }

        function BeforeDisable() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner company was selected.");
                return false;
            }
            else if (SelCount > 1) {

                alert("Only one partner company can be selected.");
                return false;
            }
            
            var result = confirm("Disabling a partner company will also disable its branches and contacts. \r\n\r\nAre you sure you want to continue?");
            if (result == false) {

                return false;
            }

            var SelCompanyID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").attr("CompanyID");

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnSelectedCompanyID").val(SelCompanyID);

            return true;
        }

        function ShowDialog_AddBranch() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner company was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one partner company can be selected.");
                return;
            }

            var CompanyID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").attr("CompanyID");
            
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SelectPartnerBranch.aspx?sid=" + RadomStr + "&Action=Add&CompanyID=" + CompanyID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 400
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select Partner Branch", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowDialog_RemoveBranch() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner company was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one partner company can be selected.");
                return;
            }

            var CompanyID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCompanyList tr:not(:first) td :checkbox:checked").attr("CompanyID");

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SelectPartnerBranch.aspx?sid=" + RadomStr + "&Action=Remove&CompanyID=" + CompanyID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 400
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 500;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select Partner Branch", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function GoToContactBranches(CompanyID) {
                    
            window.location.href = "PartnerBranchList.aspx?Company=" + CompanyID;
        }

        function GoToServiceType() {
                    
            window.location.href = "PartnerServiceTypeSetup.aspx";
        }
        
 // ]]>
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divListContainer">
        <div id="divModuleName" class="Heading">Contact Management - Partner Companies</div>
        <div class="SplitLine"></div>
        <div id="divFilters" style="margin-top: 20px;">
            <table>
                <tr>
                    <td style="width: 210px;">
                        <asp:DropDownList ID="ddlServiceType" runat="server" DataValueField="ServiceTypeId" DataTextField="Name" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input id="btnFilter" type="button" value="Filter" class="Btn-66" onclick="btnFilter_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="width: 700px; margin-top: 20px;">
            <table style="width: 100%;">
                <tr>
                    <td style="width: 50px;">
                        <select id="ddlAlphabet" style="width: 40px;" onchange="btnFilter_onclick()">
                            <option value=''></option>
                            <option value="A">A</option>
                            <option value="B">B</option>
                            <option value="C">C</option>
                            <option value="D">D</option>
                            <option value="E">E</option>
                            <option value="F">F</option>
                            <option value="G">G</option>
                            <option value="H">H</option>
                            <option value="I">I</option>
                            <option value="J">J</option>
                            <option value="K">K</option>
                            <option value="L">L</option>
                            <option value="M">M</option>
                            <option value="N">N</option>
                            <option value="O">O</option>
                            <option value="P">P</option>
                            <option value="Q">Q</option>
                            <option value="R">R</option>
                            <option value="S">S</option>
                            <option value="T">T</option>
                            <option value="U">U</option>
                            <option value="V">V</option>
                            <option value="W">W</option>
                            <option value="X">X</option>
                            <option value="Y">Y</option>
                            <option value="Z">Z</option>
                        </select>
                    </td>
                    <td style="width: 450px;">
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aCreate" href="javascript:aCreate_onclick()">Create</a><span>|</span></li>
                            <li><a id="aRemove" href="javascript:aUpdate_onclick()">Update</a><span>|</span></li>
                            <li><asp:LinkButton ID="lnkDisable" runat="server" OnClientClick="return BeforeDisable()" OnClick="lnkDisable_Click">Disable</asp:LinkButton><span>|</span></li>
                            <li><asp:LinkButton ID="lnkDelete" runat="server" OnClientClick="return BeforeDelete()" OnClick="lnkDelete_Click">Delete</asp:LinkButton><span>|</span></li>
                            <li><a id="aAddBranch" href="javascript:ShowDialog_AddBranch()">Add Branch</a><span>|</span></li>
                            <li><a id="aRemoveBranch" href="javascript:ShowDialog_RemoveBranch()">Remove Branch</a></li>
                        </ul>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                            CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divCompanyList" class="ColorGrid" style="margin-top: 5px; width: 700px;">
            <asp:GridView ID="gridCompanyList" runat="server" DataSourceID="CompanySqlDataSource" AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkChecked" type="checkbox" CompanyID="<%# Eval("ContactCompanyId") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Company" SortExpression="Name">
                        <ItemTemplate>
                            <a href="javascript:GoToUpdateContactCompany('<%# Eval("ContactCompanyId")%>')"><%# Eval("Name")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Service Type" SortExpression="ServiceTypeName" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <a href="javascript:GoToServiceType()"><%# Eval("ServiceTypeName")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CompanyEnabled" HeaderText="Enabled" SortExpression="CompanyEnabled" ItemStyle-Width="80px" />
                    <asp:TemplateField HeaderText="Branches" SortExpression="Branches" ItemStyle-Width="80px">
                        <ItemTemplate>
                            <a href="javascript:GoToContactBranches(<%# Eval("ContactCompanyId")%>)"><%# Eval("Branches")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:SqlDataSource ID="CompanySqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
            <SelectParameters>
                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="Name" />
                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select b.*, case when ISNULL(b.Enabled,1)=1 then 'Yes' else 'No' end as CompanyEnabled, (select COUNT(1) from ContactBranches as a where a.ContactCompanyId=b.ContactCompanyId) as Branches, c.Name as ServiceTypeName from ContactCompanies as b inner join ServiceTypes as c on b.ServiceTypeId = c.ServiceTypeId) t" />
                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="15" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <asp:HiddenField ID="hdnSelectedCompanyID" runat="server" />
</asp:Content>
