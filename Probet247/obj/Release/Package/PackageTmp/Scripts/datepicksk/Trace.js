if (typeof (Trace) == "undefined") {
	trace = Trace = {}
} (function () {
	Trace.printStackTrace = function (a) { };
	if (!window.console) {
		Trace.log = function (a) { };
		Trace.warn = function (a) { };
		Trace.error = function (a) { };
		Trace.info = function (a) { };
		Trace.trace = function (a) { };
		Trace.dir = function (a) { };
		Trace.debug = function (a) { }
	} else {
		Trace.log = function (a) {
			console.log(a)
		};
		Trace.info = function (a) {
			console.info(a)
		};
		Trace.warn = function (a) {
			console.warn(a)
		};
		Trace.error = function (a) {
			console.error(a)
		};
		Trace.trace = function (b) {
			try {
				console.trace(b)
			} catch (a) {
				console.log(b)
			}
		};
		Trace.dir = function (b) {
			try {
				console.dir(b)
			} catch (a) {
				console.log(b)
			}
		};
		Trace.debug = function (b) {
			try {
				console.debug(b)
			} catch (a) {
				console.log(b)
			}
		}
	}
})();