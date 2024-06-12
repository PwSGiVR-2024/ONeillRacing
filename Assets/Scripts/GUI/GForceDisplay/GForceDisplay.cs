using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class GForceDisplay : MonoBehaviour
{
    
    Gravity carGravity;
    float timeSinceContact = 100;
    float maxTimeSinceContact = 1f;
    float gForce = 0;

    [SerializeField]
    TMP_Text textMesh;

    [SerializeField]
    float gLocThreshold = 4;//g-force at which g-LOC starts to happen

    [SerializeField]
    float gLocInnertia = 10;//how long g-LOC takes to set in and clear up

    float accumulatedGLoc = 0;

    [SerializeField]
    GameObject carCamera;

    [SerializeField]
    PostProcessProfile pPProfile;
    Vignette vignette;
    ColorGrading colorGrading;


    private void Start()
    {
        textMesh.text = "0g";
        carGravity = GetComponent<Gravity>();
        pPProfile.TryGetSettings<Vignette>(out vignette);
        pPProfile.TryGetSettings<ColorGrading>(out colorGrading);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceContact > maxTimeSinceContact)
        {
            textMesh.text = "0g";
        } else {
            gForce = carGravity.GetCentrifugalAcceleration().magnitude / 9.807f;
            textMesh.text = (Mathf.Round(100 * (1 - (timeSinceContact / maxTimeSinceContact)) * gForce ) / 100).ToString() + "g";
            timeSinceContact += Time.deltaTime;
        }

        if (gForce > gLocThreshold && accumulatedGLoc < 1)
        {
            accumulatedGLoc += (Time.deltaTime * (gForce/gLocThreshold))/gLocInnertia;

        }

        if (accumulatedGLoc > 0)
        {
            accumulatedGLoc -= Time.deltaTime * Mathf.Max(3*accumulatedGLoc,1) / gLocInnertia;
        }

        if (accumulatedGLoc < 0) { 
            accumulatedGLoc = 0;
        }

        vignette.intensity.Override(accumulatedGLoc);
        colorGrading.saturation.Override(-accumulatedGLoc*100);

        print(accumulatedGLoc);
    }

    private void OnTriggerStay(Collider other)
    {
        timeSinceContact = 0;
    }
}
