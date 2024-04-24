using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class balmover : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]
    Transform trans;
    [SerializeField]
    Transform cam;
    [SerializeField]
    Transform car;
    [SerializeField]
    float drivestrength = 10;
    [SerializeField]
    float turnstrength = 1;
    [SerializeField]
    bool turning = false;
    [SerializeField]
    bool driving = false;
    [SerializeField]
    Vector3 intended_position;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        intended_position = trans.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(trans.localPosition-intended_position);
        if(driving && Input.GetKey(KeyCode.W))
        {
            rb.AddTorque(cam.right * drivestrength);
        }
        if (driving && Input.GetKey(KeyCode.S))
        {
            rb.AddTorque(-cam.right * drivestrength);
        }
        if (turning && Input.GetKey(KeyCode.D))
        {
            rb.AddTorque(cam.forward * drivestrength);
        }
        if (turning && Input.GetKey(KeyCode.A))
        {
            rb.AddTorque(cam.forward * turnstrength);
        }
    }
}
