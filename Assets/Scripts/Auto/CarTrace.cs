using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrace : MonoBehaviour
{
    List<TraceData> traceData;

    GameObject carobj = null;


    private void FindCarOject()
    {
        carobj = GameObject.Find("Car");
    }

    private bool CarExists()
    {
        if (carobj == null)
            return false;
        else
            return true;
    }

    // #TODO dodac zczytywanie czasu z obiektu Timera z Canvasa poprzez delegate

    private void CarGetTraceTime()
    {
        if (CarExists())
        {
            Vector3 pos = carobj.transform.position;             //print(pos);
            float time = 0;
            TraceData tracedata = new TraceData(pos, time);
            traceData.Add(tracedata);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        traceData = new List<TraceData>();
        FindCarOject();
    }

    // Update is called once per frame
    void Update()
    {
        CarGetTraceTime();
    }
}
