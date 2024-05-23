using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

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

    float previousAxisDistance;

    GravityParameters gravParams;
    void Start()
    {
        myTransform = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody>();
        objectMass = myRigidbody.mass;
        gravParams = GravityParameters.GetInstance();
        previousAxisDistance = DistanceFromRotAxis();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (gravParams == null) { gravParams = GravityParameters.GetInstance(); }
        float basicDownpull = gravParams.GetSurfaceAcceleration() * objectMass * gravityFactor;
        basicDownpull /= Time.deltaTime;
        Vector3 GravityForce = DownDirection() * basicDownpull;
        if (applyBasicGravity) { myRigidbody.AddForce(offcenterPosition); }
        if (cancelRealCentrifugalForce) { myRigidbody.AddForce(-1* gravityFactor * objectMass*RealCentrifugalAcceleration()); }
        if (applyFakeCentrifugalForce) { myRigidbody.AddForce(gravityFactor * objectMass * CentrifugalForce()); }
        if (rotateVelocity) {
            RotateVelocity();
            MaintainTengentialVelocity();
            previousAxisDistance = DistanceFromRotAxis();
        }
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

    Vector3 TrajectoryTurningForce()
    {
        Vector3 inPlaneVelocity = myRigidbody.velocity;
        inPlaneVelocity.x = 0;
        Vector3 turner = Vector3.Cross(inPlaneVelocity, new Vector3(1, 0, 0));
        turner *= gravParams.GetCylinderAngularVelocity();
        return turner;
    }

    void RotateVelocity()
    {
        float rot = -Time.fixedDeltaTime * gravParams.GetCylinderAngularVelocity();
        Vector3 inPlaneVelocity = myRigidbody.velocity;

        //now I rotate the y and z of the vector like it's a 2d vector
        float y = inPlaneVelocity.y;
        float z = inPlaneVelocity.z;
        inPlaneVelocity.y = (y * Mathf.Cos(rot)) - (z * Mathf.Sin(rot));
        inPlaneVelocity.z = (inPlaneVelocity.y * Mathf.Sin(rot)) + (inPlaneVelocity.z * Mathf.Cos(rot));

        myRigidbody.velocity = inPlaneVelocity;
    }

    void MaintainTengentialVelocity()
    {
        //now calculating the tangential velocity that an object static in relation to the ground would have
        //https://en.wikipedia.org/wiki/Tangential_speed
        Vector3 staticTangentialVelocity = Vector3.Cross(DownDirection(), new Vector3(1, 0, 0));
        staticTangentialVelocity *= DistanceFromRotAxis() * gravParams.GetCylinderAngularVelocity();

        float approachSpeed = Vector3.Project(myRigidbody.velocity, -1*DownDirection()).magnitude;
        //now I need a way to tell if it's moving towards the center or away from it, because otherwise it works for upwards movement yet breaks downwards movement
        //for now it's an extremely ugly fix, I just compare the distance now to previous distance
        if (previousAxisDistance < DistanceFromRotAxis()) { approachSpeed *= -1; }

       myRigidbody.AddForce(staticTangentialVelocity * approachSpeed / -DistanceFromRotAxis());
    }

    Vector3 CentrifugalForce() {
        //now calculating the tangential velocity that an object static in relation to the ground would have
        //https://en.wikipedia.org/wiki/Tangential_speed
        Vector3 staticTangentialVelocity = Vector3.Cross(DownDirection(), new Vector3(1, 0, 0));
        staticTangentialVelocity *= DistanceFromRotAxis() * gravParams.GetCylinderAngularVelocity();

        //real velocity of the object in a plane at 90deg to the cylinder's axis of rotation (x axis)
        Vector3 inPlaneVelocity = myRigidbody.velocity;
        inPlaneVelocity.x = 0;

        //tengential velocity of the object around the central axis based on it's real velocity, this part needs fixing
        Vector3 realTangentalVelocity = Vector3.Project(inPlaneVelocity,staticTangentialVelocity);
        //if the vectors are at 90deg to each other, it should result in a vector of 0,0,0

        
        Vector3 fakeTangentialVelocity = realTangentalVelocity - staticTangentialVelocity;
        float fakeAngularVelocity = fakeTangentialVelocity.magnitude/DistanceFromRotAxis();//w=v/r
        Vector3 centrifugalForce = VectorFromRotAxis() * fakeAngularVelocity * fakeAngularVelocity;

        print("real vel: " + realTangentalVelocity.magnitude + "m/s , fake vel: " + fakeTangentialVelocity.magnitude + "m/s , cent. acc: " + centrifugalForce.magnitude + "m/s^2 = " + (centrifugalForce.magnitude/9.807) +"g");

        return centrifugalForce;
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
