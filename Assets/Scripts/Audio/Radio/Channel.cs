using System.Collections;
using UnityEngine;

public class Channel : MonoBehaviour
{
    [SerializeField] private AudioClip[] clip;
    private AudioSource _bgm;
    private float _clipLength;

    private void Start() {
        _bgm = gameObject.GetComponent<AudioSource>();
        RandomizeClip();
    }

    private void RandomizeClip() {
        int index = Random.Range(0, clip.Length);
        _bgm.clip = clip[index];
        _clipLength = _bgm.clip.length;
        print(gameObject.name + " clip length: " + _clipLength);
        _bgm.Play();
        StartCoroutine(nameof(NextClipDelay));
    }

    private IEnumerator NextClipDelay() {
        yield return new WaitForSeconds(_clipLength);
        RandomizeClip();
    }
}
