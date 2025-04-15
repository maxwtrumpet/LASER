using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The kamikaze enemy.
public class KamikazeEnemy : MonoBehaviour
{

    // Velocity, previous distance, speed, and RB component.
    Vector2 vel = Vector2.zero;
    float prev_distance = 0.0f;
    float speed_factor = -1.0f;
    Rigidbody rb;

    // The smoke trail prefab.
    [SerializeField] GameObject smoke_prefab;

    void Init()
    {

        // Set animator speed and get RB component.
        GetComponent<Animator>().speed = 0.75f;
        rb = GetComponent<Rigidbody>();

        // Randomly pick whether enemy spawns on the top or right side of the screen.
        // 1/3 chance it spawns on the right side, otherwise top.
        float top_or_bottom = Random.Range(0.0f, 3.0f);

        // Set the position accordingly, with a random location.
        if (top_or_bottom <= 1.0f) transform.position = new Vector3(18.0f, Random.Range(0.0f, 8.5f), 0.0f);
        else transform.position = new Vector3(Random.Range(1.0f, 18.0f), 8.5f, 0.0f);

        // Set the rotation so the sprite faces the player.
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -Mathf.Atan(transform.position.x/transform.position.y)*180/Mathf.PI));

        // Make speed proportional to distance from player.
        speed_factor = (transform.position.magnitude - 10.0f) * 0.625f + 4.0f;

        // Get the velocity and normalize it. Also record the previous distance.
        vel = transform.position;
        prev_distance = transform.position.magnitude;
        vel.Normalize();

        // Spawn a smoke trail following the enemy.
        Instantiate(smoke_prefab, transform.parent).transform.SetPositionAndRotation(transform.position, transform.rotation);

        // Add the fast ostinato layer to the music.
        EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Fast", 1.0f));
    }

    void OnEnable()
    {
        // If speed not set, must initialize.
        if (speed_factor == -1.0f) Init();

        // Always set the velocity (resets when disabled).
        rb.velocity = new Vector2(-vel.x * speed_factor, -vel.y * speed_factor);
    }

    private void Update()
    {
        // When the enemy has traveled at least 0.16 units, spawn another smoke trail.
        float cur_distance = transform.position.magnitude;
        if (prev_distance - cur_distance >= 0.16f)
        {
            Instantiate(smoke_prefab, transform.parent).transform.SetPositionAndRotation(transform.position, transform.rotation);
            prev_distance = cur_distance;
        }
    }

    // When the last kamikaze destroyed, stop the fast ostinato layer.
    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("kamikaze").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Fast", 0.0f));
    }

}
