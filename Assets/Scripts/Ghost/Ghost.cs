using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Ghost : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    List<TraceData> trace;
    Transform ghostTransform;

    private IEnumerator ghostRiding; 

    bool isDataSetted = false;

    public void setData(List<TraceData> _trace_) // trasa pobierana po pierwszym przejechaniu mety poprzez niewidoczny obiekt
    {
        trace = new List<TraceData>(_trace_);
        print("------------Ghost otrzymal--------------------\n");
        string s = "";
        foreach (TraceData obj in trace)
        {
            s += (obj.time + " (" + obj.place.x + " " + obj.place.y + " " + obj.place.z + ") \n");
        }
        print(s);

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
        foreach(TraceData obj in trace)
        {
            timeNext = obj.time;
            waitTime = timeNext - timeFirst;
            timeFirst = timeNext;
            print("Roznica czasu " + waitTime);
            i++;
            //Vector3 position = GetGhostCurentPosition();
            //print("[Ghost]: changing position! to" + position);
            Vector3 posA = gameObject.transform.position;
            Vector3 posB = obj.place;
            gameObject.transform.position = obj.place ; // teleportacja
            gameObject.transform.rotation = obj.roatation;

            // jednak tez robi tekeoportacje przy tym 
            //float step = waitTime * 1000 * Time.deltaTime;
            //transform.position = Vector3.MoveTowards(posA, posB, step);

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
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
