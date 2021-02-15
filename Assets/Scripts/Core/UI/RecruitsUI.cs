using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;
using UnityEngine.UI;
using System;
using System.Linq;

namespace BrackeysJam.Core.UI
{
    public class RecruitsUI : MonoBehaviour
    {
        // VARIABLES
        [SerializeField] private Text recruitsTitle = null;
        [SerializeField] private Text recruitsText = null;

        private PlayerRecruitManager player;

        // EXECUTION FUNCTIONS
        private void Awake() {
            player = FindObjectOfType<PlayerRecruitManager>();
        }

        private void Update() {
            recruitsTitle.text = $"Recruits (x{player.NumberOfRecruits})";

            var recruitTypes = Enum.GetValues(typeof(RecruitableTypes)).Cast<RecruitableTypes>();
            string recruitsString = "";

            foreach (var type in recruitTypes) {
                var recruitsOfType = player.NumberOfRecruitsOfType(type);
                recruitsString += $"{type.ToString()} x{recruitsOfType}\n";   
            }

            recruitsText.text = recruitsString;
        }
    }
}
