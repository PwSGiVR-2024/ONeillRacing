using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    static int uncheckedCheckpoints = 0;
    bool isChecked = false;
    [SerializeField]
    Material checkedMaterial;

    private void Start()
    {
        uncheckedCheckpoints += 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            if (isChecked == false) {
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
