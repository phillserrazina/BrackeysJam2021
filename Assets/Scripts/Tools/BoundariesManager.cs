using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundariesManager : MonoBehaviour
{
    // VARIABLES
    private Collider myCollider;

    public static BoundariesManager Instance { get; private set; }

    // EXECUTION FUNCTIONS
    private void Awake() {
        Instance = this;

        myCollider = GetComponent<Collider>();
    }

    // METHODS
    public Vector3 GetRandomPoint() {
        Bounds bounds = myCollider.bounds;

        return new Vector3 (
            Random.Range(bounds.min.x, bounds.max.x),
            0f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
