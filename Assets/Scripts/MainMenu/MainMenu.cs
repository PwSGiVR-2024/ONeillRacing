using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject MenuOptions;
    [SerializeField] GameObject AudioOptions;
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
        MenuOptions.SetActive(false);
        AudioOptions.SetActive(true);
    }

    public void ExitGame() {

    }

    // Options Menu
    public void BackToMain() {
        cameraDelegate(0);
        MenuOptions.SetActive(true);
        AudioOptions.SetActive(false);

    }
}
