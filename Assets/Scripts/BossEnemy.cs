using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The componennt for the boss enemy.
public class BossEnemy : MonoBehaviour
{

    // Boss velocity.
    Vector3 vel = Vector3.zero;

    void Init()
    {
        // Set animation speed.
        GetComponent<Animator>().speed = 0.75f;

        // Randomize position to be somewhere on the right edge of the screen.
        transform.position = new Vector3(19.5f, Random.Range(1.25f, 8.25f), 0.0f);

        // Set velocity so it moves toward the player and normalize.
        vel = new Vector3(-transform.position.x, 1.25f - transform.position.y, 0.0f);
        vel.Normalize();

        // Turn on boss drum overlay.
        EventBus.Publish<MusicEvent>(new MusicEvent("Drum 3", 1.0f));

        // Set the rotation so the sprite faces the player.
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Atan(vel.y / vel.x) * 180 / Mathf.PI));
    }

    private void OnEnable()
    {
        // If speed not set, must initialize.
        if (vel == Vector3.zero) Init();

        // Always set the velocity (resets when disabled).
        GetComponent<Rigidbody>().velocity = new Vector2(vel.x / 2.5f, vel.y / 2.5f);
    }

    // When destroyed and no other bosses remaining, stop the drum overlay.
    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("boss").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Drum 3", 0.0f));
    }

}
