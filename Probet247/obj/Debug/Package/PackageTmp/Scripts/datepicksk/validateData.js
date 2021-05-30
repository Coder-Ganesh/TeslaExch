if (typeof (I18N) == "undefined") {
	document.write('<script language="JavaScript"  type="text/JavaScript" src="~/Scripts/datepicksk/I18N.js"></script>')
}
var Class = {
	create: function () {
		return function () {
			this.initialize.apply(this, arguments)
		}
	}
};
UserInterruptedException = function () {
	return {
		name: "Interrupted Exception",
		message: ""
	}
};
NotValidException = function (a) {
	return {
		name: "Not Valid Exception ",
		message: a
	}
};
var TextField = Class.create();
TextField.prototype = {
	required: true,
	field: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (ValidateDataUtil.isEmpty(c.value) && this.required) {
				throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	success: function () {
		return true
	},
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	},
	setRequired: function (a) {
		this.required = a;
		return this
	}
};
var Account = Class.create();
Account.prototype = {
	required: true,
	field: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (ValidateDataUtil.isEmpty(c.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (!ValidateDataUtil.isAlphaNumeric(c.value, false) && this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldNotValid", [this.fieldMessage]))
				}
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	},
	setRequired: function (a) {
		this.required = a;
		return this
	}
};
var Select = Class.create();
Select.prototype = {
	field: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (c.options.length == 0 || c.selectedIndex == -1 || ValidateDataUtil.isEmpty(c.options[c.selectedIndex].value)) {
				throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var Radio = Class.create();
Radio.prototype = {
	field: null,
	required: true,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isEmpty: function (b) {
		if (!b.length) {
			b = new Array(b)
		}
		for (var a = 0; a < b.length; a++) {
			if (b[a].checked) {
				return false
			}
		}
		return true
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (this.isEmpty(c)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
				return true
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field != null && this.field[0] != null) {
			this.field[0].focus()
		}
		throw a
	}
};
var Checkbox = Class.create();
Checkbox.prototype = {
	field: null,
	required: true,
	max: null,
	count: 0,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isEmpty: function (b) {
		if (!b.length) {
			b = new Array(b)
		}
		for (var a = 0; a < b.length; a++) {
			if (b[a].checked) {
				this.count++
			}
		}
		return (this.count == 0)
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			this.count = 0;
			if (this.isEmpty(c)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
				return true
			}
			if (this.max != null && this.count > this.max) {
				throw new NotValidException("over the max selection : " + this.fieldMessage)
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field != null && this.field[0] != null) {
			this.field[0].focus()
		}
		throw a
	}
};
var Integer = Class.create();
Integer.prototype = {
	field: null,
	required: true,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (ValidateDataUtil.isEmpty(c.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (isNaN(c.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.mustBeInteger", [this.fieldMessage]))
				}
				if (!isNaN(this.min) && c.value < this.min) {
					throw new NotValidException(I18N.get("msg.error.validation.notLess", [this.min, this.fieldMessage]))
				}
				if (!isNaN(this.max) && c.value > this.max) {
					throw new NotValidException(I18N.get("msg.error.validation.notGreater", [this.max, this.fieldMessage]))
				}
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	setRange: function (b, a) {
		if (!isNaN(b)) {
			this.min = b
		}
		if (!isNaN(a)) {
			this.max = a
		}
		return this
	},
	setMax: function (a) {
		if (!isNaN(a)) {
			this.max = a
		}
		return this
	},
	setMin: function (a) {
		if (!isNaN(a)) {
			this.min = a
		}
		return this
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var Float = Class.create();
Float.prototype = {
	required: true,
	field: null,
	precision: 0,
	scale: 0,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (b) {
		try {
			var f = this.field = ValidateDataUtil.getField(b, this.fieldName);
			if (ValidateDataUtil.isEmpty(f.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (isNaN(f.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.mustBeNumber", [this.fieldMessage]))
				}
				if (!isNaN(this.min) && f.value < this.min) {
					throw new NotValidException(I18N.get("msg.error.validation.notLess", [this.min, this.fieldMessage]))
				}
				if (!isNaN(this.max) && f.value > this.max) {
					throw new NotValidException(I18N.get("msg.error.validation.notGreater", [this.max, this.fieldMessage]))
				}
				if (this.precision > this.scale) {
					var c = this.precision - this.scale;
					var a = "(^(-?\\d{0," + c + "})(\\.\\d{0," + this.scale + "})?$)";
					if (!new RegExp(a).test(f.value)) {
						throw new NotValidException(I18N.get("msg.error.validation.decimalError", [this.precision, c, this.scale, this.fieldMessage]))
					}
				}
			}
		} catch (d) {
			this.fail(d);
			return false
		}
		this.success();
		return true
	},
	setDecimal: function (a, b) {
		this.precision = a;
		this.scale = b;
		return this
	},
	setRange: function (b, a) {
		if (!isNaN(b)) {
			this.min = b
		}
		if (!isNaN(a)) {
			this.max = a
		}
		return this
	},
	setMax: function (a) {
		if (!isNaN(a)) {
			this.max = a
		}
		return this
	},
	setMin: function (a) {
		if (!isNaN(a)) {
			this.min = a
		}
		return this
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var DateFormat = Class.create();
DateFormat.prototype = {
	required: true,
	format: "dd/MM/yyyy",
	field: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var d = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (ValidateDataUtil.isEmpty(d.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				var b = this.format;
				b = b.replace(/yyyy/, "[0-9]{4}");
				b = b.replace(/yy/, "[0-9]{2}");
				b = b.replace(/MM/, "((0[1-9])|1[0-2])");
				b = b.replace(/M/, "(([1-9])|1[0-2])");
				b = b.replace(/dd/, "((0[1-9])|([1-2][0-9])|30|31)");
				b = b.replace(/d/, "([1-9]|[1-2][0-9]|30|31))");
				b = b.replace(/HH/, "(([0-1][0-9])|20|21|22|23)");
				b = b.replace(/H/, "([0-9]|1[0-9]|20|21|22|23)");
				b = b.replace(/mm/, "([0-5][0-9])");
				b = b.replace(/m/, "([0-9]|([1-5][0-9]))");
				b = new RegExp("^" + b + "$");
				if (!b.test(d.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.errorFormat", [this.fieldMessage, "(ex : 11/12/2009)"]))
				}
			}
		} catch (c) {
			this.fail(c);
			return false
		}
		this.success();
		return true
	},
	setFormat: function (a) {
		this.format = a;
		return this
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var Password = Class.create();
Password.prototype = {
	required: true,
	fields: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (b) {
		try {
			var a = this.fields = ValidateDataUtil.getFields(b, this.fieldName);
			var f = a[0];
			var c = a[1];
			if (f == null || c == null) {
				throw new NotValidException(I18N.get("msg.error.validation.notExistedField", [this.fieldName]))
			}
			if (ValidateDataUtil.isEmpty(f.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (!ValidateDataUtil.isValidPassword(f.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.passwordFormatError", [this.fieldMessage]))
				}
				if (f.value != c.value) {
					throw new NotValidException(I18N.get("msg.error.validation.passwordNotMatch"))
				}
			}
		} catch (d) {
			this.fail(d);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (b) {
		if (this.fields != null && this.fields[0] != null) {
			var a = this.fields[0];
			if (!a.length) {
				a.focus()
			} else {
				a[0].focus()
			}
		}
		throw b
	}
};
var OpPassword = Class.create();
OpPassword.prototype = {
	required: true,
	fields: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (b) {
		try {
			var a = this.fields = ValidateDataUtil.getFields(b, this.fieldName);
			var f = a[0];
			var c = a[1];
			if (f == null || c == null) {
				throw new NotValidException(I18N.get("msg.error.validation.notExistedField", [this.fieldName]))
			}
			if (ValidateDataUtil.isEmpty(f.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (!ValidateDataUtil.isValidOpPassword(f.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.passwordFormatError", [this.fieldMessage]))
				}
				if (f.value != c.value) {
					throw new NotValidException(I18N.get("msg.error.validation.passwordNotMatch"))
				}
			}
		} catch (d) {
			this.fail(d);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (b) {
		if (this.fields != null && this.fields[0] != null) {
			var a = this.fields[0];
			if (!a.length) {
				a.focus()
			} else {
				a[0].focus()
			}
		}
		throw b
	}
};
var SinglePassword = Class.create();
SinglePassword.prototype = {
	required: true,
	field: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (c == null) {
				throw new NotValidException(I18N.get("msg.error.validation.notExistedField", [this.fieldName]))
			}
			if (ValidateDataUtil.isEmpty(c.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (!ValidateDataUtil.isValidPassword(c.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.passwordFormatError", [this.fieldMessage]))
				}
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var OpSinglePassword = Class.create();
OpSinglePassword.prototype = {
	required: true,
	field: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (c == null) {
				throw new NotValidException(I18N.get("msg.error.validation.notExistedField", [this.fieldName]))
			}
			if (ValidateDataUtil.isEmpty(c.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (!ValidateDataUtil.isValidOpPassword(c.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.passwordFormatError", [this.fieldMessage]))
				}
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var PinCode = Class.create();
PinCode.prototype = {
	required: true,
	fields: null,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (b) {
		try {
			var a = this.fields = ValidateDataUtil.getFields(b, this.fieldName);
			var f = a[0];
			var c = a[1];
			if (f == null || c == null) {
				throw new NotValidException(I18N.get("msg.error.validation.notExistedField", [this.fieldName]))
			}
			if (ValidateDataUtil.isEmpty(f.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (!ValidateDataUtil.isValidPinCode(f.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.pinCodeFormatError", [this.fieldMessage]))
				}
				if (f.value != c.value) {
					throw new NotValidException(I18N.get("msg.error.validation.pinCodeNotMatch"))
				}
			}
		} catch (d) {
			this.fail(d);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (b) {
		if (this.fields != null && this.fields[0] != null) {
			var a = this.fields[0];
			if (!a.length) {
				a.focus()
			} else {
				a[0].focus()
			}
		}
		throw b
	}
};
var Email = Class.create();
Email.prototype = {
	required: true,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (ValidateDataUtil.isEmpty(c.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (!ValidateDataUtil.isEmail(c.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.errorFormat", [this.fieldMessage, ""]))
				}
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	setRequired: function (a) {
		this.required = a;
		return this
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var Rebate = Class.create();
Rebate.prototype = {
	required: true,
	initialize: function (d, b, a) {
		this.fieldName = d;
		this.fieldMessage = b;
		for (var c in a) {
			this[c] = a[c]
		}
	},
	isValidate: function (a) {
		try {
			var c = this.field = ValidateDataUtil.getField(a, this.fieldName);
			if (ValidateDataUtil.isEmpty(c.value)) {
				if (this.required) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldEmpty", [this.fieldMessage]))
				}
			} else {
				if (isNaN(c.value)) {
					throw new NotValidException(I18N.get("msg.error.validation.mustBeNumber", [this.fieldMessage]))
				}
				if (!isNaN(c.value) && (c.value / 100 >= 10)) {
					throw new NotValidException(I18N.get("msg.error.validation.fieldNotValid", [this.fieldMessage]))
				}
			}
		} catch (b) {
			this.fail(b);
			return false
		}
		this.success();
		return true
	},
	success: function () { },
	fail: function (a) {
		if (this.field) {
			this.field.focus()
		}
		throw a
	}
};
var Confirm = Class.create();
Confirm.prototype = {
	initialize: function (a) {
		this.errorMessage = a
	},
	isValidate: function () {
		try {
			if (!confirm(this.errorMessage)) {
				throw new UserInterruptedException("")
			}
		} catch (a) {
			this.fail(a);
			return false
		}
		this.success();
		return true
	},
	success: function () { },
	fail: function (a) {
		throw a
	}
};
if (typeof (ValidateDataUtil) == "undefined") {
	ValidateDataUtil = {}
} (function () {
	ValidateDataUtil.getFormElement = function (b) {
		var a = null;
		a = document.getElementById(b);
		if (a != null) {
			return a
		}
		a = document.getElementsByName(b);
		if (a != null) {
			return a[0]
		}
		throw new NotValidException(I18N.get("msg.error.validation.notExistedForm", [b]))
	};
	ValidateDataUtil.isAlphaNumeric = function (c, a) {
		if (ValidateDataUtil.isEmpty(c)) {
			return false
		}
		var b;
		if (a) {
			b = /^[0-9a-zA-Z\\ ]+$/
		} else {
			b = /^[0-9a-zA-Z]+$/
		}
		if (!b.test(c)) {
			return false
		}
		return true
	};
	ValidateDataUtil.isEmpty = function (b) {
		for (var a = 0; a < b.length; a++) {
			if (" " != b.charAt(a)) {
				return false
			}
		}
		return true
	};
	ValidateDataUtil.isValidPassword = function (b) {
		if (ValidateDataUtil.isEmpty(b)) {
			return false
		}
		var a = /^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z]{8,15}$/;
		return a.test(b)
	};
	ValidateDataUtil.isValidOpPassword = function (h) {
		if (ValidateDataUtil.isEmpty(h)) {
			return false
		}
		var e = /^[0-9a-zA-Z]{8,15}$/;
		if (!e.test(h)) {
			return false
		}
		var b = h.split("");
		var j = "";
		var d = "";
		var g = false;
		var a = false;
		for (i = 0; i < b.length; i++) {
			var f = b[i].charCodeAt(0);
			var c = b[i];
			if (g && a) {
				return true
			}
			if (!g && (f >= 65 && f <= 90 || f >= 97 && f <= 122)) {
				if (j.length > 0 && j.indexOf(c) < 0) {
					g = true;
					if (a) {
						return true
					}
					continue
				}
				j += c
			}
			if (!a && f >= 48 && f < 57) {
				if (d.length > 0 && d.indexOf(c) < 0) {
					a = true;
					if (g) {
						return true
					}
					continue
				}
				d += c
			}
		}
		return false
	};
	ValidateDataUtil.isValidPinCode = function (b) {
		if (ValidateDataUtil.isEmpty(b)) {
			return false
		}
		var a = /^[0-9]{6,6}/;
		if (!a.test(b)) {
			return false
		}
		if (/^(\d)\1+$/.test(b)) {
			return false
		}
		var c = b.replace(/\d/g, function (d, e) {
			return parseInt(d) - e
		});
		if (/^(\d)\1+$/.test(c)) {
			return false
		}
		c = b.replace(/\d/g, function (d, e) {
			return parseInt(d) + e
		});
		if (/^(\d)\1+$/.test(c)) {
			return false
		}
		return true
	};
	ValidateDataUtil.getField = function (a, b) {
		var c = a[b];
		if (c == null) {
			throw new NotValidException(I18N.get("msg.error.validation.notExistedField", [b]))
		}
		return c
	};
	ValidateDataUtil.getFields = function (b, c) {
		var a = c.split(",");
		for (var d = 0; d < a.length; d++) {
			var e = b[ValidateDataUtil.trim(a[d])];
			if (e == null) {
				throw new NotValidException(I18N.get("msg.error.validation.notExistedField", [formFields[d]]))
			}
			a[d] = e
		}
		return a
	};
	ValidateDataUtil.trim = function (a) {
		return a.replace(/(^\s*)|(\s*$)/g, "")
	};
	ValidateDataUtil.isEmail = function (b) {
		if (ValidateDataUtil.isEmpty(b)) {
			return false
		}
		var a = /^([a-zA-Z\.0-9_-])+@([a-zA-Z0-9_-])+(\.[a-zA-Z0-9_-])+/;
		return a.test(b)
	}
}());
var FormObject = Class.create();
FormObject.prototype = {
	checkArray: [],
	initialize: function (a) {
		this.checkArray = [];
		this.form = ValidateDataUtil.getFormElement(a)
	},
	add: function (a) {
		this.checkArray[this.checkArray.length] = a
	},
	submit: function () {
		this.form.submit()
	},
	checkData: function () {
		try {
			for (var b = 0, a = this.checkArray.length; b < a; b++) {
				this.checkArray[b].isValidate(this.form)
			}
		} catch (c) {
			if (c.name == "Interrupted Exception") {
				return false
			}
			throw {
				name: c.name + "\n\n",
				message: c.message
			};
			return false
		}
		return true
	}
};