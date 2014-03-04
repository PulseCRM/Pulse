<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignLoanTeamPopup.aspx.cs" Inherits="LoanDetails_AssignLoanTeamPopup" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Assign Loan Team</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#ddlLoanRole").attr("disabled", "true");
            InitSearchInput();

        });

        function InitSearchInput() {

            // Branch
            var Branch = GetQueryString1("Branch");
            if (Branch != "") {
                $("#ddlBranch").val(Branch);
            }

            // User Role
            var UserRole = GetQueryString1("UserRole");
            if (UserRole != "") {
                $("#ddlUserRole").val(UserRole);
            }

            // Last Name
            var LastName = GetQueryString1("LastName");
            if (LastName != "") {
                $("#txtLastName").val(LastName);
            }

            // OrderByField
            var OrderByField = GetQueryString1("OrderByField");
            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByField != "") {

                if (OrderByField == "BranchName") {

                    if (OrderByType == "asc") {

                        $("#spOrderByFlag1").text("▲");
                    }
                    else if (OrderByType == "desc") {

                        $("#spOrderByFlag1").text("▼");
                    }
                }
                else if (OrderByField == "UserRole") {

                    if (OrderByType == "asc") {

                        $("#spOrderByFlag2").text("▲");
                    }
                    else if (OrderByType == "desc") {

                        $("#spOrderByFlag2").text("▼");
                    }
                }
                else if (OrderByField == "FullName") {

                    if (OrderByType == "asc") {

                        $("#spOrderByFlag3").text("▲");
                    }
                    else if (OrderByType == "desc") {

                        $("#spOrderByFlag3").text("▼");
                    }
                }
            }
        }

        function BuildQueryStrings() {

            var FileID = GetQueryString1("FileID");
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            var GetIDsFunction = GetQueryString1("GetIDsFunction");

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // 参数字符串
            var sQueryStrings = "?search=1&sid=" + sid + "&FileID=" + FileID + "&GetIDsFunction=" + GetIDsFunction + "&CloseDialogCodes=" + CloseDialogCodes;

            // Branch
            var Branch = $("#ddlBranch").val();
            if (Branch != "0") {
                sQueryStrings += "&Branch=" + encodeURIComponent(Branch);
            }

            // User Role
            var UserRole = $.trim($("#ddlUserRole").val());
            if (UserRole != "0") {
                sQueryStrings += "&UserRole=" + encodeURIComponent(UserRole);
            }

            // Last Name
            var LastName = $.trim($("#txtLastName").val());
            if (LastName != "") {
                sQueryStrings += "&LastName=" + encodeURIComponent(LastName);
            }

            if (Branch == "" && UserRole == "" && LastName == "") {

                alert("Please enter search condition at first.");
                return;
            }


            return sQueryStrings;
        }

        function btnSearch_onclick() {

            var sQueryStrings = BuildQueryStrings();

            // PageIndex
            sQueryStrings += "&PageIndex=1";

            window.location.href = window.location.pathname + sQueryStrings;
        }

        function CheckOne(CheckBox) {

            var IsChecked = CheckBox.checked;

            $("#gridLoanTeamList tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }

            if ($("#gridLoanTeamList tr td :checkbox:checked").length == 1) {

                $("#ddlLoanRole").attr("disabled", "");
            }
            else {

                $("#ddlLoanRole").attr("disabled", "true");
            }
        }

        function OrderBy(field) {

            var sQueryStrings = BuildQueryStrings();

            // PageIndex
            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "") {
                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            sQueryStrings += "&OrderByField=" + field;

            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByType == "") {

                sQueryStrings += "&OrderByType=asc";
            }
            else if (OrderByType == "asc") {

                sQueryStrings += "&OrderByType=desc";
            }
            else if (OrderByType == "desc") {

                sQueryStrings += "&OrderByType=asc";
            }

            window.location.href = window.location.pathname + sQueryStrings;
        }

        function btnSave_onclick() {

            var LoanRoleID = $("#ddlLoanRole").val();
            if (LoanRoleID == "0") {

                alert("Please select a Loan Role.");
                return;
            }

            if ($("#gridLoanTeamList tr td :checkbox:checked").length == 0) {

                alert("Please select a user.");
                return;
            }

            var UserID = $("#gridLoanTeamList tr td :checkbox:checked").attr("UserID");

            var GetIDsFunction = GetQueryString1("GetIDsFunction");
            //alert(GetIDsFunction);

            var InvokeStr = GetIDsFunction + "('" + UserID + "', '" + LoanRoleID + "')";
            //alert(InvokeStr);

            eval(InvokeStr);
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        <div id="divModuleName" class="ModuleTitle">Assign Loan Team</div>

        <div id="divFilter" style="margin-top: 10px; margin-left: 10px;">
            <table>
                <tr>
                    <td style="width: 210px;">Borrower: <asp:Label ID="lbBorrower" runat="server" Text="Label"></asp:Label></td>
                    <td>Property: <asp:Label ID="lbProperty" runat="server" Text="Label"></asp:Label></td>
                </tr>
            </table>
            <table style="margin-top: 5px;">
                <tr>
                    <td style="width: 70px;">Branch:</td>
                    <td style="width: 160px;">
                        <asp:DropDownList ID="ddlBranch" runat="server" DataValueField="BranchId" DataTextField="Name" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px;">User Role:</td>
                    <td style="width: 160px;">
                        <asp:DropDownList ID="ddlUserRole" runat="server" DataValueField="RoleId" DataTextField="Name" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px;">Last Name:</td>
                    <td style="width: 140px;">
                        <asp:TextBox ID="txtLastName" runat="server" Width="120px"></asp:TextBox>
                    </td>
                    <td>
                        <input id="btnDisplay" type="button" value="Display" class="Btn-66" onclick="btnSearch_onclick()" />
                    </td>
                </tr>
            </table>
            <table style="margin-top: 10px; width: 100%;">
                <tr>
                    <td style="width: 55px;">Loan Role:</td>
                    <td style="width: 155px;">
                        <asp:DropDownList ID="ddlLoanRole" runat="server" DataValueField="RoleId" DataTextField="Name" Width="140px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 100px;">
                        <input id="btnSave" type="button" value="Save" class="Btn-66" onclick="btnSave_onclick()" />
                    </td>
                    <td style="text-align: right;">
                    		<webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="25" CssClass="AspNetPager"
				                UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
				                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
				                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
				            </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        
        <div style="height: 440px; overflow: auto;">
        <div id="divLoanTeamList" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridLoanTeamList" runat="server" EmptyDataText="There is no loan team." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <ItemTemplate>
                            <input id="chkSelected" type="checkbox" UserID="<%# Eval("UserID")%>" onclick="CheckOne(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="200px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('BranchName')" style="text-decoration: underline;">Branch</a><span id="spOrderByFlag1" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <span title="<%# Eval("BranchName")%>"> <%# (Eval("BranchName")!= DBNull.Value && Eval("BranchName").ToString().Length > 30) ? Eval("BranchName").ToString().Substring(0, 30) + "..." : Eval("BranchName")%></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('UserRole')" style="text-decoration: underline;">User Role</a><span id="spOrderByFlag2" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("UserRole")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-Width="200px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('FullName')" style="text-decoration: underline;">Name</a><span id="spOrderByFlag3" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("FullName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        </div>
    </div>
    </form>
</body>
</html>