using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;

public class BossProjectile : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float speed = 50f;
    [SerializeField] private float rotSpeed = 1f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private ParticleSystem deathFX = null;

    private Transform currentTarget;

    private Rigidbody rb;

    // EXECUTION FUNCTIONS
    private void Awake() {
        rb = GetComponent<Rigidbody>();

        Destroy(gameObject, 2f);   
    }

    void FixedUpdate()
    {
        var targetRotation = Quaternion.LookRotation((currentTarget.transform.position + Vector3.up * 0.5f) - transform.position);
       
        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

        rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ground")) {
            SelfDestroy();
            return;
        }

        var recruit = other.GetComponent<Recruitable>();
        if (recruit != null) {
            if (recruit.CompareTag("Wall")) {
                SelfDestroy();
                recruit.SelfDestroy();
            }
            return;
        }

        var player = other.GetComponent<PlayerStats>();

        if (player == null) return;

        player.Damage(damage);
        SelfDestroy();
    }

    // METHODS
    public void Initialize(Transform target) {
        currentTarget = target;
    }

    private void SelfDestroy(float timer=0) {
        var obj = Instantiate(deathFX, transform.position, Quaternion.identity);
        Destroy(obj, 3f);

        Destroy(gameObject, timer);
    }
}
