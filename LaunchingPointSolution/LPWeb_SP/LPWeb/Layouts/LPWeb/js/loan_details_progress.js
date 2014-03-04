$(document).ready(function () {

    //#region init scroller
    var DisplayCount = 6;
    var $container = $("#divLoanDetailsProgress");
    var x = $container.wtScroller({
        num_display: DisplayCount,
        slide_width: 108,
        slide_height: 32,
        slide_margin: 0,
        button_width: 30,
        ctrl_height: 20,
        margin: 10,
        auto_scroll: false,
        delay: 4000,
        scroll_speed: 1000,
        easing: "easeInOutSine",
        move_one: false,
        display_buttons: true,
        display_scrollbar: false,
        display_indexes: false,
        display_caption: true,
        mouseover_caption: false,
        caption_align: "bottom",
        caption_position: "inside",
        cont_nav: true,
        shuffle: false,
        lightbox_on: false,
        afterScroll: AfterScroll,
        beforeScroll: BeforeScroll
    });

    //#endregion

    if ($("#divLoanDetailsProgress .slides ul li").length == 1) {

        return;
    }

    //#region reset scroller width

    var ItemCountPer = 6;
    if ($("#divLoanDetailsProgress .slides ul li").length < DisplayCount) {

        ItemCountPer = $("#divLoanDetailsProgress .slides ul li").length;
    }
    $("#divLoanDetailsProgress .wt-scroller").width($(".wt-scroller").width() - (ItemCountPer - 1) * 15);
    $("#divLoanDetailsProgress .slides").width($(".slides").width() - (ItemCountPer - 1) * 15);

    //#endregion

    //#region move image to left -15

    $("#divLoanDetailsProgress .slides ul li").each(function (i) {

        if (i > 0) {

            var LeftPos = -15 * i;
            $(this).css("position", "relative");
            $(this).css("left", LeftPos);

            //alert($(this).html());
        }
    });

    //#endregion

    //#region the last one per scroll

    var LastIndexPerScroll = DisplayCount - 1;
    if ($("#divLoanDetailsProgress .slides ul li").length < DisplayCount) {

        LastIndexPerScroll = $("#divLoanDetailsProgress .slides ul li").length - 1;
    }

    var Left = -15 * (LastIndexPerScroll + 1) + 15;
    $("#divLoanDetailsProgress .slides ul li").eq(LastIndexPerScroll + 1).css("left", Left);

    //#endregion

});

function BeforeScroll(prevSlots, numItems, numDisplay) {

    $("#divLoanDetailsProgress .slides ul li").each(function (i) {

        // move image to left -15px
        if (i > 0) {

            var LeftPos = -15 * i;
            $(this).css("position", "relative");
            $(this).css("left", LeftPos);

        }
    });
}

function AfterScroll(prevSlots, numItems, numDisplay) {

    // stage items
    var StageItems = $("#divLoanDetailsProgress .slides ul li");

    if(prevSlots > 0){
    
        // first stage per scroll
        var Pos1 = prevSlots - 1;
        var Left1 = -15 * Pos1 - 15;
        StageItems.eq(Pos1).css("left", Left1);
    }
    
    var Pos2 = prevSlots + 6;
    var Left2 = -15 * Pos2 + 15;
    StageItems.eq(Pos2).css("left", Left2);

}

function GetProgressImageFileName_Left(ImageSrc) {

    var ImageFileName = ImageSrc.substring(ImageSrc.lastIndexOf('/') + 1);
    //alert("ImageFileName: " + ImageFileName);

    var ImageFileNameWithoutExt = ImageFileName.replace(".gif", "");
    //alert("ImageFileNameWithoutExt: " + ImageFileNameWithoutExt);

    var ImageFileNameKeys = ImageFileNameWithoutExt.split("_");

    var ImageFileName_Mide = "";
    if (ImageFileNameKeys.length == 3) {

        ImageFileName_Mide = ImageFileNameKeys[0] + "_Left_" + ImageFileNameKeys[2] + ".gif";
    }
    else {

        ImageFileName_Mide = ImageFileNameKeys[0] + "_Left.gif";
    }

    //alert(ImageFileName_Mide);

    return ImageFileName_Mide;
}

function GetProgressImageFileName_Mid(ImageSrc) {

    var ImageFileName = ImageSrc.substring(ImageSrc.lastIndexOf('/') + 1);
    //alert("ImageFileName: " + ImageFileName);

    var ImageFileNameWithoutExt = ImageFileName.replace(".gif", "");
    //alert("ImageFileNameWithoutExt: " + ImageFileNameWithoutExt);

    var ImageFileNameKeys = ImageFileNameWithoutExt.split("_");

    var ImageFileName_Mide = "";
    if (ImageFileNameKeys.length == 3) {

        ImageFileName_Mide = ImageFileNameKeys[0] + "_Mid_" + ImageFileNameKeys[2] + ".gif";
    }
    else {

        ImageFileName_Mide = ImageFileNameKeys[0] + "_Mid.gif";
    }

    //alert(ImageFileName_Mide);

    return ImageFileName_Mide;
}

function GetProgressImageFileName_Right(ImageSrc) {

    var ImageFileName = ImageSrc.substring(ImageSrc.lastIndexOf('/') + 1);
    //alert("ImageFileName: " + ImageFileName);

    var ImageFileNameWithoutExt = ImageFileName.replace(".gif", "");
    //alert("ImageFileNameWithoutExt: " + ImageFileNameWithoutExt);

    var ImageFileNameKeys = ImageFileNameWithoutExt.split("_");

    var ImageFileName_Mide = "";
    if (ImageFileNameKeys.length == 3) {

        ImageFileName_Mide = ImageFileNameKeys[0] + "_Right_" + ImageFileNameKeys[2] + ".gif";
    }
    else {

        ImageFileName_Mide = ImageFileNameKeys[0] + "_Right.gif";
    }

    //alert(ImageFileName_Mide);

    return ImageFileName_Mide;
}