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

        if (GameManager.Instance.CurrentTime <= 20) {
            timerText.transform.localScale = new Vector3(2f, 2f, 2f);
            timerText.color = Color.red;
        }

        else if (GameManager.Instance.CurrentTime <= 40) {
            timerText.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            timerText.color = Color.yellow;
        }

        timerText.text = $"{ minutes }:{ seconds }";
    }
}
