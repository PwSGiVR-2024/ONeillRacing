using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] SoundManager SfxManager;
    [SerializeField] Finnish finnish;
    [SerializeField] GameObject[] UIElements;

    private bool _paused = false;
    private bool _finnished = false;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        finnish.AFinnish += FinnishListener;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !_finnished) 
            ChangeUIElements();
    }

    private void FinnishListener() {
        _finnished = true;
    }

    // Publics
    public void ChangeUIElements() {
        if (!_paused) {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        _paused = !_paused;
        SfxManager.LoadAndPlaySound("EnginePedal");

        foreach (var element in UIElements)
            element.SetActive(!element.activeSelf);
    }

    public void BackToMenu() {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
