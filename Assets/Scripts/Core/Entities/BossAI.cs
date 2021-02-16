using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrackeysJam.Core.Entities
{
    public class BossAI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private BossProjectile projectile = null;

        [SerializeField] private float maxHealth = 100f;
        private float currentHealth = 0f;

        public float HealthPercentage => currentHealth / maxHealth;

        private PlayerRecruitManager player;

        // EXECUTION FUNCTIONS
        private void Awake() {
            player = FindObjectOfType<PlayerRecruitManager>();
        }

        private void Start() {
            currentHealth = maxHealth;
        
            Invoke("Attack", Random.Range(2f, 5f));
        }

        private void Update() {
            transform.LookAt(player.transform);
        }

        // METHODS
        public void Damage(float val) {
            currentHealth -= val;

            if (currentHealth <= 0) {
                Time.timeScale = 0f;

                BossEndScreenUI.Instance.Show("You have won!");
            }
        }

        public void Attack() {
            var proj = Instantiate(projectile, transform.position, transform.rotation);
            proj.Initialize(player.transform);
        
            Destroy(proj.gameObject, 2f);

            Invoke("Attack", Random.Range(2f, 5f));
        }
    }
}
