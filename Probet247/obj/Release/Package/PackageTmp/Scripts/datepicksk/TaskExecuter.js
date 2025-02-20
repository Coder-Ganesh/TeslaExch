if (typeof (TaskExecuter) == "undefined") {
	TaskExecuter = {}
} (function () {
	var a = [];
	var b = null;
	TaskExecuter.execute = function () {
		if (a.length != 0) {
			b = a.pop();
			b.execute()
		} else {
			setTimeout(TaskExecuter.execute, 500)
		}
	};
	TaskExecuter.addTask = function (c) {
		a.push(c)
	};
	TaskExecuter.reExecute = function () {
		b.execute()
	};
	TaskExecuter.createTask = function (f, e, c) {
		var d = {
			cycleTime: f,
			cycleTick: e,
			execute: c,
			isStop: false,
			run: function () {
				if (this.isStop) {
					return
				}
				this.cycleTick = this.cycleTime;
				TaskExecuter.addTask(this)
			},
			check: function () {
				if (this.cycleTick < 0) {
					return
				}
				var g = this;
				if (this.cycleTick == 0) {
					this.cycleTick = -1;
					setTimeout(function () {
						g.run()
					}, 10);
					return
				}
				if (this.cycleTick > 0) {
					this.cycleTick--;
					setTimeout(function () {
						g.check()
					}, 1000);
					return
				}
			},
			refresh: function () {
				this.cycleTick = 0
			},
			stop: function () {
				this.isStop = true
			}
		};
		return d
	}
})();