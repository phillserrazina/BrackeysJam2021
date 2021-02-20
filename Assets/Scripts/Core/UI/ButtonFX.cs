using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lucerna.Audio;

public class ButtonFX : MonoBehaviour
{
    public void PlaySound() {
        AudioManager.instance.Play("Button");
    }
}
