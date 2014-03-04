<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Title="User Production Goals Report" Language="C#" AutoEventWireup="true" CodeBehind="ReportUserProduction.aspx.cs" Inherits="LPWeb.Layouts.LPWeb.Reports.ReportUserProduction" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        // <!CDATA[

        $(document).ready(function () {

            InitFilter();

            // add change event
            $("#ddlAlphabet").change(ddlAlphabet_onchange);

            // set parent iframe height
            var h = document.body.scrollHeight + 10;
            if (h < 200)
                h = 200;
            $("#ifReportList", window.parent.document).height(h);
        });

        function InitFilter() {

            // Alphabet
            var Alphabet = GetQueryString1("Alphabet");
            $("#ddlAlphabet").val(Alphabet);
        }

        function ddlAlphabet_onchange() {

            var QueryString = "";

            // sid
            var sid = GetQueryString1("sid");
            if (sid != "") {

                QueryString += "?sid=" + sid;
            }

            // Region
            var Region = GetQueryString1("Region");
            if (Region != "") {

                QueryString += "&Region=" + Region;
            }

            // Division
            var Division = GetQueryString1("Division");
            if (Division != "") {

                QueryString += "&Division=" + Division;
            }

            // Branch
            var Branch = GetQueryString1("Branch");
            if (Branch != "") {

                QueryString += "&Branch=" + Branch;
            }

            // Alphabet
            var Alphabet = $("#ddlAlphabet").val();
            if (Alphabet != "") {

                QueryString += "&Alphabet=" + Alphabet;
            }


            window.location.href = window.location.pathname + QueryString;
        }

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#divReportGrid tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#divReportGrid tr td :checkbox").attr("checked", "");
            }
        }

        // show popup for set goals
        function ShowDialog_SetGoals() {

            var SelectedCount = $("#divReportGrid tr td :checkbox:checked").length;
            if (SelectedCount == 0) {

                alert("No record has been selected.");
                return;
            }

            var UserIDs = "";
            $("#divReportGrid tr td :checkbox:checked").each(function (i) {

                var UserID = $(this).attr("myUserID");

                if (UserIDs == "") {

                    UserIDs = UserID;
                }
                else {

                    UserIDs += "," + UserID;
                }
            });

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            $("#ifrSetGoals").attr("src", "../Settings/UserGoalsSetup.aspx?t=" + RadomStr + "&ids=" + UserIDs);

            // show modal
            $("#divSetGoals").dialog({
                height: 480,
                width: 845,
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseDialog_EditEmailTemplate() {

            $("#divSetGoals").dialog("close");
        }
        // ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="ReportContainer" style="border: solid 0px red; height: 500px;">
        <div id="divToolBar" style="margin-top: 10px; width: 1100px;">
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 40px;">
                        <asp:DropDownList ID="ddlAlphabet" runat="server">
                            <asp:ListItem Value=""></asp:ListItem>
                            <asp:ListItem Value="A">A</asp:ListItem>
                            <asp:ListItem Value="B">B</asp:ListItem>
                            <asp:ListItem Value="C">C</asp:ListItem>
                            <asp:ListItem Value="D">D</asp:ListItem>
                            <asp:ListItem Value="E">E</asp:ListItem>
                            <asp:ListItem Value="F">F</asp:ListItem>
                            <asp:ListItem Value="G">G</asp:ListItem>
                            <asp:ListItem Value="H">H</asp:ListItem>
                            <asp:ListItem Value="I">I</asp:ListItem>
                            <asp:ListItem Value="J">J</asp:ListItem>
                            <asp:ListItem Value="K">K</asp:ListItem>
                            <asp:ListItem Value="L">L</asp:ListItem>
                            <asp:ListItem Value="M">M</asp:ListItem>
                            <asp:ListItem Value="N">N</asp:ListItem>
                            <asp:ListItem Value="O">O</asp:ListItem>
                            <asp:ListItem Value="P">P</asp:ListItem>
                            <asp:ListItem Value="Q">Q</asp:ListItem>
                            <asp:ListItem Value="R">R</asp:ListItem>
                            <asp:ListItem Value="S">S</asp:ListItem>
                            <asp:ListItem Value="T">T</asp:ListItem>
                            <asp:ListItem Value="U">U</asp:ListItem>
                            <asp:ListItem Value="V">V</asp:ListItem>
                            <asp:ListItem Value="W">W</asp:ListItem>
                            <asp:ListItem Value="X">X</asp:ListItem>
                            <asp:ListItem Value="Y">Y</asp:ListItem>
                            <asp:ListItem Value="Z">Z</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <ul class="ToolStrip" style="width: 300px;">
                            <li><a id="aSetGoals" href="javascript:ShowDialog_SetGoals()">Set Goals</a><span>|</span></li>
                            <li><asp:LinkButton ID="lbtnExport" runat="server" OnClick="btnExport_Click" Text="Export"></asp:LinkButton></li>
                        </ul>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager" UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divReportGrid" class="ColorGrid" style="margin-top: 5px; width: 1100px;">
            <asp:GridView ID="gridReportGrid" runat="server" DataSourceID="ReportSqlDataSource" DataKeyNames="UserId" EmptyDataText="There is no data for user production goals report." AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="Checkbox1" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="Checkbox2" type="checkbox" myUserID="<%# Eval("UserId") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="BranchName" HeaderText="Branch" />
                    <asp:BoundField DataField="FullName" HeaderText="Name" />
                    <asp:TemplateField HeaderText="Progress" ItemStyle-Width="150px">
                        <ItemTemplate>
                            <div style="margin-left: 10px; background: url('../images/ProgressBarBG.gif') no-repeat; width: 142; height: 20px;">
                                <div style="background: url('../images/Progress.gif') repeat-x; width: <%# this.GetProgressBarWidth(Eval("Progress").ToString()) %>px; height: 20px;"><div style="position: relative; left: <%# this.GetPercentPosition(Eval("Progress").ToString()) %>px; top: 1px; width: 50px;"><%# Eval("Progress") %>%</div></div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RunningTotal" HeaderText="Running Total" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="LowRange" HeaderText="Low Range" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="MediumRange" HeaderText="Medium Range" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" />
                    <asp:BoundField DataField="HighRange" HeaderText="High Range" DataFormatString="${0:N0}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="120px" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>

        <asp:SqlDataSource ID="ReportSqlDataSource" runat="server"
            SelectCommand="lpsp_ExecSqlByPager" SelectCommandType="StoredProcedure" DataSourceMode="DataReader">
            <SelectParameters>
                <asp:Parameter Name="OrderByField" Type="String" DefaultValue="LastName" />
                <asp:Parameter Name="AscOrDesc" Type="String" DefaultValue="asc" />
                <asp:Parameter Name="Fields" Type="String" DefaultValue="*" />
                <asp:Parameter Name="DbTable" Type="String" DefaultValue="" />
                <asp:Parameter Name="Where" Type="String" DefaultValue="" ConvertEmptyStringToNull="False" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="1" Name="StartIndex" PropertyName="StartRecordIndex" Type="Int32" />
                <asp:ControlParameter ControlID="AspNetPager1" DefaultValue="20" Name="EndIndex" PropertyName="EndRecordIndex" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    <div id="divSetGoals" title="User Goals Setup" style="display: none;">
        <iframe id="ifrSetGoals" frameborder="0" scrolling="no" width="820px" height="440px"></iframe>
    </div>
    </form>
</body>
</html>