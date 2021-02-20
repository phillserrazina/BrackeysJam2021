using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lucerna.Audio;

public class PlayerAnimationHelper : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ground")) {
            AudioManager.instance.Play("Step");
        }
    }
}
