using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData {
    // JSON object class
    // this is a mirror of the data from the server
    // it comes in as a jSon object with these key values:
    public int id;
    public string displayText;
    public string choice1;
    public string choice2;
    public int choice1Num;
    public int choice2Num;
}

public class cameraController : MonoBehaviour {

    // Where are we?
    public int currentPosition = 0;

    // Are we moving?
    enum moveHolder { MOVING, ARRIVED };
    moveHolder movementState;

    // Where are we going?
    private Vector3 whereToMoveTo = Vector3.zero;
    private List<positionScript> positions = new List<positionScript>();

    // How do we get there?
    public float movementSpeed = 10f;
    public float minimumDistance = 10f;

    // Used to get a transform.LookAt function without modifying the camera's tranform
    // turns out transforms are pass by reference
    // AND they're protected, so you can't dynamically create one
    // Thanks, Unity
    public Transform getNextRotationHolder;

    // When do we ask the server for new info?
    private float askTimer = 1.0f;
    private float lastTime = 0f;
    private bool coRoutineRunning = false;

    // ————— ————— ————— ————— 

	void Start () {
        // Begin not moving
        movementState = moveHolder.ARRIVED;

        // Grab where to go
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("positions");
        foreach( GameObject go in gameObjects) {
            // print( positions );
            positions.Add( go.GetComponent<positionScript>() );
        }

        // Initial Server Request
        lastTime = Time.time;
        StartCoroutine(getPositionFromServer());
	}
	
    // ————— ————— ————— ————— 

    void Update () {
        // ————— State Machine: Moving or not. Could add more states...
        switch (movementState) {
            case moveHolder.ARRIVED:
                waitForMoveInstructions();
                break;
            case moveHolder.MOVING:
                moveCamera();
                break;
        }
    }

    // —————
    // ————— If we need to move, use this function to fly to the next position
    // —————
    void moveCamera() {
        // print("moving " + whereToMoveTo);
        if (whereToMoveTo != Vector3.zero) {
            transform.position = Vector3.Lerp(transform.position, whereToMoveTo, movementSpeed * Time.deltaTime);
            //Quaternion.LookRotation(whereToMoveTo)
            transform.rotation = Quaternion.Lerp(transform.rotation, getNextRotationHolder.rotation, movementSpeed * Time.deltaTime);
            //transform.rotation *= lookWhere;
        }
        // ————— stop if you're close enough
        if (Vector3.Distance(transform.position, whereToMoveTo) < minimumDistance) {
            movementState = moveHolder.ARRIVED;
            whereToMoveTo = Vector3.zero;
            // print("arrived!");
        }
    }

    // —————
    // ————— Figure out which position to go to next.
    // —————
    void getNextPosition(int _goWhere, string _displayText) {
        // print("go Move!" + _goWhere + " " + _displayText);

        // Update State Machine
        currentPosition = _goWhere;
        movementState = moveHolder.MOVING;

        // ————— Cycle through positions until we found the next one
        if (whereToMoveTo == Vector3.zero) {
            foreach (positionScript ps in positions) {
                if (_goWhere == ps.id) {
                    whereToMoveTo = ps.transform.position;
                    ps.changeDisplayText(_displayText);
                    break;
                }
            }
            if (whereToMoveTo == Vector3.zero) { // catch error where the id is out of range
                print("Error found!");
                whereToMoveTo = positions[0].transform.position; // go to first position as fallback
            }
        }
        // ————— set next look rotation
        getNextRotationHolder.position = transform.position;
        getNextRotationHolder.rotation = transform.rotation;
        getNextRotationHolder.LookAt(whereToMoveTo);
    }
    // —————
    // ————— Main loop to wait for next instructions from server. 
    // —————
    void waitForMoveInstructions() { // movementState.ARRIVED

        // this was debug code, but could be integrated back in
        /*
        if (Input.GetKey("1")) {
            getPositionFromSever(1);
        } else if (Input.GetKey("0")) {
            getPositionFromSever(0);
        } else if (Input.GetKey("2")) {
            getPositionFromSever(2);
        }
        */

        // if we don't have an open request (coRoutineRunning) 
        // && it's been askTimer amount of time since last request, make a request
        if (!coRoutineRunning && Time.time > lastTime + askTimer) {
            print("get new positions");
            lastTime = Time.time;
            StartCoroutine(getPositionFromServer());
        }
    }
    // —————
    // ————— Send web request to get json of active object
    // —————
    IEnumerator getPositionFromServer() {
        coRoutineRunning = true;
        // php page that spits out a json object
        WWW positionRequest = new WWW("http://bennorskov.com/experiments/currentState.php");
        yield return positionRequest;

        if (positionRequest.error != null) {
            print("server request error " + positionRequest.error);
        } else {

            print(positionRequest.text);
            PositionData positionData = JsonUtility.FromJson<PositionData>(positionRequest.text);

            if (currentPosition != positionData.id) {
                turnOffTextDisplays();
                getNextPosition(positionData.id, positionData.displayText);
            }
        }
        coRoutineRunning = false;
    }

    void turnOffTextDisplays() {
        foreach (positionScript ps in positions) {
            ps.hideDisplayText();
        }
    }

    void getPositionFromSever(int _thisPosition) {
        /*
         * 
         * This would be used if we wanted specfic controls from the native app
         * but you control everything from the website as it now stands.
         * It's a matter of sending a _GET to sendNewPosition.php
         * 
         * 
         */
    }
}
