using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrackeysJam.Core.Entities;

public class BossHealthBarUI : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private Image barImage = null;
    [SerializeField] private Color[] barColors = null;
    
    private BossAI boss;

    // EXECUTION FUNCTIONS
    private void Awake() {
        boss = FindObjectOfType<BossAI>();
    }

    private void Update() {
        barImage.color = barColors[boss.BossPhase];
        barImage.fillAmount = boss.HealthPercentage;
    }
}
