using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GravityParameters : MonoBehaviour
{
    static GravityParameters instance;

    [SerializeField]
    float CylinderRadius = 100f;
    [SerializeField]
    float SurfaceAcceleration = 9.807f;
    float CylinderAngularVelocity;
    void Start()
    {
        GravityParameters.instance = this;
        CylinderAngularVelocity = Mathf.Sqrt(SurfaceAcceleration/CylinderRadius);
    }

    public static GravityParameters GetInstance() {
        return GravityParameters.instance;
    }

    public float GetSurfaceAcceleration() { 
        return SurfaceAcceleration;
    }

    public float GetCylinderRadius() { 
        return CylinderRadius;
    }

    public float GetCylinderAngularVelocity() {
        return CylinderAngularVelocity;
    }
}
