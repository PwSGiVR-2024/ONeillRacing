using TMPro;
using UnityEngine;
using System;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerTxt;
    static float time = 0.0f;
    bool timerOn = false;

    StartCounter counter;
    Finnish finnish;

    private void Start() {
        try
        {
            counter = GameObject.FindGameObjectWithTag("StartCounter").GetComponent<StartCounter>();
            //finnish = GameObject.FindGameObjectWithTag("Finnish").GetComponent<Finnish>();
            counter.AStart += ChangeTimer;
            //finnish.finnishedEvent += ChangeTimer;
        }
        catch (Exception e){
            Debug.LogWarning($"[Timer.cs -> Start() ] na scenie nie ma obiektu z komponentem <Finish> \n Blad ->> {e}");
        }
    }

    private void Counter_AStart() {
        throw new NotImplementedException();
    }

    private void Update() {
        if (timerOn) {
            time += Time.deltaTime;
            UpdateTimer(time);
        }
    }

    private void UpdateTimer(float currentTime) {
        currentTime += 1;
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        float centy = Mathf.FloorToInt((currentTime % 1) * 100);
        timerTxt.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centy);
    }

    private void ChangeTimer() {
        timerOn = !timerOn;
    }

    public static void ResetTimer()
    {
        time = 0;
    }

    public static float GetTime(){
        //return time;
        return (float) Math.Round(time,2);
        //return Mathf.Round(time * 10f) / 10f; // Zaokr¹gla do jednego miejsca po przecinku;
    }
}
