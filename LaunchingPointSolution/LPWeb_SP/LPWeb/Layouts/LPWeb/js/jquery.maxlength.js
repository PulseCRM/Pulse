/*
 * Textarea Maxlength Setter JQuery Plugin 
 * Version 1.0
 * Copyright (c) 2008 Viral Patel
 * website : http://viralpatel.net/blogs
*/
jQuery.fn.maxlength = function(max){	
	$(this).keypress(function(event){ 
		var key = event.which;
		//all keys including return.
		if(key >= 33 || key == 13) {
		    //var maxLength = $(this).attr("maxlength");
		    var maxLength = max;
			var length = this.value.length;
			if(length >= maxLength) {
				event.preventDefault();
			}
		}
	});

    $(this).bind("paste", function () { return false; });
}
