function selectTeams(id) {
	$.ajax({
      url: "select_teams.php",
      method: "Post",
      data: {id:id},
      success: function(result){
        $("#team_a").html(result);
        $("#team_b").html(result);
    }});
}

function checkTeamA() {
	var team_a = $("#team_a").val();
	var team_b = $("#team_b").val();

	if (team_a==team_b) {
		alert("Select diffrent team");
		$("#team_b").val("");
	}
}

function checkTeamB() {
	var team_a = $("#team_a").val();
	var team_b = $("#team_b").val();

	if (team_a==team_b) {
		alert("Select diffrent team");
		$("#team_a").val("");
	}
}