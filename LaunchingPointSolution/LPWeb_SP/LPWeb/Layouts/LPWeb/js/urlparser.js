if (typeof Poly9 == 'undefined') {
    var Poly9 = {};
}
Poly9.URLParser = function(url) {
    this._fields = {
        'Username': 4,'Password': 5,'Port': 7,'Protocol': 2,'Host': 6,'Pathname': 8,'URL': 0,'Querystring': 9,'Fragment': 10
    };
    this._values = {};
    this._regex = null;
    this.version = 0.1;
    this._regex = /^((\w+):\/\/)?((\w+):?(\w+)?@)?([^\/\?:]+):?(\d+)?(\/?[^\?#]+)?\??([^#]+)?#?(\w*)/;
    for (var f in this._fields) {
        this['get' + f] = this._makeGetter(f);
    }
    if (typeof url != 'undefined') {
        this._parse(url);
    }
}
Poly9.URLParser.prototype.setURL = function(url) {
    this._parse(url);
}

Poly9.URLParser.prototype._initValues = function() {
    for (var f in this._fields) {
        this._values[f] = '';
    }
}
Poly9.URLParser.prototype._parse = function(url) {
    this._initValues();
    var r = this._regex.exec(url);
    if (!r) throw "DPURLParser::_parse -> Invalid URL";
    for (var f in this._fields) if (typeof r[this._fields[f]] != 'undefined') {
        this._values[f] = r[this._fields[f]];
    }
}
Poly9.URLParser.prototype._makeGetter = function(field) {
    return function() {
        return this._values[field];
    }
}
// 获取参数
function GetQueryString1(sParameterName) {
    var strReturn = "";
    var UrlParser = new Poly9.URLParser(window.location.href);
    var strQueryString = UrlParser.getQuerystring();
    if (strQueryString == "") {
        return "";
    }
    var aQueryString = strQueryString.split("&");
    for (var iParam = 0; iParam < aQueryString.length; iParam++) {
        if (aQueryString[iParam].indexOf(sParameterName + "=") > -1) {
            var aParam = aQueryString[iParam].split("=");
            strReturn = aParam[1];
            break;
        }
    }
    if (strReturn == "") {
        return strReturn;
    }
    return decodeURIComponent(strReturn);
}
function GetQueryString2(sParameterName, sUrl) {
    if (sUrl == "") {
        return "";
    }
    var strReturn = "";
    var UrlParser = new Poly9.URLParser(sUrl);
    var strQueryString = UrlParser.getQuerystring();
    if (strQueryString == "") {
        return "";
    }
    var aQueryString = strQueryString.split("&");
    for (var iParam = 0; iParam < aQueryString.length; iParam++) {
        if (aQueryString[iParam].indexOf(sParameterName + "=") > -1) {
            var aParam = aQueryString[iParam].split("=");
            strReturn = aParam[1];
            break;
        }
    }
    if (strReturn == "") {
        return strReturn;
    }
    return decodeURIComponent(strReturn);
}