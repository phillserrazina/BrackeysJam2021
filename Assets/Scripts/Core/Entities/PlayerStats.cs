using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Animator barAnimator = null;
    private float currentHealth = 0f;

    public float HealthPercentage => currentHealth / maxHealth;

    // EXECUTION FUNCTIONS
    private void Start() {
        currentHealth = maxHealth;
    }

    // METHODS
    public void Damage(float val) {
        barAnimator.SetTrigger("Hit");

        currentHealth -= val;

        if (currentHealth <= 0) {
            Time.timeScale = 0f;

            BossEndScreenUI.Instance.Show("You have lost!");
        }
    }
}
