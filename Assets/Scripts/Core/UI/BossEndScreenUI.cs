using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossEndScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject playerHudObject = null;
    [SerializeField] private GameObject resultObject = null;
    [SerializeField] private Text resultText = null;

    public static BossEndScreenUI Instance { get; private set; }

    private void Awake() {
        Instance = this;
    }

    public void Show(string result) {
        playerHudObject.SetActive(false);
 
        resultText.text = result;
        resultObject.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
}
