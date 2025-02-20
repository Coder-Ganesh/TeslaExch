if (typeof (DateUtil) == "undefined") {
	DateUtil = {}
} (function () {
	var a = {
		1: "Jan",
		2: "Feb",
		3: "Mar",
		4: "Apr",
		5: "May",
		6: "Jun",
		7: "Jul",
		8: "Aug",
		9: "Sep",
		10: "Oct",
		11: "Nov",
		12: "Dec"
	};
	DateUtil.format = function (c, d) {
		c = c ? new Date(c) : new Date;
		if (isNaN(c)) {
			throw SyntaxError("invalid date")
		}
		d = d.replace(/yyyy/, c.getFullYear());
		var b = c.getMonth() + 1;
		d = d.replace(/MMM/, a[b]);
		d = d.replace(/MM/, (b < 10 ? "0" + b : b));
		b = c.getDate();
		d = d.replace(/dd/, (b < 10 ? "0" + b : b));
		b = c.getHours();
		d = d.replace(/hh/, (b < 10 ? "0" + b : b));
		d = d.replace(/TT/, b < 12 ? "AM" : "PM");
		b = c.getMinutes();
		d = d.replace(/mm/, (b < 10 ? "0" + b : b));
		b = c.getSeconds();
		d = d.replace(/ss/, (b < 10 ? "0" + b : b));
		return d
	};
	DateUtil.countTime = function (c, b) {
		b.setTime(b.getTime() + 60000);
		c.html(DateUtil.format(b, "MM/dd/yyyy hh:mm") + " GMT+8");
		setTimeout(function () {
			DateUtil.countTime(c, b)
		}, 60000)
	};
	DateUtil.showTime = function (c) {
		var b = new Date(PageConfig.serverCurrentTimeMillis);
		c.html(DateUtil.format(b, "MM/dd/yyyy hh:mm") + " GMT+8");
		var d = b.getSeconds();
		b.setTime(b.getTime() - (d * 1000));
		setTimeout(function () {
			DateUtil.countTime(c, b)
		}, (60 - d) * 1000)
	};
	DateUtil.compare = function (d, c) {
		return (isFinite(d = DateUtil.convert(d).valueOf()) && isFinite(c = DateUtil.convert(c).valueOf()) ? (d > c) - (d < c) : NaN)
	};
	DateUtil.convert = function (b) {
		return (b.constructor === Date ? b : b.constructor === Array ? new Date(b[0], b[1], b[2]) : b.constructor === Number ? new Date(b) : b.constructor === String ? new Date(b) : typeof b === "object" ? new Date(b.year, b.month, b.date) : NaN)
	};
	DateUtil.getDateFormat = function (b) {
		from = b.split("/");
		return new Date(from[2], from[1] - 1, from[0])
	};
	DateUtil.onInputHHmm = function (d) {
		if (d.length > 0 && !StringUtil.isOnlyNumberAndColon(d)) {
			return ""
		}
		var c = (d.match(/:/g) || []).length;
		if (c > 1) {
			d = d.replace(/:/g, "")
		}
		if (d.length == 1) {
			if (d.charAt(0) == ":") {
				return ""
			}
		}
		if (d.length == 2) {
			if (parseInt(d) > 23) {
				d = "23"
			}
			if (d.charAt(1) == ":") {
				d = d.replace(/:/g, "")
			}
			return d
		}
		if (d.length > 2) {
			var b = d.charAt(2);
			if (b != ":") {
				d = d.replace(/:/g, "");
				d = d.substr(0, 2) + ":" + d.substr(2, d.length);
				return d
			}
		}
		if (d.length == 5) {
			var e = d.substr(3, 5);
			if (parseInt(e) > 59) {
				e = "59";
				d = d.substr(0, 3) + e
			}
			return d
		}
		if (d.length > 5) {
			return d.substr(0, 5)
		}
		return d
	}
})();