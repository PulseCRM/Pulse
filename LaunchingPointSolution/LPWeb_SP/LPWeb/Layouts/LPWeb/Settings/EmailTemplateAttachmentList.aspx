<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmailTemplateAttachmentList.aspx.cs" Inherits="Settings_EmailTemplateAttachmentList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Add Email Attachment</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    
    
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
// <![CDATA[

        var EmailTemplateID = GetQueryString1("EmailTemplateID");

        $(document).ready(function () {

            InitSearchInput();
        });

        function InitSearchInput() {

            // OrderByField
            var OrderByField = GetQueryString1("OrderByField");
            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByField != "") {

                if (OrderByField == "Name") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag1").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag1").text("▼");
                    }
                }
                else if (OrderByField == "FileType") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag2").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag2").text("▼");
                    }
                }
                else if (OrderByField == "Enabled") {

                    if (OrderByType == "0") {

                        $("#spOrderByFlag3").text("▲");
                    }
                    else if (OrderByType == "1") {

                        $("#spOrderByFlag3").text("▼");
                    }
                }
            }
            else {

                $("#spOrderByFlag1").text("▲");
            }
        }

        function OrderBy(field) {

            var sid = Math.random().toString().substr(2);

            // 参数字符串
            var sQueryStrings = "?EmailTemplateID=" + EmailTemplateID + "&sid=" + sid + "&OrderByField=" + field;

            var OrderByType = GetQueryString1("OrderByType");
            if (OrderByType == "") {

                sQueryStrings += "&OrderByType=1";
            }
            else if (OrderByType == "0") {

                sQueryStrings += "&OrderByType=1";
            }
            else if (OrderByType == "1") {

                sQueryStrings += "&OrderByType=0";
            }

            window.location.href = window.location.pathname + sQueryStrings;
        }

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {
                $("#" + '<%=gridAttachmentList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {
                $("#" + '<%=gridAttachmentList.ClientID %>' + " tr td input[type=checkbox]").attr("checked", "");
            }
        }

        function Delete() {

            var SelCount = $("#" + "<%=gridAttachmentList.ClientID %>" + " tr:not(:first) td input:checkbox[checked=true]").length;
            if (SelCount == 0) {

                alert("Please select an attachment.");
                return;
            }

            var result = confirm("Are you sure to continue?");
            if (result == false) {

                return;
            }

            // show waiting dialog
            window.parent.parent.ShowWaitingDialog("Deleting email attachment(s)...");

            var TemplAttachIDs = "";
            $("#" + "<%=gridAttachmentList.ClientID %>" + " tr:not(:first) td input:checkbox[checked=true]").each(function (i) {

                var TemplAttachId = $(this).attr("TemplAttachId");
                if (i == 0) {

                    TemplAttachIDs = TemplAttachId;
                }
                else {

                    TemplAttachIDs += "," + TemplAttachId;
                }
            });
            //alert(TemplAttachIDs);

            // Ajax
            var sid = Math.random().toString().substr(2);
            $.getJSON("EmailTemplateAttachmentDeleteAjax.aspx?sid=" + sid + "&TemplAttachIDs=" + TemplAttachIDs, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        window.parent.parent.CloseWaitingDialog();

                        return;
                    }

                    window.parent.parent.CloseWaitingDialog();
                    window.location.href = window.location.href;

                }, 2000);
            });
        }

        function Enabled(Enabled) {

            var SelCount = $("#" + "<%=gridAttachmentList.ClientID %>" + " tr:not(:first) td input:checkbox[checked=true]").length;
            if (SelCount == 0) {

                alert("Please select an attachment.");
                return;
            }

            if (Enabled == "true") {

                window.parent.parent.ShowWaitingDialog("Enabling email attachment(s)...");
            }
            else {

                window.parent.parent.ShowWaitingDialog("Disabling email attachment(s)...");
            }

            var TemplAttachIDs = "";
            $("#" + "<%=gridAttachmentList.ClientID %>" + " tr:not(:first) td input:checkbox[checked=true]").each(function (i) {

                var TemplAttachId = $(this).attr("TemplAttachId");
                if (i == 0) {

                    TemplAttachIDs = TemplAttachId;
                }
                else {

                    TemplAttachIDs += "," + TemplAttachId;
                }
            });
            //alert(TemplAttachIDs);

            // Ajax
            var sid = Math.random().toString().substr(2);
            $.getJSON("EmailTemplateAttachmentEnableAjax.aspx?sid=" + sid + "&TemplAttachIDs=" + TemplAttachIDs + "&Enabled=" + Enabled, function (data) {

                setTimeout(function () {

                    if (data.ExecResult == "Failed") {

                        alert(data.ErrorMsg);
                        window.parent.parent.CloseWaitingDialog();

                        return;
                    }

                    window.parent.parent.CloseWaitingDialog();
                    window.location.href = window.location.href;

                }, 2000);
            });
        }

        function ShowDialog_AddAttachment() {

            if (EmailTemplateID == "0") {

                alert("please save the email template first.");
                return;
            }

            var sid = Math.random().toString().substr(2);
            var iFrameSrc = "EmailTemplateAttachmentAdd.aspx?sid=" + sid + "&EmailTemplateID=" + EmailTemplateID;

            var BaseWidth = 470;
            var iFrameWidth = BaseWidth + 2;
            var divWidth = iFrameWidth + 25;

            var BaseHeight = 210;
            var iFrameHeight = BaseHeight + 2;
            var divHeight = iFrameHeight + 40;

            window.parent.parent.ShowDialog("Email Attachment", iFrameWidth, iFrameHeight, divWidth, divHeight, iFrameSrc);
        }

// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    
    <div id="divContainer">
                
        <div id="divToolBar" style="margin-top: 10px;">
            <ul class="ToolStrip" style="margin-left: 5px;">
                <li><a href="javascript:ShowDialog_AddAttachment()">Add</a><span>|</span></li>
                <li><a href="javascript:Delete()">Delete</a><span>|</span></li>
                <li><a href="javascript:Enabled('true')">Enable</a><span>|</span></li>
                <li><a href="javascript:Enabled('false')">Disable</a></li>
            </ul>                
        </div>

        <div id="divAttachmentList" class="ColorGrid" style="width:500px;">
            <asp:GridView ID="gridAttachmentList" runat="server" EmptyDataText="There is no attachment." AutoGenerateColumns="False" CellPadding="3" 
                CssClass="GrayGrid" GridLines="None">
                <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                        <HeaderTemplate>
                            <input id="chkCheckAll" type="checkbox" onclick="CheckAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <input id="chkSelect" type="checkbox" TemplAttachId="<%# Eval("TemplAttachId") %>" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('Name')" style="text-decoration: underline;">Name</a><span id="spOrderByFlag1" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="EmailTemplateAttachmentShowFile.aspx?TemplAttachId=<%# Eval("TemplAttachId") %>" target="_blank"><%# Eval("Name")%></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-Width="80px" >
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('FileType')" style="text-decoration: underline;">Type</a><span id="spOrderByFlag2" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("FileType")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-Width="80px" >
                        <HeaderTemplate>
                            <a href="javascript:OrderBy('Enabled')" style="text-decoration: underline;">Enabled</a><span id="spOrderByFlag3" style="margin-left: 3px;"></span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# Eval("Enabled").ToString() == "True" ? "Yes" : "No"%>
                        </ItemTemplate>
                    </asp:TemplateField>
                            
                </Columns>
            </asp:GridView>
            <div class="GridPaddingBottom">&nbsp;</div>
        </div>
                
    </div>

    

    

    </form>

</body>
</html>