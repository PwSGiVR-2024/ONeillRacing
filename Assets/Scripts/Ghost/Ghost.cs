using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    List<TraceData> trace;
    Transform ghostTransform;

    private IEnumerator ghostRiding; 

    bool isDataSetted = false;

    [SerializeField]
    bool useTeleprot;
    [SerializeField]
    bool useMoveForwardFcn;

    private IEnumerator setDataCorutine(List<TraceData> _trace_)
    {
        trace = new List<TraceData>(_trace_);
        yield return null;
    }

    public void setData(List<TraceData> _trace_) // trasa pobierana po pierwszym przejechaniu mety poprzez niewidoczny obiekt
    {
        StartCoroutine(setDataCorutine(_trace_));
        //print("------------Ghost otrzymal--------------------\n");
        //string s = "";
        //foreach (TraceData obj in trace)
        //{
        //    s += (obj.time + " (" + obj.place.x + " " + obj.place.y + " " + obj.place.z + ") \n");
        //}
        //print(s);
    }

    public void runGhost(){
        StartCoroutine(GhostRiding());
        //Destroy(gameObject);
    }

    private IEnumerator GhostRiding() // run ghost odpalany przez ukryty obiekt na mecie rowniez
    {

        int i = 0;
        float timeFirst = 0;
        float timeNext = 0;
        float waitTime = 0;
        print("[Ghost] rozpoczynam korutyne" + trace);
        //Timer.ResetTimer();
        foreach(TraceData obj in trace) {
            timeNext = obj.time;
            waitTime = timeNext - timeFirst;
            timeFirst = timeNext;
            
            i++;
            //Vector3 position = GetGhostCurentPosition();
            //print("[Ghost]: changing position! to" + position);
            Vector3 posA = gameObject.transform.position;
            Vector3 posB = obj.place;

            float S = Vector3.Distance(posA, posB); // droga
            float T = waitTime; // czas
            float V = S / T; // predkosc
            float step = V * Time.deltaTime * 1000;
            print("Roznica czasu " + waitTime + ", step: " + step + "waitime: "+waitTime+" Droga: " + S + "Predkosc " + V);
            if (useTeleprot) {
                gameObject.transform.position = obj.place; // teleportacja
                gameObject.transform.rotation = obj.roatation;
            }
            else if (useMoveForwardFcn)
            {
                //jednak tez robi tekeoportacje przy tym 
                //float step = waitTime * 5000 * Time.deltaTime;
                transform.position = Vector3.MoveTowards(posA, posB, step);
            }
            yield return new WaitForSeconds(waitTime);
        }

        Destroy(gameObject);
        print("Petla forech z korutyny wykonala sie " + i + " razy");

        
    }

    private bool PositionChanged(Vector3 pos1, Vector3 pos2)
    {
        if (pos1 != pos2)
        {
            return true;
        }
        else return false;
    }

    private Vector3 GetGhostCurentPosition()
    {
        Vector3 possd = gameObject.transform.position;
        //print("[Ghost]: current position: " + possd.x + " " + possd.y + " " + possd.z);
        return gameObject.transform.position;
    }

    void Start()
    {
        trace = new List<TraceData>();
        ghostTransform = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Transform>();
        ghostRiding = GhostRiding();
        useTeleprot = true; ;
        useMoveForwardFcn = false;
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
