using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The component for fast enemies.
public class FastEnemy : MonoBehaviour
{

    // The current destination the enemy is traveling to and its ease factor.
    Vector3 cur_dest = new Vector3(-1.0f, -1.0f, -1.0f);
    [SerializeField] float ease_factor = 0.2f;

    void Initialize()
    {
        // Set the animation speed to a fixed value.
        GetComponent<Animator>().speed = 0.75f;

        // Randomly pick whether enemy spawns on the top or right side of the screen.
        // 1/3 chance it spawns on the right side, otherwise top.
        float top_or_bottom = Random.Range(0.0f, 3.0f);

        // Set the position and current destintion accordingly, with a random location.
        if (top_or_bottom <= 1.0f) transform.position = new Vector3(18.0f, Random.Range(0.0f, 8.5f), 0.0f);
        else transform.position = new Vector3(Random.Range(1.0f, 18.0f), 8.5f, 0.0f);
        cur_dest = transform.position;

        // Start the slow ostinato layer.
        EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Slow", 1.0f));
    }

    // When enabled, initialize if applicable and always run the moving coroutine.
    private void OnEnable()
    {
        if (cur_dest.x == -1.0f) Initialize();
        StartCoroutine(Move());
    }

    // Move coroutine.
    IEnumerator Move()
    {
        // While the enemy has not reached the player:
        while (cur_dest.magnitude > 1.0f)
        {

            // Wait for a random amount of time to move.
            yield return new WaitForSeconds(Random.Range(0.75f, 1.25f));

            // If the current destination is close enough, head directly for the player.
            if (cur_dest.magnitude <= 2.5f)
            {
                cur_dest = new Vector3(0.0f, 0.0f, 0.0f);
            }

            // Otherwise, pick a random amount to move and angle to the player. Update current destination accordingly.
            else
            {
                float dist_diff = Random.Range(0.25f, 1.0f);
                float distance = cur_dest.magnitude - dist_diff;
                float angle = Random.Range(0, 1.4f);
                cur_dest = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0.0f);
            }

        }
    }

    // Update the position to the current destination with an ease factor.
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x + (cur_dest.x - transform.position.x) * ease_factor,
                                         transform.position.y + (cur_dest.y - transform.position.y) * ease_factor,
                                         0.0f);
    }

    // When the last fast enemy destroyed, stop the slow ostinato layer.
    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("fast").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Slow", 0.0f));
    }
}
