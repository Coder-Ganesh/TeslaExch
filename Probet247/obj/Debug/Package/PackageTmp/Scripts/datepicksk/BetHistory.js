if (typeof BetHistoryHandler == "undefined") {
	BetHistoryHandler = {}
} (function () {
	BetHistoryHandler.init = function () {
		$j("#getPL").text(I18N.get("form.text.function.getHistory"));
		$j("#betStatus").append($j("<option></option>").attr("value", 2).text(I18N.get("form.text.function.settled"))).append($j("<option></option>").attr("value", 3).text(I18N.get("form.text.function.cancelled"))).append($j("<option></option>").attr("value", 4).text(I18N.get("form.text.function.voided")));
		$j("#statusCondition").show();
		$j("#reportType_exchange").click(function () {
			$j("#reportType_exchange").addClass("select");
			$j("#reportType_sportsBook").removeClass("select")
		});
		$j("#reportType_sportsBook").click(function () {
			$j("#reportType_sportsBook").addClass("select");
			$j("#reportType_exchange").removeClass("select")
		});
		FunctionHandler.init(BetHistoryHandler.queryReport, I18N.get("form.text.bettingHistory.noReportMessage"))
	};
	BetHistoryHandler.queryReport = function () {
		BetHistoryHandler.list(1)
	};
	BetHistoryHandler.list = function (c) {
		try {
			var a = FunctionHandler.getStartDate();
			var h = FunctionHandler.getEndDate();
			var g = JsCache.get("#betStatus")[0];
			if (g.options.length == 0 || g.selectedIndex == -1 || ValidateDataUtil.isEmpty(g.options[g.selectedIndex].value)) {
				throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", "betStatus"))
			}
			var b = $j(".report-tab.select").data("reporttabtype");
			var d = JsCache.get("#betStatus").val();
			/*$j.ajax({
				type: "POST",
				url: "/member/reportController/queryBetHistory",
				data: {
					userID: PageConfig.userName,
					site: PageConfig.site,
					startDate: a,
					endDate: h,
					betStatus: d,
					reportTabType: b,
					pageNumber: c
				},
				beforeSend: function () {
					if ($j("#matchTable").length) {
						$j("#matchTable").remove()
					}
					JsCache.get("#loading").show()
				},
				complete: function () {
					JsCache.get("#loading").hide();
					BettingHistoryUtilHandler.populate();
					FunctionHandler.initialDate()
				},
				success: function (j) {
					if (j == null) {
						BettingHistoryUtilHandler.data = "";
						return
					}
					if (j.error) {
						NoticeHandler.error(j.error)
					} else {
						var e = j.totalPage;
						var i = j.currentPage;
						PageHandler.init(i, e, function (k) {
							BetHistoryHandler.list(k)
						});
						PageHandler.pageList();
						BettingHistoryUtilHandler.data = j.resultList
					}
				}
			})*/
		} catch (f) {
			//NoticeHandler.error(f.message)
		}
	}
})();