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
    float scaleForces = 1;

    float previousAxisDistance;
    float previousVelocity = 0;
    float previousGforce = 0;
    Vector3 neutralForces;//forces applied by all the functions to keep the body moving in a straight line

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
        neutralForces = Vector3.zero;
        myRigidbody.AddForce(objectMass * FakeCentrifugalAcceleration() * scaleForces);//applies centrifugal force calculated as if the cylinder was rotating
        RotateVelocity();//velocity vector is rotated oposite to rotation of the cylinder, causing the direction of movement change as if it was actually moving in a straight line with the cylinder rotating around it instead
        MaintainTengentialVelocity();//changes velocity of a body moving in a "straight line" so that it does not veer off to the sides - ie. makes it veer off to the side when you assume cylinder is static
        previousVelocity = myRigidbody.velocity.magnitude;
        print(neutralForces + " / " + (myRigidbody.GetAccumulatedForce()/objectMass));
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

    void RotateVelocity()
    {
        Vector3 velocityBefore = myRigidbody.velocity;
        float rot = -Time.fixedDeltaTime * gravParams.GetCylinderAngularVelocity();
        Vector3 inPlaneVelocity = myRigidbody.velocity;

        //now I rotate the y and z of the vector like it's a 2d vector
        float y = inPlaneVelocity.y;
        float z = inPlaneVelocity.z;
        inPlaneVelocity.y = (y * Mathf.Cos(rot)) - (z * Mathf.Sin(rot));
        inPlaneVelocity.z = (inPlaneVelocity.y * Mathf.Sin(rot)) + (inPlaneVelocity.z * Mathf.Cos(rot));

        myRigidbody.velocity = inPlaneVelocity;

        neutralForces += (myRigidbody.velocity - velocityBefore)/Time.fixedDeltaTime;
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

        Vector3 force = staticTangentialVelocity * approachSpeed / -DistanceFromRotAxis();
        //at exactly center of the cylinder this divides by zero, but that's a zero probability scenario
        myRigidbody.AddForce(force*myRigidbody.mass* scaleForces);
        previousAxisDistance = DistanceFromRotAxis();
        neutralForces += force;
    }

    Vector3 FakeCentrifugalAcceleration() {
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

        //print("real vel: " + realTangentalVelocity.magnitude + "m/s , fake vel: " + fakeTangentialVelocity.magnitude + "m/s , cent. acc: " + centrifugalForce.magnitude + "m/s^2 = " + (centrifugalForce.magnitude/9.807) +"g");

        neutralForces += centrifugalForce;
        return centrifugalForce;
    }

    public float GetGForce(int precision=100, int smoothing=0) {
        //float gravityAcceleration = FakeCentrifugalAcceleration().magnitude;
        //float actualAcceleration = Mathf.Abs(myRigidbody.velocity.magnitude - previousVelocity)/Time.fixedDeltaTime;
        float totalAcceleration = (neutralForces - (myRigidbody.GetAccumulatedForce()/myRigidbody.mass)).magnitude;
        float newGForce = totalAcceleration / 9.807f;
        float gForce =  ((previousGforce * smoothing) + (newGForce) )/(smoothing+1);
        previousGforce =  gForce;
        return (float)((int)(precision * gForce)) / precision;
    }
}
