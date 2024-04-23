using System;
using UnityEngine;

public class Finnish : MonoBehaviour
{
    public event Action finnishedEvent;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            finnishedEvent();
            Destroy(gameObject);
        }
    }
}
