<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanProgramSetupAdd.aspx.cs" Inherits="LoanProgramSetupAdd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Loan Program Setup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    
    <script src="../js/jquery.js" type="text/javascript"></script>

    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.button.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>

    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        $(document).ready(function () {

            
            $("#txtMargin").mask("99.999");
            $("#txtFirstAdj").mask("99.999");
            $("#txtSubAdj").mask("99.999");
            $("#txtLifetimeCap").mask("99.999");
            $("#txtTerm").mask("999");
            $("#txtDue").mask("999");

        });

        
        function BeforeSave() {

            var InvestorID = $("#ddlInvestor").val();
            if (InvestorID == "0") {

                alert("Please select Investor.");
                return false;
            }

            var LoanProgramID = $("#ddlLoanProgram").val();
            if (LoanProgramID == "0") {

                alert("Please select Loan Program.");
                return false;
            }
            var Term = $("#txtTerm").val();
            if (Term == "") {

                alert("Please enter Term.");
                return false;
            }
            var Due = $("#txtDue").val();
            if (Due == "") {

                alert("Please enter Due.");
                return false;
            }
            return true;
        }

        function btnCancel_onclick() {

            window.parent.location.href = window.parent.location.href;
        }

        function btnCreateLoanProgram_onclick() {

            $("#divContainer").block({ message: $('#divCreateLoanProgram'), css: { width: '400px'} });
        }

        function btnCreate2_onclick() {

            var LoanProgram = $.trim($("#txtLoanProgram").val());
            if (LoanProgram == "") {

                alert("Please enter Loan Program.");
                return;
            }
            var Term = $.trim($("#txtTerm").val());
            if (Term == "") {

                alert("Please enter Term.");
                return;
            }
            var Due = $.trim($("#txtDue").val());
            if (Due == "") {

                alert("Please enter Due.");
                return;
            }
            var IsARM = $("#chkIsARM").attr("checked");

            var sid = Math.random().toString().substr(2);

            $.getJSON("LoanProgramCreateAjax.aspx?sid=" + sid + "&LoanProgram=" + encodeURIComponent(LoanProgram) + "&IsARM=" + IsARM, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return;
                }

                window.location.href = window.location.href;
            });
        }

        function btnCreate3_onclick() {

            var LoanProgram = $.trim($("#txtLoanProgram").val());
            if (LoanProgram == "") {

                alert("Please enter Loan Program.");
                return;
            }
            var Term = $.trim($("#txtTerm").val());
            if (Term == "") {

                alert("Please enter Term.");
                return;
            }
            var Due = $.trim($("#txtDue").val());
            if (Due == "") {

                alert("Please enter Due.");
                return;
            }
            var IsARM = $("#chkIsARM").attr("checked");

            var sid = Math.random().toString().substr(2);

            $.getJSON("LoanProgramCreateAjax.aspx?sid=" + sid + "&LoanProgram=" + encodeURIComponent(LoanProgram) + "&IsARM=" + IsARM, function (data) {

                if (data.ExecResult == "Failed") {

                    alert(data.ErrorMsg);
                    return;
                }

                // success
                alert("Created loan program successfully.");

                $("#txtLoanProgram").val("");
                $("#chkIsARM").attr("checked", "true");
            });
        }

        function btnCancel2_onclick() {

            $('#txtLoanProgram').val("");

            $("#divContainer").unblock();
        }

        function btnCreateInvestor_onclick() {

            var sid = Math.random().toString().substr(2);

            var iFrameSrc = "PartnerCompanyCreate.aspx?sid=" + sid;

            var BaseWidth = 450
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 180;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            ShowGlobalPopup("Partner Company Setup", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

        //#region Show/Close Global Popup

        function ShowGlobalPopup(Title, iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc) {

            $("#divGlobalPopup").attr("title", Title);

            $("#ifrGlobalPopup").attr("src", iFrameSrc);
            $("#ifrGlobalPopup").width(iFrameWidth);
            $("#ifrGlobalPopup").height(iFrameHeight);

            // show modal
            $("#divGlobalPopup").dialog({
                height: divHeight,
                width: divWidth,
                modal: true,
                resizable: false,
                close: function (event, ui) {
                    $("#divGlobalPopup").dialog("destroy");
                    $("#ifrGlobalPopup").attr("src", "");
                }
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function CloseGlobalPopup() {

            $("#divGlobalPopup").dialog("close");
        }

        //#endregion


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="height:300px;">
        <br />
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSaveAndClose" runat="server" Text="Save and Close" OnClick="btnSaveAndClose_Click" OnClientClick="return BeforeSave()" CssClass="Btn-115" />
                </td>
                <td>
                    <asp:Button ID="btnSaveAndCreate" runat="server" Text="Save and Create Another" OnClick="btnSaveAndCreate_Click" OnClientClick="return BeforeSave()" CssClass="Btn-140" />
                </td>
                <td>
                    <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                </td>
            </tr>
        </table>
        <div style="margin-top: 15px;">
            <table cellpadding="3" cellspacing="3">
                <tr>
                    <td style="width: 70px;">Investor:</td>
                    <td>
                        <asp:DropDownList ID="ddlInvestor" runat="server" DataValueField="ContactCompanyId" DataTextField="Name" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <input id="btnCreateInvestor" type="button" value="Create" onclick="btnCreateInvestor_onclick()" class="Btn-66" />
                    </td>
                    <td style="padding-left: 15px;">
                        <asp:CheckBox ID="chkEnabled" runat="server" Text=" Enabled" Checked="true" />
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:CheckBox ID="chkARM" runat="server" Text=" ARM" Checked="true"/>
                    </td>
                </tr>
                <tr>
                    <td>Loan Program:</td>
                    <td>
                        <asp:DropDownList ID="ddlLoanProgram" runat="server" DataTextField="LoanProgram" DataValueField="LoanProgramID" Width="250px">
                        </asp:DropDownList>
                    </td>
                    <td style="padding-left: 15px;">
                        <input id="btnCreateLoanProgram" type="button" value="Create" onclick="btnCreateLoanProgram_onclick()" class="Btn-66" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr><td>Term:</td>
                <td>
                <asp:TextBox ID="txtTerm" runat="server" DataTextField="Term" Width="60px"/>&nbsp;months
                </td>
                <td colspan="2">Due:&nbsp;&nbsp;
                <asp:TextBox ID="txtDue" runat="server" DataTextField="Due" Width="60px"/>&nbsp;months
                </td>
                </tr>
                <tr>
                    <td>Index:</td>
                    <td>
                        <asp:TextBox ID="txtIndex" runat="server" MaxLength="255" Width="247px"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>

        <div style="margin-top: 15px;">
            <table>
                <tr>
                    <td>Margin:</td>
                    <td>
                        <asp:TextBox ID="txtMargin" runat="server" Width="60" MaxLength="6" style="text-align: right;"></asp:TextBox> %
                    </td>
                    <td style="padding-left: 15px;">1st Adj:</td>
                    <td>
                        <asp:TextBox ID="txtFirstAdj" runat="server" Width="60" MaxLength="6" style="text-align: right;"></asp:TextBox> %
                    </td>
                    <td style="padding-left: 15px;">Sub Adj:</td>
                    <td>
                        <asp:TextBox ID="txtSubAdj" runat="server" Width="60" MaxLength="6" style="text-align: right;"></asp:TextBox> %
                    </td>
                    <td style="padding-left: 15px;">Lifetime Cap:</td>
                    <td>
                        <asp:TextBox ID="txtLifetimeCap" runat="server" Width="60" MaxLength="6" style="text-align: right;"></asp:TextBox> %
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divCreateLoanProgram" style="display: none; padding: 5px; padding-top: 15px; padding-bottom: 15px;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>Loan Program:</td>
                <td style="padding-left: 5px;">
                    <input id="txtLoanProgram" type="text" maxlength="150" style="width: 200px;" />
                </td>
                <td style="padding-left: 5px;">
                    <label><input id="chkIsARM" type="checkbox"/> ARM</label>
                </td>
            </tr>
        </table>
        <br />
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <input id="btnCreate2" type="button" value="Save and Close" class="Btn-115" onclick="btnCreate2_onclick()" />
                </td>
                <td style="padding-left: 5px;">
                    <input id="btnCreate3" type="button" value="Save and Create Another" class="Btn-140" onclick="btnCreate3_onclick()" />
                </td>
                <td>
                    <input id="btnCancel2" type="button" value="Cancel" class="Btn-66" onclick="btnCancel2_onclick()" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divGlobalPopup" title="Global Popup" style="display: none;">
        <iframe id="ifrGlobalPopup" frameborder="0" scrolling="no" width="100px" height="100px">
        </iframe>
    </div>
    </form>
</body>
</html>