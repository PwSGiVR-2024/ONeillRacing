using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] SoundManager sfxManager;
    [SerializeField] Finnish finnish;
    [SerializeField] StartCounter startCounter;
    [SerializeField][Tooltip("Add GUI and pause menu kits. It should be two of them.")] GameObject[] uiElements;
    
    private float _backToMenuDelay = 4.0f;
    private bool _paused = false;
    private bool _started = false;
    private bool _finnished = false;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startCounter.AStart += StartListener;
        finnish.AFinnish += FinnishListener;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !_finnished && _started) 
            ChangeUIElements();
    }

    private void StartListener() {
        _started = true;
    }

    private void FinnishListener() {
        _finnished = true;
        StartCoroutine(nameof(BackToMainDelay));
    }

    private IEnumerator BackToMainDelay() {
        yield return new WaitForSeconds(_backToMenuDelay);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    // Publics
    public void ChangeUIElements() {
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        if (!_paused) {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            foreach (AudioSource source in audioSources) {

                source.Pause();
            }

            sfxManager.LoadAndPlaySound("PauseOn");
        }
        else {
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            foreach (AudioSource source in audioSources)
                if (source.CompareTag("Player") || source.CompareTag("Radio"))
                    source.Play();

            sfxManager.LoadAndPlaySound("PauseOff");
        }
        foreach (var element in uiElements)
            element.SetActive(!element.activeSelf);

        _paused = !_paused;
    }

    public void BackToMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
