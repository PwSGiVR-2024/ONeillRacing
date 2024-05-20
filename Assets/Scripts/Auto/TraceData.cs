using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TraceData
{
    public Vector3 place;
    public float time;
    public Quaternion roatation;

    public TraceData(Vector3 trace,float time_)
    {
        place = trace; 
        time = time_;
    }
    public TraceData(Vector3 trace, float time_, Quaternion rot)
    {
        place = trace;
        time = time_;
        roatation = rot;
    }
    public Vector3 tryGetTraceFromTime(float _time_, Vector3 _place_)
    {
        if (time == _time_)
        {
            return place;
        }
        else {
            Debug.Log("cant get trace from" + _time_);
            return _place_; // w przeciwnym wypadku duch pozostanie w miejscu gdzie stal
            
        }
        
    }
}
