<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartnerContactsSetupPopup.aspx.cs" Inherits="PartnerContactsSetupPopup" %>
<%@ Register Assembly="AspNetPager" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Partner Contacts Setup</title>
    <link href="../css/style.web.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.custom.css" rel="stylesheet" type="text/css" />
    <link href="../css/style.grid.css" rel="stylesheet" type="text/css" />
    <link href="../css/jqueryui/jquery.ui.all.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.cleditor.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery.js" type="text/javascript"></script>
    <script src="../js/common.js" type="text/javascript"></script>
    <script src="../js/jquery.maxlength.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script src="../js/jquery.only_press_num.js" type="text/javascript"></script>
    <script src="../js/jquery.alphanumeric.js" type="text/javascript"></script>
    <script src="../js/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../js/urlparser.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.core.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.widget.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.mouse.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.draggable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.position.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.resizable.min.js" type="text/javascript"></script>
    <script src="../js/jqueryui/jquery.ui.dialog.min.js" type="text/javascript"></script>
    <script src="../js/jquery.cleditor.js" type="text/javascript"></script>

        <script src="../js/jquery.datepick.js" type="text/javascript"></script>
    <script src="../js/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script src="../js/jquery.formatCurrency.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
// <![CDATA[

        var sHasAccessAllContacts = "<%=sHasAccessAllContacts %>";

        $(document).ready(function () {

            AddValidators();

            if (sHasAccessAllContacts == "0") {
                $("#divAssignUser").css("display", "none");
            }


            //txtBizPhone
            $("#txtCellPhone").mask("(999) 999-999?9");
            $("#txtBizPhone").mask("(999) 999-999?9");
            $("#txtFax").mask("(999) 999-999?9");



            $("#ddlBranch").change(function () {

                var Id = $(this).val();
                var RadomNum = Math.random();
                $.getJSON("PartnerContactsSetupPopup_BG.aspx?ContactBranchId=" + Id + "&r=" + RadomNum, function (data) {

                    if (data == null || data.ContactBranchId != Id) {
                        return;
                    }

                    $("#txtAddress").val(data.Address);
                    $("#txtCity").val(data.City);
                    $("#ddlStates").val(data.State);
                    $("#txtZip").val(data.Zip);

                });


            });

        });

        // add jQuery Validators
        function AddValidators() {

            $("#form1").validate({

                rules: {
                    txtLastName: {
                        required: true
                    },
                    txtFirstName: {
                        required: true
                    }
                },
                messages: {
                    txtLastName: {
                        required: "<div>Please enter Contact Last Name.</div>"
                    },
                    txtFirstName: {
                        required: "<div>Please enter Contact First Name.</div>"
                    }
                }
            });
        }

        // check/decheck all
        function CheckAll(CheckBox) {

            if (CheckBox.checked) {

                $("#gvUserList tr td :checkbox").attr("checked", "true");
            }
            else {

                $("#gvUserList tr td :checkbox").attr("checked", "");
            }
        }

        //#endregion

        function aAddUser_onclick() {

            CreateUser();  //Create User
        }

        //Create user
        function CreateUser() {

            var radomNum = Math.random();
            var radomStr = radomNum.toString().substr(2);
            $("#ifrUserAdd").attr("src", "AssignContactAccess.aspx?sid=" + radomStr + "&CloseDialogCodes=window.parent.AssignContactAccessPopupSelected(returnValue)&FromPage=PartnerContactsSetupPopup.aspx");

            $("#divAddUser").dialog({
                height: 450,
                width: 550,
                title: "Assign Contact Access",
                modal: true,
                resizable: false
            });
            $(".ui-dialog").css("border", "solid 3px #aaaaaa");
            $("body>div[role=dialog]").appendTo("#aspnetForm");
            return false;
        }

        //
        function AssignContactAccessPopupSelected(sSelUserIDs) {
            $("#hdnAddUserID").val(sSelUserIDs);

            $("#divAddUser").dialog("close");
            $("#divAddUser").dialog("destroy");

            //Add row to grid
            if (sSelUserIDs != "") {
                var pos = sSelUserIDs.indexOf("$");
                var i = 0;
                while (pos != -1) {
                    var myText = sSelUserIDs.substring(0, pos);
                    if (myText != "") {
                        //Split ID and Full Name and Role Name and Branch Name
                        var myArr = myText.split("|");
                        var sUserID = myArr[0];
                        var sFullName = myArr[1];
                        var sRoleName = myArr[2];
                        var sBranchName = myArr[3];
                        if (sUserID == "") {
                            sSelUserIDs = sSelUserIDs.substr(pos + 1);
                            pos = sSelUserIDs.indexOf("$");
                            continue;
                        }
                        if ($("#gvUserList tr td :checkbox[title=" + sUserID + "]").val() != undefined) {
                            sSelUserIDs = sSelUserIDs.substr(pos + 1);
                            pos = sSelUserIDs.indexOf("$");
                            continue;
                        }
                        // add th
                        if ($("#gvUserList tr td :checkbox").val() == undefined) {
                            $("#gvUserList").empty();
                            var sAddHeader = "<tr><th class=\"CheckBoxHeader\" scope=\"col\"><input type=\"checkbox\" onclick=\"CheckAll(this)\" /></th><th scope=\"col\"><a href=\"javascript:doPostBack('gvUserList','Sort$Name')\">User</a></th><th scope=\"col\"><a href=\"javascript:__doPostBack('gvUserList','Sort$RoleName')\">Role</a></th><th scope=\"col\"><a href=\"javascript:__doPostBack('gvUserList','Sort$BranchName')\">Branch</a></th><th scope=\"col\"><a href=\"javascript:__doPostBack('gvUserList','Sort$Enabled')\">Enabled</a></th></tr>";
                            $("#gvUserList").append(sAddHeader);
                        }
                        var sAddrow = "<tr><td class='CheckBoxColumn'><input type='checkbox' title='" + sUserID + "' /></td><td>" + sFullName + "</td><td>" + sRoleName + "</td><td>" + sBranchName + "</td><td>Yes</td></tr>";
                        $("#gvUserList").append(sAddrow);

                        i++;
                    }
                    sSelUserIDs = sSelUserIDs.substr(pos + 1);
                    pos = sSelUserIDs.indexOf("$");
                    continue;
                }
            }
        }


        // remove user
        function aRemove_onclick() {

            if ($("#gvUserList tr td :checkbox[checked=true]").length == 0) {
                alert("Please select one or more records.");
                return;
            }

            var RemoveUserIDs = $("#hdnRemoveUserID").val();
            $("#gvUserList tr td :checkbox[checked=true]").each(function (i) {

                var UserID = $(this).attr("title");
                if (RemoveUserIDs == "") {
                    RemoveUserIDs = UserID;
                }
                else {
                    RemoveUserIDs += "," + UserID;
                }
            });

            $("#hdnRemoveUserID").val(RemoveUserIDs);
            $("#gvUserList tr td :checkbox[checked=true]").parent().parent().remove();
        }

        // do sth. before saving
        function BeforeSave() {

            if ($("#ddlBranch").val() == "0") {
                alert("Please select a branch.");
                return false;
            }
            var UserIDs = "";
            $("#gvUserList tr:not(:first)").each(function (i) {

                var UserID = $(this).find("td :checkbox").attr("title") ;
                if (i == 0) {
                    UserIDs = UserID;
                }
                else {
                    UserIDs += "," + UserID;
                }
            });

            $("#hdnContactUserID").val(UserIDs);

            return true;
        }

        function btnDelete_onclick() {

            window.parent.document.getElementById("ifrLoanInfo").contentWindow.btnDelete_onclick();
            window.parent.CloseGlobalPopup();
        }
// ]]>
    </script>
    <script type="text/javascript">
// <![CDATA[
        //#region neo
        var fileUploadCtlId = "<%=FileUpload1.ClientID %>";
        var FileUploadHtml;

        var HtmlEditor;

        $(document).ready(function () {

            //alert($.browser.version);

            FileUploadHtml = GetFileUploadHtml(fileUploadCtlId);
            //alert(FileUploadHtml);

            // add event
            $("#" + fileUploadCtlId).change(FileUpload1_onchange);

            // set file upload position
            SetFileUploadPosition(fileUploadCtlId, "btnFakeBrowse");

            InitHtmlEditor();

            // set image width and height
            $("#divPreviewX").width($("#" + "<%=imgPhoto.ClientID %>").width());
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

                $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left, top: $("#" + FakeBrowseBtnID).offset().top - 1 });
                $("#" + FakeBrowseBtnID).addClass("Btn-115");

            }
            else {

                $("#" + FileUploadID).offset({ left: $("#" + FakeBrowseBtnID).offset().left, top: $("#" + FakeBrowseBtnID).offset().top - 1 });
                $("#" + FakeBrowseBtnID).addClass("Btn-115");

            }
        }

        // init cleditor
        function InitHtmlEditor() {

            if (HtmlEditor == undefined && $("#" + "<%=txtSignature.ClientID %>").is(":visible") == true) {

                HtmlEditor = $("#" + "<%=txtSignature.ClientID %>").cleditor({ width: 300, height: 200, bodyStyle: "font:11px Arial", docType: '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3.org/TR/html4/loose.dtd">' });
                HtmlEditor[0].focus();
            }
        }

        var MaxWidth = 120;
        var MaxHeight = 150;

        function FileUpload1_onchange() {

            var FakeImagePath = $("#" + fileUploadCtlId).val();
            var IsValid = ValidateImageExt(FakeImagePath);
            if (IsValid == false) {

                alert("Please select a valid image file.");

                $("#txtFakeUpload").val("");
                $("#" + fileUploadCtlId).replaceWith(FileUploadHtml)
                $("#" + fileUploadCtlId).change(FileUpload1_onchange);
                SetFileUploadPosition(fileUploadCtlId, "btnFakeBrowse");
                return;
            }

            if ($.browser.msie == true) {

                //#region get width and height of image

                var ImageSize = GetImageSize_IE(fileUploadCtlId);

                var ImageWidth = ImageSize.ImageWidth;
                var ImageHeight = ImageSize.ImageHeight;

                //alert("ImageWidth: " + ImageWidth + "; ImageHeight: " + ImageHeight);

                //#endregion

                // it's fobidden to exceed
                if (ImageWidth > MaxWidth || ImageHeight > MaxHeight) {

                    alert("The image that you tried to upload exceeded the size limitation (" + MaxWidth + "x" + MaxHeight + ").");

                    $("#txtFakeUpload").val("");
                    $("#" + fileUploadCtlId).replaceWith(FileUploadHtml)
                    $("#" + fileUploadCtlId).change(FileUpload1_onchange);
                    SetFileUploadPosition(fileUploadCtlId, "btnFakeBrowse");
                    return;
                }

                PreviewImage_IE78(ImageWidth, ImageHeight, fileUploadCtlId, "tdPreview", "divPrevivew1");
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

            $("#txtFakeUpload").val($("#" + fileUploadCtlId).val());
        }

        //#endregion

        //#region preivew for firefox

        function FFPreviewImage_FileUpload1() {

            InitFFTempImage(fileUploadCtlId);

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
                $("#" + fileUploadCtlId).replaceWith(FileUploadHtml)
                $("#" + fileUploadCtlId).change(FileUpload1_onchange);
                SetFileUploadPosition(fileUploadCtlId, "btnFakeBrowse");
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

            $("#txtFakeUpload").val($("#" + fileUploadCtlId).val());
        }


        //#endregion


        //#endregion
// ]]>
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="divContainer" style="width: 800px; height: 600px; border: solid 0px red; padding-left:20px; padding-top:10px;">
        <div id="divContactSetup">
            <table>
                <tr>
                    <td style="width: 110px;">Branch:</td>
                    <td style="width: 600px; padding-left:15px;">
                        <asp:DropDownList ID="ddlBranch" runat="server" Width="550" DataTextField="Name" DataValueField="ContactBranchId">
                        </asp:DropDownList>
                        <div style="display:none">
                        <asp:DropDownList ID="ddlBranchAddress" runat="server"  DataTextField="Address" DataValueField="ContactBranchId">
                        </asp:DropDownList>
                        </div>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">Contact Last Name:</td>
                    <td style="padding-left:15px;">
                        <asp:TextBox ID="txtLastName" runat="server" Width="120px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 110px; text-align:right;">First Name:</td>
                    <td style="padding-left:15px;">
                        <asp:TextBox ID="txtFirstName" runat="server" Width="120px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="padding-left:20px;">
                        <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" />
                    </td>
                    <td><label for="chkEnabled">Enabled</label></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">Address:</td>
                    <td style="width: 600px; padding-left:15px;">
                        <asp:TextBox ID="txtAddress" runat="server" Width="545" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">City:</td>
                    <td style="padding-left:15px; width:123px;">
                        <asp:TextBox ID="txtCity" runat="server" Width="100px" MaxLength="50"></asp:TextBox>
                    </td>
                    <td style="width: 110px; text-align:right;">State:</td>
                    <td style="padding-left:15px;">
                        <asp:DropDownList ID="ddlStates" runat="server"  Width="120px">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align:right; width:50px;">Zip:</td>
                    <td style="padding-left:15px;">
                        <asp:TextBox ID="txtZip" runat="server" Width="100px" MaxLength="12"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">Business Phone:</td>
                    <td style="padding-left:15px; width:120px">
                        <asp:TextBox ID="txtBizPhone" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="width: 110px; text-align:right;">Cell Phone:</td>
                    <td style="padding-left:15px;">
                        <asp:TextBox ID="txtCellPhone" runat="server" Width="120px" MaxLength="20"></asp:TextBox>
                    </td>
                    <td style="width: 50px; text-align:right;">Fax:</td>
                    <td style="padding-left:15px;">
                        <asp:TextBox ID="txtFax" runat="server" Width="98px" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">Email:</td>
                    <td style="width: 600px; padding-left:15px;">
                        <asp:TextBox ID="txtEmail" runat="server" Width="545" MaxLength="255"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="width: 110px;">
                        My Picture:
                    </td>
                    <td style="padding-left:15px; width: 350px;">
                        <input id="txtFakeUpload" type="text" style="width: 200px;" readonly />
                        <input id="btnFakeBrowse" type="button" value="Browse..." class="Btn-66" />
                        <div style="height:0px; width: 0px;">
                            <asp:FileUpload ID="FileUpload1" size="1" runat="server" Width="70px" Height="25px" Style="z-index: 10000;
                                opacity: 0; filter: alpha(opacity=0);" />
                        </div>
                    </td>
                    <td>
                        My Signature:
                    </td>
                </tr>
                <tr>
                    <td style="width: 110px;">
                        &nbsp;
                    </td>
                    <td style="padding-left:15px;">
                        <div style="width: 160px; margin-top: 5px;">
                            <table cellpadding="0" cellspacing="0" style="border: solid 1px #d8d8d8;">
                                <tr>
                                    <td id="tdPreview" style="width: 150px; height: 180px; background-color: #f6f6f6;">
                                        <div id="divPreviewX" style="margin-left: 15px;">
                                            <asp:Image ID="imgPhoto" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="text-align: center;">
                                Max Size: 120px * 150px</div>
                        </div>
                    </td>
                    <td>
                        <asp:TextBox ID="txtSignature" runat="server" TextMode="MultiLine" Height="100px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table style="margin-top: 2px;">
                <tr>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClientClick="return BeforeSave();" OnClick="btnSave_Click"/>
                        &nbsp;&nbsp;
                        <input id="btnDelete" runat="server" type="button" value="Delete" onclick="btnDelete_onclick()" class="Btn-66" />
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-top: 20px;" id="divAssignUser">
            <div id="divToolBar">
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                    <tr>
                        <td style="width: 40px;">
                            <asp:DropDownList ID="ddlAlphabets" runat="server" OnSelectedIndexChanged="ddlAlphabets_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width: 300px;">
                            <ul class="ToolStrip">
                                <li id="liCreate"><a id="aAddUser" href="javascript:aAddUser_onclick()">Add</a><span>|</span> </li>
                                <li id="liDelete"><a id="aRemove" href="javascript:aRemove_onclick()">Remove</a><span>|</span> 
                                </li>
                            </ul>
                        </td>
                        <td style="letter-spacing: 1px; text-align: right; font-size: 12px;">
                            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="15" CssClass="AspNetPager"
                                UrlPageIndexName="PageIndex" UrlPaging="True" FirstPageText="<<" LastPageText=">>"
                                NextPageText=">" PrevPageText="<" ShowCustomInfoSection="Never" ShowPageIndexBox="Never"
                                CustomInfoClass="PagerCustomInfo" PagingButtonClass="PagingButton" LayoutType="Table">
                            </webdiyer:AspNetPager>
                        </td>
                    </tr>
                </table>
            </div>

            <div id="divUserList" class="ColorGrid" style="width: 760px; margin-top: 5px;">
                <asp:GridView ID="gvUserList" runat="server" CssClass="GrayGrid" AutoGenerateColumns="false"
                    Width="100%" AllowSorting="true" EmptyDataText="There is no data in database."
                     CellPadding="3" GridLines="None">
                    <EmptyDataRowStyle CssClass="EmptyDataRow" HorizontalAlign="Center" />
                    <AlternatingRowStyle CssClass="EvenRow" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="CheckBoxHeader" ItemStyle-CssClass="CheckBoxColumn">
                            <HeaderTemplate>
                                <input type="checkbox" onclick="CheckAll(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input type="checkbox" title='<%# Eval("UserId")%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User" SortExpression="Name" ItemStyle-Wrap="false"
                            ItemStyle-Width="150">
                            <ItemTemplate>
                                    <%# Eval("FullName")%></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Role" SortExpression="RoleName"
                            ItemStyle-Wrap="false" ItemStyle-Width="100">
                            <ItemTemplate>
                                <%# Eval("RoleName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Branch" SortExpression="BranchName"
                            ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("BranchName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Enabled" SortExpression="EnabledName" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <%# Eval("EnabledName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnContactID" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hdnAddUserID" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hdnRemoveUserID" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hdnContactUserID" EnableViewState="true" runat="server" />
    <asp:HiddenField ID="hdnPageFrom" EnableViewState="true" runat="server" />
    <div id="divWaiting" style="display: none; padding: 5px;">
		    <table style="margin-left: auto; margin-right: auto;">
			    <tr>
				    <td>
					    <img id="imgWaiting" src="../images/waiting.gif" />
				    </td>
				    <td style="padding-left: 5px;">
					    <label id="WaitingMsg" style="color: #818892; font-weight: bold;"></label>
				    </td>
			    </tr>
		    </table>
	</div>
    <div id="divAddUser" title="Assign Contact Access" style="display: none;">
        <iframe id="ifrUserAdd" frameborder="0" scrolling="no" width="100%" height="100%">
        </iframe>
    </div>
    </form>
</body>
</html>
