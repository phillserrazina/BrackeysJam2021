using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Text timerText = null;

    // EXECUTION FUNCTIONS
    private void Update() {
        string minutes = Mathf.Floor(GameManager.Instance.CurrentTime / 60).ToString("00");
        string seconds = Mathf.Floor(GameManager.Instance.CurrentTime % 60).ToString("00");

        timerText.text = $"{ minutes }:{ seconds }";
    }
}
