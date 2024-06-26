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
    Rigidbody ghostRigidBody;

    private bool showGhostTraceInfo = false;
    private bool debugOn = false;

    private IEnumerator ghostRiding;

    private float waitTime = 1; 

    private bool useTeleprot;
    
    private bool useMoveForwardFcn;

    float ghostTime = 0;

    public void setWaitTime(float t)
    {
        waitTime = t;
    }

    Rigidbody getGhostRigidBody()
    {
        try
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            return rb;
        }
        catch (Exception e)
        {
            print("[Ghost]: Samochod nie ma na sobie Rigidbody? \n");
            return null;
        }
    }

    private IEnumerator setDataCorutine(List<TraceData> _trace_)
    {
        trace = new List<TraceData>(_trace_);
        yield return null;
    }

    public void setData(List<TraceData> _trace_) // trasa pobierana po pierwszym przejechaniu mety poprzez niewidoczny obiekt
    {
        StartCoroutine(setDataCorutine(_trace_));
    }

    public void runGhost(){
        StartCoroutine(GhostRiding());
    }

    private IEnumerator GhostRiding() // run ghost odpalany przez ukryty obiekt na mecie rowniez
    { 

        int i = 0;
        float timeFirst = 0;
        float timeNext = 0;
        float waitTime0 = 0;

        if(debugOn) print("[Ghost] rozpoczynam korutyne" + trace);

        foreach(TraceData traceData in trace) {
            timeNext = traceData.time;
            waitTime0 = timeNext - timeFirst; //(float) Math.Round(timeNext - timeFirst,3);
            timeFirst = timeNext;
            i++;
            Vector3 posA = gameObject.transform.position;
            Vector3 posB = traceData.place;

            Vector3 positionExpected = traceData.place;
            Vector3 ghostPosition = gameObject.transform.position;
            if (debugOn) print("pozycja nr:" + i +  " [Ghost] Roznica czasu " + waitTime + "waitime: "+waitTime +" GhostTime " + ghostTime + " time z cartrace: "+ traceData.time + " pozycja oczekiwana " + positionExpected +" pozycja ghosta " + ghostPosition);
            if (useTeleprot) {
                gameObject.transform.position = traceData.place; // teleportacja
                gameObject.transform.rotation = traceData.roatation;
            }
            else if (useMoveForwardFcn)
            {
                ghostRigidBody.angularVelocity = traceData.rotationVelocity;
                gameObject.transform.rotation = traceData.roatation;
                ghostRigidBody.velocity = traceData.velocity;

                // regulator bo samo velocity nadane na obiekt nie do ko�ca dzia�a
                // dziala to tak ze biore sobie rozkladam wektory na skladowe x y z 
                // wektory: pozycja aktualna, oczekiwana, wektor przyspieszenia
                // jezeli np. x oczekiwane jest > x aktualnego to biore sobie z nich roznice 
                // i dodaje do przyspieszenia ktore bylo wczesniej zapisane dla danej pozycji 

                float expectedX = positionExpected.x; // expexted x,y,z
                float expectedY = positionExpected.y; // to sa wartosci jakie mial gracz gdy jechal
                float expectedZ = positionExpected.z;

                float currentX = ghostPosition.x; // current x,y,z
                float currentY = ghostPosition.y; // to sa wartosci jakie ma duch teraz 
                float currentZ = ghostPosition.z;

                float velocityX = traceData.velocity.x; // velocity x,y,z // aktualnie zapisane przyspieszenie
                float velocityY = traceData.velocity.y; // to jest przyspieszenie jakie ma duch teraz
                float velocityZ = traceData.velocity.z;

                float additionalVelocityX = 0; // to beda wartosci dodane do wektora przyspieszenia
                float additionalVelocityY = 0;
                float additionalVelocityZ = 0;

                float newVelocityX = 0; // additional x,y,z
                float newVelocityY = 0; // to beda nowe wartosci dla wektora przyspieszenia
                float newVelocityZ = 0;

                if (expectedX > currentX) {
                    additionalVelocityX = expectedX - currentX;
                    newVelocityX = velocityX + additionalVelocityX;
                }
                    
                else if (expectedX < currentX) {
                    additionalVelocityX = currentX - expectedX;
                    newVelocityX = velocityX - additionalVelocityX;
                }
                    
                if (expectedY > currentY) {
                    additionalVelocityY = expectedY - currentY;
                    newVelocityY = velocityY + additionalVelocityY;
                }
                    
                else if (expectedY < currentY) {
                    additionalVelocityY = currentY - expectedY;
                    newVelocityY = velocityY - additionalVelocityY;
                }
                    
                if (expectedZ > currentZ) {
                    additionalVelocityZ = expectedZ - currentZ;
                    newVelocityZ = velocityZ + additionalVelocityZ;
                }
                    
                else if (expectedZ < currentZ) {
                    additionalVelocityZ = currentZ - expectedZ;
                    newVelocityZ = velocityZ - additionalVelocityZ;
                }
                Vector3 additionalVelocity = new Vector3(newVelocityX,newVelocityY,newVelocityZ);
                ghostRigidBody.velocity = additionalVelocity;
                if(debugOn) print("[Ghost] : wektor przyspieszenia zostal zmieniony z " + traceData.velocity + " na " + additionalVelocity);

            }

            if (debugOn)  ghostTime += waitTime;
            if (debugOn)  ghostTime = (float)Math.Round(ghostTime, 2);

            yield return new WaitForSeconds(waitTime0);
        }

        Destroy(gameObject);
        if(debugOn) print("Petla forech z korutyny wykonala sie " + i + " razy");
        //yield return null;
        
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
        ghostRigidBody = getGhostRigidBody();
        useMoveForwardFcn = true;
        useTeleprot = false;
        useMoveForwardFcn = true;
    //player = GameObject.FindGameObjectWithTag("Player");
}

    // Update is called once per frame
    void Update()
    {
        
    }
}
