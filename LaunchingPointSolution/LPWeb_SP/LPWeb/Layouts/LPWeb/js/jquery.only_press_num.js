/*
jquery.onlypressnum.js 1.1
code by Deeka
date in 2008-08-06 1:31
QQ:80140281
e-mail:huangdijia@163.com
技术群:24810664
用法：
$("#id or .类").onlypressnum();   //就是如此简单
作用：
限定指定容器只能输入数字，并不能操作复制/粘贴/右击/选择及其他输入法。
修正:
1.IE下禁止ctrl+c/v操作
2.firefox下可以选择内容
3.通过拖动，仍然可以把非数字内容输入到容器里(鹏衷铃已经解决)
未修正：
*/
jQuery.fn.onlypressnum = function () {
    $(this).css({ imeMode: "disabled", '-moz-user-select': "none" });
    $(this).bind("keypress", function (e) {
        if (e.ctrlKey == true || e.shiftKey == true)
            return false;
        if ((e.which >= 48 && e.which <= 57 && e.ctrlKey == false && e.shiftKey == false) || e.which == 0 || e.which == 8)
            return true;
        else if (e.ctrlKey == true && (e.which == 99 || e.which == 118))
            return false;
        else
            return false;
    })
 .bind("contextmenu", function () { return false; })
 .bind("selectstart", function () { return false; })
 .bind("drop", function () { return false; })
 .bind("paste", function () { return false; });
};

jQuery.fn.OnlyInt = function () {
    $(this).css({ imeMode: "disabled", '-moz-user-select': "none" });
    $(this).bind("keypress", function (e) {
        if (e.ctrlKey == true || e.shiftKey == true)
            return false;
        if ((e.which >= 48 && e.which <= 57 && e.ctrlKey == false && e.shiftKey == false) || e.which == 0 || e.which == 8)
            return true;
        else if (e.ctrlKey == true && (e.which == 99 || e.which == 118))
            return false;
        else if (e.which == 45 && $(this).val().length == 0) {  // '-' only allowed at start
            return true;
        }
        else
            return false;
    })
 .bind("contextmenu", function () { return false; })
 .bind("selectstart", function () { return false; })
 .bind("drop", function () { return false; })
 .bind("paste", function () { return false; });
};


jQuery.fn.OnlyDigit = function () {
    $(this).css({ imeMode: "disabled", '-moz-user-select': "none" });
    $(this).bind("keypress", function (e) {
        if (e.ctrlKey == true || e.shiftKey == true)
            return false;
        if ((e.which >= 48 && e.which <= 57 && e.ctrlKey == false && e.shiftKey == false) || e.which == 0 || e.which == 8)
            return true;
        else if (e.ctrlKey == true && (e.which == 99 || e.which == 118))
            return false;
        else
            return false;
    })
 .bind("contextmenu", function () { return false; })
 .bind("selectstart", function () { return false; })
 .bind("drop", function () { return false; })
    // .bind("paste", function() { return false; });
};

// ReadOnly Input-File
jQuery.fn.ReadOnly = function () {
    $(this).css({ imeMode: "disabled", '-moz-user-select': "none" });
    $(this).bind("keypress", function (e) {
        if (e.ctrlKey == true || e.shiftKey == true)
            return false;
        if (e.which == 8) {     // backspace
            return false;
        }
        //        if ((e.which >= 48 && e.which <= 57 && e.ctrlKey == false && e.shiftKey == false) || e.which == 0 || e.which == 8)
        //            return true;
        else if (e.ctrlKey == true && (e.which == 99 || e.which == 118))
            return false;
        else
            return false;
    })
    $(this).bind("keydown", function (e) {
        if (e.which == 8) {     // backspace
            return false;
        }
    })
 .bind("contextmenu", function () { return false; })
 .bind("selectstart", function () { return false; })
 .bind("drop", function () { return false; })
 .bind("paste", function () { return false; });
};