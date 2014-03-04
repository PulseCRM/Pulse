<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmploymentDetailPopup.aspx.cs" Inherits="Prospect_EmploymentDetailPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Client Detail</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        jQuery.validator.addMethod("phoneUS", function (phone_number, element) {
            phone_number = phone_number.replace(/\s+/g, "");
            return this.optional(element) || phone_number.length > 9 &&
		phone_number.match(/^(1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/);
        }, "Please enter valid phone number");

        jQuery.validator.addMethod("MMYYDate", function (date, element) {
            if (jQuery.trim(date) == "")
                return true;
            if (date.indexOf('/') < 0)
                return false;
            var array = date.split('/');
            if (array.length != 2) {
                return false;
            }
            if (isNaN(array[0]) || isNaN(array[1]) || array[0] > 12 || array[0] < 1) {
                return false;
            }
            return true;
        }, "Please enter valid date");

        $(document).ready(function () {
            $("#txtYearsExp").onlypressnum();
            $("#txtSalary").change(txtSalary_onchange);
            AddValidators();
        });

        function txtSalary_onchange() {
            var value = $("#txtSalary").val();
            var fvalue = parseFloat(value);
            if (isNaN(fvalue)) {
                $("#txtSalary").val("");
                return;
            }
            fvalue = Math.round(fvalue * 100) / 100;
            $("#txtSalary").val(fvalue);
        }

        // add jQuery Validators
        function AddValidators() {

            $("#EmploymentDetailForm").validate({

                rules: {

                    txtEmployer: {
                        required: true
                    },
                    txtPisition: {
                        required: true
                    },
                    txtStart: {
                        MMYYDate: true
                    },
                    txtEnd: {
                        MMYYDate: true
                    },
                    txtPhone: {
                        phoneUS: true
                    }
                },
                messages: {

                    txtEmployer: {
                        required: "*"
                    },
                    txtPisition: {
                        required: "*"
                    },
                    txtStart: {
                        MMYYDate: "<span title='Please enter valid date.' style='cursor: pointer;'>?<span>"
                    },
                    txtEnd: {
                        MMYYDate: "<span title='Please enter valid date.' style='cursor: pointer;'>?<span>"
                    },
                    txtPhone: {
                        phoneUS: "<span title='Please enter valid phone number.' style='cursor: pointer;'>?<span>"
                    }
                }
            });
        }

        function BeforeSave() {
            return true;
        }

        function CancelOnClick() {
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

        function CloneData() {
            var fileId = jQuery("#<%= this.txtEmployer.ClientID %>").val();
            jQuery("#<%= this.txtEmployer.ClientID %>").val("Copy of " + fileId);
            jQuery("#<%= this.hfdEmploymentId.ClientID %>").val("-1");
            jQuery("#<%= this.hfdBranchContractId.ClientID %>").val(""); 
            return false;
        }

        function ShowSelectContactPopup() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Contact/SelectContactsCompanyPopup.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 550;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight =400;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            $("#ifrSearchContact").attr("src", iFrameSrc);
            $("#ifrSearchContact").attr("width", iFrameWidth);
            $("#ifrSearchContact").attr("height", iFrameHeight);

            // show modal
            $("#divSearchContact").dialog({
                height: divHeight,
                width: divWidth,
                title: "Select Contact Company",
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function SelectBranchName(SelectedName, SelectContactBranchId, SelectedAddress, SelectedCity, SelectedState, SelectedPhone, SelectedZip) {
            jQuery("#<%= this.hfdBranchContractId.ClientID %>").val(SelectContactBranchId);
            jQuery("#<%= this.txtEmployer.ClientID %>").val(SelectedName);
            jQuery("#<%= this.txtAddress.ClientID %>").val(SelectedAddress);
            jQuery("#<%= this.txtCity.ClientID %>").val(SelectedCity);
            jQuery("#<%= this.ddlState.ClientID %>").val(SelectedState);
            jQuery("#<%= this.txtPhone.ClientID %>").val(SelectedPhone);
            jQuery("#<%= this.txtZip.ClientID %>").val(SelectedZip);
        }

        function CloseGlobalPopup() {
            $("#divSearchContact").dialog("close");
            $("#divSearchContact").dialog("destroy");
        }
    </script>
</head>
<body>
    <form id="EmploymentDetailForm" runat="server">
    <asp:HiddenField ID="hfdEmploymentId" runat="server" />
    <asp:HiddenField ID="hfdContractId" runat="server" />
    <asp:HiddenField ID="hfdBranchContractId" runat="server" />
    <div id="divContainer" style="width: 780px; height: 400px; border: solid 0px red;">
        <div id="divBtns">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnClone" runat="server" Text="Clone" CssClass="Btn-91" OnClientClick="CloneData()" />
                    </td>
                    <td>
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="CancelOnClick()"/>
                    </td>
                </tr>
            </table>
            
        </div>
        <div id="divDetails" style="margin-top: 10px;">
            <table>
                <tr>
                    <td style="width: 70px;">Employer/<br>Company:</td>
                    <td colspan="6">
                        <asp:TextBox ID="txtEmployer" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                    <td colspan="1">
                        <input id="btnSelect" type="button" value="Select" class="Btn-91" onclick="ShowSelectContactPopup()"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px;">Position:</td>
                    <td colspan="6">
                        <asp:TextBox ID="txtPisition" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                    </td>
                    <td colspan="1">
                        <asp:CheckBox ID="checkSelf" runat="server" Text="  Self-Employed"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px;">Start:</td>
                    <td>
                        <asp:TextBox ID="txtStart" runat="server" MaxLength="5" Width="70%"></asp:TextBox>
                    </td>
                    <td>End:</td>
                    <td>
                        <asp:TextBox ID="txtEnd" runat="server" MaxLength="5" Width="70%"></asp:TextBox>
                    </td>
                    <td colspan="3" style="text-align:right">Monthly Income:</td>
                    <td colspan="1">
                        <asp:TextBox ID="txtSalary" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                    </tr>
                <tr>
                    <td style="width: 70px;">Phone:</td>
                    <td colspan ="2">
                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" Width="98%"></asp:TextBox>
                    </td>
                    <td colspan="2" style="text-align:right">Years in profession:</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtYearsExp" runat="server" MaxLength="2" Width="93%"></asp:TextBox>
                    </td>
                    <td></td>
                 </tr>
                <tr>
                    <td style="width: 70px;">Business<br>Type:</td>
                    <td colspan ="2">
                        <asp:TextBox ID="txtBusinessType" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                    <td colspan="4">
                        <asp:CheckBox ID="checkVerify" runat="server" Text="  Verify Taxes"></asp:CheckBox>
                    </td>
                    <td></td>
                 </tr>

                    <tr>
                    <td style="width: 70px;">Address:</td>
                    <td colspan="6">
                        <asp:TextBox ID="txtAddress" runat="server"  MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 70px;">City:</td>
                    <td colspan="3">
                        <asp:TextBox ID="txtCity" runat="server" MaxLength="100" Width="98%"></asp:TextBox>
                    </td>
                    <td>State:</td>
                    <td><asp:DropDownList ID="ddlState"  runat="server">
                        </asp:DropDownList></td>
                    <td style="text-align:right">Zip:</td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
            </table>
            
        </div>
    </div>
    </form>
    <div id="divSearchContact" title="Select Contact Company" style="display: none;"> 
        <iframe id="ifrSearchContact" frameborder="0" style="overflow: SCROLL; overflow-x: HIDDEN">
        </iframe>
    </div>
</body>
</html>