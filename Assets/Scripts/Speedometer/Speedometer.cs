using TMPro;
using UnityEngine;

public class Speedometer : MonoBehaviour
{
    [SerializeField] Rigidbody target;
    [SerializeField] float maxSpeed = 0.0f;
    [SerializeField] float minArrowAngle;
    [SerializeField] float maxArrowAngle;

    [SerializeField] TextMeshProUGUI speedTxt;
    [SerializeField] RectTransform arrow;

    [SerializeField] float speed = 0.0f;

    private void Update() {
        speed = target.velocity.magnitude * 3.6f;   // Dodatkowa konwersja na km/h

        if(speedTxt != null)
            speedTxt.text = ((int)speed) + " km/h";
        if (arrow != null)
            arrow.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(minArrowAngle, maxArrowAngle, speed / maxSpeed));
    }
}
