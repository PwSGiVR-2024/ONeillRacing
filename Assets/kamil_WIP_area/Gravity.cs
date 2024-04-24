using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    Transform MyTransform;
    Rigidbody MyRigidbody;
    Vector3 OffcenterPosition;
    [SerializeField]
    float strength = 0.01f;
    float mass = 1f;
    // Start is called before the first frame update
    void Start()
    {
        MyTransform = GetComponent<Transform>();
        MyRigidbody = GetComponent<Rigidbody>();
        mass = MyRigidbody.mass;
    }

    // Update is called once per frame
    void Update()
    {
        OffcenterPosition = MyTransform.position;
        OffcenterPosition.x = 0;
        OffcenterPosition *= strength;
        MyRigidbody.AddForce(OffcenterPosition);
    }
}
