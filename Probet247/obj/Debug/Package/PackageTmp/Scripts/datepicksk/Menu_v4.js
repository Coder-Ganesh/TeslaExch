if (typeof (TopMenuHandler) == "undefined") {
	TopMenuHandler = {}
} (function () {
	var b = {};
	TopMenuHandler.init = function () {
		if (PageConfig.hasAccount) {
			LogoutUtilHandler.init();
			var d = $j("#logout");
			if (d.length > 0) {
				d.click(function () {
					try {
						if (PageConfig.isFromAPI) {
							LogoutUtilHandler.logoutAPI()
						} else {
							LogoutUtilHandler.logout()
						}
					} catch (f) { }
				})
			}
			JsCache.get("#accountPopup").click(function (e) {
				TopMenuHandler.accountPopup(this);
				$j(document).click(function () {
					JsCache.get("#account_pop").hide();
					$j(document).unbind("click")
				});
				if (JsCache.get("#set_pop").is(":visible")) {
					CoinHandler.coinSelectCancel();
					SparkHandler.setEnableCheckBox();
					TopMenuHandler.appendAllAcceptAnyOdds();
					JsCache.get("#set_pop").hide()
				}
				e.stopPropagation()
			});
			JsCache.get("#menuRefresh").click(function (e) {
				JsCache.get("#accountCredit").hide();
				JsCache.get("#menuRefreshIcon").show();
				TopMenuHandler.getPlayerInfoTask.refresh();
				setTimeout(function () {
					JsCache.get("#accountCredit").show();
					JsCache.get("#menuRefreshIcon").hide()
				}, 1000)
			})
		}
		var c = "height=330,width=680,location=no";
		$j("#openStreaming").click(function () {
			if (typeof (LoginHandler) != "undefined" && !LoginHandler.userIsLogin()) {
				return
			}
			window.open(PageConfig.landingPath + "/member/streaming/streaming.jsp", "_blank", c)
		});
		$j("#casinoLoginBtn").on("click", function () {
			if (typeof (LoginHandler) != "undefined" && !LoginHandler.userIsLogin()) {
				return
			}
			var e = "height=1000,width=1256,location=no";
			window.open(PageConfig.PLAYER_AWC_CASINO_PAGE, "_blank", e)
		});
		$j("#casinoBetgameLoginBtn").on("click", function () {
			if (typeof (LoginHandler) != "undefined" && !LoginHandler.userIsLogin()) {
				return
			}
			$j.ajax({
				type: "POST",
				dataType: "JSON",
				url: "/exchange/member/vendorController/loginAndLaunchGame",
				data: {
					platform: PageConfig.launchBetGamePlatform
				},
				success: function (e) {
					if (e == null) {
						return
					}
					if (e.error) {
						NoticeHandler.error(e.error);
						return
					}
					if (e.success) {
						window.open(e.success)
					}
				}
			})
		});
		a("fancyBet");
		a("sportsBook")
	};
	TopMenuHandler.accountPopup = function (c) {
		JsCache.get("#account_pop").toggle();
		return
	};
	TopMenuHandler.getPlayerInfoTask = TaskExecuter.createTask(5, 0, function () {
		var c = this;
		TopMenuHandler.getPlayerInfo(c)
	});
	TopMenuHandler.getPlayerInfo = function (c) {
		if (c == null) {
			return
		}
		$j.ajax({
			type: "POST",
			dataType: "JSON",
			url: "/member/playerService/queryPlayerInfo",
			data: {},
			success: function (g) {
				try {
					if (g == null) {
						return
					}
					var k = JsCache.get("#betCredit");
					var f = JsCache.get("#totalExposure");
					var d = CurrencyType.getInstanceOf(PageConfig.playerCurrency);
					var h = function (m, l) {
						var e = StringUtil.getlength(StringUtil.trim(l.currencySymbol)) == 0 ? "" : l.currencySymbol;
						if (m.indexOf("-") == 0) {
							return "(" + e + m.replace("-", "") + ")"
						}
						return e + m
					};
					var i = {
						precision: 2,
						separateSign: ",",
						currencySymbol: d.symbol,
						formatter: h,
						"trailingZeros,": false
					};
					k.removeClass("red");
					f.removeClass("red");
					k.html(d.name + " " + CurrencyUtil.formatter(g.betCredit, i));
					f.html(CurrencyUtil.formatter(g.totalExposure, i));
					if (g.betCredit < 0) {
						k.addClass("red")
					}
					if (g.totalExposure < 0) {
						f.addClass("red")
					}
					if (g.minBet != null) {
						PageConfig.minBet = g.minBet
					}
				} catch (j) {
					Trace.printStackTrace(j)
				} finally {
					c.check();
					TaskExecuter.execute()
				}
			},
			error: function () {
				c.check();
				TaskExecuter.execute()
			}
		})
	};

	function a(c) {
		b[c] = CookieUtil.getCookie(c + "AcceptAnyOdds_" + PageConfig.userID);
		if (b[c] == null) {
			b[c] = 0;
			if ("sportsBook" == c) {
				b[c] = 1
			}
		}
		TopMenuHandler.appendAcceptAnyOdds(c);
		var d = $j("#set_pop");
		d.find("#coinSave").on("click", function (f) {
			TopMenuHandler.getAcceptAnyOdds(c);
			TopMenuHandler.setCookie(c);
			TopMenuHandler.appendAcceptAnyOdds(c)
		});
		d.find("#closeSet").on("click", function () {
			TopMenuHandler.appendAcceptAnyOdds(c)
		})
	}
	TopMenuHandler.appendAcceptAnyOdds = function (d) {
		var c = $j("#" + d + "AcceptAnyOddsCheckBox");
		var e = $j("[id^=" + d + "AcceptAnyOdds]");
		if (b[d] == 1) {
			c.prop("checked", true);
			e.prop("checked", true);
			return
		}
		c.prop("checked", false);
		e.prop("checked", false)
	};
	TopMenuHandler.appendAllAcceptAnyOdds = function () {
		TopMenuHandler.appendAcceptAnyOdds("fancyBet");
		TopMenuHandler.appendAcceptAnyOdds("sportsBook")
	};
	TopMenuHandler.getAcceptAnyOdds = function (d) {
		var c = $j("#" + d + "AcceptAnyOddsCheckBox");
		if (c.prop("checked") == true) {
			b[d] = 1
		} else {
			b[d] = 0
		}
	};
	TopMenuHandler.setCookie = function (c) {
		CookieUtil.setCookie(c + "AcceptAnyOdds_" + PageConfig.userID, b[c], 999)
	};
	TopMenuHandler.isAcceptAnyOdds = function (c) {
		return b[c] == 1
	}
})();
var TabMenuHandler = {};
(function () {
	TabMenuHandler.updateSelect = function (b, a) {
		JsCache.get("#" + b).find("a").removeClass("select");
		JsCache.get("#" + b).find("#" + a).addClass("select")
	}
})();
if (typeof (SparkHandler) == "undefined") {
	SparkHandler = {}
} (function () {
	var a = 1;
	SparkHandler.init = function () {
		var b = JsCache.get("#enableSparkCheck");
		a = CookieUtil.getCookie("enableSpark_" + PageConfig.userID);
		if (a == null) {
			a = 1
		}
		if (a == 1) {
			b.prop("checked", true)
		} else {
			b.prop("checked", false)
		}
		JsCache.get("#slipSet").click(function (c) {
			$j(document).click(function () {
				SparkHandler.setEnableCheckBox();
				$j(document).unbind("click")
			})
		});
		JsCache.get("#coinSave").click(function (c) {
			SparkHandler.getEnableSpark();
			SparkHandler.setCookie();
			SparkHandler.setEnableCheckBox();
			c.stopPropagation()
		});
		JsCache.get("#closeSet").click(function (c) {
			SparkHandler.setEnableCheckBox()
		})
	};
	SparkHandler.setCookie = function () {
		CookieUtil.setCookie("enableSpark_" + PageConfig.userID, a, 999)
	};
	SparkHandler.setEnableCheckBox = function () {
		var b = JsCache.get("#enableSparkCheck");
		if (a == 1) {
			b.prop("checked", true)
		} else {
			b.prop("checked", false)
		}
	};
	SparkHandler.getEnableSpark = function () {
		var b = JsCache.get("#enableSparkCheck");
		if (b.prop("checked") == true) {
			a = 1
		} else {
			a = 0
		}
	};
	SparkHandler.addSparkClass = function (d, c, b) {
		if (a == 1 && (c != null || b != null)) {
			var f = d.prop("sparkOdds");
			var e = d.prop("sparkStake");
			if ((f != null && f != c) || (e != null && e != b)) {
				d.addClass("spark")
			} else {
				d.removeClass("spark")
			}
			if (c != null) {
				d.prop("sparkOdds", c)
			}
			if (b != null) {
				d.prop("sparkStake", b)
			}
		}
	};
	SparkHandler.addFancyBetSpark = function (c, g, b) {
		if (a == 1 && (g != null || b != null)) {
			var e = c.prop("sparkRuns");
			var f = c.prop("sparkOdds");
			if (e != g || f != b) {
				c.addClass("spark");
				var d = setTimeout(function () {
					c.removeClass("spark");
					if (d != null) {
						clearTimeout(d)
					}
				}, 1000)
			}
			if (e != g) {
				c.prop("sparkRuns", g)
			}
			if (f != b) {
				c.prop("sparkOdds", b)
			}
		}
	}
})();
if (typeof (ZoomHandler) == "undefined") {
	ZoomHandler = {}
} (function () {
	ZoomHandler.zoomLevel = 1;
	var a = {
		S: {
			name: "S",
			value: 1
		},
		M: {
			name: "M",
			value: 1.2
		},
		L: {
			name: "L",
			value: 1.35
		}
	};
	ZoomHandler.init = function () {
		var b = null;
		ZoomHandler.zoomLevel = ZoomHandler.readCookie();
		if (ZoomHandler.zoomLevel == a.S.value) {
			b = JsCache.get("#zoom_S")
		} else {
			if (ZoomHandler.zoomLevel == a.M.value) {
				b = JsCache.get("#zoom_M")
			} else {
				if (ZoomHandler.zoomLevel == a.L.value) {
					b = JsCache.get("#zoom_L")
				}
			}
		}
		ZoomHandler.updateZoom(b, ZoomHandler.zoomLevel);
		JsCache.get("#zoom_S").click(function () {
			ZoomHandler.updateZoom(JsCache.get("#zoom_S"), a.S.value)
		});
		JsCache.get("#zoom_M").click(function () {
			ZoomHandler.updateZoom(JsCache.get("#zoom_M"), a.M.value)
		});
		JsCache.get("#zoom_L").click(function () {
			ZoomHandler.updateZoom(JsCache.get("#zoom_L"), a.L.value)
		})
	};
	ZoomHandler.updateZoom = function (c, b) {
		ZoomHandler.zoomLevel = b;
		$j("body").css({
			zoom: ZoomHandler.zoomLevel,
			"-moz-transform": "scale(" + ZoomHandler.zoomLevel + ")",
			"-moz-transform-origin": "left top"
		});
		$j("#zoomBoard").find("[id^=zoom_]").removeClass("select");
		c.addClass("select");
		ZoomHandler.saveCookie(b)
	};
	ZoomHandler.readCookie = function () {
		var b = CookieUtil.getCookie("zoomSize_" + PageConfig.userID);
		if (b == null || ZoomHandler.isValid(b) == false) {
			return 1
		}
		return b
	};
	ZoomHandler.saveCookie = function (b) {
		if (ZoomHandler.isValid(b) == false) {
			CookieUtil.setCookie("zoomSize_" + PageConfig.userID, a.S.value, 999);
			return
		}
		CookieUtil.setCookie("zoomSize_" + PageConfig.userID, b, 999)
	};
	ZoomHandler.isValid = function (b) {
		return (b == a.S.value || b == a.M.value || b == a.L.value)
	}
})();