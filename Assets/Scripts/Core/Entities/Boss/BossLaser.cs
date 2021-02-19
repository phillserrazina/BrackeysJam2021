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

    private float killTimer = 0f;

    // EXECUTION FUNCTIONS
    private void OnEnable() {
        if (player == null) {
            player = FindObjectOfType<PlayerStats>();
        }

        var laserStartPos = player.transform.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));

        transform.LookAt(laserStartPos);
    }

    private void Update() {
        if (killTimer > 0f) {
            killTimer -= Time.deltaTime;   
        }

        var targetRotation = Quaternion.LookRotation((player.transform.position) - transform.position);
       
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other) {
        var player = other.GetComponent<PlayerStats>();

        if (player != null) {
            player.Damage(Time.deltaTime * damage);
        }

        if (killTimer > 0f) return;

        killTimer = 0.2f;
        var recruit = other.GetComponent<Recruitable>();

        if (recruit != null) {
            if (recruit.CompareTag("Wall")) {
                GetComponentInParent<Animator>().SetTrigger("Hit");
                gameObject.SetActive(false);
            }
            recruit.SelfDestroy();
        }
    }
}
