using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lucerna.Audio;

public class PlayerStats : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Animator barAnimator = null;
    private float currentHealth = 0f;

    public float HealthPercentage => currentHealth / maxHealth;

    private float hitCooldown = 0f;

    // EXECUTION FUNCTIONS
    private void Start() {
        currentHealth = maxHealth;
    }

    private void Update() {
        if (hitCooldown > 0f) {
            hitCooldown -= Time.deltaTime;
        }
    }

    // METHODS
    public void Damage(float val) {
        if (hitCooldown > 0f) return;

        barAnimator.SetTrigger("Hit");
        AudioManager.instance.Play("Player Damage", 2);

        currentHealth -= val;

        if (currentHealth <= 0) {
            Time.timeScale = 0f;

            BossEndScreenUI.Instance.Show("You have lost!");
        }
    }
}
