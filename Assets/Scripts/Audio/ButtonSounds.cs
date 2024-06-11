using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{

    public void HoverSound() {
        GameObject.Find("AudioManager").GetComponent<SoundManager>().LoadAndPlaySound("BtnHover");
    }

    public void SelectSound() {
        GameObject.Find("AudioManager").GetComponent<SoundManager>().LoadAndPlaySound("BtnSelect");
    }
}
