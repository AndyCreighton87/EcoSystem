using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour {
    [SerializeField] private TMP_Text FPSText;

    [Header("Colours")]
    [SerializeField] private Color highFPSColour = Color.green;
    [SerializeField] private Color mediumFPSColour = Color.yellow;
    [SerializeField] private Color lowFPSColor = Color.red;

    [Header("Values")]
    [SerializeField] private float highFPSValue = 60.0f;
    [SerializeField] private float lowFPSValue = 30.0f;


    private float deltaTime = 0.0f;

    void Update() {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        if (FPSText != null) {
            FPSText.text = string.Format("FPS: {0:0.}", fps);

            if (fps >= highFPSValue) {
                FPSText.color = highFPSColour;
            } else if (fps > lowFPSValue) {
                FPSText.color = lowFPSColor;
            } else {
                FPSText.color = mediumFPSColour;
            }
        }
    }
}