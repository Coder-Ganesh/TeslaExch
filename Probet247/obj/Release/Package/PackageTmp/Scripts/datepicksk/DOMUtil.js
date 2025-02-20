if (typeof (DOMUtil) == "undefined") {
	DOMUtil = {}
} (function () {
	var a = {
		ELEMENT_NODE: 1,
		ATTRIBUTE_NODE: 2,
		TEXT_NODE: 3,
		CDATA_SECTION_NODE: 4,
		ENTITY_REFERENCE_NODE: 5,
		ENTITY_NODE: 6,
		PROCESSING_INSTRUCTION_NODE: 7,
		COMMENT_NODE: 8,
		DOCUMENT_NODE: 9,
		DOCUMENT_TYPE_NODE: 10,
		DOCUMENT_FRAGMENT_NODE: 11,
		NOTATION_NODE: 12
	};
	DOMUtil.selectAnchor = function (b, c) {
		b.className = c
	};
	DOMUtil.findById = function (c) {
		var b = null;
		b = document.getElementById(c);
		if (b != null) {
			return b
		}
		b = document.getElementsByName(c);
		if (b != null) {
			return b[0]
		}
		return null
	};
	DOMUtil.insertAfter = function (b, d) {
		var c = b.parentNode;
		if (c.lastChild == b) {
			c.appendChild(d)
		} else {
			c.insertBefore(d, b.nextSibling)
		}
	};
	DOMUtil.prependChild = function (b, d) {
		var c = b.parentNode;
		if (c.firstChild) {
			c.insertBefore(d, c.firstChild)
		} else {
			c.appendChild(d)
		}
	};
	DOMUtil.getAttribute = function (c, b) {
		if (c.attributes[b]) {
			return c.attributes[b].nodeValue
		}
		return null
	};
	DOMUtil.setAttribute = function (c, b, d) {
		c.setAttribute(b, d)
	};
	DOMUtil.setClass = function (b, c) {
		b.className = c
	};
	DOMUtil.setStyle = function (c, b, d) {
		c.style[b] = d
	};
	DOMUtil.nextElement = function (b) {
		while ((b = b.nextSibling)) {
			if (b.nodeType == a.ELEMENT_NODE) {
				return b
			}
		}
	};
	DOMUtil.nextChildElement = function (e, f) {
		var c = (f ? f : 0);
		for (var d = 0, b = 0; d < e.childNodes.length; d++) {
			if (e.childNodes[d].nodeType == a.ELEMENT_NODE) {
				if (b == c) {
					return e.childNodes[d]
				}
				b++
			}
		}
	};
	DOMUtil.findChildElementByName = function (d, e) {
		for (var b = d.firstChild; b != null; b = b.nextSibling) {
			if (b.nodeType == a.ELEMENT_NODE) {
				if (e == DOMUtil.getAttribute(b, "name")) {
					return b
				}
				if (b.hasChildNodes()) {
					var c = DOMUtil.findChildElementByName(b, e);
					if (c != null) {
						return c
					}
				}
			}
		}
		return null
	};
	DOMUtil.getNextChildElementByName = function (d, b) {
		for (var c = 0; c < d.childNodes.length; c++) {
			if (d.childNodes[c].nodeType == a.ELEMENT_NODE) {
				if (b == DOMUtil.getAttribute(d.childNodes[c], "name")) {
					return d.childNodes[c]
				}
			}
		}
	};
	DOMUtil.hasTBody = function (c) {
		var e = c.getElementsByTagName("tbody");
		if (!e) {
			return false
		}
		for (var d = 0; d < e.length; d++) {
			var b = e[d];
			if (b.firstChild) {
				return true
			}
		}
		return false
	}
})();