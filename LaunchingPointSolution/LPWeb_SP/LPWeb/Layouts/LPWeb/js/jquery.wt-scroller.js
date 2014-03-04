/**
 * jQuery Image Scroller
 * Copyright (c) 2010 Allan Ma (http://codecanyon.net/user/webtako)
 * Version: 1.23 (01/17/2011)
 */
;(function($) {
	$.fn.wtScroller = function(params) {		
		var TOP = "top";
		var BOTTOM = "bottom";
		var OUTSIDE = "outside";
		var INSIDE = "inside";
		var SLIDE_OPACITY = 0.9;		
		var BUTTON_OPACITY = 0.7;
		var SCROLL_SPEED = 1000;
		var DEFAULT_DURATION = 600;
		var ANIMATE_SPEED = 300;
		var DEFAULT_DELAY = 4000;
		var LIGHTBOX_SIZE = 250;
		var LIGHTBOX_MARGIN = 40;
		var UPDATE_BUTTONS ="update_buttons";
		var UPDATE_THUMB = 	"update_thumb";
		var UPDATE_INDEX = 	"update_index";
		var UPDATE_TEXT =	"update_text";
		var START_TIMER = 	"start_timer";
		var IE6_INIT = 		"ie6_init";
		var IE6_CLEANUP =	"ie6_cleanup";
		
		//Class Lightbox
		function Lightbox(scroller, opts) {
			var rotateOn =		opts.rotate;
			var delay = 	 	getPosNumber(opts.delay, DEFAULT_DELAY);
			var duration = 	 	getPosNumber(opts.transition_speed, DEFAULT_DURATION);
			var textAlign =  	opts.caption_align.toLowerCase();
			var displayText =	opts.display_caption;
			var displayTimer = 	opts.display_timer;
			var displayNum	= 	opts.display_number;						
			var contNav = 	 	opts.cont_nav;
			var autoFit = 	 	opts.auto_fit;
			var easing =		opts.easing;
			
			var $overlay;
			var $lightbox;
			var $textPanel;
			var $preloader;
			var $cPanel;
			var $playBtn;
			var $infoPanel;
			var $innerBox;
			var $backPane;
			var $backBtn;
			var $fwdPane;
			var $fwdBtn;
			var $timer;
			var $item;
			
			var currIndex;		
			var numItems;
			var rotate;
			var padding;
			var timerId = null;			
			var content =  "<div id='overlay'></div>\
							<div id='lightbox'>\
								<div class='preloader'></div>\
								<div class='inner-box'>\
									<div class='content'></div>\
									<div class='timer'></div>\
									<div class='desc'><div class='inner-text'></div></div>\
									<div class='btn-panel'><div id='back-btn'></div></div>\
									<div class='btn-panel'><div id='fwd-btn'></div></div>\
								</div>\
								<div class='cpanel'>\
									<div id='play-btn'></div>\
									<div id='info'></div>\
									<div id='close-btn'></div>\
								</div>\
							</div>";
			
			//init light box
			var init = function() {
				currIndex = 0;
				numItems = scroller.getNumItems();			
				$("body").append(content);
				$overlay = $("#overlay").click(closeLightbox);
				initLightbox();	
				msie6Check();
			}
						
			//init lightbox
			var initLightbox = function() {
				$lightbox =		$("#lightbox");
				$preloader = 	$lightbox.find("div.preloader");	
				$innerBox = 	$lightbox.find("div.inner-box");
				$timer =		$innerBox.find("div.timer");
				$textPanel =	$innerBox.find("div.desc");							
				$backBtn = 		$innerBox.find("#back-btn");	
				$fwdBtn = 		$innerBox.find("#fwd-btn");
				$backPane = 	$innerBox.find("div.btn-panel").has($backBtn);												
				$fwdPane =  	$innerBox.find("div.btn-panel").has($fwdBtn);
				$cPanel = 		$lightbox.find("div.cpanel").bind("click", cpanelClick);				
				$playBtn = 		$cPanel.find("#play-btn");
				$infoPanel = 	$cPanel.find("#info");	
				padding = $lightbox.outerWidth() - $lightbox.width();
				
				if (!displayNum) {
					$infoPanel.hide();
				}
				
				if (displayText) {
					$lightbox.bind(UPDATE_TEXT, updateTextPanel);
				}
				else {
					$textPanel.remove();
				}
				
				if (rotateOn) {		
					rotate = false;
					$playBtn.toggleClass("pause", rotate);
					$lightbox.bind(START_TIMER, startTimer);
					initTimerBar();
				}
				else {
					$timer.remove();
					$playBtn.remove();
				}	
			}
			
			//init timer bar
			var initTimerBar = function() {
				$timer.data("pct", 1);
				if (displayTimer) {
					$timer.css(textAlign == BOTTOM ? "top" : "bottom", 0).css("visibility","visible");
				}
				else {
					$timer.css({visibility:"hidden"});
				}
			}
			
			//open lightbox
			this.display = function(i) {
				currIndex = i;
				$lightbox.data("visible", true).trigger(IE6_INIT);
				$(document).unbind().bind("keyup", keyClose);			 				
				$overlay.stop(true).show();
				$lightbox.css({width:LIGHTBOX_SIZE, height:LIGHTBOX_SIZE, "margin-left":-LIGHTBOX_SIZE/2, "margin-top":-LIGHTBOX_SIZE/2}).show();								   
				loadContent(currIndex);
			}
			
			//close lightbox 
			var closeLightbox = function() {
				resetTimer();			
				$lightbox.data("visible", false).trigger(IE6_CLEANUP);
				$(window, document).unbind();
				disableCtrl();
				$textPanel.stop(true).hide();
				$lightbox.stop(true).hide();
				$overlay.stop(true).fadeOut("fast");		
				scroller.onFocus();
			}
			
			//load content
			var loadContent = function(i) {					
				$item = scroller.getItem(i);
				disableCtrl();
				$textPanel.stop(true).hide();
				
				var $img = $("<img/>");
				$("div.content", $innerBox).empty().append($img);
				$img.css("opacity", 0).attr("src", $item.find(">a:first").attr("href"));
				if (!$img[0].complete || $img[0].width == 0) {
					$preloader.show();
					$img.load(
						function() {
							$preloader.hide();	
							displayContent($img);
						}
					);
				}
				else {		
					$preloader.hide();
					displayContent($img);					
				}
			}
			
			//display content
			var displayContent = function($img) {
				if ($lightbox.data("visible")) {
					if (autoFit) {
						resizeImg($img[0]);
					}
					var width  = $img[0].width;
					var innerHeight = $img[0].height;
					var outerHeight = innerHeight + $cPanel.height();
					var newDuration = getDuration(width, outerHeight);
					var marginLeft =  -(width + padding)/2;
					var marginTop = -(outerHeight + (padding/2))/2;					
					$lightbox.stop(true).animate({"margin-left":marginLeft, "margin-top":marginTop, 
												   width:width, height:outerHeight}, newDuration, easing, 
							function() {
								$innerBox.height(innerHeight);
								$infoPanel.html((currIndex + 1) + "/" + numItems);								
								$lightbox.trigger(UPDATE_TEXT);
								enableCtrl();
								$img.animate({opacity:1}, "normal", 
										function() {
											if (jQuery.browser.msie) { 
												this.style.removeAttribute('filter'); 
											}											
											$lightbox.trigger(START_TIMER);
										});									
							}
					);
				}
			}
			
			//display text panel
			var updateTextPanel = function() {
				var text = $item.find(">p:first").html();		
				if (text) {
					$textPanel.find("div.inner-text").html(text);	
					$textPanel.stop().css("top", textAlign == BOTTOM ? $innerBox.height() : -$textPanel.height()).show()
							  .animate({top:textAlign == BOTTOM ? $innerBox.height() - $textPanel.height() : 0}, "normal");
				}				
			}
			
			//resize image
			var resizeImg = function(img) {
				var ratio;
				var maxWidth  = $(window).width() - padding - LIGHTBOX_MARGIN;
				var maxHeight = $(window).height() - padding/2 - $cPanel.height() - LIGHTBOX_MARGIN;
				
				if (img.width > maxWidth) {
					ratio = img.height/img.width;
					img.width = maxWidth;
					img.height = ratio * maxWidth;
				}
				if (img.height > maxHeight) {
					ratio = img.width/img.height;
					img.width = ratio * maxHeight;
					img.height = maxHeight;
				}
			}
			
			//enable control panel
			var enableCtrl = function() {
				$(document).unbind("keyup", keyCtrl).bind("keyup", keyCtrl);
				$cPanel.show();
				
				var backWidth = Math.round($innerBox.width()/2);				
				$backPane.css({width:backWidth, height:"100%"}).unbind().hover(showPrevButton, hidePrevButton);				
				if	(!contNav && currIndex == 0) {
					$backPane.css("cursor","default");
				}
				else { 				
					$backPane.bind("click", prevImg).css("cursor","pointer");					
					$backBtn.show();
				}
			
				var fwdWidth = $innerBox.width() - $backPane.width();
				$fwdPane.css({width:fwdWidth, height:"100%"}).unbind().hover(showNextButton, hideNextButton);
				if (!contNav && currIndex == numItems - 1) {
					$fwdPane.css("cursor","default");
				}
				else {
					$fwdPane.bind("click", nextImg).css("cursor","pointer");					
					$fwdBtn.show();					
				}
			}

			//disable control panel
			var disableCtrl = function() {
				$(document).unbind("keyup", keyCtrl);
				$cPanel.hide();
				$backBtn.hide();
				$fwdBtn.hide();
			}
			
			//control panel click
			var cpanelClick = function(e) {
				switch($(e.target).attr("id")) {
					case "play-btn":
						togglePlay();
						break;
					case "close-btn":
						closeLightbox();
						break;
				}
			}
			
			//play/pause
			var togglePlay = function() {
				rotate = !rotate;
				$playBtn.toggleClass("pause", rotate);
				rotate ? $lightbox.trigger(START_TIMER) : pauseTimer();
			}
		
			//previous
			var prevImg = function() {
				resetTimer();
				if (currIndex > 0) {
					currIndex--;
				}
				else if (contNav) {
					currIndex = numItems - 1;
				}
				else {
					return;
				}				
				loadContent(currIndex);
			}
			
			//next
			var nextImg = function() {
				resetTimer();
				if (currIndex < numItems - 1) {
					currIndex++;
				}
				else if (contNav) {
					currIndex = 0;
				}
				else {
					return;
				}				
				loadContent(currIndex);
			}

			//rotate next
			var rotateNext = function() {
				resetTimer();
				currIndex = (currIndex < numItems - 1) ? currIndex + 1 : 0;
				loadContent(currIndex);
			}
			
			//show previous button
			var showPrevButton = function() {
				$backBtn.stop().animate({"margin-left":0}, ANIMATE_SPEED);
			}
			
			//hide previous button
			var hidePrevButton = function() {
				$backBtn.stop().animate({"margin-left":-$backBtn.width()}, ANIMATE_SPEED);
			}
			
			//show next button
			var showNextButton = function() {			
				$fwdBtn.stop().animate({"margin-left":-$fwdBtn.width()}, ANIMATE_SPEED);
			}
			
			//hide next button
			var hideNextButton = function() {
				$fwdBtn.stop().animate({"margin-left":0}, ANIMATE_SPEED);
			}
			
			//key press
			var keyCtrl = function(e) {
				switch(e.keyCode) {
					case 37:
					case 80:
						prevImg();
						break;
					case 39:
					case 78:
						nextImg();
						break;
					case 32:
						togglePlay();
						break;
				}
			}
			
			//key press close
			var keyClose = function(e) {
				switch(e.keyCode) {
					case 27: case 67: case 88:
						closeLightbox();
				}
			}
			
			//get duration
			var getDuration = function(width, height) {
				var wDiff = Math.abs($lightbox.width() - width);
				var hDiff = Math.abs($lightbox.height() - height);
				return Math.max(duration, wDiff, hDiff);
			}
			
			//check for msie 6
			var msie6Check = function() {
				if (jQuery.browser.msie) {
					if (parseInt(jQuery.browser.version) <= 6) {
						var winWidth, winHeight;
						$overlay.css({position:"absolute", width:$(document).width(), height:$(document).height()});
						$lightbox.css("position", "absolute");	
						$(window).bind("resize", 
									   function() {  
											if(winHeight != document.documentElement.clientHeight || winWidth != document.documentElement.clientWidth) {
												$overlay.css({width:$(document).width(), height:$(document).height()});
											}
											winWidth =  document.documentElement.clientWidth;
											winHeight = document.documentElement.clientHeight; 
										});	  
						$lightbox.bind(IE6_INIT, function() { $("body").find("select").addClass("hide-selects"); })
								 .bind(IE6_CLEANUP, function() { $("body").find("select").removeClass("hide-selects"); });									 
					}
  				}
			}
			
			//start timer
			var startTimer = function() {
				if (rotate && timerId == null) {
					var newDelay = Math.round($timer.data("pct") * delay);
					$timer.animate({width:$innerBox.width()+1}, newDelay);
					timerId = setTimeout(rotateNext, newDelay);
				}
			}
			
			//reset timer
			var resetTimer = function() {
				clearTimeout(timerId);
				timerId = null;
				$timer.stop(true).width(0).data("pct", 1);
			}
			
			//pause timer
			var pauseTimer = function() {
				clearTimeout(timerId);
				timerId = null;
				var pct = 1 - ($timer.width()/($innerBox.width()+1));
				$timer.stop(true).data("pct", pct);
			}
			
			init();
		}
		
		//Class Scroller
		function Scroller($obj, opts) {
			var numDisplay = 		getPosNumber(opts.num_display,3);
			var slideWidth = 		getPosNumber(opts.slide_width,300);
			var slideHeight = 		getPosNumber(opts.slide_height,200);
			var slideMargin = 		getNonNegNumber(opts.slide_margin,1);
			var margin = 			getNonNegNumber(opts.margin,10);
			var buttonWidth =		getPosNumber(opts.button_width,30);
			var ctrlHeight =		getPosNumber(opts.ctrl_height,20);
			var autoScroll = 		opts.auto_scroll;
			var delay = 			getPosNumber(opts.delay,DEFAULT_DELAY);
			var duration = 			getPosNumber(opts.scroll_speed,SCROLL_SPEED);
			var easing = 			opts.easing;
			var lightboxOn = 		opts.lightbox_on;
			var displayButtons = 	opts.display_buttons;
			var displayScrollbar = 	opts.display_scrollbar;
			var displayIndexes =	opts.display_indexes;
			var displayCaption = 	opts.display_caption;
			var mouseoverCaption = 	opts.mouseover_caption;
			var captionPos = 		opts.caption_position.toLowerCase();
			var captionAlign = 		opts.caption_align.toLowerCase();
			var moveBy1 = 			opts.move_one;
			var contNav = 			opts.cont_nav;
			var shuffle =			opts.shuffle;
			
			var $scroller =   $(".wt-scroller", $obj);
			var $slidePanel = $scroller.find(".slides");
			var $slideList =  $slidePanel.find(">ul:first");
			var $slides =	  $slideList.find(">li");
			var $prevBtn =    $scroller.find(".prev-btn");			
			var $nextBtn =    $scroller.find(".next-btn");
			var $cbar = 	  $scroller.find(".lower-panel");
			var $indexes;			
			var $scrollbar;
			var $thumb;
			var $items;
			
			var lightbox;
			var numItems;
			var unitSize;
			var prevSlots;
			var nextSlots;
			var maxSlots;	
			var range;
			var pos;	
			var timerId = null;
			var extOffset = 0;
			var extHeight = 0;
			
			this.init = function() {
				numItems = $slides.size();
				if (numItems <= numDisplay) {
					displayButtons = displayScrollbar = displayIndexes = false;
					numDisplay = numItems;
				}
				maxSlots = numItems - numDisplay;
				prevSlots = 0;
				nextSlots = maxSlots;
				pos = 0;
				
				//init components
				initItems();
				initSlidePanel();
				initCtrls();
				
				$scroller.css({width:$slidePanel.width() + $prevBtn.width() + $nextBtn.width(), 
							   height:$slidePanel.height() + $cbar.outerHeight(), "padding-top": margin});
				
				if (autoScroll) {
					$scroller.bind(START_TIMER, startTimer).hover(scrollerOver, scrollerOut);
				}
				
				if (lightboxOn) {
					lightbox = new Lightbox(this, opts.lightbox);
				}
				if (shuffle) {
					shuffleItems();
				}
				updateCPanel();
				this.onFocus();
			}
			
			//get item at
			this.getItem = function(i) {
				return $($slides.get(i));
			}
			
			//get number of items
			this.getNumItems = function() {
				return numItems;
			}
			
			//init slides
			var initItems = function() {
				initCaptions();
				$items = new Array(numItems);
				$slides.each(
					function(n) {
						var $img = $(this).find("img:first");
						var $link = $(this).find(">a:first");
						if (!$link.attr("href")) {
							$(this).click(preventDefault);
							$link.css("cursor", "default");
						}
						else if (lightboxOn) {
							$(this).bind("click", {index:n}, openLightbox);
						}
						$img[0].complete && $img[0].width > 0 ? processImg($img) : $img.load(processLoadedImg);									
						if (displayCaption && captionPos == INSIDE) {
							initCaption($(this));						
						}
						$items[n] = $(this);						
					});		
			}
			
			//init captions
			var initCaptions = function() {
				var $captions = $slides.find(">p:first");
				if (displayCaption) {
					var padding = $captions.outerWidth() - $captions.width();
					$captions.css({width:slideWidth - padding, visibility:"visible"}).click(stopPropagation);
					if (captionPos == OUTSIDE) {
						var heights = $captions.map(function() { return $(this).height(); }).get();
						var maxHeight = Math.max.apply(Math, heights);	
                        $captions.css({top:captionAlign == TOP ? 0 : slideHeight, height:maxHeight});
						
						extHeight = $captions.outerHeight();					
						if (captionAlign == TOP) {
							extOffset = extHeight;
						}
						$captions.addClass("outside");
					}
					else {
						$captions.addClass("inside");
					}					
				}
				else {
					$captions.hide();
				}
			}
			
			//init caption
			var initCaption = function($item) {
				var $caption = $item.find(">p:first");				
				if ($caption.length < 1 || $caption.html() == "") {
					$caption.remove();
					return;
				}
				if (mouseoverCaption) {
					$caption.css("top", captionAlign == TOP ? -$caption.outerHeight() : slideHeight);
					$item.hover(showCaption, hideCaption);
				}
				else {
					
                    //alert("slideHeight: "+slideHeight);
                    //alert("$caption.outerHeight(): "+$caption.outerHeight());
                    //$caption.css("top", captionAlign == TOP ? 0 : slideHeight - $caption.outerHeight());
                    $caption.css("top", captionAlign == TOP ? 0 : (slideHeight - $caption.outerHeight())/2);
				}
			}
			
			//init slide panel
			var initSlidePanel = function() {
				$slides.css({width:slideWidth, height:slideHeight + extHeight, "margin-right":slideMargin}).hover(itemMouseover, itemMouseout);
				$slidePanel.css({width:(numDisplay * $slides.width()) + ((numDisplay - 1) * slideMargin), height:$slides.height()});
				unitSize = $slides.outerWidth(true);
				$slideList.width(numItems * unitSize);				
				range = (($slideList.width() - slideMargin) - $slidePanel.width());
			}
			
			var initCtrls = function() {
				initButtons();
				$cbar.css({width:$slidePanel.width(), "padding-left":$prevBtn.width(), "padding-right":$nextBtn.width()});
				if (displayScrollbar) {
					initScrollbar();
					var ctrlMargin = Math.max((ctrlHeight - $scrollbar.height())/2, 0);
					$cbar.css({"padding-top":ctrlMargin, "padding-bottom":ctrlMargin});
				}
				else if (displayIndexes) {
					initIndexes();
					var ctrlMargin = Math.max((ctrlHeight - $indexes.height())/2, 0);
					$cbar.css({"padding-top":ctrlMargin, "padding-bottom":ctrlMargin});
				}
				else {
					$cbar.remove();
					$scroller.css("padding-bottom", margin);
				}				
			}
			
			//init buttons
			var initButtons = function() {							
				if (displayButtons) {		
					$prevBtn.css({width:buttonWidth, height:$slides.height()}).mousedown(preventDefault).click(moveBack);
					$nextBtn.css({width:buttonWidth, height:$slides.height()}).mousedown(preventDefault).click(moveFwd);
					if (!contNav) {
						$scroller.bind(UPDATE_BUTTONS, updateButtons);
					}
				}
				else {
					$prevBtn.remove();
					$nextBtn.remove();
					$scroller.css({"padding-left":margin, "padding-right":margin});
				}
			}
			
			//update buttons
			var updateButtons = function() {
				var begIndex = Math.abs(pos/unitSize);
				var endIndex = begIndex + numDisplay;
				$prevBtn.css(begIndex > 0 ? {opacity:1, cursor:"pointer"} : {opacity:BUTTON_OPACITY, cursor:"default"});
				$nextBtn.css(endIndex < numItems ? {opacity:1, cursor:"pointer"} : {opacity:BUTTON_OPACITY, cursor:"default"});
			}
			
			//init scrollbar
			var initScrollbar = function() {				
				$cbar.append("<div class='scroll-bar'><div class='thumb'></div></div>");
				$scrollbar = $cbar.find("div.scroll-bar");
				$thumb = 	 $cbar.find("div.thumb");				
				$scrollbar.width($cbar.width()).click(trackClick).mousedown(preventDefault);				
				$thumb.width(Math.floor((numDisplay/numItems) * $scrollbar.width())).click(stopPropagation);

				var scrollRange = $scrollbar.width() - $thumb.width();
				var moveRatio = range/scrollRange;
				try {
					$thumb.draggable({containment: "parent"})
						  .bind("drag", function() { $slideList.css({left: Math.round(-$thumb.position().left * moveRatio)}); })
						  .bind("dragstop", function() { autoStop($thumb.position().left/scrollRange); });
				}
				catch (ex) { //not draggable. 
				}
				
				$scroller.bind(UPDATE_THUMB, 
							   	function() {
									var move = Math.round(Math.abs(pos) * (1/moveRatio));				
									$thumb.stop(true, true).animate({left:move}, duration, easing);
								});
			}
			
			//init indexes
			var initIndexes = function() {
				var n = Math.ceil(numItems/numDisplay);
				var str = "";
				for (var i = 0; i < n; i++) {
					str += "<span class='index'></span>";
				}
				$cbar.prepend(str);
				$indexes = $cbar.find(".index").mousedown(preventDefault).bind("click", goToIndex);
				$scroller.bind(UPDATE_INDEX, updateIndexes);				
			}			
				
			//update indexes
			var updateIndexes = function() {
				if (prevSlots%numDisplay == 0 || prevSlots == maxSlots) {
					var i = Math.ceil(prevSlots/numDisplay);
					$indexes.filter(".index-hl").removeClass("index-hl");
					$($indexes.get(i)).addClass("index-hl");				
				}
			}
			
			//update slide list
			var updateSlideList = function() {
			    
                pos = -prevSlots * (unitSize-15);   // neo
                //alert("prevSlots: "+ prevSlots +", unitSize: "+ unitSize +", pos: "+ pos);
                
                // add event by neo
				if(opts.beforeScroll){
                
                    opts.beforeScroll(prevSlots, numItems, numDisplay);
                }
                
                $slideList.stop(true, true).animate(
                    {left:pos}, 
                    duration, 
                    easing, 
					function() { 
                        $scroller.trigger(START_TIMER); 
                        
                        // add event by neo
                        if(opts.afterScroll){
                
                            opts.afterScroll(prevSlots, numItems, numDisplay);
                        }
					});
                
				updateCPanel();
			}
			
			//update controls
			var updateCPanel = function() {
				$scroller.trigger(UPDATE_BUTTONS).trigger(UPDATE_THUMB).trigger(UPDATE_INDEX);					
			}

			//open lightbox
			var openLightbox = function(e) {
				onBlur();			
				lightbox.display(e.data.index);								
				return false;
			}
			
			//track click
			var trackClick = function(e) {
				autoStop((e.pageX - $scrollbar.offset().left)/$scrollbar.width());
			}
			
			//auto stop
			var autoStop = function(pct) {
				var move;
				var newPos = pct * range;
				if (newPos > Math.abs(pos)) {
					move = Math.ceil(newPos/unitSize);			
				}
				else if (newPos < Math.abs(pos)) {
					move = Math.floor(newPos/unitSize);
				}
				else {
					return;
				}
				var slots = move + (pos/unitSize);			
				prevSlots += slots;
				nextSlots -= slots;
				updateSlideList();
			}
			
			//go to index			
			var goToIndex = function() {
				stopTimer();
				var slots = $(this).index() * numDisplay;
				if (slots > maxSlots) {
					slots = maxSlots;
				}		
				prevSlots= slots;
				nextSlots = maxSlots - slots;
				updateSlideList();
				return false;
			}
			
			//move slides back
			var moveBack = function() {
				stopTimer();
				if (nextSlots < maxSlots) {
					var slots = moveBy1 ? 1 : Math.min(maxSlots - nextSlots, numDisplay);
					nextSlots += slots;
					prevSlots -= slots;
				}
				else if (contNav) {
					nextSlots = 0;
					prevSlots = maxSlots;
				}
				else {
					return;
				}
				updateSlideList();
			}
			
			//move slides forward
			var moveFwd = function() {
				stopTimer();
				if (prevSlots < maxSlots) {
					var slots = moveBy1 ? 1 : Math.min(maxSlots - prevSlots, numDisplay);
					prevSlots += slots;
					nextSlots -= slots;
				}
				else if (contNav) {
					prevSlots = 0;
					nextSlots = maxSlots;		
				}
				else {
					return;
				}
				updateSlideList();
			}
			
			//rotate slides forward
			var rotateFwd = function() {
				stopTimer();
				if (prevSlots < maxSlots) {
					var slots = moveBy1 ? 1 : Math.min(maxSlots - prevSlots, numDisplay);
					prevSlots += slots;
					nextSlots -= slots;					
				}
				else {
					prevSlots = 0;
					nextSlots = maxSlots;		
				}
				updateSlideList();
			}
			
			//process loaded image size & position
			var processLoadedImg = function() {
				processImg($(this));
			}
			
			//process image size & position
			var processImg = function($img) {
				var ratio;
				if ($img.outerWidth() > slideWidth) {							
					ratio = $img.outerHeight()/$img.outerWidth();
					$img.css({width:slideWidth, height:ratio * slideWidth});
				}
				if ($img.outerHeight() > slideHeight) {
					ratio = $img.outerWidth()/$img.outerHeight();
					$img.css({width:ratio * slideHeight, height: slideHeight});
				}						
				$img.css({left:(slideWidth - $img.outerWidth())/2, top:extOffset + (slideHeight - $img.outerHeight())/2});				
			}
			
			//scroller blur
			var onBlur = function() {
				$scroller.unbind(START_TIMER).unbind("mouseenter").unbind("mouseleave");
				stopTimer();
				$(document).unbind();
			}
			
			//scroller focus
			this.onFocus = function() {
				if (autoScroll) {
					$scroller.bind(START_TIMER, startTimer).hover(scrollerOver, scrollerOut).trigger(START_TIMER);
				}
				$(document).unbind().bind("keyup", onKeyPress);
			}
			
			//item mouseover
			var itemMouseover = function() {
				$(this).find("img:first").stop().animate({opacity:SLIDE_OPACITY}, ANIMATE_SPEED);				
			}
			
			//item mouseout
			var itemMouseout = function() {
				$(this).find("img:first").stop().animate({opacity:1}, ANIMATE_SPEED, 
					function() {
						if (jQuery.browser.msie) {
							this.style.removeAttribute('filter');						
						}
					});
			}
			
			//show caption
			var showCaption = function() {
				var $caption = $(this).find(">p:first");
				$caption.stop().animate({top:captionAlign == BOTTOM ? slideHeight - $caption.outerHeight() : 0}, ANIMATE_SPEED);
			}
			
			//hide caption
			var hideCaption = function() {
				var $caption = $(this).find(">p:first"); 
				$caption.stop().animate({top:captionAlign == BOTTOM ? slideHeight : -$caption.outerHeight()}, ANIMATE_SPEED);
			}
			
			//scroller over
			var scrollerOver = function() {
				$scroller.unbind(START_TIMER);
				stopTimer();
			}
			
			//scroller out
			var scrollerOut = function() {
				$scroller.bind(START_TIMER, startTimer).trigger(START_TIMER);
			}
			
			//key press
			var onKeyPress = function(e) {
				switch(e.keyCode) {
					case 37:
						moveBack();
						break;
					case 39:
						moveFwd();
						break;
				}
			}
			
			//prevent default behavior
			var preventDefault = function() {
				return false;
			}
			
			//stop propagation
			var stopPropagation = function(e) {
				e.stopPropagation();
			}
			
			//shuffle items
			var shuffleItems = function() {		
				for (var i = 0; i < $items.length; i++) {
					var ri = Math.floor(Math.random() * $items.length);
					var temp = $items[i];	
					$items[i] = $items[ri];
					$items[ri] = temp;				
				}
				
				for (var i = 0; i < $items.length; i++) {
					$items[i] = $items[i].clone(true);
				}
				
				for (var i = 0; i < $items.length; i++) {
					$($slides.get(i)).replaceWith($items[i]);
				}
			}
			
			//start timer
			var startTimer = function() {
				if (timerId == null) {
					timerId = setTimeout(rotateFwd, delay);
				}
			}
			
			//stop timer
			var stopTimer = function() {
				clearTimeout(timerId);
				timerId = null;
			}
		}
		
		//get positive number
		var getPosNumber = function(val, defaultVal) {
			if (!isNaN(val) && val > 0) {
				return val;
			}
			return defaultVal;
		}
		
		//get nonnegative number
		var getNonNegNumber = function(val, defaultVal) {
			if (!isNaN(val) && val >= 0) {
				return val;
			}
			return defaultVal;
		}
		
		var defaults = { 
			num_display:3,
			slide_width:300,
			slide_height:200,
			slide_margin:1,
			button_width:30,
			ctrl_height:20,
			margin:8,	
			auto_scroll:true,
			delay:DEFAULT_DELAY,
			scroll_speed:SCROLL_SPEED,
			easing:"",
			move_one:false,
			display_buttons:true,			
			display_scrollbar:true,
			display_indexes:false,
			display_caption:true,
			mouseover_caption:false,
			caption_align:BOTTOM,
			caption_position:INSIDE,
			lightbox_on:true,
			cont_nav:true,
			shuffle:false,
			lightbox: {
				rotate:true,
				delay:DEFAULT_DELAY,
				easing:"",
				transition_speed:DEFAULT_DURATION,
				display_number:true,
				display_timer:true,
				display_caption:true,
				caption_align:BOTTOM,
				cont_nav:true,
				auto_fit:true
			}
		};
		
		var opts = $.extend(true, {}, defaults, params);		
		return this.each(
			function() {
				var scroller = new Scroller($(this), opts);
				scroller.init();
			}
		);
	}
})(jQuery);