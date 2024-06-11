using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRotator : MonoBehaviour
{
    Transform myTransform;
    GravityParameters gravParams;
    [SerializeField]
    bool x;
    [SerializeField]
    bool y;
    [SerializeField]
    bool z;
    [SerializeField]
    bool invert;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = GetComponent<Transform>();
        gravParams = GravityParameters.GetInstance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (gravParams == null) { gravParams = GravityParameters.GetInstance(); }
        Vector3 angle = new Vector3(0,0,0);

        if (x) {
            angle.x = -gravParams.GetCylinderAngularVelocityDegrees() * Time.fixedDeltaTime;
        }
        if (y) {
            angle.y = -gravParams.GetCylinderAngularVelocityDegrees() * Time.fixedDeltaTime;
        }
        if (z) {
            angle.z = -gravParams.GetCylinderAngularVelocityDegrees() * Time.fixedDeltaTime;
        }
        if(invert) { angle *= -1; }

        myTransform.Rotate(angle);
    }
}
