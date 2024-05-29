using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GForceDisplay : MonoBehaviour
{
    [SerializeField]
    Gravity gravityScript;

    TMP_Text textMesh;

    private void Start()
    {
        textMesh = GetComponent<TMP_Text>();
        textMesh.text = "a";
    }

    // Update is called once per frame
    void Update()
    {
        textMesh.text = gravityScript.GetGForce(100,10).ToString()+"g";
    }
}
