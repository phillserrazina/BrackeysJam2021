using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace BrackeysJam.Core.Entities
{
    public class PlayerBattleRecruitManager : MonoBehaviour
    {
        public IEnumerable<RecruitableTypes> RecruitTypes {
            get {
                return Enum.GetValues(typeof(RecruitableTypes)).Cast<RecruitableTypes>();
            } 
        }

        // Start is called before the first frame update
        void Start()
        {
            foreach (var type in RecruitTypes) {
                Debug.Log("Should spawn " + PlayerPrefs.GetInt($"{type.ToString()}") + " " + type.ToString());
            }
        }
    }
}
