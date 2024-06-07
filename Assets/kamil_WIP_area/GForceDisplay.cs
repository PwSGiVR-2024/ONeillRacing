using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GForceDisplay : MonoBehaviour
{
    
    Rigidbody carRigidbody;
    float timeSinceContact = 100;
    float maxTimeSinceContact = 1f;

    [SerializeField]
    TMP_Text textMesh;

    private void Start()
    {
        textMesh.text = "0g";
        carRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeSinceContact > maxTimeSinceContact)
        {
            textMesh.text = "0g";
        } else {
            textMesh.text = (Mathf.Round(100 * (1 - (timeSinceContact / maxTimeSinceContact)) * (carRigidbody.GetAccumulatedForce().magnitude / carRigidbody.mass) / 9.807f) / 100).ToString() + "g";
            timeSinceContact += Time.fixedDeltaTime;
            print(carRigidbody.GetAccumulatedForce());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        timeSinceContact = 0;
    }
}
