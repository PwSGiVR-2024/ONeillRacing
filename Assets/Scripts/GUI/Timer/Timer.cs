using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTxt;
    static float time = 0.0f;
    bool timerOn = true;

    Finnish finnish;

    private void Start() {
        finnish = GameObject.FindGameObjectWithTag("Finnish").GetComponent<Finnish>();
        finnish.finnishedEvent += ChangeTimer;
    }

    private void Update() {
        if (timerOn) {
            time += Time.deltaTime;
            UpdateTimer(time);
        }
    }

    void UpdateTimer(float currentTime) {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float centy = Mathf.FloorToInt((currentTime % 1) * 100);
        timerTxt.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centy);
    }

    void ChangeTimer() {
        timerOn = !timerOn;
    }

    public static void ResetTimer()
    {
        time = 0;
    }

    public static float GetTime(){
        return Mathf.Round(time * 10f) / 10f; // Zaokr¹gla do jednego miejsca po przecinku;
    }
}
