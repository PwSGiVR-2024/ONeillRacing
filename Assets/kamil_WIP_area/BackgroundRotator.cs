using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotator : MonoBehaviour
{
    Transform myTransform;
    GravityParameters gravParams;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        gravParams = GravityParameters.GetInstance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gravParams == null) { gravParams = GravityParameters.GetInstance(); }
        Vector3 angle = myTransform.eulerAngles;
        angle.z -= gravParams.GetCylinderAngularVelocityDegrees()*Time.fixedDeltaTime;
        myTransform.eulerAngles = angle;
    }
}
