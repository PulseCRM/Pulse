<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanDetailEdit.aspx.cs"
    Inherits="LoanDetailEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" /> 
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script> 
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $(".DateField").datepick();
            // add onchange event 
            var ddlPointFolderClientId = "#<%= ddlPointFolder.ClientID %>";
            var txbPointFileNameClientId = "#<%= txbPointFileName.ClientID %>";
//            $(ddlPointFolderClientId).change(ChangeExportBtnStatus);
//            $(txbPointFileNameClientId).change(ChangeExportBtnStatus);
        });

        function ChangeExportBtnStatus() {
            var ddlPointFolderClientId = "#<%= ddlPointFolder.ClientID %>";
            var txbPointFileNameClientId = "#<%= txbPointFileName.ClientID %>";
            var btnExportClientId = "#<%= btnExport.ClientID %>";
            if ($(ddlPointFolderClientId).val() == "" || $(txbPointFileNameClientId).val() == "") {
                $(btnExportClientId).attr("disabled", true);
            }
            else {
                $(btnExportClientId).attr("disabled", false);
            }
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

        var IsSelectReferral = 1;
        function SelectReferral() {
            OpenSelectContact("");
            IsSelectReferral = 1;
            return false;
        }
        //
        //Show search contact popup
        function ShowDialog_Search() {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "../Contact/SearchContactsPopup.aspx?sid=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 600
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 240;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            $("#ifrSearchContact").attr("src", iFrameSrc);
            $("#ifrSearchContact").attr("width", iFrameWidth);
            $("#ifrSearchContact").attr("height", iFrameHeight);
            $("#divSearchContact").dialog({
                height: divHeight,
                width: divWidth,
                title: "Partner Contact Search",
                modal: true,
                resizable: false
            });

            $(".ui-dialog").css("border", "solid 3px #aaaaaa")

            //window.parent.parent.ShowGlobalPopup("Partner Contact Search", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        //
        //Get search condition
        function GetSearchCondition(sWhere) {

            CloseGlobalPopup();

            //Show select contact popup
            OpenSelectContact(sWhere);
            IsSelectReferral = 0;
        }


        //Show search contact popup
        function OpenSelectContact(sWhere) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var stype = "prospectdetail";
            var iFrameSrc = "../Contact/SelectContactsPopup.aspx?sid=" + RadomNum + "&sCon=" + sWhere + "&pagesize=15&type=" + stype + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 750;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 550;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            $("#ifrSearchContact").attr("src", iFrameSrc);
            $("#ifrSearchContact").attr("width", iFrameWidth);
            $("#ifrSearchContact").attr("height", iFrameHeight);
            $("#divSearchContact").dialog({
                height: divHeight,
                width: divWidth,
                title: "Partner Contact Search",
                modal: false,
                resizable: false
            });


            //$(".ui-dialog").css("border", "solid 3px #aaaaaa")

            $("#divSearchContact").dialog("open");
            return false;

        }

        function ShowReferral(sContactID, sContactName) {

            CloseGlobalPopup();

            if (IsSelectReferral == 1) {
                $("#txtReferral").val(sContactName);
                $("#hdnReferralID").val(sContactID);
            }
            else if (IsSelectReferral == 0) {

                var n = sContactName.split(',');
                if (n.length == 2) {
                    $("#txtCBFirstname").val(n[1]);
                    $("#txtCBLastname").val(n[0]);
                    $("#txtCBMiddlename").val("");
                }
                else {
                    $("#txtCBLastname").val(sContactName);
                }

                $("#hdnCoBorrowerID").val(sContactID);
            }

        }

        function CloseGlobalPopup() {

            $("#divSearchContact").dialog("close");
            $("#divSearchContact").dialog("destroy");
        }



// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="divModuleName" class="ModuleTitle">Loan Detail
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text=" Save " class="Btn-66" OnClick="btnSave_Click" />
                </td>
                <td>
                    <asp:Button ID="btnExport" runat="server" Text=" Export to Point " class="Btn-115"
                        Enabled="true" OnClick="btnExport_Click" />
                </td>
                <td>
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                </td>
            </tr>
        </table></div>
        <asp:HiddenField ID="hfFileID" runat="server" />
        <table style="margin-left: 20px;">
            <tr>
                <td>
                    Copy From
                </td>
                <td colspan="7">
                    <asp:DropDownList ID="ddlCopyFrom" runat="server" Height="20px" Width="450px">
                    </asp:DropDownList>
                    &nbsp;
                    <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="Btn-66" OnClick="btnCopy_Click" />&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Loan Officer
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlLoanOfficer" runat="server" Height="18px" Width="235px"
                        AutoPostBack="True" OnSelectedIndexChanged="ddlLoanOfficer_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                    Ranking
                </td>
                <td>
                    <asp:DropDownList ID="ddlRanking" runat="server" Height="18px" Width="141px">
                        <asp:ListItem Text="Hot" Value="hot"></asp:ListItem>
                        <asp:ListItem Text="Warm" Value="warm"></asp:ListItem>
                        <asp:ListItem Text="Cold" Value="cold"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>

            <tr><!--gdc crm33-->
                <td>
                    Referral
                </td>
                <td colspan="5">

                    <asp:TextBox ID="txtReferral" runat="server" Width="200"></asp:TextBox>
                    <asp:HiddenField ID="hdnReferralID" runat="server" />
                    
                    
                </td>
                
                <td>
                <input type="button" class="Btn-66" id="btnSelectReferral" value="Select" onclick="return SelectReferral();" />
                </td>
                <td colspan="2">
                    <table>
                        <tr>
                            <td >Lead Source </td>
                            <td>&nbsp;&nbsp;
                               <asp:DropDownList ID="ddlLeadSource" runat="server" DataTextField="LeadSource" DataValueField="LeadSource" Width="200">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    
                    </table>
                </td>
            </tr>

            <tr>
                <td>
                    Borrower
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlBorrower" runat="server" Height="18px" Width="235px">
                    </asp:DropDownList>
                </td>

            </tr>

            <tr><!--gdc crm33-->
                <td style="">
                    Coborrower Firstname:
                </td>
                <td colspan="5">
                    <asp:TextBox ID="txtCBFirstname" runat="server" MaxLength="20" Width="100"></asp:TextBox>
                </td>

                <td style="">
                    Coborrower Middlename:
                </td>
                <td>
                   <table>
                        <tr>
                            <td ><asp:TextBox ID="txtCBMiddlename" runat="server" MaxLength="20"  Width="70"></asp:TextBox></td>
                            <td>Coborrower Lastname:</td>
                            <td><asp:TextBox ID="txtCBLastname" runat="server"  MaxLength="20" Width="100"></asp:TextBox></td>
                            <td>
                            <asp:HiddenField ID="hdnCoBorrowerID" runat="server" Value="" />
                            <asp:HiddenField ID="hdnCoBorrowerName" runat="server" Value="" />
                            <asp:Button ID="btnSelectCoBorrower"  CssClass="Btn-66"  runat="server" Text="Select"  OnClientClick="ShowDialog_Search();return false;" /></td>
                        </tr>
                    
                    </table>
                </td>

            </tr>

            <tr>
                <td>
                    Amount
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txbAmount" runat="server" Width="110px"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                    Interest Rate
                </td>
                <td>
                    <asp:TextBox ID="txbInterestRate" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    Estimated Close Date
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txbEstimatedDate" runat="server" Width="110px" CssClass="DateField"
                        MaxLength="10"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                    Loan Program
                </td>
                <td>
                    <asp:DropDownList ID="ddlLoanProgram" runat="server" Height="17px" Width="235px">
                    </asp:DropDownList>
                </td>
                </tr>
            <tr>
                <td>
                    Purpose
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlPurpose" runat="server" Height="18px" Width="235px">
                         <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
                        <asp:ListItem Text="Purchase" Value="Purchase"></asp:ListItem>
                        <asp:ListItem Text="No Cash-Out Refinance" Value="No Cash-Out Refinance"></asp:ListItem>
                        <asp:ListItem Text="Cash-Out Refinance" Value="Cash-Out Refinance"></asp:ListItem>
                        <asp:ListItem Text="Construction" Value="Construction"></asp:ListItem>
                        <asp:ListItem Text="Construction-Perm" Value="Construction-Perm"></asp:ListItem>
                        <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td></td>
                <td>
                    Lien Position
                </td>
                <td>
                    <asp:DropDownList ID="ddlLienPosition" runat="server" Height="18px" Width="141px">
                        <asp:ListItem Text="-- select --" Value=""></asp:ListItem>
                        <asp:ListItem Text="First" Value="First"></asp:ListItem>
                        <asp:ListItem Text="Second" Value="Second"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>


            <tr>
                <td>
                    Propety Type
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txbPropetyType" runat="server" MaxLength="50" Width="235px"></asp:TextBox>&nbsp;
                </td>
                <td>
                </td>
                <td>
                    Housing Status
                </td>
                <td>
                    <asp:TextBox ID="txbHousingStatus" runat="server" MaxLength="50" Width="235px"></asp:TextBox>&nbsp;
                </td>
            </tr>

            <tr>
                <td>
                    
                </td>
                <td colspan="4">
                    <asp:CheckBox ID="cbInterestOnly" runat="server" /><label for="cbInterestOnly" >Interest Only</label> &nbsp;
                </td>
                <td>
                </td>
                <td>
                    
                </td>
                <td>
                    <asp:CheckBox ID="cbIncludeEscrows" runat="server" /><label for="cbIncludeEscrows" >Include Escrows</label> &nbsp;
                </td>
            </tr>

            <tr>
                <td>
                    Coborrower Type
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txbCoborrowerType" runat="server" MaxLength="50" Width="235px"></asp:TextBox>&nbsp;
                </td>
                <td>
                </td>
                <td>
                    Rent Amount:
                </td>
                <td>
                    <asp:TextBox ID="txbRentAmount" runat="server" MaxLength="50" Width="235px"></asp:TextBox>&nbsp;
                </td>
            </tr>


            <tr>
                <td>
                    Copy
                </td>
                <td colspan="7">
                    <asp:Button ID="btnCopyAddress" runat="server" Text="Copy from Borrower's Mailing Address"
                        OnClick="btnCopyAddress_Click" Width="250px" class="Btn-250" />&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    Property Address
                </td>
                <td colspan="4">
                    <asp:TextBox ID="txbPropertyAddress" runat="server" MaxLength="50" Width="235px"></asp:TextBox>
                </td>
                <td>
                </td>
                <td>
                    City
                </td>
                <td>
                    <asp:TextBox ID="txbCity" runat="server" MaxLength="50" ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    State
                </td>
                <td>
                    <asp:DropDownList ID="ddlState" runat="server" Height="16px" Width="68px">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                    Zip
                </td>
                <td>
                    <asp:TextBox ID="txbZip" runat="server"  MaxLength="10" Width="128px"></asp:TextBox>
                </td>
                <td colspan="3">
                </td>
            </tr>
            <tr>
                <td>
                    Point Folder
                </td>
                <td colspan="4">
                    <asp:DropDownList ID="ddlPointFolder" runat="server" Height="18px" Width="235px">
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                    Point Filename
                </td>
                <td>
                    <asp:TextBox ID="txbPointFileName" runat="server" MaxLength="255"  Width="235px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="8">
                    <table width="100%">
                        <tr>
                            <td colspan="2">
                                Created On: 
                                <asp:Label ID="lbCreatedOn" runat="server" Text=""></asp:Label>
                            </td>
                            <td  colspan="2">
                                Created By: 
                                <asp:Label ID="lbCreatedBy" runat="server" Text=""></asp:Label>
                            </td>
                            <td colspan="2">
                                Modified On: 
                                <asp:Label ID="lbModifiedOn" runat="server" Text=""></asp:Label>
                            </td>
                            <td colspan="2">
                                Modified By:
                            </td>
                            <td>
                                <asp:Label ID="lbModifiedBy" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        <asp:HiddenField ID="hfLoanOfficer" runat="server" />
    </div>
    </form>
    <div id="divSearchContact" title="Partner Contact Search" style="display: none;"> 
        <iframe id="ifrSearchContact" frameborder="0" style="overflow: SCROLL; overflow-x: HIDDEN">
        </iframe>
    </div>
</body>
</html>
