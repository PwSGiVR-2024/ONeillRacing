using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject AudioOptionsa;
    public delegate void CameraDelegate(int cameraID);
    public CameraDelegate cameraDelegate;

    // Main Menu
    public void NewGame() {
        cameraDelegate(0);
    }

    public void Leadreboard() {
        cameraDelegate(1);
    }

    public void Options() {
        cameraDelegate(2);
        menuOptions.SetActive(false);
        AudioOptionsa.SetActive(true);
    }

    public void ExitGame() {
        Application.Quit();
    }

    // Options Menu
    public void BackToMain() {
        cameraDelegate(0);
        menuOptions.SetActive(true);
        AudioOptionsa.SetActive(false);
    }
}
