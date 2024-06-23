using System;
using UnityEngine;

public class Finnish : MonoBehaviour
{
    public event Action AFinnish;

    [SerializeField] GameObject carCamera;
    [SerializeField] GameObject finnishCamera;

    public event Action finnishedEvent;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && Checkpoint.AreAllChecked()) {
            AFinnish();
            GameObject.Find("AudioManager").GetComponent<SoundManager>().LoadAndPlaySound("Finnish");
            finnishCamera.SetActive(true);
            carCamera.SetActive(false);
            finnishedEvent();
            Destroy(gameObject);
        }
    }
}
