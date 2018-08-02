<?php
	require_once "config.php";
	// echo $mysqli->host_info . "\n";
	$choice1Text = "not set";
	$choice2Text = "not set";
	$choice1Num = -1;
	$choice2Num = -1;

	$currentPositionId = 1;
	if ($result = $mysqli->query("SELECT * FROM choiceStruct WHERE id = '$currentPositionId'")) {
	 	while ($row = $result->fetch_assoc()) {
	        $choice1Text = ($row["choice1"]);
	        $choice2Text = ($row["choice2"]);
	        $choice1Num = ($row["choice1Num"]);
	        $choice2Num = ($row["choice2Num"]);

	    }
	 } else {
	 	echo "<h1>query failed</h1>";
	 }

	 // Database: displayText	choice1	choice2	id	choice1Num	choice2Num


	 /* close connection */
	$mysqli->close();

	// fetch the current node based on the $_GET from the website
	// set the position in the currentState db
	// display the choises available at the place in .html
?> 
<html lang="en">
<head>
	<link href="styles.css" rel="stylesheet">
	<script src="scripts.js" type="text/Javascript"></script> 
</head>
<body onload="init()">
	<div id="choiceHolder">
		<h1 id='choice1' data-choice-number="<?php echo $choice1Num;?>">
			<?php echo $choice1Text; ?>
		</h1>
		<h1 id='choice2' data-choice-number="<?php echo $choice2Num;?>">
			<?php echo $choice2Text; ?>
		</h1>
	</div>
</body>
</html>