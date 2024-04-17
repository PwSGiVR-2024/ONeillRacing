using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour {
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] TextMeshProUGUI[] audioLevel;
    [SerializeField] Slider[] audioSlider;

    private void Start() {
        if(PlayerPrefs.GetInt("FirstExe") == 0) {
            for (int i = 0; i < audioLevel.Length; i++) {
                audioSlider[i].value = PlayerPrefs.GetFloat($"Mixer_{i}");
            }
        }
        else {
            for (int i = 0; i < audioLevel.Length; i++) {
                audioSlider[i].value = 1;
            }
            PlayerPrefs.SetInt("FirstExe", 0);
        }
        PlayerPrefs.Save();
    }

    public void SetMasterLevel(float value) {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(value) * 20);
        audioLevel[0].text = (value * 100).ToString("F0") + "%";
        PlayerPrefs.SetFloat("Mixer_0", value);
        PlayerPrefs.Save();
    }

    public void SetMusicLevel(float value) {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
        audioLevel[1].text = (value * 100).ToString("F0") + "%";
        PlayerPrefs.SetFloat("Mixer_1", value);
        PlayerPrefs.Save();
    }

    public void SetSoundLevel(float value) {
        audioMixer.SetFloat("SoundVol", Mathf.Log10(value) * 20);
        audioLevel[2].text = (value * 100).ToString("F0") + "%";
        PlayerPrefs.SetFloat("Mixer_2", value);
        PlayerPrefs.Save();
    }
}
