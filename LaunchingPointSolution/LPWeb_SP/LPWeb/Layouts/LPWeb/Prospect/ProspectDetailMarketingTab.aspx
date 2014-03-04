﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProspectDetailMarketingTab.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Prospect.ProspectDetailMarketingTab" DynamicMasterPageFile="~masterurl/default.master" %>

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
        Array.prototype.remove = function (s) {
            var nIndex = -1;
            for (var i = 0; i < this.length; i++) {
                if (this[i] == s) {
                    nIndex = i;
                    break;
                }
            }
            if (nIndex != -1) {
                this.splice(nIndex, 1);
                return true;
            }
            else
                return false;
        }

        function CheckAllClicked(me, areaID, hiAllIDs, hiSelectedIDs) {
            var bCheck = $(me).attr('checked');
            if (bCheck) {
                // copy all ids to selected id holder
                $('#' + hiSelectedIDs).val($('#' + hiAllIDs).val());
            }
            else
                $('#' + hiSelectedIDs).val('');
            $('input:checkbox', $('#' + areaID) + '.CheckBoxColumn').each(function () { $(this).attr('checked', bCheck); });
        }

        function CheckBoxClicked(me, ckAllID, hiAllIDs, hiSelectedIDs, id) {
            var sAllIDs = $('#' + hiAllIDs).val();
            var sSelectedIDs = $('#' + hiSelectedIDs).val();
            var allIDs = new Array();
            var selectedIDs = new Array();
            if (sAllIDs.length > 0)
                allIDs = sAllIDs.split(',');

            if (sSelectedIDs.length > 0)
                selectedIDs = sSelectedIDs.split(',');

            if ($(me).attr('checked'))
                selectedIDs.push(id);
            else
                selectedIDs.remove(id);

            // set the CheckAll check box checked status
            // $('#' + ckAllID).attr('checked', selectedIDs.length >= allIDs.length);

            if (selectedIDs.length > 0)
                $('#' + hiSelectedIDs).val(selectedIDs);
            else
                $('#' + hiSelectedIDs).val('');
        }

        function SelectedItemCount() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (null == sIds || sIds.length == 0)
                return 0;
            var arrIds = sIds.split(",");
            return arrIds.length;
        }

        function ItemInfo(sID, sStatus) {
            this.ID = sID;
            this.Status = sStatus;
        }

        function GetItemInfoById(sId) {
            var sItemInfo = $("#" + "<%=hiItemInfo.ClientID %>").val();
            var arrItemInfo = sItemInfo.split(";");
            for (var i = 0; i < arrItemInfo.length; i++) {
                var arrTemp = arrItemInfo[i].split(":");
                if (arrTemp.length == 2 && arrTemp[0] == sId) {
                    return new ItemInfo(arrTemp[0], arrTemp[1]);
                }
            }
            return null;
        }

        function IsCompletedCampaignsSelected() {
            var sIds = $("#" + "<%=hiCheckedIds.ClientID %>").val();
            if (null == sIds || sIds.length == 0)
                return false;
            var arrIds = sIds.split(",");
            // check all selected ids
            for (var n = 0; n < arrIds.length; n++) {
                var sStatus = $("#" + "<%=hiItemInfo.ClientID %>").val();
                var arrStatus = sStatus.split(";");
                // check all items
                for (var i = 0; i < arrStatus.length; i++) {
                    var arrTemp = arrStatus[i].split(":");
                    if (arrTemp.length == 2) {
                        if (arrIds[n] != arrTemp[0])
                            continue;
                        var sTemp = arrTemp[1];
                        if ("completed" == sTemp.toLowerCase())
                            return true;
                    }
                }
            }
            return false;
        }
    </script>
    <script type="text/javascript">
        var hdnCreateNotes = "#<%= hdnCreateNotes.ClientID %>";

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
            
            if ($(hdnCreateNotes).val() == "0") {
                DisableLink("btnNew");
            }

            $("#<%= btnRemove.ClientID %>").click(function () {
                if (SelectedItemCount() <= 0) {
                    alert("No record has been selected.");
                    return false;
                }
                else {
                    if (IsCompletedCampaignsSelected()) {
                        alert("You cannot remove a campaign that has been completed.");
                        return false;
                    }
                    else
                        return true;
                }
            });
        });

        function getCampaignSelectionReturn(result) {
       
         var SelID = result.substring(0, result.indexOf("^"));
            $("#<%=hfSelCampaigns.ClientID %>").val(SelID);
            if($("#<%=hfSelCampaigns.ClientID %>").val() != "")
            {
                ShowCampaignStartDate();
            }
       }

        function ShowCampaignStartDate() {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var sCurrSlider = getCurrentSliderWin();
            if ("" == sCurrSlider)
            {
                alert("Slider error.");
                return;
            }
            var sRecieveCode = "window.parent.document.getElementById('" + sCurrSlider + "').contentWindow.document.getElementById('tabFrame').contentWindow.SaveCampaignStartDate('returnValue')";
            window.parent.parent.ShowGlobalPopup("Start Date Selection", 280, 200, 320, 240, "SelectCampaignStartDate.aspx?t=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()&RecieveDataCodes=" + sRecieveCode);
            return;
        }

         function SaveCampaignStartDate(theDate) { 
            $("#<%=hfStartDate.ClientID %>").val(theDate);
            <%=this.ClientScript.GetPostBackEventReference(this.btnSaveSel, null) %>
              ShowWaitingDialog('Please wait...');
        }

        function PopupAddMarketingWindow() {
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            var sFId = $("#<%=hfFileID.ClientID %>").val();
            var sCurrSlider = getCurrentSliderWin();
            if ("" == sCurrSlider)
            {
                alert("Slider error.");
                return;
            }
            var sRecieveCode = "window.parent.document.getElementById('" + sCurrSlider + "').contentWindow.document.getElementById('tabFrame').contentWindow.getCampaignSelectionReturn('returnValue')";
            window.parent.parent.ShowGlobalPopup("Add Marketing", 610, 520, 640, 560, "../Marketing/SelectMarketingCampaignForRule.aspx?type=1&t=" + RadomStr + "&f=" + sFId + "&CloseDialogCodes=window.parent.parent.CloseGlobalPopup()&RecieveDataCodes=" + sRecieveCode);
            return;
        }

        function getCurrentSliderWin()
        {
            var sSliderFrameId = "";
            switch(window.parent.parent.idivIndex)
            {
                case 0:
                sSliderFrameId = "ifrGlider1";
                break;
                case 1:
                sSliderFrameId = "ifrGlider2";
                break;
                case 2:
                sSliderFrameId = "ifrGlider3";
                break;  
            }
            return sSliderFrameId;
        }

        function ShowLoanDetails(status,fileID)
        {
            if(status=="Active")
            {
                window.parent.parent.location.href = "../LoanDetails/LoanDetails.aspx?FromPage=<%= FromURL %>&fieldid=" + fileID + "&fieldids=" + fileID;
            }
            else
            {
                window.parent.parent.location.href = "../Prospect/ProspectLoanDetails.aspx?FromPage=<%= FromURL %>&FileID=" + fileID + "&FileIDs=" + fileID;
            }
        }

        function ShowMarketingCampaigns(CampaignId)
        {
            window.parent.parent.location.href = '../Marketing/MarketingCampaign.aspx?CampaignId=' + CampaignId;
        }

        function ShowEventContent(eventid)
        {
            if(eventid==null || eventid=="" || eventid == "undefined")
            {
                return false;
            }
            var RadomNum = Math.random();
            var RadomStr = RadomNum.toString().substr(2);
            window.parent.parent.ShowGlobalPopup("Event Content", 610, 520, 640, 560, 
                "../LoanDetails/LoanMarketingEventContent.aspx?eventid=" + eventid + "&t=" + RadomStr + "&CloseDialogCodes=window.parent.CloseGlobalPopup()");
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
    <div class="JTab" style="margin-top: 5px; border: solid 0px red; width: 1030px; margin-bottom: 20px;">
        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
            <tr>
                <td style="width: 10px;">&nbsp;</td>
                <td>
                    <div id="tabs10">
                        <ul>
                            <li id="current"><a href="javascript:window.location.href=window.location.href"><span>Leadstar</span></a></li>
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
                
                <div id="divFilters" style="margin-top: 10px;">
                    <table>
                        <tr>
                            <td style="width: 210px;">
                                <asp:DropDownList ID="ddlCampaigns" runat="server" DataValueField="CampaignName"
                                    DataTextField="CampaignName" Width="200px">
                                    <asp:ListItem Text="All Campaigns" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:DropDownList ID="ddlStatuses" runat="server" Width="120px">
                                    <asp:ListItem Text="All Statuses" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:DropDownList ID="ddlStartedBy" runat="server" DataValueField="UserId" DataTextField="StartedByUser"
                                    Width="120px">
                                    <asp:ListItem Text="Started By" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td style="width: 210px;">
                                <asp:DropDownList ID="ddlEvents" runat="server" 
                                    Width="120px">
                                    <asp:ListItem Text="All Events" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Completed" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Not Completed" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:TextBox ID="tbSentStart" runat="server" CssClass="DateField"></asp:TextBox>
                            </td>
                            <td>
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
                                            <li>
                                                <asp:LinkButton ID="lbtnAdd" runat="server" Text="Add" OnClientClick="PopupAddMarketingWindow(); return false;"></asp:LinkButton><span>|</span></li>
                                            <li>
                                                <asp:LinkButton ID="btnRemove" runat="server" Text="Remove" OnClick="btnRemove_Click"></asp:LinkButton></li>
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
                <div id="divGrid" class="ColorGrid" style="width: 1000px; margin-top: 5px;">
                    <asp:GridView ID="gridList" runat="server" DataKeyNames="LoanMarketingId,FileId,Status" EmptyDataText="There is no data information."
                        AutoGenerateColumns="False" OnRowDataBound="gridList_RowDataBound" OnPreRender="gridList_PreRender" CellPadding="3"
                        CssClass="GrayGrid" GridLines="None" OnSorting="gridList_Sorting" AllowSorting="true">
                        <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                        <AlternatingRowStyle CssClass="EvenRow" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="ckbAll" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckbSelect" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="CheckBoxHeader"></HeaderStyle>
                                <ItemStyle CssClass="CheckBoxColumn"></ItemStyle>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" SortExpression="Status" ItemStyle-Width="70px">
                                <ItemTemplate>
                                    <img src="../images/Marketing-Red.gif" title="<%# Eval("Error") %>" style="margin-right: 5px;
                                        <%# Eval("Success").ToString() == "False" ? "": "display:none;" %>" /><%# Eval("Status") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Auto" SortExpression="Auto" >
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkAuto" runat="server" Enabled="false" />
                                    <asp:TextBox ID="tbAuto" runat="server" Visible="false" Text='<%# Eval("Type")%>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CategoryName" SortExpression="CategoryName" HeaderText="Category" />
                            <asp:TemplateField HeaderText="Campaign" SortExpression="CampaignName">
                                <ItemTemplate>
                                    <a href="#" onclick="javascript:ShowMarketingCampaigns(<%# Eval("CampaignId")%>)">
                                        <%# Eval("CampaignName")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                             <asp:TemplateField HeaderText="Loan" SortExpression="Loan">
                                <ItemTemplate>
                                   <a href="#" onclick=javascript:ShowLoanDetails('<%# Eval("LoanStatus")%>',<%# Eval("FileID")%>)><%# Eval("Loan")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="StartedByUser" SortExpression="StartedByUser" HeaderText="Started By" />
                            <asp:TemplateField HeaderText="Started" SortExpression="Started" ItemStyle-Wrap="false" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <%# Eval("Started", "{0:d}")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:TemplateField>            
                            <asp:TemplateField HeaderText="Event" SortExpression="Event" ItemStyle-Wrap="false" 
                                ItemStyle-Width="120">
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
                    <div class="GridPaddingBottom">
                        &nbsp;</div>
                    <asp:HiddenField ID="hiAllIds" runat="server" />
                    <asp:HiddenField ID="hiCheckedIds" runat="server" />
                    <asp:HiddenField ID="hiItemInfo" runat="server" />
                    <asp:HiddenField ID="hfFileID" runat="server" />
                    <asp:HiddenField ID="hdnCreateNotes" runat="server" />
                    <asp:HiddenField ID="hfSelCampaigns" runat="server" />
                    <asp:HiddenField ID="hfStartDate" runat="server" />
                </div>

        </div>
        </div>
    </div>
    
    <div style="display: none">
        <asp:Button ID="btnSaveSel" runat="server" OnClick="btnSaveSel_Click" />
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
    </form>
</div>
</body>
</html>
