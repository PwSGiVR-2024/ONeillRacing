using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestForceApplicator : MonoBehaviour
{
    [SerializeField]
    float forceConstant;
    [SerializeField]
    float forceInstant;
    // Update is called once per frame

    [SerializeField]
    bool zeroTangentialVelocity;

    Rigidbody myRigidbody;
    Transform myTransform;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myTransform = GetComponent<Transform>();
        if (zeroTangentialVelocity)
        {
            Vector3 offcenterPosition = myTransform.position;
            offcenterPosition.x = 0;
            Vector3 offcenterVelocity = myRigidbody.velocity;
            offcenterVelocity.x = 0;

            myRigidbody.velocity = Vector3.Cross(new Vector3(-1,0,0), offcenterPosition) * GravityParameters.GetInstance().GetCylinderAngularVelocity();
        }
    }
    void FixedUpdate()
    {
        Vector3 offcenterPosition = myTransform.position;
        offcenterPosition.x = 0;
        if (Input.GetKey(KeyCode.W)) { 
            GetComponent<Rigidbody>().AddForce(GetComponent<Transform>().forward*forceConstant);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GetComponent<Rigidbody>().AddForce(offcenterPosition.normalized*-1*forceInstant);
        }
        if (Input.GetKey(KeyCode.X)) {
            myRigidbody.velocity += offcenterPosition.normalized * Time.fixedDeltaTime;
        }
    }
}
