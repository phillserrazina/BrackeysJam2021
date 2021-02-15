using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float speed = 50f;

    private Transform currentTarget;

    private Rigidbody rb;

    // EXECUTION FUNCTIONS
    private void Awake() {
        rb = GetComponent<Rigidbody>();    
    }

    void FixedUpdate()
    {
        transform.LookAt(currentTarget);
        rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            Destroy(gameObject);
            Destroy(other.gameObject);
            return;
        }

        var player = other.GetComponent<PlayerStats>();

        if (player == null) return;

        player.Damage(20f);
        Destroy(gameObject);
    }

    // METHODS
    public void Initialize(Transform target) {
        currentTarget = target;
    }
}
