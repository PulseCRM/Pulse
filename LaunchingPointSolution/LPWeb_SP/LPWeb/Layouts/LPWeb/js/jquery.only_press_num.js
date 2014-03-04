/*
jquery.onlypressnum.js 1.1
code by Deeka
date in 2008-08-06 1:31
QQ:80140281
e-mail:huangdijia@163.com
����Ⱥ:24810664
�÷���
$("#id or .��").onlypressnum();   //������˼�
���ã�
�޶�ָ������ֻ���������֣������ܲ�������/ճ��/�һ�/ѡ���������뷨��
����:
1.IE�½�ֹctrl+c/v����
2.firefox�¿���ѡ������
3.ͨ���϶�����Ȼ���԰ѷ������������뵽������(�������Ѿ����)
δ������
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