<?php
	require_once "config.php";
	//update currentState=>position to the _GET request
	// sendNewPosition.php/?idToGoTo=[#]

	$nextId = $_GET["idToGoTo"];
	
	/*
	// test code. 
	if($_GET["idToGoTo"] === "") echo "a is an empty string<br>";
	if($_GET["idToGoTo"] === false) echo "a is false<br>";
	if($_GET["idToGoTo"] === null) echo "a is null<br>";
	if(isset($_GET["idToGoTo"])) echo "a is set<br>";
	if(!empty($_GET["idToGoTo"])) echo "a is not empty<br>";
	echo "<br>";

	echo $_GET["idToGoTo"];
	echo "<br>";
	
	echo $nextId;
	*/

	// Database: displayText	choice1	choice2	id	choice1Num	choice2Num

	$nextId = strip_tags($nextId); // I ain't good at security, but I know a little.
	
	if ($result = $mysqli->query("SELECT * FROM choiceStruct WHERE id = '$nextId'")) {
	 	
	 	$row = $result->fetch_assoc();

        $jsonObj->choice1 = $row["choice1"];
        $jsonObj->choice2 = $row["choice2"];
        $jsonObj->choice1Num = $row["choice1Num"];
        $jsonObj->choice2Num = $row["choice2Num"];

        //Update teh currentState db with information from node we're at
	 	$sqlQuery = "UPDATE currentState SET ";
	 	$sqlQuery .= "id=".$nextId.", ";
      	$sqlQuery .= "choice2='".$row["choice2"]."', ";
     	$sqlQuery .= "choice1Num=".$row["choice1Num"].", ";
		$sqlQuery .= "choice1='".$row["choice1"]."', ";
        $sqlQuery .= "choice2Num=".$row["choice2Num"].", ";
        $sqlQuery .= "displayText='".$row["displayText"]."'";

       # echo "<h1>".$sqlQuery."</h1>";
	} else {
		echo "<h1>query failed</h1>";
	}
	$mysqli->query($sqlQuery);
	$myJSON = json_encode($jsonObj);
	echo $myJSON;
?>




