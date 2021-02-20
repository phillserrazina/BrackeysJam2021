using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private PlayerRecruitManager player;
    [SerializeField] private float gameTime = 60;
    [SerializeField] private bool isBattleScene = false;
    public float CurrentTime { get; private set; }

    private bool isPaused = false;

    [Space(10)]
    [SerializeField] private Animator pauseMenuUI = null;

    public static GameManager Instance { get; private set; }

    // EXECUTION FUNCTIONS
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        CurrentTime = gameTime;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Pause();
        }

        if (isPaused) return;
        if (isBattleScene) return;

        CurrentTime -= Time.deltaTime;

        if (CurrentTime <= 0 || Input.GetKeyDown(KeyCode.E)) {
            player.SaveRecruits();
            SceneManager.LoadScene(2);
        }
    }

    // METHODS
    public void Pause() {
        isPaused = !isPaused;

        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = isPaused ? true : false;

        if (isPaused) {
            pauseMenuUI.gameObject.SetActive(true);
        }
        else {
            pauseMenuUI.Play("OnExit");
        }
    }

    public void Quit() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
