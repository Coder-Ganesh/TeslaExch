if (typeof (CurrencyType) == "undefined") {
	CurrencyType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	CurrencyType.HKD = {
		value: 1,
		editBetMin: 1,
		symbol: "$",
		getChipSetting: function () {
			return CoinType.chipSetting3
		},
		stakeMin: 1
	};
	CurrencyType.INR = {
		value: 2,
		editBetMin: 10,
		symbol: "&#8377;",
		getChipSetting: function () {
			return CoinType.chipSetting5
		},
		stakeMin: 1
	};
	CurrencyType.EUR = {
		value: 3,
		editBetMin: 1,
		symbol: "&#8364;",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.U = {
		value: 4,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.THB = {
		value: 5,
		editBetMin: 5,
		symbol: "&#3647;",
		getChipSetting: function () {
			return CoinType.chipSetting4
		},
		stakeMin: 1
	};
	CurrencyType.AED = {
		value: 6,
		editBetMin: 5,
		symbol: "&#1583;.&#1573;",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.PHP = {
		value: 7,
		editBetMin: 10,
		symbol: "&#8369;",
		getChipSetting: function () {
			return CoinType.chipSetting4
		},
		stakeMin: 1
	};
	CurrencyType.GBP = {
		value: 8,
		editBetMin: 1,
		symbol: "&#163;",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.RMB = {
		value: 9,
		editBetMin: 1,
		symbol: "&#165;",
		getChipSetting: function () {
			return CoinType.chipSetting3
		},
		stakeMin: 1
	};
	CurrencyType.PTS = {
		value: 10,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.KRW = {
		value: 11,
		editBetMin: 100,
		symbol: "&#8361;",
		getChipSetting: function () {
			return CoinType.chipSetting6
		},
		stakeMin: 1
	};
	CurrencyType.IDR = {
		value: 12,
		editBetMin: 1000,
		symbol: "Rp",
		getChipSetting: function () {
			return CoinType.chipSetting7
		},
		stakeMin: 1
	};
	CurrencyType.PTU = {
		value: 13,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.PTE = {
		value: 14,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.PTG = {
		value: 15,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.USD = {
		value: 16,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.PTH = {
		value: 16,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.PTI = {
		value: 17,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.PUI = {
		value: 18,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.PTR = {
		value: 19,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting3
		},
		stakeMin: 1
	};
	CurrencyType.PTA = {
		value: 20,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.PTM = {
		value: 21,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.P = {
		value: 22,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.PTD = {
		value: 23,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.IR = {
		value: 24,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting8
		},
		stakeMin: 100
	};
	CurrencyType.PTAD = {
		value: 25,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.PBU = {
		value: 26,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting1
		},
		stakeMin: 1
	};
	CurrencyType.VND = {
		value: 27,
		editBetMin: 5,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting4
		},
		stakeMin: 5
	};
	CurrencyType.EHK = {
		value: 28,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting3
		},
		stakeMin: 1
	};
	CurrencyType.PKR = {
		value: 29,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting9
		},
		stakeMin: 100
	};
	CurrencyType.PKU = {
		value: 30,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting2
		},
		stakeMin: 1
	};
	CurrencyType.PR = {
		value: 31,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting10
		},
		stakeMin: 100
	};
	CurrencyType.PIN = {
		value: 32,
		editBetMin: 1,
		symbol: " ",
		getChipSetting: function () {
			return CoinType.chipSetting8
		},
		stakeMin: 100
	};
	(function () {
		for (atr in CurrencyType) {
			var c = CurrencyType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	CurrencyType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (UserType) == "undefined") {
	UserType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	UserType.COMPANY = {
		value: 6,
		treeHeight: 0,
		shortCode: "COM",
		cssName: "lv_0"
	};
	UserType.SHAREHOLDER = {
		value: 5,
		treeHeight: 1,
		shortCode: "SS",
		cssName: "lv_1"
	};
	UserType.SENIOR_MASTER_AGENT = {
		value: 4,
		treeHeight: 2,
		shortCode: "SUP",
		cssName: "lv_2"
	};
	UserType.MASTER_AGENT = {
		value: 3,
		treeHeight: 3,
		shortCode: "MA",
		cssName: "lv_3"
	};
	UserType.PLAYER = {
		value: 0,
		treeHeight: 6,
		shortCode: "PL",
		cssName: "lv_4"
	};
	UserType.SUBACCOUNT = {
		value: -1,
		treeHeight: -1,
		shortCode: "SUB",
		cssName: ""
	};
	(function () {
		for (atr in UserType) {
			var c = UserType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	UserType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	UserType.getByTreeHeight = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["treeHeight"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (ManagerLockType) == "undefined") {
	ManagerLockType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	ManagerLockType.UNLOCK = {
		value: 0
	};
	ManagerLockType.LOCK = {
		value: 1
	};
	(function () {
		for (atr in ManagerLockType) {
			var c = ManagerLockType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ManagerLockType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (ManagerStatusType) == "undefined") {
	ManagerStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	ManagerStatusType.CLOSE = {
		value: 0
	};
	ManagerStatusType.ACTIVE = {
		value: 1
	};
	(function () {
		for (atr in ManagerStatusType) {
			var c = ManagerStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ManagerStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (WebSiteType) == "undefined") {
	WebSiteType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	WebSiteType.WICKETS = {
		value: 0,
		shortCode: "9W"
	};
	WebSiteType.SKYEXCHANGE = {
		value: 1,
		shortCode: "Sky"
	};
	WebSiteType.LIVESPORTS = {
		value: 2,
		shortCode: "Demo"
	};
	WebSiteType.MYSPORTS247 = {
		value: 3,
		shortCode: "My247"
	};
	WebSiteType.MAZAPLAY = {
		value: 4,
		shortCode: "Maza"
	};
	WebSiteType.OW = {
		value: 5,
		shortCode: "Ow"
	};
	WebSiteType.SKYINPLAY = {
		value: 6,
		shortCode: "SkyInPlay"
	};
	WebSiteType.MATADOR = {
		value: 7,
		shortCode: "Matador"
	};
	WebSiteType.FAIRENTER = {
		value: 8,
		shortCode: "FairEnter"
	};
	WebSiteType.BIGEXCH = {
		value: 9,
		shortCode: "BigExch"
	};
	WebSiteType.MASTEREXCHANGE = {
		value: 10,
		shortCode: "MasterExchange"
	};
	WebSiteType.LUCKY7 = {
		value: 11,
		shortCode: "Lucky7"
	};
	WebSiteType.QEXCH = {
		value: 12,
		shortCode: "QExch"
	};
	WebSiteType.BETMYGAME = {
		value: 13,
		shortCode: "BetMyGame"
	}, WebSiteType.FANCYFAIR = {
		value: 14,
		shortCode: "FancyFair"
	};
	WebSiteType.MARUTI = {
		value: 15,
		shortCode: "Maruti"
	};
	WebSiteType.MARUTIBOOK = {
		value: 16,
		shortCode: "MarutiBook"
	};
	WebSiteType.SKYEXCHANGE247 = {
		value: 17,
		shortCode: "SkyExchange247"
	};
	WebSiteType.OCEANEXCH1 = {
		value: 18,
		shortCode: "OceanExch1"
	};
	WebSiteType.OCEANBOOK1 = {
		value: 19,
		shortCode: "OceanBook1"
	};
	WebSiteType.WICKETSLIVE = {
		value: 20,
		shortCode: "9WicketsLive"
	};
	WebSiteType.BETBARTER = {
		value: 21,
		shortCode: "BetBarter"
	};
	WebSiteType.PROBETX = {
		value: 22,
		shortCode: "ProBetX"
	};
	WebSiteType.ALPHAEXCH = {
		value: 23,
		shortCode: "AlphaExch"
	};
	WebSiteType.ALPHAEXCHLIVE = {
		value: 24,
		shortCode: "AlphaExchLive"
	};
	WebSiteType.CFTBET365 = {
		value: 25,
		shortCode: "CftBet365"
	};
	WebSiteType.MAXEXCH9 = {
		value: 26,
		shortCode: "MaxExch9"
	};
	WebSiteType.CRICKZOOM = {
		value: 27,
		shortCode: "CrickZoom"
	};
	WebSiteType.CRICKZOOMLIVE = {
		value: 28,
		shortCode: "CrickZoomLive"
	};
	WebSiteType.MAXINPLAY = {
		value: 29,
		shortCode: "MaxInPlay"
	};
	WebSiteType.PROBET247 = {
		value: 30,
		shortCode: "ProBet247"
	};
	WebSiteType.GOEXCHANGE = {
		value: 31,
		shortCode: "GOEXCHANGE"
	};
	WebSiteType.KINGFAIR24 = {
		value: 32,
		shortCode: "KINGFAIR24"
	};
	WebSiteType.SGEXCH247 = {
		value: 33,
		shortCode: "SGEXCH247"
	};
	WebSiteType.SIXBALL = {
		value: 34,
		shortCode: "6BALL"
	};
	WebSiteType.SIXBALLIO = {
		value: 35,
		shortCode: "6BALLIO"
	};
	WebSiteType.TRIPLE9 = {
		value: 36,
		shortCode: "999EXCHANGE"
	};
	(function () {
		for (atr in WebSiteType) {
			var c = WebSiteType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a;
			c.isSkyInplayGroup = function () {
				return (this.value == WebSiteType.SKYINPLAY.value || this.value == WebSiteType.BIGEXCH.value || this.value == WebSiteType.MASTEREXCHANGE.value || this.value == WebSiteType.QEXCH.value || this.value == WebSiteType.MARUTIBOOK.value || this.value == WebSiteType.OCEANBOOK1.value || this.value == WebSiteType.PROBETX.value || this.value == WebSiteType.ALPHAEXCHLIVE.value || this.value == WebSiteType.CFTBET365.value || this.value == WebSiteType.CRICKZOOMLIVE.value || this.value == WebSiteType.MAXINPLAY.value || this.value == WebSiteType.BETBARTER.value || this.value == WebSiteType.SIXBALLIO.value)
			};
			c.isFancyFairGroup = function () {
				return (this.value == WebSiteType.FANCYFAIR.value)
			};
			c.isBookModeGroup = function () {
				return this.isSkyInplayGroup() || this.isFancyFairGroup()
			}
		}
	})();
	WebSiteType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (VendorSiteType) == "undefined") {
	VendorSiteType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	VendorSiteType.AWC = {
		value: 1
	};
	(function () {
		for (atr in VendorSiteType) {
			var c = VendorSiteType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	VendorSiteType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (VendorCategoryType) == "undefined") {
	VendorCategoryType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	VendorCategoryType.LIVE = {
		name: "LIVE",
		categoryType: 1
	};
	VendorCategoryType.RNG = {
		name: "RNG",
		categoryType: 2
	};
	VendorCategoryType.SLOT = {
		name: "SLOT",
		categoryType: 3
	};
	(function () {
		for (atr in VendorCategoryType) {
			var c = VendorCategoryType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	VendorCategoryType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["categoryType"] == d) {
				return b[c]
			}
		}
		return null
	};
	VendorCategoryType.getInstanceByName = function (c) {
		for (var d = 0; d < b.length; d++) {
			if (b[d]["name"] == c) {
				return b[d]
			}
		}
		return null
	};
	VendorCategoryType.values = function () {
		return b
	}
})();
if (typeof (ApiFancyBetSiteType) == "undefined") {
	ApiFancyBetSiteType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	ApiFancyBetSiteType.FANCYBET = {
		value: 0,
		shortCode: "FANCYBET"
	};
	(function () {
		for (atr in ApiFancyBetSiteType) {
			var c = ApiFancyBetSiteType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ApiFancyBetSiteType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (ApiStreamingSiteType) == "undefined") {
	ApiStreamingSiteType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	ApiStreamingSiteType.DEMO = {
		value: 0,
		shortCode: "DEMO"
	};
	ApiStreamingSiteType.GLIVES = {
		value: 1,
		shortCode: "GLIVES"
	};
	ApiStreamingSiteType.SPORTRADAR = {
		value: 2,
		shortCode: "SPORTRADAR"
	};
	(function () {
		for (atr in ApiStreamingSiteType) {
			var c = ApiStreamingSiteType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ApiStreamingSiteType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (MyTransactionStatusType) == "undefined") {
	MyTransactionStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	MyTransactionStatusType.Active = {
		value: 1
	};
	MyTransactionStatusType.Void = {
		value: 2
	};
	MyTransactionStatusType.OPCancel = {
		value: 4
	};
	MyTransactionStatusType.Settle = {
		value: 8
	};
	(function () {
		for (atr in MyTransactionStatusType) {
			var c = MyTransactionStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	MyTransactionStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	MyTransactionStatusType.getName = function (c) {
		if ((c & MyTransactionStatusType.Active.unique()) > 0) {
			return MyTransactionStatusType.Active.name
		}
		if ((c & MyTransactionStatusType.Void.unique()) > 0) {
			return MyTransactionStatusType.Void.name
		}
		if ((c & MyTransactionStatusType.OPCancel.unique()) > 0) {
			return MyTransactionStatusType.OPCancel.name
		}
		if ((c & MyTransactionStatusType.Settle.unique()) > 0) {
			return MyTransactionStatusType.Settle.name
		}
		return ""
	}
})();
if (typeof (LanguageType) == "undefined") {
	LanguageType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	LanguageType.ENGLISH = {
		value: 1,
		resourceKey: "en"
	};
	LanguageType.CHINESE = {
		value: 2,
		resourceKey: "cn"
	};
	LanguageType.VIETNAMESE = {
		value: 3,
		resourceKey: "vi"
	};
	LanguageType.JAPANESE = {
		value: 4,
		resourceKey: "jp"
	};
	(function () {
		for (atr in LanguageType) {
			var c = LanguageType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	LanguageType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	LanguageType.values = function () {
		return b
	}
})();
if (typeof (EventStatusType) == "undefined") {
	EventStatusType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	EventStatusType.READY = {
		value: 1
	};
	EventStatusType.CANCEL = {
		value: 2
	};
	EventStatusType.OPEN = {
		value: 4
	};
	EventStatusType.SUSPEND = {
		value: 8
	};
	EventStatusType.CLOSE = {
		value: 16
	};
	EventStatusType.END = {
		value: 32
	};
	EventStatusType.UNSETTLED = {
		value: 64
	};
	EventStatusType.SETTLED = {
		value: 128
	};
	EventStatusType.VOIDED = {
		value: 256
	};
	(function () {
		for (atr in EventStatusType) {
			var c = EventStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})()
})();
if (typeof (TransactionResultType) == "undefined") {
	TransactionResultType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	TransactionResultType.LOSE = {
		value: -1,
		I18NKey: "form.text.lose",
		className: "tdLose"
	};
	TransactionResultType.DRAW = {
		value: 0,
		I18NKey: "form.text.draw",
		className: "tdDraw"
	};
	TransactionResultType.WIN = {
		value: 1,
		I18NKey: "form.text.win",
		className: "tdWin"
	};
	(function () {
		for (atr in TransactionResultType) {
			var c = TransactionResultType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	TransactionResultType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (CoinType) == "undefined") {
	CoinType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	CoinType.One = {
		value: 1,
		amounts: "10",
		display: "10"
	};
	CoinType.Two = {
		value: 2,
		amounts: "30",
		display: "30"
	};
	CoinType.Three = {
		value: 3,
		amounts: "50",
		display: "50"
	};
	CoinType.Four = {
		value: 4,
		amounts: "100",
		display: "100"
	};
	CoinType.Five = {
		value: 5,
		amounts: "200",
		display: "200"
	};
	CoinType.Six = {
		value: 6,
		amounts: "300",
		display: "300"
	};
	CoinType.Seven = {
		value: 7,
		amounts: "500",
		display: "500"
	};
	CoinType.Eight = {
		value: 8,
		amounts: "1000",
		display: "1000"
	};
	CoinType.Nine = {
		value: 9,
		amounts: "3000",
		display: "3000"
	};
	CoinType.TEN = {
		value: 10,
		amounts: "5000",
		display: "5000"
	};
	CoinType.ELEVEN = {
		value: 11,
		amounts: "10000",
		display: "10000"
	};
	CoinType.TWELVE = {
		value: 12,
		amounts: "50000",
		display: "50000"
	};
	CoinType.chipSetting1 = [
		[10, "10"],
		[30, "30"],
		[50, "50"],
		[100, "100"],
		[200, "200"],
		[300, "300"],
		[500, "500"],
		[1000, "1K"],
		[3000, "3K"],
		[5000, "5K"],
		[10000, "10K"],
		[50000, "50K"]
	];
	CoinType.chipSetting2 = [
		[50, "50"],
		[100, "100"],
		[200, "200"],
		[300, "300"],
		[500, "500"],
		[1000, "1K"],
		[3000, "3K"],
		[5000, "5K"],
		[10000, "10K"],
		[50000, "50K"],
		[100000, "100K"],
		[200000, "200K"]
	];
	CoinType.chipSetting3 = [
		[100, "100"],
		[200, "200"],
		[300, "300"],
		[500, "500"],
		[1000, "1K"],
		[3000, "3K"],
		[5000, "5K"],
		[10000, "10K"],
		[50000, "50K"],
		[100000, "100K"],
		[200000, "200K"],
		[300000, "300K"]
	];
	CoinType.chipSetting4 = [
		[500, "500"],
		[1000, "1K"],
		[3000, "3K"],
		[5000, "5K"],
		[10000, "10K"],
		[50000, "50K"],
		[100000, "100K"],
		[200000, "200K"],
		[300000, "300K"],
		[500000, "500K"],
		[1000000, "1000K"],
		[5000000, "5000K"]
	];
	CoinType.chipSetting5 = [
		[1000, "1K"],
		[3000, "3K"],
		[5000, "5K"],
		[10000, "10K"],
		[50000, "50K"],
		[100000, "100K"],
		[200000, "200K"],
		[300000, "300K"],
		[500000, "500K"],
		[1000000, "1000K"],
		[5000000, "5000K"],
		[10000000, "10000K"]
	];
	CoinType.chipSetting6 = [
		[10000, "10K"],
		[15000, "15K"],
		[20000, "20K"],
		[25000, "25K"],
		[50000, "50K"],
		[100000, "100K"],
		[150000, "150K"],
		[200000, "200K"],
		[250000, "250K"],
		[300000, "300K"],
		[500000, "500K"],
		[1000000, "1000K"]
	];
	CoinType.chipSetting7 = [
		[15000, "15K"],
		[20000, "20K"],
		[25000, "25K"],
		[50000, "50K"],
		[100000, "100K"],
		[150000, "150K"],
		[200000, "200K"],
		[250000, "250K"],
		[300000, "300K"],
		[500000, "500K"],
		[1000000, "1000K"],
		[5000000, "5000K"]
	];
	CoinType.chipSetting8 = [
		[1000, "1000"],
		[10000, "10K"],
		[30000, "30K"],
		[50000, "50K"],
		[100000, "100K"],
		[150000, "150K"],
		[300000, "300K"],
		[500000, "500K"]
	];
	CoinType.chipSetting9 = [
		[5500, "5500"],
		[10000, "10K"],
		[15000, "15K"],
		[20000, "20K"],
		[25000, "25K"],
		[30000, "30K"],
		[50000, "50K"],
		[100000, "100K"]
	];
	CoinType.chipSetting10 = [
		[1000, "1k"],
		[3000, "3K"],
		[5000, "5K"],
		[10000, "10K"],
		[15000, "15K"],
		[20000, "20K"],
		[25000, "25K"],
		[50000, "50K"]
	];
	CoinType.chipSetting11 = [
		[40, "40"],
		[200, "200"],
		[300, "300"],
		[500, "500"],
		[1000, "1K"],
		[3000, "3K"],
		[5000, "5K"],
		[10000, "10K"]
	];
	(function () {
		for (atr in CoinType) {
			var c = CoinType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	CoinType.getInstanceOf = function (d, h) {
		var c = CurrencyType.getInstanceOf(d);
		var g = c.getChipSetting();
		for (var e = 0; e < b.length; e++) {
			if (b[e]["value"] == h) {
				var f = g[h - 1];
				b[e]["amounts"] = f[0];
				b[e]["display"] = f[1];
				return b[e]
			}
		}
	}
})();
if (typeof (SideType) == "undefined") {
	SideType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	SideType.Back = {
		name: "Back",
		lineName: "Buy",
		value: 1
	};
	SideType.Lay = {
		name: "Lay",
		lineName: "Sell",
		value: 2
	};
	(function () {
		for (atr in SideType) {
			var c = SideType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	SideType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	SideType.getInstanceByName = function (c) {
		for (var d = 0; d < b.length; d++) {
			if (b[d]["name"] == c) {
				return b[d]
			}
		}
		return null
	}
})();
if (typeof (UnMatchTicketStatusType) == "undefined") {
	UnMatchTicketStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	UnMatchTicketStatusType.Ready = {
		value: 0
	};
	UnMatchTicketStatusType.Processing = {
		value: 1
	};
	UnMatchTicketStatusType.Active = {
		value: 2
	};
	UnMatchTicketStatusType.Lapsed = {
		value: 3
	};
	UnMatchTicketStatusType.Delete = {
		value: 9
	};
	(function () {
		for (atr in UnMatchTicketStatusType) {
			var c = UnMatchTicketStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	UnMatchTicketStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	UnMatchTicketStatusType.getName = function (c) {
		if (c == UnMatchTicketStatusType.Ready.unique()) {
			return UnMatchTicketStatusType.Ready.name
		}
		if (c == UnMatchTicketStatusType.Processing.unique()) {
			return UnMatchTicketStatusType.Processing.name
		}
		if (c == UnMatchTicketStatusType.Active.unique()) {
			return UnMatchTicketStatusType.Active.name
		}
		if (c == UnMatchTicketStatusType.Lapsed.unique()) {
			return UnMatchTicketStatusType.Lapsed.name
		}
		if (c == UnMatchTicketStatusType.Delete.unique()) {
			return UnMatchTicketStatusType.Delete.name
		}
		return ""
	}
})();
if (typeof (PersistenceType) == "undefined") {
	PersistenceType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	PersistenceType.Lapse = {
		name: "Lapse",
		value: 0
	};
	PersistenceType.Persist = {
		name: "Persist",
		value: 1
	};
	PersistenceType.MarketOnClose = {
		name: "MarketOnClose",
		value: 2
	};
	(function () {
		for (atr in PersistenceType) {
			var c = PersistenceType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	PersistenceType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	PersistenceType.getInstanceByName = function (c) {
		for (var d = 0; d < b.length; d++) {
			if (b[d]["name"] == c) {
				return b[d]
			}
		}
		return null
	}
})();
if (typeof (EventType) == "undefined") {
	EventType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	EventType.SOCCER = {
		name: "SOCCER",
		value: 1,
		images: "/images/kv/KV02.jpg"
	};
	EventType.TENNIS = {
		name: "TENNIS",
		value: 2,
		images: "/images/kv/KV03.jpg"
	};
	EventType.CRICKET = {
		name: "CRICKET",
		value: 4,
		images: "/images/kv/KV01.jpg"
	};
	EventType.RUGBY_UNION = {
		name: "RUGBY_UNION",
		value: 5,
		images: "/images/kv/KV08.jpg"
	};
	EventType.BOXING = {
		name: "BOXING",
		value: 6,
		images: "/images/kv/KV.jpg"
	};
	EventType.HORSE_RACING = {
		name: "HORSE_RACING",
		value: 7,
		images: "/images/kv/KV05.jpg"
	};
	EventType.MOTOR_SPORT = {
		name: "MOTOR_SPORT",
		value: 8,
		images: "/images/kv/KV.jpg"
	};
	EventType.CYCLING = {
		name: "CYCLING",
		value: 11,
		images: "/images/kv/KV.jpg"
	};
	EventType.RUGBY_LEAGUE = {
		name: "RUGBY_LEAGUE",
		value: 1477,
		images: "/images/kv/KV.jpg"
	};
	EventType.DARTS = {
		name: "DARTS",
		value: 3503,
		images: "/images/kv/KV.jpg"
	};
	EventType.ATHLETICS = {
		name: "ATHLETICS",
		value: 3988,
		images: "/images/kv/KV.jpg"
	};
	EventType.GREYHOUND_RACING = {
		name: "GREYHOUND_RACING",
		value: 4339,
		images: "/images/kv/KV06.jpg"
	};
	EventType.FINANCIAL_BETS = {
		name: "FINANCIAL_BETS",
		value: 6231,
		images: "/images/kv/KV.jpg"
	};
	EventType.SNOOKER = {
		name: "SNOOKER",
		value: 6422,
		images: "/images/kv/KV.jpg"
	};
	EventType.AMERICAN_FOOTBALL = {
		name: "AMERICAN_FOOTBALL",
		value: 6423,
		images: "/images/kv/KV07.jpg"
	};
	EventType.BASEBALL = {
		name: "BASEBALL",
		value: 7511,
		images: "/images/kv/KV.jpg"
	};
	EventType.BASKETBALL = {
		name: "BASKETBALL",
		value: 7522,
		images: "/images/kv/KV04.jpg"
	};
	EventType.ICE_HOCKEY = {
		name: "ICE_HOCKEY",
		value: 7524,
		images: "/images/kv/KV.jpg"
	};
	EventType.AUSTRALIAN_RULES = {
		name: "AUSTRALIAN_RULES",
		value: 61420,
		images: "/images/kv/KV.jpg"
	};
	EventType.CHESS = {
		name: "CHESS",
		value: 136332,
		images: "/images/kv/KV.jpg"
	};
	EventType.POKER = {
		name: "POKER",
		value: 315220,
		images: "/images/kv/KV.jpg"
	};
	EventType.NETBALL = {
		name: "NETBALL",
		value: 606611,
		images: "/images/kv/KV.jpg"
	};
	EventType.GAELIC_GAMES = {
		name: "GAELIC_GAMES",
		value: 2152880,
		images: "/images/kv/KV.jpg"
	};
	EventType.MIXED_MARTIAL_ARTS = {
		name: "MIXED_MARTIAL_ARTS",
		value: 26420387,
		images: "/images/kv/KV.jpg"
	};
	EventType.ESPORTS = {
		name: "ESPORTS",
		value: 27454571,
		images: "/images/kv/KV.jpg"
	};
	EventType.ELECTION = {
		name: "ELECTION",
		value: 2378962,
		images: "/images/kv/KV.jpg"
	};
	EventType.FANCYBET = {
		name: "FANCYBET",
		value: 9999999,
		images: "/images/kv/KV.jpg"
	};
	EventType.SOCCER.popularTabType = [{
		name: "popular",
		marketTypes: ["MATCH_ODDS", "BOTH_TEAMS_TO_SCORE", "HALF_TIME", "FIRST_HALF_GOALS", "OVER_UNDER", "HALF_TIME_FULL_TIME", "CORRECT_SCORE", "DRAW_NO_BET", "DOUBLE_CHANCE"]
	}, {
		name: "goals",
		marketTypes: ["OVER_UNDER"]
	}, {
		name: "handicap",
		marketTypes: ["TEAM_A", "TEAM_B"]
	}, {
		name: "half_time",
		marketTypes: ["HALF_TIME_SCORE", "HALF_TIME", "FIRST_HALF_GOALS"]
	}, {
		name: "other_markets",
		marketTypes: ["ALL"]
	}];
	EventType.TENNIS.popularTabType = [{
		name: "popular",
		marketTypes: ["MATCH_ODDS", "SET_BETTING", "SET_WINNER"]
	}, {
		name: "set_markets",
		marketTypes: ["SET_BETTING", "SET_WINNER"]
	}, {
		name: "other_markets",
		marketTypes: ["ALL"]
	}];
	EventType.CRICKET.popularTabType = [{
		name: "line",
		marketTypes: ["LINE_INNINGS_RUNS"]
	}, {
		name: "popular",
		marketTypes: ["MATCH_ODDS", "INNINGS_RUNS", "COMPLETED_MATCH", "TIED_MATCH", "SUPER_OVER"]
	}, {
		name: "over_markets",
		marketTypes: ["FIRST_OVER_RUNS"]
	}, {
		name: "innings_markets",
		marketTypes: ["INNINGS_RUNS", "OPENING_PARTNERSHIP", "1ST_DISMISSAL_METHOD", "CENTURY_SCORED"]
	}, {
		name: "batsman_markets",
		marketTypes: ["TOP_BATSMAN"]
	}, {
		name: "over_runs_betting",
		marketTypes: ["OVER_BY_OVER"]
	}, {
		name: "other_markets",
		marketTypes: ["ALL"]
	}];
	EventType.RUGBY_UNION.popularTabType = [{
		name: "popular",
		marketTypes: ["MATCH_ODDS", "WINNING_MARGIN", "DRAW_NO_BET"]
	}, {
		name: "half_time_markets",
		marketTypes: ["HALF_TIME"]
	}, {
		name: "other_markets",
		marketTypes: ["ALL"]
	}];
	EventType.AMERICAN_FOOTBALL.popularTabType = [{
		name: "popular",
		marketTypes: ["MATCH_ODDS"]
	}, {
		name: "1st_quarter_markets",
		marketTypes: ["QUARTER_MATCH_ODDS", "FIRST_SCORING_PLAY"]
	}, {
		name: "1st_half_markets",
		marketTypes: ["HALF_MATCH_ODDS"]
	}, {
		name: "player_markets",
		marketTypes: ["1ST_TOUCHDOWN_SCORER", "TO_SCORE_A_TOUCHDOWN"]
	}, {
		name: "spread_markets",
		marketTypes: ["WINNING_MARGIN"]
	}, {
		name: "other_markets",
		marketTypes: ["ALL"]
	}];
	EventType.BASKETBALL.popularTabType = [{
		name: "other_markets",
		marketTypes: ["ALL"]
	}];
	(function () {
		for (atr in EventType) {
			var c = EventType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a;
			c.isRacingEvent = function () {
				return (this.value == EventType.HORSE_RACING.value || this.value == EventType.GREYHOUND_RACING.value)
			};
			c.isTwoTeamsEvent = function () {
				if (this.value == EventType.SOCCER.value || this.value == EventType.TENNIS.value || this.value == EventType.CRICKET.value || this.value == EventType.RUGBY_UNION.value || this.value == EventType.RUGBY_LEAGUE.value || this.value == EventType.DARTS.value || this.value == EventType.SNOOKER.value || this.value == EventType.AMERICAN_FOOTBALL.value || this.value == EventType.BASEBALL.value || this.value == EventType.BASKETBALL.value || this.value == EventType.ICE_HOCKEY.value || this.value == EventType.AUSTRALIAN_RULES.value) {
					return true
				}
				return false
			};
			c.hasDraw = function () {
				if (this.value == EventType.TENNIS.value || this.value == EventType.DARTS.value || this.value == EventType.SNOOKER.value || this.value == EventType.AMERICAN_FOOTBALL.value || this.value == EventType.BASEBALL.value || this.value == EventType.BASKETBALL.value) {
					return false
				}
				return true
			}
		}
	})();
	EventType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (MarketStatusType) == "undefined") {
	MarketStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	MarketStatusType.Inactive = {
		value: 0
	};
	MarketStatusType.Open = {
		value: 1
	};
	MarketStatusType.Suspend = {
		value: 2
	};
	MarketStatusType.Closed = {
		value: 3
	};
	(function () {
		for (atr in MarketStatusType) {
			var c = MarketStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	MarketStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	MarketStatusType.getName = function (c) {
		if ((c & MarketStatusType.Inactive.unique()) > 0) {
			return MarketStatusType.Inactive.name
		}
		if ((c & MarketStatusType.Open.unique()) > 0) {
			return MarketStatusType.Open.name
		}
		if ((c & MarketStatusType.Suspend.unique()) > 0) {
			return MarketStatusType.Suspend.name
		}
		if ((c & MarketStatusType.Closed.unique()) > 0) {
			return MarketStatusType.Closed.name
		}
		return ""
	}
})();
if (typeof (SelectionStatusType) == "undefined") {
	SelectionStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	SelectionStatusType.Removed = {
		value: 0
	};
	SelectionStatusType.Active = {
		value: 1
	};
	SelectionStatusType.Winner = {
		value: 2
	};
	SelectionStatusType.Loser = {
		value: 3
	};
	SelectionStatusType.Placed = {
		value: 4
	};
	SelectionStatusType.RemovedVacant = {
		value: 5
	};
	SelectionStatusType.Hidden = {
		value: 6
	};
	(function () {
		for (atr in SelectionStatusType) {
			var c = SelectionStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	SelectionStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	};
	SelectionStatusType.getName = function (c) {
		if ((c & SelectionStatusType.Removed.unique()) > 0) {
			return SelectionStatusType.Removed.name
		}
		if ((c & SelectionStatusType.Active.unique()) > 0) {
			return SelectionStatusType.Active.name
		}
		if ((c & SelectionStatusType.Winner.unique()) > 0) {
			return SelectionStatusType.Winner.name
		}
		if ((c & SelectionStatusType.Loser.unique()) > 0) {
			return SelectionStatusType.Loser.name
		}
		if ((c & SelectionStatusType.Placed.unique()) > 0) {
			return SelectionStatusType.Placed.name
		}
		if ((c & SelectionStatusType.RemovedVacant.unique()) > 0) {
			return SelectionStatusType.RemovedVacant.name
		}
		if ((c & SelectionStatusType.Hidden.unique()) > 0) {
			return SelectionStatusType.Hidden.name
		}
		return ""
	}
})();
if (typeof (ManagerType) == "undefined") {
	ManagerType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	ManagerType.MANAGER = {
		value: 1
	};
	ManagerType.RISK_MANAGER = {
		value: 2
	};
	ManagerType.TRADER = {
		value: 3
	};
	(function () {
		for (atr in ManagerType) {
			var c = ManagerType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ManagerType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (BetfairMarketBettingType) == "undefined") {
	BetfairMarketBettingType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	BetfairMarketBettingType.ODDS = {
		value: 1
	};
	BetfairMarketBettingType.ASIAN_HANDICAP_DOUBLE_LINE = {
		value: 2
	};
	BetfairMarketBettingType.ASIAN_HANDICAP_SINGLE_LINE = {
		value: 3
	};
	BetfairMarketBettingType.LINE = {
		value: 4
	};
	BetfairMarketBettingType.RANGE = {
		value: 5
	};
	BetfairMarketBettingType.FIXED_ODDS = {
		value: 6
	};
	(function () {
		for (atr in BetfairMarketBettingType) {
			var c = BetfairMarketBettingType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	BetfairMarketBettingType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (PaymentModeType) == "undefined") {
	PaymentModeType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	PaymentModeType.CREDIT = {
		value: 0
	};
	PaymentModeType.CASH = {
		value: 1
	};
	PaymentModeType.MIX = {
		value: 2
	};
	(function () {
		for (atr in PaymentModeType) {
			var c = PaymentModeType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	PaymentModeType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (HomePageType) == "undefined") {
	HomePageType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	HomePageType.MEMBER = {
		value: 1,
		url: "/member/index.jsp"
	};
	HomePageType.AGENT = {
		value: 2,
		url: "/agent/index.jsp"
	};
	HomePageType.MANAGER = {
		value: 4,
		url: "/manage/index.jsp"
	};
	(function () {
		for (atr in HomePageType) {
			var c = HomePageType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	HomePageType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (ServerInfoType) == "undefined") {
	ServerInfoType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	ServerInfoType.MAINTAIN = {
		value: -1
	};
	ServerInfoType.PLAYER = {
		value: 1
	};
	ServerInfoType.AGENT = {
		value: 2
	};
	ServerInfoType.MANAGER = {
		value: 4
	};
	ServerInfoType.RESULT_SERVER = {
		value: 8
	};
	ServerInfoType.TRANSACTION_SERVER = {
		value: 16
	};
	ServerInfoType.API_SERVER = {
		value: 32
	};
	(function () {
		for (atr in ServerInfoType) {
			var c = ServerInfoType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ServerInfoType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (GameProductType) == "undefined") {
	GameProductType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	GameProductType.ALL = {
		value: 0
	};
	GameProductType.BETFAIR_EXCHANGE = {
		value: 1
	};
	GameProductType.FANCYBET = {
		value: 2
	};
	GameProductType.IBC_SPORTSBOOK = {
		value: 3
	};
	GameProductType.AWC_CASINO = {
		value: 4
	};
	GameProductType.SPORTRADAR_SPORTSBOOK = {
		value: 5
	};
	(function () {
		for (atr in GameProductType) {
			var c = GameProductType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	GameProductType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (BetfairMissTicketType) == "undefined") {
	BetfairMissTicketType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	BetfairMissTicketType.SETTLE = {
		displayName: "Settle",
		value: 1
	};
	BetfairMissTicketType.VOID = {
		displayName: "Void",
		value: 2
	};
	BetfairMissTicketType.LAPSE = {
		displayName: "Lapse",
		value: 3
	};
	BetfairMissTicketType.TIME_OUT = {
		displayName: "TimeOut",
		value: 4
	};
	BetfairMissTicketType.PROCESSED_WITH_ERRORS = {
		displayName: "ProcessedWithErrors",
		value: 5
	};
	(function () {
		for (atr in BetfairMissTicketType) {
			var c = BetfairMissTicketType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	BetfairMissTicketType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (FancySideType) == "undefined") {
	FancySideType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	FancySideType.YES = {
		name: "Yes",
		value: 1
	};
	FancySideType.NO = {
		name: "No",
		value: 2
	};
	(function () {
		for (atr in FancySideType) {
			var c = FancySideType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	FancySideType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (CategoryType) == "undefined") {
	CategoryType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	CategoryType.BETFAIR = {
		name: "BETFAIR",
		value: 1
	};
	CategoryType.FANCY_BET = {
		name: "FANCY_BET",
		value: 2
	};
	CategoryType.IBC_FT1X2 = {
		name: "IBC_FT1X2",
		value: 3
	};
	CategoryType.SPORTRADAR_SPORTSBOOK = {
		name: "SPORTRADAR_SPORTSBOOK",
		value: 4
	};
	(function () {
		for (atr in CategoryType) {
			var c = CategoryType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	CategoryType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (FancyBetMarketStatusType) == "undefined") {
	FancyBetMarketStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	FancyBetMarketStatusType.OFFLINE = {
		value: 1
	};
	FancyBetMarketStatusType.ONLINE = {
		value: 2
	};
	FancyBetMarketStatusType.SUSPEND = {
		value: 4
	};
	FancyBetMarketStatusType.BALL_RUN = {
		value: 8
	};
	FancyBetMarketStatusType.CLOSED = {
		value: 16
	};
	FancyBetMarketStatusType.SETTLE_PROCESSING = {
		value: 32
	};
	FancyBetMarketStatusType.VOID_PROCESSING = {
		value: 64
	};
	FancyBetMarketStatusType.UNSETTLE_PROCESSING = {
		value: 128
	};
	FancyBetMarketStatusType.SETTLED = {
		value: 256
	};
	FancyBetMarketStatusType.VOIDED = {
		value: 512
	};
	(function () {
		for (atr in FancyBetMarketStatusType) {
			var c = FancyBetMarketStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	FancyBetMarketStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (FancyBetMarketType) == "undefined") {
	FancyBetMarketType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	FancyBetMarketType.OVERS = {
		name: "OVERS",
		value: 1
	};
	FancyBetMarketType.BATSMAN = {
		name: "BATSMAN",
		value: 2
	};
	FancyBetMarketType.SINGLE_OVER = {
		name: "SINGLE_OVER",
		value: 3
	};
	(function () {
		for (atr in FancyBetMarketType) {
			var c = FancyBetMarketType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	FancyBetMarketType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (ManagerUserLevelType) == "undefined") {
	ManagerUserLevelType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	ManagerUserLevelType.A = {
		value: 0
	};
	ManagerUserLevelType.B = {
		value: 1
	};
	ManagerUserLevelType.C = {
		value: 2
	};
	ManagerUserLevelType.D = {
		value: 3
	};
	ManagerUserLevelType.E = {
		value: 4
	};
	ManagerUserLevelType.F = {
		value: 5
	};
	ManagerUserLevelType.SUB_ACCOUNT = {
		value: -2
	};
	(function () {
		for (atr in ManagerUserLevelType) {
			var c = ManagerUserLevelType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ManagerUserLevelType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (FeedingSiteMarketStatusType) == "undefined") {
	FeedingSiteMarketStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	FeedingSiteMarketStatusType.RUNNING = {
		value: 1
	};
	FeedingSiteMarketStatusType.INTERNAL = {
		value: 2
	};
	FeedingSiteMarketStatusType.SUSPEND = {
		value: 3
	};
	FeedingSiteMarketStatusType.CLOSEPRICE = {
		value: 4
	};
	FeedingSiteMarketStatusType.CLOSED = {
		value: 5
	};
	(function () {
		for (atr in FeedingSiteMarketStatusType) {
			var c = FeedingSiteMarketStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	FeedingSiteMarketStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (FeedingSiteMarketSettleStatusType) == "undefined") {
	FeedingSiteMarketSettleStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	FeedingSiteMarketSettleStatusType.UNSETTLE = {
		value: 0
	};
	FeedingSiteMarketSettleStatusType.SETTLE = {
		value: 1
	};
	FeedingSiteMarketSettleStatusType.RESETTLE = {
		value: 2
	};
	FeedingSiteMarketSettleStatusType.REFUND = {
		value: 3
	};
	FeedingSiteMarketSettleStatusType.SETTLE_BY_SYSTEM = {
		value: 4
	};
	(function () {
		for (atr in FeedingSiteMarketSettleStatusType) {
			var c = FeedingSiteMarketSettleStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	FeedingSiteMarketSettleStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (SportsBookEventStatusType) == "undefined") {
	SportsBookEventStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	SportsBookEventStatusType.READY = {
		value: 1
	};
	SportsBookEventStatusType.CANCEL = {
		value: 2
	};
	SportsBookEventStatusType.OPEN = {
		value: 4
	};
	SportsBookEventStatusType.SUSPEND = {
		value: 8
	};
	SportsBookEventStatusType.CLOSE = {
		value: 16
	};
	SportsBookEventStatusType.END = {
		value: 32
	};
	SportsBookEventStatusType.UNSETTLED = {
		value: 64
	};
	SportsBookEventStatusType.SETTLED = {
		value: 128
	};
	SportsBookEventStatusType.VOIDED = {
		value: 256
	};
	(function () {
		for (atr in SportsBookEventStatusType) {
			var c = SportsBookEventStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	SportsBookEventStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (SportsBookMarketStatusType) == "undefined") {
	SportsBookMarketStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	SportsBookMarketStatusType.INACTIVE = {
		value: 0
	};
	SportsBookMarketStatusType.OPEN = {
		value: 1
	};
	SportsBookMarketStatusType.SUSPEND = {
		value: 2
	};
	SportsBookMarketStatusType.CLOSED = {
		value: 3
	};
	SportsBookMarketStatusType.SETTLED = {
		value: 4
	};
	(function () {
		for (atr in SportsBookMarketStatusType) {
			var c = SportsBookMarketStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	SportsBookMarketStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (GameType) == "undefined") {
	GameType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	GameType.FT1X2 = {
		value: 0,
		displayName: "FT 1X2"
	};
	(function () {
		for (atr in GameType) {
			var c = GameType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	GameType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (VendorTransferStatusType) == "undefined") {
	VendorTransferStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	VendorTransferStatusType.SUCCESS = {
		value: 1
	};
	VendorTransferStatusType.FAIL = {
		value: 2
	};
	VendorTransferStatusType.UNKNOWN = {
		value: 3
	};
	(function () {
		for (atr in VendorTransferStatusType) {
			var c = VendorTransferStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	VendorTransferStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (BookModeType) == "undefined") {
	BookModeType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	BookModeType.CLOSE = {
		name: "CLOSE",
		value: 0
	};
	BookModeType.OPEN = {
		name: "OPEN",
		value: 1
	};
	(function () {
		for (atr in BookModeType) {
			var c = BookModeType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	BookModeType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (BookSuspendType) == "undefined") {
	BookSuspendType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	BookSuspendType.NONE = {
		name: "NONE",
		value: 0
	};
	BookSuspendType.SUSPEND = {
		name: "SUSPEND",
		value: 1
	};
	(function () {
		for (atr in BookSuspendType) {
			var c = BookSuspendType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	BookSuspendType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (AnnouncementReceiverType) == "undefined") {
	AnnouncementReceiverType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	AnnouncementReceiverType.PLAYER = {
		value: 0,
		name1: "Player"
	};
	AnnouncementReceiverType.AGENT = {
		value: 1,
		name1: "Agent"
	};
	AnnouncementReceiverType.SYSTEM = {
		value: 2,
		name1: "System (Agent & Player)"
	};
	(function () {
		for (atr in AnnouncementReceiverType) {
			var c = AnnouncementReceiverType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	AnnouncementReceiverType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (AnnouncementStatusType) == "undefined") {
	AnnouncementStatusType = {}
} (function () {
	var a = function () {
		return this.value
	};
	var b = [];
	AnnouncementStatusType.ACTIVE = {
		value: 1,
		name: "Active"
	};
	AnnouncementStatusType.CLOSE = {
		value: 0,
		name: "Close"
	};
	(function () {
		for (atr in AnnouncementStatusType) {
			var c = AnnouncementStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	AnnouncementStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (ApiSiteType) == "undefined") {
	ApiSiteType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	ApiSiteType.OW = {
		name: "OW",
		value: 1
	};
	ApiSiteType.SPORTRADAR = {
		name: "SPORTRADAR",
		value: 2
	};
	(function () {
		for (atr in ApiSiteType) {
			var c = ApiSiteType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	ApiSiteType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (SportradarProducerStatusType) == "undefined") {
	SportradarProducerStatusType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	SportradarProducerStatusType.DOWN = {
		value: 0
	};
	SportradarProducerStatusType.ACTIVE = {
		value: 1
	};
	(function () {
		for (atr in SportradarProducerStatusType) {
			var c = SportradarProducerStatusType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	SportradarProducerStatusType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (SportradarProducerType) == "undefined") {
	SportradarProducerType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	SportradarProducerType.PremiumCricket = {
		value: 5
	};
	(function () {
		for (atr in SportradarProducerType) {
			var c = SportradarProducerType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	SportradarProducerType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();
if (typeof (SportradarMarketType) == "undefined") {
	SportradarMarketType = {}
} (function () {
	var b = [];
	var a = function () {
		return this.value
	};
	SportradarMarketType.WinnerSuperOver = {
		value: 340,
		group: "Match",
		sort: 1,
		popular: true
	};
	SportradarMarketType.MatchWinner = {
		value: 1,
		group: "Match",
		sort: 2,
		popular: true
	};
	SportradarMarketType.InningsFirstTotal = {
		value: 606,
		group: "Innings",
		sort: 3,
		popular: false
	};
	SportradarMarketType.InningsSecondTotal = {
		value: 607,
		group: "Innings",
		sort: 3,
		popular: true
	};
	SportradarMarketType.InningsOverDeliveryFirstTotal = {
		value: 362,
		group: "Innings",
		sort: 4,
		popular: false
	};
	SportradarMarketType.InningsOverDeliverySecondTotal = {
		value: 363,
		group: "Innings",
		sort: 4,
		popular: true
	};
	SportradarMarketType.InningsOverFirstTotal = {
		value: 356,
		group: "Over",
		sort: 5,
		popular: true
	};
	SportradarMarketType.InningsOverSecondTotal = {
		value: 357,
		group: "Over",
		sort: 5,
		popular: true
	};
	SportradarMarketType.InningsPlayerTotal = {
		value: 638,
		group: "Player",
		sort: 6,
		popular: false
	};
	SportradarMarketType.InningsFirstTopBatter = {
		value: 674,
		group: "Innings",
		sort: 7,
		popular: false
	};
	SportradarMarketType.InningsSecondTopBatter = {
		value: 675,
		group: "Innings",
		sort: 7,
		popular: true
	};
	SportradarMarketType.InningsFirstTotalatDismissal = {
		value: 349,
		group: "Innings",
		sort: 8,
		popular: true
	};
	SportradarMarketType.InningsSecondTotalatDismissal = {
		value: 350,
		group: "Innings",
		sort: 8,
		popular: true
	};
	SportradarMarketType.InningsOvers0toFirstTotal = {
		value: 352,
		group: "Innings",
		sort: 9,
		popular: true
	};
	SportradarMarketType.InningsOvers0toSecondTotal = {
		value: 353,
		group: "Innings",
		sort: 9,
		popular: true
	};
	SportradarMarketType.InningsFirstTopBowler = {
		value: 676,
		group: "Innings",
		sort: 10,
		popular: false
	};
	SportradarMarketType.InningsSecondTopBowler = {
		value: 677,
		group: "Innings",
		sort: 10,
		popular: true
	};
	SportradarMarketType.Drawnobet = {
		value: 11,
		group: "Match",
		sort: 11,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalfours = {
		value: 658,
		group: "Innings",
		sort: 12,
		popular: true
	};
	SportradarMarketType.InningsSecondTotalfours = {
		value: 659,
		group: "Innings",
		sort: 12,
		popular: true
	};
	SportradarMarketType.InningsFirstTotalsixes = {
		value: 660,
		group: "Innings",
		sort: 13,
		popular: true
	};
	SportradarMarketType.InningsSecondTotalsixes = {
		value: 661,
		group: "Innings",
		sort: 13,
		popular: true
	};
	SportradarMarketType.InningsPlayertoscore = {
		value: 662,
		group: "Player",
		sort: 14,
		popular: false
	};
	SportradarMarketType.Doublechance = {
		value: 10,
		group: "Match",
		sort: 15,
		popular: false
	};
	SportradarMarketType.TopBatter = {
		value: 683,
		group: "Match",
		sort: 16,
		popular: false
	};
	SportradarMarketType.TopBowler = {
		value: 684,
		group: "Match",
		sort: 17,
		popular: false
	};
	SportradarMarketType.Playerofthematch = {
		value: 685,
		group: "Match",
		sort: 18,
		popular: false
	};
	SportradarMarketType.InningsBatterout = {
		value: 657,
		group: "Innings",
		sort: 19,
		popular: false
	};
	SportradarMarketType.InningsRunsBanded = {
		value: 0,
		group: "Innings",
		sort: 20,
		popular: false
	};
	SportradarMarketType.InningsFirstDismissalmethod = {
		value: 816,
		group: "Innings",
		sort: 21,
		popular: false
	};
	SportradarMarketType.InningsSecondDismissalmethod = {
		value: 817,
		group: "Innings",
		sort: 21,
		popular: false
	};
	SportradarMarketType.InningsFirstexactruns = {
		value: 672,
		group: "Innings",
		sort: 22,
		popular: false
	};
	SportradarMarketType.InningsSecondexactruns = {
		value: 673,
		group: "Innings",
		sort: 22,
		popular: false
	};
	SportradarMarketType.InningsPlayerTotalfours = {
		value: 643,
		group: "Player",
		sort: 23,
		popular: false
	};
	SportradarMarketType.InningsPlayerTotalsixes = {
		value: 644,
		group: "Player",
		sort: 24,
		popular: false
	};
	SportradarMarketType.InningsOverFirstDismissal = {
		value: 641,
		group: "Over",
		sort: 25,
		popular: false
	};
	SportradarMarketType.InningsOverSecondDismissal = {
		value: 642,
		group: "Over",
		sort: 25,
		popular: false
	};
	SportradarMarketType.Whichteamwinsthecointossandthematch = {
		value: 710,
		group: "Match",
		sort: 26,
		popular: false
	};
	SportradarMarketType.Whichteamwinsthecointoss = {
		value: 694,
		group: "Match",
		sort: 27,
		popular: false
	};
	SportradarMarketType.InningsFirstlastplayerstanding = {
		value: 678,
		group: "Innings",
		sort: 28,
		popular: false
	};
	SportradarMarketType.InningsSecondlastplayerstanding = {
		value: 679,
		group: "Innings",
		sort: 28,
		popular: false
	};
	SportradarMarketType.OverRunsBanded = {
		value: 0,
		group: "Range",
		sort: 29,
		popular: false
	};
	SportradarMarketType.Innings1x2 = {
		value: 711,
		group: "Match",
		sort: 30,
		popular: false
	};
	SportradarMarketType.Totalfours = {
		value: 639,
		group: "Match",
		sort: 31,
		popular: false
	};
	SportradarMarketType.Totalsixes = {
		value: 640,
		group: "Match",
		sort: 32,
		popular: false
	};
	SportradarMarketType.daysessionTotal = {
		value: 665,
		group: "Match",
		sort: 33,
		popular: false
	};
	SportradarMarketType.Mostfours = {
		value: 647,
		group: "Head-to-Head",
		sort: 34,
		popular: false
	};
	SportradarMarketType.Mostsixes = {
		value: 648,
		group: "Head-to-Head",
		sort: 35,
		popular: false
	};
	SportradarMarketType.Willtherebeatie = {
		value: 342,
		group: "Match",
		sort: 36,
		popular: false
	};
	SportradarMarketType.InningsPlayerDismissalmethod = {
		value: 651,
		group: "Player",
		sort: 37,
		popular: false
	};
	SportradarMarketType.InningsOvers0toFirstTotalDismissals = {
		value: 663,
		group: "Innings",
		sort: 38,
		popular: false
	};
	SportradarMarketType.InningsOvers0toSecondTotalDismissals = {
		value: 664,
		group: "Innings",
		sort: 38,
		popular: false
	};
	SportradarMarketType.Inningsanyplayertoscore = {
		value: 700,
		group: "Match",
		sort: 39,
		popular: false
	};
	SportradarMarketType.Anyplayertoscore = {
		value: 701,
		group: "Match",
		sort: 40,
		popular: false
	};
	SportradarMarketType.Inningspartnership1x2 = {
		value: 656,
		group: "Innings",
		sort: 41,
		popular: false
	};
	SportradarMarketType.InningsOverFirstboundary = {
		value: 652,
		group: "Over",
		sort: 42,
		popular: false
	};
	SportradarMarketType.InningsOverSecondboundary = {
		value: 653,
		group: "Over",
		sort: 42,
		popular: false
	};
	SportradarMarketType.GroupRunsBanded = {
		value: 0,
		group: "Range",
		sort: 43,
		popular: false
	};
	SportradarMarketType.InningsOver1x2 = {
		value: 645,
		group: "Head-to-Head",
		sort: 44,
		popular: false
	};
	SportradarMarketType.InningsOvers0to1x2 = {
		value: 351,
		group: "Head-to-Head",
		sort: 45,
		popular: false
	};
	SportradarMarketType.Batterhead2headHandicap = {
		value: 717,
		group: "Head-to-Head",
		sort: 46,
		popular: false
	};
	SportradarMarketType.TeamwithTopBatter = {
		value: 698,
		group: "Head-to-Head",
		sort: 47,
		popular: false
	};
	SportradarMarketType.InningsFirstOddEven = {
		value: 690,
		group: "Innings",
		sort: 48,
		popular: false
	};
	SportradarMarketType.InningsSecondOddEven = {
		value: 691,
		group: "Innings",
		sort: 48,
		popular: false
	};
	SportradarMarketType.InningsFirsttofinishwithaboundary = {
		value: 692,
		group: "Innings",
		sort: 49,
		popular: false
	};
	SportradarMarketType.InningsSecondtofinishwithaboundary = {
		value: 693,
		group: "Innings",
		sort: 49,
		popular: false
	};
	SportradarMarketType.InningsOverFirstOddEven = {
		value: 359,
		group: "Over",
		sort: 50,
		popular: false
	};
	SportradarMarketType.InningsOverSecondOddEven = {
		value: 360,
		group: "Over",
		sort: 50,
		popular: false
	};
	SportradarMarketType.TeamwithhighestscoreatDismissal = {
		value: 646,
		group: "Head-to-Head",
		sort: 51,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalinthehighestscoringOver = {
		value: 670,
		group: "Innings",
		sort: 52,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalinthehighestscoringOver = {
		value: 671,
		group: "Innings",
		sort: 52,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalrunouts = {
		value: 668,
		group: "Innings",
		sort: 53,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalrunouts = {
		value: 669,
		group: "Innings",
		sort: 53,
		popular: false
	};
	SportradarMarketType.TopBatterTotal = {
		value: 702,
		group: "Match",
		sort: 54,
		popular: false
	};
	SportradarMarketType.InningsPlayerTotalDismissals = {
		value: 708,
		group: "Player",
		sort: 55,
		popular: false
	};
	SportradarMarketType.PlayerTotalplayerperformance = {
		value: 709,
		group: "Player",
		sort: 56,
		popular: false
	};
	SportradarMarketType.TeamwithTopBowler = {
		value: 699,
		group: "Head-to-Head",
		sort: 57,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalextras = {
		value: 666,
		group: "Innings",
		sort: 58,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalextras = {
		value: 667,
		group: "Innings",
		sort: 58,
		popular: false
	};
	SportradarMarketType.Totalrunouts = {
		value: 654,
		group: "Match",
		sort: 59,
		popular: false
	};
	SportradarMarketType.Totalextras = {
		value: 655,
		group: "Match",
		sort: 60,
		popular: false
	};
	SportradarMarketType.TotalinthehighestscoringOver = {
		value: 682,
		group: "Match",
		sort: 61,
		popular: false
	};
	SportradarMarketType.Batterhead2head = {
		value: 686,
		group: "Head-to-Head",
		sort: 62,
		popular: false
	};
	SportradarMarketType.Bowlerhead2head = {
		value: 687,
		group: "Head-to-Head",
		sort: 63,
		popular: false
	};
	SportradarMarketType.Allrounderhead2head = {
		value: 688,
		group: "Head-to-Head",
		sort: 64,
		popular: false
	};
	SportradarMarketType.Keeperhead2head = {
		value: 689,
		group: "Head-to-Head",
		sort: 65,
		popular: false
	};
	SportradarMarketType.Mostkeepercatches = {
		value: 712,
		group: "Head-to-Head",
		sort: 66,
		popular: false
	};
	SportradarMarketType.Mostcatches = {
		value: 713,
		group: "Head-to-Head",
		sort: 67,
		popular: false
	};
	SportradarMarketType.Moststumpings = {
		value: 714,
		group: "Head-to-Head",
		sort: 68,
		popular: false
	};
	SportradarMarketType.Mostrunouts = {
		value: 681,
		group: "Head-to-Head",
		sort: 69,
		popular: false
	};
	SportradarMarketType.Totalducks = {
		value: 695,
		group: "Match",
		sort: 70,
		popular: false
	};
	SportradarMarketType.Totalwides = {
		value: 696,
		group: "Match",
		sort: 71,
		popular: false
	};
	SportradarMarketType.TotalDismissals = {
		value: 697,
		group: "Match",
		sort: 72,
		popular: false
	};
	SportradarMarketType.RabbitTotal = {
		value: 703,
		group: "Match",
		sort: 73,
		popular: false
	};
	SportradarMarketType.Mostextras = {
		value: 680,
		group: "Head-to-Head",
		sort: 74,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalwidesbowled = {
		value: 704,
		group: "Innings",
		sort: 75,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalwidesbowled = {
		value: 705,
		group: "Innings",
		sort: 75,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalducks = {
		value: 706,
		group: "Innings",
		sort: 76,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalducks = {
		value: 707,
		group: "Innings",
		sort: 76,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalDismissals = {
		value: 649,
		group: "Innings",
		sort: 77,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalDismissals = {
		value: 650,
		group: "Innings",
		sort: 77,
		popular: false
	};
	SportradarMarketType.WilltherebeasuperOver = {
		value: 341,
		group: "Match",
		sort: 78,
		popular: false
	};
	SportradarMarketType.InningsDismissalmethod = {
		value: 343,
		group: "Innings",
		sort: 79,
		popular: false
	};
	SportradarMarketType.InningsDismissalmethodLimited = {
		value: 344,
		group: "Innings",
		sort: 80,
		popular: false
	};
	SportradarMarketType.InningsFirstrunrange = {
		value: 345,
		group: "Range",
		sort: 81,
		popular: false
	};
	SportradarMarketType.InningsSecondrunrange = {
		value: 346,
		group: "Range",
		sort: 81,
		popular: false
	};
	SportradarMarketType.InningsteamwithhighestscoringOver = {
		value: 347,
		group: "Innings",
		sort: 82,
		popular: false
	};
	SportradarMarketType.InningsrunrangeinthehighestscoringOver = {
		value: 348,
		group: "Range",
		sort: 83,
		popular: false
	};
	SportradarMarketType.InningsOvers0toFirstrunrange = {
		value: 354,
		group: "Range",
		sort: 84,
		popular: false
	};
	SportradarMarketType.InningsOvers0toSecondrunrange = {
		value: 355,
		group: "Range",
		sort: 84,
		popular: false
	};
	SportradarMarketType.FirstOverTotal = {
		value: 358,
		group: "Over",
		sort: 85,
		popular: false
	};
	SportradarMarketType.FirstOverOddEven = {
		value: 361,
		group: "Over",
		sort: 86,
		popular: false
	};
	SportradarMarketType.InningsTotal = {
		value: 605,
		group: "Innings",
		sort: 87,
		popular: false
	};
	SportradarMarketType.InningsOddEven = {
		value: 608,
		group: "Innings",
		sort: 88,
		popular: false
	};
	SportradarMarketType.InningsOverFirstrunrange = {
		value: 715,
		group: "Over",
		sort: 89,
		popular: false
	};
	SportradarMarketType.InningsOverSecondrunrange = {
		value: 716,
		group: "Over",
		sort: 89,
		popular: false
	};
	SportradarMarketType.InningsDismissalmethodExtended = {
		value: 718,
		group: "Innings",
		sort: 90,
		popular: false
	};
	SportradarMarketType.Firstwindex = {
		value: 835,
		group: "Over",
		sort: 91,
		popular: false
	};
	SportradarMarketType.Secondwindex = {
		value: 836,
		group: "Over",
		sort: 91,
		popular: false
	};
	SportradarMarketType.FirstSecondsupremacy = {
		value: 837,
		group: "Over",
		sort: 92,
		popular: false
	};
	SportradarMarketType.SecondFirstsupremacy = {
		value: 838,
		group: "Over",
		sort: 92,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalspread = {
		value: 839,
		group: "Over",
		sort: 93,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalspread = {
		value: 840,
		group: "Over",
		sort: 93,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalspreadatDismissal = {
		value: 841,
		group: "Over",
		sort: 94,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalspreadatDismissal = {
		value: 842,
		group: "Over",
		sort: 94,
		popular: false
	};
	SportradarMarketType.InningsPlayerTotalspread = {
		value: 843,
		group: "Over",
		sort: 95,
		popular: false
	};
	SportradarMarketType.InningsOvers0toFirstTotalspread = {
		value: 844,
		group: "Over",
		sort: 96,
		popular: false
	};
	SportradarMarketType.InningsOvers0toSecondTotalspread = {
		value: 845,
		group: "Over",
		sort: 96,
		popular: false
	};
	SportradarMarketType.InningsOverFirstTotalspread = {
		value: 846,
		group: "Over",
		sort: 97,
		popular: false
	};
	SportradarMarketType.InningsOverSecondTotalspread = {
		value: 847,
		group: "Over",
		sort: 97,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalatDismissalMaxovers = {
		value: 875,
		group: "Over",
		sort: 98,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalatDismissalMaxovers = {
		value: 876,
		group: "Over",
		sort: 98,
		popular: false
	};
	SportradarMarketType.InningsFirstTotalMaxover = {
		value: 877,
		group: "Over",
		sort: 99,
		popular: false
	};
	SportradarMarketType.InningsSecondTotalMaxover = {
		value: 878,
		group: "Over",
		sort: 99,
		popular: false
	};
	(function () {
		for (atr in SportradarMarketType) {
			var c = SportradarMarketType[atr];
			b[b.length] = c;
			c.name = atr;
			c.unique = a
		}
	})();
	SportradarMarketType.getInstanceOf = function (d) {
		for (var c = 0; c < b.length; c++) {
			if (b[c]["value"] == d) {
				return b[c]
			}
		}
		return null
	}
})();