using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Checkpoint : MonoBehaviour
{
    public static int uncheckedCheckpoints = 0;
    bool isChecked = false;
    [SerializeField]
    Material checkedMaterial;

    [SerializeField]
    Material ucheckedMaterial;

    public void setChecked(bool option)
    {
        isChecked = option;
    }
    public static void setUncheckedCheckpointsToValue(int value_per_level)
    {
        uncheckedCheckpoints = value_per_level;
    }

    private void Start()
    {
        uncheckedCheckpoints += 1;
    }

    public void resetMeshToGreen()
    {
        GetComponent<MeshRenderer>().material = ucheckedMaterial;
        gameObject.GetComponentInParent<Light>().color = Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (isChecked == false) {

                // checkpoint bedzie dodawac punkty dla samochodu, GhostTrigger bedzie je zerowac po przekroczeniu mety
                // GhostTrigger takze bedzie resetowac wszystkie checkpointy 
                try
                {
                    CarTrace carTrace = other.GetComponent<CarTrace>();
                    carTrace.incrementPoints(); 
                } catch (Exception e)
                {
                    Debug.LogWarning($"Nie moge sie dostaac do komponentu CarTrace {e}");
                }

                uncheckedCheckpoints -= 1; 
                isChecked = true;
                GetComponent<MeshRenderer>().material = checkedMaterial;
                gameObject.GetComponentInParent<Light>().color = Color.green;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public static bool AreAllChecked(){
        if(uncheckedCheckpoints > 0) { return false; } else { return true; }
    }
}
