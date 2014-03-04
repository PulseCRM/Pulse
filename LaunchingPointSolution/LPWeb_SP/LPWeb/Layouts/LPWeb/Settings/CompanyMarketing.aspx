<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Settings - Company Marketings" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" AutoEventWireup="true" CodeBehind="CompanyMarketing.aspx.cs" Inherits="Settings_CompanyMarketing" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            DrawTab();

            $("#aspnetForm").validate();
        });

        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :checkbox").attr("checked", "");
            }
        }

        //#region Add/Remove Campaigns

        function aAdd_onclick() {

            var TrCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr").length;

            // add th
            if (TrCount == 1) {

                // clear tr
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList").empty();

                // add th
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList").append($("#gridCampaignList1 tr").eq(0).clone());
            }

            //#region Add Tr

            // next index
            var NowIndex = new Number($("#hdnCounter").val());
            var NextIndex = NowIndex + 1;

            // clone tr
            var TrCopy = $("#gridCampaignList1 tr").eq(1).clone(true);

            //alert(TrCopy.html());

            // ddlCategory
            var ddlCategory_NewID = "ddlCategory" + NextIndex;
            var ddlCategory_Code = "<select id='" + ddlCategory_NewID + "' name='" + ddlCategory_NewID + "' style='width: 180px;' onclick='ShowDialog_SelectCampaign(this)'><option value=''></option></select>"
            // replace
            TrCopy.find("#ddlCategory").replaceWith(ddlCategory_Code);

            // ddlCampaign
            var ddlCampaign_NewID = "ddlCampaign" + NextIndex;
            var ddlCampaign_Code = "<select id='" + ddlCampaign_NewID + "' name='" + ddlCampaign_NewID + "' style='width: 335px;' onclick='ShowDialog_SelectCampaign(this)'><option value=''></option></select>"
            // replace
            TrCopy.find("#ddlCampaign").replaceWith(ddlCampaign_Code);

            // ddlRule
            var ddlRule_NewID = "ddlRule" + NextIndex;
            var ddlRule_Code = "<select id='" + ddlRule_NewID + "' name='" + ddlRule_NewID + "' style='width: 320px;' onclick='ShowDialog_SelectRule(this)'><option value=''></option></select>"
            // replace
            TrCopy.find("#ddlRule").replaceWith(ddlRule_Code);

            //alert(TrCopy.html());

            // add tr
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList").append(TrCopy);

            // set counter
            $("#hdnCounter").val(NextIndex);

            // add validate
            $("#" + ddlCategory_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please select Category.</div>"
                }
            });

            $("#" + ddlCampaign_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please select Marketing Campaign.</div>"
                }
            });

            $("#" + ddlRule_NewID).rules("add", {
                required: true,
                messages: {
                    required: "<div>Please select Rule.</div>"
                }
            });


            //#endregion
        }

        function aRemove_onclick() {

            var SelectedCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr:not(:first) td :checkbox[id='chkSelected']:checked").length;
            //alert(SelectedCount);
            if (SelectedCount == 0) {

                alert("No campaign was selected.");
                return;
            }

            var Result = confirm("Are you sure you want to continue?");
            if (Result == false) {

                return;
            }

            // remove row
            if ($("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr:not(:first) td :checkbox[id='chkSelected']:checked").length == $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr:not(:first)").length) {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList").empty();
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList").append("<tr class='EmptyDataRow' align='center'><td colspan='2'>There is no auto campaign.</td></tr>");
            }
            else {

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr:not(:first) td :checkbox[id='chkSelected']:checked").parent().parent().remove();
            }

        }

        //#endregion

        function BeforeSave() {

            // call validate
            var IsValid = $("#aspnetForm").valid();
            if (IsValid == false) {

                return false;
            }

            //#region check campaign id duplicated

            var bDuplicated = false;
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlCampaign']").each(function (i) {

                var ThisCampaignID = $(this).val();

                for (var j = i + 1; j < $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlCampaign']").length; j++) {

                    var OtherCampaignID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlCampaign']").eq(j).val();

                    if (ThisCampaignID == OtherCampaignID) {

                        bDuplicated = true;
                        break;
                    }
                }
            });

            if (bDuplicated == true) {

                alert("The auto campaign id can not be duplicated.");
                return false;
            }

            //#endregion

            //#region check rule id duplicated

            bDuplicated == false;

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlRule']").each(function (i) {

                var ThisRuleID = $(this).val();

                for (var j = i + 1; j < $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlRule']").length; j++) {

                    var OtherRuleID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlRule']").eq(j).val();

                    if (ThisRuleID == OtherRuleID) {

                        bDuplicated = true;
                        break;
                    }
                }
            });

            if (bDuplicated == true) {

                alert("The rule id can not be duplicated.");
                return false;
            }

            //#endregion

            //#region build campaign list data string

            //#region CategoryIDs

            var CategoryIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id='ddlCategory']").each(function (i) {

                var ThisCategoryID = $(this).val();

                if (CategoryIDs == "") {

                    CategoryIDs = ThisCategoryID
                }
                else {

                    CategoryIDs += "," + ThisCategoryID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnCategoryIDs").val(CategoryIDs);

            //#endregion

            //#region CampaignIDs

            var CampaignIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlCampaign']").each(function (i) {

                var ThisCampaignID = $(this).val();

                if (CampaignIDs == "") {

                    CampaignIDs = ThisCampaignID
                }
                else {

                    CampaignIDs += "," + ThisCampaignID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnCampaignIDs").val(CampaignIDs);

            //#endregion

            //#region RuleIDs

            var RuleIDs = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id^='ddlRule']").each(function (i) {

                var ThisRuleID = $(this).val();

                if (RuleIDs == "") {

                    RuleIDs = ThisRuleID
                }
                else {

                    RuleIDs += "," + ThisRuleID;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnRuleIDs").val(RuleIDs);

            //#endregion

            //#region Enabled

            var EnableStatuses = "";
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridCampaignList tr td :input[id='chkEnabled']").each(function (i) {

                var Enabled = $(this).attr("checked");
                //alert(Enabled);
                if (Enabled == "") {

                    Enabled = "false";
                }

                if (EnableStatuses == "") {

                    EnableStatuses = Enabled
                }
                else {

                    EnableStatuses += "," + Enabled;
                }
            });

            $("#ctl00_ctl00_PlaceHolderMain_MainArea_hdnEnableStatuses").val(EnableStatuses);
            //alert($("#hdnEnableStatuses").val());

            //#endregion

            //#endregion

            return true;
        }

        //#region Select Rule

        var SelRuleDropdownList;
        function ShowDialog_SelectRule(dropdownlist) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "RuleTemplateSelection.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            ShowGlobalPopup("Select Rule", 410, 500, 435, 540, iFrameSrc);

            SelRuleDropdownList = dropdownlist;
        }

        function SetRuleIDAndName(RuleID, RuleName) {

            var RuleNameDecode = $.base64Decode(RuleName);

            $(SelRuleDropdownList).children().attr("value", RuleID);
            $(SelRuleDropdownList).children().text(RuleNameDecode);
        }

        //#endregion

        //#region Select Campaign

        var SelCategoryDropdownList;
        var SelCampaignDropdownList;
        function ShowDialog_SelectCampaign(dropdownlist) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "MarketingCampaignSelection.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            ShowGlobalPopup("Select Marketing Campaign", 510, 550, 535, 590, iFrameSrc);

            if ($(dropdownlist).attr("id").indexOf("ddlCategory") != -1) {

                SelCategoryDropdownList = dropdownlist;
                SelCampaignDropdownList = $(dropdownlist).parent().parent().find(":input[id^='ddlCampaign']").get(0);

            }
            else {

                SelCategoryDropdownList = $(dropdownlist).parent().parent().find(":input[id^='ddlCategory']").get(0);
                SelCampaignDropdownList = dropdownlist;

            }
        }

        function SetCampaignInfo(CategoryID, CategoryName, CampaignID, CampaignName) {

            var CategoryNameDecode = $.base64Decode(CategoryName);
            var CampaignNameDecode = $.base64Decode(CampaignName);

            $(SelCategoryDropdownList).children().attr("value", CategoryID);
            $(SelCategoryDropdownList).children().text(CategoryNameDecode);

            $(SelCampaignDropdownList).children().attr("value", CampaignID);
            $(SelCampaignDropdownList).children().text(CampaignNameDecode);
        }

        //#endregion

// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divContainer">
        <div id="divModuleName" class="Heading">Company Setup - Marketing</div>
        <div style="margin-top: 15px;">
            <div class="JTab">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 10px;">&nbsp;</td>
                        <td>
                            <div id="tabs10">
                                <ul>
                                    <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                    <li><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                    <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                    <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                    <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>

                                     <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
                                 <%--<li id="current"><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>



                                 
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
                <div id="TabBody">
                    <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
                    <div id="TabLine2" class="TabRightLine">&nbsp;</div>
                    <div class="TabContent">
                        
                        <table>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkEnableMarketing" runat="server" />
                                </td>
                                <td>Enable marketing features</td>
                                <td style="padding-left: 10px;">
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave();" onclick="btnSave_Click" />
                                </td>
                                <td style="padding-left: 10px;">
                                    <asp:Button ID="btnSync" runat="server" Text="Sync Marketing Info Now" CssClass="Btn-140" OnClientClick="return BeforeSave();" onclick="btnSync_Click" />
                                </td>
                            </tr>
                        </table>

                        <div class="ModuleTitle" style="margin-top: 20px;">Company-sponsored auto campaigns:</div>
                        
                        <div id="divToolBar" style="margin-top: 10px;">
                            <ul class="ToolStrip" style="margin-left: 0px;">
                                <li><a id="aAdd" href="javascript:aAdd_onclick()">Add</a><span>|</span></li>
                                <li><a id="aRemove" href="javascript:aRemove_onclick()">Remove</a></li>
                            </ul>
                        </div>
                        <div id="divCampaignList" class="ColorGrid" style="margin-top: 5px; width: 1000px;">
                            <asp:GridView ID="gridCampaignList" runat="server" EmptyDataText="There is no auto campaign." AutoGenerateColumns="False" CellPadding="4" CssClass="GrayGrid" GridLines="None">
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
                                    <asp:TemplateField HeaderText="Category" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <select id="ddlCategory" style="width: 180px;" onclick="ShowDialog_SelectCampaign(this)">
                                                <option value="<%# Eval("CategoryId") %>"><%# Eval("CategoryName")%></option>
                                            </select>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Marketing Campaign">
                                        <ItemTemplate>
                                            <select id="ddlCampaign" style="width: 335px;" onclick="ShowDialog_SelectCampaign(this)">
                                                <option value="<%# Eval("CampaignId") %>"><%# Eval("CampaignName")%></option>
                                            </select>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rule" ItemStyle-Width="340px">
                                        <ItemTemplate>
                                            <select id="ddlRule" style="width: 320px;" onclick="ShowDialog_SelectRule(this)">
                                                <option value="<%# Eval("RuleId") %>"><%# Eval("Name")%></option>
                                            </select>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                        <HeaderTemplate>
                                            Enabled
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <input id="chkEnabled" type="checkbox" <%# Eval("AutoCampaignEnabled").ToString() == "True" ? "checked" : "" %> />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                        <div id="divCampaignList1" class="ColorGrid" style="margin-top: 5px; width: 1000px; display:none;">
                            <div>
                                <table class="GrayGrid" cellspacing="0" cellpadding="4" id="gridCampaignList1" style="border-collapse: collapse;">
                                    <tr>
                                        <th class="CheckBoxHeader" scope="col"><input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" /></th>
                                        <th scope="col">Category</th>
                                        <th scope="col">Marketing Campaign</th>
                                        <th scope="col">Rule</th>
                                        <th class="CheckBoxHeader" scope="col">Enabled</th>
                                    </tr>
                                    <tr>
                                        <td class="CheckBoxColumn">
                                            <input id="chkSelected" type="checkbox" />
                                        </td>
                                        <td style="width: 200px;">
                                            <select id="ddlCategory" style="width: 180px;" onclick="ShowDialog_SelectCampaign(this)">
                                                <option value=""></option>
                                            </select>
                                        </td>
                                        <td>
                                            <select id="ddlCampaign" style="width: 335px;" onclick="ShowDialog_SelectCampaign(this)">
                                                <option value=""></option>
                                            </select>
                                        </td>
                                        <td style="width: 340px;">
                                            <select id="ddlRule" style="width: 320px;" onclick="ShowDialog_SelectRule(this)">
                                                <option value=""></option>
                                            </select>
                                        </td>
                                        <td class="CheckBoxColumn">
                                            <input id="chkEnabled" type="checkbox" checked="True" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="GridPaddingBottom">&nbsp;</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="divHiddenFields" style="display: none">
        <input id="hdnCounter" type="text" value="0" />
        <asp:HiddenField ID="hdnCategoryIDs" runat="server" />
        <asp:HiddenField ID="hdnCampaignIDs" runat="server" />
        <asp:HiddenField ID="hdnRuleIDs" runat="server" />
        <asp:HiddenField ID="hdnEnableStatuses" runat="server" />
    </div>
</asp:Content>
