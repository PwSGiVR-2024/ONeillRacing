using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    [SerializeField] private AudioClip[] clip;
    private AudioSource _bgm;
    private float _clipLength;
    private int _clipIndex = 0;

    private void Start() {
        _bgm = gameObject.GetComponent<AudioSource>();
        ShuffleClips();
        PlayNextClip();
    }

    private void ShuffleClips() {
        for(int i = clip.Length - 1; i > 0; i--) {
            int randomIndex = Random.Range(0, i + 1);
            (clip[randomIndex], clip[i]) = (clip[i], clip[randomIndex]);
        }
    }

    private void PlayNextClip() {
        _bgm.clip = clip[_clipIndex];
        _clipLength = _bgm.clip.length;
        print(gameObject.name + " clip length: " + _clipLength);
        _bgm.Play();
        _clipIndex++;
        if (_clipIndex >= clip.Length)
            _clipIndex = 0;
        StartCoroutine(nameof(NextClipDelay));
    }

    private IEnumerator NextClipDelay() {
        yield return new WaitForSeconds(_clipLength);
        PlayNextClip();
    }
}
