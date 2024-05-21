using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestForceApplicator : MonoBehaviour
{
    [SerializeField]
    float forceConstant;
    [SerializeField]
    float forceInstant;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) { 
            GetComponent<Rigidbody>().AddForce(GetComponent<Transform>().forward*forceConstant);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<Rigidbody>().AddForce(GetComponent<Transform>().up*forceInstant);
        }
    }
}
