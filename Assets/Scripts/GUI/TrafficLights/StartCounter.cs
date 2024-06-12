using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

public class StartCounter : MonoBehaviour
{
    public event Action AStart;

    [SerializeField] CarUserControl carUserControl;
    [SerializeField] GameObject trafficLightsImages;
    [SerializeField] AudioClip startSfx;
    [SerializeField] Image[] trafficLights;
    [SerializeField] float countDelay = 1;

    AudioSource _sfx;

    private void Start() {
        _sfx = GetComponent<AudioSource>();
        StartCoroutine(nameof(CountToBeginTheRace));
    }

    IEnumerator CountToBeginTheRace() {
        yield return new WaitForSeconds(countDelay);
        trafficLightsImages.SetActive(true);
        yield return new WaitForSeconds(countDelay);
        _sfx.Play();
        trafficLights[0].color = Color.red;
        yield return new WaitForSeconds(countDelay);
        _sfx.Play();
        trafficLights[1].color = Color.yellow;
        yield return new WaitForSeconds(countDelay);
        _sfx.clip = startSfx;
        _sfx.Play();
        carUserControl.enabled = true;
        trafficLights[2].color = Color.green;
        yield return new WaitForSeconds(countDelay);
        trafficLightsImages.SetActive(false);
        AStart();
    }
}
