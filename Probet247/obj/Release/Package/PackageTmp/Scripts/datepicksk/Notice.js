if (typeof NoticeHandler == "undefined") {
	NoticeHandler = {}
} (function () {
	NoticeHandler.success = function (a) {
		$j("#message").removeClass().addClass("message-wrap success");
		NoticeHandler.show(a)
	};
	NoticeHandler.error = function (a) {
		$j("#message").removeClass().addClass("message-wrap error");
		NoticeHandler.show(a);
		Trace.log(a)
	};
	NoticeHandler.info = function (a) {
		$j("#message").removeClass().addClass("message-wrap info");
		NoticeHandler.show(a)
	};
	NoticeHandler.show = function (a) {
		$j("#message").hide();
		$j("#message").show().find("p").text(a);
		setTimeout(function () {
			$j("#message").find("p").text("");
			$j("#message").hide()
		}, 3000)
	};
	NoticeHandler.hide = function (a) {
		$j("#message").hide().find("p").text("")
	}
})();