using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BrackeysJam.Core.Entities;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image barImage = null;
    
    private PlayerStats boss;

    private void Awake() {
        boss = FindObjectOfType<PlayerStats>();
    }

    private void Update() {
        barImage.fillAmount = boss.HealthPercentage;
    }
}
