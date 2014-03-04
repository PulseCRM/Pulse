<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectDetailIncomeTab.aspx.cs" Inherits="Prospect_ProspectDetailIncomeTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Prospect Detail - Income</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
    .NumText{width: 80px; text-align: right;}
    </style>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.numeric.js" type="text/javascript"></script>
    <script src="../js/jquery.formatCurrency.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#form1").validate();

            $("#gridOtherIncomeList tr td :text").each(function () {

                $(this).rules("add", {
                    required: true,
                    messages: {
                        required: "<div>Please enter Monthly Income.</div>"
                    }
                });

                $(this).numeric({ int: 7, point: 2 });
                $(this).blur(CalcOtherIncomeTotal);
            });

            $(".NumText").numeric({ int: 7, point: 2 });
            $("#txtBase").numeric({ int: 9, point: 2 });

            $("#ddlBorrowerList").change(ddlBorrowerList_onchange);
        });

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridOtherIncomeList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridOtherIncomeList tr td :checkbox").attr("checked", "");
            }
        }

        function CalcOtherIncomeTotal() {

            var Sum = 0.00;

            $("#gridOtherIncomeList tr td :input[id^='txtMonthlyIncome']").each(function (i) {

                var ThisMonthlyIncome = $(this).val().replace(",", "");
                var ThisMonthlyIncomeNum = new Number(ThisMonthlyIncome);
                Sum = Sum + ThisMonthlyIncomeNum

            });

            $("#lbOtherIncomeTotal").text("$" + Sum.toString());

            $('#lbOtherIncomeTotal').formatCurrency();
        }

        //#region Add/Remove OtherIncomes

        function aAdd_onclick() {

            var TrCount = $("#gridOtherIncomeList tr").length;

            // add th
            if (TrCount == 1) {

                // clear tr
                $("#gridOtherIncomeList").empty();

                // add th
                $("#gridOtherIncomeList").append($("#gridOtherIncomeList1 tr").eq(0).clone());
            }

            //#region Add Tr

            // next index
            var NowIndex = new Number($("#hdnCounter").val());
            var NextIndex = NowIndex + 1;

            // clone tr
            var TrCopy = $("#gridOtherIncomeList1 tr").eq(1).clone(true);

            //alert(TrCopy.html());

            // txtMonthlyIncome
            var txtMonthlyIncome_NewID = "txtMonthlyIncomeX" + NextIndex;
            var txtMonthlyIncome_Code = "<input id='" + txtMonthlyIncome_NewID + "' name='" + txtMonthlyIncome_NewID + "' type='text' value='' style='text-align: right;' />"
            // replace
            TrCopy.find("#txtMonthlyIncome").replaceWith(txtMonthlyIncome_Code);

            //alert(TrCopy.html());

            // add tr
            $("#gridOtherIncomeList").append(TrCopy);

            // set counter
            $("#hdnCounter").val(NextIndex);

            // add validate
            $("#" + txtMonthlyIncome_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please enter Monthly Income.</div>"
                }
            });
            $("#" + txtMonthlyIncome_NewID).numeric({ int: 7, point: 2 });

            $("#" + txtMonthlyIncome_NewID).blur(CalcOtherIncomeTotal);

            //#endregion
        }

        function aRemove_onclick() {

            var SelectedCount = $("#gridOtherIncomeList tr:not(:first) td :checkbox[id='chkSelected']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No other income was selected.");
                return;
            }

            var Result = confirm("Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if ($("#gridOtherIncomeList tr:not(:first) td :checkbox[id='chkSelected']:checked").length == $("#gridOtherIncomeList tr:not(:first)").length) {

                $("#gridOtherIncomeList").empty();
                $("#gridOtherIncomeList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no other income.</td></tr>");
            }
            else {

                $("#gridOtherIncomeList tr:not(:first) td :checkbox[id='chkSelected']:checked").parent().parent().remove();
            }

            CalcOtherIncomeTotal();
        }

        //#endregion

        function ddlBorrowerList_onchange() {

            var LoanID = GetQueryString1("LoanID");
            var ContactID = $("#ddlBorrowerList").val();

            var QueryString = "?LoanID=" + LoanID + "&ProspectID=" + ContactID;
            window.location.href = window.location.pathname + QueryString;
        }

        function btnCalc_onclick() {

            var SubTotal = 0.00;

            var Base = $("#txtBase").val().replace(",", "");
            var Overtime = $("#txtOvertime").val().replace(",", "");
            var Bonuses = $("#txtBonuses").val().replace(",", "");
            var Commission = $("#txtCommission").val().replace(",", "");
            var DivInt = $("#txtDivInt").val().replace(",", "");
            var NetRent = $("#txtNetRent").val().replace(",", "");
            var Other = $("#txtOther").val().replace(",", "");

            //alert(Base + "|" + Overtime + "|" + Bonuses + "|" + Commission + "|" + DivInt + "|" + NetRent + "|" + Other);

            var BaseNum = new Number(Base);
            var OvertimeNum = new Number(Overtime);
            var BonusesNum = new Number(Bonuses);
            var CommissionNum = new Number(Commission);
            var DivIntNum = new Number(DivInt);
            var NetRentNum = new Number(NetRent);
            var OtherNum = new Number(Other);

            SubTotal = BaseNum + OvertimeNum + BonusesNum + CommissionNum + DivIntNum + NetRentNum + OtherNum;

            $("#lbSubtotal").text("$" + SubTotal.toString());

            $('#lbSubtotal').formatCurrency();
        }

        function BeforeSave() {

            // call validate
            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return false;
            }

            //#region check Type duplicated

            var bDuplicated = false;
            $("#gridOtherIncomeList tr td :input[id^='ddlType']").each(function (i) {

                var ThisOtherIncomeID = $(this).val();

                for (var j = i + 1; j < $("#gridOtherIncomeList tr td :input[id^='ddlType']").length; j++) {

                    var OtherOtherIncomeID = $("#gridOtherIncomeList tr td :input[id^='ddlType']").eq(j).val();

                    if (ThisOtherIncomeID == OtherOtherIncomeID) {

                        bDuplicated = true;
                        break;
                    }
                }
            });
            // gdc 20120428 Bug #1659
//            if (bDuplicated == true) {

//                alert("The Type column in Other Income List can not be duplicated.");
//                return false;
//            }

            //#endregion

            //#region build OtherIncome list data string

            //#region OtherIncomeTypes

            var OtherIncomeTypes = "";
            $("#gridOtherIncomeList tr td :input[id^='ddlType']").each(function (i) {

                var ThisOtherIncomeType = $(this).val();

                if (OtherIncomeTypes == "") {

                    OtherIncomeTypes = ThisOtherIncomeType
                }
                else {

                    OtherIncomeTypes += "," + ThisOtherIncomeType;
                }
            });

            $("#hdnOtherIncomeTypes").val(OtherIncomeTypes);
            //alert($("#hdnOtherIncomeTypes").val());

            //#endregion

            //#region Monthly Income

            var MonthlyIncomes = "";
            $("#gridOtherIncomeList tr td :input[id^='txtMonthlyIncome']").each(function (i) {

                var ThisMonthlyIncome = $(this).val().replace(",", "");

                if (MonthlyIncomes == "") {

                    MonthlyIncomes = ThisMonthlyIncome
                }
                else {

                    MonthlyIncomes += "," + ThisMonthlyIncome;
                }
            });

            $("#hdnMonthlyIncomes").val(MonthlyIncomes);
            //alert($("#hdnMonthlyIncomes").val());

            //#endregion


            //#endregion

            return true;
        }

        function SendAjax(ProspectID) {

            $("#WaitingMsg").text("Sending request to update borrower info of point file...");
            $("#divTabContent").block({ message: $('#divWaiting'), css: { width: '450px'} });

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            // Ajax
            $.getJSON("UpdateBorrower_Background.aspx?sid=" + Radom + "&ContactID=" + ProspectID, AfterSendAjax);
        }

        function AfterSendAjax(data) {

            if (data.ExecResult == "Failed") {

                alert(data.ErrorMsg);
            }
            else {

                alert("The Point file(s) has been updated successfully.");
            }

            window.location.href = window.location.href;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divTabContent" style="height: 600px;">
        <table>
            <tr>
                <td style="width: 320px; vertical-align: top;">
                    <div style="margin-top: 10px;">
                        <asp:Button ID="btnSaveIncome" runat="server" Text="Save Income" 
                            CssClass="Btn-91" OnClientClick="return BeforeSave()" 
                            onclick="btnSaveIncome_Click" />
                    </div>
                    <div style="margin-left: 20px; margin-top: 10px;">
                        <div>
                            <asp:DropDownList ID="ddlBorrowerList" runat="server" Width="205px" DataValueField="ContactId" DataTextField="ContactName">
                            </asp:DropDownList>
                        </div>
                        <table style="margin-top: 10px;">
                            <tr>
                                <td>Base:</td>
                                <td>
                                    <asp:TextBox ID="txtBase" runat="server" style="text-align: right; width: 80px;"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Overtime:</td>
                                <td>
                                    <asp:TextBox ID="txtOvertime" runat="server" CssClass="NumText"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Bonuses:</td>
                                <td>
                                    <asp:TextBox ID="txtBonuses" runat="server" CssClass="NumText"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 70px;">Commission:</td>
                                <td>
                                    <asp:TextBox ID="txtCommission" runat="server" CssClass="NumText"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Div/Int:</td>
                                <td>
                                    <asp:TextBox ID="txtDivInt" runat="server" CssClass="NumText"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Net Rent:</td>
                                <td>
                                    <asp:TextBox ID="txtNetRent" runat="server" CssClass="NumText"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>Other:</td>
                                <td>
                                    <asp:TextBox ID="txtOther" runat="server" CssClass="NumText"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table style="margin-top: 5px;">
                            <tr>
                                <td style="width: 70px;">Subtotal:</td>
                                <td style="width: 100px;"><asp:Label ID="lbSubtotal" runat="server" Text=""></asp:Label></td>
                                <td>
                                    <input id="btnCalc" type="button" value="Calculate" onclick="btnCalc_onclick()" class="Btn-91" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    
                </td>
                <td style="vertical-align: top; padding-left: 30px; border-left: solid 1px #e4e7ef;">
                    <div class="ModuleTitle">Other Income:</div>
                    <div id="divToolBar" style="margin-top: 10px;">
                        <ul class="ToolStrip" style="margin-left: 0px;">
                            <li><a id="aAdd" href="javascript:aAdd_onclick()">Add</a><span>|</span></li>
                            <li><a id="aRemove" href="javascript:aRemove_onclick()">Remove</a></li>
                        </ul>
                    </div>
                    <div id="divOtherIncomeList" class="ColorGrid" style="margin-top: 5px; width: 610px;">
                            <asp:GridView ID="gridOtherIncomeList" runat="server" EmptyDataText="There is no other income." AutoGenerateColumns="False" CellPadding="4" CssClass="GrayGrid" GridLines="None">
                                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="chkSelected" type="checkbox" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Type" ItemStyle-Width="410px">
                                        <ItemTemplate>
                                            <select id="ddlType" style="width: 390px;" onclick="ShowDialog_SelectOtherIncome(this)">
                                                <option <%# Eval("Type").ToString() == "Alimony/Child Support Income" ? "selected" : "" %> value="Alimony/Child Support Income">Alimony/Child Support Income</option>
                                                <option <%# Eval("Type").ToString() == "Automobile/Expense Account Income" ? "selected" : "" %> value="Automobile/Expense Account Income">Automobile/Expense Account Income</option>
                                                <option <%# Eval("Type").ToString() == "Boarder Income" ? "selected" : "" %> value="Boarder Income">Boarder Income</option>
                                                <option <%# Eval("Type").ToString() == "Foster Care" ? "selected" : "" %> value="Foster Care">Foster Care</option>
                                                <option <%# Eval("Type").ToString() == "Military Base Pay" ? "selected" : "" %> value="Military Base Pay">Military Base Pay</option>
                                                <option <%# Eval("Type").ToString() == "Military Clothes Allowance" ? "selected" : "" %> value="Military Clothes Allowance">Military Clothes Allowance</option>
                                                <option <%# Eval("Type").ToString() == "Military Combat Pay" ? "selected" : "" %> value="Military Combat Pay">Military Combat Pay</option>
                                                <option <%# Eval("Type").ToString() == "Military Flight Pay" ? "selected" : "" %> value="Military Flight Pay">Military Flight Pay</option>
                                                <option <%# Eval("Type").ToString() == "Military Hazard Pay" ? "selected" : "" %> value="Military Hazard Pay">Military Hazard Pay</option>
                                                <option <%# Eval("Type").ToString() == "Military Overseas Pay" ? "selected" : "" %> value="Military Overseas Pay">Military Overseas Pay</option>
                                                <option <%# Eval("Type").ToString() == "Military Prop Pay" ? "selected" : "" %> value="Military Prop Pay">Military Prop Pay</option>
                                                <option <%# Eval("Type").ToString() == "Military Quarters Allowance" ? "selected" : "" %> value="Military Quarters Allowance">Military Quarters Allowance</option>
                                                <option <%# Eval("Type").ToString() == "Military Rations Allowance" ? "selected" : "" %> value="Military Rations Allowance">Military Rations Allowance</option>
                                                <option <%# Eval("Type").ToString() == "Military Variable Housing Allowance" ? "selected" : "" %> value="Military Variable Housing Allowance">Military Variable Housing Allowance</option>
                                                <option <%# Eval("Type").ToString() == "Mortgage Credit Certificate (MCC)" ? "selected" : "" %> value="Mortgage Credit Certificate (MCC)">Mortgage Credit Certificate (MCC)</option>
                                                <option <%# Eval("Type").ToString() == "Notes Receivable/Installment" ? "selected" : "" %> value="Notes Receivable/Installment">Notes Receivable/Installment</option>
                                                <option <%# Eval("Type").ToString() == "Other Income" ? "selected" : "" %> value="Other Income">Other Income</option>
                                                <option <%# Eval("Type").ToString() == "Pension/Retirement Income" ? "selected" : "" %> value="Pension/Retirement Income">Pension/Retirement Income</option>
                                                <option <%# Eval("Type").ToString() == "Real Estate, Mortgage Differential Income" ? "selected" : "" %> value="Real Estate, Mortgage Differential Income">Real Estate, Mortgage Differential Income</option>
                                                <option <%# Eval("Type").ToString() == "Social Security/Disability Income" ? "selected" : "" %> value="Social Security/Disability Income">Social Security/Disability Income</option>
                                                <option <%# Eval("Type").ToString() == "Subject Property Net Cash Flow (two-to-four unit owner-occupied properties)" ? "selected" : "" %> value="Subject Property Net Cash Flow (two-to-four unit owner-occupied properties)" title="Subject Property Net Cash Flow (two-to-four unit owner-occupied properties)">Subject Property Net Cash Flow (two-to-four unit owner-occupied properties)</option>
                                                <option <%# Eval("Type").ToString() == "Trailing Co-Borrower Income" ? "selected" : "" %> value="Trailing Co-Borrower Income">Trailing Co-Borrower Income</option>
                                                <option <%# Eval("Type").ToString() == "Trust Income" ? "selected" : "" %> value="Trust Income">Trust Income</option>
                                                <option <%# Eval("Type").ToString() == "Unemployment/Welfare Income" ? "selected" : "" %> value="Unemployment/Welfare Income">Unemployment/Welfare Income</option>
                                                <option <%# Eval("Type").ToString() == "VA Benefits (Non-education)" ? "selected" : "" %> value="VA Benefits (Non-education)">VA Benefits (Non-education)</option>
                                            </select>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Monthly Income">
                                        <ItemTemplate>
                                            <input id="txtMonthlyIncome<%# Eval("ProspectOtherIncomeId") %>" name="txtMonthlyIncome<%# Eval("ProspectOtherIncomeId") %>" type="text" value="<%# Eval("MonthlyIncome") %>" style="text-align: right;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                    <div id="divOtherIncomeList1" class="ColorGrid" style="margin-top: 5px; width: 610px; display: none;">
                        <div>
                            <table class="GrayGrid" cellspacing="0" cellpadding="4" id="gridOtherIncomeList1" style="border-collapse: collapse;">
                                <tr>
                                    <th class="CheckBoxHeader" scope="col"><input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                                    </th><th scope="col">Type</th>
                                    <th scope="col">Monthly Income</th>
                                </tr>
                                <tr>
                                    <td class="CheckBoxColumn">
                                        <input id="chkSelected" type="checkbox" />
                                    </td>
                                    <td style="width:410px;">
                                        <select id="ddlType" style="width: 390px;" onclick="ShowDialog_SelectOtherIncome(this)">
                                            <option  value="Alimony/Child Support Income">Alimony/Child Support Income</option>
                                            <option  value="Automobile/Expense Account Income">Automobile/Expense Account Income</option>
                                            <option  value="Boarder Income">Boarder Income</option>
                                            <option  value="Foster Care">Foster Care</option>
                                            <option  value="Military Base Pay">Military Base Pay</option>
                                            <option  value="Military Clothes Allowance">Military Clothes Allowance</option>
                                            <option  value="Military Combat Pay">Military Combat Pay</option>
                                            <option  value="Military Flight Pay">Military Flight Pay</option>
                                            <option  value="Military Hazard Pay">Military Hazard Pay</option>
                                            <option  value="Military Overseas Pay">Military Overseas Pay</option>
                                            <option  value="Military Prop Pay">Military Prop Pay</option>
                                            <option  value="Military Quarters Allowance">Military Quarters Allowance</option>
                                            <option  value="Military Rations Allowance">Military Rations Allowance</option>
                                            <option  value="Military Variable Housing Allowance">Military Variable Housing Allowance</option>
                                            <option  value="Mortgage Credit Certificate (MCC)">Mortgage Credit Certificate (MCC)</option>
                                            <option  value="Notes Receivable/Installment">Notes Receivable/Installment</option>
                                            <option  value="Other Income">Other Income</option>
                                            <option  value="Pension/Retirement Income">Pension/Retirement Income</option>
                                            <option  value="Real Estate, Mortgage Differential Income">Real Estate, Mortgage Differential Income</option>
                                            <option  value="Social Security/Disability Income">Social Security/Disability Income</option>
                                            <option  value="Subject Property Net Cash Flow (two-to-four unit owner-occupied properties)" title="Subject Property Net Cash Flow (two-to-four unit owner-occupied properties)">Subject Property Net Cash Flow (two-to-four unit owner-occupied properties)</option>
                                            <option  value="Trailing Co-Borrower Income">Trailing Co-Borrower Income</option>
                                            <option  value="Trust Income">Trust Income</option>
                                            <option  value="Unemployment/Welfare Income">Unemployment/Welfare Income</option>
                                            <option  value="VA Benefits (Non-education)">VA Benefits (Non-education)</option>
                                        </select>
                                    </td>
                                    <td>
                                        <input id="txtMonthlyIncome" type="text" value="" style="text-align: right;" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="GridPaddingBottom">&nbsp;</div>
                    </div>
                    <div style="text-align: right; margin-top: 10px; padding-right: 30px; font-weight: bold;">Total: <asp:Label ID="lbOtherIncomeTotal" runat="server" Text="$1,546.00" style="padding-left: 10px;"></asp:Label></div>
                    
                </td>
            </tr>
        </table>
    </div>
    <div id="divHiddenFields" style="display: none">
        <input id="hdnCounter" type="text" value="0" />
        <asp:HiddenField ID="hdnOtherIncomeTypes" runat="server" />
        <asp:HiddenField ID="hdnMonthlyIncomes" runat="server" />
    </div>
    <div id="divWaiting" style="display: none; padding: 5px;">
		<table style="margin-left: auto; margin-right: auto;">
			<tr>
				<td>
					<img id="imgWaiting" src="../images/waiting.gif" />
				</td>
				<td style="padding-left: 5px;">
					<label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
				</td>
			</tr>
		</table>
	</div>
    </form>
</body>
</html>