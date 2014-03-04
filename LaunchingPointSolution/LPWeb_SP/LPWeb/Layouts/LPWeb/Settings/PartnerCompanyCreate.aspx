<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerCompanyCreate.aspx.cs" Inherits="Settings_PartnerCompanyCreate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Partner Company Setup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    

    <script src="../js/jquery.js" type="text/javascript"></script>

    
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {


        });



        function BeforeSave() {

            var CompanyName = $.trim($("#txtCompanyName").val());
            if (CompanyName == "") {

                alert("Please select Company Name.");
                return false;
            }

            return true;
        }

        function btnCancel_onclick() {

            window.parent.CloseGlobalPopup();
        }

        function btnCreateLoanProgram_onclick() {

            $("#divContainer").block({ message: $('#divCreateLoanProgram'), css: { width: '400px'} });
        }

        function btnCreate2_onclick() {

            var LoanProgram = $.trim($("#txtLoanProgram").val());
            if (LoanProgram == "") {

                alert("Please enter Loan Program.");
                return;
            }
            var IsARM = $("#chkIsARM").attr("checked");

            var sid = Math.random().toString().substr(2);

            $.getJSON("LoanProgramCreateAjax.aspx?sid=" + sid + "&LoanProgram=" + encodeURIComponent(LoanProgram) + "&IsARM=" + IsARM, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return;
                }

                window.location.href = window.location.href;
            });
        }

        function btnCreate3_onclick() {

            var LoanProgram = $.trim($("#txtLoanProgram").val());
            if (LoanProgram == "") {

                alert("Please enter Loan Program.");
                return;
            }
            var IsARM = $("#chkIsARM").attr("checked");

            var sid = Math.random().toString().substr(2);

            $.getJSON("LoanProgramCreateAjax.aspx?sid=" + sid + "&LoanProgram=" + encodeURIComponent(LoanProgram) + "&IsARM=" + IsARM, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return;
                }

                // success
                alert("Create loan program successfully.");

                $("#txtLoanProgram").val("");
                $("#chkIsARM").attr("checked", "true");
            });
        }

        function btnCancel2_onclick() {

            $('#txtLoanProgram').val("");

            $("#divContainer").unblock();
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="height:250px;">
        
        <table>
            <tr>
                <td style="width:90px;">Company Name:</td>
                <td>
                    <asp:TextBox ID="txtCompanyName" runat="server" MaxLength="255" Width="280px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width:90px;">Service Type:</td>
                <td style="width: 200px;">
                    <asp:DropDownList ID="ddlServiceType" runat="server" DataTextField="Name" DataValueField="ServiceTypeId" Width="170px">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:CheckBox ID="chkEnabled" runat="server" Text=" Enabled" Checked="true" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="width:90px;">Address:</td>
                <td>
                    <asp:TextBox ID="txtAddress" runat="server" MaxLength="255" Width="280px"></asp:TextBox>
                </td>
            </tr>
        </table>

        <table>
            <tr>
                <td style="width:90px;">City:</td>
                <td>
                   <asp:TextBox ID="txtCity" runat="server" MaxLength="50" Width="84px"></asp:TextBox>
                </td>
                <td style="padding-left: 16px;">State:</td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" Width="50">
                            <asp:ListItem Text="" Value=""></asp:ListItem>
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
                <td style="padding-left: 17px;">Zip:</td>
                <td>
                   <asp:TextBox ID="txtZip" runat="server" Width="49px" MaxLength="12"></asp:TextBox>
                </td>
            </tr>
        </table>

        <table>
            <tr>
                <td style="width:90px;">Website:</td>
                <td>
                    <asp:TextBox ID="txtWebsite" runat="server" MaxLength="255" Width="280px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return BeforeSave()" OnClick="btnSave_Click" CssClass="Btn-66" />

    </div>
    
    </form>
</body>
</html>