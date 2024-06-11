using System;
using System.Collections;
using UnityEngine;

public class Finnish : MonoBehaviour
{
    [SerializeField] GameObject CarCamera;
    [SerializeField] GameObject FinnishCamera;
    public event Action finnishedEvent;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameObject.Find("AudioManager").GetComponent<SoundManager>().LoadAndPlaySound("Finnish");
            FinnishCamera.SetActive(true);
            CarCamera.SetActive(false);
            finnishedEvent();
            Destroy(gameObject);
        }
    }
}
