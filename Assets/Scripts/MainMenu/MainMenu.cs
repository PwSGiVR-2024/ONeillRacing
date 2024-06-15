using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] segments;
    [SerializeField] CinemachineBrain brain;

    public delegate void CameraDelegate(int cameraID);
    public CameraDelegate cutCameraDelegate;
    public delegate void BlendCameraDelegate(int cam, CinemachineBlendDefinition.Style blend, float time);
    public BlendCameraDelegate blendCameraDelegate;
    public event Action ALeaderOn;
    public event Action ALeaderOff;

    private int _coroutineCam;

    // Main Menu
    public void NewGame() {
        blendCameraDelegate(0, CinemachineBlendDefinition.Style.EaseInOut, 1);
    }

    public void Leadreboard() {
        segments[0].SetActive(false);
        _coroutineCam = 2;
        StartCoroutine(nameof(NextCameraDelay));
    }

    public void Options() {
        blendCameraDelegate(3, CinemachineBlendDefinition.Style.EaseInOut, 1);
        segments[0].SetActive(false);
        segments[1].SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    // Options Menu
    public void BackToMain() {
        blendCameraDelegate(0, CinemachineBlendDefinition.Style.EaseInOut, 1);
        segments[0].SetActive(true);
        segments[1].SetActive(false);
    }

    public void CloseLeaderboard() {
        segments[2].SetActive(false);
        _coroutineCam = 0;
        StartCoroutine(nameof(NextCameraDelay));
        ALeaderOff();
    }

    private IEnumerator NextCameraDelay() {
        blendCameraDelegate(1, CinemachineBlendDefinition.Style.EaseIn, 1);
        yield return new WaitForSeconds(1);
        blendCameraDelegate(_coroutineCam, CinemachineBlendDefinition.Style.HardOut, 1);
        yield return new WaitForSeconds(1);
        if (_coroutineCam == 2) ALeaderOn();
        if (_coroutineCam == 0) segments[0].SetActive(true);
    }
}
