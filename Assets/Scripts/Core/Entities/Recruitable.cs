using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrackeysJam.Core.Entities
{
    public enum RecruitableTypes { Basic, Attack, Defense }

    public class Recruitable : MonoBehaviour
    {
        // VARIABLES
        private PlayerRecruitManager leader;

        private float DistanceFromLeader { 
            get { 
                if (leader == null) return -1f;

                return Vector3.Distance(transform.position, leader.transform.position);
            }
        }

        [SerializeField] private float speed = 2f;
        [SerializeField] private float distanceToStop = 1f;

        [SerializeField] private RecruitableTypes recruitType = RecruitableTypes.Basic;

        [Space(10)]
        [SerializeField] private ParticleSystem deathFX = null;
        public RecruitableTypes Type => recruitType;

        private Rigidbody rb;

        private Vector3 currentPoint = Vector3.zero;

        private float movementTimer = 5f;

        private Transform currentTarget = null;

        private bool roaming = true;

        // EXECUTION FUNCTIONS
        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            if (currentTarget != null) {
                if (recruitType == RecruitableTypes.Attack)
                {
                    var targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
       
                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
                
                    rb.velocity = transform.forward * speed * 5f * Time.fixedDeltaTime;
                }

                else if (recruitType == RecruitableTypes.Defense) 
                {
                    Debug.Log("Positioning! " + leader.name);

                    transform.LookAt(currentTarget);
                    transform.position = leader.transform.position + leader.transform.forward;
                    transform.localScale = new Vector3(5, 5, 1);
                }
            }
            
            else if (leader != null) {
                FollowLeader();
                return;
            }


            if (roaming)
                RandomMovement();
        }

        private void OnCollisionEnter(Collision other) {
            if (other.transform == currentTarget) {
                other.gameObject.GetComponent<BossAI>().Damage(30f);

                var fx = Instantiate(deathFX, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
                
                Destroy(gameObject);
            }
        }

        // METHODS
        public void TurnToRecruit(PlayerRecruitManager leader) {
            this.leader = leader;
        }

        public void Use(Transform target) 
        {
            roaming = false;

            if (recruitType == RecruitableTypes.Attack) 
            {
                transform.position = transform.position + Vector3.up;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                currentTarget = target;

                /*
                var dir = (currentTarget.position - transform.position).normalized;
                dir *= speed * 6f;
                dir += Vector3.up * 100f;
                */

                //rb.AddForce(dir);
            }

            else if (recruitType == RecruitableTypes.Defense)
            {
                currentTarget = target;
                SelfDestroy(3f);
            }

            else 
            {
                SelfDestroy();
            }
        }

        private void FollowLeader() {
            transform.LookAt(leader.transform);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

            if (DistanceFromLeader > distanceToStop) {
                rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
            }
        }

        private void RandomMovement() {
            if (currentPoint == null) 
                currentPoint = GetNewPoint();

            if (movementTimer > 0f) {
                movementTimer -= Time.deltaTime;
            }
            else {
                currentPoint = GetNewPoint();
            }

            if (Vector3.Distance(transform.position, currentPoint) < 2f) {
                currentPoint = GetNewPoint();
            }

            transform.LookAt(currentPoint);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

            rb.velocity = transform.forward * speed * 0.5f * Time.fixedDeltaTime;
        }

        private Vector3 GetNewPoint() {
            movementTimer = Random.Range(3f, 5f);
            return BoundariesManager.Instance.GetRandomPoint();
        }
    
        private void SelfDestroy(float timer=0) {
            var obj = Instantiate(deathFX, transform.position, Quaternion.identity);
            Destroy(obj, 3f);

            Destroy(gameObject, timer);
        }
    }
}
