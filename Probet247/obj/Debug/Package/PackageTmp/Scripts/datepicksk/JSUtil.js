var StringUtil = {
	startsWith: function (b, a) {
		return (b.substr(0, a.length) == a)
	},
	endsWith: function (b, a) {
		return b.substring(b.length - a.length) == a
	},
	concat: function (a, b) {
		return new String(a.toString() + b)
	},
	toCharArray: function (c) {
		var b = new Array(c.length);
		for (var a = 0; a < c.length; a++) {
			b[a] = c.charAt(a)
		}
		return b
	},
	containedChinese: function (a) {
		return (/[^\x00-\xff]/g.test(a))
	},
	getlength: function (b) {
		var a = b.match(/[^\x00-\xff]/ig);
		return b.length + (a == null ? 0 : a.length)
	},
	trim: function (a) {
		return a.replace(/(^\s*)|(\s*$)/g, "")
	},
	getXMLEncode: function (a) {
		a = a.replace("&", "&amp;");
		a = a.replace("<", "&lt;");
		a = a.replace(">", "&gt;");
		a = a.replace("'", "&apos;");
		a = a.replace('"', "&quot;")
	},
	replaceAll: function (f, c, d) {
		var a = f;
		var b = a.indexOf(c);
		while (b != -1) {
			a = a.replace(c, d);
			b = a.indexOf(c)
		}
		return a
	},
	isOnlyNumberAndColon: function (b) {
		var a = /^[\d:]+$/;
		if (a.test(b)) {
			return true
		}
		return false
	},
	ucfirst: function (a) {
		var b = a.toLowerCase();
		b = b.replace(/\b\w+\b/g, function (c) {
			return c.substring(0, 1).toUpperCase() + c.substring(1)
		});
		return b
	},
	commaSeparateNumber: function (a) {
		return a.toString().replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, "")
	},
	isEmpty: function (b) {
		var a = b;
		if (a) {
			a = StringUtil.trim(a)
		}
		return !a
	}
};
var ArrayUtil = {
	max: function (c) {
		var b, a = c[0];
		for (b = 1; b < c.length; b++) {
			if (a < c[b]) {
				a = c[b]
			}
		}
		return a
	},
	contains: function (c, b) {
		for (var a = 0; a < c.length; a++) {
			if (c[a] == b) {
				return true
			}
		}
		return false
	},
	remove: function (c, b) {
		for (var a = 0; a < c.length; a++) {
			if (c[a] == b) {
				c.splice(a, 1);
				return true
			}
		}
		return false
	},
	isArray: function (a) {
		return Object.prototype.toString.call(a) === "[object Array]"
	},
	indexOf: function (a, d, f) {
		if (typeof a.indexOf == "function") {
			return a.indexOf(d, f)
		}
		for (var c = (f || 0), b = a.length; c < b; c++) {
			if (a[c] === d) {
				return c
			}
		}
		return -1
	}
};
var ElementUtil = {
	hiddenElement: function (a) {
		var c = document.getElementsByTagName(a);
		for (var b = 0; b < c.length; b++) {
			c[b].style.visibility = "hidden"
		}
	},
	showElement: function (a) {
		var c = document.getElementsByTagName(a);
		for (var b = 0; b < c.length; b++) {
			c[b].style.visibility = ""
		}
	}
};
var SelectUtil = {
	selectOption: function (a, c, b) {
		if (isNaN(b)) {
			document.getElementsByName(a)[0].value = c
		} else {
			document.getElementsByName(a)[b].value = c
		}
	},
	addSelectOption: function (a, d, c) {
		var b = document.createElement("OPTION");
		b.text = d;
		b.value = c;
		a.options.add(b);
		return b
	},
	removeSelectOption: function (b) {
		var a = document.getElementsByName(b)[0];
		for (var c = a.options.length - 1; c >= 0; c--) {
			if (a.options[c].selected) {
				a.removeChild(a.options[c])
			}
		}
	}
};
var CheckboxUtil = {
	checkedByValues: function (c, a, h) {
		var g = document.getElementsByName(c);
		var f = a;
		if (!ArrayUtil.isArray(f)) {
			f = [a]
		}
		for (var b = 0; b < f.length; b++) {
			for (var d = 0; d < g.length; d++) {
				if (g[d].value == f[b]) {
					g[d].checked = h;
					break
				}
			}
		}
	},
	checkedAll: function (c) {
		var a = document.getElementsByTagName("input");
		for (var b = 0; b < a.length; b++) {
			if (a[b].type == "checkbox") {
				a[b].checked = c
			}
		}
	}
};
var RadioUtil = {
	checkedByValue: function (a, d) {
		var c = document.getElementsByName(a);
		if (d === "") {
			c[0].checked = true;
			return
		}
		for (var b = 0; b < c.length; b++) {
			if (c[b].value == d) {
				c[b].checked = true;
				break
			}
		}
	},
	getIndex: function (a) {
		var c = document.getElementsByName(a);
		for (var b = 0; b < c.length; b++) {
			if (c[b].checked) {
				return b
			}
		}
		return -1
	},
	getChecked: function (a) {
		var c = document.getElementsByName(a);
		for (var b = 0; b < c.length; b++) {
			if (c[b].checked) {
				return c[b].value
			}
		}
		return null
	}
};
var ObjectUtil = {
	clone: function (b) {
		var a = new Object();
		for (elements in b) {
			a[elements] = b[elements]
		}
		return a
	},
	cloneAll: function (d) {
		function a() { }
		a.prototype = d;
		var c = new a();
		for (var b in c) {
			if (typeof (c[b]) == "object") {
				c[b] = c[b].cloneAll()
			}
		}
		return c
	},
	isEmpty: function (b) {
		for (var a in b) {
			if (b.hasOwnProperty(a)) {
				return false
			}
		}
		return true
	},
	toJSON: function (b) {
		var a = "";
		for (e in b) {
			if (typeof (b[e]) === "function") {
				continue
			}
			a = a + '"' + e + '":';
			if (typeof (b[e]) === "string") {
				a = a + '"' + b[e] + '",'
			} else {
				a = a + b[e] + ","
			}
		}
		return "{" + a.substring(0, a.length - 1) + "}"
	}
};
var IFrameUtil = {
	calcHeight: function (a) {
		var d = document.getElementById(a);
		d.contentWindow.scrollTo(0, 0);
		var b = d.contentWindow.document.documentElement.scrollHeight;
		var c = d.contentWindow.document.body.scrollHeight;
		if (c > b) {
			b = c
		}
		document.getElementById(a).height = b
	},
	SetCwinHeight: function (b, c) {
		var d = document.getElementById(b);
		if (document.getElementById) {
			if (d && !window.opera) {
				if (typeof (window.innerWidth) == "number") {
					var a = d.contentDocument.body.offsetHeight;
					d.height = a + (c ? c : 0)
				} else {
					var a = d.contentWindow.document.body.scrollHeight + "px";
					d.style.height = a + (c ? c : 0)
				}
			}
		}
	},
	reinitIframe: function (d) {
		try {
			var b = d.contentWindow.document.body.scrollHeight + 20;
			var f = d.contentWindow.document.documentElement.scrollHeight + 20;
			var a = Math.max(b, f);
			d.height = a
		} catch (c) { }
	}
};

function TreeMap() {
	this.elements = new Array();
	this.size = function () {
		return this.elements.length
	};
	this.put = function (c, a) {
		var b = 0;
		for (; b < this.elements.length; b++) {
			if (this.elements[b].key == c) {
				this.elements[b].value = a;
				return
			}
		}
		if (b == this.elements.length) {
			this.elements.push({
				key: c,
				value: a
			})
		}
	};
	this.putAll = function (b) {
		var a = b.entrySet();
		for (var c = 0; c < a.length; c++) {
			this.put(a[c].key, a[c].value)
		}
	};
	this.remove = function (b) {
		for (var a = 0; a < this.elements.length; a++) {
			if (this.elements[a].key == b) {
				this.elements.splice(a, 1);
				return true
			}
		}
		return false
	};
	this.containsKey = function (b) {
		for (var a = 0; a < this.elements.length; a++) {
			if (this.elements[a].key == b) {
				return true
			}
		}
		return false
	};
	this.get = function (c) {
		for (var b = 0, a = this.elements.length; b < a; b++) {
			if (this.elements[b].key == c) {
				return this.elements[b].value
			}
		}
		return null
	};
	this.values = function () {
		var a = new Array(this.elements.length);
		for (var b = 0; b < this.elements.length; b++) {
			a[b] = this.elements[b].value
		}
		return a
	};
	this.sort = function (a) {
		this.elements = this.elements.sort(a)
	};
	this.entrySet = function () {
		return this.elements
	};
	this.clear = function () {
		this.elements.length = 0
	};
	this.keySet = function () {
		var a = new Array(this.elements.length);
		for (var b = 0; b < this.elements.length; b++) {
			a[b] = this.elements[b].key
		}
		return a
	};
	this.showOrder = function () {
		var a = "";
		for (var b = 0; b < this.elements.length; b++) {
			a += this.elements[b].key + ","
		}
		return a
	};
	this.sortByKey = function () {
		this.elements.sort(function (d, c) {
			return (d.key - c.key)
		})
	}
}

function HashMap() {
	this.length = 0;
	this.elements = {};
	this.size = function () {
		return this.length
	};
	this.put = function (b, a) {
		if (!this.elements.hasOwnProperty(b)) {
			this.length++
		}
		this.elements[b] = a
	};
	this.putAll = function (a) {
		for (var b in a) {
			if (!this.elements.hasOwnProperty(b)) {
				this.length++
			}
			this.elements[b] = a[b]
		}
	};
	this.remove = function (a) {
		if (this.elements.hasOwnProperty(a)) {
			this.length--;
			delete this.elements[a];
			return true
		}
		return false
	};
	this.containsKey = function (a) {
		return this.elements.hasOwnProperty(a)
	};
	this.get = function (a) {
		return this.elements.hasOwnProperty(a) ? this.elements[a] : null
	};
	this.values = function () {
		var a = new Array(this.length);
		var b = 0;
		for (var c in this.elements) {
			a[b++] = this.elements[c]
		}
		return a
	};
	this.entrySet = function () {
		var a = new Array(this.length);
		var b = 0;
		for (var c in this.elements) {
			a[b++] = {
				key: c,
				value: this.elements[c]
			}
		}
		return a
	};
	this.clear = function () {
		this.elements = {};
		this.length = 0
	};
	this.keySet = function () {
		var a = new Array(this.length);
		var b = 0;
		for (var c in this.elements) {
			a[b++] = c
		}
		return a
	}
}
if (typeof (EventUtils) == "undefined") {
	EventUtils = {}
} (function () {
	var a = function (b, c) {
		return (b & c.unique()) > 0
	};
	EventUtils.isReady = function (b) {
		return a(b, EventStatusType.READY)
	};
	EventUtils.isCancel = function (b) {
		return a(b, EventStatusType.CANCEL)
	};
	EventUtils.isOpen = function (b) {
		return a(b, EventStatusType.OPEN)
	};
	EventUtils.isSuspend = function (b) {
		return a(b, EventStatusType.SUSPEND)
	};
	EventUtils.isClose = function (b) {
		return a(b, EventStatusType.CLOSE)
	};
	EventUtils.isEnd = function (b) {
		return a(b, EventStatusType.END)
	};
	EventUtils.isUnsettled = function (b) {
		return a(b, EventStatusType.UNSETTLED)
	};
	EventUtils.isSettled = function (b) {
		return a(b, EventStatusType.SETTLED)
	};
	EventUtils.isVoided = function (b) {
		return a(b, EventStatusType.VOIDED)
	};
	EventUtils.isCloseSite = function (c, b) {
		return WebSiteUtil.isCloseSite(c, b)
	};
	EventUtils.isBookModeShow = function (c, b) {
		var d = WebSiteType.getInstanceOf(b);
		if (d == null || (!d.isSkyInplayGroup() && !d.isFancyFairGroup())) {
			return true
		}
		return WebSiteUtil.isBookMode(c, b)
	}
}());
if (typeof (CompetitionUtils) == "undefined") {
	CompetitionUtils = {}
} (function () {
	CompetitionUtils.isCloseSite = function (b, a) {
		return WebSiteUtil.isCloseSite(b, a)
	}
}());
if (typeof (GameProductUtils) == "undefined") {
	GameProductUtils = {}
} (function () {
	GameProductUtils.isClose = function (a) {
		return a == 0
	}
}());
if (typeof (JsCache) == "undefined") {
	JsCache = {}
} (function () {
	var a = {};
	JsCache.get = function (c) {
		if (c == null || c.length == 0) {
			return null
		}
		var b = a[c];
		if (!b) {
			b = $j(c);
			if (b.length == 0) {
				return undefined
			}
			a[c] = b
		}
		return b
	};
	JsCache.clone = function (b) {
		var c = JsCache.get(b);
		if (c == null) {
			return null
		}
		return c.clone()
	};
	JsCache.removeCache = function (b) {
		if (b == null || b.length == 0) {
			return null
		}
		a[b] = null
	}
}());
if (typeof (JsonUtil) == "undefined") {
	JsonUtil = {}
} (function () {
	JsonUtil.stringify = function (f) {
		if ("JSON" in window) {
			return JSON.stringify(f)
		}
		var d = typeof (f);
		if (d != "object" || f === null) {
			if (d == "string") {
				f = '"' + f.replace(/"/g, '\\"') + '"'
			}
			return String(f)
		} else {
			var g, b, c = [],
				a = (f && f.constructor == Array);
			for (g in f) {
				b = f[g];
				d = typeof (b);
				if (f.hasOwnProperty(g)) {
					if (d == "string") {
						b = '"' + b.replace(/"/g, '\\"') + '"'
					} else {
						if (d == "object" && b !== null) {
							b = JsonUtil.stringify(b)
						}
					}
					c.push((a ? "" : '"' + g + '":') + String(b))
				}
			}
			return (a ? "[" : "{") + String(c) + (a ? "]" : "}")
		}
	}
}());
if (typeof (WindowEventUtil) == "undefined") {
	WindowEventUtil = {}
} (function () {
	WindowEventUtil.stopEvent = function (c, a, b) {
		if (!c) {
			var c = window.event
		}
		if (!a && !b) {
			a = true;
			b = true
		}
		if (a) {
			if (c.stopPropagation) {
				c.stopPropagation()
			} else {
				c.cancelBubble = true
			}
		}
		if (b) {
			if (c.preventDefault) {
				c.preventDefault()
			} else {
				c.returnValue = false
			}
		}
		return false
	}
}());
if (typeof (UiUtils) == "undefined") {
	UiUtils = {}
} (function () {
	var b = ["-", "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
	var a = ["-", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
	UiUtils.bindToExpand = function (c, d, g) {
		var f = c.parent().parent();
		c.click(function () {
			if (f.hasClass("close")) {
				f.removeClass("close");
				if (d != null && (typeof d === "function")) {
					d.call(this, c)
				}
			} else {
				f.addClass("close");
				if (g != null && (typeof g === "function")) {
					g.call(this, c)
				}
			}
		})
	};
	UiUtils.getShortDayName = function (c) {
		if (c < b.length) {
			return b[c]
		}
		return "-"
	};
	UiUtils.getDayName = function (c) {
		if (c < a.length) {
			return a[c]
		}
		return "-"
	};
	UiUtils.view = function (h, f) {
		for (var c = 0; c < f.length; c++) {
			for (var g = 0; g < f[c]["elem"].length; g++) {
				var d = f[c]["elem"][g];
				if (h == f[c]["name"]) {
					if ($j(d) != null) {
						$j(d).show()
					} else {
						Trace.error(d + " not found")
					}
				} else {
					if ($j(d) != null) {
						$j(d).hide()
					} else {
						Trace.error(d + " not found")
					}
				}
			}
		}
	};
	UiUtils.appendHeight = function (j) {
		var c = 0;
		var h = $j("#overWrap");
		var g = $j("#gameHead");
		var d = $j("#inPlayTab");
		var i = $j("#sportFilter");
		var f = $j(".marquee-box");
		if (g.is(":visible") && g.height() > 0) {
			c = g.height()
		}
		if (d.is(":visible") && d.height() > 0) {
			c += 39
		}
		if (i.is(":visible") && i.height() > 0) {
			c += 55
		}
		if (j) {
			c = 0
		}
		if (PageConfig.ENABLE_ONE_CLICK_BET == "true" && $j("[name=oneClickBetStakeBox]").is(":visible")) {
			if (c > 0 && !d.is(":visible")) {
				c += 15
			}
			c = (c + 31)
		}
		if (f.is(":visible") && f.height() > 0) {
			if (d.is(":visible") && d.height() > 0) {
				c += 25
			} else {
				c += 26
			}
		}
		h.attr("style", "height: calc(100% - " + c + "px)")
	};
	UiUtils.removeContents = function (c) {
		c.contents().filter(function () {
			return this.nodeType == 3
		}).remove()
	}
})();
if (!String.prototype.endsWith) {
	String.prototype.endsWith = function (c, b) {
		var a = this.toString();
		if (typeof b !== "number" || !isFinite(b) || Math.floor(b) !== b || b > a.length) {
			b = a.length
		}
		b -= c.length;
		var d = a.lastIndexOf(c, b);
		return d !== -1 && d === b
	}
}
if (typeof (PositionUtils) == "undefined") {
	PositionUtils = {}
} (function () {
	PositionUtils.getPosition = function (b) {
		var d = 0;
		var c = 0;
		for (var a = 0; a < b.length; a++) {
			var f = b[a].position();
			d = MathUtil.decimal.add(f.left, d);
			c = MathUtil.decimal.add(f.top, c)
		}
		if (PageConfig.ENABLE_ZOOM == "true" && ZoomHandler.zoomLevel > 1 && (BrowserUtil.isFirefox() || BrowserUtil.isIE() || BrowserUtil.isEdge())) {
			d = MathUtil.decimal.divide(d, ZoomHandler.zoomLevel);
			c = MathUtil.decimal.divide(c, ZoomHandler.zoomLevel)
		}
		return {
			left: d,
			top: c
		}
	}
})();
if (typeof (RateUtils) == "undefined") {
	RateUtils = {}
} (function () {
	RateUtils.convertRate = function (c, b, a) {
		return MathUtil.decimal.divide(MathUtil.decimal.multiply(c, b), a)
	}
})();
if (typeof (BrowserUtil) == "undefined") {
	BrowserUtil = {}
} (function () {
	var isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(" OPR/") >= 0;
	var isFirefox = typeof InstallTrigger !== "undefined";
	var isSafari = /constructor/i.test(window.HTMLElement) || (function (p) {
		return p.toString() === "[object SafariRemoteNotification]"
	})(!window.safari || safari.pushNotification);
	var isIE =
		/*@cc_on!@*/
		false || !!document.documentMode;
	var isEdge = !isIE && !!window.StyleMedia;
	var isChrome = !!window.chrome && !!window.chrome.webstore;
	var isBlink = (isChrome || isOpera) && !!window.CSS;
	BrowserUtil.isOpera = function () {
		return isOpera
	};
	BrowserUtil.isFirefox = function () {
		return isFirefox
	};
	BrowserUtil.isSafari = function () {
		return isSafari
	};
	BrowserUtil.isIE = function () {
		return isIE
	};
	BrowserUtil.isEdge = function () {
		return isEdge
	};
	BrowserUtil.isChrome = function () {
		return isChrome
	};
	BrowserUtil.isBlink = function () {
		return isBlink
	}
})();
if (typeof (MarketUtil) == "undefined") {
	MarketUtil = {}
} (function () {
	MarketUtil.isInactive = function (a) {
		return (a == MarketStatusType.Inactive.unique())
	};
	MarketUtil.isOpen = function (a) {
		return (a == MarketStatusType.Open.unique())
	};
	MarketUtil.isSuspend = function (a) {
		return (a == MarketStatusType.Suspend.unique())
	};
	MarketUtil.isClosed = function (a) {
		return (a == MarketStatusType.Closed.unique())
	};
	MarketUtil.isCloseSite = function (b, a) {
		return WebSiteUtil.isCloseSite(b, a)
	};
	MarketUtil.isBookModeShow = function (b, a) {
		var c = WebSiteType.getInstanceOf(a);
		if (c == null || (!c.isSkyInplayGroup() && !c.isFancyFairGroup())) {
			return true
		}
		return WebSiteUtil.isBookMode(b, a)
	};
	MarketUtil.isLineMarket = function (a, b) {
		return ("INNINGS_RUNS" == a && BetfairMarketBettingType.LINE.unique() == b)
	};
	MarketUtil.getLineMarketActualRuns = function (a) {
		return MathUtil.decimal.subtract(a, 0.5)
	};
	MarketUtil.getLineMarketDisplayRuns = function (a) {
		return MathUtil.decimal.add(a, 0.5)
	}
})();
if (typeof (SelectionUtil) == "undefined") {
	SelectionUtil = {}
} (function () {
	SelectionUtil.isRemoved = function (a) {
		return (a == SelectionStatusType.Removed.unique())
	};
	SelectionUtil.isActive = function (a) {
		return (a == SelectionStatusType.Active.unique())
	};
	SelectionUtil.isWinner = function (a) {
		return (a == SelectionStatusType.Winner.unique())
	};
	SelectionUtil.isLoser = function (a) {
		return (a == SelectionStatusType.Loser.unique())
	};
	SelectionUtil.isPlaced = function (a) {
		return (a == SelectionStatusType.Placed.unique())
	};
	SelectionUtil.isRemovedVacant = function (a) {
		return (a == SelectionStatusType.RemovedVacant.unique())
	};
	SelectionUtil.isHidden = function (a) {
		return (a == SelectionStatusType.Hidden.unique())
	};
	SelectionUtil.isCloseSite = function (b, a) {
		return WebSiteUtil.isCloseSite(b, a)
	};
	SelectionUtil.isSuspendSite = function (b, a) {
		return WebSiteUtil.isSuspendSite(b, a)
	};
	SelectionUtil.isBookMode = function (b, a) {
		return WebSiteUtil.isBookMode(b, a)
	};
	SelectionUtil.isBookSuspend = function (b, a) {
		return WebSiteUtil.isBookSuspend(b, a)
	};
	SelectionUtil.isShowSelection = function (c, b, a) {
		if (WebSiteType.getInstanceOf(a).isBookModeGroup()) {
			return SelectionUtil.isBookMode(b, a)
		}
		return !SelectionUtil.isCloseSite(c, a)
	};
	SelectionUtil.isShowSuspend = function (b, c, a) {
		if (WebSiteType.getInstanceOf(a).isBookModeGroup()) {
			return SelectionUtil.isBookSuspend(c, a)
		}
		return SelectionUtil.isSuspendSite(b, a)
	}
})();
if (typeof (MapUtil) == "undefined") {
	MapUtil = {}
} (function () {
	MapUtil.cloneMap = function (g) {
		var a = new HashMap();
		var b = g.entrySet();
		for (var d in b) {
			var c = JSON.parse(JSON.stringify(b[d].key));
			var f = JSON.parse(JSON.stringify(b[d].value));
			a.put(c, f)
		}
		return a
	}
})();
var IPCheckUtil = {
	isIPv6Format: function (b) {
		var a = new RegExp("((^s*((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]))s*$)|(^s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?s*$))");
		if (a.test(b)) {
			return true
		}
		return false
	},
	isIPv4Format: function (b) {
		var a = new RegExp("^([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])\\.([01]?[0-9]?[0-9]|2[0-4][0-9]|25[0-5])$");
		if (a.test(b)) {
			return true
		}
		return false
	},
	isIPv4SubnetFormat: function (b) {
		var a = new RegExp("^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]).){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])(/(3[0-2]|[1-2][0-9]|[0-9]))$");
		return a.test(b)
	}
};
if (typeof (WebSiteUtil) == "undefined") {
	WebSiteUtil = {}
} (function () {
	WebSiteUtil.isCloseSite = function (b, a) {
		if (WebSiteType.getInstanceOf(a).isSkyInplayGroup() || WebSiteType.getInstanceOf(a).isFancyFairGroup()) {
			return false
		}
		return WebSiteUtil.isContainSite(b, a)
	};
	WebSiteUtil.isSuspendSite = function (b, a) {
		return WebSiteUtil.isCloseSite(b, a)
	};
	WebSiteUtil.isBookMode = function (b, a) {
		var c = WebSiteType.getInstanceOf(a);
		if (c == null || (!c.isSkyInplayGroup() && !c.isFancyFairGroup())) {
			return false
		}
		var d = WebSiteType.SKYINPLAY.unique();
		if (c.isFancyFairGroup()) {
			d = WebSiteType.FANCYFAIR.unique()
		}
		return WebSiteUtil.isContainSite(b, d)
	};
	WebSiteUtil.isBookSuspend = function (c, a) {
		var b = WebSiteType.getInstanceOf(a);
		if (b == null || (!b.isSkyInplayGroup() && !b.isFancyFairGroup())) {
			return false
		}
		var d = WebSiteType.SKYINPLAY.unique();
		if (b.isFancyFairGroup()) {
			d = WebSiteType.FANCYFAIR.unique()
		}
		return WebSiteUtil.isContainSite(c, d)
	};
	WebSiteUtil.isDisableBettingSite = function (b, a) {
		var c = WebSiteType.getInstanceOf(a);
		if (c == null || (!c.isSkyInplayGroup() && !c.isFancyFairGroup())) {
			return false
		}
		var d = WebSiteType.SKYINPLAY.unique();
		if (c.isFancyFairGroup()) {
			d = WebSiteType.FANCYFAIR.unique()
		}
		return WebSiteUtil.isContainSite(b, d)
	};
	WebSiteUtil.isContainSite = function (d, a) {
		if (d == null) {
			return false
		}
		try {
			var b = JSON.parse(d);
			if (b.indexOf(a.toString()) > -1) {
				return true
			}
		} catch (c) {
			Trace.log(c)
		}
		return false
	}
})();