using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The egg enemy.
public class EggEnemy : MonoBehaviour
{
    // The prefab for the gnats.
    [SerializeField] GameObject gnat_prefab;

    // The lose screen.
    GameObject lose_screen;

    // Remaining time till gnats spawn.
    float remaining_time = 10.0f;

    // The animator component.
    Animator am;

    void Start()
    {
        // Get the animator and set its speed.
        am = GetComponent<Animator>();
        am.speed = 0.5f;

        // Get the lose screen from the health manager.
        lose_screen = GetComponent<HealthManager>().lose_screen;

        // Randomly pick whether enemy spawns on the top or right side of the screen proportional to the screen dimensions.
        float top_or_bottom = Random.Range(0.0f, 19.0f);

        // Set the position and final destination accordingly, with a random location.
        if (top_or_bottom <= 5.0f)
        {
            transform.position = new Vector3(18.0f, Random.Range(1.5f, 6.5f), 0.0f);
            GetComponent<MoveWithEase>().desired_dest = new Vector3(16.0f, transform.position.y);
        }
        else {
            transform.position = new Vector3(Random.Range(2.0f, 16.0f), 8.5f, 0.0f);
            GetComponent<MoveWithEase>().desired_dest = new Vector3(transform.position.x, 6.5f);
        }

        // Add the low bass layer to the music.
        EventBus.Publish<MusicEvent>(new MusicEvent("Bass Low", 1.0f));

    }

    private void OnEnable()
    {
        // If no remaining time, immediately spawn gnats.
        if (remaining_time <= 0.0f) StartCoroutine(SpawnGnats());
    }

    void Update()
    {
        // If there's any remaining time, subtract the time delta and increase the animation speed proportionally.
        // Spwan gnats when it hits 0.
        if (remaining_time > 0.0f)
        {
            remaining_time -= Time.deltaTime;
            am.speed = (10.0f - remaining_time) / 10.0f * 1.5f + 0.5f;
            if (remaining_time <= 0.0f) StartCoroutine(SpawnGnats());
        }
    }

    IEnumerator SpawnGnats()
    {
        // Every 0.1 seconds, spawn a gnat.
        // Set its transform and lose screen prefab.
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject gnat = Instantiate(gnat_prefab, transform.parent);
            gnat.transform.position = transform.position;
            gnat.GetComponent<HealthManager>().lose_screen = lose_screen;
        }
    }

    // When the last egg destroyed, stop the low bass track.
    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("egg").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Bass Low", 0.0f));
    }

}
