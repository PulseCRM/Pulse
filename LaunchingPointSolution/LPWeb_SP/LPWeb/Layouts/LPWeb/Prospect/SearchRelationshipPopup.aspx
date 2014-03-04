<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchRelationshipPopup.aspx.cs" Inherits="Prospect_SearchRelationshipPopup" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Search Relationship Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        // <!CDATA[

        $(document).ready(function () {

            InitSearchInput();

            $("#form1").validate();

            // add event
            $("#gridContactList tr:not(:first) td :input[id^='ddlToRelationship']").change(ddlToRelationship_onchange);
        });

        function InitSearchInput() {

            // LastName
            var LastName = GetQueryString1("LastName");
            if (LastName != "") {
                $("#txtLastName").val(LastName);
            }

            // FirstName
            var FirstName = GetQueryString1("FirstName");
            if (FirstName != "") {
                $("#txtFirstName").val(FirstName);
            }
        }

        function btnSearch_onclick() {

            var ContactID = GetQueryString1("ContactID");
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            var sid = GetQueryString1("sid");

            // 参数字符串
            var sQueryStrings = "?sid=" + sid + "&ContactID=" + ContactID + "&CloseDialogCodes=" + CloseDialogCodes;

            // LastName
            var LastName = $.trim($("#txtLastName").val());
            if (LastName != "") {
                sQueryStrings += "&LastName=" + encodeURIComponent(LastName);
            }

            // FirstName
            var FirstName = $("#txtFirstName").val();
            if (FirstName != "") {
                sQueryStrings += "&FirstName=" + encodeURIComponent(FirstName);
            }

            if (LastName == "" && FirstName == "") {

                alert("Please enter search condition at first.");
                return;
            }

            // PageIndex
            var PageIndex = GetQueryString1("PageIndex");
            if (PageIndex != "") {
                sQueryStrings += "&PageIndex=" + PageIndex;
            }

            if (sQueryStrings == "") {

                window.location.href = window.location.href;
            }
            else {

                window.location.href = window.location.pathname + sQueryStrings;
            }
        }


        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridContactList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridContactList tr td :checkbox").attr("checked", "");
            }
        }

        function CheckOne(CheckBox) {

            if (CheckBox.checked) {

                $(CheckBox).parent().parent().find(":input[id^='ddlToRelationship']").rules("add", {
                    required: true,
                    messages: {
                        required: "*"
                    }
                });
            }
            else {

                $(CheckBox).parent().parent().find(":input[id^='ddlToRelationship']").rules("remove");
            }
        }

        function ddlToRelationship_onchange() {

            var ToRelationship = $(this).val();
            //alert(ToRelationship);

            if (ToRelationship.toLowerCase() == "nephew" || ToRelationship.toLowerCase() == "niece") {

                $(this).parent().parent().find("td.myToRelationship").empty();
                $(this).parent().parent().find("td.myToRelationship").append("<select id='ddlFromRelationship' name='ddlToRelationship' style='width: 100px;'><option value='Uncle'>Uncle</option><option value='Aunt'>Aunt</option></select>");
            }
            else if (ToRelationship.toLowerCase() == "uncle" || ToRelationship.toLowerCase() == "aunt") {

                $(this).parent().parent().find("td.myToRelationship").empty();
                $(this).parent().parent().find("td.myToRelationship").append("<select id='ddlFromRelationship' name='ddlToRelationship' style='width: 100px;'><option value='Nephew'>Nephew</option><option value='Niece'>Niece</option></select>");
            }
            else {

                $(this).parent().parent().find("td.myToRelationship").empty();
            }
        }

        function BeforeAddRelationship() {

            var SelectedCount = $("#gridContactList tr:not(:first) td :checkbox:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No contact was selected.");
                return false;
            }

            // call validate
            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return;
            }

            // ToContactIDs and ToRelationship
            var ToContactIDs = "";
            var ToRelationships = "";
            var FromRelationships = "";
            $("#gridContactList tr:not(:first) td :checkbox:checked").each(function (i) {

                var ToContactID = $(this).attr("myContactID");
                var ToRelationship = $(this).parent().parent().find(":input[id^='ddlToRelationship']").val();

                var FromRelationship = "";
                if ($(this).parent().parent().find(":input[id='ddlFromRelationship']").length == 1) {

                    FromRelationship = $(this).parent().parent().find(":input[id='ddlFromRelationship']").val();
                }

                if (i == 0) {

                    ToContactIDs = ToContactID;
                    ToRelationships = ToRelationship;
                    FromRelationships = FromRelationship;
                }
                else {

                    ToContactIDs += "," + ToContactID;
                    ToRelationships += "," + ToRelationship;
                    FromRelationships += "," + FromRelationship;
                }
            });

            $("#hdnToContactIDs").val(ToContactIDs);
            $("#hdnToRelationships").val(ToRelationships);
            $("#hdnFromRelationships").val(FromRelationships);

            //alert($("#hdnToContactIDs").val());
            //alert($("#hdnToRelationships").val());
            //alert($("#hdnFromRelationships").val());

            return true;
        }

        function Cancel() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }
        // ]]>
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="border: solid 0px red; width: 500px;">
        <table style="margin-top: 5px;">
            <tr>
                <td style="width: 180px;">Client: <asp:Label ID="lbClient" runat="server" Text="Label"></asp:Label></td>
                <td>DOB: <asp:Label ID="lbDOB" runat="server" Text="Label"></asp:Label></td>
            </tr>
        </table>
        <div style="margin-top: 5px; padding-left: 5px;">
            Address: <asp:Label ID="lbAddress" runat="server" Text="Label"></asp:Label>
        </div>
        <table style="margin-top: 20px;">
            <tr>
                <td style="width: 58px;">Last Name:</td>
                <td style="width: 130px;">
                    <input id="txtLastName" type="text" style="width: 100px;" />
                </td>
                <td style="width: 58px;">First Name:</td>
                <td style="width: 110px;">
                    <input id="txtFirstName" type="text" style="width: 100px;" />
                </td>
                <td>
                    <input id="btnSearch" type="button" value="Search" class="Btn-66" onclick="btnSearch_onclick()" />
                </td>
            </tr>
        </table>
        <table style="margin-top: 10px;">
            <tr>
                <td>
                    <asp:Button ID="btnAddRelationship" runat="server" Text="Add Relationship" CssClass="Btn-140" OnClientClick="return BeforeAddRelationship();" onclick="btnAddRelationship_Click" />
                </td>
                <td>
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="Cancel()" />
                </td>
            </tr>
        </table>
        <div style="text-align: right;">
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="10" CssClass="AspNetPager"
                UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
            </webdiyer:AspNetPager>
        </div>
        <div id="divContactList" class="ColorGrid" style="margin-top: 10px; width: 500px;">
            <asp:GridView ID="gridContactList" runat="server" DataSourceID="ContactSqlDataSource" AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkChecked" type="checkbox" onclick="CheckOne(this)" myContactID="<%# Eval("ContactId") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ContactName" HeaderText="Contact" />
                    <asp:TemplateField HeaderText="Relationship to Contact" ItemStyle-Width="115px">
                        <ItemTemplate>
                            <select id="ddlToRelationship<%# Eval("ContactId") %>" name="ddlToRelationship<%# Eval("ContactId") %>" style="width: 100px;">
                                <%# this.GetOptions_ToRelationship()%>
                            </select>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Relationship to Client" ItemStyle-Width="110px" ItemStyle-CssClass="myToRelationship">
                        <ItemTemplate>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
        <asp:SqlDataSource ID="ContactSqlDataSource" runat="server" SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
            <SelectParameters>
                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="ContactID" />
                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                <asp:Parameter Name="DbTable" Type="String" DefaultValue="(select LastName +', '+ FirstName + case when MiddleName is null then '' when MiddleName='' then '' else ' '+ MiddleName end as ContactName,* from Contacts) t" />
                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="10" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <asp:HiddenField ID="hdnToContactIDs" runat="server" />
    <asp:HiddenField ID="hdnToRelationships" runat="server" />
    <asp:HiddenField ID="hdnFromRelationships" runat="server" />
    </form>
</body>
</html>