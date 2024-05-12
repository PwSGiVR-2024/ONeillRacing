using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrace : MonoBehaviour
{
    List<TraceData> traceData;

    GameObject carobj = null;
    float carTime = 0; // wewnetrzny czas auta, zrobiony dla obslugi opoznienia, np. co 0.5 sekundy odczytuje czas z timera

    [SerializeField]
    float readTimedelay = 0.5f; // x sekund opoznienia

    private void FindCarOject(){
        carobj = GameObject.Find("Car");
    }

    private bool CarExists(){
        if (carobj == null)
            return false;
        else
            return true;
    }

    private bool checkTimeAndDelay(float timerTime, float carTime){
        if (timerTime >= carTime + readTimedelay)
            return true;
        
        return false;
    }

    private void CarGetTraceTime(){
        float currentTime = Timer.GetTime();
        bool timeToReadTimer = checkTimeAndDelay(currentTime, carTime);

        if (timeToReadTimer){

            carTime = currentTime;    
            
            if (CarExists()){
                Vector3 pos = carobj.transform.position;
                print("pos: " + pos + " time" + carTime);
                TraceData tracedata = new TraceData(pos, carTime);
                traceData.Add(tracedata);
            }
        }
    }

    // Start is called before the first frame update
    void Start(){
        traceData = new List<TraceData>();
        FindCarOject();
    }

    // Update is called once per frame
    void Update(){
        CarGetTraceTime();
    }
}
