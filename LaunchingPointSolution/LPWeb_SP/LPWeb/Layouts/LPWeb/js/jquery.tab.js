function DrawTab() {

    //alert($.browser.msie);
    //alert($.browser.webkit );
    //alert($.browser.version);

    if ($.browser.msie && $.browser.version == 7) {

        var liOffset = $("#current").offset();
        var divOffset = $("#TabBody").offset();
        if (liOffset.length == 0 || divOffset.length) {
            return;
        }

        var LeftLineWidth = liOffset.left - divOffset.left + 1;

        var liRightPos = liOffset.left + $("#current").width();
        var divRightPos = divOffset.left + $("#TabBody").width();

        var RightLineWidt = divRightPos - liRightPos + 3;

        $("#TabLine1").css("width", LeftLineWidth);
        $("#TabLine2").css("width", RightLineWidt);
    }
    else if ($.browser.webkit || $.browser.mozilla || $.browser.msie && $.browser.version >= 8) {

        var liOffset = $("#current a").offset();
        var divOffset = $("#TabBody").offset();
        if (liOffset.length == 0 || divOffset.length) {
            return;
        }

        var LeftLineWidth = liOffset.left - divOffset.left + 1;

        var liRightPos = liOffset.left + $("#current a").width();
        var divRightPos = divOffset.left + $("#TabBody").width();
        var RightLineWidt = divRightPos - liRightPos - 1;


        $("#TabLine1").css("width", LeftLineWidth);
        $("#TabLine2").css("width", RightLineWidt);
    }
}


function DrawSubTab() {

    //    alert($.browser.msie);
    //    alert($.browser.version);

    if ($.browser.msie && $.browser.version == 7) {

        var liOffset = $("#currentsub").offset();
        var divOffset = $("#TabsubBody").offset();
        if (liOffset.length == 0 || divOffset.length) {
            return;
        }

        var LeftLineWidth = liOffset.left - divOffset.left + 1;

        var liRightPos = liOffset.left + $("#currentsub").width();
        var divRightPos = divOffset.left + $("#TabsubBody").width();

        var RightLineWidt = divRightPos - liRightPos + 3;

        $("#TabsubLine1").css("width", LeftLineWidth);
        $("#TabsubLine2").css("width", RightLineWidt);
    }
    else if ($.browser.webkit || $.browser.mozilla || $.browser.msie && $.browser.version >= 8) {

        var liOffset = $("#currentsub a").offset();
        var divOffset = $("#TabsubBody").offset();
        if (liOffset.length == 0 || divOffset.length) {
            return;
        }

        var LeftLineWidth = liOffset.left - divOffset.left + 1;

        var liRightPos = liOffset.left + $("#currentsub a").width();
        var divRightPos = divOffset.left + $("#TabsubBody").width();
        var RightLineWidt = divRightPos - liRightPos - 1;


        $("#TabsubLine1").css("width", LeftLineWidth);
        $("#TabsubLine2").css("width", RightLineWidt);
    }
}