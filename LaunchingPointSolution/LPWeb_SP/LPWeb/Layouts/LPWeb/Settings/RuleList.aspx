<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" AutoEventWireup="true"
    Inherits="LPWeb.Settings.Settings_RuleList" CodeBehind="RuleList.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        var gridRuleClientId = "#<%= gridRuleList.ClientID %>";
        var hdnRuleIDsClientId = "#<%= hdnRuleIDs.ClientID %>";
        var hdnAlertRuleTempl = "#<%= hdnAlertRuleTempl.ClientID %>";

        $(document).ready(function () {
            if ($(hdnAlertRuleTempl).val().indexOf('1') == -1) {
                $("#aCreate").removeAttr('href');
            }
            if ($(hdnAlertRuleTempl).val().indexOf('2') == -1) {
                $("#aUpdate").removeAttr('href');
            }
            // Others Control in View Code
            InitSearchInput();
        });
        // check/decheck all
        function CheckAll(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function CheckSelected() {

            if ($(gridRuleClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }
            else {
                BeforeSave();
                return confirm('Are you sure you want to continue?');
            }
        }

        function InitSearchInput() {

            // Alphabet
            var Alphabet = GetQueryString1("Alphabet");
            if (Alphabet != "") {
                $("#ddlAlphabet").val(Alphabet);
            }
        }

        function BeforeSave() {

            var TmpIDs = "";
            $(gridRuleClientId + " tr td :checkbox[checked=true]").each(function (i) {

                var RuleTemplId = $(this).attr("tag");
                if (i == 0) {
                    TmpIDs = RuleTemplId;
                }
                else {
                    TmpIDs += "," + RuleTemplId;
                }
            });

            $(hdnRuleIDsClientId).val(TmpIDs);

            return true;
        }



        function ViewEmail() {

            alert("View Email");

        }

        function ddlAlphabet_onchange() {

            var sQueryStrings = "";

            // Alphabet
            var Alphabet = $.trim($("#ddlAlphabet").val());
            if (Alphabet != "") {
                sQueryStrings += "&Alphabet=" + encodeURIComponent(Alphabet);
            }

            if (sQueryStrings != "") {

                var sPageIndex = GetQueryString1("PageIndex");

                if (sPageIndex == "") {
                    sQueryStrings += "&PageIndex=1";
                }
                else {
                    sQueryStrings += "&PageIndex=" + sPageIndex;
                }

                var RadomNum = Math.random();
                var RadomStr = RadomNum.toString().substr(2);
                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings;
            }
            else {

                window.location.href = window.location.pathname
            }
        }


        //#region Create/Update Rule neo

        // show popup for add email template
        function ShowDialog_AddRule() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrAdd").attr("src", "RuleAdd.aspx?sid=" + RadomStr);

            // show modal
            $("#divAdd").dialog({
                height: 740,
                width: 830,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_AddRule() {

            $("#divAdd").dialog("close");
        }

        function aUpdate_onclick() {

            var SelectedCount = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No rule has been selected.");
                return;
            }
            else if (SelectedCount > 1) {

                alert("Only one rule can be selected.");
                return;
            }

            var RuleID = $("#ctl00_ctl00_PlaceHolderMain_MainArea_gridRuleList tr:not(:first) td :checkbox:checked").attr("tag");

            ShowDialog_EditRule(RuleID);
        }

        // show popup for add email template
        function ShowDialog_EditRule(RuleID) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrRuleEdit").attr("src", "RuleEdit.aspx?sid=" + RadomStr + "&RuleID=" + RuleID);

            // show modal
            $("#divEdit").dialog({
                height: 740,
                width: 830,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_EditRule() {

            $("#divEdit").dialog("close");
        }

        function UpdateEmailTemplate(EmailTemplateID) {

            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);

            window.location.href = "EmailTemplateEditParent.aspx?sid=" + sid + "&EmailTemplateID=" + EmailTemplateID;

        }

        //#region Show Dialog

        function ShowDialog(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divEmailTemplateSetup").attr("title", Title);

            $("#ifrEmailTemplateSetup").attr("src", "");
            $("#ifrEmailTemplateSetup").attr("src", iFrameSrc);
            $("#ifrEmailTemplateSetup").width(iFrameWidth);
            $("#ifrEmailTemplateSetup").height(iFrameHeight);

            // show modal
            $("#divEmailTemplateSetup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) { $("#divEmailTemplateSetup").dialog("destroy"); }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog() {

            $("#divEmailTemplateSetup").dialog("close");
        }

        //#endregion

        //#endregion
// ]]>
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="ModuleTitle" style="padding-left: 10px">Rule List</div>
    <div class="SplitLine"></div>
    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Rule Scope:&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlRuleScope" runat="server" >
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Company" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Loan" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width:30px"></td>
                    <td>
                        Rule Target:&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlRuleTarget" runat="server">
                            <asp:ListItem Text="All" Value=""></asp:ListItem>
                            <asp:ListItem Text="Processing" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Prospect" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Processing and Prospect" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 770px;">
                <tr>
                    <td style="width: 40px;">
                        <select id="ddlAlphabet" onchange="return ddlAlphabet_onchange();">
                            <option></option>
                            <option>A</option>
                            <option>B</option>
                            <option>C</option>
                            <option>D</option>
                            <option>E</option>
                            <option>F</option>
                            <option>G</option>
                            <option>H</option>
                            <option>I</option>
                            <option>J</option>
                            <option>K</option>
                            <option>L</option>
                            <option>M</option>
                            <option>N</option>
                            <option>O</option>
                            <option>P</option>
                            <option>Q</option>
                            <option>R</option>
                            <option>S</option>
                            <option>T</option>
                            <option>U</option>
                            <option>V</option>
                            <option>W</option>
                            <option>X</option>
                            <option>Y</option>
                            <option>Z</option>
                        </select>
                    </td>
                    <td style="width: 300px;">
                        <ul class="ToolStrip">
                            <li id="liCreate"><a id="aCreate" href="javascript:ShowDialog_AddRule()">Create</a><span>|</span></li>
                            <li id="liDisable">
                                <asp:LinkButton ID="btnDisable" runat="server" OnClientClick="return CheckSelected(); "
                                    Text="Disable" OnClick="btnDisable_Click"></asp:LinkButton><span>|</span>
                            </li>
                            <li id="liEnable">
                                <asp:LinkButton ID="btnEnable" runat="server" OnClientClick="return CheckSelected(); "
                                    Text="Enable" OnClick="btnEnable_Click"></asp:LinkButton><span>|</span>
                            </li>
                            <li id="liDelete">
                                <asp:LinkButton ID="btnDelete" runat="server" Text="Delete" OnClientClick="return CheckSelected();"
                                    OnClick="btnDelete_Click"></asp:LinkButton><span>|</span> </li>
                            <li id="liUpdate"><a id="aUpdate" href="javascript:aUpdate_onclick()">Update</a></li>
                        </ul>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                            NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                            CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divRuleList" class="ColorGrid" style="width: 770px; margin-top: 5px;">
            <asp:GridView ID="gridRuleList" runat="server" DataSourceID="RuleSqlDataSource"
                DataKeyNames="RuleId" EmptyDataText="There is no rule data in database." OnSorting="gridRuleList_Sorting" 
                AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None" AllowSorting="true">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" tag='<%# Eval("RuleId") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rule" HeaderStyle-Width="260" SortExpression="Name">
                        <ItemTemplate>
                            <a href="javascript:ShowDialog_EditRule('<%# Eval("RuleId") %>')" class="RuleTemplSetup" tag='<%# Eval("RuleId") %>'><%# Eval("Name")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Alert Email Template" HeaderStyle-Width="300" SortExpression="EmailTemplateName">
                        <ItemTemplate>
                            <a href="javascript:UpdateEmailTemplate('<%# Eval("AlertEmailTemplId") %>')"><%# Eval("EmailTemplateName") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Scope" HeaderStyle-Width="60" SortExpression="ScopeName">
                        <ItemTemplate>
                            <asp:Label ID="lbScope" runat="server" Text='<%# Eval("ScopeName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Target" HeaderStyle-Width="150" SortExpression="TargetName">
                        <ItemTemplate>
                            <asp:Label ID="lbTarget" runat="server" Text='<%# Eval("TargetName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Enabled" HeaderStyle-Width="55" SortExpression="Enabled">
                        <ItemTemplate>
                            <asp:Label ID="lbEnabled" runat="server" Text='<%# Eval("Enabled") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="RuleSqlDataSource" runat="server"
                SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataSet">
                <SelectParameters>
                    <asp:Parameter Name="OrderByField" Type="String" DefaultValue="RuleId" />
                    <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                    <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                    <asp:Parameter Name="DbTable" Type="String" DefaultValue="(SELECT Template_Rules.*,Template_Email.Name AS EmailTemplateName, CASE RuleScope WHEN 0 THEN 'Loan' WHEN 1 THEN 'Company' WHEN 2 THEN 'Region' WHEN 3 THEN 'Division' WHEN 4 THEN 'Branch' ELSE '' END AS ScopeName, (SELECT STUFF((SELECT ', ' + LoanTargets.LT FROM (SELECT CASE Template_Rules.LoanTarget WHEN 0 THEN 'Active Loans' END AS LT UNION SELECT CASE Template_Rules.LoanTarget WHEN 1 THEN 'Active Leads' END AS LT UNION SELECT CASE Template_Rules.LoanTarget WHEN 2 THEN 'Active Loans, Active Leads' END AS LT UNION SELECT CASE WHEN Template_Rules.LoanTarget & 17 = 17 THEN 'Active Loans' END AS LT UNION SELECT CASE WHEN Template_Rules.LoanTarget & 18 = 18 THEN 'Active Leads' END AS LT UNION SELECT CASE WHEN Template_Rules.LoanTarget & 20 = 20 THEN 'Archived Loans' END AS LT UNION SELECT CASE WHEN Template_Rules.LoanTarget & 24 = 24 THEN 'Archived Leads' END AS LT) AS LoanTargets FOR XML PATH('')), 1, 2, '')) AS TargetName FROM Template_Rules LEFT OUTER JOIN Template_Email ON Template_Rules.AlertEmailTemplId=Template_Email.TemplEmailId) t" />
                    <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                    <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex"
                        PropertyName="StartRecordIndex" Type="Int32" />
                    <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="10" Name="EndIndex"
                        PropertyName="EndRecordIndex" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </div>
    <asp:HiddenField ID="hdnRuleIDs" runat="server" />
    <div id="divAdd" title="Rule Setup" style="display: none;">
        <iframe id="ifrAdd" frameborder="0" scrolling="no" width="807px" height="700px"></iframe>
    </div>
    <div id="divEdit" title="Rule Setup" style="display: none;">
        <iframe id="ifrRuleEdit" frameborder="0" scrolling="no" width="807px" height="700px"></iframe>
    </div>
    <div id="divEmailTemplateSetup" title="Email Template Setup" style="display: none;">
        <iframe id="ifrEmailTemplateSetup" frameborder="0" scrolling="no" width="820px" height="740px"></iframe>
    </div>
     <asp:HiddenField ID="hdnAlertRuleTempl" runat="server" />
</asp:Content>
