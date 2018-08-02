<?php
	
	require_once "config.php";
	
	$jsonObj;
	if ($result = $mysqli->query("SELECT * FROM currentState")) {
		
	 	// normally, you'd do a loop here
	 	// I'm not, because I only want one response
	 	$result = $result->fetch_assoc();
	 	# echo $result;
        $jsonObj->id = $result["id"];
        $jsonObj->displayText = $result["displayText"];
        $jsonObj->choice1 = $result["choice1"];
        $jsonObj->choice2 = $result["choice2"];
        $jsonObj->choice1Num = $result["choice1Num"];
        $jsonObj->choice2Num = $result["choice2Num"];
	 } else {
	 	$jsonObj->id = 0;
        $jsonObj->displayText = "didn't load";
        $jsonObj->choice1 = "go to 1";
        $jsonObj->choice2 = "go to 0";
        $jsonObj->choice1Num = 1;
        $jsonObj->choice2Num = 0;
	 }

	echo json_encode($jsonObj);
?>	