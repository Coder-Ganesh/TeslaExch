if (typeof (KeyEventUtils) == "undefined") {
	KeyEventUtils = {}
} (function () {
	KeyEventUtils.isNumberKey = function (b) {
		var a = b.which || b.keyCode;
		return (!b.shiftKey && ((a > 47 && a < 58) || (a > 95 && a < 106)))
	};
	KeyEventUtils.isBackspaceKey = function (a) {
		return (a === 8)
	};
	KeyEventUtils.isDeleteKey = function (a) {
		return (a === 46)
	};
	KeyEventUtils.isEnterKey = function (a) {
		return (a === 13)
	};
	KeyEventUtils.isEscKey = function (a) {
		return (a == 27)
	};
	KeyEventUtils.isShiftKey = function (a) {
		return (a === 16)
	};
	KeyEventUtils.isCtrlKey = function (a) {
		return (a === 17)
	};
	KeyEventUtils.isAltKey = function (a) {
		return (a === 18)
	};
	KeyEventUtils.isArrowKey = function (a) {
		return (a > 36 && a < 41)
	};
	KeyEventUtils.isUpArrowKey = function (a) {
		return (a === 38)
	};
	KeyEventUtils.isDownArrowKey = function (a) {
		return (a === 40)
	};
	KeyEventUtils.isF5Key = function (a) {
		return (a == 116)
	};
	KeyEventUtils.isTabKey = function (a) {
		return (a == 9)
	};
	KeyEventUtils.isDecimalPointKey = function (a) {
		return (a == 110 || a == 190)
	};
	KeyEventUtils.isSubtractKey = function (b) {
		var a = b.which || b.keyCode;
		return (!b.shiftKey && (a == 109 || a == 189 || a == 173))
	};
	KeyEventUtils.isPlusKey = function (b) {
		var a = b.which || b.keyCode;
		return (!b.shiftKey && (a == 107 || a == 187 || a == 61))
	};
	KeyEventUtils.isSpaceKey = function (a) {
		return (a == 32)
	};
	KeyEventUtils.isAlphabetKey = function (a) {
		return (a > 64 && a < 91)
	};
	KeyEventUtils.isParenthesis = function (b) {
		var a = b.which || b.keyCode;
		return (b.shiftKey && (a == 57 || a == 48))
	};
	KeyEventUtils.isIME = function (a) {
		return (a == 229)
	};
	KeyEventUtils.isRefresh = function (b) {
		var a = b.which || b.keyCode;
		return KeyEventUtils.isF5Key(a) || a == 82 && b.ctrlKey
	};
	KeyEventUtils.checkNumberKeyAndValue = function (f, d, a, g, b) {
		var c = f.which || f.keyCode;
		if (!(KeyEventUtils.isNumberKey(f) || (a && KeyEventUtils.isDecimalPointKey(c)) || (d && KeyEventUtils.isSubtractKey(f)) || KeyEventUtils.isBackspaceKey(c) || KeyEventUtils.isDeleteKey(c))) {
			return false
		}
		if (d && b.length !== 0 && KeyEventUtils.isSubtractKey(f)) {
			return false
		}
		if (((d && b === "-") || b.length === 0 || b.indexOf(".") !== -1) && KeyEventUtils.isDecimalPointKey(c)) {
			return false
		}
		if (MathUtil.outOfScale(b, g - 1) && KeyEventUtils.isNumberKey(f)) {
			return false
		}
		return true
	}
})();