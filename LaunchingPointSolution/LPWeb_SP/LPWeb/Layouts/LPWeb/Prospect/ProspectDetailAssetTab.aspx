<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectDetailAssetTab.aspx.cs" Inherits="Prospect_ProspectDetailAssetTab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Prospect Detail - Income</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.numeric.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#form1").validate();

            $("#gridAssetsList tr td :text[id^='txtAmount']").each(function () {

                $(this).rules("add", {
                    required: true,
                    messages: {
                        required: "<div>Please enter Monthly Income.</div>"
                    }
                });

                $(this).numeric({ int: 9, point: 2 });
            });



            $("#ddlBorrowerList").change(ddlBorrowerList_onchange);
        });

        function chkJoint_onclick(checkbox) {

            if (checkbox.checked == true) {

                $("#ddlBorrowerList").attr("disabled", "true");
            }
            else {

                $("#ddlBorrowerList").attr("disabled", "");
            }
        }

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gridAssetsList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gridAssetsList tr td :checkbox").attr("checked", "");
            }
        }

        //#region Add/Remove Assetss

        function aAdd_onclick() {

            var TrCount = $("#gridAssetsList tr").length;

            // add th
            if (TrCount == 1) {

                // clear tr
                $("#gridAssetsList").empty();

                // add th
                $("#gridAssetsList").append($("#gridAssetsList1 tr").eq(0).clone());
            }

            //#region Add Tr

            // next index
            var NowIndex = new Number($("#hdnCounter").val());
            var NextIndex = NowIndex + 1;

            // clone tr
            var TrCopy = $("#gridAssetsList1 tr").eq(1).clone(true);

            //alert(TrCopy.html());

            //#region replace textbox

            // txtAmount
            var txtAmount_NewID = "txtAmountX" + NextIndex;
            var txtAmount_Code = "<input id='" + txtAmount_NewID + "' name='" + txtAmount_NewID + "' type='text' value='' style='text-align: right; width: 120px;' />"
            // replace
            TrCopy.find("#txtAmount").replaceWith(txtAmount_Code);

            // txtName
            var txtName_NewID = "txtNameX" + NextIndex;
            var txtName_Code = "<input id='" + txtName_NewID + "' name='" + txtName_NewID + "' type='text' value='' maxlength='50' style='width: 260px;' />"
            // replace
            TrCopy.find("#txtName").replaceWith(txtName_Code);

            // txtAccount
            var txtAccount_NewID = "txtAccountX" + NextIndex;
            var txtAccount_Code = "<input id='" + txtAccount_NewID + "' name='" + txtAccount_NewID + "' type='text' value='' maxlength='50' style='width: 150px;' />"
            // replace
            TrCopy.find("#txtAccount").replaceWith(txtAccount_Code);

            //#endregion

            //alert(TrCopy.html());

            // add tr
            $("#gridAssetsList").append(TrCopy);

            // set counter
            $("#hdnCounter").val(NextIndex);

            //#region add validate

            $("#" + txtAmount_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please enter Amount.</div>"
                }
            });

            $("#" + txtAmount_NewID).numeric({ int: 9, point: 2 });

            //#endregion

            //#endregion
        }

        function aRemove_onclick() {

            var SelectedCount = $("#gridAssetsList tr:not(:first) td :checkbox[id='chkSelected']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No asset was selected.");
                return;
            }

            var Result = confirm("Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if ($("#gridAssetsList tr:not(:first) td :checkbox[id='chkSelected']:checked").length == $("#gridAssetsList tr:not(:first)").length) {

                $("#gridAssetsList").empty();
                $("#gridAssetsList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no assets.</td></tr>");
            }
            else {

                $("#gridAssetsList tr:not(:first) td :checkbox[id='chkSelected']:checked").parent().parent().remove();
            }

        }

        //#endregion

        function ddlBorrowerList_onchange() {

            var LoanID = GetQueryString1("LoanID");
            var ContactID = $("#ddlBorrowerList").val();

            var QueryString = "?LoanID=" + LoanID + "&ProspectID=" + ContactID;
            window.location.href = window.location.pathname + QueryString;
        }

        function BeforeSave() {

            // call validate
            var IsValid = $("#form1").valid();
            if (IsValid == false) {

                return false;
            }

            //#region check Type duplicated

            var bDuplicated = false;
            $("#gridAssetsList tr td :input[id^='ddlType']").each(function (i) {

                var ThisAssetsID = $(this).val();

                for (var j = i + 1; j < $("#gridAssetsList tr td :input[id^='ddlType']").length; j++) {

                    var OtherAssetsID = $("#gridAssetsList tr td :input[id^='ddlType']").eq(j).val();

                    if (ThisAssetsID == OtherAssetsID) {

                        bDuplicated = true;
                        break;
                    }
                }
            });

            if (bDuplicated == true) {

                alert("The Type column in Other Income List can not be duplicated.");
                return false;
            }

            //#endregion

            //#region build Assets list data string

            //#region AssetsTypes

            var AssetsTypes = "";
            $("#gridAssetsList tr td :input[id^='ddlType']").each(function (i) {

                var ThisAssetsType = $(this).val();

                if (AssetsTypes == "") {

                    AssetsTypes = ThisAssetsType
                }
                else {

                    AssetsTypes += "," + ThisAssetsType;
                }
            });

            $("#hdnAssetsTypes").val(AssetsTypes);
            //alert($("#hdnAssetsTypes").val());

            //#endregion

            //#region Amounts

            var Amounts = "";
            $("#gridAssetsList tr td :input[id^='txtAmount']").each(function (i) {

                var ThisAmount = $(this).val().replace(",", "");

                if (Amounts == "") {

                    Amounts = ThisAmount
                }
                else {

                    Amounts += "," + ThisAmount;
                }
            });

            $("#hdnAmounts").val(Amounts);
            //alert($("#hdnAmounts").val());

            //#endregion

            //#region Names

            var Names = "";
            $("#gridAssetsList tr td :input[id^='txtName']").each(function (i) {

                var ThisName = $(this).val();

                if (Names == "") {

                    Names = ThisName
                }
                else {

                    Names += "|" + ThisName;
                }
            });

            $("#hdnNames").val(Names);
            //alert($("#hdnNames").val());

            //#endregion

            //#region AccountNums

            var AccountNums = "";
            $("#gridAssetsList tr td :input[id^='txtAccount']").each(function (i) {

                var ThisAccountNum = $(this).val();

                if (AccountNums == "") {

                    AccountNums = ThisAccountNum
                }
                else {

                    AccountNums += "|" + ThisAccountNum;
                }
            });

            $("#hdnAccountNums").val(AccountNums);
            //alert($("#hdnAccountNums").val());

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
                    
                    <table style="margin-top: 10px;">
                        <tr>
                            <td>
                                <input id="chkJoint" runat="server" type="checkbox" onclick="chkJoint_onclick(this)" />
                            </td>
                            <td>Joint</td>
                            <td style="padding-left: 10px;">
                                <asp:DropDownList ID="ddlBorrowerList" runat="server" Width="205px" DataValueField="ContactId" DataTextField="ContactName">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="margin-top: 15px;">
                        <asp:Button ID="btnSaveIncome" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" OnClick="btnSave_Click" />
                    </div>
                    <div style="margin-top: 20px;">
                        <div id="divToolBar">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aAdd" href="javascript:aAdd_onclick()">Add</a><span>|</span></li>
                                <li><a id="aRemove" href="javascript:aRemove_onclick()">Remove</a></li>
                            </ul>
                        </div>
                        <div id="divAssetsList" class="ColorGrid" style="margin-top: 5px; width: 900px;">
                                <asp:GridView ID="gridAssetsList" runat="server" EmptyDataText="There is no assets." AutoGenerateColumns="False" CellPadding="4" CssClass="GrayGrid" GridLines="None">
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
                                        <asp:TemplateField HeaderText="Type" ItemStyle-Width="240px">
                                            <ItemTemplate>
                                                <select id="ddlType" style="width: 220px;" onclick="ShowDialog_SelectAssets(this)">
                                                    <option <%# Eval("Type").ToString() == "Bridge Loan Not Deposited" ? "selected" : "" %> value="Bridge Loan Not Deposited">Bridge Loan Not Deposited</option>
                                                    <option <%# Eval("Type").ToString() == "Cash-on-Hand" ? "selected" : "" %> value="Cash-on-Hand">Cash-on-Hand</option>
                                                    <option <%# Eval("Type").ToString() == "Checking Account" ? "selected" : "" %> value="Checking Account">Checking Account</option>
                                                    <option <%# Eval("Type").ToString() == "Gift" ? "selected" : "" %> value="Gift">Gift</option>
                                                    <option <%# Eval("Type").ToString() == "Gift of Equity" ? "selected" : "" %> value="Gift of Equity">Gift of Equity</option>
                                                    <option <%# Eval("Type").ToString() == "Money Market Fund" ? "selected" : "" %> value="Money Market Fund">Money Market Fund</option>
                                                    <option <%# Eval("Type").ToString() == "Mutual Funds" ? "selected" : "" %> value="Mutual Funds">Mutual Funds</option>
                                                    <option <%# Eval("Type").ToString() == "Net Equity" ? "selected" : "" %> value="Net Equity">Net Equity</option>
                                                    <option <%# Eval("Type").ToString() == "Other Liquid Asset" ? "selected" : "" %> value="Other Liquid Asset">Other Liquid Asset</option>
                                                    <option <%# Eval("Type").ToString() == "Pending Sale Proceeds from Real Estate" ? "selected" : "" %> value="Pending Sale Proceeds from Real Estate">Pending Sale Proceeds from Real Estate</option>
                                                    <option <%# Eval("Type").ToString() == "Savings" ? "selected" : "" %> value="Savings">Savings</option>
                                                    <option <%# Eval("Type").ToString() == "Secured Borrowed Funds" ? "selected" : "" %> value="Secured Borrowed Funds">Secured Borrowed Funds</option>
                                                    <option <%# Eval("Type").ToString() == "Time Deposit" ? "selected" : "" %> value="Time Deposit">Time Deposit</option>
                                                    <option <%# Eval("Type").ToString() == "Trust Fund Account" ? "selected" : "" %> value="Trust Fund Account">Trust Fund Account</option>
                                                </select>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Amount" ItemStyle-Width="140px">
                                            <ItemTemplate>
                                                <input id="txtAmount<%# Eval("ProspectAssetId") %>" name="txtAmount<%# Eval("ProspectAssetId") %>" type="text" value="<%# Eval("Amount") %>" style="text-align: right; width: 120px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name">
                                            <ItemTemplate>
                                                <input id="txtName<%# Eval("ProspectAssetId") %>" name="txtName<%# Eval("ProspectAssetId") %>" type="text" value="<%# Eval("Name") %>" maxlength="50" style="width: 260px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Account #" ItemStyle-Width="170px">
                                            <ItemTemplate>
                                                <input id="txtAccount<%# Eval("ProspectAssetId") %>" name="txtAccount<%# Eval("ProspectAssetId") %>" type="text" value="<%# Eval("Account") %>" maxlength="50" style="width: 150px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div class="GridPaddingBottom">&nbsp;</div>
                            </div>
                        <div id="divAssetsList1" class="ColorGrid" style="margin-top: 5px; width: 900px; display: none;">
                            <div>
                                <table class="GrayGrid" cellspacing="0" cellpadding="4" id="gridAssetsList1" style="border-collapse: collapse;">
                                    <tr>
                                        <th class="CheckBoxHeader" scope="col">
                                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                                        </th>
                                        <th scope="col">
                                            Type
                                        </th>
                                        <th scope="col">
                                            Amount
                                        </th>
                                        <th scope="col">
                                            Name
                                        </th>
                                        <th scope="col">
                                            Account #
                                        </th>
                                    </tr>
                                    <tr>
                                        <td class="CheckBoxColumn">
                                            <input id="chkSelected" type="checkbox" />
                                        </td>
                                        <td style="width: 240px;">
                                            <select id="ddlType" style="width: 220px;" onclick="ShowDialog_SelectAssets(this)">
                                                <option value="Bridge Loan Not Deposited">Bridge Loan Not Deposited</option>
                                                <option value="Cash-on-Hand">Cash-on-Hand</option>
                                                <option value="Checking Account">Checking Account</option>
                                                <option value="Gift">Gift</option>
                                                <option value="Gift of Equity">Gift of Equity</option>
                                                <option value="Money Market Fund">Money Market Fund</option>
                                                <option value="Mutual Funds">Mutual Funds</option>
                                                <option value="Net Equity">Net Equity</option>
                                                <option value="Other Liquid Asset">Other Liquid Asset</option>
                                                <option value="Pending Sale Proceeds from Real Estate">Pending Sale Proceeds from Real Estate</option>
                                                <option value="Savings">Savings</option>
                                                <option value="Secured Borrowed Funds">Secured Borrowed Funds</option>
                                                <option value="Time Deposit">Time Deposit</option>
                                                <option value="Trust Fund Account">Trust Fund Account</option>
                                            </select>
                                        </td>
                                        <td style="width: 140px;">
                                            <input id="txtAmount" name="txtAmount" type="text" value="" style="text-align: right; width: 120px;" />
                                        </td>
                                        <td>
                                            <input id="txtName" name="txtName" type="text" value="" maxlength="50" style="width: 260px;" />
                                        </td>
                                        <td style="width: 170px;">
                                            <input id="txtAccount" name="txtAccount" type="text" value="" maxlength="50" style="width: 150px;" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divHiddenFields" style="display: none">
        <input id="hdnCounter" type="text" value="0" />
        <asp:HiddenField ID="hdnAssetsTypes" runat="server" />
        <asp:HiddenField ID="hdnAmounts" runat="server" />
        <asp:HiddenField ID="hdnNames" runat="server" />
        <asp:HiddenField ID="hdnAccountNums" runat="server" />
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