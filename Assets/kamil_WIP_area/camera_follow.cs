using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{
    Transform camtrans;
    [SerializeField]
    Transform balltrans;
    [SerializeField]
    Vector3 CamOffset;

    // Start is called before the first frame update
    void Start()
    {
        camtrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        camtrans.position = balltrans.position + CamOffset;
    }
}
