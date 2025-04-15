using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The basic slow enemy.
public class BasicEnemy : MonoBehaviour
{
    // Velocity, speed, and RB component.
    Vector2 vel = Vector2.zero;
    float speed_factor = -1.0f;
    Rigidbody rb;


    void Init()
    {
        // Set animator speed and get RB component.
        GetComponent<Animator>().speed = 0.5f;
        rb = GetComponent<Rigidbody>();

        // Randomly pick whether enemy spawns on the top or right side of the screen.
        // 1/3 chance it spawns on the right side, otherwise top.
        float top_or_bottom = Random.Range(0.0f, 3.0f);

        // Set the position accordingly, with a random location.
        if (top_or_bottom <= 1.0f) transform.position = new Vector3(18.0f, Random.Range(0.0f, 8.5f), 0.0f);
        else transform.position = new Vector3(Random.Range(1.0f, 18.0f), 8.5f, 0.0f);

        // Make speed proportional to distance from player.
        speed_factor = transform.position.magnitude / 10.0f;

        // Get the velocity and normalize it.
        vel = transform.position;
        vel.Normalize();

        // Set the rotation so the sprite faces the player.
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Atan(transform.position.y / transform.position.x) * 180 / Mathf.PI));
        
    }

    void OnEnable()
    {
        // If speed not set, must initialize.
        if (speed_factor == -1.0f) Init();

        // Always set the velocity (resets when disabled).
        rb.velocity = new Vector2(-vel.x * speed_factor, -vel.y * speed_factor);
    }

}
