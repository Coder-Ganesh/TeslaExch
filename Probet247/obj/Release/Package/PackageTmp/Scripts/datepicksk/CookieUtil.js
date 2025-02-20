if (typeof (CookieUtil) == "undefined") {
	CookieUtil = {}
} (function () {
	CookieUtil.setCookie = function (c, e, g, f, d) {
		CookieUtil.deleteCookie(c);
		g = g ? g : 30;
		f = f ? f : "/";
		var h = c + "=" + escape(e);
		if (g != -1) {
			var b = new Date();
			b.setTime(b.getTime() + g * 24 * 3600 * 1000);
			h += ";expires=" + b.toGMTString()
		}
		if (f) {
			h += ";path=" + f
		}
		if (d) {
			h += ";domain=" + d
		}
		document.cookie = h
	};
	CookieUtil.getCookie = function (e, c) {
		if (!c) {
			c = c || document.cookie
		}
		var b = c.split(";");
		for (var f = 0; f < b.length; f++) {
			var d = b[f].split("=");
			if (d[0].replace(/^\s+|\s+$/g, "") == e) {
				return unescape(d[1])
			}
		}
		return null
	};
	CookieUtil.deleteCookie = function (c) {
		var b = new Date();
		b.setTime(b.getTime() - 1000);
		var d = c + "=null;expires=" + b.toGMTString() + ";path=/";
		document.cookie = d
	};
	CookieUtil.getCookieDomainFormat = function (b) {
		if (a(b) || "localhost" == b) {
			return ""
		}
		var c = b.indexOf(".");
		if (c >= 0) {
			return b.substring(c)
		}
		return ""
	};

	function a(b) {
		return (/^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/.test(b))
	}
})();