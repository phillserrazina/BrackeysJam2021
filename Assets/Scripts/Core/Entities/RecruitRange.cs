using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BrackeysJam.Core.Entities
{
    public class RecruitRange : MonoBehaviour
    {
        [SerializeField] private PlayerRecruitManager recruitManager = null;

        private void OnTriggerEnter(Collider other) {
            var recruitable = other.GetComponent<Recruitable>();

            if (recruitable == null) return;

            recruitManager.Recruit(recruitable);
        }
    }
}
