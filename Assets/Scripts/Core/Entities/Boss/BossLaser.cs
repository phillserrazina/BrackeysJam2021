using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;

public class BossLaser : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float speed = 5f;
    [SerializeField] private float damage = 10f;
    private PlayerStats player;

    // EXECUTION FUNCTIONS
    private void OnEnable() {
        if (player == null) {
            player = FindObjectOfType<PlayerStats>();
        }

        var laserStartPos = player.transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

        transform.LookAt(laserStartPos);
    }

    private void Update() {
        var targetRotation = Quaternion.LookRotation(player.transform.position - transform.position);
       
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other) {
        var player = other.GetComponent<PlayerStats>();

        if (player != null) {
            player.Damage(Time.deltaTime * damage);
        }
    }
}
