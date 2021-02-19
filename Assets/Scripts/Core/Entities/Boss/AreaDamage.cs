using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;

public class AreaDamage : MonoBehaviour
{
    [SerializeField] private float damage = 40f;

    private void OnTriggerEnter(Collider other) {
        var playerStats = other.GetComponent<PlayerStats>();
        Debug.Log(other.gameObject.name);

        if (playerStats != null) {
            playerStats.Damage(damage);

            var dir = playerStats.transform.position - transform.position;
            dir.y = 0f;

            playerStats.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(400f, 600f), 0f, Random.Range(400f, 600f)), ForceMode.Force);
        }

        var recruit = other.GetComponent<Recruitable>();

        if (recruit != null) {
            if (Random.value > 0.8f)
                recruit.SelfDestroy();
        }
    }
}
