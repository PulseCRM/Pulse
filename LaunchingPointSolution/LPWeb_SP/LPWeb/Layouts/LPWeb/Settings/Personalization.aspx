<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="User Personalization" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="LPWeb.Settings.Personalization" CodeBehind="Personalization.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.js" type="text/javascript"></script>
    <script src="../js/jquery.cleditor.js" type="text/javascript"></script>
    
    <style type="text/css">
        .validSign
        {
            padding-top: 4px;
            color: Red;
            display: none;
        }
    </style>
    <script type="text/javascript">
// <![CDATA[
        $(document).ready(function () {
            $('#msgBtnOK').click(function () {
                $.unblockUI();
                return false;
            });
        });

        function ShowMsg(args) {
            if ("invalidInput" == args)
                alert("Invalid input!");
            else if ("savesuccess" == args)
                alert("Saved!");
            else if ("unknowerror" == args)
                alert("User does not exists, unknow error.");
        }
// ]]>
    </script>
    <script type="text/javascript">
// <![CDATA[
        $(document).ready(function () {
            initUserGoalsSetupWin();
        });
        function initUserGoalsSetupWin() {
            $('#dialogUserGoalsSetup').dialog({
                modal: true,
                autoOpen: false,
                title: 'User Goals Setup',
                width: 850,
                height: 620,
                resizable: false,
                close: clearUserGoalsSetupWin
            });
        }
        function showUserGoalsSetupWin(allIDs) {
            var f = document.getElementById('iframeUG');
            f.src = "UserGoalsSetup.aspx?ids=" + allIDs + "&t=" + Math.random().toString();
            $('#dialogUserGoalsSetup').dialog('open');
        }
        function clearUserGoalsSetupWin(bR) {
            var f = document.getElementById('iframeUG');
            f.src = "about:blank";
        }
        function closeUserGoalsSetupWin() {
            $('#dialogUserGoalsSetup').dialog('close');
        }

        function onSaveBtnClick() {
            if ($("#" + "<%=trPwd.ClientID %>").is(":visible")) {
                var sPwd = $("#" + "<%=tbPWD.ClientID %>").val();
                var sPwdRe = $("#" + "<%=tbPWDCfm.ClientID %>").val();
                if (sPwd.length <= 0) {
                    $("#spanPwd").show();
                    $("#spanPwdRe").show();
                    alert("Please ensure that the password and confirmation match exactly.");
                    return false;
                }
                else if (sPwd != sPwdRe) {
                    $("#spanPwd").show();
                    $("#spanPwdRe").show();
                    alert("Please ensure that the password and confirmation match exactly.");
                    return false;
                }
                else {
                    $("#spanPwd").hide();
                    $("#spanPwdRe").hide();
                    return true;
                }
            }
            return true;
        }
        function changePWDBtnClicked(me) {
            $(me).hide();
            $("#" + "<%=lbtnCancelPwd.ClientID %>").show();
            $("#" + "<%=trPwd.ClientID %>").show();
        }
        function cancelPWDChange(me) {
            $(me).hide();
            $("#" + "<%=lbtnChangePwd.ClientID %>").show();
            $("#" + "<%=trPwd.ClientID %>").hide();

            $("#" + "<%=tbPWD.ClientID %>").val("");
            $("#" + "<%=tbPWDCfm.ClientID %>").val("");
        }
// ]]>
    </script>
    <script type="text/javascript">
// <![CDATA[
        var nPCSChecked = 0;
        function PCSSelected(ckb) {
            if (ckb.checked) {
                if (nPCSChecked >= 6) {
                    alert("You can not pick up more than six selections.");
                    ckb.checked = false;
                    return false;
                }
                else {
                    nPCSChecked++;
                    return true;
                }
            }
            else {
                nPCSChecked--;
                return true;
            }
        }
        var nPPsCSChecked = 0;
        function PPsCSSelected(ckb) {
            if (ckb.checked) {
                if (nPPsCSChecked >= 6) {
                    alert("You can not pick up more than six selections.");
                    ckb.checked = false;
                    return false;
                }
                else {
                    nPPsCSChecked++;
                    return true;
                }
            }
            else {
                nPPsCSChecked--;
                return true;
            }
        }
        var nPVPCSChecked = 0;
        function PVPCSelected(ckb) {
            if (ckb.checked) {
                if (nPVPCSChecked >= 6) {
                    alert("You can not pick up more than six selections.");
                    ckb.checked = false;
                    return false;
                }
                else {
                    nPVPCSChecked++;
                    return true;
                }
            }
            else {
                nPVPCSChecked--;
                return true;
            }
        }
        var allChartCkbCtlIds = new Array('<%=ckbPipelineChart.ClientID %>', '<%=ckbSalesBreakdownChart.ClientID %>',
        '<%=ckbOrgProductionChart.ClientID %>', '<%=ckbOrgProductSaleBreakdownChart.ClientID %>');
        var needCheckAllChart = true;
        var nHPChecked = 0;
        function HPSelected(ckb) {
            if (ckb.checked) {
                if (needCheckAllChart) {
                    var index = Array.indexOf(allChartCkbCtlIds, ckb.id);
                    if (!isNaN(index) && index != -1) {
                        for (var i = 0; i < allChartCkbCtlIds.length; i++) {
                            if (ckb.id != allChartCkbCtlIds[i] && $("#" + allChartCkbCtlIds[i]).attr("checked")) {
                                $("#" + allChartCkbCtlIds[i]).attr("checked", false);
                                nHPChecked--;
                            }
                        }
                    }
                }
                if (nHPChecked >= 6) {
                    alert("You can not pick up more than six selections.");
                    ckb.checked = false;
                    return false;
                }
                else {
                    nHPChecked++;
                    return true;
                }
            }
            else {
                nHPChecked--;
                return true;
            }
        }
// ]]>
    </script>
    <script type="text/javascript">
// <![CDATA[
        //#region neo

        var FileUploadHtml;

        var HtmlEditor;

        $(document).ready(function () {

            //alert($.browser.version);

            FileUploadHtml = GetFileUploadHtml("ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1");
            //alert(FileUploadHtml);

            // add event
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").change(FileUpload1_onchange);

            // set file upload position
            SetFileUploadPosition("ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1", "btnFakeBrowse");

            InitHtmlEditor();

            // set image width and height
            $("#divPreviewX").width($("#ctl00_ctl00_PlaceHolderMain_MainArea_imgUserPhoto").width());
            //$("#divPreviewY").width($("#ctl00_ctl00_PlaceHolderMain_MainArea_subPageLogo").width());
        });

        function GetFileUploadHtml(ID) {

            if ($.browser.msie == true) {

                return $("#" + ID).get(0).outerHTML;
            }
            else {

                return $.trim($("#" + ID).parent().html());
            }
        }

        function SetFileUploadPosition(FileUploadID, FakeBrowseBtnID) {

            if ($.browser.msie == true) {

                if ($.browser.version == "7.0") {

                    $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left - 4, top: $("#" + FakeBrowseBtnID).offset().top - 1 });
                }
                else if ($.browser.version == "8.0") {

                    $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left, top: $("#" + FakeBrowseBtnID).offset().top - 1 });
                }
            }
            else if ($.browser.mozilla == true) {

                $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left - 153, top: $("#" + FakeBrowseBtnID).offset().top - 1 });
            }
        }

        // init cleditor
        function InitHtmlEditor() {

            if (HtmlEditor == undefined && $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtSignature").is(":visible") == true) {

                HtmlEditor = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtSignature").cleditor({ width: 400, height: 200, bodyStyle: "font:11px Arial", docType: '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">' });
                HtmlEditor[0].focus();
            }
        }

        var MaxWidth = 120;
        var MaxHeight = 150;

        function FileUpload1_onchange() {

            var FakeImagePath = $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").val();
            var IsValid = ValidateImageExt(FakeImagePath);
            if (IsValid == false) {

                alert("Please select a valid image file.");

                $("#txtFakeUpload").val("");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").replaceWith(FileUploadHtml)
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").change(FileUpload1_onchange);
                SetFileUploadPosition("ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1", "btnFakeBrowse");
                return;
            }

            if ($.browser.msie == true) {

                //#region get width and height of image

                var ImageSize = GetImageSize_IE("ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1");

                var ImageWidth = ImageSize.ImageWidth;
                var ImageHeight = ImageSize.ImageHeight;

                //alert("ImageWidth: " + ImageWidth + "; ImageHeight: " + ImageHeight);

                //#endregion

                // it's fobidden to exceed
                if (ImageWidth > MaxWidth || ImageHeight > MaxHeight) {

                    alert("The image that you tried to upload exceeded the size limitation (" + MaxWidth + "x" + MaxHeight + ").");

                    $("#txtFakeUpload").val("");
                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").replaceWith(FileUploadHtml)
                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").change(FileUpload1_onchange);
                    SetFileUploadPosition("ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1", "btnFakeBrowse");
                    return;
                }

                PreviewImage_IE78(ImageWidth, ImageHeight, "ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1", "tdPreview", "divPrevivew1");
            }
            else if ($.browser.mozilla == true) {

                FFPreviewImage_FileUpload1();
            }
        }

        function ValidateImageExt(sImagePath) {

            var ext = sImagePath.substr(sImagePath.lastIndexOf('.'));
            ext = ext.replace(".", "");
            var AllowExt = "jpg|gif|jpeg|png|tif";
            var ExtOK = false;
            var ArrayExt;
            if (AllowExt.indexOf('|') != -1) {
                ArrayExt = AllowExt.split('|');
                for (i = 0; i < ArrayExt.length; i++) {
                    if (ext.toLowerCase() == ArrayExt[i]) {
                        ExtOK = true;
                        break;
                    }
                }
            }
            else {
                ArrayExt = AllowExt;
                if (ext.toLowerCase() == ArrayExt) {
                    ExtOK = true;
                }
            }
            return ExtOK;
        }

        //#region preview for ie

        function GetImageSize_IE(FileUploadID) {

            // get image path
            $("#" + FileUploadID).get(0).select();
            var ImageFilePath = document.selection.createRange().text;
            //alert(ImageFilePath);

            //#region get width and height of image

            var ImageWidth = 0;
            var ImageHeight = 0;

            // append temp div
            $("#tdPreview").append("<div id='divPreview2' style='width: 1px; height: 1px; margin-left: auto; margin-right: auto; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=image); visibility: hidden;'></div>");

            // set hidden filter.src
            var divPreview2 = document.getElementById("divPreview2");
            divPreview2.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = ImageFilePath;

            // get width and height of image
            var ImageWidth2 = $("#divPreview2").get(0).offsetWidth;
            var ImageHeight2 = $("#divPreview2").get(0).offsetHeight;
            //alert("ImageWidth2: " + ImageWidth2 + "; ImageHeight2: " + ImageHeight2);

            // remove temp div
            $("#divPreview2").remove();

            ImageWidth = ImageWidth2;
            ImageHeight = ImageHeight2;

            //#endregion

            var ImageSize = { "ImageWidth": ImageWidth, "ImageHeight": ImageHeight };
            return ImageSize;
        }

        function PreviewImage_IE78(ImageWidth, ImageHeight, FileUploadID, ContainerID, divPreviewID) {

            // append
            $("#" + ContainerID).empty();
            $("#" + ContainerID).append("<div id='" + divPreviewID + "' style='width: 1px; height: 1px; margin-left: auto; margin-right: auto; filter: progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale');'></div>");

            // get image path
            $("#" + FileUploadID).get(0).select();
            var ImageFilePath = document.selection.createRange().text;

            // set filter.src
            var divPreview = document.getElementById(divPreviewID);
            divPreview.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').src = ImageFilePath;

            if (ImageWidth > MaxWidth || ImageHeight > MaxHeight) {

                // set max width and length
                $("#" + divPreviewID).width(MaxHeight);
                $("#" + divPreviewID).height(MaxHeight);

                divPreview.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').sizingMethod = "scale";
            }
            else {

                // set width and height of divPreview
                $("#" + divPreviewID).width(ImageWidth);
                $("#" + divPreviewID).height(ImageHeight);

                divPreview.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').sizingMethod = "image";
            }

            $("#txtFakeUpload").val($("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").val());
        }

        //#endregion

        //#region preivew for firefox

        function FFPreviewImage_FileUpload1() {

            InitFFTempImage("ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1");

            GetImageSize("imgFFTempPreview", AfterGetImageSize_FileUpload1, "AfterGetImageSize_FileUpload1");
        }

        function InitFFTempImage(FileUploadID) {

            // append temp frame
            $("body").append("<table id='tbFFTempFrame' cellpadding='0' cellspacing='0' style='border: solid 1px #d8d8d8; visibility: hidden'><tr><td id='tdFFTempPreview'></td></tr></table>");

            // get image file path
            var ImagePath1 = $("#" + FileUploadID).get(0).files[0].getAsDataURL();

            // append temp image
            $("#tdFFTempPreview").empty();
            $("#tdFFTempPreview").append("<img id='imgFFTempPreview' alt='' src='' />");
            $("#imgFFTempPreview").attr("src", ImagePath1);
        }

        // get image width and height when image load complete
        function GetImageSize(ImageID, callback, callbackName) {

            var ThisImage = $("#" + ImageID);

            if (ThisImage.attr("complete") == true) {

                var ImageWidth = ThisImage.width();
                var ImageHeight = ThisImage.height();
                var src = ThisImage.attr("src");

                // remove temp
                ThisImage.remove();
                $("#tbFFTempFrame").remove();

                // callback
                callback(ImageWidth, ImageHeight, src);
            }
            else {

                window.setTimeout("GetImageSize('" + ImageID + "', " + callbackName + ")", 100);
            }
        }

        function GetImageScaleSize(MaxWidth, MaxHeight, ImageWidth, ImageHeight) {

            //#region scale

            var NewWidth = 0;
            var NewHeight = 0;

            if (ImageWidth > MaxWidth && ImageHeight < MaxHeight) {

                // scale by MaxWidth
                var Percent = MaxWidth / ImageWidth;
                //alert("Percent1: " + Percent);

                NewWidth = MaxWidth;
                //alert("NewWidth1: " + NewWidth);

                NewHeight = ImageHeight * Percent;
                //alert("NewHeight1: " + NewHeight);
            }
            else if (ImageWidth < MaxWidth && ImageHeight > MaxHeight) {

                // scale by MaxHeight
                var Percent = MaxHeight / ImageHeight;
                //alert("Percent2: " + Percent);

                NewWidth = ImageWidth * Percent;
                //alert("NewWidth2: " + NewWidth);

                NewHeight = MaxHeight;
                //alert("NewHeight2: " + NewHeight);
            }
            else if (ImageWidth > MaxWidth && ImageHeight > MaxHeight) {

                if (MaxWidth < MaxHeight) {

                    // scale by MaxWidth
                    var Percent = MaxWidth / ImageWidth;
                    //alert("Percent3: " + Percent);

                    NewWidth = MaxWidth;
                    //alert("NewWidth3: " + NewWidth);

                    NewHeight = ImageHeight * Percent;
                    //alert("NewHeight3: " + NewHeight);
                }
                else {

                    // scale by MaxHeight
                    var Percent = MaxHeight / ImageHeight;
                    //alert("Percent4: " + Percent);

                    NewWidth = ImageWidth * Percent;
                    //alert("NewWidth4: " + NewWidth);

                    NewHeight = MaxHeight;
                    //alert("NewHeight4: " + NewHeight);
                }
            }
            else {

                // if less than or equal to MaxWidth & MaxHeight, do not scale
                NewWidth = ImageWidth;
                //alert("NewWidth5: " + NewWidth);

                NewHeight = ImageHeight;
                //alert("NewHeight5: " + NewHeight);
            }

            //#endregion

            var ImageSize = { "NewWidth": NewWidth, "NewHeight": NewHeight };
            return ImageSize;
        }

        function AfterGetImageSize_FileUpload1(ImageWidth, ImageHeight, src) {

            //alert("ImageWidth: " + ImageWidth + "; ImageHeight: " + ImageHeight);

            // it's fobidden to exceed
            if (ImageWidth > MaxWidth || ImageHeight > MaxHeight) {

                alert("The image that you tried to upload exceeded the size limitation (" + MaxWidth + "x" + MaxHeight + ").");

                $("#txtFakeUpload").val("");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").replaceWith(FileUploadHtml)
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").change(FileUpload1_onchange);
                SetFileUploadPosition("ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1", "btnFakeBrowse");
                return;
            }

            // get new width and height after scale
            var NewSize = GetImageScaleSize(MaxWidth, MaxHeight, ImageWidth, ImageHeight);
            //alert("NewSize.NewWidth: " + NewSize.NewWidth);
            //alert("NewSize.NewHeight: " + NewSize.NewHeight);

            var NewWidth = NewSize.NewWidth;
            var NewHeight = NewSize.NewHeight;

            // append image for preview
            $("#tdPreview").empty();
            $("#tdPreview").append("<div id='divPreview'><img id='imgPreview' alt='' src='' /></div>");

            // set width and height of image
            $("#imgPreview").width(NewWidth);
            $("#imgPreview").height(NewHeight);

            // set width and height of divPreview
            $("#divPreview").width(NewWidth);
            $("#divPreview").height(NewHeight);
            $("#divPreview").css("margin-left", "auto");
            $("#divPreview").css("margin-right", "auto");

            // set img.src
            var ImagePath = src;
            $("#imgPreview").attr("src", ImagePath);

            $("#txtFakeUpload").val($("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").val());
        }


        //#endregion


        //#endregion
// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div class="Heading">
        User Personalization</div>
    <div class="SplitLine">
    </div>
    <div class="DetailsContainer">
        <div>
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td style="padding-top: 9px; width: 100px;">User Name</td>
                    <td style="padding-top: 9px; width: 350px;">
                        <asp:Label ID="lbUserName" runat="server"></asp:Label>
                        <asp:HiddenField ID="hiPrefix" runat="server" />
                        <asp:HiddenField ID="hiUsername" runat="server" />
                        <asp:HiddenField ID="hiFirstName" runat="server" />
                        <asp:HiddenField ID="hiLastName" runat="server" />
                    </td>
                    <td style="padding-top: 9px; width: 127px;">Email address</td>
                    <td style="padding-top: 9px;">
                        <asp:Label ID="lbEmail" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr id="trPwd" style="display: none;" runat="server">
                    <td style="padding-top: 9px; width: 100px;">password</td>
                    <td style="padding-top: 9px; width: 350px;">
                        <asp:TextBox ID="tbPWD" runat="server" TextMode="Password" class="iTextBox" Width="168px" MaxLength="50"></asp:TextBox>
                        <span id="spanPwd" class="validSign">*</span>
                    </td>
                    <td style="padding-top: 9px; width: 127px;">Re-type password</td>
                    <td style="padding-top: 9px;">
                        <asp:TextBox ID="tbPWDCfm" runat="server" TextMode="Password" class="iTextBox" Width="168px" MaxLength="50"></asp:TextBox>
                        <span id="spanPwdRe" class="validSign">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="padding-top: 9px;">
                        <asp:LinkButton ID="lbtnChangePwd" runat="server" OnClientClick="changePWDBtnClicked(this); return false;">Change your password</asp:LinkButton>
                        <asp:LinkButton ID="lbtnCancelPwd" runat="server" OnClientClick="cancelPWDChange(this); return false;">Cancel</asp:LinkButton>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" style="margin-top: 15px;" border="0">
                <tr>
                    <td style="width: 100px;padding-top: 9px;">My Picture</td>
                    <td style="width: 350px;padding-top: 9px;">
                        <input id="txtFakeUpload" type="text" style="width: 200px;" readonly />
                        <input id="btnFakeBrowse" type="button" value="Browse..." class="Btn-66" />
                        <span>
                            <asp:FileUpload ID="FileUpload1" runat="server" Width="70px" Height="25px" Style="z-index: 10000; opacity: 0; filter: alpha(opacity=0);" />
                        </span>
                    </td>
                    <td>My Signature</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <div style="width: 160px; margin-top: 5px;">
                            <table cellpadding="0" cellspacing="0" style="border: solid 1px #d8d8d8;">
                                <tr>
                                    <td id="tdPreview" style="width: 150px; height: 180px; background-color: #f6f6f6;">
                                        <div id="divPreviewX" style="margin-left: 15px;">
                                            <asp:Image ID="imgUserPhoto" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">Max Size: 120px * 150px</div>
                        </div>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSignature" runat="server" TextMode="MultiLine" Height="200px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="0" style="margin-top: 15px;" >
                <tr>
                    <td style="width: 100px;">
                        Loans per page
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLoanPerPage" runat="server" Width="170px">
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            <asp:ListItem Text="25" Value="25"></asp:ListItem>
                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                            <asp:ListItem Text="100" Value="100"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divModuleName1" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">Pipeline Column Selections (pick up to six)</div>
        <div class="DashedBorder" style="margin-top: 8px;">&nbsp;</div>
        <div style="margin-top: 3px;">
            <table cellpadding="2" cellspacing="0">
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbPointFolder" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Point folder
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbAmount" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Amount
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbPercentComplete" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Percent complete
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbStage" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Stage
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbLien" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Lien
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbProcessor" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Processor
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbBranch" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Branch
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbRate" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Rate
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbTaskCount" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Task count
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbAlerts" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Alerts
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbLender" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Lender
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbFilename" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Filename
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbLoanOfficer" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Loan Officer
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbLockExpirDate" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Lock expiration date
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbEstimatedClose" runat="server" onclick="PCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Estimated Close
                    </td>
                </tr>
            </table>
        </div>
        <div id="divModuleName2" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">Client View Pipeline Column Selections (pick up to six)</div>
        <div class="DashedBorder" style="margin-top: 8px;">&nbsp;</div>
        <div style="margin-top: 3px;">
            <table cellpadding="2" cellspacing="0">
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbpvBranch" runat="server" onclick="PVPCSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Branch
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbpvCreate" runat="server" onclick="PVPCSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Date Created
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbpvLeadSource" runat="server" onclick="PVPCSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Lead Source
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbpvLoanOfficer" runat="server" onclick="PVPCSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Loan Officer
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbpvRefCode" runat="server" onclick="PVPCSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Reference Code
                    </td>                    
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbpvReferral" runat="server" onclick="PVPCSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Referral
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbpvPartner" runat="server" onclick="PVPCSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Partner(Referral Company)
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div id="divModuleName3" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">Prospect Loan View Pipeline Column Selections (pick up to six)</div>
        <div class="DashedBorder" style="margin-top: 8px;">&nbsp;</div>
        <div style="margin-top: 3px;">
            <table cellpadding="2" cellspacing="0">
                <tr>
                    <td>
                        <asp:CheckBox ID="ckblvAmount" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Amount
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvBranch" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Branch
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvLeadSource" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Lead Source
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckblvLien" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Lien
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvLoanOfficer" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Loan Officer
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvLoanProgram" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Loan Program
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckblvPointFilename" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Point Filename
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvRanking" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Ranking
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvRate" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Rate
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckblvRefCode" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Reference Code
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvEstClose" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Est Close
                    </td>
                     <td>
                        <asp:CheckBox ID="ckblvProgress" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Progress
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:CheckBox ID="ckblvReferral" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Referral
                    </td>
                    <td>
                        <asp:CheckBox ID="ckblvPartner" runat="server" onclick="PPsCSSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Partner(Referral Company)
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
        <div id="div1" class="ModuleTitle" style="padding-left: 0px; padding-top: 15px;">Homepage Selections (pick up to six)</div>
        <div class="DashedBorder" style="margin-top: 8px;">&nbsp;</div>
        <div style="margin-top: 3px;">
            <table cellpadding="2" cellspacing="0">
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbCompanyCalendar" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 443px;">
                        Company calendar
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbRatesSummary" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px; width: 210px;">
                        Rates summary
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbPipelineChart" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Pipeline chart
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbGoalsChart" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Goals chart
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbSalesBreakdownChart" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Pipeline summary with sales breakdown chart
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbOverdueTasks" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Overdue tasks and alert summary
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbOrgProductionChart" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Pipeline summary with organizational production chart
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbAnnouncements" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Company announcements
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbOrgProductSaleBreakdownChart" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Pipeline summary with organizational production chart & sales breakdown chart
                    </td>
                    <td>
                        <asp:CheckBox ID="ckbExchangeInbox" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;">
                        Exchange inbox
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:CheckBox ID="ckbExchangeCalendar" runat="server" onclick="HPSelected(this)" />
                    </td>
                    <td style="padding-left: 4px;" colspan="3">
                        Exchange calendar
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-top: 20px;">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click" OnClientClick="return onSaveBtnClick();" />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnSetGoals" runat="server" Text="Set Goals" class="Btn-66" CausesValidation="false" />
                    </td>
                    <td style="padding-left: 8px;">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="Btn-66" CausesValidation="false"
                            OnClientClick="javascript:history.back(); return false;" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div style="display: none;">
        <div id="dialogUserGoalsSetup">
            <iframe id="iframeUG" name="iframeUG" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
    </div>
</asp:Content>
