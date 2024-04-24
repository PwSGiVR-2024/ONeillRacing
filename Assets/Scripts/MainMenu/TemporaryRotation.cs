using UnityEngine;

public class TemporaryRotation : MonoBehaviour
{
    private void Update() {
        gameObject.transform.Rotate(0, -0.1f, 0);
    }
}
