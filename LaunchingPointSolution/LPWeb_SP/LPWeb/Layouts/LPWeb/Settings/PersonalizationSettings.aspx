<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonalizationSettings.aspx.cs"
    Inherits="LPWeb.Layouts.LPWeb.Settings.PersonalizationSettings" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
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
    <script src="../js/jquery.jscale.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <style type="text/css">
        .TabContent table td
        {
            padding-top: 9px;
        }
    </style>
    <style type="text/css">
        .validSign
        {
            padding-top: 4px;
            color: Red;
            display: none;
        }
    </style>
    <script language="javascript" type="text/javascript">
        $(document).ready(function () {
            DrawTab();

            initUserGoalsSetupWin();
        });
    </script>
    <script type="text/javascript">
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
    </script>
    <script type="text/javascript">
// <![CDATA[
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
                    return SaveAll_Licenses();
                }
            }
            return SaveAll_Licenses();
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
            //$("#divPreviewX").width($("#ctl00_ctl00_PlaceHolderMain_MainArea_imgUserPhoto").width()); // commented by peter 20111025
            //$("#divPreviewY").width($("#ctl00_ctl00_PlaceHolderMain_MainArea_subPageLogo").width());
        });

        // check/decheck all
        function CheckAll(CheckBox) {
            var operGridId = "#" + $(CheckBox).parents("table").attr("id");
            if (CheckBox.checked) {
                $(operGridId + " tr td input[type=checkbox]").attr("checked", "true");
            }
            else {

                $(operGridId + " tr td input[type=checkbox]").attr("checked", "");
            }
        }


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

                    $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left - 4, top: $("#" + FakeBrowseBtnID).offset().top - 6 });
                }
                else if ($.browser.version == "8.0") {

                    $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left, top: $("#" + FakeBrowseBtnID).offset().top - 6 });
                }
            }
            else if ($.browser.mozilla == true) {

                $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left - 148, top: $("#" + FakeBrowseBtnID).offset().top - 1 });
            } else {

                $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left, top: $("#" + FakeBrowseBtnID).offset().top - 1 });

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
            $("#txtFakeUpload").val($("#ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1").val());

            if ($.browser.msie == true) {
                return;
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

                //PreviewImage_IE78(ImageWidth, ImageHeight, "ctl00_ctl00_PlaceHolderMain_MainArea_FileUpload1", "tdPreview", "divPrevivew1");
            }
            else if ($.browser.mozilla == true) {

                //FFPreviewImage_FileUpload1();
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

        function resizeImage(imgID) {
            // set image size, width
            var imageObj = $("#" + imgID);
            var w = imageObj.width();
            var h = imageObj.height();
            if (w / h > 120 / 150) {
                if (w < 120)
                    return;
                imageObj.jScale({ w: "120px" });
            }
            else {
                if (h < 150)
                    return;
                imageObj.jScale({ h: "150px" });
            }
            imageObj.css("display", "");
        }

        //Licenses
        var gvLicensesID = "<%= gridLicensesList.ClientID%>";

        function AddLicensesRow() {

            var TrCopy = $("#gridLicensesListTMP tr").eq(0).clone(true);

            $("#" + gvLicensesID).append(TrCopy);

        }

        

        function RemoveLicensesRow() {

            $("#" + gvLicensesID + " input[uid='cbLicenses']:checked").parent().parent().remove();
        }


        var hidLicenseNumberListID = "<%= hidLicenseNumberList.ClientID%>";

        function SaveAll_Licenses() {

            var LicenseNumberList = $("#" + gvLicensesID + " input[uid='LicenseNumber']");
            var str = "";

            LicenseNumberList.each(function (i) {
                
                str = str + $(this).val() + ",";
            });



            $("#" + hidLicenseNumberListID).val(str);

            return true;
        }


// ]]>
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">
        User Personalization</div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">
                        &nbsp;
                    </td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li id="current"><a href="PersonalizationSettings.aspx"><span>Settings</span></a></li>
                                <li><a href="PersonalizationPreferences.aspx"><span>Preferences</span></a></li>
                                <%--<li><a href="PersonalizationMarketing.aspx"><span>Marketing</span></a></li>--%>
                                <li><a href="PersonalizationLoansViewtTab.aspx"><span>Pipeline View</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine" style="width: 242px">
                    &nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">
                    &nbsp;</div>
                <div class="TabContent">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-top: 9px; width: 100px;">
                                User Name
                            </td>
                            <td style="padding-top: 9px; width: 350px;">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td >
                                            <asp:Label ID="lbUserName" runat="server"></asp:Label>
                                            <asp:HiddenField ID="hiPrefix" runat="server" />
                                            <asp:HiddenField ID="hiUsername" runat="server" />
                                            <asp:HiddenField ID="hiFirstName" runat="server" />
                                            <asp:HiddenField ID="hiLastName" runat="server" />
                                        </td>
                                        <td style="padding-left: 8px;">
                                            <asp:Button ID="btnSave" runat="server" Text="Save" class="Btn-66" OnClick="btnSave_Click"
                                                OnClientClick="return onSaveBtnClick();" />
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
                            </td>
                            <td style="padding-top: 9px; width: 127px;">
                                Email address
                            </td>
                            <td style="padding-top: 9px;">
                                <asp:Label ID="lbEmail" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trPwd" style="display: none;" runat="server">
                            <td style="padding-top: 9px; width: 100px;">
                                password
                            </td>
                            <td style="padding-top: 9px; width: 350px;">
                                <asp:TextBox ID="tbPWD" runat="server" TextMode="Password" class="iTextBox" Width="168px"
                                    MaxLength="50"></asp:TextBox>
                                <span id="spanPwd" class="validSign">*</span>
                            </td>
                            <td style="padding-top: 9px; width: 127px;">
                                Re-type password
                            </td>
                            <td style="padding-top: 9px;">
                                <asp:TextBox ID="tbPWDCfm" runat="server" TextMode="Password" class="iTextBox" Width="168px"
                                    MaxLength="50"></asp:TextBox>
                                <span id="spanPwdRe" class="validSign">*</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 9px;">
                                <asp:LinkButton ID="lbtnChangePwd" runat="server" OnClientClick="changePWDBtnClicked(this); return false;">Change your password</asp:LinkButton>
                                <asp:LinkButton ID="lbtnCancelPwd" runat="server" OnClientClick="cancelPWDChange(this); return false;">Cancel</asp:LinkButton>
                            </td>

                            <td>
                             Exchange Password
                            </td>
                            <td>
                                <asp:TextBox ID="txbExchangPwd" runat="server" TextMode="Password" ReadOnly="false" class="iTextBox" Width="168px" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            Phone
                            </td>
                            <td>
                                <asp:TextBox ID="txbPhone" runat="server" class="iTextBox" ReadOnly="false" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>

                            <td>
                            Cell
                            </td>
                            <td>
                                <asp:TextBox ID="txbCell" runat="server" class="iTextBox" ReadOnly="false" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            Fax
                            </td>
                            <td>
                                <asp:TextBox ID="txbFax" runat="server" class="iTextBox" ReadOnly="false" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>

<%--                            <td>
                            Fax
                            </td>
                            <td>
                                <asp:TextBox ID="txbFax" runat="server" class="iTextBox" ReadOnly="false" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>--%>
                        </tr>

                        <!--<tr>
                            <td>
                            License
                            </td>
                            <td>
                                <asp:TextBox ID="txbLicense" runat="server" class="iTextBox" ReadOnly="false" Width="100px" MaxLength="20"></asp:TextBox>
                            </td>

                            <td>
                           
                            </td>
                            <td>
                                
                            </td>
                        </tr>-->

                    </table>

                    <table cellpadding="0" cellspacing="0" style="margin-top: 0px;" border="0">
                        <tr>
                            <td style="width: 100px; padding-top: 9px;">
                                My Picture
                            </td>
                            <td style="width: 350px; padding-top: 9px;">
                                <input id="txtFakeUpload" type="text" style="width: 200px;" readonly />
                                <input id="btnFakeBrowse" type="button" value="Browse..." class="Btn-66" />
                                <span>
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="70px" Height="25px" Style="z-index: 10000;
                                        opacity: 0; filter: alpha(opacity=0);" />
                                </span>
                            </td>
                            <td style="padding-top: 9px; width: 127px;">
                                NMLS
                            </td>
                            <td>
                                <asp:TextBox ID="txbNMLS" runat="server" class="iTextBox" ReadOnly="false" Width="265px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
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
                                    <div style="text-align: center;">
                                        Max Size: 120px * 150px</div>
                                </div>
                            </td>
                            <td colspan="2">
                                <div style=" margin-bottom:10px;" > My Signature</div>
                                <asp:TextBox ID="txtSignature" runat="server" TextMode="MultiLine" Height="200px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <div >Licenses:</div>
                    

                    <div id="divLicensesList" class="" style="width: 200px; margin-left:10px; margin-top:10px;">
                        <div > <a href="javascript:void(0);" onclick="AddLicensesRow();" >Add Row</a> &nbsp;&nbsp;|&nbsp;&nbsp; <a href="javascript:void(0);" onclick="RemoveLicensesRow();"  >Remove Row</a></div> 
                        <asp:GridView ID="gridLicensesList" runat="server" DataKeyNames="UserLicenseId"
                            EmptyDataText="There is no data to display." Width="200px" CellPadding="3" CssClass="GrayGrid"
                            GridLines="None" AutoGenerateColumns="false" ShowHeader="false">
                            <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                            <AlternatingRowStyle CssClass="EvenRow" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                                    <HeaderTemplate>
                                        <input id="cbLicensesAll" type="checkbox" onclick="CheckAll(this)" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input uid="cbLicenses" type="checkbox" title="<%# Eval("UserLicenseId") %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField >
                                    <ItemTemplate>
                                        
                                        <input type="text"  uid="LicenseNumber" style=" width:250px;" value="<%# Eval("LicenseNumber") %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hidLicenseNumberList" runat="server" Value="" />
                        <div class="GridPaddingBottom">
                            &nbsp;</div>

                        <table class="GrayGrid" cellspacing="0" cellpadding="3" border="0" id="gridLicensesListTMP" style="width:200px;border-collapse:collapse; display:none;">
                            <tr>
	                            <td class="CheckBoxColumn">
                                    <input uid="cbLicenses" type="checkbox" />
                                </td><td>
                                        
                                    <input type="text" uid="LicenseNumber" style=" width:250px;" value="" />
                                </td>
                            </tr>
                        </table>


                    </div>

                </div>
                
            </div>
        </div>
    </div>
    <div style="display: none;">
        <div id="dialogUserGoalsSetup">
            <iframe id="iframeUG" name="iframeUG" frameborder="0" width="100%" height="100%">
            </iframe>
        </div>
    </div>
</asp:Content>
