<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LinkLoanDetails.aspx.cs" Inherits="LPWeb.Prospect.LinkLoanDetails" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Link Loan</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>

    <style>

        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
    <script language="javascript" type="text/javascript">
// <![CDATA[
        var gvLinkLoanListClientId = "#<%= gvLinkLoanList.ClientID %>";
        var hdLoanIdsClientId = "#<%= hdLoanIds.ClientID %>";
        var hdProspectIDClientId = "#<%= hdProspectID.ClientID %>";
        var hdLinkTypeClientId = "#<%= hdLinkType.ClientID %>";

        $(document).ready(function () {

            $("#" + '<%=btnLink.ClientID %>').contextMenu("divMenuLink", {
                menuStyle: {
                    listStyle: 'none',
                    padding: '1px',
                    margin: '0px',
                    backgroundColor: '#fff',
                    border: '1px solid #999',
                    width: '150px'
                },
                itemStyle: {
                    margin: '0px',
                    color: '#000',
                    display: 'block',
                    cursor: 'default',
                    padding: '3px',
                    border: '1px solid #fff',
                    backgroundColor: 'transparent'
                },
                itemHoverStyle: {
                    border: '1px solid #0a246a',
                    backgroundColor: '#b6bdd2'
                },
                bindings: {
                    Borrower: LinkBorrower,
                    Coborrower: LinkCoborrower
                },
                onContextMenu: function (e) {
                    return true;
                },
                onShowMenu: function (e, menu) {

                    return menu;
                }
            });

        });

        
        function LinkBorrower()
        {

            BeforeLink();

            if ($(gvLinkLoanListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }

            $(hdLinkTypeClientId).val("borrower");
            
            <%=this.ClientScript.GetPostBackEventReference(this.btnLink, null) %>
        }

        function LinkCoborrower()
        {

            BeforeLink();

            if ($(gvLinkLoanListClientId + " tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return false;
            }

            $(hdLinkTypeClientId).val("coborrower");

            <%=this.ClientScript.GetPostBackEventReference(this.btnLink, null) %>
        }


        function BeforeLink() {

            var TmpIDs = "";
            $(gvLinkLoanListClientId + " tr td :checkbox[checked=true]").each(function (i) {

                var LoanId = $(this).attr("tag");
                if (i == 0) {
                    TmpIDs = LoanId;
                }
                else {
                    TmpIDs += "," + LoanId;
                }
            });

            $(hdLoanIdsClientId).val(TmpIDs);

            return true;
        }


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


        function BuildQueryString1() {

            var sQueryStrings = "";

            // FirstName
            var FirstName = $.trim($("#<%= this.tbxBrwFirstName.ClientID %>").val());
            if (FirstName != "") {
                sQueryStrings += "&FirstName=" + encodeURIComponent(FirstName);
            }
            // LastName
            var LastName = $.trim($("#<%= this.tbxBrwLastName.ClientID %>").val());
            if (LastName != "") {
                sQueryStrings += "&LastName=" + encodeURIComponent(LastName);
            }
            // LoanStatus
            var LoanStatus = $.trim($("#<%= this.ddlLoanStatus.ClientID %>").val());
            if (LoanStatus != "" && LoanStatus != "All") {
                sQueryStrings += "&LoanStatus=" + encodeURIComponent(LoanStatus);
            }
            // PropertyAddr
            var PropertyAddr = $.trim($("#<%= this.tbxAddress.ClientID %>").val());
            if (PropertyAddr != "") {
                sQueryStrings += "&PropertyAddr=" + encodeURIComponent(PropertyAddr);
            }
            // PropertyCity
            var PropertyCity = $.trim($("#<%= this.tbxCity.ClientID %>").val());
            if (PropertyCity != "") {
                sQueryStrings += "&PropertyCity=" + encodeURIComponent(PropertyCity);
            }
            // PropertyState
            var PropertyState = $.trim($("#<%= this.ddlState.ClientID %>").val());
            if (PropertyState != "") {
                sQueryStrings += "&PropertyState=" + encodeURIComponent(PropertyState);
            }
            // PropertyZip
            var PropertyZip = $.trim($("#<%= this.tbxZip.ClientID %>").val());
            if (PropertyZip != "") {
                sQueryStrings += "&PropertyZip=" + encodeURIComponent(PropertyZip);
            }

            return sQueryStrings;
        }


        function btnSearch_onclick() {

            var sQueryStrings = BuildQueryString1();
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
                window.location.href = window.location.pathname + "?sid=" + RadomStr + sQueryStrings + "&ProspectID=" + $(hdProspectIDClientId).val() + "&Type='S'&CloseDialogCodes=window.parent.CloseGlobalPopup()&RefreshCodes=window.parent.RefreshLoanDetailInfo()";
            }
            else {

                window.location.href = window.location.pathname + "?sid=" + RadomStr + "&ProspectID=" + $(hdProspectIDClientId).val() + "&Type='S'&CloseDialogCodes=window.parent.CloseGlobalPopup()&RefreshCodes=window.parent.RefreshLoanDetailInfo()";
            }
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }
// ]]>
    </script>

    

</head>
<body>

<form id="form1" runat="server">
    <asp:HiddenField ID="hdProspectID" runat="server" />
    <asp:HiddenField ID="hdLoanIds" runat="server" />
    <asp:HiddenField ID="hdLinkType" runat="server" />
    <div id="divContent" class="DetailsContainer">
    <div>
        <table cellpadding="0" cellspacing="0" width="750px">
            <tr>
                <td style="width:70px; padding-left:9px;">
                    Prospect:
                </td>
                <td style="width:230px; padding-left:9px;">
                    <asp:Label ID="lbProspect" runat="server" Width="220px"></asp:Label>
                </td>
                <td style="width:220px; padding-left:9px;">
                    SSN: &nbsp;&nbsp;
                    <asp:Label ID="lbSSN" runat="server" Width="150px"></asp:Label>
                </td>
                <td style="width:220px; padding-left:9px;">
                    DOB: &nbsp;&nbsp;
                    <asp:Label ID="lbDOB" runat="server" Width="150px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="padding-left:9px; padding-top:5px;">
                    Address:
                </td>
                <td style="padding-left:9px; padding-top:5px">
                    <asp:Label ID="lbAddress" runat="server" Width="220px"></asp:Label>
                </td>
                <td style=" padding-left:9px; padding-top:5px;" colspan="2">
                    Loan Officer: &nbsp;&nbsp;
                    <asp:Label ID="lbLoanOfficer" runat="server" Width="200px"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </div>
    <br />
    <br />
<%--    <asp:UpdatePanel ID="updatePanel" runat="server">
        <ContentTemplate>--%>
            <div>
                <table cellpadding="0" cellspacing="0" width="700px">
                    <tr>
                        <td style="width:115px; padding-left:9px;">
                            Borrower Last Name:
                        </td>
                        <td style="width:120px; padding-left:9px;">
                            <asp:TextBox ID="tbxBrwLastName" runat="server" Width="100px" MaxLength="200"></asp:TextBox>
                        </td>
                        <td style="width:115px; padding-left:9px; text-align:left;">
                            Borrower First Name:
                        </td>
                        <td style="width:140px; padding-left:9px;">
                            <asp:TextBox ID="tbxBrwFirstName" runat="server" Width="100px" MaxLength="200"></asp:TextBox>
                        </td>
                        <td style="width:90px; padding-left:9px; text-align:right;">
                            Loan Status:
                        </td>
                        <td style="width:100px; padding-left:9px;">
                            <asp:DropDownList ID="ddlLoanStatus" runat="server" DataTextField="StatusName" DataValueField="StatusId" Width="90px">
                            </asp:DropDownList>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:9px; padding-top:5px;">
                            Property Address:
                        </td>
                        <td style="padding-left:9px; padding-top:5px;" colspan="6">
                            <asp:TextBox ID="tbxAddress" runat="server" Width="315px" MaxLength="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left:9px; padding-top:5px;">
                            City:
                        </td>
                        <td style="padding-left:9px; padding-top:5px;">
                            <asp:TextBox ID="tbxCity" runat="server" Width="100px" MaxLength="200"></asp:TextBox>
                        </td>
                        <td style=" padding-top:5px; padding-left:9px; text-align:right;">
                            State:
                        </td>
                        <td style=" padding-top:5px; padding-left:9px;">
                            <asp:DropDownList ID="ddlState" runat="server" Width="105px">
                                <asp:ListItem Text="All" Value="" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="AL" Value="AL"></asp:ListItem>
                                <asp:ListItem Text="AK" Value="AK"></asp:ListItem>
                                <asp:ListItem Text="AZ" Value="AZ"></asp:ListItem>
                                <asp:ListItem Text="AR" Value="AR"></asp:ListItem>
                                <asp:ListItem Text="CA" Value="CA"></asp:ListItem>
                                <asp:ListItem Text="CO" Value="CO"></asp:ListItem>
                                <asp:ListItem Text="CT" Value="CT"></asp:ListItem>
                                <asp:ListItem Text="DC" Value="DC"></asp:ListItem>
                                <asp:ListItem Text="DE" Value="DE"></asp:ListItem>
                                <asp:ListItem Text="FL" Value="FL"></asp:ListItem>
                                <asp:ListItem Text="GA" Value="GA"></asp:ListItem>
                                <asp:ListItem Text="HI" Value="HI"></asp:ListItem>
                                <asp:ListItem Text="ID" Value="ID"></asp:ListItem>
                                <asp:ListItem Text="IL" Value="IL"></asp:ListItem>
                                <asp:ListItem Text="IN" Value="IN"></asp:ListItem>
                                <asp:ListItem Text="IA" Value="IA"></asp:ListItem>
                                <asp:ListItem Text="KS" Value="KS"></asp:ListItem>
                                <asp:ListItem Text="KY" Value="KY"></asp:ListItem>
                                <asp:ListItem Text="LA" Value="LA"></asp:ListItem>
                                <asp:ListItem Text="ME" Value="ME"></asp:ListItem>
                                <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                                <asp:ListItem Text="MA" Value="MA"></asp:ListItem>
                                <asp:ListItem Text="MI" Value="MI"></asp:ListItem>
                                <asp:ListItem Text="MN" Value="MN"></asp:ListItem>
                                <asp:ListItem Text="MS" Value="MS"></asp:ListItem>
                                <asp:ListItem Text="MO" Value="MO"></asp:ListItem>
                                <asp:ListItem Text="MT" Value="MT"></asp:ListItem>
                                <asp:ListItem Text="NE" Value="NE"></asp:ListItem>
                                <asp:ListItem Text="NV" Value="NV"></asp:ListItem>
                                <asp:ListItem Text="NH" Value="NH"></asp:ListItem>
                                <asp:ListItem Text="NJ" Value="NJ"></asp:ListItem>
                                <asp:ListItem Text="NM" Value="NM"></asp:ListItem>
                                <asp:ListItem Text="NY" Value="NY"></asp:ListItem>
                                <asp:ListItem Text="NC" Value="NC"></asp:ListItem>
                                <asp:ListItem Text="ND" Value="ND"></asp:ListItem>
                                <asp:ListItem Text="OH" Value="OH"></asp:ListItem>
                                <asp:ListItem Text="OK" Value="OK"></asp:ListItem>
                                <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                <asp:ListItem Text="PA" Value="PA"></asp:ListItem>
                                <asp:ListItem Text="PR" Value="PR"></asp:ListItem>
                                <asp:ListItem Text="RI" Value="RI"></asp:ListItem>
                                <asp:ListItem Text="SC" Value="SC"></asp:ListItem>
                                <asp:ListItem Text="SD" Value="SD"></asp:ListItem>
                                <asp:ListItem Text="TN" Value="TN"></asp:ListItem>
                                <asp:ListItem Text="TX" Value="TX"></asp:ListItem>
                                <asp:ListItem Text="UT" Value="UT"></asp:ListItem>
                                <asp:ListItem Text="VT" Value="VT"></asp:ListItem>
                                <asp:ListItem Text="VA" Value="VA"></asp:ListItem>
                                <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                                <asp:ListItem Text="WV" Value="WV"></asp:ListItem>
                                <asp:ListItem Text="WI" Value="WI"></asp:ListItem>
                                <asp:ListItem Text="WY" Value="WY"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style=" padding-top:5px; padding-left:9px; text-align:right;">
                            Zip:
                        </td>
                        <td style=" padding-top:5px; padding-left:9px;">
                            <asp:TextBox ID="tbxZip" runat="server" Width="90px" MaxLength="10"></asp:TextBox>
                        </td>
                        <td style=" padding-top:5px; padding-left:9px;">
                            <input id="btnSearch" type="button" value="Search" class="Btn-66" onclick="return btnSearch_onclick()" style="width:75px;"/>
                       </td>
                    </tr>
                    <tr>
                        <td style="padding-left:9px; padding-top:25px;" colspan="7">
                            <asp:Button ID="btnLink" Text="Link" runat="server" Width="75px" class="Btn-66" OnClick="btnLink_Click"/>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" Width="75px" onclick="btnCancel_onclick()" />
                            <div id="divMenuLink" class="contextMenu" style="display: none;">
                            <ul>
                                <li id="Borrower">Link as Borrower</li>
                                <li id="Coborrower">Link as Coborrower</li>
                            </ul>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="7" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divLinkLoanGrid" class="ColorGrid" style="width: 750px; margin-top: 5px; margin-left:9px;">
            <asp:GridView ID="gvLinkLoanList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                Width="100%" EmptyDataText="There is no data in database." CellPadding="3" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <AlternatingRowStyle CssClass="EvenRow" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input type="checkbox" tag='<%# Eval("FileId") %>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status" ItemStyle-Wrap="false">
                        <ItemTemplate>
                            <%# Eval("Status")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Est Close" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="70" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("EstCloseDate", "{0:d}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="80" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("LoanAmount", "{0:c0}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="75" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <%# Eval("Rate", "{0:N4}")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lien" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="60">
                        <ItemTemplate>
                            <%# Eval("LienPosition")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Purpose" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                        <ItemTemplate>
                            <%# Eval("Purpose")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Point File" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                        <ItemTemplate>
                            <%# Eval("FileName")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Property" ItemStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="130">
                        <ItemTemplate>
                            <%# Eval("PropertyAddr")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
<%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
</form>
</body>
</html>