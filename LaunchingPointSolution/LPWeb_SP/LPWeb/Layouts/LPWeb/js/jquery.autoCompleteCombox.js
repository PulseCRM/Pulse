(function ($) {
    $.widget("ui.combobox", {
        _create: function () {
            var self = this,
					select = this.element.hide(),
					selected = select.children(":selected"),
					value = selected.val() ? selected.text() : "";
            var input = this.input = $("<input id='" + select.attr('id') + "_accTextBox' style='color: #818892; border: solid 1px #d9d8d8; font:11px, Arial; padding: 2px 5px 2px 2px;'>")
					.insertAfter(select)
					.val(value)
					.autocomplete({
					    delay: 0,
					    minLength: 0,
					    source: function (request, response) {
					        var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
					        response(select.children("option").map(function () {
					            var text = $(this).text();
					            if (this.value && (!request.term || matcher.test(text)))
					                return {
					                    label: text.replace(
											new RegExp(
												"(?![^&;]+;)(?!<[^<>]*)(" +
												$.ui.autocomplete.escapeRegex(request.term) +
												")(?![^<>]*>)(?![^&;]+;)", "gi"
											), "<strong>$1</strong>"),
					                    value: text,
					                    option: this
					                };
					        }));
					    },
					    select: function (event, ui) {
					        ui.item.option.selected = true;
					        self._trigger("selected", event, {
					            item: ui.item.option
					        });
					    },
					    change: function (event, ui) {
					        if (!ui.item) {
					            var matcher = new RegExp("^" + $.ui.autocomplete.escapeRegex($(this).val()) + "$", "i"),
									valid = false;
					            var sval = "";
					            select.children("option").each(function () {
					                if ($(this).text().match(matcher)) {
					                    this.selected = valid = true;
					                    sval = $(this).text();
					                    return false;
					                }
					            });
					            if (!valid) {
					                // remove invalid value, as it didn't match anything
					                $(this).val("");
					                select.val("");
					                input.data("autocomplete").term = "";
					                if (input.data('invalidValueHandler')) {
					                    input.data('invalidValueHandler')();
					                }
					                return false;
					            }

					            self._trigger("selected", event, {
					                item: { value: sval }
					            });
					        }
					    }
					})
					.addClass("ui-widget ui-widget-content ui-corner-left");

            input.data("autocomplete")._renderItem = function (ul, item) {
                return $("<li></li>")
						.data("item.autocomplete", item)
						.append("<a style='font: 11px, Arial; color: #818892;'>" + item.label + "</a>")
						.appendTo(ul);
            };

            // check ie version
            var btnCode = "<button type='button' style='width: 18px; height: 20px; margin-left: -1px;'></button>";

            if ($.browser.msie == true) {

                //alert($.browser.version);
                if ($.browser.version == 7) {

                    btnCode = "<button style='width: 18px; height: 20px; margin-left: -1px; position: relative; top: 0px;'></button>";
                }
                else if ($.browser.version == 8 || $.browser.version == 9 || $.browser.version == 10) {

                    btnCode = "<button style='width: 18px; height: 20px; margin-left: -1px; position: relative; top: 6px;'></button>";
                }
            }
            else if ($.browser.mozilla == true) {

                btnCode = "<button type='button' style='width: 18px; height: 20px; margin-left: -1px; position: relative; top: 1px;'></button>";
            } else {
                btnCode = "<button type='button' style='width: 18px; height: 20px; margin-left: -1px; position: relative; top: 6px;'></button>";
            }
            var op = {
                icons: {
                    primary: "ui-icon-triangle-1-s"
                },
                text: false
            }
            if ($.browser.mozilla == true) {

                op = {
                    icons: {
                        primary: "ui-icon-triangle-1-sx"
                    },
                    text: false
                }
            }
            this.button = $(btnCode)
					.attr("tabIndex", -1)
					.attr("title", "Show All Items")
					.insertAfter(input)
					.button(op)
					.removeClass("ui-corner-all")
					.addClass("ui-corner-right ui-button-icon")
					.click(function () {
					    // close if already visible
					    if (input.autocomplete("widget").is(":visible")) {
					        input.autocomplete("close");
					        return false;
					    }

					    // pass empty string as value to search for, displaying all results
					    input.autocomplete("search", "");
					    input.focus();

					    return false;
					});
        },

        destroy: function () {
            this.input.remove();
            this.button.remove();
            this.element.show();
            $.Widget.prototype.destroy.call(this);
        }
    });
})(jQuery);