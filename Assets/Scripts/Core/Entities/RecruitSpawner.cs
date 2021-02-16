using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;

public class RecruitSpawner : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private int amountToSpawn = 100;
    [SerializeField] private Transform recruitGroupObject = null;
    [SerializeField] private Recruitable[] recruitablePrefabs = null;
    [Range(0f, 1f)]
    [SerializeField] private float[] spawnRates = null;

    // EXECUTION FUNCTIONS
    private void Start() {
        for (int i = 0; i < amountToSpawn; i++) {
            float val = Random.value;

            for (int j = 0; j < recruitablePrefabs.Length; j++) {
                if (val < spawnRates[j]) {
                    var spawnPos = BoundariesManager.Instance.GetRandomPoint();
                    spawnPos += Vector3.up * 0.4f;
                    var spawned = Instantiate(recruitablePrefabs[j], spawnPos, Quaternion.identity);
                    spawned.transform.SetParent(recruitGroupObject);
                }
            }
        }
    }
}
