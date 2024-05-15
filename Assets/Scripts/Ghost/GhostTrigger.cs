using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GhostTriger
 * 
 * Klasa umieszczana na niewidocznym obiekcie na mecie, 
 * powoduje uaktywnienie ducha, kt�ry b�dzie pod��a� zapisan� wczesniej tras�. 
 * 
 */

public class GhostTrigger : MonoBehaviour
{

    Ghost ghost;
    GameObject player;
    bool isPlayer = false;
    bool isGhost = false; 
    CarTrace playerTrace;
    public GameObject ghostobj; 

    [SerializeField]
    public GameObject myPrefab;

    void tryGetPlayer()
    {
        try
        {
            player = GameObject.Find("Car"); //GameObject.FindGameObjectWithTag("Player");
            isPlayer = true;
        } catch (Exception e)
        {
            print("[GhostTrigger]: Nie moge sie dosta� do objektu o tagu Player");
        }
    }

    void tryGetGhost()
    {
        try
        {
            ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
            isGhost = true;
        }
        catch (Exception e)
        {
            print("[GhostTrigger]: Nie moge sie dosta� do objektu o tagu Player");
        }
    }


    public void OnTriggerExit(Collider colider)
    {
        print(colider.gameObject.tag + " " + colider.name);

        if (colider.CompareTag("Player")) //colider.gameObject.tag == "Player"
        {
            if (isPlayer)
            {
                playerTrace = player.GetComponent<CarTrace>();
                if (playerTrace != null){
                    print(playerTrace.rand);
                }
                else
                {
                    Debug.LogWarning("Brak skryptu CarTrace na obiekcie: " + colider.name);
                    return;
                }
                //gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            //player = colider.gameObject;
            //ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();

            playerTrace = player.GetComponent<CarTrace>();

            // print(playerTrace.rand);

            List<TraceData> traceDataForGhost = playerTrace.GetTraceData() ;

            //print("--------------------------------------------------------\n");
            //string s = "";
            //foreach (TraceData obj in traceDataForGhost)
            //{
            //    s += (obj.time + " (" + obj.place.x + " " + obj.place.y + " " + obj.place.z + ") \n");
            //}
            //print(s);

            ghostobj = Instantiate(myPrefab, player.GetComponent<Transform>().position, Quaternion.identity);
            Ghost ghostobj_ghost = ghostobj.GetComponent<Ghost>();

            ghostobj_ghost.setData(traceDataForGhost);
            playerTrace.ClearCarTrace();
            Timer.ResetTimer();
            playerTrace.ResetCarTimer();
            ghostobj_ghost.runGhost();

            

            print("Zrobi��m run ghost!");

           //traceDataForGhost.Clear();
            //traceDataForGhost = new List<TraceData>();
        }
        else
        {
            print("[GhostTrigger]: Nie widze colidera");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tryGetPlayer();
        tryGetGhost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
