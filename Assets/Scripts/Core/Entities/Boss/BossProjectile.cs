using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float speed = 50f;
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
        transform.LookAt(currentTarget);
        rb.velocity = transform.forward * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            SelfDestroy();
            Destroy(other.gameObject);
            return;
        }

        var player = other.GetComponent<PlayerStats>();

        if (player == null) return;

        player.Damage(5f);
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
