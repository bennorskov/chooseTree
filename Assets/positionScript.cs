using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class positionScript : MonoBehaviour {

    public int id = 0;
    private TextMesh displayContainer;
    private void Start() {
        displayContainer = GetComponentInChildren<TextMesh>(true);
    }

    public void changeDisplayText(string _displayText) {
        displayContainer.text = _displayText;
    }
    public void hideDisplayText() {
        displayContainer.text = "";
    }
}
