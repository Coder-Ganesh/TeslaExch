if (typeof (MathUtil) == "undefined") {
	MathUtil = {}
} (function () {
	MathUtil.decimal = {};
	MathUtil.decimal.createDecimalSet = function (f, b, e) {
		var c = new Array();
		for (var d = f; d <= b; d = MathUtil.decimal.add(d, e)) {
			c.push(d)
		}
		return c
	};
	MathUtil.decimal.add = function (c, b) {
		return function (i, h) {
			if (i == null || i.length == 0) {
				i = 0
			}
			if (h == null || h.length == 0) {
				h = 0
			}
			var g, f, d;
			try {
				g = i.toString().split(".")[1].length
			} catch (j) {
				g = 0
			}
			try {
				f = h.toString().split(".")[1].length
			} catch (j) {
				f = 0
			}
			d = Math.pow(10, Math.max(g, f));
			return (a(i, d) + a(h, d)) / d
		}(c, b)
	};
	MathUtil.decimal.subtract = function (c, b) {
		return function (i, h) {
			if (i == null || i.length == 0) {
				i = 0
			}
			if (h == null || h.length == 0) {
				h = 0
			}
			var g, f, d;
			try {
				g = i.toString().split(".")[1].length
			} catch (j) {
				g = 0
			}
			try {
				f = h.toString().split(".")[1].length
			} catch (j) {
				f = 0
			}
			d = Math.pow(10, Math.max(g, f));
			return (a(i, d) - a(h, d)) / d
		}(c, b)
	};
	MathUtil.decimal.multiply = function (c, b) {
		return function (e, d) {
			if (e == null || e.length == 0) {
				e = 0
			}
			if (d == null || d.length == 0) {
				d = 0
			}
			return a(e, d)
		}(c, b)
	};
	MathUtil.decimal.divide = function (c, b) {
		return function (h, g) {
			var j = 0,
				i = 0,
				f, d;
			try {
				j = h.toString().split(".")[1].length
			} catch (k) { }
			try {
				i = g.toString().split(".")[1].length
			} catch (k) { }
			f = Number(h.toString().replace(".", ""));
			d = Number(g.toString().replace(".", ""));
			return a((f / d), Math.pow(10, i - j))
		}(c, b)
	};
	MathUtil.isInteger = function (b) {
		return /^\d+$/.test(b)
	};
	MathUtil.isNegativeInteger = function (b) {
		return /^-\d+$/.test(b)
	};
	MathUtil.isNumeric = function (h, d, c) {
		var g = true;
		var f = 0;
		var b = "0123456789";
		if (d) {
			b += "."
		}
		if (c) {
			b += "-"
		}
		var e;
		if (h.length == 0) {
			return false
		}
		for (var j = 0; j < h.length && g == true; j++) {
			e = h.charAt(j);
			if (c && e == "-" && j > 0) {
				g = false
			}
			if (d && e == ".") {
				f = f + 1;
				if (j == 0 || j == h.length - 1) {
					g = false
				}
				if (f > 1) {
					g = false
				}
			}
			if (b.indexOf(e) == -1) {
				g = false
			}
		}
		return g
	};
	MathUtil.isPositive = function (b) {
		if ((b.toString()).indexOf(",") > -1) {
			b = parseInt(StringUtil.replaceAll(b, ",", ""))
		}
		return (b >= 0)
	};
	MathUtil.isNegative = function (b) {
		if ((b.toString()).indexOf(",") > -1) {
			b = parseInt(StringUtil.replaceAll(b, ",", ""))
		}
		return (b < 0)
	};
	MathUtil.roundp = function (b, d) {
		var c = Math.pow(10, d);
		return Math.round(b * c) / c
	};
	MathUtil.floorp = function (b, d) {
		var c = Math.pow(10, d);
		return Math.floor(b * c) / c
	};
	MathUtil.outOfScale = function (c, d) {
		var b = ("" + c).split(".");
		return (b.length > 1 && b[1].length > d)
	};
	var a = function (h, f) {
		var g = 0,
			d = 0,
			c, b, k = h.toString(),
			j = f.toString();
		try {
			g = k.split(".")[1].length
		} catch (i) { }
		try {
			d = j.split(".")[1].length
		} catch (i) { }
		c = Number(k.toString().replace(".", ""));
		b = Number(j.toString().replace(".", ""));
		return c * b / Math.pow(10, g + d)
	};
	MathUtil.kFormatter = function (b) {
		return b > 999 ? MathUtil.trailingZeros((b / 1000).toFixed(0)) + "K" : b
	};
	MathUtil.trailingZeros = function (b) {
		if (b.match(/\./)) {
			return b.replace(/\.?0*$/, "")
		}
		return b
	}
})();