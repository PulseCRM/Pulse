// set style of selected menu item of left navigation menu
function SetSelectedMenuItemStyle(index) {

    $("#LeftNavMenuList li").eq(index).prepend("<img src=\"../images/LeftNavMenu-Arrow.gif\" />");
    $("#LeftNavMenuList li").eq(index).attr("class", "SelectedMenuItem");
}

function isID(str) {

    var Regex = /^[1-9][0-9]*$/;
    return Regex.test(str);
}

function OpenWindow(mypage, myname, iWidth, iHeight, scroll, pos) {

    if (pos == "random") {
        LeftPosition = (screen.width) ? Math.floor(Math.random() * (screen.width - iWidth)) : 100; TopPosition = (screen.height) ? Math.floor(Math.random() * ((screen.height - iHeight) - 75)) : 100;
    }

    if (pos == "center") {
        LeftPosition = (screen.width) ? (screen.width - iWidth) / 2 : 100; TopPosition = (screen.height) ? (screen.height - iHeight) / 2 : 100;
    }
    else if ((pos != "center" && pos != "random") || pos == null) {
        LeftPosition = 0; TopPosition = 20
    }

    settings = 'width=' + iWidth + ',height=' + iHeight + ',top=' + TopPosition + ',left=' + LeftPosition + ',scrollbars=' + scroll + ',location=no,directories=no,status=no,menubar=no,toolbar=no,resizable=no';

    var PopWindow = window.open(mypage, myname, settings);

    return PopWindow;
}

function DisableLink(LinkID) {

    $("#" + LinkID).attr("disabled", "true");
    $("#" + LinkID).removeAttr("href");
    $("#" + LinkID).css("text-decoration", "none");
}

function EnableLink(LinkID, href) {

    $("#" + LinkID).attr("disabled", "");
    $("#" + LinkID).attr("href", href);
    $("#" + LinkID).css("text-decoration", "");
}