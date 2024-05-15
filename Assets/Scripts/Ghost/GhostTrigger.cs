using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* GhostTriger
 * 
 * Klasa umieszczana na niewidocznym obiekcie na mecie, 
 * powoduje uaktywnienie ducha, który bêdzie pod¹¿a³ zapisan¹ wczesniej tras¹. 
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
            print("[GhostTrigger]: Nie moge sie dostaæ do objektu o tagu Player");
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
            print("[GhostTrigger]: Nie moge sie dostaæ do objektu o tagu Player");
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

            float timeScore = playerTrace.GetCarTime();
            playerTrace.SetScore(timeScore);
            bool isBestScore = playerTrace.SetBestScore(timeScore);
            if (isBestScore)
            {
                playerTrace.setBestTraceData(traceDataForGhost);
            }

            ghostobj_ghost.setData(playerTrace.getBestTraceData()); // ghost odzwierciedla tylko najlepsza trase
            
            playerTrace.ClearCarTrace();
            Timer.ResetTimer();
            
            playerTrace.DebugShowScores();
            playerTrace.DebugShowCarTime();
            playerTrace.ResetCarTimer();
            ghostobj_ghost.runGhost();
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
