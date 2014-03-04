<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailsProfitability.aspx.cs" Inherits="LoanDetails_LoanDetailsProfitability" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Loan Detail – Profitability</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .dollar-text{text-align: right;}
    .percent-text{text-align: right; padding-right: 3px;}
    </style>

    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/accounting.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>

    <script type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            // show waiting
            window.parent.parent.ShowWaitingDialog3("Checking whether point file is locked...");

            var iLoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            // check point file locked
            $.getJSON("CheckPointFileStatusAjax.aspx?sid=" + sid + "&FileId=" + iLoanID, function (data) {

                window.parent.parent.CloseWaitingDialog3();

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return false;
                }
                else {

                    // if locked
                    if (data.ErrorMsg != "") {

                        alert(data.ErrorMsg);

                        // disabled all
                        $("input").attr("disabled", "true");
                        $("select").attr("disabled", "true");

                        return false;
                    }
                    else {

                        AfterCheckPointFileLocked();
                    }
                }
            });

        });

        function AfterCheckPointFileLocked() {

            var ModifyLoan = $("#hdnModifyLoan").val();
            if (ModifyLoan == "false") {

                // disabled all
                $("input").attr("disabled", "true");
                $("select").attr("disabled", "true");
            }
            else {
                $("#txtPrice").numeric({ allow: "-." });

                $("#txtMandatory_Amount").numeric({ allow: "-." });
                $("#txtMandatory_Amount").blur(function () { CalcPoints("txtMandatory_Amount", "txtMandatory_Points"); FormatMoney(this); CalcFinalMandatoryMargin(); });

                // Points
                $("#txtMandatory_Points").numeric({ allow: "-." });
                $("#txtMandatory_Points").blur(function () { CalcAmount("txtMandatory_Amount", "txtMandatory_Points"); FormatPoints(this); });

                $("#txtBestEffortPrice_Amount").numeric({ allow: "-." });
                $("#txtBestEffortPrice_Amount").blur(function () { CalcPoints("txtBestEffortPrice_Amount", "txtBestEffortPrice_Points"); FormatMoney(this); CalcBestEffortMargin(); CalcFinalBestEffortMargin(); });

                // Points
                $("#txtBestEffortPrice_Points").numeric({ allow: "-." });
                $("#txtBestEffortPrice_Points").blur(function () { CalcAmount("txtBestEffortPrice_Amount", "txtBestEffortPrice_Points"); FormatPoints(this); });

                // Points
                $("#txtLenderCredit_Points").numeric({ allow: "-." });
                $("#txtLenderCredit_Points").blur(function () { CalcAmount("txtLenderCredit_Amount", "txtLenderCredit_Points"); FormatPoints(this); });
                
                $("#txtLenderCredit_Amount").numeric({ allow: "-." });
                $("#txtLenderCredit_Amount").blur(function () { CalcPoints("txtLenderCredit_Amount", "txtLenderCredit_Points"); FormatMoney(this); });

                $("#txtLPMI_Points").numeric({ allow: "-." });
                $("#txtLPMI_Points").blur(function () { CalcAmount("txtLPMI_Amount", "txtLPMI_Points"); FormatPoints(this); });

                $("#txtLPMI_Amount").numeric({ allow: "-." });
                $("#txtLPMI_Amount").blur(function () { CalcPoints("txtLPMI_Amount", "txtLPMI_Points"); FormatMoney(this); });
                
                //$("#txtBestEffortPriceToLO_Amount").numeric({ allow: ",." });
                //$("#txtBestEffortPriceToLO_Amount").blur(function () { CalcPoints("txtBestEffortPriceToLO_Amount", "txtBestEffortPriceToLO_Points"); FormatMoney(this); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });

                $("#txtHedgeCost_Amount").numeric({ allow: "-." });
                $("#txtHedgeCost_Amount").blur(function () { CalcPoints("txtHedgeCost_Amount", "txtHedgeCost_Points"); FormatMoney(this); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });
 
                $("#txtHedgeCost_Points").numeric({ allow: "-." });
                $("#txtHedgeCost_Points").blur(function () { CalcAmount("txtHedgeCost_Amount", "txtHedgeCost_Points"); FormatMoney(this); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });

                $("#txtCostOnSale_Amount").numeric({ allow: "-." });
                $("#txtCostOnSale_Amount").blur(function () { CalcPoints("txtCostOnSale_Amount", "txtCostOnSale_Points"); FormatMoney(this); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });

                $("#txtOriginationPts_Points").numeric({ allow: "-." });
                $("#txtOriginationPts_Points").blur(function () { FormatPoints(this); CalcAmount("txtOriginationPts_Amount", "txtOriginationPts_Points"); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });
                $("#txtOriginationPts_Amount").blur(function () { FormatMoney(this); CalcPoints("txtOriginationPts_Amount", "txtOriginationPts_Points"); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });

                $("#txtDiscountPts_Points").numeric({ allow: "-." });
                $("#txtDiscountPts_Points").blur(function () { FormatPoints(this); CalcAmount("txtDiscountPts_Amount", "txtDiscountPts_Points"); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });
                $("#txtDiscountPts_Amount").blur(function () { FormatMoney(this); CalcPoints("txtDiscountPts_Amount", "txtDiscountPts_Points"); CalcBestEffortMargin(); CalcFinalMandatoryMargin(); CalcFinalBestEffortMargin(); });

                $("#txtExtension1_Amount").numeric({ allow: "-." });
                $("#txtExtension1_Amount").blur(function () { CalcPoints("txtExtension1_Amount", "txtExtension1_Points"); FormatMoney(this); CalcFinalBestEffortMargin(); });

                // Points
                $("#txtExtension1_Points").numeric({ allow: "-." });
                $("#txtExtension1_Points").blur(function () { CalcAmount("txtExtension1_Amount", "txtExtension1_Points"); FormatPoints(this); });

                $("#txtExtension2_Amount").numeric({ allow: "-." });
                $("#txtExtension2_Amount").blur(function () { CalcPoints("txtExtension2_Amount", "txtExtension2_Points"); FormatMoney(this); CalcFinalBestEffortMargin(); });

                // Points
                $("#txtExtension2_Points").numeric({ allow: "-." });
                $("#txtExtension2_Points").blur(function () { CalcAmount("txtExtension2_Amount", "txtExtension2_Points"); FormatPoints(this); });
            }

            $("input:text[readonly]").css("background-color", "#f9f9f9");
            $("input:text[disabled]").css("background-color", "#f9f9f9");
        }

        function FormatMoney(textbox) {

            var m0 = $(textbox).val();

            // format
            var m0 = accounting.formatMoney(m0);

            var m1 = m0.replace("$", "");

            $(textbox).val(m1);
        }

        function FormatPoints(textbox) {

            var v = $(textbox).val();

            var num = accounting.formatNumber(v, 3, "");

            $(textbox).val(num);
        }

        function CalcPoints(amount_text_id, points_text_id) {

            var LoanAmount = $("#txtLoanAmount").val();
            LoanAmount = LoanAmount.replace(/,/g, "");
            if (LoanAmount == "") {

                $("#" + amount_text_id).val("");
                $("#" + points_text_id).val("");
                return;
            }

            var Amount = $("#" + amount_text_id).val();
            Amount = Amount.replace(/,/g, "");
            if (Amount == "") {

                $("#" + points_text_id).val("");
                return;
            }

            var AmountNum = new Number(Amount);
            var LoanAmountNum = new Number(LoanAmount);

            if (LoanAmountNum == 0) {

                return;
            }

            var Points = AmountNum / LoanAmountNum * 100;
            //alert(Points);

            $("#" + points_text_id).val(Points.toFixed(3).toString());
        }

        function CalcAmount(amount_text_id, points_text_id) {

            var LoanAmount = $("#txtLoanAmount").val();
            LoanAmount = LoanAmount.replace(/,/g, "");
            if (LoanAmount == "") {

                $("#" + amount_text_id).val("");
                $("#" + points_text_id).val("");
                return;
            }

            var Points = $("#" + points_text_id).val();
            Points = Points.replace(/,/g, "");
            if (Points == "") {

                $("#" + amount_text_id).val("");
                return;
            }

            var PointsNum = new Number(Points);
            var LoanAmountNum = new Number(LoanAmount);

            if (LoanAmountNum == 0) {

                return;
            }

            var Amount = PointsNum * LoanAmountNum / 100;
            //alert(Points);

            $("#" + amount_text_id).val(Amount.toFixed(2).toString());

            FormatMoney($("#" + amount_text_id).get(0));
        }

        function CalcBestEffortMargin() {

            // Best Effort Price
            var txtBestEffortPrice = $("#txtBestEffortPrice_Amount").val();
            txtBestEffortPrice = txtBestEffortPrice.replace(/,/g, "");
            if (txtBestEffortPrice == "") {

                txtBestEffortPrice = "0";
            }
            var txtBestEffortPriceNum = new Number(txtBestEffortPrice);

            // Best Effort Price to LO
            var txtBestEffortPriceToLO = $("#txtBestEffortPriceToLO_Amount").val();
            txtBestEffortPriceToLO = txtBestEffortPriceToLO.replace(/,/g, "");
            if (txtBestEffortPriceToLO == "") {

                txtBestEffortPriceToLO = "0";
            }
            var txtBestEffortPriceToLONum = new Number(txtBestEffortPriceToLO);

            // Hedge Cost
            // LCW 10/05/2013 per MSA's Request Remove HedgeCost from BestEffortMargin Calc
//            var txtHedgeCost = $("#txtHedgeCost_Amount").val();
//            txtHedgeCost = txtHedgeCost.replace(/,/g, "");
//            if (txtHedgeCost == "") {

//                txtHedgeCost = "0";
//            }
//            var txtHedgeCostNum = new Number(txtHedgeCost);

            // Cost On Sale
            var txtCostOnSale = $("#txtCostOnSale_Amount").val();
            txtCostOnSale = txtCostOnSale.replace(/,/g, "");
            if (txtCostOnSale == "") {

                txtCostOnSale = "0";
            }
            var txtCostOnSaleNum = new Number(txtCostOnSale);

            // Discount Pts
            var txtOriginationPts = $("#txtOriginationPts_Amount").val();
            txtOriginationPts = txtOriginationPts.replace(/,/g, "");
            if (txtOriginationPts == "") {

                txtOriginationPts = "0";
            }
            var txtOriginationPtsNum = new Number(txtOriginationPts);

            // Discount Pts
            var txtDiscountPts = $("#txtDiscountPts_Amount").val();
            txtDiscountPts = txtDiscountPts.replace(/,/g, "");
            if (txtDiscountPts == "") {

                txtDiscountPts = "0";
            }
            var txtDiscountPtsNum = new Number(txtDiscountPts);


            var BestEffortMarginNum = txtBestEffortPriceNum - txtBestEffortPriceToLONum + txtCostOnSaleNum + txtOriginationPtsNum + txtDiscountPtsNum;

            var x = accounting.formatNumber(BestEffortMarginNum, 2, ",");
            $("#txtBestEffortMargin_Amount").val(x);
 
            CalcPoints("txtBestEffortMargin_Amount", "txtBestEffortMargin_Points");
        }

        function CalcFinalMandatoryMargin() {

            // Mandatory/Final Price
            var txtMandatory = $("#txtMandatory_Amount").val();
            txtMandatory = txtMandatory.replace(/,/g, "");
            if (txtMandatory == "") {

                txtBestEffortPrice = "0";
            }
            var txtMandatoryNum = new Number(txtMandatory);

            // Best Effort Price to LO
            var txtBestEffortPriceToLO = $("#txtBestEffortPriceToLO_Amount").val();
            txtBestEffortPriceToLO = txtBestEffortPriceToLO.replace(/,/g, "");
            if (txtBestEffortPriceToLO == "") {

                txtBestEffortPriceToLO = "0";
            }
            var txtBestEffortPriceToLONum = new Number(txtBestEffortPriceToLO);

            // Hedge Cost
            var txtHedgeCost = $("#txtHedgeCost_Amount").val();
            txtHedgeCost = txtHedgeCost.replace(/,/g, "");
            if (txtHedgeCost == "") {

                txtHedgeCost = "0";
            }
            var txtHedgeCostNum = new Number(txtHedgeCost);

            // Cost On Sale
            var txtCostOnSale = $("#txtCostOnSale_Amount").val();
            txtCostOnSale = txtCostOnSale.replace(/,/g, "");
            if (txtCostOnSale == "") {

                txtCostOnSale = "0";
            }
            var txtCostOnSaleNum = new Number(txtCostOnSale);

            // Discount Pts
            var txtOriginationPts = $("#txtOriginationPts_Amount").val();
            txtOriginationPts = txtOriginationPts.replace(/,/g, "");
            if (txtOriginationPts == "") {

                txtOriginationPts = "0";
            }
            var txtOriginationPtsNum = new Number(txtOriginationPts);

            // Discount Pts
            var txtDiscountPts = $("#txtDiscountPts_Amount").val();
            txtDiscountPts = txtDiscountPts.replace(/,/g, "");
            if (txtDiscountPts == "") {

                txtDiscountPts = "0";
            }
            var txtDiscountPtsNum = new Number(txtDiscountPts);

            var FinalMandatoryMarginNum = txtMandatoryNum - txtBestEffortPriceToLONum + txtHedgeCostNum + txtCostOnSaleNum + txtOriginationPtsNum + txtDiscountPtsNum;

            var x = accounting.formatNumber(FinalMandatoryMarginNum, 2, ",");
            $("#txtFinalMandatoryMargin_Amount").val(x);

            CalcPoints("txtFinalMandatoryMargin_Amount", "txtFinalMandatoryMargin_Points");
            CalcMandatoryPickup();
        }

        function CalcFinalBestEffortMargin() {

            // Best Effort Price
            var txtBestEffortMargin = $("#txtBestEffortMargin_Amount").val();
            txtBestEffortMargin = txtBestEffortMargin.replace(/,/g, "");
            if (txtBestEffortMargin == "") {

                txtBestEffortMargin = "0";
            }
            var txtBestEffortMarginNum = new Number(txtBestEffortMargin);

            // Extention1
            var txtExtension1 = $("#txtExtension1_Amount").val();
            txtExtension1 = txtExtension1.replace(/,/g, "");
            if (txtExtension1 == "") {

                txtExtension1 = "0";
            }
            var txtExtension1Num = new Number(txtExtension1);


            // Extention2
            var txtExtension2 = $("#txtExtension2_Amount").val();
            txtExtension2 = txtExtension2.replace(/,/g, "");
            if (txtExtension2 == "") {

                txtExtension2 = "0";
            }
            var txtExtension2Num = new Number(txtExtension2);

            var FinalBestEffortMarginNum = txtBestEffortMarginNum + txtExtension1Num + txtExtension2Num;

            var x = accounting.formatNumber(FinalBestEffortMarginNum, 2, ",");
            $("#txtFinalBestEffortMargin_Amount").val(x);

            CalcPoints("txtFinalBestEffortMargin_Amount", "txtFinalBestEffortMargin_Points");
            CalcMandatoryPickup();
        }

        function CalcMandatoryPickup() {
            var txtFinalBestEffortMargin= $("#txtFinalBestEffortMargin_Amount").val();
            var FinalBestEffortMarginNum = new Number(txtFinalBestEffortMargin);
            var txtFinalMandatoryMargin = $("#txtFinalMandatoryMargin_Amount").val();
            var FinalMandatoryMarginNum = new Number(txtFinalMandatoryMargin);
            var MandatoryPickupNum = FinalMandatoryMarginNum - FinalBestEffortMarginNum;
            var x = accounting.formatNumber(MandatoryPickupNum, 2, ",");
            $("#txtMandatoryPickup_Amount").val(x);

            CalcPoints("txtMandatoryPickup_Amount", "txtMandatoryPickup_Points");
        }
        function btnLock_onclick() {

            var bShowLockRatePopup = $("#<%= hdnShowLockRatePopup.ClientID %>").val();
            if (bShowLockRatePopup != "true") {

                alert("You do not have the privilege to view the lock information or lock the rate.");
                return false;
            }
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var FileId = GetQueryString1("FileID");

            var iFrameSrc = "LockRatePopup.aspx?FileId=" + FileId + "&sid=" + sid;

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Lock Rate", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function btnComp_onclick() {

            var bShowLockRatePopup = $("#<%= hdnShowCompensationDetailPopup.ClientID %>").val();
            if (bShowLockRatePopup != "true") {

                alert("You do not have the privilege to view compensation.");
                return false;
            }
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var FileId = GetQueryString1("FileID");

            var iFrameSrc = "CompensationDetailPopup.aspx?LoanId=" + FileId + "&sid=" + sid;

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Compensation Detail", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

// ]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="margin-top:10px;">
        
        <table>
            <tr>
                <td>
                    <h5>Best Effort Loan Profitability</h5>
                </td>
                <td style="padding-left:80px;">
                    <input id="btnLock" type="button" value="Lock" onclick="btnLock_onclick()" class="Btn-66" />
                </td>
                <td style="padding-left:10px;">
                    <input id="btnComp" type="button" value="Comp" onclick="btnComp_onclick()" class="Btn-66" />
                </td>
            </tr>
        </table>

        <table class="margin-top-10">
            <tr>
                <td style="width: 400px;">
                    
                    <table>
                        <tr>
                            <td style="width:130px;">&nbsp;</td>
                            <td style="width:130px; text-align: center;">Amount</td>
                            <td style="text-align: center;">Basis Points</td>
                        </tr>
                        <tr>
                            <td>Loan Amount</td>
                            <td>
                                $ <asp:TextBox ID="txtLoanAmount" runat="server" Width="100px" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Mandatory / Final Price</td>
                            <td>
                                $ <asp:TextBox ID="txtMandatory_Amount" runat="server" Width="100px" MaxLength="15" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMandatory_Points" runat="server" Width="80px" CssClass="percent-text" ReadOnly="true"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Best Effort Price</td>
                            <td>
                                $ <asp:TextBox ID="txtBestEffortPrice_Amount" runat="server" Width="100px" MaxLength="15" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBestEffortPrice_Points" runat="server" Width="80px" CssClass="percent-text"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Commission Total</td>
                            <td>
                                $ <asp:TextBox ID="txtCommissionTotal_Amount" runat="server" Width="100px" CssClass="dollar-text" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCommissionTotal_Points" runat="server" Width="80px" CssClass="percent-text" ></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Lender Credit</td>
                            <td>
                                $ <asp:TextBox ID="txtLenderCredit_Amount" runat="server" Width="100px" CssClass="dollar-text" ></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLenderCredit_Points" runat="server" Width="80px" CssClass="percent-text"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>LPMI</td>                    
                            <td>
                                $ <asp:TextBox ID="txtLPMI_Amount" runat="server" Width="100px" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLPMI_Points" runat="server" Width="80px" CssClass="percent-text" ></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>                                                    
                                <td>Best Effort Price to LO</td>                           
                            <td>
                                $ <asp:TextBox ID="txtBestEffortPriceToLO_Amount" runat="server" Width="100px" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBestEffortPriceToLO_Points" runat="server" Width="80px" CssClass="percent-text" ReadOnly="true"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Hedge Cost</td>
                            <td>
                                $ <asp:TextBox ID="txtHedgeCost_Amount" runat="server" Width="100px" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHedgeCost_Points" runat="server" Width="80px" CssClass="percent-text" ReadOnly="true"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Cost on sale</td>
                            <td>
                                $ <asp:TextBox ID="txtCostOnSale_Amount" runat="server" Width="100px" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCostOnSale_Points" runat="server" Width="80px" CssClass="percent-text" ></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <a href="#">Origination Pts</a>
                            </td>
                            <td>
                                $ <asp:TextBox ID="txtOriginationPts_Amount" runat="server" Width="100px" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOriginationPts_Points" runat="server" Width="80px" CssClass="percent-text" ></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <a href="#">Discount Pts</a>
                            </td>
                            <td>
                                $ <asp:TextBox ID="txtDiscountPts_Amount" runat="server" Width="100px" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiscountPts_Points" runat="server" Width="80px" CssClass="percent-text" ></asp:TextBox>%
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="border-bottom: 1px solid silver;">&nbsp;</td>
                        </tr>

                        <tr>
                            <td style="padding-top:10px;">Best Effort Margin</td>
                            <td style="padding-top:10px;">
                                $ <asp:TextBox ID="txtBestEffortMargin_Amount" runat="server" Width="100px" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="padding-top:10px;">
                                <asp:TextBox ID="txtBestEffortMargin_Points" runat="server" Width="80px" CssClass="percent-text" ReadOnly="true"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Extension</td>
                            <td>
                                $ <asp:TextBox ID="txtExtension1_Amount" runat="server" Width="100px" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExtension1_Points" runat="server" Width="80px" CssClass="percent-text"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Extension</td>
                            <td>
                                $ <asp:TextBox ID="txtExtension2_Amount" runat="server" Width="100px" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExtension2_Points" runat="server" Width="80px" CssClass="percent-text"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="border-bottom: 1px solid silver;">&nbsp;</td>
                        </tr>
                
                        <tr>
                            <td style="padding-top:10px;">Final Mandatory Margin</td>
                            <td style="padding-top:10px;">
                                $ <asp:TextBox ID="txtFinalMandatoryMargin_Amount" runat="server" Width="100px" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="padding-top:10px;">
                                <asp:TextBox ID="txtFinalMandatoryMargin_Points" runat="server" Width="80px" CssClass="percent-text" ReadOnly="true"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Final Best Effort Margin</td>
                            <td>
                                $ <asp:TextBox ID="txtFinalBestEffortMargin_Amount" runat="server" Width="100px" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFinalBestEffortMargin_Points" runat="server" Width="80px" CssClass="percent-text" ReadOnly="true"></asp:TextBox> %
                            </td>
                        </tr>
                        <tr>
                            <td>Mandatory Pickup</td>
                            <td>
                                $ <asp:TextBox ID="txtMandatoryPickup_Amount" runat="server" Width="100px" CssClass="dollar-text" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMandatoryPickup_Points" runat="server" Width="80px" CssClass="percent-text" ReadOnly="true"></asp:TextBox> %
                            </td>
                        </tr>
                    </table>

                </td>
                <td style="vertical-align: top;">
                    
                    <table>
                        <tr>
                            <td>&nbsp;</td>
                            <td style="text-align: center;">Loan<br />Amount</td>
                            <td style="text-align: center;">Sum<br />Price</td>
                            <td style="text-align: center;">LLPA</td>
                            <td style="text-align: center;">Net<br />Sell</td>
                            <td style="text-align: center;">SRP</td>
                            <td style="text-align: center;">Investor</td>
                        </tr>
                        <tr>
                            <td style="width: 90px;">Market to Market<br />Daily Price</td>
                            <td style="width: 95px;">
                                $ <asp:TextBox ID="txtMM_LoanAmount" runat="server" Width="80px" ReadOnly="true" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td style="width: 70px;">
                                <asp:TextBox ID="txtMM_SumPrice" runat="server" Width="55px" ReadOnly="true" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td style="width: 70px;">
                                <asp:TextBox ID="txtMM_LLPA" runat="server" Width="55px" ReadOnly="true" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td style="width: 70px;">
                                <asp:TextBox ID="txtMM_NetSell" runat="server" Width="55px" ReadOnly="true" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td style="width: 70px;">
                                <asp:TextBox ID="txtMM_SRP" runat="server" Width="55px" ReadOnly="true" CssClass="dollar-text"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMM_Investor" runat="server" ReadOnly="true" Width="80px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>

                    <table>
                        <tr>
                            <td style="width: 99px;">Commitment #:</td>
                            <td>
                                <asp:TextBox ID="txtCommitmentNumber" runat="server" Width="66px" ReadOnly="true"></asp:TextBox>
                            </td>                            
                        </tr>
                    </table>

                    <table>
                        <tr>
                            <td style="width: 99px;">Commitment Date:</td>
                            <td>
                                <asp:TextBox ID="txtCommitmentDate" runat="server" Width="66px" ReadOnly="true"></asp:TextBox>
                            </td>                            
                        </tr>
                    </table>

                    <table>
                        <tr>
                            <td style="width: 99px;">Commitment Term:</td>
                            <td>
                                <asp:TextBox ID="txtCommitmentTerm" runat="server" Width="66px" ReadOnly="true"></asp:TextBox>
                            </td>                            
                        </tr>
                    </table>

                    <table>
                        <tr>
                            <td style="width: 99px;">Commitment Expiration Date:</td>
                            <td>
                                <asp:TextBox ID="txtCommitmentExpirationDate" runat="server" Width="66px" ReadOnly="true"></asp:TextBox>
                            </td>                            
                        </tr>
                    </table>

                    <table>
                        <tr>
                            <td style="width: 99px;">Compensation<br />Plan:</td>
                            <td>
                                <asp:DropDownList ID="ddlCompensationPlan" runat="server" Width="120px">
                                    <asp:ListItem>Borrower paid</asp:ListItem>
                                    <asp:ListItem>Lender paid</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        
                    </table>
                    <table>
                        <tr>
                            <td style="width: 99px;">Price:</td>
                            <td>
                                <asp:TextBox ID="txtPrice" runat="server" Width="66px" ReadOnly="true" CssClass="percent-text"></asp:TextBox>
                            </td>
                            <td>%</td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="width: 99px;">Lock Option:</td>
                            <td>
                                <asp:DropDownList ID="ddlLockOption" runat="server" Width="220px">
                              <asp:ListItem Text="--select--" Value="0" Selected="True"/>
                            <asp:ListItem Text="Float" Value="6100" Selected="False"/>
                           <asp:ListItem Text="Lock" Value="6101" Selected="False"/>
                           </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 99px;">MI Option:</td>
                            <td>
                                <asp:DropDownList ID="ddlMIOption" runat="server" Width="220px">
                                    <asp:ListItem Value="N">No MI Required</asp:ListItem>
                                    <asp:ListItem Value="B">Borrower Paid MI</asp:ListItem>
                                    <asp:ListItem Value="L">Lender Paid MI</asp:ListItem>
                                    <asp:ListItem Value="O">No MI Option</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 99px;">First time<br />homebuyer:</td>
                            <td>
                                <asp:DropDownList ID="ddlFirsTimeHomebuyer" runat="server" Width="84px">
                                    <asp:ListItem>No</asp:ListItem>
                                    <asp:ListItem>Yes</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 99px;">Escrow taxes:</td>
                            <td>
                                <asp:DropDownList ID="ddlEscrowTaxes" runat="server" Width="84px">
                                    <asp:ListItem Value="N">No</asp:ListItem>
                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 99px;">Escrow<br />Insurance:</td>
                            <td>
                                <asp:DropDownList ID="ddlEscrowInsurance" runat="server" Width="84px">
                                    <asp:ListItem Value="N">No</asp:ListItem>
                                    <asp:ListItem Value="Y">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
        </table>

        <div class="margin-top-20">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="Btn-66" />
        </div>
        <asp:HiddenField ID="hdnShowLockRatePopup" runat="server" />
        <asp:HiddenField ID="hdnShowCompensationDetailPopup" runat="server" />
        <asp:HiddenField ID="hdnModifyLoan" runat="server" />
    </div>
    </form>
</body>
</html>