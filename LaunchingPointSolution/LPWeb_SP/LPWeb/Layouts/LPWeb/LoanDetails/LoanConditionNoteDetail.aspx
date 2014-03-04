<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanConditionNoteDetail.aspx.cs" Inherits="LoanDetails_LoanConditionNoteDetail" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Condition Note Detail</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
// <![CDATA[

        $(document).ready(function () {

            $("#txtNote").maxlength(4000);
        });

        function btnCancel_onclick() {

            window.top.CloseGlobalPopup();
        }

        function aSelectAll_onclick() {

            if ($("#aSelectAll").text() == "Select All") {

                $("#tbNoteList tr td :checkbox").attr("checked", "true");
                $("#aSelectAll").text("de-select");
            }
            else {

                $("#tbNoteList tr td :checkbox").removeAttr("checked");
                $("#aSelectAll").text("Select All");
            }

        }


        function BeforeEnable() {

            if ($("#tbNoteList tr td :checkbox:checked").length == 0) {

                alert("Please select a note first.");
                return false;
            }

            var NoteIDs = "";
            $("#tbNoteList tr td :checkbox:checked").each(function (i) {

                var noteid = $(this).attr("noteid");
                if (i == 0) {

                    NoteIDs = noteid;
                }
                else {

                    NoteIDs += "," + noteid;
                }
            });

            $("#hdnNoteIDs").val(NoteIDs);
            //alert($("#hdnNoteIDs").val());
            if ($("#hdnNoteIDs").val() == "") {

                alert("Please select a note first.");
                return false;
            }

            return true;
        }


// ]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer">
        <div class="margin-top-10">
            <input id="btnCancel" type="button" value="Close" class="Btn-66" onclick="btnCancel_onclick()" />
        </div>
        <div class="margin-top-10">
            <table cellpadding="5" cellspacing="5">
                <tr>
                    <td style="width: 250px;">Borrower: <asp:Label ID="lbBorrower" runat="server" Text="Label"></asp:Label></td>
                    <td style="width: 450px;">Property Address: <asp:Label ID="lbPropertyAddress" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td>Point Folder: <asp:Label ID="lbPointFolder" runat="server" Text="Label"></asp:Label></td>
                    <td>Point filename: <asp:Label ID="lbPointfilename" runat="server" Text="Label"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2">Condition Name: <asp:Label ID="lbConditionName" runat="server" Text="Label"></asp:Label></td>
                </tr>
                
            </table>
            
        </div>
        <div id="divToolBar" style="margin-top: 10px; ">
            <ul class="ToolStrip" style="margin-left: 0px;">
                <li><a id="aSelectAll" href="javascript:aSelectAll_onclick()">Select All</a><span>|</span></li>
                <li><asp:LinkButton ID="lnkEnable" runat="server" OnClientClick="return BeforeEnable()" OnClick="lnkEnable_Click">Enable External Viewing</asp:LinkButton><span>|</span></li>
                <li><asp:LinkButton ID="lnkDisable" runat="server" OnClientClick="return BeforeEnable()" OnClick="lnkDisable_Click">Disable External Viewing</asp:LinkButton></li>
            </ul>
        </div>
        <div style="height: 235px; overflow: auto;">
            <table id="tbNoteList" cellpadding="6" cellspacing="0" border="1" style="border-collapse: collapse; border-bottom: none; border-right: none;">
                
                <asp:Repeater ID="rptNoteList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="width: 13px;">
                                <input id="chkSelect" type="checkbox" noteid="<%# Eval("NoteId") %>" />
                            </td>
                            <td>
                                <table cellpadding="3" cellspacing="3">
                                    <tr>
                                        <td style="width: 400px;">
                                            <h6><%# Eval("Created").ToString() == string.Empty ? string.Empty : Convert.ToDateTime(Eval("Created")).ToString("MM/dd/yyyy hh:mm:ss") %> by <%# Eval("Sender")%></h6>
                                        </td>
                                        <td style="width: 465px;">
                                            External Viewing = <%# Eval("ExternalViewing").ToString() == "True" ? "Yes" : "No" %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="line-height: 1.5;">
                                            <%# Eval("Note")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                
            </table>
        </div>
    </div>
    <asp:HiddenField ID="hdnNoteIDs" runat="server" />
    </form>
</body>
</html>