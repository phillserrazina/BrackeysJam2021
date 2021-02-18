using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        [SerializeField] private GameObject onSlamFX = null;

        private float slamAnimator;

        [SerializeField] private BossLaser laser = null;
        private bool usingLaser = false;

        [SerializeField] private BossAttackSO[] attacks = null;

        [SerializeField] private Animator animator = null;

        // EXECUTION FUNCTIONS
        private void Awake() {
            player = FindObjectOfType<PlayerRecruitManager>();
        }

        private void Start() {
            currentHealth = maxHealth;
        
            Invoke("Attack", Random.Range(2f, 4f));
        }

        private void Update() {
            int newPhase = getCurrentPhase;

            if (BossPhase != newPhase) {
                // TODO: Play animation
                
                BossPhase = newPhase;
            }

            laser.gameObject.SetActive(usingLaser);

            if (usingLaser) {
                var newRot = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
                transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, newRot, 3f * Time.deltaTime);
            }
            else {
                var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
       
                // Smoothly rotate towards the target point.
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
            }
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

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Ground")) {
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

        private void Attack() 
        {
            float randVal = Random.value;

            switch (BossPhase)
            {
                case 0:
                    PlayAttack("Projectile");
                    break;
                
                case 1:
                    if (randVal > 0.5f) PlayAttack("Projectile");
                    else PlayAttack("Slam");
                    break;
                
                case 2:
                    if (randVal < 0.3f) PlayAttack("Projectile");
                    else if (randVal < 0.6f) PlayAttack("Slam");
                    else PlayAttack("Laser");
                    
                    break;
                
                case 3:
                    break;
                
                default:
                    Debug.LogError("BossAI::Attack() --- Invalid State.");
                    break;
            }


            if (!usingLaser)
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

            slamPos = player.transform.position + Vector3.up * 3f;
        }

        private void stopSlamming() {
            slamingPlayer = false;
            slamAnimator = 0f;

            var fxPos = transform.position;
            fxPos.y = 0.1f;

            var ps = Instantiate(onSlamFX, fxPos, onSlamFX.transform.rotation);
            Destroy(ps, 1f);
        }

        private void PlayAttack(string attackName) {
            var attack = attacks.Where(a => a.Name == attackName).ToArray()[0];

            animator.SetInteger("Attack", attack.AnimationIndex);
        }
    }
}
