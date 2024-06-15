using Cinemachine;
using System;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject[] segments;
    [SerializeField] CinemachineBrain brain;

    public delegate void CameraDelegate(int cameraID);
    public CameraDelegate cameraDelegate;
    public event Action ALeaderOn;
    public event Action ALeaderOff;

    private int _coroutineCam;

    // Main Menu
    public void NewGame() {
        cameraDelegate(0);
    }

    public void Leadreboard() {
        segments[0].SetActive(false);
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseIn;
        _coroutineCam = 2;
        cameraDelegate(1);
        StartCoroutine(nameof(NextCameraDelay));
    }

    public void Options() {
        cameraDelegate(3);
        segments[0].SetActive(false);
        segments[1].SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    // Options Menu
    public void BackToMain() {
        cameraDelegate(0);
        segments[0].SetActive(true);
        segments[1].SetActive(false);
    }

    public void CloseLeaderboard() {
        segments[2].SetActive(false);
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseIn;
        _coroutineCam = 0;
        cameraDelegate(1);
        StartCoroutine(nameof(NextCameraDelay));
        ALeaderOff();
    }

    private IEnumerator NextCameraDelay() {
        yield return new WaitForSeconds(1);
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseOut;
        cameraDelegate(_coroutineCam);
        yield return new WaitForSeconds(1);
        brain.m_DefaultBlend.m_Style = CinemachineBlendDefinition.Style.EaseInOut;
        if (_coroutineCam == 2) ALeaderOn();
        if (_coroutineCam == 0) segments[0].SetActive(true);
    }
}
