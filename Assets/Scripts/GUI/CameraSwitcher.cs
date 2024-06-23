using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    GameObject cam1;
    [SerializeField]
    GameObject cam2;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            cam1.active = !cam1.active;
            cam2.active = !cam1.active;
        }
    }
}
