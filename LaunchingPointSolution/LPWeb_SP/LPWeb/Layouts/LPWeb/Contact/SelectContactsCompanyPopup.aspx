<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectContactsCompanyPopup.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Contact.SelectContactsCompanyPopup"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Select Contact Company Popup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {


        });

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gvCompanyList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gvCompanyList tr td :checkbox").attr("checked", "");
            }
        }

        function BeforeSubmit() {

            if ($("#gvCompanyList tr:not(:first) td :checkbox:checked").length == 0) {

                alert("Please select a partner Contact.");
                return false;
            }

            var SelectContactBranchId = "";
            var SelectedName = "";
            var SelectedAddress = "";
            var SelectedCity = "";
            var SelectedState = "";
            var SelectedPhone = "";
            var SelectedZip = "";

            if ($("#gvCompanyList tr:not(:first) td :checkbox:checked").length > 1) {

                alert("You can select only one record.");
                return false;
            }
            $("#gvCompanyList tr:not(:first) td :checkbox:checked").each(function (i) {

                var BranchName = $(this).attr("BranchName");
                var ContactBranchId = $(this).attr("ContactBranchId");
                var Address = $(this).attr("Address");
                var City = $(this).attr("City");
                var State = $(this).attr("State");
                var Phone = $(this).attr("Phone");
                var Zip = $(this).attr("Zip");
                //alert(ContactID);
                if (i == 0) {
                    SelectContactBranchId = ContactBranchId;
                    SelectedName = BranchName;
                    SelectedAddress = Address;
                    SelectedCity = City;
                    SelectedState = State;
                    SelectedPhone = Phone;
                    SelectedZip = Zip;
                }
                else {
                    SelectContactBranchId += "," + ContactBranchId;
                    SelectedName += "," + BranchName;
                    SelectedAddress += "," + Address;
                    SelectedCity += "," + City;
                    SelectedState += "," + State;
                    SelectedPhone += "," + Phone;
                    SelectedZip += "," + Zip;
                }
            });

            if (window.parent.SelectBranchName != null) {

                window.parent.SelectBranchName(SelectedName, SelectContactBranchId, SelectedAddress, SelectedCity, SelectedState, SelectedPhone, SelectedZip);

            } else if (window.parent.parent.SelectBranchName != null) {
                window.parent.parent.SelectBranchName(SelectedName, SelectContactBranchId, SelectedAddress, SelectedCity, SelectedState, SelectedPhone, SelectedZip);
            }
            btnCancel_onclick();
            return false;
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
    <div id="PopupContainer" style=" margin-left:10px;">
         
        
        <div id="divFilter" style="margin-top: 15px; ">
              <table>
                <tr>
                    <td>Service Type:</td>
                    <td>
                        <asp:DropDownList ID="ddlServiceType" runat="server" Width="175">
                        </asp:DropDownList>
                    </td>
                    <td style=" width:15px;"></td>
                    <td colspan ="2"></td>
                </tr>
                <tr>
                    <td>Company:</td>
                    <td>
                        <asp:TextBox ID="txbCompany" runat="server" Width="170"></asp:TextBox>
                    </td>
                    <td style=" width:15px;"></td>
                    <td>Branch:</td>
                    <td>
                        <asp:TextBox ID="txbBranch" runat="server" Width="170"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td>City:</td>
                    <td>
                        <asp:TextBox ID="txbCity" runat="server" Width="80"></asp:TextBox>
                        State:
                        <asp:DropDownList ID="ddlState" runat="server" Width="60">
                        </asp:DropDownList>
                    </td>
                    <td style=" width:15px;"></td>
                    <td >
                        <asp:Button ID="btSearch" runat="server"  CssClass="Btn-66" Text="Search" OnClick="btSearch_Click" /></td>
                    <td> </td>
                </tr>
              </table> 
         </div>
        <div id="divButtons" style="margin-top: 20px;">
             <input ID="btnSelect" type="button" value="Select" class="Btn-66" onclick="return BeforeSubmit();" />
        </div>
        <div id="divGridContainer" style="margin-top: 5px; width:300px;"  class="ColorGrid" >

                <asp:GridView ID="gvCompanyList" runat="server" EmptyDataText="There is no Branch." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid tablesorter" GridLines="None" Width="300">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="chkChecked" type="checkbox" ContactBranchId = "<%# Eval("ContactBranchId") %>"  BranchName="<%# Eval("BranchName") %>" Address="<%# Eval("Address") %>" Zip="<%# Eval("Zip") %>"
                                City="<%# Eval("City") %>" State="<%# Eval("State") %>" Phone="<%# Eval("Phone") %>"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="BranchName" ControlStyle-Width="100px" HeaderText="Branch" />
                        
                    </Columns>
                </asp:GridView>
                
        </div>
    </div>
    </form>
</body>
</html>