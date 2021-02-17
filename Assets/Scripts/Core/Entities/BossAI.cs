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

        public int BossPhase { get; private set; }

        private bool slamingPlayer = false;

        private Vector3 slamPos = Vector3.zero;
        private Vector3 slamStartPos;
        [SerializeField] private float slamSpeed = 5f;
        [SerializeField] private float slamHeight = 1f;

        private float slamAnimator;


        // EXECUTION FUNCTIONS
        private void Awake() {
            player = FindObjectOfType<PlayerRecruitManager>();
        }

        private void Start() {
            currentHealth = maxHealth;
        
            Invoke("Attack", Random.Range(2f, 5f));
        }

        private void Update() {
            int newPhase = getCurrentPhase;

            if (BossPhase != newPhase) {
                // TODO: Play animation
                
                BossPhase = newPhase;
            }

            transform.LookAt(player.transform);
        }

        private void FixedUpdate() {
            if (slamingPlayer) {

                slamAnimator += Time.deltaTime;
                slamAnimator = slamAnimator % slamSpeed;

                transform.position = MathParabola.Parabola(slamStartPos, slamPos, slamHeight, slamAnimator / slamSpeed);
            
                if (Vector3.Distance(transform.position, slamPos) < 0.1f) {
                    stopSlamming();
                }

                return;
            }
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Ground") || other.gameObject == player.gameObject) {
                stopSlamming();
            }
        }

        // METHODS
        public void Damage(float val) {
            currentHealth -= val;

            if (currentHealth <= 0) {
                Time.timeScale = 0f;

                BossEndScreenUI.Instance.Show("You have won!");
            }
        }

        private void Attack() {
            if (BossPhase == 0)
            {
                throwProjectile();
            }
            else if (BossPhase == 1) 
            {
                if (Random.value > 0.5f) {
                    throwProjectile();
                }
                else {
                    slamAttack();
                }
            }

            Invoke("Attack", Random.Range(3f, 5f));
        }

        private int getCurrentPhase
        {
            get {
                if (HealthPercentage > 0.8f) return 0;
                else if (HealthPercentage > 0.5f) return 1;
                else if (HealthPercentage > 0.3f) return 2;
                else return 3;
            }
        }

        private void throwProjectile() {
            var proj = Instantiate(projectile, transform.position, transform.rotation);
            proj.Initialize(player.transform);
        
            Destroy(proj.gameObject, 2f);
        }

        private void slamAttack() {
            slamStartPos = transform.position;
            slamingPlayer = true;

            slamPos = player.transform.position;
        }

        private void stopSlamming() {
            slamingPlayer = false;
            slamAnimator = 0f;

            var newPos = transform.position;
            newPos.y = 1.5f;
            transform.position = newPos;
        }
    }
}
