if (typeof (PageHandler) == "undefined") {
	PageHandler = {}
} (function () {
	PageHandler.currentPage = -1;
	PageHandler.totalPage = -1;
	PageHandler.pageLimit = 10;
	PageHandler.pageScopeArr = [];
	PageHandler.pageScopeIndex = 0;
	PageHandler.goToPage = false;
	PageHandler.init = function (g, f, i) {
		if (g > f) {
			g = f
		}
		var h = false;
		if (PageHandler.totalPage != f && PageHandler.pageScopeArr.length > 0) {
			h = true
		}
		PageHandler.currentPage = g;
		PageHandler.totalPage = f;
		if (!$j.isFunction(i)) {
			throw "Please implement clickPageFunc = function (pageNumber, showCount) {};"
		}
		d();
		if (h) {
			$j.each(PageHandler.pageScopeArr, function (k, j) {
				if (j.begin <= g && g <= j.end) {
					PageHandler.pageScopeIndex = k;
					return false
				}
			})
		}
		PageHandler.clickPage = function (j, k) {
			i(j, k)
		}
	};
	PageHandler.pageList = function () {
		var v = $j("#pageNumberContent");
		v.html("");
		if (PageHandler.totalPage < 1) {
			return
		}
		var o = e();
		var w = JsCache.get("#prev").clone();
		var j = w.find("a");
		if (PageHandler.currentPage == 1) {
			j.toggleClass("disable");
			j.css("pointer-events", "none")
		} else {
			j.click(function () {
				var i = PageHandler.currentPage - 1;
				PageHandler.clickPage(i);
				if (i < o.begin) {
					c()
				}
			})
		}
		v.append(w);
		if (o.isShowPreMore) {
			var f = JsCache.get("#pageNumber").clone();
			var y = f.find("a");
			y.html(1);
			y.click(function () {
				PageHandler.pageScopeIndex = 0;
				PageHandler.clickPage(1)
			});
			v.append(f);
			var p = JsCache.get("#more").clone();
			var k = p.find("a");
			k.click(function () {
				c();
				var i = e();
				PageHandler.clickPage(i.end)
			});
			v.append(p)
		}
		for (var u = o.begin; u <= o.end; u++) {
			var m = JsCache.get("#pageNumber").clone();
			var q = m.find("a");
			q.html(u);
			if (u == PageHandler.currentPage) {
				q.toggleClass("select");
				q.css("pointer-events", "none")
			} else {
				q.click(a(u))
			}
			v.append(m)
		}
		if (o.isShowAfterMore) {
			var n = JsCache.get("#more").clone();
			var l = n.find("a");
			l.click(function () {
				b();
				var i = e();
				PageHandler.clickPage(i.begin)
			});
			v.append(n);
			var h = JsCache.get("#pageNumber").clone();
			var r = h.find("a");
			r.html(PageHandler.totalPage);
			r.click(function () {
				PageHandler.pageScopeIndex = PageHandler.pageScopeArr.length - 1;
				PageHandler.clickPage(PageHandler.totalPage)
			});
			v.append(h)
		}
		var x = JsCache.get("#next").clone();
		var t = x.find("a");
		if (PageHandler.currentPage == PageHandler.totalPage) {
			t.toggleClass("disable");
			t.css("pointer-events", "none")
		} else {
			t.click(function () {
				var i = PageHandler.currentPage + 1;
				PageHandler.clickPage(i);
				if (i > o.end) {
					b()
				}
			})
		}
		v.append(x);
		if (PageHandler.goToPage) {
			var s = JsCache.get("#goToPageNumber").clone();
			s.attr("id", "goToPageNumber_1");
			var g = JsCache.get("#goPageBtn").clone();
			g.attr("id", "goPageBtn_1");
			v.append(s);
			v.append(g);
			g.click(function () {
				var i = s.val();
				if (StringUtil.isEmpty(i)) {
					alert("Page Number is invalid!");
					return
				}
				if (i > PageHandler.totalPage) {
					i = PageHandler.totalPage
				}
				while (i < e().begin) {
					c()
				}
				while (i > e().end) {
					b()
				}
				PageHandler.clickPage(i)
			})
		}
	};

	function a(f) {
		return function () {
			PageHandler.clickPage(f)
		}
	}

	function e() {
		return PageHandler.pageScopeArr[PageHandler.pageScopeIndex]
	}

	function c() {
		return PageHandler.pageScopeIndex = PageHandler.pageScopeIndex - 1
	}

	function b() {
		return PageHandler.pageScopeIndex = PageHandler.pageScopeIndex + 1
	}

	function d() {
		PageHandler.pageScopeArr = [];
		var f = PageHandler.pageLimit;
		var o = PageHandler.totalPage - PageHandler.pageLimit;
		if (o == 1) {
			PageHandler.pageLimit = PageHandler.totalPage
		}
		var k = Math.ceil(PageHandler.totalPage / PageHandler.pageLimit);
		var g = 1;
		var j = PageHandler.pageLimit;
		for (var l = 1; l <= k; l++) {
			if (PageHandler.totalPage < j) {
				j = PageHandler.totalPage
			}
			var h = (j - g + 1);
			if (h < PageHandler.pageLimit) {
				g = j - PageHandler.pageLimit + 1;
				if (g < 1) {
					g = 1
				}
			}
			var m = true;
			if (l == 1) {
				m = false
			}
			var p = true;
			if (l == k) {
				p = false
			}
			var n = {
				begin: g,
				end: j,
				isShowPreMore: m,
				isShowAfterMore: p
			};
			PageHandler.pageScopeArr.push(n);
			g = j + 1;
			j = j + PageHandler.pageLimit
		}
		PageHandler.pageLimit = f
	}
	PageHandler.validatePageNumber = function (f) {
		var g = f.value;
		f.value = g.replace(/[\D]|^0|^0[\d]+/, "")
	}
})();