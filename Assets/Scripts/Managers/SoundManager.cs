using UnityEngine;

public class SoundManager : MonoBehaviour {
    AudioSource sfx;

    private void Start() {
        sfx = GetComponent<AudioSource>();
    }

    public void LoadAndPlaySound(string filename) {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Sounds/{filename}");
        sfx.PlayOneShot(clip);
    }
}
