<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignLoanContactPopup.aspx.cs" Inherits="LoanDetails_AssignLoanContactPopup" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Assign Loan Contact</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#ddlContactRole").attr("disabled", "true");
            InitSearchInput();

        });

        function InitSearchInput() {

            // ServiceType
            var ServiceType = GetQueryString1("ServiceType");
            if (ServiceType != "") {
                $("#ddlServiceType").val(ServiceType);
            }

            // Company
            var Company = GetQueryString1("Company");
            if (Company != "") {
                $("#txtCompanyName").val(Company);
            }

            // Zip
            var Zip = GetQueryString1("Zip");
            if (Zip != "") {
                $("#txtZip").val(Zip);
            }

            // OrderByField
            var OrderByField = GetQueryString1("OrderByField");
            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByField != "") {

                if (OrderByField == "ServiceTypes") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag1").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag1").text("▼");
                    }
                }
                else if (OrderByField == "CompanyName") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag2").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag2").text("▼");
                    }
                }
                else if (OrderByField == "ContactsName") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag3").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag3").text("▼");
                    }
                }
                else if (OrderByField == "MailingCity") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag4").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag4").text("▼");
                    }
                }
                else if (OrderByField == "MailingZip") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag5").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag5").text("▼");
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

            // ServiceType
            var ServiceType = $("#ddlServiceType").val();
            if (ServiceType != "All") {
                sQueryStrings += "&ServiceType=" + encodeURIComponent(ServiceType);
            }

            // Company
            var Company = $.trim($("#txtCompanyName").val());
            if (Company != "") {
                sQueryStrings += "&Company=" + encodeURIComponent(Company);
            }

            // Zip
            var Zip = $.trim($("#txtZip").val());
            if (Zip != "") {
                sQueryStrings += "&Zip=" + encodeURIComponent(Zip);
            }

            if (ServiceType == "" && CompanyName == "" && Zip == "") {

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

            $("#gridLoanContactList tr td :checkbox").attr("checked", "");

            if (IsChecked == true) {

                $(CheckBox).attr("checked", "true");
            }
            else {

                $(CheckBox).attr("checked", "");
            }

            if ($("#gridLoanContactList tr td :checkbox:checked").length == 1) {

                $("#ddlContactRole").attr("disabled", "");
            }
            else {

                $("#ddlContactRole").attr("disabled", "true");
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

                sQueryStrings += "&OrderByType=1";
            }
            else if (OrderByType == "0") {

                sQueryStrings += "&OrderByType=1";
            }
            else if (OrderByType == "1") {

                sQueryStrings += "&OrderByType=0";
            }

            window.location.href = window.location.pathname + sQueryStrings;
        }

        function btnSave_onclick() {

            if ($("#gridLoanContactList tr td :checkbox:checked").length == 0) {

                alert("Please select a contact.");
                return;
            }

            var ContactRoleID = $("#ddlContactRole").val();
            if (ContactRoleID == "0") {

                alert("Please select Contact Role.");
                return;
            }

            var ContactID = $("#gridLoanContactList tr td :checkbox:checked").attr("ContactID");

            var GetIDsFunction = GetQueryString1("GetIDsFunction");
            //alert(GetIDsFunction);

            var InvokeStr = GetIDsFunction + "('" + ContactID + "', '" + ContactRoleID + "')";
            //alert(InvokeStr);

            eval(InvokeStr);
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        <div id="divModuleName" class="ModuleTitle">Assign Loan Contact</div>

        <div id="divFilter" style="margin-top: 10px; margin-left: 10px;">
            <table>
                <tr>
                    <td style="width: 255px;">Borrower: <asp:Label ID="lbBorrower" runat="server" Text="Label"></asp:Label></td>
                    <td>Property: <asp:Label ID="lbProperty" runat="server" Text="Label"></asp:Label></td>
                    
                </tr>
            </table>
            <table style="margin-top: 5px;">
                <tr>
                    <td style="width: 70px;">Service Type:</td>
                    <td style="width: 180px;">
                        <asp:DropDownList ID="ddlServiceType" runat="server" DataValueField="Name" DataTextField="Name" Width="160px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 51px;">Company:</td>
                    <td style="width: 190px;">
                        <asp:TextBox ID="txtCompanyName" runat="server" Width="170px"></asp:TextBox>
                    </td>
                    <td style="width: 22px;">Zip:</td>
                    <td style="width: 70px;">
                        <asp:TextBox ID="txtZip" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <td>
                        <input id="btnDisplay" type="button" value="Display" class="Btn-66" onclick="btnSearch_onclick()" />
                    </td>
                </tr>
            </table>
            <table style="margin-top: 10px; width: 100%;">
                <tr>
                    <td style="width: 70px;">Contact Role:</td>
                    <td style="width: 180px;">
                        <asp:DropDownList ID="ddlContactRole" runat="server" DataValueField="ContactRoleId" DataTextField="Name" Width="160px">
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
        <div id="divLoanContactList" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridLoanContactList" runat="server" EmptyDataText="There is no contact." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <ItemTemplate>
                            <input id="chkSelected" type="checkbox" ContactID="<%# Eval("ContactID")%>" onclick="CheckOne(this)" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('ServiceTypes')" style="text-decoration: underline;">Service Type</a><span id="spOrderByFlag1" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("ServiceTypes")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('CompanyName')" style="text-decoration: underline;">Company</a><span id="spOrderByFlag2" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("CompanyName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="180px" ItemStyle-Width="180px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('ContactsName')" style="text-decoration: underline;">Name</a><span id="spOrderByFlag3" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("ContactsName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-Width="100px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('MailingCity')" style="text-decoration: underline;">City</a><span id="spOrderByFlag4" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("MailingCity")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-Width="60px">
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('MailingZip')" style="text-decoration: underline;">Zip</a><span id="spOrderByFlag5" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("MailingZip")%>
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