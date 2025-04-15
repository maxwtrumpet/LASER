using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For objects with a set lifetime.
public class AutoDestroy : MonoBehaviour
{

    // Variable lifetime.
    [SerializeField] float lifetime = 0.5f;

    // Every frame:
    private void Update()
    {
        // Subtract time difference from remaining time. If none left, delete.
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f) Destroy(gameObject);
    }
}
