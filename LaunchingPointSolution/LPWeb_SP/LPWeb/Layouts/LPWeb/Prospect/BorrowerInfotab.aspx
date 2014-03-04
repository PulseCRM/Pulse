<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BorrowerInfotab.aspx.cs" Inherits="BorrowerInfotab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css" runat="server" />
    <style type="text/css">
    .dollar-text{width:170px; padding-left:12px;}
    .percent-text{width:167px; padding-left:15px;}
    </style>
     <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.tabs.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>

    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jquery.formatCurrency.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
   <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">


        $(document).ready(function () {

            // init tab
            $("#tabs").tabs();

            setTimeout("AfterTabInit()", 1000);

        });

        function AfterTabInit() {

            $("#tabs").show();

            AddValidators();

            SetDollarPos();

            //#region Borrower

            $("#txtBirthday").datepick({ yearRange: "1910:2012" });
            $("#txtBirthday").mask("99/99/9999");

            $("#txtCellPhone").mask("(999) 999-9999");
            $("#txtHomePhone").mask("(999) 999-9999");
            $("#txtWorkPhone").mask("(999) 999-9999");

            $("#txtSSN").mask("999-99-9999");

            $("#txtPropertyValue").numeric({ allow: ",." });
            $("#txtPropertyValue").blur(function () { FormatMoney(this); });

            $("#txtRentAmount").numeric({ allow: ",." });
            $("#txtRentAmount").blur(function () { FormatMoney(this); });

            $("#txtFICOScore").OnlyInt();

            //#endregion

            //#region Co-Borrower

            $("#txtBirthdayCoBorrower").datepick({ yearRange: "1910:2012" });
            $("#txtBirthdayCoBorrower").mask("99/99/9999");

            $("#txtCellPhoneCoBorrower").mask("(999) 999-9999");
            $("#txtHomePhoneCoBorrower").mask("(999) 999-9999");
            $("#txtWorkPhoneCoBorrower").mask("(999) 999-9999");

            $("#txtSSNCoBorrower").mask("999-99-9999");

            $("#txtFICOScoreCoBorrower").OnlyInt();

            //#endregion

            //#region General Info

            $("#txtMailingZip").mask("99999");
            $("#txtPropertyZip").mask("99999");

            //#endregion

            //#region Loan Info

            $("#txtAmount").numeric({ allow: ",." });
            $("#txtAmount").blur(function () { FormatMoney(this); });

            $("#txtAmountNewLoan").numeric({ allow: ",." });
            $("#txtAmountNewLoan").blur(function () { FormatMoney(this); });

            $("#txtPMI").numeric({ allow: ",." });
            $("#txtPMI").blur(function () { FormatMoney(this); });

            $("#txtPMINewLoan").numeric({ allow: ",." });
            $("#txtPMINewLoan").blur(function () { FormatMoney(this); });

            $("#txtPMITax").numeric({ allow: ",." });
            $("#txtPMITax").blur(function () { FormatMoney(this); });

            $("#txtPMITaxNewLoan").numeric({ allow: ",." });
            $("#txtPMITaxNewLoan").blur(function () { FormatMoney(this); });

            $("#txt2ndAmount").numeric({ allow: ",." });
            $("#txt2ndAmount").blur(function () { FormatMoney(this); });

//            $("#txtRate").mask("99.999");
//            $("#txtRateNewLoan").mask("99.999");

            $("#txtTerm").OnlyInt();
            $("#txtTermNewLoan").OnlyInt();

            //#endregion

            //#region Income/Employment

            $("#txtMonthlySalary").numeric({ allow: ",." });
            $("#txtMonthlySalary").blur(function () { FormatMoney(this); });

            $("#txtYearsinFiled").OnlyInt();

            //#endregion

            //#region Other Income/Comments

            $("#txtOtherMonthlyIncome").numeric({ allow: ",." });
            $("#txtOtherMonthlyIncome").blur(function () { FormatMoney(this); });

            $("#txtLiquidAssets").numeric({ allow: ",." });
            $("#txtLiquidAssets").blur(function () { FormatMoney(this); });

            $("#txtComments").maxlength(4000);

            //#endregion
        }

        function SetDollarPos() {

            $("#Dollar01").position({

                of: $("#tdRentAmount"),
                my: "left top",
                at: "left top",
                offset: "6 7",
                collision: "none none"
            });

            $("#Dollar02").position({

                of: $("#tdPropertyValue"),
                my: "left top",
                at: "left top",
                offset: "465 -50",
                collision: "none none"
            });

            $("#Dollar03").position({

                of: $("#tdAmount"),
                my: "left top",
                at: "left top",
                offset: "123 -195",
                collision: "none none"
            });

            $("#Dollar04").position({

                of: $("#tdAmount"),
                my: "left top",
                at: "left top",
                offset: "487 -195",
                collision: "none none"
            });

            $("#Dollar05").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "105 -137",
                collision: "none none"
            });

            $("#Dollar06").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "469 -137",
                collision: "none none"
            });

            $("#Dollar07").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "87 -108",
                collision: "none none"
            });

            $("#Dollar08").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "451 -108",
                collision: "none none"
            });

            $("#Dollar09").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "115 -21",
                collision: "none none"
            });

            $("#Dollar10").position({

                of: $("#tdMonthlySalary"),
                my: "left top",
                at: "left top",
                offset: "96 -79",
                collision: "none none"
            });

            $("#Dollar11").position({

                of: $("#tdOtherMonthlyIncome"),
                my: "left top",
                at: "left top",
                offset: "128 -168",
                collision: "none none"
            });

            $("#Dollar12").position({

                of: $("#tdOtherMonthlyIncome"),
                my: "left top",
                at: "left top",
                offset: "119 -139",
                collision: "none none"
            });
        }

        function AddValidators() {

            $("#aspnetForm").validate({

                rules: {

                    txtFirstName: {
                        required: true
                    },
                    txtLastName: {
                        required: true
                    },
                   txtEmail: {
                        email: true
                    },
                    txtEmailCoBorrower: {
                        email: true
                    }
                },
                messages: {

                    txtFirstName: {
                        required: "*"
                    },
                    txtLastName: {
                        required: "*"
                    },
                    txtEmail: {
                        email: "x"
                    },
                    txtEmailCoBorrower: {
                        email: "x"
                    }
                }
            });
        }

        function FormatMoney(textbox) {

            // format
            $(textbox).formatCurrency();

            // substring
            var m0 = $(textbox).val();
            var m1 = m0.replace("$", "");
            var m2 = m1.replace(".00", "");

            var maxlength = $(textbox).attr("maxlength");
            var maxnum = new Number(maxlength);

            if (m2.length > maxnum) {

                var substr = m2.substring(0, maxnum);

                var m3 = substr.replace(/,/g, "");

                $(textbox).val(m3);

                $(textbox).formatCurrency();
                var m4 = $(textbox).val();
                var m5 = m4.replace("$", "");
                var m6 = m5.replace(".00", "");
                $(textbox).val(m6);
            }
            else {

                $(textbox).val(m2);
            }
        }

        //#region referral source

        function aSelectContact_onlick() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var iFrameSrc = "../Contact/SearchContactsPopup.aspx?sid=" + sid + "&CloseDialogCodes=window.parent.CloseGlobalPopup()&from=TabPage";

            var BaseWidth = 600
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 240;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Search Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function InvokeFn(fn, arg) {

            eval(fn + "('" + arg + "')");
        }

        function GetSearchCondition(sWhere) {

            CloseGlobalPopup();
            ShowDialog_SelectContact(sWhere);
        }

        function ShowDialog_SelectContact(sWhere) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var stype = "TabPage";
            var iFrameSrc = "../Contact/SelectContactsPopup.aspx?sid=" + sid + "&pagesize=15&sCon=" + sWhere + "&type=" + stype + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 750
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 550;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

           ShowGlobalPopup("Select Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function ShowReferral2(s) {

            window.parent.parent.CloseGlobalPopup();
            var ss = s.split(':');

            $("#txtReferralSource").val(ss[1]);
            $("#hdnReferralID").val(ss[0]);
        }

        //#endregion

        function BeforeSave() {

            var loanId = $("#" + '<%=hdnLoanId.ClientID %>').val();
             var res =  CheckPointFileStatus(loanId,function(){
                  
               
                //#region Borrower

                var IsValid1 = $("#aspnetForm").validate().element("#txtFirstName");
                var IsValid2 = $("#aspnetForm").validate().element("#txtLastName");

                if (IsValid1 == false || IsValid2 == false) {

                    $("#tabs").tabs('select', 0);
                    alert("Please enter the borrower information.");
                    return false;
                }

                var IsValid3 = $("#aspnetForm").validate().element("#txtEmail");
                if (IsValid3 == false) {

                    $("#tabs").tabs('select', 0);
                    alert("Please enter a valid email.");
                    return false;
                }

                var Email = $.trim($("#txtEmail").val());
                var CellPhone = $.trim($("#txtCellPhone").val());
                var HomePhone = $.trim($("#txtHomePhone").val());
                var WorkPhone = $.trim($("#txtWorkPhone").val());
                if (Email == "" && CellPhone == "" && HomePhone == "" && WorkPhone == "") {

                    $("#tabs").tabs('select', 0);
                    alert("Please enter one of Email, Cell Phone, Home Phone or Work Phone of Borrower.");
                    return false;
                }

                //#endregion

                //#region Co-Borrower

                var IsValid4 = $("#aspnetForm").validate().element("#txtEmailCoBorrower");
                if (IsValid4 == false) {

                    $("#tabs").tabs('select', 1);
                    alert("Please enter a valid email.");
                    return false;
                }

                var FirstNameCoBorrower = $.trim($("#txtFirstNameCoBorrower").val());
                var LastNameCoBorrower = $.trim($("#txtLastNameCoBorrower").val());
                if (FirstNameCoBorrower != "" || LastNameCoBorrower != "") {

                    var EmailCoBorrower = $.trim($("#txtEmailCoBorrower").val());
                    var CellPhoneCoBorrower = $.trim($("#txtCellPhoneCoBorrower").val());
                    var HomePhoneCoBorrower = $.trim($("#txtHomePhoneCoBorrower").val());
                    var WorkPhoneCoBorrower = $.trim($("#txtWorkPhoneCoBorrower").val());
                    if (EmailCoBorrower == "" && CellPhoneCoBorrower == "" && HomePhoneCoBorrower == "" && WorkPhoneCoBorrower == "") {

                        $("#tabs").tabs('select', 1);
                        alert("Please enter one of Email, Cell Phone, Home Phone or Work Phone of Co-Borrower.");
                        return false;
                    }
                }

                //#endregion


                //ShowWaitingDialog("waiting...");

                $("#<%=btnSaveGoToDetail.ClientID %>").click();

            });
            return false;

        }


        // CR50 Start CheckPointFileStatus
        function CheckPointFileStatus(FileId, Fun) {
            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);
            var isAsync = false;
            if (Fun != null) {
                isAsync = true;
            }
            //var FileId = GetQueryString1("FileID");
            var res = false;
            $.blockUI({ message: "Please wait..." });
            $.ajax({
                url: "../CheckPointFileStatus_BG_JSON.aspx?sid=" + Radom + "&FileId=" + FileId,
                async: isAsync,
                cache: false,
                dataType: "json",
                success: function (data) {

                    if (data.ExecResult == "Failed") {
                        //$('#divDetail').unblock();
                        alert(data.ErrorMsg);
                        //return false;
                        res = false;
                    }
                    else {
                        //return true;
                        res = true;
                        Fun();
                    }
                    $.unblockUI();
                }
            });
            return res;
        }

        //End CheckPointFileStatus
        

    </script>
</head>
<body>
<form  id="aspnetForm" runat="server">
<div id="divLeadContainer" class="margin-top-10 margin-bottom-20">
        
        <div id="tabs" style="display:none;">
            <ul style="height:30px;"  >
                <li><a href="#tabs-1">Borrower</a></li>
                <li><a href="#tabs-2">Co-Borrower</a></li>
                <li><a href="#tabs-3">General Info</a></li>
                <li><a href="#tabs-4">Loan Info</a></li>
                <li><a href="#tabs-5">Income / Employment</a></li>
                <li><a href="#tabs-6">Other Income / Comments</a></li>
            </ul>
            <div id="tabs-1">
                
                <table cellpadding="3" cellspacing="3">
				    <tr>
				        <td>First Name:</td>
				        <td style="width:220px;">
				            <asp:TextBox ID="txtFirstName" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
				        </td>
                        <td>Dependants:</td>
                        <td>
                            <asp:DropDownList ID="ddlDependants" runat="server" Width="184px">
                                <asp:ListItem>Yes</asp:ListItem>
                                <asp:ListItem>No</asp:ListItem>
                            </asp:DropDownList>
                        </td>
				    </tr>
				    <tr>
				        <td>Last Name:</td>
				        <td>
				            <asp:TextBox ID="txtLastName" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
				        </td>
                        <td>Credit Ranking:</td>
                        <td>
                            <asp:DropDownList ID="ddlCreditRanking" runat="server" Width="184px">
                                <asp:ListItem Value="" Selected="True">-- select --</asp:ListItem>
                                <asp:ListItem Value="Excellent">Excellent</asp:ListItem>
                                <asp:ListItem Value="Very Good">Very Good</asp:ListItem>
                                <asp:ListItem Value="Good">Good</asp:ListItem>
                                <asp:ListItem Value="Fair">Fair</asp:ListItem>
                                <asp:ListItem Value="Poor">Poor</asp:ListItem>
                            </asp:DropDownList>
                        </td>
				    </tr>
				    <tr>
				        <td>Email:</td>
				        <td>
				            <asp:TextBox ID="txtEmail" runat="server" Width="180px" MaxLength="255"></asp:TextBox>
				        </td>
                        <td>Housing Status:</td>
                        <td>
                            <asp:DropDownList ID="ddlHousingStatus" runat="server" Width="184px">
                                <asp:ListItem>Rent</asp:ListItem>
                                <asp:ListItem>Own</asp:ListItem>
                            </asp:DropDownList>
                        </td>
				    </tr>
				    <tr>
			            <td>Cell Phone:</td>
			            <td>
			                <asp:TextBox ID="txtCellPhone" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			            </td>
                        <td>Rent Amount($):</td>
                        <td id="tdRentAmount">
                            <asp:TextBox ID="txtRentAmount" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
			        </tr>
			        <tr>
			            <td>Home Phone:</td>
			            <td>
			                <asp:TextBox ID="txtHomePhone" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			            </td>
                        <td>FICO Score:</td>
                        <td>
                            <asp:TextBox ID="txtFICOScore" runat="server" Width="180px" MaxLength="3"></asp:TextBox>
                        </td>
			        </tr>
			        <tr>
			            <td>Work Phone:</td>
			            <td>
			                <asp:TextBox ID="txtWorkPhone" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			            </td>
                        
			        </tr>
			        <tr>
			            <td>Date of Birth:</td>
			            <td>
			                <asp:TextBox ID="txtBirthday" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
			            </td>
			        </tr>
                    <tr>
			            <td>SSN:</td>
			            <td>
			                <asp:TextBox ID="txtSSN" runat="server" Width="180px"></asp:TextBox>
			            </td>
			        </tr>
			    </table>

                <%--<div>
                    <span id="Dollar01" class="positionable">$</span>
                </div>--%>

            </div>
            <div id="tabs-2" >

                <table cellpadding="3" cellspacing="3">
				    <tr>
				        <td>First Name:</td>
				        <td style="width:220px;">
				            <asp:TextBox ID="txtFirstNameCoBorrower" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
				        </td>
                        <td>Date of Birth:</td>
			            <td>
			                <asp:TextBox ID="txtBirthdayCoBorrower" runat="server" Width="180px" MaxLength="10"></asp:TextBox>
			            </td>
				    </tr>
				    <tr>
				        <td>Last Name:</td>
				        <td>
				            <asp:TextBox ID="txtLastNameCoBorrower" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
				        </td>
                        <td>SSN:</td>
			            <td>
			                <asp:TextBox ID="txtSSNCoBorrower" runat="server" Width="180px"></asp:TextBox>
			            </td>
				    </tr>
				    <tr>
				        <td>Email:</td>
				        <td>
				            <asp:TextBox ID="txtEmailCoBorrower" runat="server" Width="180px" MaxLength="255"></asp:TextBox>
				        </td>
                        <td>FICO Score:</td>
                        <td>
                            <asp:TextBox ID="txtFICOScoreCoBorrower" runat="server" Width="180px" MaxLength="3"></asp:TextBox>
                        </td>
				    </tr>
				    <tr>
			            <td>Cell Phone:</td>
			            <td>
			                <asp:TextBox ID="txtCellPhoneCoBorrower" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			            </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
			        </tr>
			        <tr>
			            <td>Home Phone:</td>
			            <td>
			                <asp:TextBox ID="txtHomePhoneCoBorrower" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			            </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
			        </tr>
			        <tr>
			            <td>Work Phone:</td>
			            <td>
			                <asp:TextBox ID="txtWorkPhoneCoBorrower" runat="server" Width="180px" MaxLength="20"></asp:TextBox>
			            </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
			        </tr>
			        
			    </table>

            </div>
            <div id="tabs-3">

                <table cellpadding="3" cellspacing="3">
				    <tr>
                        <td>
                            <h6>Mailing Address:</h6>
                        </td>
                        <td style="width:250px;">
                            <asp:DropDownList ID="ddlMailingAddress" runat="server" Width="184px">
                                <asp:ListItem>Both</asp:ListItem>
                                <asp:ListItem>Borrower</asp:ListItem>
                                <asp:ListItem>Co-Borrower</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2">
                            <h6>Subject Property Address:</h6>
                        </td>
                        <%--<td>&nbsp;</td>--%>
                    </tr>
                    <tr>
                        <td>Street Address 1:</td>
                        <td>
                            <asp:TextBox ID="txtMailingStreetAddress1" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>Street Address 1:</td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txtPropertyStreetAddress1" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td style="vertical-align: middle;">
                            Ranking: <asp:DropDownList ID="ddlRanking" runat="server">
                            <asp:ListItem >Hot</asp:ListItem>
                            <asp:ListItem >Warm</asp:ListItem>
                            <asp:ListItem >Cold</asp:ListItem>
                            </asp:DropDownList>
                            <!--<asp:Label ID="lbRanking" runat="server" Text="Hot"></asp:Label>-->
                        </td>
                        <td>
                            <!-- <asp:Image ID="imgRanking" runat="server" ImageUrl="../images/loan/Ranking-Hot.gif" />-->
                        </td>
                    </tr>
                    <tr>
                        <td>Street Address 2:</td>
                        <td>
                            <asp:TextBox ID="txtMailingStreetAddress2" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>Street Address 2:</td>
                        <td>
                            <asp:TextBox ID="txtPropertyStreetAddress2" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>
                            Status: <asp:Label ID="lbStatus" runat="server" Text="Active"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>City:</td>
                        <td>
                            <asp:TextBox ID="txtMailingCity" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>City:</td>
                        <td>
                            <asp:TextBox ID="txtPropertyCity" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>State:</td>
                        <td>
                            <asp:DropDownList ID="ddlMailingState" runat="server" Width="184px">
                            </asp:DropDownList>
                        </td>
                        <td>State:</td>
                        <td>
                            <asp:DropDownList ID="ddlPropertyState" runat="server" Width="184px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Zip:</td>
                        <td>
                            <asp:TextBox ID="txtMailingZip" runat="server" Width="180px" MaxLength="5"></asp:TextBox>
                        </td>
                        <td>Zip:</td>
                        <td>
                            <asp:TextBox ID="txtPropertyZip" runat="server" Width="180px" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Lead Source:</td>
                        <td>
                            <asp:DropDownList ID="ddlLeadSource" runat="server" Width="184px">
	                        </asp:DropDownList>
                        </td>
                        <td>Property Value($):</td>
                        <td id="tdPropertyValue">
                            <asp:TextBox ID="txtPropertyValue" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Referral Source:</td>
                        <td>
                            <asp:TextBox ID="txtReferralSource" runat="server" Width="180px" MaxLength="300"></asp:TextBox>
				        	<a id="aSelectContact" href="javascript:aSelectContact_onlick()" class="ui-icon ui-icon-search" style="position:absolute; top:259px; left:290px;">&nbsp;&nbsp;&nbsp;&nbsp;</a>
	                        <asp:HiddenField ID="hdnReferralID" runat="server" />
                            <script>
                                $(function () {

                                    if ($.browser.msie == true) {
                                        $("#aSelectContact").css("top", "274px").css("left", "290px");
                                    }
                                    var isChrome = navigator.userAgent.toLowerCase().match(/chrome/) != null;
                                    if (isChrome) {
                                        $("#aSelectContact").css("top", "270px").css("left", "290px");
                                    }


                                });
                                </script>
                        </td>
                    </tr>
                </table>

                <%--<div>
                    <span id="Dollar02" class="positionable">$</span>
                </div>--%>

            </div>
            <div id="tabs-4">
                
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td colspan="2">
                            <h6>Current Loan (if applicable)</h6>
                        </td>
                        <td colspan="2">
                            <h6>New Loan:</h6>
                        </td>
                    </tr>
                    <tr>
                        <td>Archive Loan:</td>
                        <td>
                            <asp:DropDownList ID="ddlArchiveLoan" runat="server" Width="184px">
                                <asp:ListItem Value="">-- select --</asp:ListItem>
                                <asp:ListItem>Closed</asp:ListItem>
                                <asp:ListItem>Canceled</asp:ListItem>
                                <asp:ListItem>Denied</asp:ListItem>
                                <asp:ListItem>Suspended</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Purpose:</td>
                        <td style="width:250px;">
                            <asp:DropDownList ID="ddlPurpose" runat="server" Height="18px" Width="184px">
	                            <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
	                            <asp:ListItem Text="Purchase" Value="Purchase"></asp:ListItem>
	                            <asp:ListItem Text="No Cash-Out Refinance" Value="No Cash-Out Refinance"></asp:ListItem>
	                            <asp:ListItem Text="Cash-Out Refinance" Value="Cash-Out Refinance"></asp:ListItem>
	                            <asp:ListItem Text="Construction" Value="Construction"></asp:ListItem>
	                            <asp:ListItem Text="Construction-Perm" Value="Construction-Perm"></asp:ListItem>
	                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
	                        </asp:DropDownList>
                        </td>
                        <td>Purpose:</td>
                        <td>
                            <asp:DropDownList ID="ddlPurposeNewLoan" runat="server" Height="18px" Width="184px">
	                            <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
	                            <asp:ListItem Text="Purchase" Value="Purchase"></asp:ListItem>
	                            <asp:ListItem Text="No Cash-Out Refinance" Value="No Cash-Out Refinance"></asp:ListItem>
	                            <asp:ListItem Text="Cash-Out Refinance" Value="Cash-Out Refinance"></asp:ListItem>
	                            <asp:ListItem Text="Construction" Value="Construction"></asp:ListItem>
	                            <asp:ListItem Text="Construction-Perm" Value="Construction-Perm"></asp:ListItem>
	                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
	                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Type:</td>
                        <td>
                            <asp:DropDownList ID="ddlType" runat="server" Width="184px">
	                            <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
	                            <asp:ListItem>Conv</asp:ListItem>
	                            <asp:ListItem>FHA</asp:ListItem>
	                            <asp:ListItem>VA</asp:ListItem>
	                            <asp:ListItem>USDA/RH</asp:ListItem>
	                            <asp:ListItem>Other</asp:ListItem>
				            </asp:DropDownList>
                        </td>
                        <td>Type:</td>
                        <td>
                            <asp:DropDownList ID="ddlTypeNewLoan" runat="server" Width="184px">
	                            <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
	                            <asp:ListItem>Conv</asp:ListItem>
	                            <asp:ListItem>FHA</asp:ListItem>
	                            <asp:ListItem>VA</asp:ListItem>
	                            <asp:ListItem>USDA/RH</asp:ListItem>
	                            <asp:ListItem>Other</asp:ListItem>
				            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Program:</td>
                        <td>
                            <asp:DropDownList ID="ddlProgram" runat="server" DataValueField="LoanProgram" DataTextField="LoanProgram" Width="184px">
				            </asp:DropDownList>
                        </td>
                        <td>Program:</td>
                        <td>
                            <asp:DropDownList ID="ddlProgramNewLoan" runat="server" DataValueField="LoanProgram" DataTextField="LoanProgram" Width="184px">
				            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Amount($):</td>
                        <td id="tdAmount">
                            <asp:TextBox ID="txtAmount" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                        <td>Amount($):</td>
                        <td>
                            <asp:TextBox ID="txtAmountNewLoan" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Rate(%):</td>
                        <td>
                            <asp:TextBox ID="txtRate" runat="server" MaxLength="10" class="percent-text"></asp:TextBox>
                        </td>
                        <td>Rate(%):</td>
                        <td>
                            <asp:TextBox ID="txtRateNewLoan" runat="server" MaxLength="10" class="percent-text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Monthly PMI($):</td>
                        <td id="tdPMI">
                            <asp:TextBox ID="txtPMI" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                        <td>Monthly PMI($):</td>
                        <td>
                            <asp:TextBox ID="txtPMINewLoan" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Monthly Property Tax($):</td>
                        <td>
                            <asp:TextBox ID="txtPMITax" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                        <td>Monthly Property Tax($):</td>
                        <td>
                            <asp:TextBox ID="txtPMITaxNewLoan" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Term:</td>
                        <td>
                            <asp:TextBox ID="txtTerm" runat="server" Width="70px" MaxLength="3"></asp:TextBox> months
                        </td>
                        <td>Term:</td>
                        <td>
                            <asp:TextBox ID="txtTermNewLoan" runat="server" Width="70px" MaxLength="3"></asp:TextBox> months
                        </td>
                    </tr>
                    <tr>
                        <td>Start Date:</td>
                        <td>
                            <asp:DropDownList ID="ddlStartYear" runat="server" Width="74px">
                                
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:DropDownList ID="ddlStartMonth" runat="server" Width="74px">
                                <asp:ListItem Value="">month</asp:ListItem>
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                                <asp:ListItem Value="7">07</asp:ListItem>
                                <asp:ListItem Value="8">08</asp:ListItem>
                                <asp:ListItem Value="9">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>Start Date:</td>
                        <td>
                            <asp:DropDownList ID="ddlStartYearNewLoan" runat="server" Width="74px">
                                
                            </asp:DropDownList>
                            &nbsp;&nbsp;
                            <asp:DropDownList ID="ddlStartMonthNewLoan" runat="server" Width="74px">
                                <asp:ListItem Value="">month</asp:ListItem>
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                                <asp:ListItem Value="7">07</asp:ListItem>
                                <asp:ListItem Value="8">08</asp:ListItem>
                                <asp:ListItem Value="9">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <lable><asp:CheckBox ID="chk2nd" runat="server" Checked="true" /> 2nd TD</lable>
                        </td>
                        <td>
                            Amount($):&nbsp;&nbsp;<asp:TextBox ID="txt2ndAmount" runat="server" MaxLength="15" style="width:135px; padding-left:12px;"></asp:TextBox>
                        </td>
                        <td>
                            <lable><asp:CheckBox ID="chkSubordinate" runat="server" Checked="true" /> Subordinate</lable>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>

               <%-- <div>
                    <span id="Dollar03" class="positionable">$</span>
                    <span id="Dollar04" class="positionable">$</span>
                    <span id="Dollar05" class="positionable">$</span>
                    <span id="Dollar06" class="positionable">$</span>
                    <span id="Dollar07" class="positionable">$</span>
                    <span id="Dollar08" class="positionable">$</span>
                    <span id="Dollar09" class="positionable">$</span>
                </div>--%>

            </div>
            <div id="tabs-5">
                
                <table cellpadding="3" cellspacing="3">
                    <tr>
                        <td>Company Name:</td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txtCompanyName" runat="server" Width="180px" MaxLength="255"></asp:TextBox>
                        </td>
                        <td>Start Date:</td>
                        <td>
                            <asp:DropDownList ID="ddlWorkStartMonth" runat="server" Width="74px">
                                <asp:ListItem Value="">month</asp:ListItem>
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                                <asp:ListItem Value="7">07</asp:ListItem>
                                <asp:ListItem Value="8">08</asp:ListItem>
                                <asp:ListItem Value="9">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Self Employed:</td>
                        <td style="width:250px;">
                            <asp:DropDownList ID="ddlSelfEmployed" runat="server" Width="184px">
                                <asp:ListItem>No</asp:ListItem>
                                <asp:ListItem>Yes</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlWorkStartYear" runat="server" Width="74px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Title / Position:</td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txtPosition" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>End Date:</td>
                        <td>
                            <asp:DropDownList ID="ddlWorkEndMonth" runat="server" Width="74px">
                                <asp:ListItem Value="">month</asp:ListItem>
                                <asp:ListItem Value="1">01</asp:ListItem>
                                <asp:ListItem Value="2">02</asp:ListItem>
                                <asp:ListItem Value="3">03</asp:ListItem>
                                <asp:ListItem Value="4">04</asp:ListItem>
                                <asp:ListItem Value="5">05</asp:ListItem>
                                <asp:ListItem Value="6">06</asp:ListItem>
                                <asp:ListItem Value="7">07</asp:ListItem>
                                <asp:ListItem Value="8">08</asp:ListItem>
                                <asp:ListItem Value="9">09</asp:ListItem>
                                <asp:ListItem Value="10">10</asp:ListItem>
                                <asp:ListItem Value="11">11</asp:ListItem>
                                <asp:ListItem Value="12">12</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Monthly Salary($):</td>
                        <td id="tdMonthlySalary" style="width:250px;">
                            <asp:TextBox ID="txtMonthlySalary" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlWorkEndYear" runat="server" Width="74px">
                            </asp:DropDownList>
                            
                        </td>
                    </tr>
                    <tr>
                        <td>Profession:</td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txtProfession" runat="server" Width="180px" MaxLength="50"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>Years in Filed:</td>
                        <td style="width:250px;">
                            <asp:TextBox ID="txtYearsinFiled" runat="server" Width="180px" MaxLength="2"></asp:TextBox>
                        </td>
                        <td>&nbsp;</td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <%--<div>
                    <span id="Dollar10" class="positionable">$</span>
                </div>--%>
            </div>
            <div id="tabs-6">
                
                <table>
                    <tr>
                        <td style="vertical-align:top;">
                            
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <td>Other Monthly Income($):</td>
                                    <td id="tdOtherMonthlyIncome">
                                        <asp:TextBox ID="txtOtherMonthlyIncome" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Liquid Assets($):</td>
                                    <td>
                                        <asp:TextBox ID="txtLiquidAssets" runat="server" MaxLength="15" class="dollar-text"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>

                        </td>
                        <td style="padding-left:30px;">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <td style="vertical-align:top; padding-top:7px;">Comments:</td>
                                    <td>
                                        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" style="height:150px; overflow:auto;"></asp:TextBox>
                                    </td>
                                </tr>
                                
                            </table>
                        </td>
                    </tr>
                </table>

              <%--  <div>
                    <span id="Dollar11" class="positionable">$</span>
                    <span id="Dollar12" class="positionable">$</span>
                </div>--%>

            </div>
        </div>

        <div class="margin-top-20">
            <input id="btnSave" type="button" value="Save and Go to Lead Detail" class="Btn-160" onclick="BeforeSave()" />
            <asp:Button ID="btnSaveGoToDetail" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSaveGoToDetail_Click" style="display:none;"  />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
          <!--  <asp:Button ID="btnCreateAnother" runat="server" Text="Save and Create Another" CssClass="Btn-160" OnClientClick="return BeforeSave()" OnClick="btnCreateAnother_Click" />
            -->

             <asp:HiddenField ID="hdnLoanId" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>