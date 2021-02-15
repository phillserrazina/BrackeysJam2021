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
            if (currentPoint == null) currentPoint = BoundariesManager.Instance.GetRandomPoint();

            if (Vector3.Distance(transform.position, currentPoint) < 2f) {
                currentPoint = BoundariesManager.Instance.GetRandomPoint();
            }

            transform.LookAt(currentPoint);
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

            rb.velocity = transform.forward * speed * 0.5f * Time.fixedDeltaTime;
        }
    }
}
