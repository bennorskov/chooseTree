<?php 
	$mysqli = new mysqli("bennorskov.fatcowmysql.com", "unityd4ta", "!need_all88", "choose_a_tree");
	if ($mysqli->connect_errno) {
	    echo "Failed to connect to MySQL: (" . $mysqli->connect_errno . ") " . $mysqli->connect_error;
	}
?>