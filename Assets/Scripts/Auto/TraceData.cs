using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceData
{
    Vector3 place;
    float time; 
    public TraceData(Vector3 trace,float time_)
    {
        place = trace; 
        time = time_;
    }
}
