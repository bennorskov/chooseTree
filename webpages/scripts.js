function init() {
	console.log("script working");

	// DOM element looks like this:
	// <h1 id="choice[#]" data-choice-number="[#]">
	$choice1=document.getElementById("choice1");
	$choice1.addEventListener("click", function () {
		gotoThisChoice( Number($choice1.getAttribute("data-choice-number")) );
	});

	$choice2=document.getElementById("choice2");
	$choice2.addEventListener("click", function () {
		gotoThisChoice( Number($choice2.getAttribute("data-choice-number")) );

	});
}

function gotoThisChoice(_num) {
	console.log("go to " + _num);
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function() {
		// {"choice1":"Goto Choice 1","choice2":"Goto Choice 2","choice1Num":"1","choice2Num":"2"}
		if (xhttp.readyState == 4 && xhttp.status == 200) {
			var jsonResponce = JSON.parse(this.responseText);
			$choice1=document.getElementById("choice1");
			$choice2=document.getElementById("choice2");
			$choice1.setAttribute("data-choice-number", Number(jsonResponce.choice1Num));
			$choice2.setAttribute("data-choice-number", Number(jsonResponce.choice2Num));

			$choice1.innerText = jsonResponce.choice1;
			$choice2.innerText = jsonResponce.choice2;

			console.log(xhttp.responseText);
		}
	};

	xhttp.open("GET", "sendNewPosition.php/?idToGoTo="+_num, true);
	xhttp.send();

}