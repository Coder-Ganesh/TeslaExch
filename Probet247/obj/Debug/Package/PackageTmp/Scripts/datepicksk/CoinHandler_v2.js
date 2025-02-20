if (typeof (CoinHandler) == "undefined") {
	CoinHandler = {}
} (function () {
	var o = new Array();
	var j = 0;
	var k = new Array();
	var l = 0;
	var n = 8;
	var i = [];
	var c = 7;
	CoinHandler.init = function () {
		d();
		JsCache.get("[id^=coin_]").click(function () {
			CoinHandler.coinListClick(this)
		});
		JsCache.get("#slipSet").click(function (r) {
			if (typeof (LoginHandler) != "undefined" && !LoginHandler.userIsLogin()) {
				return
			}
			k = o;
			l = j;
			JsCache.get("#set_pop").toggle();
			$j(document).click(function () {
				CoinHandler.coinSelectCancel();
				TopMenuHandler.appendAllAcceptAnyOdds();
				JsCache.get("#set_pop").hide();
				$j(document).unbind("click")
			});
			if (JsCache.get("#set_pop").is(":visible")) {
				if (PageConfig.hasAccount) {
					JsCache.get("#account_pop").hide()
				}
			}
			r.stopPropagation()
		});
		JsCache.get("#set_pop").click(function (r) {
			r.stopPropagation()
		});
		JsCache.get("#closeSet").click(function () {
			if (JsCache.get("#closeSet").hasClass("disable")) {
				return
			}
			CoinHandler.coinSelectCancel();
			JsCache.get("#set_pop").hide()
		});
		JsCache.get("#coinSave").click(function () {
			if (JsCache.get("#coinSave").hasClass("disable")) {
				return
			}
			CoinHandler.coinSelectSubmit(this)
		});
		var q = $j("#coinList");
		q.find("#edit").unbind("click").click(function () {
			$j(document).keydown(function (s) {
				var r = s.which;
				if (KeyEventUtils.isEnterKey(r)) {
					q.find("#ok").click();
					$j(document).unbind("keydown")
				}
			});
			m(q)
		});
		q.find("#ok").unbind("click").click(function () {
			h(q)
		});
		q.find("[id^=stakeEdit_]").unbind("click").click(function () {
			q.find(this).select()
		});
		q.find("[id^=stakeEdit_]").keydown(e);
		q.find("#userCoin").keydown(e)
	};

	function e(r) {
		var q = r.which;
		if (KeyEventUtils.isNumberKey(r) || KeyEventUtils.isBackspaceKey(q) || KeyEventUtils.isDeleteKey(q) || KeyEventUtils.isArrowKey(q) || KeyEventUtils.isTabKey(q) || KeyEventUtils.isEnterKey(q)) {
			return
		}
		WindowEventUtil.stopEvent(r)
	}

	function d() {
		a();
		b();
		CoinHandler.coinClean();
		CoinHandler.coinInit()
	}

	function b() {
		if (PageConfig.customizeStake == "null" || PageConfig.customizeStake.split(",").length != n) {
			for (var q = 1; q <= n; q++) {
				i.push(CoinType.getInstanceOf(PageConfig.playerCurrency, q).amounts)
			}
		} else {
			i = PageConfig.customizeStake.split(",")
		}
	}

	/*function p(q) {
		$j.ajax({
			type: "POST",
			dataType: "JSON",
			url: "/member/playerService/setCustomizeStake",
			data: {
				newCustomizeStake: q
			},
			success: function (r) {
				if (!r) {
					d();
					return
				}
				if (r.error) {
					NoticeHandler.error(r.error);
					d();
					return
				}
				if (r.result == "SUCCESS" && r.customizeStake != null) {
					PageConfig.customizeStake = r.customizeStake;
					d()
				}
			},
			error: function (r) {
				Trace.log(r);
				d()
			}
		})
	}*/

	function g() {
		var q = $j("#coinList");
		q.find("#closeSet").addClass("disable");
		q.find("#coinSave").addClass("disable")
	}

	function f() {
		var q = $j("#coinList");
		q.find("#closeSet").removeClass("disable");
		q.find("#coinSave").removeClass("disable")
	}

	function m(u) {
		var t = u.find("#stakeSet");
		var q = u.find("#editCustomizeStakeList");
		var r = q.find("[id^=stakeEdit_]");
		for (var s = 0; s < r.length; s++) {
			u.find(r[s]).val(i[s])
		}
		q.show();
		t.hide();
		g()
	}

	function h(x) {
		var u = x.find("#stakeSet");
		var r = x.find("#editCustomizeStakeList");
		var s = r.find("[id^=stakeEdit_]");
		var w = [];
		for (var t = 0; t < s.length; t++) {
			var v = x.find(s[t]).val();
			if (v.length > c) {
				NoticeHandler.error("CustomizeStake can not more than 7 bits");
				continue
			}
			if (!MathUtil.isNumeric(v, false, false) || isNaN(v)) {
				NoticeHandler.error("[ " + v + " ] is illegal");
				continue
			}
			if (v < PageConfig.minBet) {
				var q = CurrencyType.getInstanceOf(PageConfig.playerCurrency);
				NoticeHandler.error("Stake setting can not be less than MinBet " + PageConfig.minBet + " " + q.name);
				continue
			}
			w.push(v)
		}
		if (w.length != n) {
			a();
			return
		}
		w.sort(function (z, y) {
			return z > y
		});
		if (w.toString() == PageConfig.customizeStake) {
			a();
			return
		}
		p(w.toString())
	}

	function a() {
		var q = $j("#coinList");
		q.find("#editCustomizeStakeList").hide();
		q.find("#stakeSet").show();
		f()
	}
	CoinHandler.setCoinPreference = function (r, q) {
		/*$j.ajax({
			type: "POST",
			dataType: "JSON",
			url: "/member/playerService/setCoinPreference",
			data: {
				newCoinPreference: r,
				newUserCoin: q
			}
		});
		PageConfig.coinPreference = r;
		PageConfig.userCoin = q*/
	};
	CoinHandler.coinClean = function () {
		var q = JsCache.get("#coinList");
		for (var r = 1; r <= n; r++) {
			q.find("#coin_" + r).removeClass("select")
		}
		q.find("#userCoin").val("")
	};
	CoinHandler.coinInit = function () {
		o = [];
		j = 0;
		var q = JsCache.get("#coinList");
		var t = PageConfig.coinPreference.split(",");
		$j.each(t, function (u, v) {
			if (ArrayUtil.indexOf(o, parseInt(v)) == -1) {
				o.push(parseInt(v))
			}
		});
		if (PageConfig.userCoin != "" && PageConfig.userCoin != 0) {
			j = PageConfig.userCoin;
			q.find("#userCoin").val(j)
		} else {
			q.find("#userCoin").val("")
		}
		for (var s = 0; s < o.length; s++) {
			q.find("#coin_" + o[s].toString()).addClass("select")
		}
		for (var r = 1; r <= n; r++) {
			q.find("#coin_" + r).html(i[r - 1])
		}
		CoinHandler.buildStakePopupList()
	};
	CoinHandler.buildStakePopupList = function () {
		var q = $j("[id^=stakePopupList]");
		for (var r = 0; r < o.length; r++) {
			var t = i[o[r] - 1];
			var s = q.find("#selectStake_" + (r + 1));
			s.attr("stake", t).html(t)
		}
		if (o.length == 5 && j.toString() != "") {
			q.find("#selectStake_6").attr("stake", j).html(j)
		}
	};
	CoinHandler.clickStakePopupList = function (r, q, t) {
		var u = "[id=stakePopupList]";
		u += "[bet=" + r.attr("id") + "]";
		var s = q.find(u);
		if (s.length == 0) {
			s = q.find("[id=stakePopupList][stakePopupListType=template]").clone();
			s.attr("bet", r.attr("id"));
			s.attr("stakePopupListType", "extended");
			s.show();
			s.find("[id^=selectStake_]").mousedown(function () {
				if (r.find("#inputStake").hasClass("disable")) {
					return
				}
				var v = CoinHandler.clickSelectStake(r.attr("eventId"), r.attr("marketId"), r.attr("selectionId"), r.attr("sideType"), this, r);
				var w = BetHandler.calculateTotalLiability();
				JsCache.get("#betSlipFullBtn").find("#total").html(CurrencyUtil.formatter(w));
				if (v == true) {
					BetHandler.getWinLoss(r.attr("eventId"), r.attr("marketId"))
				}
				if (t) {
					TxnHandler.commonEvent(r)
				} else {
					BetHandler.oddsAndStakeCheck(r)
				}
			})
		}
		r.find("#profitLiability").after(s);
		s.fadeIn();
		BetHandler.hideStakeTipsPopup(r)
	};
	CoinHandler.showQuickBetCoin = function (q) {
		var s = $j("#betList");
		var r = q.find("[id=stakePopupList][quickBetBoard=" + q.attr("id") + "]");
		if (r.length == 0) {
			r = s.find("[id=stakePopupList][stakePopupListType=template]").clone();
			r.attr("quickBetBoard", q.attr("id"));
			r.attr("stakePopupListType", "extended");
			r.find("[id^=selectStake_]").mousedown(function () {
				var t = CoinHandler.clickSelectStake(q.attr("eventId"), q.attr("marketId"), q.attr("selectionId"), q.attr("sideType"), this, q);
				if (t == true) {
					BetHandler.getWinLoss(q.attr("eventId"), q.attr("marketId"))
				}
				if (BrowserUtil.isEdge() || BrowserUtil.isIE()) {
					BetHandler.oddsAndStakeCheck(q)
				}
			})
		}
		q.find("#placeBet").after(r);
		r.fadeIn()
	};
	CoinHandler.showFancyBetCoin = function (q) {
		var s = $j("#betList");
		var r = q.find("[id=stakePopupList][fancyBetBoard=" + q.attr("id") + "]");
		if (r.length == 0) {
			r = s.find("[id=stakePopupList][stakePopupListType=template]").clone();
			r.attr("fancyBetBoard", q.attr("id"));
			r.attr("stakePopupListType", "extended");
			r.find("[id^=selectStake_]").mousedown(function () {
				var w = $j(this);
				var u = q.find("#inputStake");
				var x = parseInt(u.val()) || 0;
				var t = parseInt(w.attr("stake"));
				u.val(t);
				var v = DataBase.fancyBet.updateStake(q.prop("eventId"), q.prop("marketId"), q.prop("sideType"), t);
				CoinHandler.hideStakePopupList();
				if (v == true) {
					FancyBetHandler.getExposure(q.prop("eventId"), q.prop("marketId"))
				}
				FancyBetHandler.eventCalculate(q, q.prop("eventId"), q.prop("marketId"), q.prop("sideType"), v)
			})
		}
		q.find("#placeBet").parent().after(r);
		r.fadeIn()
	};
	CoinHandler.showFeedingSiteBetCoin = function (q) {
		var s = $j("#betList");
		var r = q.find("[id=stakePopupList][feedingSiteBetBoard=" + q.attr("id") + "]");
		if (r.length == 0) {
			r = s.find("[id=stakePopupList][stakePopupListType=template]").clone();
			r.attr("feedingSiteBetBoard", q.attr("id"));
			r.attr("stakePopupListType", "extended");
			r.find("[id^=selectStake_]").mousedown(function () {
				var w = $j(this);
				var u = q.find("#inputStake");
				var x = parseInt(u.val()) || 0;
				var t = parseInt(w.attr("stake"));
				u.val(t);
				var v = DataBase.feedingSiteBet.updateStake(q.prop("eventId"), q.prop("marketId"), q.prop("selectionId"), t);
				CoinHandler.hideStakePopupList();
				FeedingSiteBetHandler.eventCalculate(q, q.prop("eventId"), q.prop("marketId"), q.prop("selectionId"), v)
			})
		}
		q.find("#placeBet").parent().after(r);
		r.fadeIn()
	};
	CoinHandler.showSportsBookBetCoin = function (q) {
		var s = $j("#betList");
		var r = q.find("[id=stakePopupList][sportsBookBetBoard=" + q.attr("id") + "]");
		if (r.length == 0) {
			r = s.find("[id=stakePopupList][stakePopupListType=template]").clone();
			r.attr("sportsBookBetBoard", q.attr("id"));
			r.attr("stakePopupListType", "extended");
			r.find("[id^=selectStake_]").mousedown(function () {
				var w = $j(this);
				var u = q.find("#inputStake");
				var x = parseInt(u.val()) || 0;
				var t = parseInt(w.attr("stake"));
				u.val(t);
				var v = DataBase.sportsBookBet.updateStake(q.prop("apiSiteType"), q.prop("eventId"), q.prop("marketId"), q.prop("selectionId"), t);
				CoinHandler.hideStakePopupList();
				SportsBookBetHandler.eventCalculate(q, q.prop("apiSiteType"), q.prop("eventId"), q.prop("marketId"), q.prop("selectionId"), v)
			})
		}
		q.find("#placeBet").parent().after(r);
		r.fadeIn()
	};
	CoinHandler.hideStakePopupList = function () { };
	CoinHandler.clickSelectStake = function (q, z, v, u, A, w) {
		var x = $j(A);
		var s = w.find("#inputStake");
		var r = parseInt(s.val()) || 0;
		var t = parseInt(x.attr("stake"));
		s.val(t);
		var y = DataBase.bet.updateStake(q, z, v, u, t);
		CoinHandler.hideStakePopupList();
		BetHandler.calculateProfitAndLiability();
		return y
	};
	CoinHandler.coinListClick = function (t) {
		var r = t.id;
		var s = $j(t);
		var u = parseInt(r.replace("coin_", ""));
		var q = JsCache.get("#coinList");
		if (s.hasClass("select")) {
			q.find("#" + r).removeClass("select");
			o = $j.grep(o, function (x) {
				return x != u
			})
		} else {
			var w = 6;
			if (o.length >= w) {
				var v = o[0];
				o = o.slice(1, 6);
				q.find("#coin_" + v).removeClass("select")
			}
			if (ArrayUtil.indexOf(o, u) == -1) {
				s.addClass("select");
				o.push(u);
				o.sort(function (y, x) {
					return y - x
				})
			}
		}
	};
	CoinHandler.coinSelectSubmit = function (r) {
		var t = JsCache.get("#userCoin");
		var q = 0;
		if (parseInt(o.length) == 6 || parseInt(o.length) == 5 && t.val() != 0) { } else {
			NoticeHandler.error(I18N.get("msg.member.coin.amoutsLess"));
			return
		}
		var s = t.val();
		if (s.toString() != "" && s != 0) {
			q = parseFloat(PageConfig.minBet);
			if (s < 0) {
				NoticeHandler.error("DefaultStake can not less than 0");
				return
			}
			if (s > 9999999) {
				NoticeHandler.error("DefaultStake can not more than 7 bits");
				return
			}
			if (!MathUtil.isNumeric(s, false, false) || !MathUtil.isPositive(s)) {
				NoticeHandler.error("DefaultStake is illegal");
				return
			}
			if (s > q) {
				q = s
			}
		}
		CoinHandler.setCoinPreference(o.toString(), q);
		JsCache.get("#set_pop").hide();
		d()
	};
	CoinHandler.coinSelectCancel = function (q) {
		o = k;
		j = l;
		d();
		if (q == true) {
			JsCache.get("#coinList").hide()
		}
	}
})();