using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData {
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

    public int currentPosition = 0;
    enum moveHolder { MOVING, ARRIVED };
    moveHolder movementState;

    private Vector3 whereToMoveTo = Vector3.zero;
    private List<positionScript> positions = new List<positionScript>();

    public float movementSpeed = 10f;
    public float minimumDistance = 10f;

    public Transform getNextRotationHolder;

	void Start () {
        movementState = moveHolder.ARRIVED;

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("positions");
        foreach( GameObject go in gameObjects) {
            // print( positions );
            positions.Add( go.GetComponent<positionScript>() );
        }
	}
	
	void Update () {
        switch (movementState) {
            case moveHolder.ARRIVED:
                waitForMoveInstructions();
                break;
            case moveHolder.MOVING:
                moveCamera();
                break;
        }
    }

    void moveCamera() {
        // print("moving " + whereToMoveTo);
        if (whereToMoveTo != Vector3.zero) {
            transform.position = Vector3.Lerp(transform.position, whereToMoveTo, movementSpeed * Time.deltaTime);
            //Quaternion.LookRotation(whereToMoveTo)
            transform.rotation = Quaternion.Lerp(transform.rotation, getNextRotationHolder.rotation, movementSpeed * Time.deltaTime);
            //transform.rotation *= lookWhere;
        }
        if (Vector3.Distance(transform.position, whereToMoveTo) < minimumDistance) {
            movementState = moveHolder.ARRIVED;
            whereToMoveTo = Vector3.zero;
            print("arrived!");
        }
    }

    void getNextPosition(int _goWhere, string _displayText) {
        print("go Move!" + _goWhere + " " + _displayText);
        currentPosition = _goWhere;
        movementState = moveHolder.MOVING;
        if (whereToMoveTo == Vector3.zero) {
            foreach (positionScript ps in positions) {
                if (_goWhere == ps.id) {
                    whereToMoveTo = ps.transform.position;
                    break;
                }
            }
            if (whereToMoveTo == Vector3.zero) { // catch error where the id is out of range
                print("Error found!");
                whereToMoveTo = positions[0].transform.position; // go to first position
            }
        }
        // set next look rotation
        getNextRotationHolder.position = transform.position;
        getNextRotationHolder.rotation = transform.rotation;
        getNextRotationHolder.LookAt(whereToMoveTo);
    }

    void waitForMoveInstructions() {
        if (Input.GetKey("1")) {
            getPositionFromSever(1);
        } else if (Input.GetKey("0")) {
            getPositionFromSever(0);
        } else if (Input.GetKey("2")) {
            getPositionFromSever(2);
        }

        if (Input.GetKey(KeyCode.A)) {
            StartCoroutine(getPositionFromServer());
        }
    }

    IEnumerator getPositionFromServer() {
        WWW positionRequest = new WWW("http://bennorskov.com/experiments/currentState.php");
        yield return positionRequest;

        if (positionRequest.error != null) {
            print("server request error " + positionRequest.error);
        } else {

            print(positionRequest.text);
            PositionData positionData = JsonUtility.FromJson<PositionData>(positionRequest.text);

            if (currentPosition != positionData.id) {
                getNextPosition(positionData.id, positionData.displayText);
            }
        }
    }

    void getPositionFromSever(int _thisPosition) {
        /*
         * 
         * This would be used if we wanted specfic controls from the native app
         * but you control everything from the website currently.
         * It's a matter of sending a get to sendNewPosition.php
         * 
         */
    }
}
