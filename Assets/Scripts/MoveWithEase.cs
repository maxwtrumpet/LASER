using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component for moving egg enemies into frame.
public class MoveWithEase : MonoBehaviour
{

    // The ease factor and desired destination.
    [SerializeField] float ease_factor = 0.05f;
    public Vector3 desired_dest;

    // Move the current position closer to the desired location.
    void Update()
    {
        if (transform.position != desired_dest)
        {
            transform.position += (desired_dest - transform.position) * ease_factor;
        }
    }
}
