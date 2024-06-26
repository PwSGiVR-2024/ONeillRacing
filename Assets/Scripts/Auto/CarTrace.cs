using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using UnityEngine.Rendering;
//using UnityEngine.SocialPlatforms.Impl;
using System;
using UnityStandardAssets.Cameras;
//using UnityEngine.UIElements;
//using UnityEngine.Experimental.AI;

public class CarTrace : MonoBehaviour
{
    private List<TraceData> traceData;
    private List<TraceData> bestTraceData;
    private bool debugOn = false;
    private int points = 0;

    Rigidbody carRigidBody; 

    GameObject carobj = null;
    float carTime = 0.0f; // wewnetrzny czas auta, zrobiony dla obslugi opoznienia, np. co 0.5 sekundy odczytuje czas z timera
    float lastTime = 0.0f; // czas do korutyny bo timer z canvasa raz dodaje 0.01 a raz 0.02 do czasu  

    [SerializeField]
    bool showCarTraceDataFlow;
    
    [SerializeField]
    bool showCarTimeAndTimerTime;

    [SerializeField]
    float readTimedelay = 5f; // x sekund opoznienia // w unity ustawione na 0.1 sekundy 

    private float bestScore = 9999; // ze wzgledu na metode SetBestScore ustawiam go na maximum
    private float currentScore = 0;

    private int loopIterator = 0; // do debugowania pokazuje id zapisana do listy 

    public int getPoints()
    {
        return points;
    }
    public void setPoints(int value)
    {
        points = value; 
    }
    public void incrementPoints()
    {
        points += 1;
    }

    public float getWaitTime()
    {
        return readTimedelay;
    }

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
        if (debugOn) print("Score : " + currentScore + " BesScore: " + bestScore);
    }

    private void FindCarOject(){
        carobj = GameObject.FindGameObjectWithTag("Player");
    }

    Rigidbody getCarRigidBody()
    {
        try
        {
            Rigidbody rb = carobj.GetComponent<Rigidbody>();
            return rb; 
        } catch (Exception e)
        {
            Debug.LogWarning($"[CarTrace]: Samochod nie ma na sobie Rigidbody? \n {e}");
            return null;
        }
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
        lastTime = 0;
    }

    public float GetCarTime()
    {
        return carTime;
    }

    public void DebugShowCarTime()
    {
        if (debugOn) print("CarTime: " + carTime*1.01 + " Timer time: " + Timer.GetTime());
    }

    private bool checkTimeAndDelay(float timerTime, float carTime){
        if (timerTime >= carTime + readTimedelay)
            return true;
        
        return false;
    }

    private IEnumerator ReadTraceCorutine()
    {
        while (true)
        {
            carTime = (float)Math.Round(carTime, 2);
            carTime += 0.010f;//(float) Math.Round(readTimedelay,2);
            CarGetTraceTime();
            yield return new WaitForSeconds(0.010f); //readTimedelay
        }
    }

    private void CarGetTraceTime(){

        float currentTime = carTime;
        bool timeToReadTimer = checkTimeAndDelay(carTime,lastTime);

        if (timeToReadTimer){

            lastTime = carTime;
            //carTime = currentTime;    
            
            if (CarExists()){
                Vector3 pos = carobj.transform.position;
                Quaternion rot = carobj.transform.rotation;
                Vector3 velocity = carRigidBody.velocity;
                Vector3 rotationVelocity = carRigidBody.angularVelocity;
                   
                if (showCarTraceDataFlow) print("pozycja nr:" + loopIterator + "[CarTrace] pos: " + pos + " carTime " + carTime + " rot: " + rot + " veolocity: "+velocity + " predkosc katowa rotacji: "+ rotationVelocity);
                if (showCarTimeAndTimerTime) DebugShowCarTime();
                TraceData tracedata = new TraceData(pos, carTime,rot,velocity,rotationVelocity);
                traceData.Add(tracedata);
                loopIterator += 1;
            }
        }
    }

    public List<TraceData> GetTraceData()
    {
        if(debugOn) print("[CarTrace] Robie return traceData");
        return traceData;
    }

    // Start is called before the first frame update
    void Start(){
        traceData = new List<TraceData>();
        FindCarOject();
        carRigidBody = getCarRigidBody();
        StartCoroutine(ReadTraceCorutine());
    }

    public void ClearCarTrace()
    {
        traceData.Clear();
    }

    

    // Update is called once per frame
    void FixedUpdate(){
        
        //print("------------------czas auta playera------------------------\n");
        //string s = "";
        //foreach (TraceData obj in traceData)
        //{
        //    s += (obj.time + " (" + obj.place.x + " " + obj.place.y + " " + obj.place.z + ") \n");
        //}
        //print(s);
    }
}
