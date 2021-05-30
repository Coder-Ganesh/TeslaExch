if (typeof (CurrencyUtil) == "undefined") {
	CurrencyUtil = {}
} (function () {
	CurrencyUtil.roundingMode = {};
	CurrencyUtil.roundingMode.DOWN = "DOWN";
	CurrencyUtil.format = function (c) {
		var g = new String(c).replace(/,/g, "");
		var h = g.split(".");
		var f = h[0];
		var d = h[1] ? "." + h[1] : "";
		var e = /(\d+)(\d{3})/;
		while (e.test(f)) {
			f = f.replace(e, "$1,$2")
		}
		return v = f + d
	};
	CurrencyUtil.isNumeric = function (k, e, d) {
		var h = true;
		var g = 0;
		var c = "0123456789";
		if (e) {
			c += "."
		}
		if (d) {
			c += "-"
		}
		var f;
		for (var l = 0; l < k.length && h == true; l++) {
			f = k.charAt(l);
			if (d && f == "-" && l > 0) {
				h = false
			}
			if (e && f == ".") {
				g = g + 1;
				if (l == 0 || l == k.length - 1) {
					h = false
				}
				if (g > 1) {
					h = false
				}
			}
			if (c.indexOf(f) == -1) {
				h = false
			}
		}
		return h
	};
	CurrencyUtil.DefaultFormatter = function (d, c) {
		if (d.indexOf("-") == 0) {
			return '<span class="red">(' + c.currencySymbol + d.replace("-", "") + ")</span>"
		}
		return c.currencySymbol + d
	};
	CurrencyUtil.MobileDefaultFormatter = function (d, c) {
		if (d.indexOf("-") == 0) {
			return "(" + c.currencySymbol + d.replace("-", "") + ")"
		}
		return c.currencySymbol + d
	};
	CurrencyUtil.setting = {
		precision: 2,
		separateSign: ",",
		currencySymbol: "$",
		formatter: CurrencyUtil.DefaultFormatter,
		"trailingZeros,": false
	};
	CurrencyUtil.updateSetting = function (e) {
		for (var d in e) {
			if (!e.hasOwnProperty(d)) {
				continue
			}
			if (d == "precision") {
				var c = e[d];
				if (c % 1 === 0 && c > -1) {
					CurrencyUtil.setting[d] = c
				}
			} else {
				CurrencyUtil.setting[d] = e[d]
			}
		}
	};
	CurrencyUtil.formatter = function (d, c) {
		c = (c == undefined ? CurrencyUtil.setting : c);
		d = b(d, c.precision, c.separateSign, c.roundingMode);
		if (c.trailingZeros) {
			d = a(d)
		}
		return c.formatter(d, c)
	};

	function b(f, l, e, g) {
		if (g && CurrencyUtil.roundingMode.DOWN == g) {
			if (f < 0) {
				return CurrencyUtil.thousandComma((Math.ceil((f * Math.pow(10, l)).toFixed(1)) / Math.pow(10, l)).toFixed(l))
			}
			return CurrencyUtil.thousandComma((Math.floor((f * Math.pow(10, l)).toFixed(1)) / Math.pow(10, l)).toFixed(l))
		}
		var k = f,
			l = isNaN(l = Math.abs(l)) ? 2 : l;
		var h = ".";
		e = e == undefined ? "," : e;
		s = k.toFixed(l) < 0 ? "-" : "";
		i = parseInt(k = Math.abs(+k || 0).toFixed(l)) + "";
		j = (j = i.length) > 3 ? j % 3 : 0;
		return s + (j ? i.substr(0, j) + e : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + e) + (l ? h + Math.abs(k - i).toFixed(l).slice(2) : "")
	}

	function a(c) {
		if (c.match(/\./)) {
			return c.replace(/\.?0*$/, "")
		}
		return c
	}
	CurrencyUtil.thousandComma = function (c) {
		var e = c.toString().split(".");
		var d = /(-?\d+)(\d{3})/;
		while (d.test(e[0])) {
			e[0] = e[0].replace(d, "$1,$2")
		}
		if (e[1]) {
			return e[0] + "." + e[1]
		}
		return e[0]
	};
	CurrencyUtil.getFormatter = function (f) {
		var e = {};
		var c = {};
		for (var d in CurrencyUtil.setting) {
			c[d] = f.hasOwnProperty(d) ? f[d] : CurrencyUtil.setting[d]
		}
		e.setting = c;
		e.format = function (g) {
			return CurrencyUtil.formatter(g, e.setting)
		};
		return e
	}
})();
if (typeof (RateToSGUtils) == "undefined") {
	RateToSGUtils = {}
} (function () {
	RateToSGUtils.convertRate = function (f, g, a, c) {
		if (f == g) {
			return a
		}
		var b = c.get(f);
		var e = c.get(g);
		var d = MathUtil.decimal.divide(e, b);
		return Math.floor(MathUtil.decimal.multiply(a, d))
	};
	RateToSGUtils.convertRateForecast = function (f, g, a, c) {
		if (f == g) {
			return a
		}
		var b = c.get(f);
		var e = c.get(g);
		var d = MathUtil.decimal.divide(e, b);
		return MathUtil.decimal.multiply(a, d)
	}
})();