<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="Company Overview" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="CompanyOverview" CodeBehind="CompanyOverview.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link id="Link1" href="../css/style.web.css" rel="stylesheet" type="text/css" runat="server" />
    <link id="Link2" href="../css/style.custom.css" rel="stylesheet" type="text/css"
        runat="server" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/common.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.autocomplete.min.js" type="text/javascript"></script>
    <style type="text/css">
        td
        {
            vertical-align: top;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            $("#<%= btnUserSeach.ClientID %>").click(function () {
                if ($.trim($("#<%= txbUserName.ClientID %>").val()).length == 0) {
                    alert("Please enter the user name");
                    $("#<%= txbUserName.ClientID %>").val("");
                    return false;
                }
            });

            $("#<%= btnGroupSarch.ClientID %>").click(function () {
                if ($.trim($("#<%= txbGroupName.ClientID %>").val()).length == 0) {
                    alert("Please enter the group name");
                    $("#<%= txbGroupName.ClientID %>").val("");
                    return false;
                }
            });

            $("#<%= btnFolderSearch.ClientID %>").click(function () {
                if ($.trim($("#<%= txbPointFolder.ClientID %>").val()).length == 0) {
                    alert("Please enter the point folder")
                    $("#<%= txbPointFolder.ClientID %>").val("");
                    return false;
                }
            });

            $("#<%= btnFileSearch.ClientID %>").click(function () {
                if ($.trim($("#<%= txbPointFileName.ClientID %>").val()).length == 0) {
                    alert("Please enter the file name");
                    $("#<%= txbPointFileName.ClientID %>").val("");
                    return false;
                }
            });
            $("#<%= txbPointFileName.ClientID %>").autocomplete(
            {
                minLength: 2,
                source: function (request, response) {
                    $.ajax({
                        url: "GetPointfilesbg.aspx",

                        dataType: "json",
                        data: request,
                        success: function (data) {
                            response(data);
                        }
                    })
                }
            });
        });

        function Toggle(org) {

            var operGridId = $(org).parent().parent().siblings().toggle();
            if ($(org).attr("src") == "../images/CompanyOverview/accordion.gif") {
                $(org).attr("src", "../images/CompanyOverview/expansion.gif");
            }
            else {
                $(org).attr("src", "../images/CompanyOverview/accordion.gif");
            } 
        } 
 
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div>
        <div class="Heading">
            Company Overview
        </div>
        <div class="SplitLine">
        </div>
        <br />
        <div style="margin-left: 15px">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr style="height: 30px;">
                    <td style="width: 100px;">
                        <asp:Label ID="Label1" runat="server" Text="Username"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbUserName" runat="server" Width="140"></asp:TextBox>
                    </td>
                    <td style="margin-left: 10px;">
                        <asp:ImageButton ID="btnUserSeach" Style="margin-left: 10px;" ImageUrl="../images/CompanyOverview/Search.gif"
                            runat="server" OnClick="btnUserSeach_Click" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td style="width: 100px;">
                        <asp:Label ID="Label2" runat="server" Text="Group Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbGroupName" runat="server" Width="140"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnGroupSarch" Style="margin-left: 10px;" ImageUrl="../images/CompanyOverview/Search.gif"
                            runat="server" OnClick="btnGroupSarch_Click" />
                    </td>
                </tr>
                <tr style="height: 30px;">
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Point Folder"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbPointFolder" runat="server" Width="140"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnFolderSearch" Style="margin-left: 10px;" ImageUrl="../images/CompanyOverview/Search.gif"
                            runat="server" OnClick="btnFolderSearch_Click" />
                    </td>
                    <td style="width: 20px">
                    </td>
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Point File Name"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txbPointFileName" runat="server" Width="140"></asp:TextBox>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnFileSearch" Style="margin-left: 10px;" ImageUrl="../images/CompanyOverview/Search.gif"
                            runat="server" OnClick="btnFileSearch_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div style="margin-left: 15px">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 100px;">
                    </td>
                    <td align="center" valign="middle">
                        <asp:Image ID="Image1" onclick="Toggle(this);" Visible="false" ImageUrl="../images/CompanyOverview/accordion.gif"
                            runat="server" />
                        <asp:Image ID="Image2" Visible="false" ImageUrl="../images/CompanyOverview/Company.gif"
                            runat="server" />
                        <asp:Label ID="lbCompanyName" runat="server" Text=""></asp:Label>
                    </td>
                    <td style="width: 100px;">
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="3">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <asp:Repeater ID="rptCompanyOverview" runat="server" OnItemDataBound="rptCompanyOverview_ItemDataBound">
                                    <ItemTemplate>
                                        <td align="center" valign="middle">
                                            <table border="0" cellpadding="0" cellspacing="0" width="200px;">
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="width: 200px; height: 40px; background-image: url(../images/CompanyOverview/topLine.gif);
                                                        background-position: top; background-repeat: repeat-x;">
                                                        <asp:Image ID="Image12" ImageUrl="../images/CompanyOverview/VerticalLine.gif" runat="server" />
                                                        <br />
                                                        <asp:Image ID="Image1" onclick="Toggle(this);" ImageUrl="../images/CompanyOverview/accordion.gif"
                                                            runat="server" />
                                                        <asp:Image ID="Image2" ImageUrl="../images/CompanyOverview/Region.gif" runat="server" />
                                                        <span id="Label1" <%# Eval("OrganizationType").ToString()=="Region"? "style= 'color:Blue; '":""%>>
                                                            <%# Eval("RegionName")%>
                                                        </span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center" style="width: 200px; background-color: #e4e7ef;">
                                                        <asp:Repeater ID="rptDivions" OnItemDataBound="rptDivions_ItemDataBound" runat="server">
                                                            <ItemTemplate>
                                                                <table id="tbDivision" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Image ID="Image1" onclick="Toggle(this);" ImageUrl="../images/CompanyOverview/accordion.gif"
                                                                                runat="server" />
                                                                            <asp:Image ID="Image2" ImageUrl="../images/CompanyOverview/Division.gif" runat="server" />
                                                                            <span id="Label1" <%# Eval("OrganizationType").ToString()=="Division"? "style= color:Blue; ":""%>>
                                                                                <%# Eval("DivisionName")%>
                                                                            </span>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center" style="background-color: #f9f9f9;">
                                                                            <asp:Repeater ID="rptBranchs" runat="server">
                                                                                <ItemTemplate>
                                                                                    <table id="tbBranch" border="0" cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td style="width: 30px;">
                                                                                            </td>
                                                                                            <td align="center">
                                                                                                <asp:Image ID="Image2" ImageUrl="../images/CompanyOverview/Branch.gif" runat="server" />
                                                                                                <span id="Label1" <%# Eval("OrganizationType").ToString()=="Branch"? "style= color:Blue; ":""%>>
                                                                                                    <%# Eval("BranchName")%>
                                                                                                </span>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:Repeater>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                <tr>
                                                                    <td style="height: 10px; background-color: #e4e7ef;">
                                                                    </td>
                                                                </tr>
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
        </div>
    </div>
</asp:Content>
