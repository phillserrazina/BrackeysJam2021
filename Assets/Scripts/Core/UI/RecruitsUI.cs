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
        [SerializeField] private Text[] recruitTexts = null;

        private PlayerRecruitManager player;

        // EXECUTION FUNCTIONS
        private void Awake() {
            player = FindObjectOfType<PlayerRecruitManager>();
        }

        private void Update() {
            var arr = player.RecruitTypes.ToArray();
            for (int i = 0; i < arr.Length; i++) {
                var recruitsOfType = player.NumberOfRecruitsOfType(arr[i]);
                recruitTexts[i].text = $"{recruitsOfType}\n";   
            }
        }
    }
}
