using UnityEngine;

public class TemporaryRotation : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    private void Update() {
        gameObject.transform.Rotate(_rotation * Time.deltaTime);
    }
}
