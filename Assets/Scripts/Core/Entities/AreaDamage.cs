using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        var playerStats = other.GetComponent<PlayerStats>();

        if (playerStats != null) {
            Debug.Log("Hit");

            playerStats.Damage(20f);

            var dir = playerStats.transform.position - transform.position;
            dir.y = 0f;

            playerStats.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(400f, 600f), 0f, Random.Range(400f, 600f)), ForceMode.Force);
        }
    }
}
