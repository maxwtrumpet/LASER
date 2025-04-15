using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Boss explosion effect when it dies.
public class BossExplosion : MonoBehaviour
{

    // Which explosion # this is.
    public int iteration = 0;

    // The explosion prefab.
    public GameObject explosion_prefab;

    // The list of positions and whether or not they've been used yet.
    public bool[] used_positions = { false, false, false };
    Vector3[] positions = { new Vector3(-0.15f, 0.35f, -0.02f),
                            new Vector3(-0.3f, -0.45f, -0.02f),
                            new Vector3(0.4f, -0.125f, -0.02f)};

    private void Start()
    {

        // Reset the scale.
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // If first explosion, pick any location and mark it as used.
        if (iteration == 0)
        {
            int location = Mathf.RoundToInt(Random.Range(-0.499f, 2.499f));
            transform.localPosition = positions[location];
            used_positions[location] = true;
        }

        // If second iteration, randomly pick one of the remaining two locations.
        // I know there's a better way to do this. Don't have much time left for submission.
        else if (iteration == 1)
        {
            float location = Random.Range(0.0f, 1.0f);
            int index;
            if (location < 0.5f)
            {
                if (used_positions[0]) index = 1;
                else index = 0;
            }
            else
            {
                if (used_positions[2]) index = 1;
                else index = 2;
            }
            transform.localPosition = positions[index];
            used_positions[index] = true;
        }

        // If last iteration, use remaining position.
        else
        {
            if (!used_positions[0]) transform.localPosition = positions[0];
            else if (!used_positions[1]) transform.localPosition = positions[1];
            else transform.localPosition = positions[2];
        }
    }

    private void OnDestroy()
    {
        // If there are still more explosions to spawn:
        if (iteration != 2)
        {
            // Publish an explosion event.
            EventBus.Publish(new ExplosionEvent(4));

            // Make a new explosion object and add the boss explosion component to it.
            GameObject new_explosion = Instantiate(explosion_prefab, transform.parent);
            BossExplosion be = new_explosion.AddComponent<BossExplosion>();

            // Increase the iteration and transfer the prefab/used positions.
            be.iteration = iteration + 1;
            be.explosion_prefab = explosion_prefab;
            be.used_positions = used_positions;
        }
    }
}
