<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanContactDetail.aspx.cs"
    Inherits="LoanContactDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan Contact Detail</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            AddValidators();
        });

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {
                    txbFirstName: {
                        required: true
                    },
                    txbLastName: {
                        required: true
                    }
                },
                messages: {
                    txbFirstName: {
                        required: "*"
                    },
                    txbLastName: {
                        required: "*"
                    }
                }
            });
        }
        // cancel
        function btnCancel_onclick() {

            window.parent.DialogContactEditClose();
        }

        function BeforeSave() {

            return true;
        }
        function BeforeDelete() {

            var r1 = confirm("Are you sure you want to remove the selected contact from the loan?");
            if (r1 == false) {

                return false;
            }
            return true;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="margin-top: 10px; margin-left: 15px; width: 700px">
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px;">
                            First Name:
                        </td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txbFirstName" runat="server" Width="150px" MaxLength="255"></asp:TextBox>
                        </td>
                        <td style="width: 100px; padding-left: 10px;">
                            Last Name:
                        </td>
                        <td style="width: 135px; padding-left: 10px;">
                            <asp:TextBox ID="txbLastName" runat="server" Width="110px" MaxLength="255"></asp:TextBox>
                        </td>
                        <td>
                            <asp:CheckBox ID="ckEnable" Text=" Enable" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
             <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px;">
                            Service:
                        </td>
                        <td style="width:250px;">
                            <asp:DropDownList ID="ddlServiceTypes" runat="server" Height="16px" Width="240px" EnableViewState="true" OnSelectedIndexChanged="ddlServiceTypes_onSelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px; padding-left: 10px;">
                            Fax:
                        </td>
                        <td style="width: 135px; padding-left: 10px;">
                            <asp:TextBox ID="txbFax" runat="server" Width="120px" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px;">
                            Partner Company:
                        </td>
                        <td colspan="4">
                             <asp:DropDownList ID="ddlCompany" Width="240px" runat="server" EnableViewState="true" OnSelectedIndexChanged="ddlCompany_onSelectedIndexChanged" AutoPostBack="true">
                            </asp:DropDownList>
                            &nbsp;
                       </td>
                    </tr>
                    <tr>
                        <td style="width: 120px;">
                            Partner Branch:
                        </td>
                        <td colspan="4">
                          <asp:DropDownList ID="ddlBranch" Width="240px" runat="server" EnableViewState="true" OnSelectedIndexChanged="ddlBranch_onSelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                           &nbsp;
                        </td>
                     </tr>
                </table>
            </div>
 
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px;">
                            Bussiness Phone:
                        </td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txbBPhone" runat="server" Width="150px" MaxLength="255"></asp:TextBox>
                        </td>
                        <td style="width: 100px; padding-left: 10px;">
                            Cell Phone:
                        </td>
                        <td style="width: 135px; padding-left: 10px;">
                            <asp:TextBox ID="txbCPhone" runat="server" Width="120px" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <asp:HiddenField ID="hdnContactID" runat="server" />
            <asp:HiddenField ID="hdnCompany" runat="server" />
            <asp:HiddenField ID="hdnBranchId" runat="server" />
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px;">
                            Email:
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txbEmail" runat="server" Width="240px" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px;">
                            Address:
                        </td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txbAddress" runat="server" Width="240px" MaxLength="255"></asp:TextBox>
                        </td>
                        <td style="width: 100px; padding-left: 10px;">
                            City:
                        </td>
                        <td style="width: 135px; padding-left: 10px;">
                            <asp:TextBox ID="txbCity" runat="server" Width="120px" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 9px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 120px;">
                            State :
                        </td>
                        <td style="width:250px;">
                             <asp:DropDownList ID="ddlState" runat="server" Width="150px" Height="16px">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 100px; padding-left: 10px;">
                            Zip :
                        </td>
                        <td style="width: 135px; padding-left: 10px;">
                            <asp:TextBox ID="txbZip" runat="server" Width="120px" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()"
                                OnClick="btnSave_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDelete()"
                                OnClick="btnDelete_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
