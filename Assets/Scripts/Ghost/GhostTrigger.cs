using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* GhostTriger
 * 
 * Klasa umieszczana na niewidocznym obiekcie na mecie, 
 * powoduje uaktywnienie ducha, który bêdzie pod¹¿a³ zapisan¹ wczesniej tras¹. 
 * 
 */

public class GhostTrigger : MonoBehaviour
{

    //Ghost ghost;
    GameObject player;
    bool isPlayer = false;
    bool isGhost = false;
    
    CarTrace playerTrace;
    public GameObject ghostGameObject; 

    [SerializeField]
    public GameObject myPrefab; // ghost car prefab

    [SerializeField]
    bool logTraceInfo;

    //[SerializeField]
    //public bool useTeleprot;

    void tryGetPlayer()
    {
        try
        {
            player = GameObject.FindGameObjectWithTag("Player"); //GameObject.FindGameObjectWithTag("Player");
            isPlayer = true;
        } catch (Exception e)
        {
            print($"[GhostTrigger]: Nie moge sie dostaæ do objektu o tagu Player \n {e}");
        }
    }

    //void tryGetGhost()
    //{
    //    try
    //    {
    //        ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();
    //        isGhost = true;
    //    }
    //    catch (Exception e)
    //    {
    //        print("[GhostTrigger]: Nie moge sie dostaæ do objektu o tagu Ghost");
    //    }
    //}

    CarTrace tryGetPlayerTrace()
    {
        try
        {
            CarTrace trace = player.GetComponent<CarTrace>();
            return trace;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[GhostTrigger.cs -> tryGetPlayerTrace() ] Samochód nie ma komponentu CarTrace na œobie!\n{e}");
            return null;
        }
    }

    private IEnumerator spawnGhost(Collider colider)
    {
        print(colider.gameObject.tag + " " + colider.name);

        if (colider.CompareTag("Player")) //colider.gameObject.tag == "Player"
        {
            if (isPlayer) // TODO zrobic to w try catch 
            {
                playerTrace = tryGetPlayerTrace();
                if (playerTrace == null)
                {
                    Debug.LogWarning("Brak skryptu CarTrace na obiekcie: " + colider.name);
                    StopCoroutine(spawnGhost(colider));
                }
            }

            List<TraceData> traceDataForGhost = playerTrace.GetTraceData();

            float timeScore = playerTrace.GetCarTime();
            playerTrace.SetScore(timeScore);
            
            bool isBestScore = playerTrace.SetBestScore(timeScore);
            
            if (isBestScore)
            {
                playerTrace.setBestTraceData(traceDataForGhost);
            }

            List<TraceData> bestTrace = playerTrace.getBestTraceData();

            Vector3 spawnLocation = bestTrace.First().place;  // wczesniej player.GetComponent<Transform>().position
            Quaternion spawnRotation = bestTrace.First().roatation; // wczescniej Quaternion.identity

            try
            {
                ghostGameObject = Instantiate(myPrefab, spawnLocation, spawnRotation);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[GhosTrigger.cs -> spawnGhost() ] Nie moge zainstancjonowaæ prefabu Ghosta, moze nie jest ustawiony?\n{e}");
                StopCoroutine(spawnGhost(colider));
            }

            Ghost ghost = ghostGameObject.GetComponent<Ghost>();

            ghost.setData(bestTrace); // ghost odzwierciedla tylko najlepsza trase

            if (logTraceInfo) ghost.showGhostTraceInfo = true;

            ghost.setWaitTime(playerTrace.getWaitTime());

            //if (useTeleprot)
            //{
            //    ghost.useTeleprot = true;
            //    ghost.useMoveForwardFcn = false;
            //}
            //else
            //{
            //    ghost.useTeleprot = false;
            //    ghost.useMoveForwardFcn = true;
            //}
                

            playerTrace.ClearCarTrace();
            Timer.ResetTimer();

            playerTrace.DebugShowScores();
            playerTrace.DebugShowCarTime();
            playerTrace.ResetCarTimer();
            ghost.runGhost();
        }
        else
        {
            print("[GhostTrigger]: Nie widze colidera");
        }
        yield return null;
    }

    public void OnTriggerExit(Collider colider)
    {
        if (colider.CompareTag("Player"))
        {
            StartCoroutine(spawnGhost(colider));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tryGetPlayer();
        //tryGetGhost();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//player = colider.gameObject;
//ghost = GameObject.FindGameObjectWithTag("Ghost").GetComponent<Ghost>();

//playerTrace = player.GetComponent<CarTrace>();

// print(playerTrace.rand);
//print("--------------------------------------------------------\n");
//string s = "";
//foreach (TraceData obj in traceDataForGhost)
//{
//    s += (obj.time + " (" + obj.place.x + " " + obj.place.y + " " + obj.place.z + ") \n");
//}
//print(s);