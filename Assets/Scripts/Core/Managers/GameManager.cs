﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private PlayerRecruitManager player;
    [SerializeField] private float gameTime = 60;
    public float CurrentTime { get; private set; }

    public static GameManager Instance { get; private set; }

    // EXECUTION FUNCTIONS
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        CurrentTime = gameTime;
    }

    private void Update() {
        CurrentTime -= Time.deltaTime;

        if (CurrentTime <= 0 || Input.GetKeyDown(KeyCode.E)) {
            player.SaveRecruits();
            SceneManager.LoadScene(2);
        }
    }
}