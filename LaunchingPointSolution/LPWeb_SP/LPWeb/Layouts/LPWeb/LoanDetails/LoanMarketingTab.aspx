<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoanMarketingTab.aspx.cs" Inherits="LoanMarketingTab" %>

<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
   <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.treeview.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.tablesorter.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.datepick.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jquery.treeview.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.tablesorter.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/date.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.progressbar.min.js"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.base64.js" type="text/javascript"></script>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <style type="text/css">
        div.ColorGrid table.GrayGrid th
        {
            text-align: center;
        }
    </style>
   
    <script type="text/javascript">
        var hdnCreateNotes = "#<%= hdnCreateNotes.ClientID %>";
        var allLoan = new Array();
        var allSelectedLoan = new Array();
        var gridId = "#<%=gridList.ClientID %>";

        Array.prototype.removeLoan = function (s) {
            var index = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i].ID == s) {
                    index = i;
                    break;
                }
            }
            if (index >= 0) {
                this.splice(index, 1);
                return true;
            }
            return false;
        };

        $(document).ready(function () {
            
            DrawTab();
            
            var startDate = $("#" + '<%=tbSentStart.ClientID %>');
            var endDate = $("#" + '<%=tbSentEnd.ClientID %>');
            var campaignDate = $("#tbxStartDate");
            startDate.datepick();
            endDate.datepick();
            campaignDate.datepick();
            startDate.attr("readonly", "true");
            endDate.attr("readonly", "true");
            campaignDate.attr("readonly", "true");

            var checkAll = $(gridId + " :checkbox:eq(0)");
            checkAll.click(function () {
                var allStatus = checkAll.attr("checked");
                $(gridId + " tr td.CheckBoxColumn :checkbox").each(function () {
                    $(this).attr("checked", allStatus);
                });
                getSelectedItems();
            });

            if ($(hdnCreateNotes).val() == "0") {
                // $("#btnNew").removeAttr('href');
                DisableLink("btnNew");
            }

            $("#<%= btnRemove.ClientID %>").click(function () {
                if ($("#<%=hfDeleteItems.ClientID %>").val() == "") {
                    alert("No record has been selected.");
                    return false;
                }
                else {

                    var sStauts = $("#<%=hfSelStatus.ClientID %>").val();
                    if (sStauts.indexOf("Completed") > 0) {
                        alert("You cannot remove a campaign that has been completed.");
                        return false;
                    }
                    return true;
                }
            });
        });
               
        function ClosePopupWindow() {
            $("#divAddMarketing").dialog("close");
            window.location.href = window.location.href;
        }

        function getCampaignSelectionReturn(result) {
             var SelID = result.substring(0, result.indexOf("^"));
            $("#<%=hfSelCampaigns.ClientID %>").val(SelID);
            $("#ifrMarketingAdd").dialog("destroy");
            if($("#<%=hfSelCampaigns.ClientID %>").val() != "")
            {                
                $("#divAddMarketing").dialog("close");
                ShowCampaignStartDate();
            }
        }

        function PopupAddMarketingWindow() {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var sFId = $("#<%=hfFileID.ClientID %>").val();
            $("#ifrMarketingAdd").attr("src", "../Marketing/SelectMarketingCampaignForRule.aspx?type=1&t=" + RadomStr + "&f=" + sFId + "&CloseDialogCodes=window.parent.ClosePopupWindow()").show();
            $("#divAddMarketing").dialog({
                height: 560,
                width: 640,
                modal: true,
                resizable: false
            });
        }

        function AddMarketing() {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var f = document.getElementById('ifrMarketingAdd');
            f.src = "../Marketing/SelectMarketingCampaignForRule.aspx?type=1&t=" + RadomStr ;
            $('#divAddMarketing').dialog({
                height: 560,
                width: 640,
                modal: true,
                resizable: false
            });
        }

        function ShowEventContent(eventid)
        {
            if(eventid==null || eventid=="" || eventid == "undefined")
            {
                return false;
            }
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);

            var f = document.getElementById('ifrShowEventContent');
            f.src = "LoanMarketingEventContent.aspx?eventid=" + eventid + "&t=" + RadomStr;
            $('#divShowEventContent').dialog({
                height: 560,
                width: 640,
                modal: true,
                resizable: false
            });
        }

        // Start: add by peter, for Dispose button 2010-11-15
        function onCkAllRowSelected(me) {
            var bCheck = $(me).attr('checked');
            if (bCheck)
//                allSelectedLoan = allLoan; // copy all allLoan items to allSelectedLoan
                getSelectedItems();
            else
                allSelectedLoan = new Array();
            $('input:checkbox', $('#' + '<%=gridList.ClientID %>')).each(function () { $(this).attr('checked', bCheck); });
        }
        function onCkRowClicked(me, sID, sStatus) {

            if ($(me).attr("checked"))
                allSelectedLoan.push(new SelectedLoan(sID, sStatus));
            else
                allSelectedLoan.removeLoan(sID);
        }

        function SelectedLoan(sID, sStatus) {
            this.ID = sID;
            this.Status = sStatus;
            $("#<%=hfDeleteItems.ClientID %>").val(sID);
            $("#<%=hfSelStatus.ClientID %>").val(sStatus);
        }

        function getSelectedItems() {
            var selctedItems = new Array();
            var selStatus = new Array();
            $(gridId + " :checkbox:gt(0)").each(function () {
                var item = $(this);
                if (item.attr("checked")) {
                    selctedItems.push(item.attr("tag"));
                    selStatus.push(item.attr("MType"));
                }
            });
//            alert(selctedItems.join(","));
            $("#<%=hfDeleteItems.ClientID %>").val(selctedItems.join(","));
            $("#<%=hfSelStatus.ClientID %>").val(selStatus.join(","));
             
        }

        function ShowMarketingCampaigns(CampaignId)
        {
            window.parent.parent.location.href = '../Marketing/MarketingCampaign.aspx?CampaignId=' + CampaignId;
        }

        function ShowCampaignStartDate() {
            $("#divStartDate").dialog({
                height: 120,
                width: 280,
                modal: true,
                title: "",
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa")
        }

        function SaveCampaignStartDate() { 
            $("#<%=hfStartDate.ClientID %>").val($("#tbxStartDate").val());
            
            if($("#tbxStartDate").val() == "")
            {
                alert("Please enter start date.");
                return false;
            }
            
            <%=this.ClientScript.GetPostBackEventReference(this.btnSaveSel, null) %>
            ShowWaitingDialog('Saving...');
        }

        function showUpdateCardWin()
        {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            window.parent.parent.ShowGlobalPopup("Update Card and Add Balance", 610, 520, 640, 560, "../Marketing/MarketingAccountUpdateCardPopup.aspx?t=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()");
        }

        function ShowWaitingDialog(WaitingMsg) {

            $("#divStartDate").dialog("close");
            $("#imgWaiting").show();
            $("#imgTick").hide();
            $("#WaitingMsg").text(WaitingMsg);
            $("#aClose").hide();

            //$.blockUI({ message: $('#divWaiting'), css: { width: '450px'} });
            $("#divContainer").block({ message: $('#divWaiting'), css: { width: '450px'} });
        }

      
        function CloseWaitingDialog(SuccessMsg) {

            $('#imgWaiting').hide();
            $('#imgTick').show();
            $('#WaitingMsg').text(SuccessMsg);
            $('#aClose').show();
        }


        function RefreshPage() {

            $("#divWaiting").dialog("close");
            window.parent.parent.CloseGlobalPopup();
            window.location.href = window.location.href;
        }

        function GoToMailChimp() {

            var FileID = GetQueryString1("FileID");

            var RadomNum = Math.random();
            var Radom = RadomNum.toString().substr(2);

            window.location.href = "../LoanDetails/LoanDetailMailChimpTab.aspx?sid=" + Radom + "&FileID=" + FileID;
        }

    </script>
</head>
<body style="width: 700px">
<div id="divContainer" style="width: 980px; height:600px;">
        
    <form id="form1" runat="server">
    <div class="JTab" style="margin-top: 5px; border: solid 0px red; width: 1000px; margin-bottom: 20px;">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">&nbsp;</td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <%--<li id="current"><a href="javascript:window.location.href=window.location.href"><span>Leadstar</span></a></li>--%>
                            <li><a href="javascript:GoToMailChimp()"><span>Mail Chimp</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine">&nbsp;</div>
                <div id="TabLine2" class="TabRightLine">&nbsp;</div>
                <div class="TabContent" style="padding: 10px;">
                    
                    <div style="padding-left: 0px; padding-right: 10px;">
                        <div id="divFilters">
                            <table>
                                <tr>
                                    <td style="width: 310px;">
                                        <asp:DropDownList ID="ddlCampaigns" runat="server" DataValueField="CampaignName" DataTextField="CampaignName" Width="200px">
                                            <asp:ListItem Text ="All Campaigns" Value="0" ></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                     <td style="width: 310px;">
                                        <asp:DropDownList ID="ddlStatuses" runat="server"  Width="120px">
                                            <asp:ListItem Text="All Statuses" Value="0" ></asp:ListItem>
                                             <asp:ListItem Text="Active" Value="Active" ></asp:ListItem>
                                              <asp:ListItem Text="Completed" Value="Completed" ></asp:ListItem>
                                               <asp:ListItem Text="Inactive" Value="Inactive" ></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                     <td style="width: 310px;">
                                        <asp:DropDownList ID="ddlStartedBy" runat="server" DataValueField="UserId" DataTextField="StartedByUser" Width="120px">
                                            <asp:ListItem Text="Started By" Value="0" ></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 310px;">
                                        <asp:DropDownList ID="ddlEvents" runat="server" DataValueField="UserId" DataTextField="StartedByUser" Width="120px">
                                            <asp:ListItem Text="All Events" Value="-1" Selected="True" ></asp:ListItem>
                                            <asp:ListItem Text="Completed" Value="1" ></asp:ListItem>
                                            <asp:ListItem Text ="Not Completed" Value ="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td >
                                        <asp:TextBox ID="tbSentStart" runat="server" CssClass="DateField"></asp:TextBox>
                                    </td>
                                    <td >
                                        <asp:TextBox ID="tbSentEnd" runat="server" CssClass="DateField"></asp:TextBox>
                                    </td>
                                    <td style="padding-left: 15px;">
                                         <asp:Button ID="btnFilter" runat="server" Text="Filter" class="Btn-66" OnClick="btnFilter_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div style="padding-left: 10px; padding-right: 10px;">
                            <div id="divToolBar" style="margin-top: 10px;">
                                <table cellpadding="0" cellspacing="0" border="0" style="width: 900px;">
                                    <tr>
                                        <td style="width: 300px;">
                                            <div id="div1">
                                                <ul class="ToolStrip" style="margin-left: 0px;">
                                                    <li><a id="btnNew" runat="server"  style="cursor:hand" onclick="PopupAddMarketingWindow()">Add</a><span>|</span></li>
                                                    <li><asp:LinkButton ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click"></asp:LinkButton></li>
                                                </ul>
                                                <div id="menu1" class="contextMenu" style="display: none;">
                                                    <ul>
                                                        <%=sLoanType %>
                                                    </ul>
                                                </div>
                                            </div>
                                        </td>
                                        <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
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
                        </div>
                    </div>
                    <div id="divGrid" class="ColorGrid" style="width: 900px; margin-top: 5px;">
                        <asp:GridView ID="gridList" runat="server" DataKeyNames="FileId"  OnSorting="gridList_Sorting"
                            EmptyDataText="There is no data information." AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound"
                             CellPadding="3" CssClass="GrayGrid" GridLines="None"
                            AllowSorting="true">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate>
                                       <input type="checkbox"  />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input type="checkbox" tag='<%# Eval("LoanMarketingId") %>' MType="<%# Eval("Status") %>" onclick="onCkRowClicked(this, '<%# Eval("LoanMarketingId") %>', '<%# Eval("Status") %>')" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                    <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-Width="70px">
                                    <ItemTemplate>
                                        <img src="../images/Marketing-Red.gif" title="<%# Eval("Error") %>" style="margin-right: 5px; <%# Eval("Success").ToString() == "False" ? "" : "display:none;" %>" /><%# Eval("Status") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Auto" SortExpression="Auto">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAuto" runat="server"  Enabled="false" />
                                        <asp:TextBox ID="tbAuto" runat="server" Visible="false" Text='<%# Eval("Type")%>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" ></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Campaign" SortExpression="CampaignName" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                       <a href="#" onclick="javascript:ShowMarketingCampaigns(<%# Eval("CampaignId")%>)" style="text-align:left;"><%# Eval("CampaignName")%></a>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" ></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="StartedByUser" SortExpression="StartedByUser" HeaderText="Started By" />
                               <asp:TemplateField HeaderText="Started" SortExpression="Started" ItemStyle-Wrap="false" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <%# Eval("Started", "{0:d}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Event" SortExpression="Event" ItemStyle-Wrap="false" ItemStyle-Width="120">
                                    <ItemTemplate>
                                        <span style=" display:<%# IsShow(Eval("LoanMarketingEventId")) %>;">
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td style="border: 0px;">
                                                        <a href="javascritp:void(0);" onclick="ShowEventContent(<%# Eval("LoanMarketingEventId")%>);return false;" >
                                                            <%# GetEventImg(Eval("Action"))%>
                                                        </a>
                                                    </td>
                                                    <td style="padding-left: 3px; border: 0px;">
                                                       Week <%# Eval("WeekNo")%> - <%# Eval("Action")%>
                                                       <%--call.jpg  Email-New.gif --%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ExecutionDate" SortExpression="ExecutionDate" HeaderText="Execution Date" />
                                <asp:TemplateField SortExpression="Completed" HeaderText="Completed" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkCompleted" runat="server" AutoPostBack="true" ToolTip='<%# Eval("LoanMarketingEventId")%>'  Enabled="false" OnCheckedChanged="chkCompleted_Click" />
                                        <asp:TextBox ID="tbCompleted" runat="server" Visible="false" Text='<%# Eval("Completed")%>' ToolTip='<%# Eval("Action")%>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="GridPaddingBottom">&nbsp;</div>
                    </div>

                </div>
            </div>
        </div>
    
    <asp:HiddenField ID="hdnCreateNotes" runat="server" />
    <asp:HiddenField ID="hfDeleteItems" runat="server" />
     <asp:HiddenField ID="hfSelStatus" runat="server" />
     <asp:HiddenField ID="hfSelCampaigns" runat="server" />
     <asp:HiddenField ID="hfFileID" runat="server" />
     <asp:HiddenField ID="hfStartDate" runat="server" />
     <div style="display:none">
        <asp:Button ID="btnSaveSel" runat="server" OnClick="btnSaveSel_Click" />
     </div>
     <div id="divStartDate" style="display:none">
        <br />
        <div style="text-align: center;">
        Campaign start date:&nbsp;&nbsp;<input type="text" ID="tbxStartDate" CssClass="DateField" Width="70px"/>
        <br /><br />
            <input id="btnOK" type="button" value="Yes" class="Btn-66" onclick="return SaveCampaignStartDate()" />&nbsp;&nbsp;
        </div>
    </div>
     <div id="divWaiting" style="display: none; padding: 5px;">
        <table style="margin-left: auto; margin-right: auto;">
            <tr>
                <td>
                    <img id="imgWaiting" src="../images/waiting.gif" /><img id="imgTick" src="../images/tick.png" />
                </td>
                <td style="padding-left: 5px; width:320px;">
                    <label id="WaitingMsg" style="color: #818892; font-weight: bold;">
                    </label>
                    &nbsp;&nbsp; <a id="aClose" href="javascript:RefreshPage()" style="font-weight: bold;
                        color: #6182c1;">[Close]</a>
                </td>
            </tr>
        </table>
       </div>
     <div id="divAddMarketing" title="Add Marketing" style="display: none;">
        <iframe id="ifrMarketingAdd" frameborder="0" scrolling="no" width="580px" height="500px">
        </iframe>
    </div>
    <div id="divShowEventContent" title="Event Content" style="display: none;">
        <iframe id="ifrShowEventContent" frameborder="0" scrolling="no" width="580px" height="500px">
        </iframe>
        </div>
    </form>
    </div>
</body>
</html>
