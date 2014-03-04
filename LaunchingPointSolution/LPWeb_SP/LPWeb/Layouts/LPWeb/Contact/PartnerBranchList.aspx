<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerBranchList.aspx.cs" Inherits="Contact_PartnerBranchList" MasterPageFile="~/_layouts/LPWeb/MasterPage/Contact.master"  %>

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
    <script language="javascript" type="text/javascript">
        // <!CDATA[

        var gridId = "#<%=gridBranchList.ClientID %>";

        var sHasCreate = "<%=sHasCreate %>";
        var sHasModify = "<%=sHasModify %>";
        var sHasAddBranch = "<%=sHasAddBranch %>";
        var sHasRemoveBranch = "<%=sHasRemoveBranch %>";
        var sHasModifyCompany = "<%=sHasModifyCompany %>";
        var sHasModifyServiceType = "<%=sHasModifyServiceType %>";
        $(document).ready(function () {

            InitSearchInput();

            if (sHasCreate == "0") {
                DisableLink("aCreate");
            }
            if (sHasModify == "0") {
                DisableLink("aupdate");
            }
            if (sHasAddBranch == "0") {
                DisableLink("aAdd");
            }
            if (sHasRemoveBranch == "0") {
                DisableLink("aRemove");
            }

            var checkAll = $(gridId + " :checkbox:eq(0)");
            checkAll.click(function () {
                var allStatus = checkAll.attr("checked");
                $(gridId + " :checkbox:gt(0)").each(function () {
                    $(this).attr("checked", allStatus);
                });
                getSelectedItems();
            });
            //
            $(gridId + " :checkbox:gt(0)").each(function () {
                $(this).unbind("click").click(function () {
                    if ($(this).attr("checked") == false) {
                        checkAll.attr("checked", false);
                    }
                    getSelectedItems();
                });
            });

            $("#<%= btnDisable.ClientID %>").click(function () {
                if ($("#<%=hiSelectedBranch.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }
                else {
                    return confirm("Disabling a partner branch will also disable its contacts. Are you sure you want to continue?");
                }
            });

            $("#<%= btnDelete.ClientID %>").click(function () {
                if ($("#<%=hiSelectedBranch.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }
                else {
                    return confirm("Deleting a partner branch will also delete its contacts. Are you sure you want to continue?");
                }
            });
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

           // Company
            var Company = GetQueryString1("Company");
            if (Company != "") {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCompany").val(Company);

            }
             // State
            var State = GetQueryString1("State");
            if (State != "") {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStates").val(State);
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

            // Company
            var Company = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlCompany").val();
            if (Company != "0") {

                sQueryStrings += "&Company=" + encodeURIComponent(Company);
            }

            // State
            var State = $("#ctl00_ctl00_PlaceHolderMain_MainArea_ddlStates").val();
            if (State != "") {

                sQueryStrings += "&State=" + encodeURIComponent(State);
            }

            // PageIndex
            var PageIndex = 1; //GetQueryString1("PageIndex");
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

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox").attr("checked", "");
            }
        }

        function aCreate_onclick() {

            window.location.href = "PartnerBranchSetup.aspx";
        }

        function aUpdate_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one partner branch can be selected.");
                return;
            }

            var BranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").attr("tag");

            GoToUpdateContactBranch(BranchID);
        }

        

        function GoToUpdateContactBranch(BranchId) {

            if (sHasModify != "1") {

                alert("You have no privilege to do this operation.");
                return;
            }
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerBranchEdit.aspx?sid=" + RadomStr + "&ContactBranchId=" + BranchId;
        }

      

        function getSelectedItems() {
            var selctedItems = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                }
            });
            $("#<%=hiSelectedBranch.ClientID %>").val(selctedItems.join(","));
        }

        function LinkCompany(CompanyID) {

            if (sHasModifyCompany != "1") {

                alert("You have no privilege to do this operation.");
                return;
            }
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerCompanyEdit.aspx?sid=" + RadomStr + "&CompanyID=" + CompanyID;
        }

        function LinkServiceType(ServiceTypeID) {
            if (sHasModifyServiceType != "1") {

                alert("You have no privilege to do this operation.");
                return;
            }
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerServiceTypeSetup.aspx?sid=" + RadomStr;
        }

        function LinkContacts(ContactBranchId) {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            window.location.href = "PartnerContactsPage.aspx?sid=" + RadomStr + "&BranchID=" + ContactBranchId;
        }

        function add_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one record can be selected for this operation.");
                return;
            }

            OpenSelectContact('add');
        }

        function remove_onclick() {

            var SelCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("No partner branch was selected.");
                return;
            }
            else if (SelCount > 1) {

                alert("Only one record can be selected for this operation.");
                return;
            }

            OpenSelectContact('remove');
        }

        function OpenSelectContact(stype) {
            var BranchID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridBranchList tr td :checkbox:checked").attr("tag");
           
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "SelectContactsPopup.aspx?ContactBranchId=" + BranchID + "&type=" + stype + "&t=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 800
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 680;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Select Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function closeDialog() {
            $("#dialogSelectContact").dialog('destroy');
            window.location.hre = window.location.hre;
        }

        // ]]>
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divListContainer">
        <div id="divModuleName" class="Heading">Contact Management - Partner Branches</div>
        <div class="SplitLine"></div>
        <div id="divFilters" style="margin-top: 20px;">
            <table>
                <tr>
                    <td style="width: 210px;">
                        <asp:DropDownList ID="ddlServiceType" runat="server" DataValueField="ServiceTypeId" DataTextField="Name" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 210px;">
                        <asp:DropDownList ID="ddlCompany" runat="server" DataValueField="ContactCompanyId" DataTextField="Name" Width="200px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 210px;">
                        <asp:DropDownList ID="ddlStates" runat="server"  Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <input id="btnFilter" type="button" value="Filter" class="Btn-66" onclick="btnFilter_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
         <div id="divToolBar" style="width: 900px; margin-top: 20px;">
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
                    <td style="width: 550px;">
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aCreate" href="javascript:aCreate_onclick()">Create</a><span>|</span></li>
                            <li><a id="aupdate" href="javascript:aUpdate_onclick()">Update</a><span>|</span></li>
                            <li><%--<a id="aDisable" href="" onclick="aDisable_onclick();return false;">Disable</a>--%>
                             <asp:LinkButton ID="btnDisable" runat="server" Text="Disable" OnClick="btnDisable_Click"></asp:LinkButton>
                            <span>|</span></li>
                            <li><%--<a id="aEnable" href="" onclick="aEnable_onclick();return false;">Delete</a>--%>
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClick="btnDelete_Click"></asp:LinkButton>
                            <span>|</span></li>
                            <li><a id="aAdd"  href="javascript:add_onclick()">Add Contact</a><span>|</span></li>
                            <li><a id="aRemove" href="javascript:remove_onclick()">Remove Contact</a></li>
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
        <div id="divCompanyList" class="ColorGrid" style="margin-top: 5px; width: 900px;">
            <asp:GridView ID="gridBranchList" runat="server" DataSourceID="BranchSqlDataSource" AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                 <input type="checkbox"  tag='<%# Eval("ContactBranchId") %>'  />
                            </ItemTemplate>
                            <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                            <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Company" SortExpression="Company" HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <a href="javascript:LinkCompany('<%# Eval("ContactCompanyId") %>')"><%# Eval("Company")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch" SortExpression="Branch"  HeaderStyle-Width="260px">
                            <ItemTemplate>
                                <a href="javascript:GoToUpdateContactBranch('<%# Eval("ContactBranchId") %>')"><%# Eval("Branch")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Service Type" SortExpression="ServiceType"  HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <a href="javascript:LinkServiceType('<%# Eval("ServiceType") %>')"><%# Eval("ServiceType")%></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="State" SortExpression="State"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("State")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="City" SortExpression="City"  HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# Eval("City")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" Width="140px" />
                            <HeaderStyle Width="140px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enabled" SortExpression="Enabled"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("Enabled")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contacts" SortExpression="Contacts"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <a href="javascript:LinkContacts('<%# Eval("ContactBranchId") %>')"><%# Eval("Contacts")%></a>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>
                    </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:SqlDataSource ID="BranchSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
            <SelectParameters>
                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="Branch" />
                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(SELECT     dbo.ContactBranches.ContactBranchId, dbo.ContactBranches.ContactCompanyId, dbo.ContactBranches.Name as Branch, dbo.ContactBranches.City, dbo.ContactBranches.[State], 
                      dbo.ContactCompanies.Name AS Company, dbo.ContactCompanies.ServiceTypeId,dbo.ContactCompanies.ServiceTypes as ServiceType,(select Count(contactID) from dbo.Contacts where dbo.Contacts.ContactBranchId=dbo.ContactBranches.ContactBranchId) as  Contacts, case when ISNULL(dbo.ContactBranches.[Enabled],1)=1 then 'Yes' else 'No' end as [Enabled]
FROM         dbo.ContactCompanies RIGHT OUTER JOIN
                      dbo.ContactBranches ON dbo.ContactCompanies.ContactCompanyId = dbo.ContactBranches.ContactCompanyId) t" />
                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="15" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
         <asp:HiddenField ID="hiSelectedBranch" runat="server" />
    </div>
   <div style="display: none;">
        <div id="dialogSelectContact" title="Select Contacts">
            <iframe id="iframeSelectContact" name="iframeSelectContact" frameborder="0" width="100%"
                height="100%"></iframe>
        </div>
    </div>
</asp:Content>

