﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lucerna.Audio;

namespace BrackeysJam.Core.Entities
{
    public enum RecruitableTypes { Basic, Attack, Defense, SpecialArmorPen }

    public class Recruitable : MonoBehaviour
    {
        // VARIABLES
        private PlayerRecruitManager leader;
        public PlayerRecruitManager Leader { get { return leader; } }

        private float DistanceFromLeader { 
            get { 
                if (leader == null) return -1f;

                return Vector3.Distance(transform.position, leader.transform.position);
            }
        }

        [SerializeField] private bool roam = false;
        [SerializeField] private float speed = 2f;
        [SerializeField] private float distanceToStop = 1f;

        [SerializeField] private RecruitableTypes recruitType = RecruitableTypes.Basic;

        [Space(10)]
        [SerializeField] private Animator animator = null;
        [SerializeField] private ParticleSystem deathFX = null;
        [SerializeField] private ParticleSystem onCatchFX = null;
        public RecruitableTypes Type => recruitType;

        private Rigidbody rb;

        private Vector3 currentPoint = Vector3.zero;

        private float movementTimer = 5f;

        private Transform currentTarget = null;

        private bool roaming = true;

        public bool CanMove = true;

        private Vector3 currentDir;

        [SerializeField] private AudioClip[] clips = null;
        private new AudioSource audio;

        // EXECUTION FUNCTIONS
        private void Awake() {
            rb = GetComponent<Rigidbody>();
            audio = GetComponent<AudioSource>();
        }

        private void Update() {
            var animVel = rb.velocity;
            animVel.y = 0f;
            animator.SetBool("Moving", animVel.magnitude > 0.5f);
            animator.SetBool("Thrown", currentTarget != null);
        }

        private void FixedUpdate() {
            if (currentTarget != null) {
                if (recruitType == RecruitableTypes.Attack || recruitType == RecruitableTypes.SpecialArmorPen)
                {
                    var targetRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
       
                    // Smoothly rotate towards the target point.
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
                
                    rb.velocity = transform.forward * speed * 5f * Time.fixedDeltaTime;          
                }

                else if (recruitType == RecruitableTypes.Basic)
                {
                    rb.velocity = currentDir * speed * 5f * Time.fixedDeltaTime;
                }

                else if (recruitType == RecruitableTypes.Defense) 
                {
                    transform.LookAt(currentTarget);
                    transform.position = leader.transform.position + Vector3.up + leader.transform.forward;
                    transform.localScale = new Vector3(5, 5, 1);
                }
            }
            
            else if (leader != null) {
                transform.LookAt(leader.transform);

                if (CanMove) {
                    FollowLeader();
                    return;
                }
                else {
                    rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                    return;
                }
            }


            else if (roaming && roam)
                RandomMovement();
        }

        private void OnTriggerEnter(Collider other) {
            if (other.transform == currentTarget) {
                other.GetComponent<BossAI>().Damage(Type);

                var fx = Instantiate(deathFX, transform.position, Quaternion.identity);
                Destroy(fx, 2f);
                
                Destroy(gameObject);
            }
        }

        private void OnCollisionEnter(Collision other) {

            if (Type == RecruitableTypes.Defense) return;

            if (other.gameObject.CompareTag("Ground"))
            {
                if (currentTarget == null) return;

                if (Random.value > 0.5f) {
                    SelfDestroy();
                }

                currentTarget = null;
                leader.Recruit(this, false);
                PlayerBattleRecruitManager.Instance.Requeue(this);
            }
        }

        // METHODS
        public void TurnToRecruit(PlayerRecruitManager leader, bool playSound=true) {
            this.leader = leader;

            animator.SetTrigger("Catch");

            if (playSound) {
                audio.clip = clips[0];
                audio.Play();
            }

            var catchFX = Instantiate(onCatchFX, transform.position, onCatchFX.transform.rotation);
            Destroy(catchFX, 2f);
        }

        public void Use(Transform target) 
        {
            AudioManager.instance.Play("OnThrow");

            audio.clip = clips[Random.value < 0.5f ? 2 : 3];
            audio.Play();

            roaming = false;

            if (recruitType == RecruitableTypes.Basic) 
            {
                transform.position = transform.position + Vector3.up;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                currentDir = (CameraAimUtil.Instance.CurrentPoint - transform.position).normalized;
                currentTarget = target;
            }

            else if (recruitType == RecruitableTypes.Attack) 
            {
                transform.position = transform.position + Vector3.up;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                currentTarget = target;
            }

            else if (recruitType == RecruitableTypes.Defense)
            {
                currentTarget = target;
                gameObject.tag = "Wall";
                SelfDestroy(3f);
            }

            else if (recruitType == RecruitableTypes.SpecialArmorPen) {
                transform.position = transform.position + Vector3.up;
                rb.constraints = RigidbodyConstraints.FreezeRotation;
                currentTarget = target;
            }

            else 
            {
                SelfDestroy();
            }
        }

        private void FollowLeader() {            
            float newY = rb.velocity.y;

            if (DistanceFromLeader > distanceToStop) {
                rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
                rb.velocity = new Vector3(rb.velocity.x, newY, rb.velocity.z); 
            }
            else {
                rb.velocity = new Vector3(0f, newY, 0f);
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

            float newY = rb.velocity.y;
            rb.velocity = transform.forward * speed * 0.5f * Time.fixedDeltaTime;
            rb.velocity = new Vector3(rb.velocity.x, newY, rb.velocity.z);
        }

        private Vector3 GetNewPoint() {
            movementTimer = Random.Range(3f, 5f);
            //return BoundariesManager.Instance.GetRandomPoint();
        
            var newPoint = new Vector3(
                transform.position.x + Random.Range(-3f, 3f),
                transform.position.y + Random.Range(-3f, 3f),
                transform.position.z + Random.Range(-3f, 3f)
            );

            newPoint.y = Terrain.activeTerrain.SampleHeight(newPoint);
            return newPoint;
        }
    
        public void SelfDestroy(float timer=0) {
            var obj = Instantiate(deathFX, transform.position, Quaternion.identity);
            Destroy(obj, 3f);

            audio.clip = clips[4];
            audio.Play();

            Destroy(gameObject, timer);
        }

        private void OnDestroy() {
            if (leader != null) leader.Remove(this);
            RecruitsGroupMovementManager.Instance.Remove(this);
        }
    }
}
