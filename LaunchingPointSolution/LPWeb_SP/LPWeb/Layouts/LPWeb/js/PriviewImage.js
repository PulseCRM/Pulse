//preview image
(function ($) {
    $.fn.PreviewImage = function (options) {
        var Default = {
            ImageClientId: "",
            MaxWidth: 300,
            MaxHeight: 80
        };
        $.extend(true, Default, options);
        return this.each(function () {
            if (Default.ImageClientId != "") {
                $(this).unbind("change");
                $(this).change(function () {

                    if ($.browser.msie) {

                        if ($(this).val().indexOf(".tif") > 0) {
                            alert("Pulse does not support pictures that are in TIF format.\n Please use another type of format and try again.");
                            return;
                        }

                    }
                    else {

                        if ($(this)[0].files[0].getAsDataURL().indexOf("tiff;") > 0) {
                            alert("Pulse does not support pictures that are in TIF format.\n Please use another type of format and try again.");
                            return;
                        }

                    }

                    if ($(this).val() == "") {
                        $("#" + Default.ImageClientId).parent("div").hide();
                        return;
                    }
                    else {
                        $("#" + Default.ImageClientId).parent("div").show();
                    }
                    if ($.browser.msie) {
                        $("#" + Default.ImageClientId).attr("src", $(this).val());
                    }
                    else {
                        $("#" + Default.ImageClientId).attr("src", $(this)[0].files[0].getAsDataURL());
                    }

//                    alert($("#" + Default.ImageClientId).attr("width"));
//                    alert($("#" + Default.ImageClientId).attr("height"));

                    if ($.browser.msie && $.browser.version > 6) {
                        $("#" + Default.ImageClientId).hide();
                        $("#" + Default.ImageClientId).parent("div").css({ 'z-index': '999',
                            'filter': 'progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=scale)',
                            'max-width': $("#" + Default.ImageClientId).attr("width") + 'px', 'max-height': $("#" + Default.ImageClientId).attr("height") + 'px',
                            'width': $("#" + Default.ImageClientId).attr("width") + 'px', 'height': $("#" + Default.ImageClientId).attr("height") + 'px'
                        });
                        var div = $("#" + Default.ImageClientId).parent("div")[0];
                        div.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = $("#" + Default.ImageClientId).attr("src");
                    }
                });

                $("#" + Default.ImageClientId).load(function () {

                    if ($(this).attr("src").indexOf(".tif") > 0) {
                        
                        return;
                    }

                    var image = new Image();
                    image.src = $(this).attr("src");
                    $(this).attr("width", $("#" + Default.ImageClientId).attr("width"));
                    $(this).attr("height", $("#" + Default.ImageClientId).attr("height"));
                    $(this).attr("alt", Default.MaxWidth + "x" + Default.MaxHeight);
                });
            }
        });
    };
})(jQuery);

