if (typeof (CategoryUtil) == "undefined") {
	CategoryUtil = {}
} (function () {
	CategoryUtil.getMatchOdds = function (c, a, b) {
		if (CategoryType.FANCY_BET.value == c) {
			b = a + "/" + b
		}
		return b
	};
	CategoryUtil.getAverageMatchedOdds = function (b, a) {
		if (CategoryType.FANCY_BET.value == b) {
			return "-"
		}
		return (a ? a : "-")
	};
	CategoryUtil.isFancyBet = function (a) {
		return (a && CategoryType.FANCY_BET.value == a)
	};
	CategoryUtil.getFancySideTypeCss = function (b) {
		var a = b.toLowerCase();
		if (FancySideType.YES.name.toLowerCase() == a || SideType.Back.name.toLowerCase() == a) {
			return "back"
		} else {
			if (FancySideType.NO.name.toLowerCase() == a || SideType.Lay.name.toLowerCase() == a) {
				return "lay"
			}
		}
	}
})();