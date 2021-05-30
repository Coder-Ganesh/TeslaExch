if (typeof (FunctionHandler) === "undefined") {
	FunctionHandler = {}
} (function () {
	FunctionHandler.init = function (a, b) {
		FunctionHandler.initialDate();
		$j("#startDate").change(function () {
			FunctionHandler.checkTime()
		});
		$j("#endDate").change(function () {
			FunctionHandler.checkTime()
		});
		$j("#startTime").bind("input", function () {
			this.value = DateUtil.onInputHHmm(this.value)
		});
		$j("#endTime").bind("input", function () {
			this.value = DateUtil.onInputHHmm(this.value)
		});
		$j("#startTime").change(function () {
			FunctionHandler.checkTime()
		});
		$j("#endTime").change(function () {
			FunctionHandler.checkTime()
		});
		var c = typeof (a) === "function";
		$j("#today").click(function () {
			FunctionHandler.today();
			if (c) {
				a()
			}
		});
		$j("#yesterday").click(function () {
			FunctionHandler.yesterday();
			if (c) {
				a()
			}
		});
		$j("#last7days").click(function () {
			FunctionHandler.last7days();
			if (c) {
				a()
			}
		});
		$j("#last15days").click(function () {
			FunctionHandler.last15days();
			if (c) {
				a()
			}
		});
		$j("#last30days").click(function () {
			FunctionHandler.last30days();
			if (c) {
				a()
			}
		});
		$j("#last2months").click(function () {
			FunctionHandler.last2months();
			if (c) {
				a()
			}
		});
		$j("#last3months").click(function () {
			FunctionHandler.last3months();
			if (c) {
				a()
			}
		});
		$j("#getPL").click(function () {
			if (c) {
				a()
			}
		});
		$j("#noReportMessage").html(b)
	};
	FunctionHandler.initDateEvent = function () {
		FunctionHandler.initialDate();
		$j("#startDate").change(function () {
			FunctionHandler.checkTime()
		});
		$j("#endDate").change(function () {
			FunctionHandler.checkTime()
		});
		$j("#startTime").bind("input", function () {
			this.value = DateUtil.onInputHHmm(this.value)
		});
		$j("#endTime").bind("input", function () {
			this.value = DateUtil.onInputHHmm(this.value)
		});
		$j("#startTime").change(function () {
			FunctionHandler.checkTime()
		});
		$j("#endTime").change(function () {
			FunctionHandler.checkTime()
		})
	};
	FunctionHandler.today = function () {
		JsCache.get("#startDate").val(SearchConfig.today);
		JsCache.get("#endDate").val(SearchConfig.maxDay);
		FunctionHandler.checkTime();
		FunctionHandler.initialTime()
	};
	FunctionHandler.yesterday = function () {
		JsCache.get("#startDate").val(SearchConfig.yesterday);
		JsCache.get("#endDate").val(SearchConfig.maxDay);
		FunctionHandler.initialTime()
	};
	FunctionHandler.last7days = function () {
		JsCache.get("#startDate").val(SearchConfig.last7day);
		JsCache.get("#endDate").val(SearchConfig.maxDay);
		FunctionHandler.initialTime()
	};
	FunctionHandler.last15days = function () {
		JsCache.get("#startDate").val(SearchConfig.last15day);
		JsCache.get("#endDate").val(SearchConfig.maxDay);
		FunctionHandler.initialTime()
	};
	FunctionHandler.last30days = function () {
		JsCache.get("#startDate").val(SearchConfig.last30day);
		JsCache.get("#endDate").val(SearchConfig.maxDay);
		FunctionHandler.initialTime()
	};
	FunctionHandler.last2months = function () {
		JsCache.get("#startDate").val(SearchConfig.last2months);
		JsCache.get("#endDate").val(SearchConfig.maxDay);
		FunctionHandler.initialTime()
	};
	FunctionHandler.last3months = function () {
		JsCache.get("#startDate").val(SearchConfig.last3months);
		JsCache.get("#endDate").val(SearchConfig.maxDay);
		FunctionHandler.initialTime()
	};
	FunctionHandler.initialDate = function () {
		var a = new Date(new Date(SearchConfig.minDay).getTime() - 24 * 60 * 60 * 1000);
		var b = new Date(SearchConfig.maxDay);
		calendarProperties.limitedBeginDate = a;
		calendarProperties.limitedEndDate = b;
		calendarProperties.showCurrentDate = b
	};
	FunctionHandler.updateLimitBeginDate = function (a) {
		var b = new Date(new Date(a).getTime() - 24 * 60 * 60 * 1000);
		calendarProperties.limitedBeginDate = b
	};
	FunctionHandler.initialTime = function () {
		if (!SearchConfig.enableTimeSetting) {
			return
		}
		JsCache.get("#startTime").val("");
		JsCache.get("#endTime").val("")
	};
	FunctionHandler.initStartDate = function (a, b) {
		$j("#startDate").val(a);
		$j("#startTime").val(b);
		FunctionHandler.checkTime()
	};
	FunctionHandler.initEndDate = function (a, b) {
		$j("#endDate").val(a);
		$j("#endTime").val(b);
		FunctionHandler.checkTime()
	};
	FunctionHandler.checkTime = function () {
		var b = JsCache.get("#startDate").val();
		var e = JsCache.get("#endDate").val();
		var a = JsCache.get("#startTime").val();
		var f = JsCache.get("#endTime").val();
		var c = b.split("-");
		var h = a.split(":");
		var i = e.split("-");
		var g = f.split(":");
		if (DateUtil.compare(new Date(b), new Date(SearchConfig.minDay)) === -1) {
			JsCache.get("#startDate").val(SearchConfig.minDay);
			return
		}
		if (DateUtil.compare(new Date(b), new Date(SearchConfig.today)) === 1) {
			JsCache.get("#startDate").val(SearchConfig.today);
			return
		}
		var d = new Date(SearchConfig.maxDay);
		if (DateUtil.compare(new Date(e), new Date(SearchConfig.minDay)) === -1 || DateUtil.compare(new Date(e), d) === 1) {
			JsCache.get("#endDate").val(SearchConfig.today);
			return
		}
		if (DateUtil.compare(new Date(b), new Date(e)) === 1) {
			JsCache.get("#startDate").val(e);
			JsCache.get("#endDate").val(b);
			return
		}
	};
	FunctionHandler.checkEndTime = function () {
		var e = JsCache.get("#endDate").val();
		var c = JsCache.get("#endTime").val();
		var b = e.split("-");
		var a = c.split(":");
		var d = new Date(SearchConfig.maxDay);
		if (DateUtil.compare(new Date(e), new Date(SearchConfig.minDay)) === -1 || DateUtil.compare(new Date(e), d) === 1) {
			JsCache.get("#endDate").val(SearchConfig.today);
			return
		}
	};
	FunctionHandler.getStartDate = function () {
		var a = $j("#startDate").val().trim();
		if (ValidateDataUtil.isEmpty(a)) {
			throw new NotValidException(I18N.get("msg.error.validation.startDateIsEmpty"))
		}
		var b = $j("#startTime").val();
		if (b !== "") {
			a += (" " + b)
		} else {
			a += " 09:00"
		}
		return a
	};
	FunctionHandler.getEndDate = function () {
		var b = $j("#endDate").val().trim();
		if (ValidateDataUtil.isEmpty(b)) {
			throw new NotValidException(I18N.get("msg.error.validation.endDateIsEmpty"))
		}
		var a = $j("#endTime").val();
		if (a !== "") {
			b += (" " + a)
		} else {
			b += " 08:59"
		}
		return b
	};
	FunctionHandler.getSportType = function () {
		return $j("#func_sports").val()
	}
})();