using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Transform MyTransform;
    Rigidbody MyRigidbody;
    Vector3 OffcenterPosition;
    [SerializeField]
    float surface_acceleration = 9.807f;
    float object_mass = 1f;
    [SerializeField]
    float surface_distance = 100f;
    // Start is called before the first frame update
    void Start()
    {
        MyTransform = GetComponent<Transform>();
        MyRigidbody = GetComponent<Rigidbody>();
        object_mass = MyRigidbody.mass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Downpull = surface_acceleration * object_mass * DistanceFromRotAxis()/surface_distance;
        Downpull /= Time.deltaTime;
        Vector3 GravityForce = DownDirection() * Downpull;
        MyRigidbody.AddForce(OffcenterPosition);
    }

    Vector3 DownDirection()
    {
        OffcenterPosition = MyTransform.position;
        OffcenterPosition.x = 0;
        return OffcenterPosition.normalized;
    }

    float DistanceFromRotAxis()
    {
        OffcenterPosition = MyTransform.position;
        OffcenterPosition.x = 0;
        return OffcenterPosition.magnitude;
    }
}
