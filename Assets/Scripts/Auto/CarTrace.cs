using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CarTrace : MonoBehaviour
{
    public List<TraceData> traceData;
    public List<TraceData> bestTraceData;
    public int rand = 6;

    GameObject carobj = null;
    float carTime = 0; // wewnetrzny czas auta, zrobiony dla obslugi opoznienia, np. co 0.5 sekundy odczytuje czas z timera

    [SerializeField]
    float readTimedelay = 5f; // x sekund opoznienia // w unity ustawione na 0.1 sekundy 

    private float bestScore = 9999; // ze wzgledu na metode SetBestScore ustawiam go na maximum
    private float currentScore = 0; 

    public void setBestTraceData(List<TraceData>list)
    {
        bestTraceData = new List<TraceData>(list);
    }
    public List<TraceData> getBestTraceData()
    {
        return bestTraceData;
    }
    public void SetScore(float score)
    {
        currentScore = score;
    }

    public bool SetBestScore(float score)
    {
        if (currentScore < bestScore)
        {
            bestScore = currentScore;
            return true;
        }
        return false;
    }

    public void DebugShowScores()
    {
        print("Score : " + currentScore + " BesScore: " + bestScore);
    }

    private void FindCarOject(){
        carobj = GameObject.Find("Car");
    }

    private bool CarExists(){
        if (carobj == null)
            return false;
        else
            return true;
    }

    public void ResetCarTimer()
    {
        carTime = 0;
    }

    public float GetCarTime()
    {
        return carTime;
    }

    public void DebugShowCarTime()
    {
        print("CarTime: " + carTime);
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
                //print("pos: " + pos + " time" + carTime);
                TraceData tracedata = new TraceData(pos, carTime);
                traceData.Add(tracedata);
            }
        }
    }

    public List<TraceData> GetTraceData()
    {
        print("[CarTrace] Robie return traceData");
        return traceData;
    }

    // Start is called before the first frame update
    void Start(){
        traceData = new List<TraceData>();
        FindCarOject();
    }

    public void ClearCarTrace()
    {
        traceData.Clear();
    }

    // Update is called once per frame
    void Update(){
        CarGetTraceTime();
        //print("------------------czas auta playera------------------------\n");
        //string s = "";
        //foreach (TraceData obj in traceData)
        //{
        //    s += (obj.time + " (" + obj.place.x + " " + obj.place.y + " " + obj.place.z + ") \n");
        //}
        //print(s);
    }
}
