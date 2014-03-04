<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignContactBeforeDelete.aspx.cs" Inherits="Contact_AssignContactBeforeDelete" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            InitSearchInput();

            // add event
            $("#ddlContactType").change(ddlContactType_onchange);
        });

        function ddlContactType_onchange() {

            var ContactType = $("#ddlContactType").val();
            if (ContactType == "Client") {

                $("#ddlServiceType").val("");
                $("#txtCompany").val("");
                $("#txtBranch").val("");

                $("#ddlServiceType").attr("disabled", "true");
                $("#txtCompany").attr("disabled", "true");
                $("#txtBranch").attr("disabled", "true");
            }
            else {

                $("#ddlServiceType").attr("disabled", "");
                $("#txtCompany").attr("disabled", "");
                $("#txtBranch").attr("disabled", "");
            }
        }

        function InitSearchInput() {

            //#region dropdown list

            // ContactType
            var ContactType = GetQueryString1("ContactType");
            if (ContactType != "") {

                $("#ddlContactType").val(ContactType);
            }

            // Service Type
            var ServiceType = GetQueryString1("ServiceType");
            if (ServiceType != "") {

                $("#ddlServiceType").val(ServiceType);
            }

            //#endregion

            //#region text box

            // Company
            var Company = GetQueryString1("Company");
            if (Company != "") {

                $("#txtCompany").val(Company);
            }

            // Branch
            var Branch = GetQueryString1("Branch");
            if (Branch != "") {

                $("#txtBranch").val(Branch);
            }

            // LastName
            var LastName = GetQueryString1("LastName");
            if (LastName != "") {

                $("#txtLastName").val(LastName);
            }

            // Address
            var Address = GetQueryString1("Address");
            if (Address != "") {

                $("#txtAddress").val(Address);
            }

            // City
            var City = GetQueryString1("City");
            if (City != "") {

                $("#txtCity").val(City);
            }

            // State
            var State = GetQueryString1("State");
            if (State != "") {

                $("#ddlState").val(State);
            }

            //#endregion

        }

        function btnSearch_onclick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var DelContactID = GetQueryString1("DelContactID");

            // 参数字符串
            var sQueryStrings = "?DelContactID=" + DelContactID + "&DoSearch=1&sid=" + sid;

            //#region dropdown list

            // ContactType
            var ContactType = $("#ddlContactType").val();
            if (ContactType != "") {

                sQueryStrings += "&ContactType=" + encodeURIComponent(ContactType);
            }

            // ServiceType
            var ServiceType = $("#ddlServiceType").val();
            if (ServiceType != "" && ServiceType != null) {

                sQueryStrings += "&ServiceType=" + ServiceType;
            }

            //#endregion

            //#region textbox

            // Company
            var Company = $.trim($("#txtCompany").val());
            if (Company != "") {

                sQueryStrings += "&Company=" + encodeURIComponent(Company);
            }

            // Branch
            var Branch = $.trim($("#txtBranch").val());
            if (Branch != "") {

                sQueryStrings += "&Branch=" + encodeURIComponent(Branch);
            }

            // LastName
            var LastName = $.trim($("#txtLastName").val());
            if (LastName != "") {

                sQueryStrings += "&LastName=" + encodeURIComponent(LastName);
            }

            // Address
            var Address = $.trim($("#txtAddress").val());
            if (Address != "") {

                sQueryStrings += "&Address=" + encodeURIComponent(Address);
            }

            // City
            var City = $.trim($("#txtCity").val());
            if (City != "") {

                sQueryStrings += "&City=" + encodeURIComponent(City);
            }

            // State
            var State = $("#ddlState").val();
            if (State != "") {

                sQueryStrings += "&State=" + encodeURIComponent(State);
            }

            //#endregion

            //#region PageIndex

            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "" && PageIndex != "1") {
                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            //#endregion

            //alert(sQueryStrings);

            if (sQueryStrings == "") {

                window.location.href = window.location.href;
            }
            else {

                window.location.href = window.location.pathname + sQueryStrings;
            }
        }

        function btnClear_onclick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            var DelContactID = GetQueryString1("DelContactID");

            window.location.href = "AssignContactBeforeDelete.aspx?DelContactID=" + DelContactID + "&sid=" + sid;
        }

        function CheckOne(CheckBox) {

            var IsChecked = CheckBox.checked;

            $("#gridContactList tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }
        }

        function BeforeSelect() {

            var SelCount = $("#gridContactList tr td :checkbox:checked").length;
            if (SelCount == 0) {

                alert("Please select one contact.");
                return false;
            }

            var ContactID = $("#gridContactList tr td :checkbox:checked").attr("ContactID");
            //alert(ContactID);

            $("#hdnSelContactID").val(ContactID);

            return true;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divPopupContainer">
        <div id="divSearchCriteria" runat="server">
            <table>
                <tr>
                    <td style="width: 80px;">Contact Type:</td>
                    <td style="width: 240px;">
                        <select id="ddlContactType" style="width: 203px;">
                            <option value="">All</option>
                            <option>Client</option>
                            <option>Partner</option>
                        </select>
                    </td>
                    <td style="width: 80px;">Service Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlServiceType" runat="server" Width="203px" DataValueField="ServiceTypeId" DataTextField="Name">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>Company:</td>
                    <td>
                        <asp:TextBox ID="txtCompany" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td>Branch:</td>
                    <td>
                        <asp:TextBox ID="txtBranch" runat="server" Width="200px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Last Name:</td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 80px;">Address:</td>
                    <td style="width: 240px;">
                        <asp:TextBox ID="txtAddress" runat="server" Width="200px"></asp:TextBox>
                    </td>
                    <td style="width: 30px;">City:</td>
                    <td style="width: 140px;">
                        <asp:TextBox ID="txtCity" runat="server" Width="120px"></asp:TextBox>
                    </td>
                    <td style="width: 35px;">State:</td>
                    <td>
                        <asp:DropDownList ID="ddlState" runat="server" Width="50">
                                <asp:ListItem Text="" Value="">All</asp:ListItem>
                                <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                                <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                                <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                                <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                                <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                                <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                                <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                                <asp:ListItem Text="DC" Value="DC"></asp:ListItem>
                                <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                                <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                                <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                                <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                                <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                                <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                                <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                                <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                                <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                                <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                                <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                                <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                                <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                                <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                                <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                                <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                                <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                                <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                                <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                                <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                                <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                                <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                                <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                                <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                                <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                                <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                                <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                                <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                                <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                                <asp:ListItem Text="PR" Value="PR"></asp:ListItem>
                                <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                                <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                                <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                                <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                                <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                                <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                                <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                                <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                                <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                                <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                                <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                            </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <input id="btnSearch" type="button" value="Search" class="Btn-66" onclick="btnSearch_onclick()" />
                    </td>
                    <td>
                        <input id="btnClear" type="button" value="Clear" class="Btn-66" onclick="btnClear_onclick()" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div id="divSearchResult" runat="server">
            <div style="width: 750px; text-align: right; margin-top: 10px;">
                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
                    UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                    NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                    CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                </webdiyer:AspNetPager>
            </div>
            <div id="divContactList" class="ColorGrid" style="margin-top: 5px; width: 750px;">
                <asp:GridView ID="gridContactList" runat="server" DataSourceID="ContactSqlDataSource" EmptyDataText="There is no partner contact by criteria." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" ContactID="<%# Eval("ContactId") %>" onclick="CheckOne(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="FullName" HeaderText="Contact" />
                        <asp:BoundField DataField="ContactType" HeaderText="Contact Type" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="ServiceType" HeaderText="Service Type" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="BranchName" HeaderText="Branch" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="CompanyName" HeaderText="Company" ItemStyle-Width="180px" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
            <asp:SqlDataSource ID="ContactSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
                <SelectParameters>
                    <asp:Parameter Name="OrderByField" Type="String" DefaultValue="LastName" />
                    <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                    <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                    <asp:Parameter Name="DbTable" Type="String" DefaultValue="" />
                    <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                    <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="15" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <br />
            <div>
                <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="Btn-66" OnClientClick="return BeforeSelect()" onclick="btnSelect_Click" />
            </div>
        </div>
        <asp:HiddenField ID="hdnSelContactID" runat="server" />
    </div>
    </form>
</body>
</html>
