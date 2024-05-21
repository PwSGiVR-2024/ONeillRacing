using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Transform myTransform;
    Rigidbody myRigidbody;
    Vector3 offcenterPosition;
    Vector3 offcenterVelocity;
    float objectMass = 1f;

    [SerializeField]
    float gravityFactor = 1;

    [SerializeField]
    bool applyBasicGravity = true;

    [SerializeField]
    bool cancelRealCentrifugalForce = false;

    [SerializeField]
    bool rotateVelocity = true;

    [SerializeField]
    bool applyFakeCentrifugalForce = false;

    GravityParameters gravParams;
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody>();
        objectMass = myRigidbody.mass;
        gravParams = GravityParameters.GetInstance();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gravParams == null) { gravParams = GravityParameters.GetInstance(); }
        float basicDownpull = gravParams.GetSurfaceAcceleration() * objectMass * gravityFactor;
        basicDownpull /= Time.deltaTime;
        Vector3 GravityForce = DownDirection() * basicDownpull;
        if (applyBasicGravity) { myRigidbody.AddForce(offcenterPosition); }
        if (cancelRealCentrifugalForce) { myRigidbody.AddForce(-1* gravityFactor * objectMass*RealCentrifugalAcceleration()); }
        //if (applyFakeCentrifugalForce) { myRigidbody.AddForce(gravityFactor * objectMass * FakeCentrifugalAcceleration()); }
        if (rotateVelocity) { myRigidbody.AddForce(objectMass*TrajectoryTurningForce()); }
        //print(FakeCentrifugalAcceleration().magnitude);
    }

    Vector3 VectorFromRotAxis() {
        offcenterPosition = myTransform.position;
        offcenterPosition.x = 0;
        return offcenterPosition;
    }
    Vector3 DownDirection()
    {
        return VectorFromRotAxis().normalized;
    }

    float DistanceFromRotAxis()
    {
        return VectorFromRotAxis().magnitude;
    }

    void RotateVelocity() {
        Vector3 inPlaneVelocity = myRigidbody.velocity;
        inPlaneVelocity.x = 0;
        Vector3 rotationTarget = Vector3.Cross(inPlaneVelocity, new Vector3(1,0,0));
        Vector3.RotateTowards(inPlaneVelocity, rotationTarget, gravParams.GetCylinderAngularVelocity() * Time.fixedDeltaTime,0);
        inPlaneVelocity.x = myRigidbody.velocity.x;
        myRigidbody.velocity = inPlaneVelocity;
    }

    Vector3 TrajectoryTurningForce()
    {
        Vector3 inPlaneVelocity = myRigidbody.velocity;
        inPlaneVelocity.x = 0;
        Vector3 turner = Vector3.Cross(inPlaneVelocity, new Vector3(1, 0, 0));
        turner *= gravParams.GetCylinderAngularVelocity();
        return turner;
    }

    float RealAngularVelocity() {
        offcenterPosition = myTransform.position;
        offcenterPosition.x = 0;
        offcenterVelocity = myRigidbody.velocity;
        offcenterVelocity.x = 0;
        float angle = Vector3.AngleBetween(offcenterPosition, offcenterVelocity);//as angle between 0 and 180, in degrees
        angle = Mathf.Abs((angle-90) / 90); // as angle between -1 and 1 - gives 0 when perfectly aligned, 1 when perpendicular
        float tangentalVelocity = offcenterVelocity.magnitude * angle;

        float circlePath = Mathf.PI * 2 * offcenterPosition.magnitude; //2*pi*r
        float timeToFullRotation = circlePath / tangentalVelocity; //time to complete a full circle, in seconds
        float angVel = 2*Mathf.PI/timeToFullRotation; //angular velocity in radians per second
        return angVel;
    }

    Vector3 RealCentrifugalAcceleration() {
        Vector3 centAcc = DownDirection() * DistanceFromRotAxis() * RealAngularVelocity() * RealAngularVelocity(); //r*w^2

        return centAcc;
    }

    float FakeAngularVelocity() {
        return 0; //TODO
    }

    Vector3 FakeCentrifugalAcceleration()
    {
        offcenterPosition = myTransform.position;
        offcenterPosition.x = 0;
        offcenterVelocity = myRigidbody.velocity;
        offcenterVelocity.x = 0;
        Vector3 velocityDirectionVector = Vector3.Cross(offcenterVelocity, offcenterPosition).normalized;
        float velocityDirection = velocityDirectionVector.x;//should be -1 when moving one way & 1 when moving the other way
        Vector3 centAcc = DownDirection() * DistanceFromRotAxis() * Mathf.Pow(gravParams.GetCylinderAngularVelocity()+(RealAngularVelocity()*velocityDirection),2);
        return centAcc;
    }
}
