if (typeof BettingHistoryUtilHandler == "undefined") {
	BettingHistoryUtilHandler = {}
} (function () {
	BettingHistoryUtilHandler.data = "";
	BettingHistoryUtilHandler.populate = function () {
		JsCache.get("#noReportMessage").hide();
		JsCache.get("#report").hide();
		if (BettingHistoryUtilHandler.data && BettingHistoryUtilHandler.data.length > 0) {
			var p = $j("#betStatus").val();
			var o = (3 == p);
			if (o) {
				$j("#userCancelTitle").show()
			} else {
				$j("#userCancelTitle").hide()
			}
			JsCache.get("#report").show();
			var s = JsCache.get("#matchTableTemplate").clone().show();
			s.attr("id", "matchTable");
			CurrencyUtil.updateSetting({
				currencySymbol: ""
			});
			for (var r = 0; r < BettingHistoryUtilHandler.data.length; r++) {
				var m = JsCache.get("#matchRowTemplate").clone().show();
				m.attr("id", "matchRow" + r);
				var c = BettingHistoryUtilHandler.data[r];
				var n = c.categoryType;
				var d = c.transactionData;
				var t = d && d.length > 0;
				m.find("#playerID").html(c.playerId);
				m.find("#betID").html(c.betID);
				if (t) {
					m.find("#betID").addClass("expand-close")
				}
				var b = "<strong>" + c.eventName + "</strong>";
				m.find("#matchTitle").append(c.eventTypeName).append('<img class="fromto" src="/images/transparent.gif">').append(b).append('<img class="fromto" src="/images/transparent.gif">').append(c.marketName);
				m.find("#matchSelection").html(c.selectionName);
				m.find("#matchType").html(c.matchType).attr("class", CategoryUtil.getFancySideTypeCss(c.matchType));
				m.find("#betPlaced").html(c.betPlaced);
				var e = CategoryUtil.getMatchOdds(n, c.fancyBetRuns, c.matchOddsReq);
				m.find("#matchOddsReq").html(e);
				m.find("#matchStake").html(CurrencyUtil.formatter(c.matchStake));
				m.find("#matchAvgOdds").html(CategoryUtil.getAverageMatchedOdds(n, c.matchAvgOdds));
				var h = m.find("#userCancel");
				if (o) {
					h.html(c.betfaircancelstatus == 1 ? "YES" : "-");
					h.show()
				} else {
					h.hide()
				}
				var k = "";
				if (isNaN(c.pol)) {
					k = c.pol
				} else {
					k = CurrencyUtil.formatter(Number(c.pol))
				}
				m.find("#pol").html(k);
				s.append(m).show();
				var l;
				if (c.reduction) {
					l = JsCache.get("#expandReductionRowTemplate").clone()
				} else {
					l = JsCache.get("#expandRowTemplate").clone()
				}
				l.attr("id", "expand" + r);
				var f = l.find("#txTableTemplate");
				f.attr("id", "txTable" + r);
				if (t) {
					m.find("#betID").attr("onclick", "BettingHistoryUtilHandler.toggleTx(" + r + ")");
					for (var q = 0; q < d.length; q++) {
						var a = f.find("#txRowTemplate").clone().show();
						a.attr("id", "txRow" + q);
						if (q % 2 == 0) {
							a.attr("class", "even")
						}
						var g = d[q];
						a.find("#betTaken").html(g.betTaken);
						a.find("#txOddsReq").html(e);
						a.find("#txStake").html(CurrencyUtil.formatter(g.txStake));
						a.find("#txLiability").html(g.txLiability ? CurrencyUtil.formatter(g.txLiability) : "-");
						a.find("#txOddsMatched").html(CategoryUtil.getMatchOdds(n, g.txFancyBetRuns, g.txOddsMatched));
						if (c.reduction) {
							a.find("#reduction").html('<span class="red">' + (CurrencyUtil.formatter(c.reduction * 100)) + "%</span>");
							a.find("#actualOdds").html(g.actualOdds)
						}
						f.append(a)
					}
				}
				s.append(l)
			}
			$j("#matchTableTemplate").after(s)
		} else {
			NoticeHandler.info(I18N.get("msg.error.info.betsNoData"));
			JsCache.get("#noReportMessage").show()
		}
	};
	BettingHistoryUtilHandler.toggleTx = function (a) {
		var b = $j("#matchRow" + a).find("#betID");
		if (b.hasClass("expand-open")) {
			b.addClass("expand-close");
			b.removeClass("expand-open")
		} else {
			if (b.hasClass("expand-close")) {
				b.addClass("expand-open");
				b.removeClass("expand-close")
			}
		}
		$j("#expand" + a).toggle()
	}
})();