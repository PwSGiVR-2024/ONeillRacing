using System;
using System.Collections;
using System.Collections.Generic;
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
        trace = _trace_;
        print("Ghost otrzymal " + trace);

        print("------------t ootrzxymal ghost--------------------\n");
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
        print("[Ghost] rozpoczynam korutyne" + trace);
        Timer.ResetTimer();
        foreach(TraceData obj in trace)
        {
            i++;

            Vector3 position = GetGhostCurentPosition();
            //float time = Timer.GetTime();
            //Vector3 newPosition = obj.tryGetTraceFromTime(time,position);
            
            print("[Ghost] patrze na wartosci");
           // if (PositionChanged(position, newPosition))
           // {
                print("[Ghost]: changing position! to" + position);
            gameObject.transform.position = obj.place ;
            //}
            yield return new WaitForSeconds(0.1f);
        }

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
        print("[Ghost]: current position: " + possd.x + " " + possd.y + " " + possd.z);
        return gameObject.transform.position;
    }

    void Start()
    {
        ghostTransform = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Transform>();
        ghostRiding = GhostRiding();
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
