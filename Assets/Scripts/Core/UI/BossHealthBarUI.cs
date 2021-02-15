using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrackeysJam.Core.Entities;

public class BossHealthBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage = null;
    
    private BossAI boss;

    private void Awake() {
        boss = FindObjectOfType<BossAI>();
    }

    private void Update() {
        barImage.fillAmount = boss.HealthPercentage;
    }
}
