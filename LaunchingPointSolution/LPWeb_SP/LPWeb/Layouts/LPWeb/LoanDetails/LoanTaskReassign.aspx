<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanTaskReassign.aspx.cs" Inherits="LoanDetails_LoanTaskReassign" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reassign Loan Task</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {


        });

        // cancel
        function btnCancel_onclick() {

            window.parent.CloseDialog_ReassignTask();
        }

        function BeforeSave() {

            var OwnerID = $("#ddlOwner").val();
            if (OwnerID == 0) {

                alert("Please select an owner.");
                return false;
            }

            return true;
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 560px;">
        <div class="DetailsContainer">
            <div>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 56px;">
                            Borrower:
                        </td>
                        <td style="padding-left: 15px; width: 150px;">
                            <asp:Label ID="lbBorrower" runat="server" Text=""></asp:Label>
                        </td>
                        <td>
                            Property:
                        </td>
                        <td style="padding-left: 10px;">
                            <asp:Label ID="lbProperty" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 56px; vertical-align: top;">
                            <label style="position: relative; top: 3px;">Task Name:</label>
                        </td>
                        <td style="padding-left: 15px;">
                            
                            <div>
                                <table id="gridTaskNameList" border="1" cellspacing="0" cellpadding="3" style="border-collapse:collapse; border: solid 1px #F3F4F8;">
		                            <asp:Repeater ID="rptTaskNameList" runat="server">
                                        <ItemTemplate>
                                            <tr>
			                                    <td style="width:450px; border: solid 1px #f3f4f8;"><%# Eval("Name") %></td>
		                                    </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
	                            </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 56px;">
                            Owner:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlOwner" runat="server" runat="server" DataValueField="UserID" DataTextField="FullName" Width="220px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave()" onclick="btnSave_Click" />
                        </td>
                        <td style="padding-left: 8px;">
                            <input id="btnCancel" type="button" value="Cancel" class="Btn-66" onclick="btnCancel_onclick()" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    </form>
</body>
</html>