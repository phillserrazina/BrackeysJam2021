using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrackeysJam.Core.Entities;

public class RecruitsGroupMovementManager : MonoBehaviour
{
    // VARIABLES
    private List<Recruitable> recruits = new List<Recruitable>();

    public static RecruitsGroupMovementManager Instance { get; private set; }

    // EXECUTION FUNCTIONS
    private void Awake() {
        Instance = this;
    }

    private void Update() {
        foreach (var r in recruits) {
            if (Vector3.Distance(r.transform.position, r.Leader.transform.position) < 2f) {
                Halt();
                return;
            }
        }

        Resume();
    }

    // METHODS
    public void Add(Recruitable r) {
        recruits.Add(r);
    }

    public void Remove(Recruitable r) {
        if (recruits.Contains(r))
            recruits.Remove(r);
    }

    public void Halt() {
        foreach (var r in recruits) {
            r.CanMove = false;
        }
    }

    public void Resume() {
        foreach (var r in recruits) {
            r.CanMove = true;
        }
    }
}
