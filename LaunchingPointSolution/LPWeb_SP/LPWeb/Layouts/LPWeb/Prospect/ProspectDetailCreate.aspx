<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#"  MasterPageFile="~/_layouts/LPWeb/MasterPage/Pipeline.master" AutoEventWireup="true" CodeBehind="ProspectDetailCreate.aspx.cs"
    Inherits="Prospect_ProspectDetailCreate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <%--  <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>--%>

        <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script> 
    <script src="../js/jquery.tab.js" type="text/javascript"></script> 
    <script src="../js/urlparser.js" type="text/javascript"></script>


    <%--<script language="javascript" type="text/javascript">
// <![CDATA[

        jQuery.validator.addMethod("phoneUS", function (phone_number, element) {
            phone_number = phone_number.replace(/\s+/g, "");
            return this.optional(element) || phone_number.length > 9 &&
		phone_number.match(/^(1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/);
        }, "Please enter valid phone number");

        $(document).ready(function () {

            $(".DateField").datepick();

            $("#txtExperianScore").OnlyInt();
            $("#txtTransUnitScore").OnlyInt();
            $("#txtEquifaxScore").OnlyInt();

            AddValidators();
        });

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {

                    ddlLoanOfficer: {
                        required: true
                    },
                    txtFirstName: {
                        required: true
                    },
                    txtLastName: {
                        required: true
                    },
                    txtAddress: {
                        required: true
                    },
                    txtCity: {
                        required: true
                    },
                    ddlState: {
                        required: true
                    },
                    txtZip: {
                        required: true
                    },
                    txtEmail: {
                        email: true
                    },
                    txtDOB: {
                        date: true
                    },
                    txtHomePhone: {
                        phoneUS: true
                    },
                    txtCellPhone: {
                        phoneUS: true
                    },
                    txtBizPhone: {
                        phoneUS: true
                    },
                    txbAmount: {
                        number: true
                    },
                    txbInterestRate: {
                        number: true
                    }
                },
                messages: {

                    ddlLoanOfficer: {
                        required: "*"
                    },
                    txtFirstName: {
                        required: "*"
                    },
                    txtLastName: {
                        required: "*"
                    },
                    txtAddress: {
                        required: "*"
                    },
                    txtCity: {
                        required: "*"
                    },
                    ddlState: {
                        required: "*"
                    },
                    txtZip: {
                        required: "*"
                    },
                    txtEmail: {
                        email: "Please enter valid email."
                    },
                    txtDOB: {
                        date: "Please enter valid date."
                    },
                    txtHomePhone: {
                        phoneUS: "<span title='Please enter valid phone number.' style='cursor: pointer;'>?<span>"
                    },
                    txtCellPhone: {
                        phoneUS: "<span title='Please enter valid phone number.' style='cursor: pointer;'>?<span>"
                    },
                    txtBizPhone: {
                        phoneUS: "<span title='Please enter valid phone number.' style='cursor: pointer;'>?<span>"
                    },
                    txbAmount: {
                        number: "<span title='Please enter valid number.' style='cursor: pointer;'>?<span>"
                    },
                    txbInterestRate: {
                        number: "<span title='Please enter valid number.' style='cursor: pointer;'>?<span>"
                    }
                }
            });
        }

        function BeforeSave() {
            var sHP = $("#<%=txtHomePhone.ClientID %>").val();
            var sCP = $("#<%=txtCellPhone.ClientID %>").val();
            var sBP = $("#<%=txtBizPhone.ClientID %>").val();
            var sEmail = $("#<%=txtEmail.ClientID %>").val();
            if (sHP.length <= 0 && sCP.length <= 0 && sBP.length <= 0 && sEmail.length <= 0) {
                alert("Please provide any of the home phone, cell phone, business phone, or email.");
                return false;
            }
            return true;
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

        function SelectReferral() {
            ShowDialog_Search();
            return false;
        }

        //Show search contact popup
        function ShowDialog_Search() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var stype = "prospectdetail";
            var iFrameSrc = "../Contact/SelectContactsPopup.aspx?sid=" + RadomNum + "&pagesize=15&type=" + stype + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 550;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            $("#ifrSearchContact").attr("src", iFrameSrc);
            $("#ifrSearchContact").attr("width", iFrameWidth);
            $("#ifrSearchContact").attr("height", iFrameHeight);
            $("#divSearchContact").dialog({
                height: divHeight,
                width: divWidth,
                title: "Partner Contact Search",
                modal: false,
                resizable: false
            });


            //$(".ui-dialog").css("border", "solid 3px #aaaaaa")

            $("#divSearchContact").dialog("open");
            return false;

        }

        function ShowReferral(sContactID, sContactName) {

            CloseGlobalPopup();
            $("#txtReferral").val(sContactName);
            $("#hdnReferralID").val(sContactID);

        }

        function CloseGlobalPopup() {

            $("#divSearchContact").dialog("close");
            $("#divSearchContact").dialog("destroy");
        }

// ]]>
    </script>--%>


     <script language="javascript" type="text/javascript">

         $(document).ready(function () {

          
             SetTabByName();
         });

         function ChangeTab(tab_index) {

             // select tab
             $("#tabs10 #current").removeAttr("id");
             $("#tabs10 ul li").eq(tab_index).attr("id", "current");
         }

         function aBorrower_onclick(tab_index) {

             // common parameters
             var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
             var RadomNum = Math.random();
             var sid = RadomNum.toString().substr(2);

             // set iframe.src
             $("#tabFrame").attr("src", "ProspectBorrower.aspx?sid=" + sid + "&FileID=" + FileID);

             ChangeTab(tab_index);

             return false;
         }

         function aCoBorrower_onclick(tab_index) {

             // common parameters
             var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
             var RadomNum = Math.random();
             var sid = RadomNum.toString().substr(2);

             // set iframe.src
             $("#tabFrame").attr("src", "CoBorrowerPage.aspx?sid=" + sid + "&FileID=" + FileID);

             ChangeTab(tab_index);

             return false;
         }

         function aGeneralInfo_onclick(tab_index) {

             // common parameters
             var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
             var RadomNum = Math.random();
             var sid = RadomNum.toString().substr(2);

             // set iframe.src
             $("#tabFrame").attr("src", "../LoanDetails/GeneralInfoTab.aspx?sid=" + sid + "&FileID=" + FileID);

             ChangeTab(tab_index);

             return false;
         }

         function aLoanInfo_onclick(tab_index) {

             // common parameters
             var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
             var RadomNum = Math.random();
             var sid = RadomNum.toString().substr(2);

             // set iframe.src
             $("#tabFrame").attr("src", "../LoanDetails/LoanInfoTab.aspx?sid=" + sid + "&FileID=" + FileID);

             ChangeTab(tab_index);

             return false;
         }

         function aIncomeEmployment_onclick(tab_index) {

             // common parameters
             var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
             var RadomNum = Math.random();
             var sid = RadomNum.toString().substr(2);

             // set iframe.src
             $("#tabFrame").attr("src", "xx.aspx?sid=" + sid + "&FileID=" + FileID);

             ChangeTab(tab_index);

             return false;
         }

         function aOic_onclick(tab_index) {

             // common parameters
             var FileID = jQuery("#<%= this.hfFileID.ClientID %>").val();
             var RadomNum = Math.random();
             var sid = RadomNum.toString().substr(2);

             // set iframe.src
             $("#tabFrame").attr("src", "xx.aspx?sid=" + sid + "&FileID=" + FileID);

             ChangeTab(tab_index);

             return false;
         }

         function SetTabByName() {
             var sSpeTab = GetQueryString1("tab");   // get the specified tab name
             var tabToOpen = $("a:first-child", "#tabs10 #current"); // get the default tab

             // find the specified tab
             $("li", "#tabs10").each(function () {
                 if (sSpeTab == jQuery.trim($(this).text())) {
                     tabToOpen = $("a:first-child", this);
                 }
             });

             // here, tabToOpen should not be null
             tabToOpen.trigger("click");
         }
     </script>
</asp:Content>
  <asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <%--<div id="divContainer" style="width: 800px; height: 513px; border: solid 0px red;">
        <div id="divBtns">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()"
                            OnClick="btnSave_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divDetails" style="margin-top: 10px;">
            <table>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Branch:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlBranch" runat="server" Width="150px" DataValueField="BranchId"
                            DataTextField="Name" AutoPostBack="true" OnSelectedIndexChanged="ddlBranch_OnSelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Lead Source:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLeadSource" runat="server" DataValueField="LeadSource" DataTextField="LeadSource"
                            Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Reference Code:
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefCode" runat="server" Width="120px" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Loan Officer:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLoanOfficer" runat="server" Width="150px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Referral:
                    </td>
                    <td colspan="3" style=" padding-left: 5">
                        <asp:TextBox ID="txtReferral" runat="server" Width="250"></asp:TextBox>
                        <asp:HiddenField ID="hdnReferralID" runat="server" />
                        <input type="button" class="Btn-66" id="btnSelectReferral" value="Select" onclick="return SelectReferral();" />
                    </td>
                </tr>

                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Title:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTitle" runat="server" Width="50px">
                            <asp:ListItem Value=""> </asp:ListItem>
                            <asp:ListItem>Mr.</asp:ListItem>
                            <asp:ListItem>Mrs.</asp:ListItem>
                            <asp:ListItem>Ms.</asp:ListItem>
                            <asp:ListItem>Dr.</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Generation Code:
                    </td>
                    <td>
                        <asp:TextBox ID="txtGenerationCode" runat="server" Width="50px" MaxLength="10"></asp:TextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>

                <tr>
                    <td style="width: 70px; padding-left: 5">
                       Borrower Firstname:
                    </td>
                    <td style="width: 180px;">
                        <asp:TextBox ID="txtFirstName" runat="server" Width="145px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                       Borrower Middlename:
                    </td>
                    <td style="width: 150px;">
                        <asp:TextBox ID="txtMiddleName" runat="server" Width="120px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                       Borrower Lastname:
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" Width="120px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Credit Ranking:
                    </td>
                    <td style="width: 50px;">
                        <asp:DropDownList ID="ddlCreditRanking" runat="server" Width="150px">
                            <asp:ListItem Value="" Selected="True">-- select --</asp:ListItem>
                            <asp:ListItem Value="Excellent">Excellent</asp:ListItem>
                            <asp:ListItem Value="Very Good">Very Good</asp:ListItem>
                            <asp:ListItem Value="Good">Good</asp:ListItem>
                            <asp:ListItem Value="Fair">Fair</asp:ListItem>
                            <asp:ListItem Value="Poor">Poor</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Preferred Contact:
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="ddlPreferredContact" runat="server" Width="150px">
                            <asp:ListItem Value="" Selected="True">-- select --</asp:ListItem>
                            <asp:ListItem Value="Home Phone">Home Phone</asp:ListItem>
                            <asp:ListItem Value="Business Phone">Business Phone</asp:ListItem>
                            <asp:ListItem Value="Cell Phone">Cell Phone</asp:ListItem>
                            <asp:ListItem Value="Email">Email</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Home Phone:
                    </td>
                    <td>
                        <asp:TextBox ID="txtHomePhone" runat="server" Width="145px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Cell Phone:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCellPhone" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Busniness Phone:
                    </td>
                    <td>
                        <asp:TextBox ID="txtBizPhone" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Email:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtEmail" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Fax:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Address:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtAddress" runat="server" Width="300px" MaxLength="255"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        City:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" Width="150px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        State:
                    </td>
                    <td style="width: 50px;">
                        <asp:DropDownList ID="ddlState" runat="server" Width="80px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Zip:
                    </td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server" Width="80px" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
                <%--             <table>
                <tr>
                    <td style="width: 110px;">Experian/FICO Score:</td>
                    <td style="width: 140px;">
                        <asp:TextBox ID="txtExperianScore" runat="server" Width="105px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td style="width: 100px;">TransUnion Score:</td>
                    <td style="width: 150px;">
                        <asp:TextBox ID="txtTransUnitScore" runat="server" Width="120px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td style="width: 82px;">Equifax Score:</td>
                    <td>
                        <asp:TextBox ID="txtEquifaxScore" runat="server" Width="120px" MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
            </table>
                <tr>
                     <td style="width: 70px; padding-left: 5">
                        Coborrower Firstname:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCBFirstname" runat="server" MaxLength="20"></asp:TextBox>
                    </td>

                     <td style="width: 70px; padding-left: 5">
                        Coborrower Middlename:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCBMiddlename" runat="server" MaxLength="20"></asp:TextBox>
                    </td>

                     <td style="width: 70px; padding-left: 5">
                        Coborrower Lastname:
                    </td>
                    <td>
                        <asp:TextBox ID="txtCBLastname" runat="server"  MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Lead Ranking:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRanking" runat="server" Height="18px" Width="50px">
                            <asp:ListItem Text="Hot" Value="hot"></asp:ListItem>
                            <asp:ListItem Text="Warm" Value="warm"></asp:ListItem>
                            <asp:ListItem Text="Cold" Value="cold"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Amount:
                    </td>
                    <td>
                        <asp:TextBox ID="txbAmount" runat="server" Width="50px"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Rate:
                    </td>
                    <td>
                        <asp:TextBox ID="txbInterestRate" runat="server" Width="50px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Purpose:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPurpose" runat="server" Height="18px" Width="200px">
                            <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
                            <asp:ListItem Text="Purchase" Value="Purchase"></asp:ListItem>
                            <asp:ListItem Text="No Cash-Out Refinance" Value="No Cash-Out Refinance"></asp:ListItem>
                            <asp:ListItem Text="Cash-Out Refinance" Value="Cash-Out Refinance"></asp:ListItem>
                            <asp:ListItem Text="Construction" Value="Construction"></asp:ListItem>
                            <asp:ListItem Text="Construction-Perm" Value="Construction-Perm"></asp:ListItem>
                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Lien Position:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLienPosition" runat="server" Height="18px" Width="100px">
                            <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
                            <asp:ListItem Text="First" Value="First"></asp:ListItem>
                            <asp:ListItem Text="Second" Value="Second"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td colspan="5">
                        <asp:Button ID="btnCopyAddress" runat="server" Text="Copy from Borrower's Mailing Address"
                            OnClick="btnCopyAddress_Click" Width="250px" class="Btn-250" />&nbsp;
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Property Address:
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txbPropertyAddress" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Property City:
                    </td>
                    <td>
                        <asp:TextBox ID="txbPropertyCity" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 70px; padding-left: 5">
                        Property State:
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPropState" runat="server" Height="18px" Width="68px">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 70px; padding-left: 5">
                        Property Zip:
                    </td>
                    <td>
                        <asp:TextBox ID="txbPropertyZip" runat="server" MaxLength="10" Width="128px"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>--%>
    <%--
    <div id="divSearchContact" title="Partner Contact Search" style="display: none;"> 
        <iframe id="ifrSearchContact" frameborder="0" style="overflow: SCROLL; overflow-x: HIDDEN">
        </iframe>
    </div>--%>

    <div id="divContainer">
       
        <div class="JTab" style="margin-top: 15px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 10px;">&nbsp;</td>
                    <td>
                        <div id="tabs10">
                            <ul> 
                                <li id="current"><a id="aBorrower" href="" onclick="return aBorrower_onclick(0)"><span>Borrower</span></a></li>
                                <li><a id="aCoBorrower" href="" onclick="return aCoBorrower_onclick(1)"><span>Co-Borrower</span></a></li>
                                <li><a id="aGeneralInfo" href="" onclick="return aGeneralInfo_onclick(2)"><span>General Info</span></a></li>
                                <li><a id="aLoanInfo" href="" onclick="return aLoanInfo_onclick(3)"><span>Loan Info</span></a></li>
                                <li><a id="aIncomeEmployment" href="" onclick="return aIncomeEmployment_onclick(4)"><span>Income/Employment</span></a></li>
                                <li><a id="aOic" href="" onclick="return aOic_onclick(5)"><span>Other Income/Comments</span></a></li>
                               <%-- <li><a id="aResu" href="" onclick="return aSetup_onclick(6)"><span>Results</span></a></li>--%>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
                <div id="TabLine2" class="TabRightLine">&nbsp;</div>
                <div class="TabContent">
                    <iframe id="tabFrame" frameborder="0" style="border: solid 0px blue; height: 510px; width: 900px;"></iframe>
                </div>
            </div>
        </div>
        <asp:HiddenField ID="hfFileID" runat="server" />
    </div>
</asp:Content>
