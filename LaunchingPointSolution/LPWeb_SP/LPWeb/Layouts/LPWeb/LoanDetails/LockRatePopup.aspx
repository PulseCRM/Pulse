<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LockRatePopup.aspx.cs" Inherits="LoanDetails_LockRatePopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Lock Rate</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .dollar-char{position:relative; left:10px;}
    .dollar-text{padding-left:12px;}
    .percent-text{text-align: right; padding-right: 3px;}
    .text-align-right{text-align: right;}
    </style>
    <style type="text/css">
        .ui-autocomplete-input
        {
            width: 300px;
        }
        .ui-autocomplete-loading
        {
            background: white url('../images/loading.gif') right center no-repeat;
        }
        .ui-icon-triangle-1-sx
        {
            background-position: -64px -20px;
        }
        .ui-autocomplete
        {
            max-height: 200px;
            overflow-y: auto; /* prevent horizontal scrollbar */
            overflow-x: hidden; /* add padding to account for vertical scrollbar */
            padding-right: 20px;
        }
        /* IE 6 doesn't support max-height
	 * we use height instead, but this forces the menu to always be this tall
	 */
        * html .ui-autocomplete
        {
            height: 430px;
        }
    </style>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <script src="../js/jquery.autoCompleteCombox.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jquery.formatCurrency.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
    <script src="../js/datejs.js" type="text/javascript"></script>



    <script language="javascript" type="text/javascript">

        $(document).ready(function () {

            AddValidators();
            var ViewLockRate = $("#hdnViewLockRate").val();
            if (ViewLockRate == "true") {

                // disabled all
                $("input").attr("disabled", "true");
                $("select").attr("disabled", "true");
                return;
            }

            InitAutoComplete();

            $("#txtLenderCredit").numeric({ allow: "," });
            $("#txtLenderCredit").blur(function () { FormatMoney(this); });

//            $("#txtPrice").numeric({ allow: "," });
//            $("#txtPrice").blur(function () { FormatMoney(this); });

            //$(".date-field").mask("99/99/9999");
            $(".date-field").datepick();

            $(".cap-field").attr("readonly", "true");
            $(".percent-text").mask("9?.999");

            $(".lock-expire-date").attr("readonly", "true");

            // add events
            $("#ddlLoanProgram").change(LoadARMInfo);

            $("#txtExt1LockDate").change(txtExt1LockDate_onchange);
            $("#ddlExt1Term").change(txtExt1LockDate_onchange);

            $("#txtExt2LockDate").change(txtExt2LockDate_onchange);
            $("#ddlExt2Term").change(txtExt2LockDate_onchange);

            $("#txtExt3LockDate").change(txtExt3LockDate_onchange);
            $("#ddlExt3Term").change(txtExt3LockDate_onchange);

            $("#txtLockDate,#ddlLockTerm").change(function () {
                //txtExpirationDate

                var lockDate = $("#txtLockDate").val();
                var lockTerm = $("#ddlLockTerm").val();

                if (lockDate == "" || lockTerm == "") {

                    $("#txtExpirationDate").val("");
                    return;
                }

                if (isDate(lockDate, "MM/dd/yyyy") == false) {

                    alert("please enter a valid date.");

                    $("#txtLockDate").focus();
                    return;
                }

                var dtlockDate = Date.parse(lockDate);
                var ExpDate = dtlockDate.addDays(lockTerm);

                $("#txtExpirationDate").val(ExpDate.toString("MM/dd/yyyy"));

            });

        });

        function InitAutoComplete() {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $("#txtLender").autocomplete({

                source: "GetLender_Ajax.aspx?sid=" + sid,
                minLength: 1,
                search: function (event, ui) {

                    $("#hdnLenderID").val("");
                },
                select: function (event, ui) {

                    $("#hdnLenderID").val(ui.item.id);

                    // load ARM
                    LoadARMInfo();
                }
            });

            $("#txtLender").blur(function () {

                if ($("#hdnLenderID").val() == "") {

                    $("#txtLender").val("");
                }
            });

            var settings = {
                'width': 200,
                'selected': function (selectedIndex, selectedVale) {
                    //alert('value: ' + selectedVale.item.value);
                    $("ddlInvestor").change();
                    setTimeout('__doPostBack(\'ddlInvestor\',\'\')', 0);
                }
            };

            var combobox = $("#ddlInvestor").combobox(settings);


        }

        function LoadARMInfo() {
            return;//modify to  autopostback server
            var InvestorID = $("#ddlInvestor").val(); //$("#hdnLenderID").val();
            var LoanProgramID = $("#ddlLoanProgram").val();
//            var LoanProgram = $("#ddlLoanProgram").val();

            if (InvestorID == "") {

                return;
            }

            if (LoanProgramID == "") {

                return;
            }

            var IsARM = $("#ddlLoanProgramARM option[value='" + LoanProgramID + "']").text();
            //alert(IsARM);

            if (IsARM == "false") {

                return;
            }

            //var LoanProgramID = $("#ddlLoanProgram").val(); //$("#ddlLoanProgramID option[value='" + LoanProgram + "']").text();
            //alert(LoanProgramID);

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            $.getJSON("GetLoanProgramDetails_Ajax.aspx?sid=" + sid + "&InvestorID=" + InvestorID + "&LoanProgramID=" + LoanProgramID, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return;
                }

                $("#txtAdjCap").val(data.SubAdj);
                $("#txtLifeCap").val(data.LifetimeCap);
                $("#txtInitialAdjCap").val(data.FirstAdj);
                $("#txtMargin").val(data.Margin);
                $("#txtIndex").val(data.IndexType);
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

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

        function btnSaveFake_onclick() {

            $("#btnSave").trigger("click");
        }

        function btnSyncNowFake_onclick() {

            $("#btnSyncNow").trigger("click");
        }

        function txtExt1LockDate_onchange() {

            var sExt1LockDate = $("#txtExt1LockDate").val();
            if (sExt1LockDate == "") {

                $("#txtExt1ExpireDate").val("");
                return;
            }

            if (isDate(sExt1LockDate, "MM/dd/yyyy") == false) {

                alert("please enter a valid date.");

                $("#txtExt1LockDate").focus();
                return;
            }

            var Ext1Term = $("#ddlExt1Term").val();
            if (Ext1Term == "") {

                $("#txtExt1ExpireDate").val("");
                return;
            }
            var Ext1LockDate = Date.parse(sExt1LockDate);
            var Ext1ExpDate = Ext1LockDate.addDays(Ext1Term);
            $("#txtExt1ExpireDate").val(Ext1ExpDate.toString("MM/dd/yyyy"));
            $("#txtExpirationDate").val(Ext1ExpDate.toString("MM/dd/yyyy"));
        }

        function txtExt2LockDate_onchange() {

            var sExt2LockDate = $("#txtExt2LockDate").val();
            if (sExt2LockDate == "") {

                $("#txtExt2ExpireDate").val("");
                return;
            }

            if (isDate(sExt2LockDate, "MM/dd/yyyy") == false) {

                alert("please enter a valid date.");

                $("#txtExt2LockDate").focus();
                return;
            }

            var Ext2Term = $("#ddlExt2Term").val();
            if (Ext2Term == "") {

                $("#txtExt2ExpireDate").val("");
                return;
            }
            var Ext2LockDate = Date.parse(sExt2LockDate);
            var Ext2ExpDate = Ext2LockDate.addDays(Ext2Term);

            $("#txtExt2ExpireDate").val(Ext2ExpDate.toString("MM/dd/yyyy"));
            $("#txtExpirationDate").val(Ext2ExpDate.toString("MM/dd/yyyy"));
        }

        function txtExt3LockDate_onchange() {

            var sExt3LockDate = $("#txtExt3LockDate").val();
            if (sExt3LockDate == "") {

                $("#txtExt3ExpireDate").val("");
                return;
            }

            if (isDate(sExt3LockDate, "MM/dd/yyyy") == false) {

                alert("please enter a valid date.");

                $("#txtExt3LockDate").focus();
                return;
            }

            var Ext3Term = $("#ddlExt3Term").val();
            if (Ext3Term == "") {

                $("#txtExt3ExpireDate").val("");
                return;
            }
            var Ext3LockDate = Date.parse(sExt3LockDate);
            var Ext3ExpDate = Ext3LockDate.addDays(Ext3Term);

            $("#txtExt3ExpireDate").val(Ext3ExpDate.toString("MM/dd/yyyy"));
            $("#txtExpirationDate").val(Ext3ExpDate.toString("MM/dd/yyyy"));
        }

        function BeforeSave() {

            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return false;
            }
        }

        // add jQuery Validators
        function AddValidators() {

            jQueryValidator = $("#form1").validate({

                rules: {
                    ddlInvestor: {
                        required: true
                    }
                },
                messages: {
                    ddlInvestor: {
                        required: "*"
                    }
                }
            });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 750px;">
        
<%--        <div>
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return BeforeSave();" OnClick="btnSave_Click" CssClass="Btn-66" />--%>
            <%--<asp:Button ID="btnSyncNow" runat="server" Text="Sync Now" OnClick="btnSyncNow_Click" CssClass="Btn-91" />--%>
 <%--       </div>--%>
        <table>
                <tr>
                    <td style="width: 80px;"><asp:Button ID="btnSave" runat="server" Text="Save" OnClientClick="return BeforeSave();" OnClick="btnSave_Click" CssClass="Btn-66" /></td>
                    <td align="left" style="width: 170px;">Loan Amount:<span class="dollar-char">$</span><asp:TextBox ID="txtLoanAmount" runat="server" Width="55px" CssClass="dollar-text" Enabled="false"></asp:TextBox></td>
                    <td style="width: 175px;">Appraised Value:<span class="dollar-char">$</span><asp:TextBox ID="txtApprValue" runat="server" Width="55px" CssClass="dollar-text" Enabled="false"></asp:TextBox></td>
                    <td style="width: 150px;">Sales Price:<span class="dollar-char">$</span><asp:TextBox ID="txtSalesPrice" runat="server" Width="55px" CssClass="dollar-text" Enabled="false"></asp:TextBox></td>
                    <td style="width: 95px; text-align:right;">LTV: <asp:TextBox ID="txtLTV" runat="server" Width="45px" Enabled="false"></asp:TextBox></td>
                    <td style="width: 100px; text-align:right;">CLTV: <asp:TextBox ID="txtCLTV" runat="server" Width="45px" Enabled="false"></asp:TextBox></td>
                </tr>
        </table>

        <div style="margin-top:10px;">

            <table>
                <tr>
                    <td style="width: 80px;">Investor:</td>
                    <td style="width: 360px;">
                        <div style=" display:none"> <asp:TextBox ID="txtLender" runat="server" Width="300px"></asp:TextBox>
                        <asp:HiddenField ID="hdnLenderID" runat="server" />
                        </div>
                        <asp:DropDownList ID="ddlInvestor" runat="server" DataTextField="Investor" DataValueField ="InvestorID" EnableViewState="True"  OnSelectedIndexChanged="ddlInvestor_SelectedIndexChanged" AutoPostBack="True">
                           <%-- <asp:ListItem Value="0">All Investors</asp:ListItem>--%>
                        </asp:DropDownList>
                    </td>
                    
                    <td style="width: 80px;">Loan Program:</td>
                    <td style="width: 130px;">
                        <asp:DropDownList ID="ddlLoanProgram" DataTextField="LoanProgram" DataValueField="LoanProgramID" EnableViewState="True"  OnSelectedIndexChanged="ddlLoanProgram_SelectedIndexChanged" AutoPostBack="True" runat="server" Width="220px">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlLoanProgramARM" runat="server" style="display:none;">
                        </asp:DropDownList>
                        <asp:DropDownList ID="ddlLoanProgramID" runat="server" style="display:none;">
                        </asp:DropDownList>
                    </td>

<%--                    <td colspan="3" align="left">Loan Amount: $
                        <asp:Label ID="lbLoanAmount" runat="server"></asp:Label>--%>
                        <%--<asp:TextBox ID="txtLoanAmount" runat="server" Width="88px" MaxLength="15" ReadOnly="true" BackColor="Gray" CssClass="dollar-text"></asp:TextBox>--%>
<%--                    </td>
                     <td colspan="3">&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="chkLoanChanged" runat="server" Text=" Loan changed" Enabled="false" />
                    </td>--%>
                </tr>
            </table>

            <table>
                <tr>
                    <td style="width: 80px;">Rate:</td>
                    <td style="width: 100px;">
                        <asp:TextBox ID="txtRate" runat="server" Width="80px"  style="text-align: right" Enabled="false" ></asp:TextBox> %
                    </td>
                    <td>Term/Due:</td>
                    <td colspan="2">
                        <asp:TextBox ID="txtTerm" runat="server" Width="40px" MaxLength="3" Enabled="false"></asp:TextBox> / 
                        <asp:TextBox ID="txtDue" runat="server" Width="40px" MaxLength="3" Enabled="false"></asp:TextBox>&nbsp;months
                    </td>
                    <td colspan="3">&nbsp;&nbsp;&nbsp; <asp:CheckBox ID="chkLoanChanged" runat="server" Text=" Loan changed" Enabled="false" Visible="false"/>
                    </td>
<%--                    <td style="width: 80px; text-align:right;">LTV:</td>
                    <td>
                        <asp:TextBox ID="txtLTV" runat="server" Width="50px" Enabled="false"></asp:TextBox> %
                    </td>
                    <td style="width: 80px; text-align:right;">CLTV:</td>
                    <td>
                        <asp:TextBox ID="txtCLTV" runat="server" Width="50px" Enabled="false"></asp:TextBox> %
                    </td>--%>
                </tr>
            </table>

            <table>
                <tr>
                    <td style="width: 80px;">Purpose:</td>
                    <td style="width: 330px;">
                        <!--<asp:TextBox ID="txtPurpose" runat="server" Width="200px" Enabled="false"></asp:TextBox>-->
                        <asp:DropDownList ID="ddlPurpose" runat="server">
                            <asp:ListItem Text="--select--" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Purchase" Value="1190"></asp:ListItem>
                            <asp:ListItem Text="No Cash-Out Refinance" Value="1198"></asp:ListItem>
                            <asp:ListItem Text="Cash-Out Refinance" Value="1193"></asp:ListItem>
                            <asp:ListItem Text="Construction" Value="1192"></asp:ListItem>
                            <asp:ListItem Text="Construction-Permanent" Value="1191"></asp:ListItem>
                            <asp:ListItem Text="Other" Value="1194"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    
                </tr>
            </table>

            <table>
                <tr>
                    <td style="width: 80px;">Occupancy:</td>
                    <td style="width: 330px;">
                       <!-- <asp:TextBox ID="txtOccupancy" runat="server" Width="200px" Enabled="false"></asp:TextBox> -->
                        <asp:DropDownList ID="ddlOccupancy" runat="server">
                        <asp:ListItem Value="0">--select--</asp:ListItem>
                        <asp:ListItem Value="921">Primary</asp:ListItem>
                        <asp:ListItem Value="923" >Secondary</asp:ListItem>
                        <asp:ListItem Value="924">Investment</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 80px;">Property Type:</td>                    
                    <td>
                        <asp:DropDownList ID="ddlPropertyType" runat="server" Width="130px">
                            <asp:ListItem Value="1">1 Unit</asp:ListItem>
                            <asp:ListItem Value="2">2-4 Units</asp:ListItem>
                            <asp:ListItem Value="3">CONDO</asp:ListItem>
                            <asp:ListItem Value="4">PUD</asp:ListItem>
                            <asp:ListItem Value="5">Co-Op</asp:ListItem>
                            <asp:ListItem Value="6">Manufactured Singlewide</asp:ListItem>
                            <asp:ListItem Value="7">Manufactured Multiwide</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                        <%--<asp:TextBox ID="txtPropertyType" runat="server" Width="216px" Enabled="false"></asp:TextBox>--%>
                    <%--</td>--%>
                    
                </tr>
            </table>

            <table>
                <tr>
                    <td style="width: 80px;">Borrower FICO:</td>
                    <td style="width: 100px;">
                        <asp:TextBox ID="txtBorrowerFICO" runat="server" Width="65px" Enabled="false" CssClass="text-align-right"></asp:TextBox>
                    </td>
                    <td style="width: 112px;">First time homebuyer:</td>
                    <td style="width: 110px;">
                        <asp:DropDownList ID="ddlFirsTimeHomebuyer" runat="server" Width="84px">
                            <asp:ListItem>No</asp:ListItem>
                            <asp:ListItem>Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 74px;">Lender Credit:</td>
                    <td style="width: 104px;">
                        <span class="dollar-char">$</span><asp:TextBox ID="txtLenderCredit" runat="server" Width="70px" CssClass="dollar-text"></asp:TextBox>
                    </td>
                    <td>Price:</td>
                    <td>
                        <asp:TextBox ID="txtPrice" runat="server" Width="66px" ></asp:TextBox>%
                        <%--<asp:TextBox ID="TextBox1" runat="server" Width="66px" CssClass="percent-text"></asp:TextBox>%--%>
                    </td>
                </tr>
            </table>

            <table>
                <tr>
                    <td style="width: 80px;">LPMI Factor:</td>
                    <td style="width: 100px;">
                        <asp:TextBox ID="txtLPMIFactor" runat="server" Width="65px" CssClass="percent-text"></asp:TextBox> %
                    </td>
                    <td style="width: 100px;">Compensation Plan:</td>
                    <td style="width: 123px;">
                        <asp:DropDownList ID="ddlCompensationPlan" runat="server" Width="100px">
                            <asp:ListItem>Borrower paid</asp:ListItem>
                            <asp:ListItem>Lender paid</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 80px;">Lock Option:</td>
                    <td>
                        <asp:DropDownList ID="ddlLockOption" runat="server" Width="220px">
                            <asp:ListItem Value="6100">Float</asp:ListItem>
                            <asp:ListItem Value="6101">Lock</asp:ListItem>                         
                         </asp:DropDownList>
                    </td>
                    
                </tr>
            </table>

            <table>
                <tr>
                    <td style="width: 80px;">Escrow taxes:</td>
                    <td style="width: 100px;">
                        <asp:DropDownList ID="ddlEscrowTaxes" runat="server" Width="69px">
                            <asp:ListItem Value="N">No</asp:ListItem>
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 112px;">Escrow Insurance:</td>
                    <td style="width: 110px;">
                        <asp:DropDownList ID="ddlEscrowInsurance" runat="server" Width="84px">
                            <asp:ListItem Value="N">No</asp:ListItem>
                            <asp:ListItem Value="Y">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 80px;">MI Option:</td>
                    <td>
                        <asp:DropDownList ID="ddlMIOption" runat="server" Width="220px">
                            <asp:ListItem Value="N">No MI Required</asp:ListItem>
                            <asp:ListItem Value="B">Borrower Paid MI</asp:ListItem>
                            <asp:ListItem Value="L">Lender Paid MI</asp:ListItem>
                            <asp:ListItem Value="O">No MI Option</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    
                </tr>
            </table>

            <table style="margin-top: 10px;">
                <tr>
                    <td style="width: 60px;">Lock Date:</td>
                    <td style="width: 115px;">
                        <asp:TextBox ID="txtLockDate" runat="server" Width="84px" CssClass="date-field text-align-right"></asp:TextBox>
                    </td>
                    <td style="width: 60px;">Lock Term:</td>
                    <td style="width: 110px;">
                        <asp:DropDownList ID="ddlLockTerm" runat="server" Width="84px">
                            <asp:ListItem Value="">--select--</asp:ListItem>
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>15</asp:ListItem>
                            <asp:ListItem>30</asp:ListItem>
                            <asp:ListItem>45</asp:ListItem>
                            <asp:ListItem>60</asp:ListItem>
                            <asp:ListItem>75</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 80px;">Expiration Date:</td>
                    <td>
                        <asp:TextBox ID="txtExpirationDate" runat="server" Width="80px" CssClass="date-field text-align-right"></asp:TextBox>
                    </td>
                    
                </tr>
            </table>

        </div>

        <div style="margin-top:20px;">
            <fieldset style="padding-top:5px;">
                <legend>&nbsp;ARM Caps&nbsp;</legend>
                
                <table>
                    <tr>
                        <td style="width: 50px;">Adj. Cap:</td>
                        <td style="width: 100px;">
                            <asp:TextBox ID="txtAdjCap" runat="server" Width="60px" CssClass="percent-text cap-field"></asp:TextBox> %
                        </td>
                        <td style="width: 50px;">Life Cap:</td>
                        <td style="width: 110px;">
                            <asp:TextBox ID="txtLifeCap" runat="server" Width="60px" CssClass="percent-text cap-field"></asp:TextBox> %
                        </td>
                        <td style="width: 75px;">Initial 
                        :</td>
                        <td style="width: 100px;">
                            <asp:TextBox ID="txtInitialAdjCap" runat="server" Width="60px" CssClass="percent-text cap-field"></asp:TextBox> %
                        </td>
                        <td style="width: 45px;">Margin:</td>
                        <td>
                            <asp:TextBox ID="txtMargin" runat="server" Width="60px" CssClass="percent-text cap-field"></asp:TextBox> %
                        </td>
                    </tr>
                </table>

                <table style="margin-top: 7px;">
                    <tr>
                        <td style="width: 50px;">Index:</td>
                        <td>
                            <asp:TextBox ID="txtIndex" runat="server" Width="410px" CssClass="cap-field"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </fieldset>
        </div>

        <div style="margin-top:20px;">
            <fieldset style="padding-top:10px;">
                <legend>&nbsp;Lock Info&nbsp;</legend>

                <table>
                    <tr>
                        <td style="width: 60px;">Locked By:</td>
                        <td style="width: 120px;">
                            <asp:Label ID="lbLockedBy" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 75px;">Confirmed By:</td>
                        <td style="width: 120px;">
                            <asp:Label ID="lbConfirmedBy" runat="server" Text=""></asp:Label>
                        </td>
                        
                    
                    </tr>
                </table>
                
                <table style="margin-top:10px;">
                    <tr>
                        <td style="width: 120px;">Original Lock Date:</td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtOrigLockDate" runat="server" Width="80px" ReadOnly="true" CssClass="text-align-right lock-expire-date"></asp:TextBox>
                        </td>
                        <td style="width: 40px;">Term:</td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtOrigLockDateTerm" runat="server" Width="80px" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td style="width: 87px;">Expiration Date:</td>
                        <td>
                            <asp:TextBox ID="txtOrigExpirationDate" runat="server" Width="80px" CssClass="text-align-right lock-expire-date" ReadOnly="true"></asp:TextBox>
                        </td>
                    
                    </tr>
                </table>

                <table style="margin-top:10px;">
                    <tr>
                        <td style="width: 120px;">Extension I Lock Date:</td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtExt1LockDate" runat="server" Width="80px" CssClass="date-field text-align-right"></asp:TextBox>
                        </td>
                        <td style="width: 40px;">Term:</td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlExt1Term" runat="server" Width="84px">
                                <asp:ListItem Value="">-- select --</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>60</asp:ListItem>
                                <asp:ListItem>75</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 87px;">Expiration Date:</td>
                        <td>
                            <asp:TextBox ID="txtExt1ExpireDate" runat="server" Width="80px" CssClass="text-align-right lock-expire-date"></asp:TextBox>
                        </td>
                    
                    </tr>
                </table>          

                <table>
                    <tr>
                        <td style="width: 120px;">Extension II Lock Date:</td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtExt2LockDate" runat="server" Width="80px" CssClass="date-field text-align-right"></asp:TextBox>
                        </td>
                        <td style="width: 40px;">Term:</td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlExt2Term" runat="server" Width="84px">
                                <asp:ListItem Value="">-- select --</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>60</asp:ListItem>
                                <asp:ListItem>75</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 87px;">Expiration Date:</td>
                        <td>
                            <asp:TextBox ID="txtExt2ExpireDate" runat="server" Width="80px" CssClass="text-align-right lock-expire-date"></asp:TextBox>
                        </td>
                    
                    </tr>
                </table>

                <table>
                    <tr>
                        <td style="width: 120px;">Extension III Lock Date:</td>
                        <td style="width: 120px;">
                            <asp:TextBox ID="txtExt3LockDate" runat="server" Width="80px" CssClass="date-field text-align-right"></asp:TextBox>
                        </td>
                        <td style="width: 40px;">Term:</td>
                        <td style="width: 120px;">
                            <asp:DropDownList ID="ddlExt3Term" runat="server" Width="84px">
                                <asp:ListItem Value="">-- select --</asp:ListItem>
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>15</asp:ListItem>
                                <asp:ListItem>30</asp:ListItem>
                                <asp:ListItem>45</asp:ListItem>
                                <asp:ListItem>60</asp:ListItem>
                                <asp:ListItem>75</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 87px;">Expiration Date:</td>
                        <td>
                            <asp:TextBox ID="txtExt3ExpireDate" runat="server" Width="80px" CssClass="text-align-right lock-expire-date"></asp:TextBox>
                        </td>
                   
                    </tr>
                </table>

            </fieldset>
        </div>

        <div style="margin-top:20px;">
            <input id="btnSaveFake" type="button" value="Save" OnClientClick="return BeforeSave();" onclick="btnSaveFake_onclick()" class="Btn-66" />
            <%--<input id="btnSyncNowFake" type="button" value="Sync Now" onclick="btnSyncNowFake_onclick()" class="Btn-91" />--%>
        </div>
        <asp:HiddenField ID="hdnViewLockRate" runat="server" />
    </div>
    </form>
</body>
</html>