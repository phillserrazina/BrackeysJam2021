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
        public RecruitableTypes Type => recruitType;

        private Rigidbody rb;

        private Vector3 currentPoint = Vector3.zero;

        private float movementTimer = 5f;

        // EXECUTION FUNCTIONS
        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            if (leader != null) {
                FollowLeader();
                return;
            }

            RandomMovement();
        }

        // METHODS
        public void TurnToRecruit(PlayerRecruitManager leader) {
            this.leader = leader;
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
    }


}
