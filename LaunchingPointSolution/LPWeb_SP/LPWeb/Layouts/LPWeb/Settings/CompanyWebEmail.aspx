<%@ Assembly Name="LPWeb, Version=1.0.0.0, Culture=neutral, PublicKeyToken=a2c3274f2ef313f2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Title="" Language="C#" MasterPageFile="~/_layouts/LPWeb/MasterPage/Settings.master"
    AutoEventWireup="true" Inherits="Settings_CompanyWebEmail" CodeBehind="CompanyWebEmail.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../css/style.tab.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .TabContent input.Btn-66, input.Btn-91
        {
            margin-right: 8px;
        }
        
        .TabContent table td
        {
            padding-top: 9px;
        }
    </style>
    <script src="../js/jquery.tab.js" type="text/javascript"></script>
    <script src="../js/jquery.validate.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $("#<%=btnFakeUploadHomePageLogo.ClientID %>").mouseover(function (event) {
                $("#<%=txtHomePageLogo.ClientID %>").offset({ top: $(this).offset().top, left: $(this).offset().left });
            });

            $("#<%=btnLogoforSubPages.ClientID %>").mouseover(function (event) {
                $("#<%=txtLogoforSubPages.ClientID %>").offset({ top: $(this).offset().top, left: $(this).offset().left });
            });

            $("#<%=txtHomePageLogo.ClientID %>").change(function () {
                $("#<%=txtFakeHomePageLogo.ClientID %>").val($(this).val());
            });
            $("#<%=txtLogoforSubPages.ClientID %>").change(function () {
                $("#<%=txtFakeLogoforSubPages.ClientID %>").val($(this).val());
            });


            $("[cid='chkReqAuth']").change(function () {

                //alert($(this).find("input:checked").size());

                if ($(this).find("input:checked").size()==0)
                {
                    $("[cid = 'txtEmailAccount']").attr("disabled", "disabled");
                    $("[cid = 'txtPassword']").attr("disabled", "disabled");
                    $("[cid = 'ddlEncryptMethod']").attr("disabled", "disabled");
                }
                else{
                    $("[cid = 'txtEmailAccount']").attr("disabled", "");
                    $("[cid = 'txtPassword']").attr("disabled", "");
                    $("[cid = 'ddlEncryptMethod']").attr("disabled", "");
                }

            });
            $("[cid='chkEWS']").change(function () {

                //alert($(this).find("input:checked").size());

                if ($(this).find("input:checked").size() == 0) {
                    $("[cid = 'txbEWSURL']").attr("disabled", "disabled");
                    $("[cid = 'txbEWSDomain']").attr("disabled", "disabled");
                    $("[cid = 'ddlEWSVersion']").attr("disabled", "disabled");
                }
                else {
                    $("[cid = 'txbEWSURL']").attr("disabled", "");
                    $("[cid = 'txbEWSDomain']").attr("disabled", "");
                    $("[cid = 'ddlEWSVersion']").attr("disabled", "");
                }

            });
        });
        $(document).ready(function () {
            DrawTab();
            var ua = $("#<%=btnFakeUploadHomePageLogo.ClientID %>");
            var ub = $("#<%=btnLogoforSubPages.ClientID %>");
            for (var i = 0; i < 2; i++) {
                ua.trigger("mouseover");
                ub.trigger("mouseover");
            }
        });

        //#region neo

        $(document).ready(function () {

            //alert($.browser.version);

            // add event
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").change(FileUpload1_onchange);
            $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").change(FileUpload2_onchange);

            // set image width and height
            $("#divPreviewX").width($("#ctl00_ctl00_PlaceHolderMain_MainArea_homeLogo").width());
            $("#divPreviewY").width($("#ctl00_ctl00_PlaceHolderMain_MainArea_subPageLogo").width());
        });

        function FileUpload1_onchange() {

            var FakeImagePath = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").val();
            var IsValid = ValidateImageExt(FakeImagePath);
            if (IsValid == false) {

                alert("Please select a valid image file.");

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFakeHomePageLogo").val("");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").replaceWith("<input type='file' name='ctl00$ctl00$PlaceHolderMain$MainArea$txtHomePageLogo' id='ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo' style='width:66px;position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);' />")
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").change(FileUpload1_onchange);
                return;
            }

            if ($.browser.msie == true) {

                //#region get width and height of image

                var ImageSize = GetImageSize_IE("ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo");

                var ImageWidth = ImageSize.ImageWidth;
                var ImageHeight = ImageSize.ImageHeight;

                //#endregion

                // it's fobidden to exceed
                if (ImageWidth > 300 || ImageHeight > 150) {

                    alert("The image that you tried to upload exceeded the size limitation (300x150).");

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFakeHomePageLogo").val("");
                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").replaceWith("<input type='file' name='ctl00$ctl00$PlaceHolderMain$MainArea$txtHomePageLogo' id='ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo' style='width:66px;position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);' />")
                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").change(FileUpload1_onchange);
                    return;
                }

                PreviewImage_IE78(ImageWidth, ImageHeight, "ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo", "tdPreview", "divPrevivew1");
            }
            else if ($.browser.mozilla == true) {

                FFPreviewImage_FileUpload1();
            }
        }

        function FileUpload2_onchange() {

            var FakeImagePath = $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").val();
            var IsValid = ValidateImageExt(FakeImagePath);
            if (IsValid == false) {

                alert("Please select a valid image file.");

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFakeLogoforSubPages").val("");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").replaceWith("<input type='file' name='ctl00$ctl00$PlaceHolderMain$MainArea$txtLogoforSubPages' id='ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages' style='width:66px;position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);' />")
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").change(FileUpload1_onchange);
                return;
            }

            if ($.browser.msie == true) {

                //#region get width and height of image

                var ImageSize = GetImageSize_IE("ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages");

                var ImageWidth = ImageSize.ImageWidth;
                var ImageHeight = ImageSize.ImageHeight;

                //#endregion

                // it's fobidden to exceed
                if (ImageWidth > 300 || ImageHeight > 150) {

                    alert("The image that you tried to upload exceeded the size limitation (300x150).");

                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFakeLogoforSubPages").val("");
                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").replaceWith("<input type='file' name='ctl00$ctl00$PlaceHolderMain$MainArea$txtLogoforSubPages' id='ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages' style='width:66px;position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);' />")
                    $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").change(FileUpload1_onchange);
                    return;
                }

                PreviewImage_IE78(ImageWidth, ImageHeight, "ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages", "tdPreview2", "divPrevivew2");
            }
            else if ($.browser.mozilla == true) {

                FFPreviewImage_FileUpload2();
            }
        }

        function ValidateImageExt(sImagePath) {

            var ext = sImagePath.substr(sImagePath.lastIndexOf('.'));
            ext = ext.replace(".", "");
            var AllowExt = "jpg|gif|jpeg|png|bmp";
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

            if (ImageWidth > 300 || ImageHeight > 150) {

                // set max width and length
                $("#" + divPreviewID).width(300);
                $("#" + divPreviewID).height(150);

                divPreview.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').sizingMethod = "scale";
            }
            else {

                // set width and height of divPreview
                $("#" + divPreviewID).width(ImageWidth);
                $("#" + divPreviewID).height(ImageHeight);

                divPreview.filters.item('DXImageTransform.Microsoft.AlphaImageLoader').sizingMethod = "image";
            }
        }

        //#endregion

        //#region preivew for firefox

        function FFPreviewImage_FileUpload1() {

            InitFFTempImage("ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo");

            GetImageSize("imgFFTempPreview", AfterGetImageSize_FileUpload1, "AfterGetImageSize_FileUpload1");
        }

        function FFPreviewImage_FileUpload2() {

            InitFFTempImage("ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages");

            GetImageSize("imgFFTempPreview", AfterGetImageSize_FileUpload2, "AfterGetImageSize_FileUpload2");
        }

        function InitFFTempImage(FileUploadID) {

            // append temp frame
            $("#divContainer").append("<table id='tbFFTempFrame' cellpadding='0' cellspacing='0' style='border: solid 1px #d8d8d8; visibility: hidden'><tr><td id='tdFFTempPreview'></td></tr></table>");

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

                if (MaxWidth > MaxHeight) {

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

            var MaxWidth = 300;
            var MaxHeight = 150;

            // it's fobidden to exceed
            if (ImageWidth > 300 || ImageHeight > 150) {

                alert("The image that you tried to upload exceeded the size limitation (300x150).");

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFakeHomePageLogo").val("");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").replaceWith("<input type='file' name='ctl00$ctl00$PlaceHolderMain$MainArea$txtHomePageLogo' id='ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo' style='width:66px;position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);' />")
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtHomePageLogo").change(FileUpload1_onchange);
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
        }

        function AfterGetImageSize_FileUpload2(ImageWidth, ImageHeight, src) {

            //alert("ImageWidth: " + ImageWidth + "; ImageHeight: " + ImageHeight);

            var MaxWidth = 300;
            var MaxHeight = 150;

            // it's fobidden to exceed
            if (ImageWidth > 300 || ImageHeight > 150) {

                alert("The image that you tried to upload exceeded the size limitation (300x150).");

                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtFakeLogoforSubPages").val("");
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").replaceWith("<input type='file' name='ctl00$ctl00$PlaceHolderMain$MainArea$txtLogoforSubPages' id='ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages' style='width:66px;position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);' />")
                $("#ctl00_ctl00_PlaceHolderMain_MainArea_txtLogoforSubPages").change(FileUpload1_onchange);
                return;
            }

            // get new width and height after scale
            var NewSize = GetImageScaleSize(MaxWidth, MaxHeight, ImageWidth, ImageHeight);
            //alert("NewSize.NewWidth: " + NewSize.NewWidth);
            //alert("NewSize.NewHeight: " + NewSize.NewHeight);

            var NewWidth = NewSize.NewWidth;
            var NewHeight = NewSize.NewHeight;

            // append image for preview
            $("#tdPreview2").empty();
            $("#tdPreview2").append("<div id='divPreview2'><img id='imgPreview2' alt='' src='' /></div>");

            // set width and height of image
            $("#imgPreview2").width(NewWidth);
            $("#imgPreview2").height(NewHeight);

            // set width and height of divPreview
            $("#divPreview2").width(NewWidth);
            $("#divPreview2").height(NewHeight);
            $("#divPreview2").css("margin-left", "auto");
            $("#divPreview2").css("margin-right", "auto");

            // set img.src
            var ImagePath = src;
            $("#imgPreview2").attr("src", ImagePath);
        }

        //#endregion

        //#endregion
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainArea" runat="Server">
    <div id="divModuleName" class="Heading">Company Setup</div>
    <div id="divCompanyTabs" style="margin-top: 15px;">
        <div class="JTab">
            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                <tr>
                    <td style="width: 10px;">&nbsp;</td>
                    <td>
                        <div id="tabs10">
                            <ul>
                                <li><a href="CompanyGeneral.aspx"><span>General</span></a></li>
                                <li id="current"><a href="CompanyWebEmail.aspx"><span>Web/Email</span></a></li>
                                <li><a href="CompanyPoint.aspx"><span>Sync</span></a></li>
                                <li><a href="CompanyAlerts.aspx"><span>Alert</span></a></li>
                                <li><a href="CompanyLoanPrograms.aspx"><span>Loan Programs</span></a></li>
                                <li><a href="CompanyTaskPickList.aspx"><span>Leads</span></a></li>
                               <%-- <li><a href="CompanyLeadSources.aspx"><span>Lead Sources</span></a></li>--%>
                                <li><a href="CompanyGlobalRules.aspx"><span>Global Rules</span></a></li>
                                <%--<li><a href="CompanyMarketing.aspx"><span>Marketing</span></a></li>--%>
                               <%-- <li><a href="CompanyLeadStatus.aspx"><span>Lead Status</span></a></li>--%>
                                <li><a href="CompanyReport.aspx"><span>Report</span></a></li>
                                <li><a href="CompanyMCT.aspx"><span>MCT</span></a></li>
                                <li><a href="CompanyPipelineViewLoansView.aspx"><span>Pipeline View</span></a></li>
                            </ul>
                        </div>
                    </td>
                </tr>
            </table>
            <div id="TabBody">
                <div id="TabLine1" class="TabLeftLine" style="width: 242px">&nbsp;</div>
                <div id="TabLine2" class="TabRightLine" style="width: 434px">&nbsp;</div>
                <div class="TabContent">
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 173px;">
                                <asp:CheckBox ID="chkEmailAlertsEnabled" runat="server" Checked="true" Text=" Email Alerts Enabled"></asp:CheckBox>
                            </td>
                            <td style="width: 160px;">
                             Email Interval
                                <asp:DropDownList ID="ddlEmailInterval" runat="server" Width="80">
                                                <asp:ListItem Text="Once daily " Value="1440"></asp:ListItem>
                                                <asp:ListItem Text="Twice daily " Value="720"></asp:ListItem>
                                                <asp:ListItem Text="4 hours " Value="240"></asp:ListItem>
                                                <asp:ListItem Text="2 hours " Value="120"></asp:ListItem>
                                                <asp:ListItem Text="1 hour " Value="60"></asp:ListItem>
                                                <asp:ListItem Text="30 minutes " Value="30"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvEmailInterval" runat="server" ErrorMessage="*"
                                                ControlToValidate="ddlEmailInterval"></asp:RequiredFieldValidator>
                            </td>
                         
                            <td style="width: 90px;">Default Alert Email </td>
                            <td>
                                 <asp:TextBox ID="txtDefaultAlertEmail" runat="server" Text="" Width="140px" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvDefaultAlertEmail" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtDefaultAlertEmail"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="revDefaultAlertEmail" runat="server" ErrorMessage="*"
                                                ControlToValidate="txtDefaultAlertEmail"  ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                           
                            <td colspan="4" ><asp:CheckBox ID="chkEnableEmailAuditTrail" runat="server" Text=" Email Audit Trail Enabled" /></td>

                        </tr>

                        <tr>
                            
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td style="width: 173px;">Email Relay Server</td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEmailRelayServer" runat="server" MaxLength="255" Width="290px"></asp:TextBox>
                            </td>

                            <td>SMTP Port No:</td>
                            <td>
                            <asp:TextBox ID="txtSMTPPortNo" runat="server" MaxLength="5" Width="30px"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationExpression="\d+" ControlToValidate="txtSMTPPortNo" runat="server" ErrorMessage="must be number"></asp:RegularExpressionValidator>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <asp:CheckBox ID="chkRequriesAuthentication" cid='chkReqAuth' Text=" Requries Authentication" runat="server" />
                            </td>
                            <td>Email Account:</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtEmailAccount" cid='txtEmailAccount' runat="server" MaxLength="255" Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                
                            </td>
                            <td>Password:</td>                            
                            <td>
                                <asp:TextBox ID="txtPassword" cid='txtPassword' runat="server"></asp:TextBox>
                            </td>
                            <td>Encrypt Method</td>
                            <td>
                                <asp:DropDownList ID="ddlEncryptMethod" cid='ddlEncryptMethod' runat="server">
                                <asp:ListItem Text="TLS" Value="TLS"></asp:ListItem>
                                <asp:ListItem Text="SSL" Value="SSL"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>

                    </table>
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 173px;">
                                <asp:CheckBox ID="chkSendEmailEWS" cid='chkEWS' runat="server" Checked="true" Text=" Send Emails via EWS"></asp:CheckBox>
                            </td>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="EWS URL:"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:TextBox ID="txbEWSURL" cid='txbEWSURL' runat="server" MaxLength="255" Width="527px"></asp:TextBox> 
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Label ID="Label3" runat="server" Text="EWS Domain:"></asp:Label>&nbsp;
                                <asp:TextBox ID="txbEWSDomain" cid='txbEWSDomain' runat="server" MaxLength="255" Width="527px"></asp:TextBox> 
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <asp:Label ID="Label2" runat="server" Text="EWS Version:"></asp:Label>
                                <asp:DropDownList ID="ddlEWSVersion" cid='ddlEWSVersion' runat="server">
                                <asp:ListItem Selected="True">2007</asp:ListItem>
                                <asp:ListItem>2010</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td></td></tr>
                        
                    </table>
                    


                    <table cellpadding="0" cellspacing="0" style=" margin-top:15px;">
                        <tr>
                            <td style="width: 750px;">
                            <hr />
                            </td>
                        </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 170px;">
                                
                            </td>
                            <td style="padding-top: 0px;">
                                <table>
                                    <tr>
                                        <td style="width: 285px;">
                                           
                                        </td>
                                        <td style="width: 70px;">
                                           
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>
                                           
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Pulse Website Home URL
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtWebsiteURL" runat="server" Text="" Width="582px" MaxLength="255"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="rfvWebsiteURL" runat="server" ErrorMessage="Please enter valid url."
                                    ControlToValidate="txtWebsiteURL" ValidationExpression="^(https?|ftp):\/\/(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Borrower/Partner Website URL
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPartnerWebsiteURL" runat="server" Text="" Width="582px" MaxLength="255"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="rfvPartnerWebsiteURL" runat="server" ErrorMessage="Please enter valid url."
                                    ControlToValidate="txtPartnerWebsiteURL" ValidationExpression="^(https?|ftp):\/\/(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Borrower/Partner Website<br />Greeting
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtPartnerWebsiteGreeting" runat="server" Text="" Width="582px"
                                    MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 170px; vertical-align: top;">Home Page Logo</td>
                            <td style="vertical-align: top;">
                                <asp:TextBox ID="txtFakeHomePageLogo" runat="server" Text="" ReadOnly="true" Width="200px" MaxLength="255"></asp:TextBox> &nbsp;<asp:Button ID="btnFakeUploadHomePageLogo" runat="server" Text="Browse..." CssClass="Btn-66" OnClientClick="return false;" CausesValidation="false" />
                                <asp:FileUpload ID="txtHomePageLogo" Style="position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);" runat="server" Width="66px"></asp:FileUpload>
                            </td>
                            <td>
                                <div>
                                    <table cellpadding="0" cellspacing="0" style="border: solid 1px #d8d8d8;">
                                        <tr>
                                            <td id="tdPreview" style="width: 300px; height: 150px; background-color: #f6f6f6;">
                                                <div id="divPreviewX" style="margin-left: auto; margin-right: auto;">
                                                    <asp:Image ID="homeLogo" runat="server" ImageUrl="~/_layouts/LPWeb/Settings/Image.ashx?photoID=1" Style="margin-left: auto; margin-right: auto;" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="text-align: center;">Max Size: 300px * 150px</div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">Logo for Sub-Pages</td>
                            <td style="vertical-align: top;">
                                <asp:TextBox ID="txtFakeLogoforSubPages" runat="server" Text="" ReadOnly="true" Width="200px" MaxLength="255"></asp:TextBox> &nbsp;<asp:Button ID="btnLogoforSubPages" runat="server" Text="Browse..." CssClass="Btn-66" OnClientClick="return false;" CausesValidation="false" />
                                <asp:FileUpload ID="txtLogoforSubPages" Style="position: absolute; z-index: 1000; opacity: 0; filter: alpha(opacity=0);" runat="server" Width="66px"></asp:FileUpload>
                            </td>
                            <td>
                                <div>
                                    <table cellpadding="0" cellspacing="0" style="border: solid 1px #d8d8d8;">
                                        <tr>
                                            <td id="tdPreview2" style="width: 300px; height: 150px; background-color: #f6f6f6;">
                                                <div id="divPreviewY" style="margin-left: auto; margin-right: auto;">
                                                    <asp:Image ID="subPageLogo" runat="server" ImageUrl="~/_layouts/LPWeb/Settings/Image.ashx?photoID=2" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    <div style="text-align: center;">Max Size: 300px * 150px</div>
                                </div>
                            </td>
                        </tr>
                        <tr style="height: 20px;">
                            <td>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td colspan="3">
                                <asp:Button ID="btnTestEmail" runat="server" Text="Test Email" CssClass="Btn-91"
                                    OnClick="btnTestEmail_Click" CausesValidation="false" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="Btn-66" OnClick="btnSave_Click" /><asp:Button
                                    ID="btnCancel" runat="server" Text="Cancel" CssClass="Btn-66" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="divContainer"></div>
    </div>
</asp:Content>