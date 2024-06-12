using System;
using UnityEngine;

public class Finnish : MonoBehaviour
{
    public event Action AFinnish;

    [SerializeField] GameObject CarCamera;
    [SerializeField] GameObject FinnishCamera;

    public event Action finnishedEvent;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            AFinnish();
            GameObject.Find("AudioManager").GetComponent<SoundManager>().LoadAndPlaySound("Finnish");
            FinnishCamera.SetActive(true);
            CarCamera.SetActive(false);
            finnishedEvent();
            Destroy(gameObject);
        }
    }
}
