if (typeof (AjaxUtil) == "undefined") {
	AjaxUtil = {}
} (function () {
	var a = 0;
	AjaxUtil.ajaxSetup = function () {
		if (typeof (jQuery) == "undefined") {
			a++;
			if (a >= 60) {
				alert("not import jquery lib");
				return
			}
			setTimeout(AjaxUtil.ajaxSetup, 1000)
		}
		jQuery.ajaxSetup({
			statusCode: {
				410: function (d) {
					var c = d.getResponseHeader("refreshPage");
					if (c && c !== "") {
						if (top.onbeforeunload) {
							top.onbeforeunload = null
						}
						top.location.reload();
						return
					}
					var b = d.getResponseHeader("redirectPath");
					if (b && b !== "") {
						top.location.href = b;
						return
					}
				}
			},
			beforeSend: function (c, b) {
				if (typeof (b.dataType) == "undefined" || !b.dataType) {
					b.dataType = "JSON"
				}
			}
		})
	}
})();
AjaxUtil.ajaxSetup();