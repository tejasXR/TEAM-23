using System;
using System.Linq;
using UnityEngine;

public class ColoredObject : MonoBehaviour {
    public Category objectColor;

    private bool inside;
    
    private void Awake() {
        GameState.Instance.AddTotalCounter();
    }

    private void OnTriggerEnter(Collider other) {
        var buckets = other.gameObject.GetComponentsInParent<ColoredBucket>();
        var valid = buckets.Any() && buckets[0].insideZone == other && buckets[0].color == objectColor;
        if (!valid) return;
        SetInside();
        GameState.Instance.AddCompletedCounter();
    }

    private void SetInside() {
        inside = true;
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders) {
            collider.enabled = false;
        }

        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }
    
    
}