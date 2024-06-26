using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

/* GhostTriger
 * 
 * Klasa umieszczana na niewidocznym obiekcie na mecie, 
 * powoduje uaktywnienie ducha, który bêdzie pod¹¿a³ zapisan¹ wczesniej tras¹. 
 * 
 */

public class GhostTrigger : MonoBehaviour
{

    private CarTrace playerTrace;
    private GameObject player;

    [SerializeField]
    public GameObject myPrefab; // ghost car prefab

    [SerializeField]
    bool logTraceInfo;

    private GameObject ghostGameObject;
    private int totalPoints;
    private bool debugOn = false;
    private bool isPlayer = false;

    Checkpoint[] checkpoitns;

    [SerializeField]
    float spawnPositionX = 0;
    [SerializeField]
    float spawnPositionY = 0;
    [SerializeField]
    float spawnPositionZ = 0;

    [SerializeField]
    float spawnRotationX = 0;
    [SerializeField]
    float spawnRotationY = 0;
    [SerializeField]
    float spawnRotationZ = 0;

    Vector3 spawnPosition0;
    Quaternion spawnRotation0; 

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

        if (colider.CompareTag("Player") || colider.name == "GravityCar") //colider.gameObject.tag == "Player"
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

            int playerPoints = playerTrace.getPoints();

            //if (debugOn) print($"Mam {playerPoints} / {totalPoints} punktow");

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
                ghostGameObject = Instantiate(myPrefab, spawnPosition0, spawnRotation0);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[GhosTrigger.cs -> spawnGhost() ] Nie moge zainstancjonowaæ prefabu Ghosta, moze nie jest ustawiony?\n{e}");
                StopCoroutine(spawnGhost(colider));
            }
            
            colider.transform.position = spawnPosition0; // jak sie uda zaladowac obiekt ducha to gracza teleportuje z powrotem do bazy i razem z duchem zaczyna wyscig ponownie
            colider.transform.rotation = Quaternion.Euler(0,90,0);
            colider.GetComponent<Rigidbody>().velocity = Vector3.zero;

            Ghost ghost = ghostGameObject.GetComponent<Ghost>();

            ghost.setData(bestTrace); // ghost odzwierciedla tylko najlepsza trase

            //if (logTraceInfo) ghost.showGhostTraceInfo = true;

            ghost.setWaitTime(playerTrace.getWaitTime());

            playerTrace.ClearCarTrace();
            Timer.ResetTimer();

            playerTrace.DebugShowScores();
            playerTrace.DebugShowCarTime();
            playerTrace.ResetCarTimer();
            ghost.runGhost();
        }
        else
        {
            Debug.LogWarning("[GhostTrigger]: Nie widze colidera");
        }
        yield return null;
    }

    public void getQuantityOfCheckpoints()
    {
        try
        {
            checkpoitns = GameObject.FindObjectsOfType<Checkpoint>();
            totalPoints = checkpoitns.Length;
            if (debugOn) print($"Wczytane pointy {totalPoints}");
        } catch (Exception e)
        {
            Debug.LogError("[GhostTrigger] nie moge odczytac ilosci checkpointow ze sceny");
            totalPoints = 999;
        }
    }
    public void OnTriggerExit(Collider colider)
    {
        if (colider.CompareTag("Player"))
        {
            try
            {
                CarTrace carTrace = colider.GetComponent<CarTrace>();
                int playerPoints = carTrace.getPoints();
                
                if (playerPoints  == totalPoints)
                {
                    uncheckAllCheckpoints();
                    carTrace.setPoints(0); // od nowa bedziemy zbierac punkty
                    StartCoroutine(spawnGhost(colider));
                    Rigidbody rb = colider.GetComponent<Rigidbody>();
                    rb.velocity = new Vector3(0, 0, 0);

                } // jednak dodawanie punktow dam do checkpointa to wtedy on wie np. gracz juz przez niego przejechal
                //else
               // {
                   // carTrace.points += 1; // przejechanie pod slupkiem to +1 
                //}
                if (debugOn) print($"Mam {playerPoints} / {totalPoints} punktow");

            } catch (Exception e)
            {
                Debug.LogWarning("[GhostTrigger] Nie moglem odczytac CarTrace z komponentu gracza");
            }
        }
    }

    private void setSpawnPosition()
    {
        spawnPosition0 = new Vector3(spawnPositionX,spawnPositionY,spawnPositionZ);
    }
    private void setSpawnRotation()
    {
        spawnRotation0 = new Quaternion(spawnRotationX, spawnRotationY, spawnRotationZ,0);
    }

    private void uncheckAllCheckpoints() {
        if (checkpoitns == null)
            return;
        try
        {
            Checkpoint.setUncheckedCheckpointsToValue(totalPoints); // od nowa maksymalna liczba checkpointow do przejechania
            foreach(Checkpoint checkpoint in checkpoitns)
            {
                checkpoint.setChecked(false);
                checkpoint.resetMeshToGreen();
            } 
        } catch (Exception e)
        {
            Debug.LogWarning($"[GhostTrigger] cos poszlo nie tak : {e}");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        tryGetPlayer();
        getQuantityOfCheckpoints();
        setSpawnPosition();
        setSpawnRotation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}