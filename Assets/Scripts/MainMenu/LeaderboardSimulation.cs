using System.Collections;
using TMPro;
using UnityEngine;

public class LeaderboardSimulation : MonoBehaviour
{
    [SerializeField] MainMenu menu;
    [SerializeField] SoundManager soundManager;
    [SerializeField] TextMeshProUGUI loadingPercent;
    [SerializeField] GameObject[] segments;

    private void Start() {
        menu.ALeaderOn += LeaderBoardOn;
        menu.ALeaderOff += LeaderBoardOff;
    }

    private void LeaderBoardOn() {
        soundManager.LoadAndPlaySound($"Leader_{Random.Range(0, 3)}");
        StartCoroutine(nameof(LoadingSimulation));
    }

    private void LeaderBoardOff() {
        segments[0].SetActive(true);
        segments[1].SetActive(false);
    }

    private IEnumerator LoadingSimulation() {
        for (int i = 0; i < 100; i++) {
            loadingPercent.text = $"{i}%";
            yield return new WaitForSeconds(0.025f);
        }
        segments[0].SetActive(false);
        segments[1].SetActive(true);
        segments[2].SetActive(true);
    }
}
