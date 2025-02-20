if (typeof (Browser) == "undefined") {
	Browser = {}
} (function () {
	var b = function (h) {
		var g = 0;
		var i = 0;
		if (!h) {
			var h = window.event
		}
		if (h.pageX || h.pageY) {
			g = h.pageX;
			i = h.pageY
		} else {
			if (h.clientX || h.clientY) {
				g = h.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
				i = h.clientY + document.body.scrollTop + document.documentElement.scrollTop
			}
		}
		return [g, i]
	};
	var a = function () {
		this.name = "IE";
		this.getCursorPosition = function (g) {
			return b(g)
		};
		this.attachEventListener = function (j, h, i, g) {
			j.attachEvent("on" + h, i);
			return true
		};
		this.addLoadListener = function (g) {
			window.attachEvent("onload", g)
		};
		this.getPageDimensions = function () {
			var g = document.getElementsByTagName("body")[0];
			var j = 0;
			var h = 0;
			var i = 0;
			var l = 0;
			var k = [0, 0];
			if (typeof document.documentElement != "undefined" && typeof document.documentElement.scrollWidth != "undefined") {
				k[0] = document.documentElement.scrollWidth;
				k[1] = document.documentElement.scrollHeight
			}
			j = g.offsetWidth;
			h = g.offsetHeight;
			i = g.scrollWidth;
			l = g.scrollHeight;
			if (j > k[0]) {
				k[0] = j
			}
			if (h > k[1]) {
				k[1] = h
			}
			if (i > k[0]) {
				k[0] = i
			}
			if (l > k[1]) {
				k[1] = l
			}
			return k
		};
		this.getScrollingPosition = function () {
			var g = 0;
			var h = 0;
			if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
				g = document.body.scrollTop;
				h = document.body.scrollLeft
			} else {
				if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
					g = document.documentElement.scrollTop;
					h = document.documentElement.scrollLeft
				}
			}
			return [h, g]
		};
		this.getViewportSize = function () {
			if (document.documentElement.clientWidth == 0 && document.documentElement.clientHeight == 0) {
				return [document.body.clientWidth, document.body.clientHeight]
			}
			return [document.documentElement.clientWidth, document.documentElement.clientHeight]
		};
		this.setIFrameHeight = function (g, h) {
			frame = document.getElementById(g);
			innerDoc = (frame.contentDocument) ? frame.contentDocument : frame.contentWindow.document;
			objToResize = (frame.style) ? frame.style : frame;
			objToResize.height = innerDoc.body.scrollHeight + 18 + (h ? h : 0)
		};
		this.setTextContent = function (h, g) {
			h.innerText = g
		};
		this.getTextContent = function (g) {
			return g.innerText
		}
	};
	var f = function () {
		this.name = "Mozilla";
		this.getCursorPosition = function (g) {
			return b(g)
		};
		this.attachEventListener = function (j, h, i, g) {
			j.addEventListener(h, i, g);
			return true
		};
		this.addLoadListener = function (g) {
			window.addEventListener("load", g, false)
		};
		this.getPageDimensions = function () {
			var g = document.getElementsByTagName("body")[0];
			var j = 0;
			var h = 0;
			var i = 0;
			var l = 0;
			var k = [0, 0];
			if (typeof document.documentElement != "undefined" && typeof document.documentElement.scrollWidth != "undefined") {
				k[0] = document.documentElement.scrollWidth;
				k[1] = document.documentElement.scrollHeight
			}
			j = g.offsetWidth;
			h = g.offsetHeight;
			i = g.scrollWidth;
			l = g.scrollHeight;
			if (j > k[0]) {
				k[0] = j
			}
			if (h > k[1]) {
				k[1] = h
			}
			if (i > k[0]) {
				k[0] = i
			}
			if (l > k[1]) {
				k[1] = l
			}
			return k
		};
		this.getScrollingPosition = function () {
			var g = [0, 0];
			if (typeof window.pageYOffset != "undefined") {
				g = [window.pageXOffset, window.pageYOffset]
			} else {
				if (typeof document.body.scrollTop != "undefined") {
					g = [document.body.scrollLeft, document.body.scrollTop]
				}
			}
			return g
		};
		this.getViewportSize = function () {
			return [window.innerWidth, window.innerHeight]
		};
		this.setIFrameHeight = function (h, i) {
			var j = document.getElementById(h);
			var g = j.contentDocument.body.offsetHeight;
			j.height = g + (i ? i : 0)
		};
		this.setTextContent = function (h, g) {
			h.textContent = g
		};
		this.getTextContent = function (g) {
			return g.textContent
		}
	};
	var c = function () {
		this.name = "Undefined";
		this.getCursorPosition = function (g) {
			return b(g)
		};
		this.attachEventListener = function (k, i, j, h) {
			if (typeof k.addEventListener != "undefined") {
				k.addEventListener(i, j, h);
				return
			}
			if (typeof k.attachEvent != "undefined") {
				k.attachEvent("on" + i, j);
				return
			}
			i = "on" + i;
			if (typeof k[i] != "function") {
				k[i] = j;
				return
			}
			var g = k[i];
			k[i] = function () {
				g();
				return j()
			};
			return true
		};
		this.addLoadListener = function (h) {
			if (typeof window.addEventListener != "undefined") {
				window.addEventListener("load", h, false);
				return
			}
			if (typeof document.addEventListener != "undefined") {
				document.addEventListener("load", h, false);
				return
			}
			if (typeof window.attachEvent != "undefined") {
				window.attachEvent("onload", h);
				return
			}
			if (typeof window.onload != "function") {
				window.onload = h;
				return
			}
			var g = window.onload;
			window.onload = function () {
				g();
				h()
			}
		};
		this.getPageDimensions = function () {
			var g = document.getElementsByTagName("body")[0];
			var j = 0;
			var h = 0;
			var i = 0;
			var l = 0;
			var k = [0, 0];
			if (typeof document.documentElement != "undefined" && typeof document.documentElement.scrollWidth != "undefined") {
				k[0] = document.documentElement.scrollWidth;
				k[1] = document.documentElement.scrollHeight
			}
			j = g.offsetWidth;
			h = g.offsetHeight;
			i = g.scrollWidth;
			l = g.scrollHeight;
			if (j > k[0]) {
				k[0] = j
			}
			if (h > k[1]) {
				k[1] = h
			}
			if (i > k[0]) {
				k[0] = i
			}
			if (l > k[1]) {
				k[1] = l
			}
			return k
		};
		this.getScrollingPosition = function () {
			var g = [0, 0];
			if (typeof window.pageYOffset != "undefined") {
				g = [window.pageXOffset, window.pageYOffset]
			}
			if (typeof document.documentElement.scrollTop != "undefined" && document.documentElement.scrollTop > 0) {
				g = [document.documentElement.scrollLeft, document.documentElement.scrollTop]
			} else {
				if (typeof document.body.scrollTop != "undefined") {
					g = [document.body.scrollLeft, document.body.scrollTop]
				}
			}
			return g
		};
		this.getViewportSize = function () {
			var g = [0, 0];
			if (typeof window.innerWidth != "undefined") {
				g = [window.innerWidth, window.innerHeight]
			} else {
				if (typeof document.documentElement != "undefined" && typeof document.documentElement.clientWidth != "undefined" && document.documentElement.clientWidth != 0) {
					g = [document.documentElement.clientWidth, document.documentElement.clientHeight]
				} else {
					g = [document.getElementsByTagName("body")[0].clientWidth, document.getElementsByTagName("body")[0].clientHeight]
				}
			}
			return g
		};
		this.setIFrameHeight = function (g, h) {
			frame = document.getElementById(g);
			innerDoc = (frame.contentDocument) ? frame.contentDocument : frame.contentWindow.document;
			objToResize = (frame.style) ? frame.style : frame;
			objToResize.height = innerDoc.body.scrollHeight + 18 + (h ? h : 0)
		};
		this.setTextContent = function (h, g) {
			try {
				h.textContent = g
			} catch (i) {
				h.innerText = g
			}
		};
		this.getTextContent = function (g) {
			try {
				return g.textContent
			} catch (h) {
				return g.innerText
			}
		}
	};
	var e = function () {
		var i = navigator.userAgent.toLowerCase();
		if (typeof navigator.vendor != "undefined" && navigator.vendor == "KDE" && typeof window.sidebar != "undefined") {
			return new c()
		}
		if (typeof window.opera != "undefined") {
			var g = parseFloat(i.replace(/.*opera[\/ ]([^ $]+).*/, "$1"));
			if (g >= 7) {
				return new c()
			} else {
				if (g >= 5) {
					return new c()
				}
			}
			return new c()
		}
		if (typeof document.all != "undefined") {
			if (typeof document.getElementById != "undefined") {
				var h = i.replace(/.*ms(ie[\/ ][^ $]+).*/, "$1").replace(/ /, "");
				if (typeof document.uniqueID != "undefined") {
					if (h.indexOf("5.5") != -1) {
						return new a()
					} else {
						return new a()
					}
				} else {
					return new a()
				}
			}
			return new a()
		}
		if (typeof document.getElementById != "undefined") {
			if (navigator.vendor.indexOf("Apple Computer, Inc.") != -1) {
				if (typeof window.XMLHttpRequest != "undefined") {
					return new c()
				}
				return new c()
			} else {
				if (i.indexOf("gecko") != -1) {
					return new f()
				}
			}
		}
		return new a()
	};
	var d = {
		init: function () {
			this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
			this.version = this.searchVersion(navigator.userAgent) || this.searchVersion(navigator.appVersion) || "an unknown version";
			this.OS = this.searchString(this.dataOS) || "an unknown OS"
		},
		searchString: function (k) {
			for (var g = 0; g < k.length; g++) {
				var h = k[g].string;
				var j = k[g].prop;
				this.versionSearchString = k[g].versionSearch || k[g].identity;
				if (h) {
					if (h.indexOf(k[g].subString) != -1) {
						return k[g].identity
					}
				} else {
					if (j) {
						return k[g].identity
					}
				}
			}
		},
		searchVersion: function (h) {
			var g = h.indexOf(this.versionSearchString);
			if (g == -1) {
				return
			}
			return parseFloat(h.substring(g + this.versionSearchString.length + 1))
		},
		dataBrowser: [{
			string: navigator.userAgent,
			subString: "Chrome",
			identity: "Chrome"
		}, {
			string: navigator.userAgent,
			subString: "OmniWeb",
			versionSearch: "OmniWeb/",
			identity: "OmniWeb"
		}, {
			string: navigator.vendor,
			subString: "Apple",
			identity: "Safari",
			versionSearch: "Version"
		}, {
			prop: window.opera,
			identity: "Opera",
			versionSearch: "Version"
		}, {
			string: navigator.vendor,
			subString: "iCab",
			identity: "iCab"
		}, {
			string: navigator.vendor,
			subString: "KDE",
			identity: "Konqueror"
		}, {
			string: navigator.userAgent,
			subString: "Firefox",
			identity: "Firefox"
		}, {
			string: navigator.vendor,
			subString: "Camino",
			identity: "Camino"
		}, {
			string: navigator.userAgent,
			subString: "Netscape",
			identity: "Netscape"
		}, {
			string: navigator.userAgent,
			subString: "MSIE",
			identity: "Explorer",
			versionSearch: "MSIE"
		}, {
			string: navigator.userAgent,
			subString: "Gecko",
			identity: "Mozilla",
			versionSearch: "rv"
		}, {
			string: navigator.userAgent,
			subString: "Mozilla",
			identity: "Netscape",
			versionSearch: "Mozilla"
		}],
		dataOS: [{
			string: navigator.platform,
			subString: "Win",
			identity: "Windows"
		}, {
			string: navigator.platform,
			subString: "Mac",
			identity: "Mac"
		}, {
			string: navigator.userAgent,
			subString: "iPhone",
			identity: "iPhone/iPod"
		}, {
			string: navigator.platform,
			subString: "Linux",
			identity: "Linux"
		}]
	};
	Browser = e();
	Browser.browserDetect = d
})();