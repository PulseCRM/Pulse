<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectDetailPopup.aspx.cs" Inherits="Prospect_ProspectDetailPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Update Lead</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        jQuery.validator.addMethod("phoneUS", function (phone_number, element) {
            phone_number = phone_number.replace(/\s+/g, "");
            return this.optional(element) || phone_number.length > 9 &&
		phone_number.match(/^(1-?)?(\([2-9]\d{2}\)|[2-9]\d{2})-?[2-9]\d{2}-?\d{4}$/);
        }, "Please enter valid phone number");

        $(document).ready(function () {

            $(".DateField").datepick();

            $("#txtExperianScore").OnlyInt();
            $("#txtTransUnitScore").OnlyInt();
            $("#txtEquifaxScore").OnlyInt();

            AddValidators();
        });

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {

                    ddlLoanOfficer: {
                        required: true
                    },
                    txtFirstName: {
                        required: true
                    },
                    txtLastName: {
                        required: true
                    },
                    txtAddress: {
                        required: true
                    },
                    txtCity: {
                        required: true
                    },
                    ddlState: {
                        required: true
                    },
                    txtZip: {
                        required: true
                    },
                    txtEmail: {
                        email: true
                    },
                    txtDOB: {
                        date: true
                    },
                    txtHomePhone: {
                        phoneUS: true
                    },
                    txtCellPhone: {
                        phoneUS: true
                    },
                    txtBizPhone: {
                        phoneUS: true
                    }
                },
                messages: {

                    ddlLoanOfficer: {
                        required: "*"
                    },
                    txtFirstName: {
                        required: "*"
                    },
                    txtLastName: {
                        required: "*"
                    },
                    txtAddress: {
                        required: "*"
                    },
                    txtCity: {
                        required: "*"
                    },
                    ddlState: {
                        required: "*"
                    },
                    txtZip: {
                        required: "*"
                    },
                    txtEmail: {
                        email: "Please enter valid email."
                    },
                    txtDOB: {
                        date: "Please enter valid date."
                    },
                    txtHomePhone: {
                        phoneUS: "<span title='Please enter valid phone number.' style='cursor: pointer;'>?<span>"
                    },
                    txtCellPhone: {
                        phoneUS: "<span title='Please enter valid phone number.' style='cursor: pointer;'>?<span>"
                    },
                    txtBizPhone: {
                        phoneUS: "<span title='Please enter valid phone number.' style='cursor: pointer;'>?<span>"
                    }
                }
            });
        }

        function BeforeSave() {

            
            return true;
        }

        function ShowDialog_PointFolderSelection(ContactID, Action) {

            // close dialog
            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var iFrameSrc = "DisposePointFolderList.aspx?sid=" + RadomStr + "&Action=" + Action + "&ContactID=" + ContactID + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 601
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 600;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowGlobalPopup("Point Folder Selection", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        function BeforeDelete() {

            var r1 = confirm("Deleting the client will also delete the client's leads, activity history, notes, alerts, and email notifications.\r\n Are you sure you want to continue?");

            if (r1 == false) {
                return false;
            }
            else {
                return true;
            }
            
        }

        function btnCancel_onclick() {

            var CloseDialogCodes = GetQueryString1("CloseDialogCodes");
            eval(CloseDialogCodes);
        }

        function BeforeSelect() {

            ShowDialog_Search();
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
        }


        function OpenSelectContact(sWhere) {

            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var stype = "prospectdetail";
            var iFrameSrc = "../Contact/SelectContactsPopup.aspx?sid=" + RadomNum + "&pagesize=15&sCon=" + sWhere + "&type=" + stype + "&CloseDialogCodes=window.parent.CloseGlobalPopup()";

            var BaseWidth = 750
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 550;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;
            $("#ifrSearchContact").attr("src", iFrameSrc);
            $("#ifrSearchContact").attr("width", iFrameWidth);
            $("#ifrSearchContact").attr("height", iFrameHeight);

            // show modal
            $("#divSearchContact").dialog({
                height: divHeight,
                width: divWidth,
                title: "Select Contacts",
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")

//            window.parent.parent.ShowGlobalPopup("Select Contacts", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);

        }

        function ShowReferral(sContactID, sContactName) {

            CloseGlobalPopup();
            $("#txbReferral").val(sContactName);
            $("#hdnReferralID").val(sContactID);

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
    <div id="divContainer" style="width: 780px; height: 400px; border: solid 0px red;">
        <div id="divBtns">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnUpdatePoint" runat="server" Text="Update Point" CssClass="Btn-91" onclick="btnUpdatePoint_Click" />
                    </td>
                    <td>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="Btn-66" OnClientClick="return BeforeDelete();" onclick="btnDelete_Click" />
                    </td>
                    <td>
                        <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                    </td>
                </tr>
            </table>
            
        </div>
        <div id="divDetails" style="margin-top: 10px;">
            <table>
                <tr>
                    <td style="width: 70px;">Loan Officer:</td>
                    <td style="width: 180px;">
                        <asp:DropDownList ID="ddlLoanOfficer" runat="server" Width="150px" DataValueField="UserID" DataTextField="FullName">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 92px;">Reference Code:</td>
                    <td style="width: 150px;">
                        <asp:TextBox ID="txtRefCode" runat="server" Width="120px" MaxLength="255"></asp:TextBox>
                    </td>
                    <td style="width: 40px;">Status:</td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" Width="100">
                            <asp:ListItem>Active</asp:ListItem>
                            <asp:ListItem>Bad</asp:ListItem>
                            <asp:ListItem>Lost</asp:ListItem>
                            <asp:ListItem>Suspended</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px;">Lead Source:</td>
                    <td>
                        <asp:DropDownList ID="ddlLeadSource" runat="server" DataValueField="LeadSource" DataTextField="LeadSource" Width="400px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <table>
                <tr>
                    <td style="width: 70px;">Referral:</td>
                    <td>
<%--                        <asp:DropDownList ID="DropDownList1" Visible="false" runat="server"
                         DataValueField="LeadSource" DataTextField="LeadSource" Width="400px">
                        </asp:DropDownList>--%>
                        <asp:TextBox ID="txbReferral" runat="server" Width="400px" MaxLength="255" ReadOnly="true"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSelect" runat="server" Text="Select" CssClass="Btn-66" OnClientClick="return BeforeSelect()" />
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px;">First Name:</td>
                    <td style="width: 180px;">
                        <asp:TextBox ID="txtFirstName" runat="server" Width="145px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 92px;">Middle Name:</td>
                    <td style="width: 150px;">
                        <asp:TextBox ID="txtMiddleName" runat="server" Width="120px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 94px;">Last Name:</td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" Width="120px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Title:</td>
                    <td>
                        <asp:DropDownList ID="ddlTitle" runat="server" Width="150px">
                            <asp:ListItem Value="">-- select a title --</asp:ListItem>
                            <asp:ListItem>Mr.</asp:ListItem>
                            <asp:ListItem>Mrs.</asp:ListItem>
                            <asp:ListItem>Ms.</asp:ListItem>
                            <asp:ListItem>Dr.</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>Generation Code:</td>
                    <td>
                        <asp:TextBox ID="txtGenerationCode" runat="server" Width="120px" MaxLength="10"></asp:TextBox>
                    </td>
                    <td>SSN:</td>
                    <td>
                        <asp:TextBox ID="txtSSN" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Home Phone:</td>
                    <td>
                        <asp:TextBox ID="txtHomePhone" runat="server" Width="145px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>Cell Phone:</td>
                    <td>
                        <asp:TextBox ID="txtCellPhone" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td>Busniness Phone:</td>
                    <td>
                        <asp:TextBox ID="txtBizPhone" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px;">Fax:</td>
                    <td style="width: 180px;">
                        <asp:TextBox ID="txtFax" runat="server" Width="145px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="width: 92px;">Preferred Contact:</td>
                    <td style="width: 150px;">
                        <asp:DropDownList ID="ddlPreferredContact" runat="server" Width="124px">
                            <asp:ListItem Value="" Selected="True">-- select --</asp:ListItem>
                            <asp:ListItem Value="Home Phone">Home Phone</asp:ListItem>
                            <asp:ListItem Value="Business Phone">Business Phone</asp:ListItem>
                            <asp:ListItem Value="Cell Phone">Cell Phone</asp:ListItem>
                            <asp:ListItem Value="Email">Email</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 94px;">Credit Ranking:</td>
                    <td>
                        <asp:DropDownList ID="ddlCreditRanking" runat="server" Width="124px">
                            <asp:ListItem Value="" Selected="True">-- select --</asp:ListItem>
                            <asp:ListItem Value="Excellent">Excellent</asp:ListItem>
                            <asp:ListItem Value="Very Good">Very Good</asp:ListItem>
                            <asp:ListItem Value="Good">Good</asp:ListItem>
                            <asp:ListItem Value="Fair">Fair</asp:ListItem>
                            <asp:ListItem Value="Poor">Poor</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px;">Address:</td>
                    <td style="width: 430px;">
                        <asp:TextBox ID="txtAddress" runat="server" Width="400px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 25px;">City:</td>
                    <td>
                        <asp:TextBox ID="txtCity" runat="server" Width="190px" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px;">State:</td>
                    <td style="width: 180px;">
                        <asp:DropDownList ID="ddlState" runat="server" Width="80">
                        </asp:DropDownList>
                    </td>
                    <td style="width: 25px;">Zip:</td>
                    <td>
                        <asp:TextBox ID="txtZip" runat="server" Width="80px" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px;">Email:</td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" Width="400px" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 70px;">DOB:</td>
                    <td>
                        <asp:TextBox ID="txtDOB" runat="server" CssClass="DateField"  MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">Experian/FICO Score:</td>
                    <td style="width: 140px;">
                        <asp:TextBox ID="txtExperianScore" runat="server" Width="105px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td style="width: 100px;">TransUnion Score:</td>
                    <td style="width: 150px;">
                        <asp:TextBox ID="txtTransUnitScore" runat="server" Width="120px" MaxLength="4"></asp:TextBox>
                    </td>
                    <td style="width: 82px;">Equifax Score:</td>
                    <td>
                        <asp:TextBox ID="txtEquifaxScore" runat="server" Width="120px" MaxLength="4"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="margin-top: 15px;">
                <tr>
                    <td style="width: 254px;">Created On: <asp:Label ID="lbCreatedOn" runat="server" Text="Label"></asp:Label></td>
                    <td>Created By: <asp:Label ID="lbCreatedBy" runat="server" Text="Label"></asp:Label></td>
                </tr>
                
            </table>
            <table style="margin-top: 5px;">
                <tr>
                    <td style="width: 254px;">Modified On: <asp:Label ID="lbModifiedOn" runat="server" Text="Label"></asp:Label></td>
                    <td>Modified By: <asp:Label ID="lbModifiedBy" runat="server" Text="Label"></asp:Label></td>
                </tr>
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hdnOldStatus" runat="server" />
    <asp:HiddenField ID="hdnReferralID" runat="server" />
    <asp:HiddenField ID="hfLoanOfficer" runat="server" />
    </form>
    <div id="divSearchContact" title="Partner Contact Search" style="display: none;"> 
        <iframe id="ifrSearchContact" frameborder="0" style="overflow: SCROLL; overflow-x: HIDDEN">
        </iframe>
    </div>
</body>
</html>