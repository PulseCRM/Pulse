<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserGoalsSetup.aspx.cs"
    Inherits="LPWeb.Settings.UserGoalsSetup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <script src="../js/jquery.plugin.contextmenu.js" type="text/javascript"></script>
    <title>User Goals Setup</title>
    <style type="text/css">
        .txtArea
        {
            display: none;
            width: 60px;
            text-align: right;
            float: right;
        }
        .disArea
        {
            text-align: right;
            display: block;
            margin-bottom: 2px;
            margin-top: 2px;
            height: 16px;
            width: 80px;    /*bug 425 Modify by Alex*/
            float: right;
        }
        .validSign
        {
            padding-top: 4px;
            float: left;
            color: Red;
            display: none;
        }
    </style>
    <script type="text/javascript">
// <![CDATA[
        function FstMonth() {
            this.L_D = "";
            this.L_I = "";
            this.M_D = "";
            this.M_I = "";
            this.H_D = "";
            this.H_I = "";
        }
        function SndMonth() {
            this.L_D = "";
            this.L_I = "";
            this.M_D = "";
            this.M_I = "";
            this.H_D = "";
            this.H_I = "";
        }
        function TrdMonth() {
            this.L_D = "";
            this.L_I = "";
            this.M_D = "";
            this.M_I = "";
            this.H_D = "";
            this.H_I = "";
        }
        function PrevQuarter() {
            this.UId = "";
            // save previous quarter user goals data
            this.FstM = new FstMonth();
            this.SndM = new SndMonth();
            this.TrdM = new TrdMonth();
        }
        var userPrevQuarterData = [];
        function UserCtlId() {
            this.UId = "";
            // save current user goals data input contorl id
            this.FstM = new FstMonth();
            this.SndM = new SndMonth();
            this.TrdM = new TrdMonth();
        }
        var currUserCtlId = [];

        $(document).ready(function () {
            $("#" + '<%=btnCopy.ClientID %>').contextMenu("menu1", {
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
                    cPrevQua: copyFromPrevQua,
                    cf2t: copyFirst2Second,
                    ct2t: copySecond2Third
                }
            });
        });

        // copy previous quarter user goals info to this quarter, 1->2, 2->3, 3->4, 4->1
        function copyFromPrevQua() {
            for (var i = 0; i < currUserCtlId.length; i++) {
                // get array index of userPrevQuarterData which belong to current user
                var index = -1;
                for (var j = 0; j < userPrevQuarterData.length; j++) {
                    if (userPrevQuarterData[j].UId == currUserCtlId[i].UId) {
                        index = j;
                        break;
                    }
                }
                if (index != -1) {
                    $("#" + currUserCtlId[i].FstM.L_D).text(userPrevQuarterData[index].FstM.L_D);
                    $("#" + currUserCtlId[i].FstM.L_I).val(userPrevQuarterData[index].FstM.L_I);
                    $("#" + currUserCtlId[i].FstM.M_D).text(userPrevQuarterData[index].FstM.M_D);
                    $("#" + currUserCtlId[i].FstM.M_I).val(userPrevQuarterData[index].FstM.M_I);
                    $("#" + currUserCtlId[i].FstM.H_D).text(userPrevQuarterData[index].FstM.H_D);
                    $("#" + currUserCtlId[i].FstM.H_I).val(userPrevQuarterData[index].FstM.H_I);

                    $("#" + currUserCtlId[i].SndM.L_D).text(userPrevQuarterData[index].SndM.L_D);
                    $("#" + currUserCtlId[i].SndM.L_I).val(userPrevQuarterData[index].SndM.L_I);
                    $("#" + currUserCtlId[i].SndM.M_D).text(userPrevQuarterData[index].SndM.M_D);
                    $("#" + currUserCtlId[i].SndM.M_I).val(userPrevQuarterData[index].SndM.M_I);
                    $("#" + currUserCtlId[i].SndM.H_D).text(userPrevQuarterData[index].SndM.H_D);
                    $("#" + currUserCtlId[i].SndM.H_I).val(userPrevQuarterData[index].SndM.H_I);

                    $("#" + currUserCtlId[i].TrdM.L_D).text(userPrevQuarterData[index].TrdM.L_D);
                    $("#" + currUserCtlId[i].TrdM.L_I).val(userPrevQuarterData[index].TrdM.L_I);
                    $("#" + currUserCtlId[i].TrdM.M_D).text(userPrevQuarterData[index].TrdM.M_D);
                    $("#" + currUserCtlId[i].TrdM.M_I).val(userPrevQuarterData[index].TrdM.M_I);
                    $("#" + currUserCtlId[i].TrdM.H_D).text(userPrevQuarterData[index].TrdM.H_D);
                    $("#" + currUserCtlId[i].TrdM.H_I).val(userPrevQuarterData[index].TrdM.H_I);
                }
            }
        }
        function copyFirst2Second() {
            for (var i = 0; i < currUserCtlId.length; i++) {
                $("#" + currUserCtlId[i].SndM.L_D).text($("#" + currUserCtlId[i].FstM.L_D).text());
                $("#" + currUserCtlId[i].SndM.L_I).val($("#" + currUserCtlId[i].FstM.L_I).val());
                $("#" + currUserCtlId[i].SndM.M_D).text($("#" + currUserCtlId[i].FstM.M_D).text());
                $("#" + currUserCtlId[i].SndM.M_I).val($("#" + currUserCtlId[i].FstM.M_I).val());
                $("#" + currUserCtlId[i].SndM.H_D).text($("#" + currUserCtlId[i].FstM.H_D).text());
                $("#" + currUserCtlId[i].SndM.H_I).val($("#" + currUserCtlId[i].FstM.H_I).val());
            }
        }
        function copySecond2Third() {
            for (var i = 0; i < currUserCtlId.length; i++) {
                $("#" + currUserCtlId[i].TrdM.L_D).text($("#" + currUserCtlId[i].SndM.L_D).text());
                $("#" + currUserCtlId[i].TrdM.L_I).val($("#" + currUserCtlId[i].SndM.L_I).val());
                $("#" + currUserCtlId[i].TrdM.M_D).text($("#" + currUserCtlId[i].SndM.M_D).text());
                $("#" + currUserCtlId[i].TrdM.M_I).val($("#" + currUserCtlId[i].SndM.M_I).val());
                $("#" + currUserCtlId[i].TrdM.H_D).text($("#" + currUserCtlId[i].SndM.H_D).text());
                $("#" + currUserCtlId[i].TrdM.H_I).val($("#" + currUserCtlId[i].SndM.H_I).val());
            }
        }
        function closeBox() {
            self.parent.closeUserGoalsSetupWin();
            return false;
        }
        // show text box for user input
        function onInput(disId, txtId) {
            var txt = $("#" + txtId);
            var dis = $("#" + disId);
            $(dis).css("display", "none");
            $(txt).css("display", "block");
            $(txt).focus();
        }

        // check input box
        function onIBlur(uid, month, goals) {
            // Check user input
            var index = -1;
            for (var i = 0; i < currUserCtlId.length; i++) {
                if (uid == currUserCtlId[i].UId) {
                    index = i;
                    break;
                }
            }
            if (-1 != index) {
                switch (month) {
                    case "1":
                        switch (goals) {
                            case "L":
                                return testInput(currUserCtlId[index].FstM, 1);
                            case "M":
                                return testInput(currUserCtlId[index].FstM, 2);
                            case "H":
                                return testInput(currUserCtlId[index].FstM, 3);
                        }
                        break;
                    case "2":
                        switch (goals) {
                            case "L":
                                return testInput(currUserCtlId[index].SndM, 1);
                            case "M":
                                return testInput(currUserCtlId[index].SndM, 2);
                            case "H":
                                return testInput(currUserCtlId[index].SndM, 3);
                        }
                        break;
                    case "3":
                        switch (goals) {
                            case "L":
                                return testInput(currUserCtlId[index].TrdM, 1);
                            case "M":
                                return testInput(currUserCtlId[index].TrdM, 2);
                            case "H":
                                return testInput(currUserCtlId[index].TrdM, 3);
                        }
                        break;
                }
            }
        }
        var bIsValidText = true;
        function testInput(arrInputIds, currInputIndex) {
            var reg = RegExp("^[0-9]+(.[0-9]{0,2})?$");
            var currInput = "";
            var currValue = "";
            switch (currInputIndex) {
                case 1:
                    currInput = arrInputIds.L_I;
                    break;
                case 2:
                    currInput = arrInputIds.M_I;
                    break;
                case 3:
                    currInput = arrInputIds.H_I;
                    break;
            }
            currValue = $("#" + currInput).val();
            // if valid decimal
            if (reg.test(currValue)) {
                showSign($("#" + currInput), false);
                bIsValidText = true;
                return true;
            }
            else {
                // show error sign
                bIsValidText = false;
                showSign($("#" + currInput), true);
                alert("Please input a valid digital number!");
                return false;
            }
        }
        function showSign(obj, isShow) {
            var parent = obj.parent()[0];
            if (null != parent) {
                if (isShow)
                    $(parent).children(".validSign").show();
                else
                    $(parent).children(".validSign").hide();
            }
        }
        function checkBeforeSave() {
            if (!bIsValidText) {
                alert("Please input a valid digital number!");
                return false;
            }
            var bPass = false;
            for (var i = 0; i < currUserCtlId.length; i++) {
                var dFst = new Number($("#" + currUserCtlId[i].FstM.L_I).val());
                var dSnd = new Number($("#" + currUserCtlId[i].FstM.M_I).val());
                var dTrd = new Number($("#" + currUserCtlId[i].FstM.H_I).val());
                if (doCheck(dFst, dSnd, dTrd)) {
                    // show error sign
                    showSign($("#" + currUserCtlId[i].FstM.L_I), false);
                    showSign($("#" + currUserCtlId[i].FstM.M_I), false);
                    showSign($("#" + currUserCtlId[i].FstM.H_I), false);
                    bPass = true;
                }
                else {
                    showSign($("#" + currUserCtlId[i].FstM.L_I), true);
                    showSign($("#" + currUserCtlId[i].FstM.M_I), true);
                    showSign($("#" + currUserCtlId[i].FstM.H_I), true);
                    bPass = false;
                    break;
                }
                dFst = new Number($("#" + currUserCtlId[i].SndM.L_I).val());
                dSnd = new Number($("#" + currUserCtlId[i].SndM.M_I).val());
                dTrd = new Number($("#" + currUserCtlId[i].SndM.H_I).val());
                if (doCheck(dFst, dSnd, dTrd)) {
                    // show error sign
                    showSign($("#" + currUserCtlId[i].SndM.L_I), false);
                    showSign($("#" + currUserCtlId[i].SndM.M_I), false);
                    showSign($("#" + currUserCtlId[i].SndM.H_I), false);
                    bPass = true;
                }
                else {
                    showSign($("#" + currUserCtlId[i].SndM.L_I), true);
                    showSign($("#" + currUserCtlId[i].SndM.M_I), true);
                    showSign($("#" + currUserCtlId[i].SndM.H_I), true);
                    bPass = false;
                    break;
                }
                dFst = new Number($("#" + currUserCtlId[i].TrdM.L_I).val());
                dSnd = new Number($("#" + currUserCtlId[i].TrdM.M_I).val());
                dTrd = new Number($("#" + currUserCtlId[i].TrdM.H_I).val());
                if (doCheck(dFst, dSnd, dTrd)) {
                    showSign($("#" + currUserCtlId[i].TrdM.L_I), false);
                    showSign($("#" + currUserCtlId[i].TrdM.M_I), false);
                    showSign($("#" + currUserCtlId[i].TrdM.H_I), false);
                    bPass = true;
                }
                else {
                    showSign($("#" + currUserCtlId[i].TrdM.L_I), true);
                    showSign($("#" + currUserCtlId[i].TrdM.M_I), true);
                    showSign($("#" + currUserCtlId[i].TrdM.H_I), true);
                    bPass = false;
                    break;
                }
            }
            if (!bPass) {
                alert("The value for the Low range field should be smaller than the Medium range, and the Medium range should be smaller than the High range.");
                return false;
            }
            else
                return true;
        }
        function doCheck(d1, d2, d3) {
            if (isNaN(d1) || isNaN(d2) || isNaN(d3))
                return false;
            else {
                if (d1 < d2 && d2 < d3)
                    return true;
                else
                    return false;
            }
        }
        function clearInput() {
            if (confirm('This will clear the numbers on the screen. Are you sure you want to continue?')) {
                for (var i = 0; i < currUserCtlId.length; i++) {
                    $("#" + currUserCtlId[i].FstM.L_D).text("");
                    $("#" + currUserCtlId[i].FstM.L_I).val("");
                    $("#" + currUserCtlId[i].FstM.M_D).text("");
                    $("#" + currUserCtlId[i].FstM.M_I).val("");
                    $("#" + currUserCtlId[i].FstM.H_D).text("");
                    $("#" + currUserCtlId[i].FstM.H_I).val("");

                    $("#" + currUserCtlId[i].SndM.L_D).text("");
                    $("#" + currUserCtlId[i].SndM.L_I).val("");
                    $("#" + currUserCtlId[i].SndM.M_D).text("");
                    $("#" + currUserCtlId[i].SndM.M_I).val("");
                    $("#" + currUserCtlId[i].SndM.H_D).text("");
                    $("#" + currUserCtlId[i].SndM.H_I).val("");

                    $("#" + currUserCtlId[i].TrdM.L_D).text("");
                    $("#" + currUserCtlId[i].TrdM.L_I).val("");
                    $("#" + currUserCtlId[i].TrdM.M_D).text("");
                    $("#" + currUserCtlId[i].TrdM.M_I).val("");
                    $("#" + currUserCtlId[i].TrdM.H_D).text("");
                    $("#" + currUserCtlId[i].TrdM.H_I).val("");
                }
            }
        }
// ]]>
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="aspnetForm">
        <div class="Heading">
            User Goals Setup</div>
        <div class="SplitLine">
        </div>
        <div class="DetailsContainer">
            <div>
                <table>
                    <tr>
                        <td>
                            Months:
                        </td>
                        <td style="padding-left: 15px;">
                            <asp:DropDownList ID="ddlMonths" runat="server" Width="187px" AutoPostBack="true"
                                OnSelectedIndexChanged="ddlMonths_SelectedIndexChanged">
                                <asp:ListItem Text="January, February and March" Value="1,2,3"></asp:ListItem>
                                <asp:ListItem Text="April, May and June" Value="4,5,6"></asp:ListItem>
                                <asp:ListItem Text="July, August and September" Value="7,8,9"></asp:ListItem>
                                <asp:ListItem Text="October, November and December" Value="10,11,12"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="div1" class="ColorGrid" style="margin-top: 10px;">
                <table class="GrayGrid" cellspacing="0" cellpadding="3">
                    <tr>
                        <th>&nbsp;</th>
                        <th colspan="3" style="text-align: center;">
                            <%=str1stMonth%>
                        </th>
                        <th colspan="3" style="text-align: center;">
                            <%=str2ndMonth%>
                        </th>
                        <th colspan="3" style="text-align: center;">
                            <%=str3rdMonth%>
                        </th>
                    </tr>
                    <tr>
                        <th style="text-align: center;">
                            Name
                        </th>
                        <th style="text-align: center;">
                            Low
                        </th>
                        <th style="text-align: center;">
                            Medium
                        </th>
                        <th style="text-align: center;">
                            High
                        </th>
                        <th style="text-align: center;">
                            Low
                        </th>
                        <th style="text-align: center;">
                            Medium
                        </th>
                        <th style="text-align: center;">
                            High
                        </th>
                        <th style="text-align: center;">
                            Low
                        </th>
                        <th style="text-align: center;">
                            Medium
                        </th>
                        <th style="text-align: center;">
                            High
                        </th>
                    </tr>
                    <asp:Repeater ID="rpUg" runat="server" OnItemDataBound="rpUg_ItemDataBound" OnPreRender="rpUg_PreRender">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 120px;">
                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    <asp:HiddenField ID="hiUId" runat="server" Value='<%# Bind("UserId") %>' />
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblFL_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbFL_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblFM_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbFM_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblFH_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbFH_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblSL_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbSL_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblSM_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbSM_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblSH_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbSH_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblTL_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbTL_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblTM_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbTM_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblTH_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbTH_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="EvenRow">
                                <td>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                    <asp:HiddenField ID="hiUId" runat="server" Value='<%# Bind("UserId") %>' />
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblFL_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbFL_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblFM_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbFM_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblFH_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbFH_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblSL_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbSL_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblSM_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbSM_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblSH_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbSH_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblTL_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbTL_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblTM_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbTM_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                                <td>
                                    <span class="validSign">*</span>
                                    <asp:Label ID="lblTH_Dis" runat="server" CssClass="disArea"></asp:Label>
                                    <asp:TextBox ID="tbTH_Input" runat="server" CssClass="txtArea" MaxLength="12"></asp:TextBox>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                    </asp:Repeater>
                </table>
                <div class="GridPaddingBottom">
                    &nbsp;</div>
            </div>
            <div style="margin-top: 20px;">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click"
                                OnClientClick="return checkBeforeSave();" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnClear" runat="server" Text="Clear" class="Btn-66" OnClientClick="clearInput(); return false;" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnCopy" runat="server" Text="Copy" class="Btn-66" />
                        </td>
                        <td style="padding-left: 8px;">
                            <asp:Button ID="btnClose" runat="server" Text="Close" class="Btn-66" OnClientClick="return closeBox();" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="menu1" class="contextMenu" style="display: none;">
            <ul>
                <li id="cPrevQua">Copy from previous quarter</li>
                <li id="cf2t">Copy
                    <%=str1stMonth%>
                    to
                    <%=str2ndMonth%></li>
                <li id="ct2t">Copy
                    <%=str2ndMonth%>
                    to
                    <%=str3rdMonth%></li></ul>
        </div>
    </div>
    </form>
</body>
</html>
