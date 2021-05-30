var tvData = "";
var response = CryptojsDecrypt(apiData);
tvData = response.data;
var sslTv
tvData.ssl == true ? sslTv = true : sslTv = false
showMediaPlayer(response.data);

function showMediaPlayer(tvData) {
	if (platform == "ios") {
		var webrtcPlayer = null;
		webrtcPlayer = new UnrealWebRTCPlayer("p1", camstring, "", tvData.IP, tvData.port, sslTv, true, tvData.ptype);
		webrtcPlayer.Play();
	} else {
		if ("MediaSource" in window && "WebSocket" in window) {
			RunPlayer("p1", '100%', '100%', tvData.IP, tvData.port, sslTv, camstring, "", true, true, 0.01, "", false);
			document.getElementById("p1_Video").muted = true;
			document.getElementById("p1_fullscreen").style.display = "none";
		} else {
			document.getElementById("p1").innerHTML = "Media Source Extensions or Websockets are not supported in your browser.";
		}
	}
}