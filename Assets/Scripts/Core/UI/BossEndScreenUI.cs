using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lucerna.Utils;

public class BossEndScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject playerHudObject = null;
    [SerializeField] private GameObject resultObject = null;
    [SerializeField] private Text resultText = null;

    public static BossEndScreenUI Instance { get; private set; }

    private bool hasBeenTriggered = false;

    private void Awake() {
        Instance = this;
    }

    public void Show(string result) {
        if (!hasBeenTriggered)
            Time.timeScale = 0f;

        hasBeenTriggered = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerHudObject.SetActive(false);
 
        resultText.text = result;
        resultObject.SetActive(true);
    }

    public void Restart() {
        SceneLoader.instance.LoadSceneAsync("Recruitment Scene");
        Time.timeScale = 1f;
    }
}
