using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

namespace BrackeysJam.Core.Entities
{
    public class PlayerRecruitManager : MonoBehaviour
    {
        // VARIABLES
        private List<Recruitable> recruits = new List<Recruitable>();

        public int NumberOfRecruits => recruits.Count;
        public int NumberOfRecruitsOfType(RecruitableTypes type) 
        {
            var found = recruits.Where(recruit => recruit.Type == type).ToArray();

            return (found == null) ? 0 : found.Length;
        }

        public IEnumerable<RecruitableTypes> RecruitTypes {
           get {
               return Enum.GetValues(typeof(RecruitableTypes)).Cast<RecruitableTypes>();
           } 
        }

        // METHODS
        public void Recruit(Recruitable recruitable) {
            if (recruits.Contains(recruitable)) return;

            recruits.Add(recruitable);
            recruitable.TurnToRecruit(this);

            Debug.Log("Recruited!");
        }

        public void Remove(Recruitable r) {
            recruits.Remove(r);
        }

        public void SaveRecruits() {
            foreach (var type in RecruitTypes) {
                PlayerPrefs.SetInt($"{type.ToString()}", NumberOfRecruitsOfType(type));
            }
        }
    }
}
