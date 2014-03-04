<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeadLoanInfoTab.aspx.cs" Inherits="LeadLoanInfoTab" %>
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
   
    <script type="text/javascript">
        $(document).ready(function () {

            // init tab
            //$("#tabs").tabs();



            SetDollarPos();

            //#region Loan Info

            $("#txtAmount").numeric({ allow: "," });
            $("#txtAmount").blur(function () { FormatMoney(this); });

            $("#txtAmountNewLoan").numeric({ allow: "," });
            $("#txtAmountNewLoan").blur(function () { FormatMoney(this); });

            $("#txtPMI").numeric({ allow: "," });
            $("#txtPMI").blur(function () { FormatMoney(this); });

            $("#txtPMINewLoan").numeric({ allow: "," });
            $("#txtPMINewLoan").blur(function () { FormatMoney(this); });

            $("#txtPMITax").numeric({ allow: "," });
            $("#txtPMITax").blur(function () { FormatMoney(this); });

            $("#txtPMITaxNewLoan").numeric({ allow: "," });
            $("#txtPMITaxNewLoan").blur(function () { FormatMoney(this); });

            $("#txt2ndAmount").numeric({ allow: "," });
            $("#txt2ndAmount").blur(function () { FormatMoney(this); });

            $("#txtRate").mask("99.999");
            $("#txtRateNewLoan").mask("99.999");

            $("#txtTerm").OnlyInt();
            $("#txtTermNewLoan").OnlyInt();

            //#endregion

        });

        function SetDollarPos() {


            $("#Dollar03").position({

                of: $("#tdAmount"),
                my: "left top",
                at: "left top",
                offset: "8 10",
                collision: "none none"
            });

            $("#Dollar04").position({

                of: $("#tdAmount"),
                my: "left top",
                at: "left top",
                offset: "380 10",
                collision: "none none"
            });

            $("#Dollar05").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "8 10",
                collision: "none none"
            });

            $("#Dollar06").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "380 10",
                collision: "none none"
            });

            $("#Dollar07").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "8 40",
                collision: "none none"
            });

            $("#Dollar08").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "380 40",
                collision: "none none"
            });

            $("#Dollar09").position({

                of: $("#tdPMI"),
                my: "left top",
                at: "left top",
                offset: "52 132",
                collision: "none none"
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

            ShowGlobalPopup("Search Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
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

            CloseGlobalPopup();
            var ss = s.split(':');

            $("#txtReferralSource").val(ss[1]);
            $("#hdnReferralID").val(ss[0]);
        }

        //#endregion

        function BeforeSave() {


            return true;
        }
    </script>
</head>
<body style="width: 800px">
    <form id="form1" runat="server">
    <div id="divLeadContainer" class="margin-top-10 margin-bottom-20">
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
                            Amount($):&nbsp;&nbsp;<asp:TextBox ID="txt2ndAmount" runat="server" MaxLength="15" class="dollar-text" style="width:135px; padding-left:12px;"></asp:TextBox>
                        </td>
                        <td>
                            <lable><asp:CheckBox ID="chkSubordinate" runat="server" Checked="true" /> Subordinate</lable>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>

                <%--<div style=" left:-200; position:absolute;">
                    <span id="Dollar03" class="positionable">$</span>
                    <span id="Dollar04" class="positionable">$</span>
                    <span id="Dollar05" class="positionable">$</span>
                    <span id="Dollar06" class="positionable">$</span>
                    <span id="Dollar07" class="positionable">$</span>
                    <span id="Dollar08" class="positionable">$</span>
                    <span id="Dollar09" class="positionable">$</span>
                </div>--%>

                <div class="margin-top-20">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" OnClick="btnSave_Click" />
                </div>

            </div>
    </div>
    </form>
</body>
</html>