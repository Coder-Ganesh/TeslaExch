if (typeof (I18N) == "undefined") {
	I18N = {}
} (function () {
	var a;
	I18N.setResource = function (b) {
		this.resource = b
	};
	I18N.addResource = function (c) {
		this.resource = this.resource || {};
		for (var b in c) {
			this.resource[b] = c[b]
		}
	};
	I18N.get = function (c, b) {
		c = c.toString() || "";
		var e = c;
		if (typeof this.resource !== "undefined") {
			e = (this.resource[c] ? this.resource[c] : c)
		}
		if (typeof b !== "undefined" && b.length > 0) {
			var d = new RegExp("{([0-" + b.length + "])}", "g");
			return String(e).replace(d, function (g, f) {
				return b[f]
			})
		} else {
			return e
		}
	}
})();