using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;

public class BossAnimationHelper : MonoBehaviour
{
    [SerializeField] private BossAI boss = null;
    
    public void SpawnProjectile(BossProjectile projectile) {
        var proj = Instantiate(projectile, transform.position, transform.rotation);
        proj.Initialize(boss.Player.transform);
    }

    public void SlamAttack() {
        boss.SlamAttack();
    }
}
