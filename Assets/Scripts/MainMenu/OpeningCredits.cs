using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpeningCredits : MonoBehaviour
{
    [SerializeField] GameObject[] segments;
    [SerializeField] TextMeshProUGUI[] credits;
    [SerializeField] Image logo;
    [SerializeField] AudioSource menuMusic;

    private bool _inMenu;

    private void Start() {
        StartCoroutine(nameof(Opening));
    }

    private void Update() {
        if (Input.anyKey && !_inMenu) {
            StopAllCoroutines();
            menuMusic.time = 9.4f;
            StartCoroutine(nameof(ShowLogo));
        }
    }

    private IEnumerator Opening() {
        foreach (TextMeshProUGUI txt in credits) {
            while (txt.color.a <= 1) {
                Color color = txt.color;
                color.a += 0.0125f;
                txt.color = color;
                yield return new WaitForSeconds(0.028f);
            }
        }
        StartCoroutine(nameof(ShowLogo));
    }

    private IEnumerator ShowLogo() {
        _inMenu = true;
        segments[0].SetActive(false);
        segments[1].SetActive(true);
        yield return new WaitForSeconds(1.0f);

        while (logo.color.a >= 0) {
            Color color = logo.color;
            color.a -= 0.0125f;
            logo.color = color;
            yield return new WaitForSeconds(0.025f);
        }

        segments[2].SetActive(false);
        segments[3].SetActive(true);
    }
}
