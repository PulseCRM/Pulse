<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Activity History List" Language="C#" AutoEventWireup="true" Inherits="LPWeb.Layouts.LPWeb.LoanDetails.Activity"
    CodeBehind="Activity.aspx.cs" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Activity History List</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />

    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>

    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        $(document).ready(function () {

            DrawTab();

            var startDate = $("#" + '<%=tbStartDate.ClientID %>');
            var endDate = $("#" + '<%=tbEndDate.ClientID %>');
            startDate.datepick();
            endDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");
        });

        //#region CR48 fake tab navigation

        function aTasks_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsTask.aspx?sid=" + sid + "&LoanID=" + LoanID + "&ref=" + sRef;
        }

        function aAlerts_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanDetailsAlertTab.aspx?sid=" + sid + "&ref=loan&LoanID=" + LoanID + "&ref=" + sRef;
        }

        function aEmails_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../Prospect/LoanDetailEmailTab.aspx?from=2&itemid=" + LoanID + "&sid=" + sid + "&ref=" + sRef;

        }

        function aNotes_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/LoanNoteList.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }

        function aActivityHistory_onclick() {

            var LoanID = GetQueryString1("FileID");
            var RadomNum = Math.random();
            var sid = RadomNum.toString().substr(2);
            var sRef = GetQueryString1("ref");

            window.location.href = "../LoanDetails/Activity.aspx?sid=" + sid + "&FileID=" + LoanID + "&ref=" + sRef;
        }


        //#endregion

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <div class="JTab" style="margin-top:10px;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <%
                            bool isOnlytab = false;

                            //string rUrl = Request.UrlReferrer.AbsoluteUri;

                            //if (rUrl.Contains("LoanDetail.aspx"))
                            //{
                            //    isOnlytab = true;
                            //}
                            if (this.Request.QueryString["ref"] != null && this.Request.QueryString["ref"].ToString() == "loan")
                            {
                                isOnlytab = true;
                            }
                         %>
                        <ul>
                            <%if (!isOnlytab)
                              { %>
                            <li><a id="aTasks" href="javascript:aTasks_onclick()"><span>Tasks</span></a></li>
                            <li><a id="aAlerts" href="javascript:aAlerts_onclick()"><span>Rule Alerts</span></a></li>
                            <li><a id="aEmails" href="javascript:aEmails_onclick()"><span>Emails</span></a></li>
                            <li><a id="aNotes" href="javascript:aNotes_onclick()"><span>Notes</span></a></li>
                            <%} %>
                            <li id="current"><a id="aActivityHistory" href="javascript:aActivityHistory_onclick()"><span>Activity History</span></a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
        <div id="TabBody" style="margin-bottom:10px; padding-bottom:10px;">
            <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
            <div id="TabLine2" class="TabRightLine">&nbsp;</div>
            <div class="TabContent">

    <div style="padding-left: 10px; padding-right: 10px;">
        <div id="divFilter" style="margin-top: 10px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        Start Date:
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="tbStartDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 39px;">
                        End Date:
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:TextBox ID="tbEndDate" runat="server" CssClass="DateField"></asp:TextBox>
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="Btn-66" OnClick="btnFilter_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="divToolBar" style="margin-top: 13px;">
            <table cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td>
                        Performed By: <span style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlPerformedBy" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPerformedBy_SelectedIndexChanged">
                                <asp:ListItem Value="All" Text="All"></asp:ListItem>
                            </asp:DropDownList>
                        </span>
                    </td>
                    <td style="text-align: right;">
                        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20" CssClass="AspNetPager"
                            OnPageChanged="AspNetPager1_PageChanged" UrlPageIndexName="PageIndex" UrlPaging="false"
                            FirstPageText="<<" LastPageText=">>" NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never"
                            ShowPageIndexBox="Never" CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton"
                            LayoutType="Table">
                        </webdiyer:AspNetPager>
                    </td>
                </tr>
            </table>
        </div>
        <div id="div2" class="ColorGrid" style="margin-top: 5px;">
            <asp:GridView ID="gridList" runat="server" DataKeyNames="ActivityId" EmptyDataText="There is no data in database."
                AutoGenerateColumns="False" CellPadding="3" CssClass="GrayGrid" GridLines="None"
                AllowSorting="true" OnSorting="gridList_Sorting" OnRowDataBound="gridList_DataBound">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:BoundField DataField="ActivityTime" SortExpression="ActivityTime" HeaderText="Date Time" />
                    <asp:TemplateField SortExpression="ActivityName" HeaderText="Activity Name">
                      <ItemTemplate>
                      <asp:LinkButton ID="lbtnActivity" runat="server" Text='<%# Server.HtmlEncode(Eval("ActivityName").ToString()) %>'> </asp:LinkButton>
                      </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PerformedBy" SortExpression="PerformedBy" HeaderText="Performed By" />
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">
                &nbsp;</div>
        </div>
    </div>

            </div>
        </div>
    </div>

    </form>
</body>
</html>
