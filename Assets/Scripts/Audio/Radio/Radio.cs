using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour
{
    [SerializeField] AudioSource[] channelSource;
    [SerializeField] GameObject[] channelName;
    [SerializeField] float _nameShowTime;
    private int _channel = 4;

    private void Start() {
        UpdateRadio();
    }

    private void Update() {
        CheckForInputAndChangeChannel();
    }

    private void CheckForInputAndChangeChannel() {
        if (Input.GetKeyDown(KeyCode.R)) {
            StopAllCoroutines();
            _channel++;
            if (_channel >= 5)
                _channel = 0;
            UpdateRadio();
        }
    }

    private void UpdateRadio() {
        if (_channel == 4)
            channelSource[3].volume = 0.0f;
        else {
            channelSource[_channel].volume = 1.0f;
            if(_channel != 0)
                channelSource[_channel - 1].volume = 0.0f;
        }
        StartCoroutine(nameof(ShowChannelName));
    }

    private IEnumerator ShowChannelName() {
        foreach(GameObject index in channelName)
            index.SetActive(false);
        channelName[_channel].SetActive(true);
        yield return new WaitForSeconds(_nameShowTime);
        channelName[_channel].SetActive(false);
    }
}
