<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectContactsPopup.aspx.cs" Inherits="Contact_SelectContactsPopup"  %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Select Partner Contact</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {


        });

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridPartnerContactList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridPartnerContactList tr td :checkbox").attr("checked", "");
            }
        }

        function BeforeSubmit() {

            if ($("#gridPartnerContactList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("Please select a partner Contact.");
                return false;
            }

            var type = GetQueryString1("type");

            var SelectedContactIDs = "";
            var SelectedName = "";
            if ($("#gridPartnerContactList tr:not(:first) td :checkbox:checked").length > 1 && type == "prospectdetail") {

                alert("You can select only one record.");
                return false;
            }
            if ($("#gridPartnerContactList tr:not(:first) td :checkbox:checked").length > 1 && type == "ProspectLoanDetailInfo") {

                alert("You can select only one record.");
                return false;
            }
            $("#gridPartnerContactList tr:not(:first) td :checkbox:checked").each(function (i) {

                var ContactID = $(this).attr("ContactID");
                var ContactName = $(this).attr("ContactName");

                //alert(ContactID);
                if (i == 0) {

                    SelectedContactIDs = ContactID;
                    SelectedName = ContactName;
                }
                else {

                    SelectedContactIDs += "," + ContactID;
                    SelectedName += "," + ContactName;
                }
            });

            //alert(SelectedContactIDs);

            $("#hdnSelectedContactIDs").val(SelectedContactIDs);
            $("#hdnSelectedContactName").val(SelectedName);

            return true;
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }


// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="PopupContainer">
         
        <div id="divButtons" style="margin-top: 20px;">
            <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="Btn-66" OnClientClick="return BeforeSubmit();" onclick="btnSelect_Click" />&nbsp;
            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
        </div>
        <div id="divFilter" style="margin-top: 20px;">
               <asp:DropDownList ID="ddlFilter" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="ddlFilter_SelectedIndexChanged" >
                <asp:ListItem Text="" Value=""></asp:ListItem>
                <asp:ListItem Text="A" Value="A"></asp:ListItem>
                <asp:ListItem Text="B" Value="B"></asp:ListItem>
                <asp:ListItem Text="C" Value="C"></asp:ListItem>
                <asp:ListItem Text="D" Value="D"></asp:ListItem>
                <asp:ListItem Text="E" Value="E"></asp:ListItem>
                <asp:ListItem Text="F" Value="F"></asp:ListItem>
                <asp:ListItem Text="G" Value="G"></asp:ListItem>
                <asp:ListItem Text="H" Value="H"></asp:ListItem>
                <asp:ListItem Text="I" Value="I"></asp:ListItem>
                <asp:ListItem Text="J" Value="J"></asp:ListItem>
                <asp:ListItem Text="K" Value="K"></asp:ListItem>
                <asp:ListItem Text="L" Value="L"></asp:ListItem>
                <asp:ListItem Text="M" Value="M"></asp:ListItem>
                <asp:ListItem Text="N" Value="N"></asp:ListItem>
                <asp:ListItem Text="O" Value="O"></asp:ListItem>
                <asp:ListItem Text="P" Value="P"></asp:ListItem>
                <asp:ListItem Text="Q" Value="Q"></asp:ListItem>
                <asp:ListItem Text="R" Value="R"></asp:ListItem>
                <asp:ListItem Text="S" Value="S"></asp:ListItem>
                <asp:ListItem Text="T" Value="T"></asp:ListItem>
                <asp:ListItem Text="U" Value="U"></asp:ListItem>
                <asp:ListItem Text="V" Value="V"></asp:ListItem>
                <asp:ListItem Text="W" Value="W"></asp:ListItem>
                <asp:ListItem Text="X" Value="X"></asp:ListItem>
                <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                <asp:ListItem Text="Z" Value="Z"></asp:ListItem>
               </asp:DropDownList>
            </div>
        <div id="divGridContainer" style="margin-top: 5px;">
            
            <div  style="letter-spacing: 1px; text-align: right; font-size: 12px; height:20px;" >
                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                    OnPageChanged="AspNetPager1_PageChanged">
                </webdiyer:AspNetPager>
            </div>
            <div id="divPartnerContactList" class="ColorGrid">
                <asp:GridView ID="gridPartnerContactList" runat="server" EmptyDataText="There is no partner contact." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" ContactID="<%# Eval("ContactId") %>" ContactName="<%# Eval("ContactName") %>"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ContactName" ControlStyle-Width="100px" HeaderText="Contact" />
                        <asp:BoundField DataField="ContactType" ControlStyle-Width="60px" HeaderText="Contact Type" />
                        <asp:BoundField DataField="ServiceType" ControlStyle-Width="60px" HeaderText="Service Type" />
                        <asp:BoundField DataField="BranchName" ControlStyle-Width="100px" HeaderText="Branch" />
                        <asp:BoundField DataField="CompanyName" ControlStyle-Width="100px" HeaderText="Company" />
                    </Columns>
                </asp:GridView>
                <div class="GridPaddingBottom">&nbsp;</div>
            </div>
        </div>
        <asp:HiddenField ID="hdnSelectedContactIDs" runat="server" />
        <asp:HiddenField ID="hdnSelectedContactName" runat="server" />
    </div>
    </form>
</body>
</html>