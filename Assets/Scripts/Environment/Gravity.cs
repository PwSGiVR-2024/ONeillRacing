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
    Vector3 centrifugalAcceleration = Vector3.zero;

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
        centrifugalAcceleration = FakeCentrifugalAcceleration();
        myRigidbody.AddForce(objectMass * centrifugalAcceleration);//applies centrifugal force calculated as if the cylinder was rotating
        RotateVelocity();//velocity vector is rotated oposite to rotation of the cylinder, causing the direction of movement change as if it was actually moving in a straight line with the cylinder rotating around it instead
        MaintainTengentialVelocity();//changes velocity of a body moving in a "straight line" so that it does not veer off to the sides - ie. makes it veer off to the side when you assume cylinder is static
    }

    Vector3 VectorFromRotAxis() {
        //returns vector from the closest point on cylinder's axis of rotation to the object's location, ie. radius but in vector form
        offcenterPosition = myTransform.position;
        offcenterPosition.x = 0;
        return offcenterPosition;
    }
    Vector3 DownDirection()
    {
        //returns vector of magnitude 1 pointing away from the axis of rotation, towards the "ground" on which the cars drive
        return VectorFromRotAxis().normalized;
    }

    float DistanceFromRotAxis()
    {
        //returns distance from the axis of rotation
        return VectorFromRotAxis().magnitude;
    }

    void RotateVelocity()
    {
        //changes the direction in which the body is moving as if the cylinder was rotating
        float rot = -Time.fixedDeltaTime * gravParams.GetCylinderAngularVelocity();//angle by which trajectory will change
        Vector3 inPlaneVelocity = myRigidbody.velocity;

        //I rotate the y and z of the vector like it's a 2d vector
        float y = inPlaneVelocity.y;
        float z = inPlaneVelocity.z;
        inPlaneVelocity.y = (y * Mathf.Cos(rot)) - (z * Mathf.Sin(rot));
        inPlaneVelocity.z = (inPlaneVelocity.y * Mathf.Sin(rot)) + (inPlaneVelocity.z * Mathf.Cos(rot));

        myRigidbody.velocity = inPlaneVelocity;
    }

    void MaintainTengentialVelocity()
    {
        //makes objects move in straight lines when seen by a camera rotating such that the cylinder appears to rotate at it's set angular velocity

        //calculating the tangential velocity that an object static in relation to the ground would have
        //https://en.wikipedia.org/wiki/Tangential_speed
        Vector3 staticTangentialVelocity = Vector3.Cross(DownDirection(), new Vector3(1, 0, 0));
        staticTangentialVelocity *= DistanceFromRotAxis() * gravParams.GetCylinderAngularVelocity();

        float approachSpeed = Vector3.Project(myRigidbody.velocity, -1*DownDirection()).magnitude;
        if (previousAxisDistance < DistanceFromRotAxis()) { approachSpeed *= -1; }

        Vector3 force = Vector3.zero;
        if (DistanceFromRotAxis() != 0)
        {
            force = staticTangentialVelocity * approachSpeed / -DistanceFromRotAxis();
        }
        myRigidbody.AddForce(force*myRigidbody.mass);
        previousAxisDistance = DistanceFromRotAxis();
    }

    Vector3 FakeCentrifugalAcceleration() {
        //responsible for centrifugal force that both plays part in making objects move in apparently straight lines and also provides gravity

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
        float fakeAngularVelocity = fakeTangentialVelocity.magnitude/DistanceFromRotAxis();
        Vector3 centrifugalForce = VectorFromRotAxis() * fakeAngularVelocity * fakeAngularVelocity;

        return centrifugalForce;
    }

    public Vector3 GetCentrifugalAcceleration() {
        return centrifugalAcceleration;
    }
}
