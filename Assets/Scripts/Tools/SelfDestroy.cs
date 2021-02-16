using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float delay = 2f;

    private void Awake() {
        Destroy(gameObject, delay);
    }
}
