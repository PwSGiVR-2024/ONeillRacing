using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] AudioSource Sfx;

    public void LoadAndPlaySound(string filename) {
        AudioClip clip = Resources.Load<AudioClip>($"Audio/Sounds/{filename}");
        Sfx.PlayOneShot(clip);
    }
}
