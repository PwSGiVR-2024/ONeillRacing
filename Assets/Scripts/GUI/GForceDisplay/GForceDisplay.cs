using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GForceDisplay : MonoBehaviour
{
    
    Gravity carGravity;
    float timeSinceContact = 100;
    float maxTimeSinceContact = 1f;
    float gForce = 0;
    [SerializeField]
    float minGForce = 0.29f;

    [SerializeField]
    TMP_Text textMesh;

    private void Start()
    {
        textMesh.text = "0g";
        carGravity = GetComponent<Gravity>();
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
            timeSinceContact += Time.fixedDeltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        timeSinceContact = 0;
    }
}
