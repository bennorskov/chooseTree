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

        displayContainer.color = Color.clear;
        displayContainer.text = _displayText;
    }
    public void startTextFadeIn() {
        StartCoroutine(fadeInText());
    }
    IEnumerator fadeInText() {
        float lerpAmount = 0f;

        while (lerpAmount < .95) {
            lerpAmount += Time.deltaTime * .5f;
            displayContainer.color = Color.Lerp(Color.clear, Color.white, lerpAmount);
            yield return null;
        }

        displayContainer.color = Color.white;
    }
    public void hideDisplayText() {
        displayContainer.text = "";
    }
}
