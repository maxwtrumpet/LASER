using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages the player beam.
public class BeamManager : MonoBehaviour
{
    // Desired y thickness for the beam.
    public float goal_thickness = 2.0f;

    // Total and remaining times to grow.
    public float grow_time;
    float remaining_time;

    // If the beam is growing or shrinking.
    bool up = true;

    // How much damage this beam does.
    public int damage;

    // All the animation frames and the current frame index.
    [SerializeField] Texture[] frames;
    int cur_index = 0;

    // The angle of the beam.
    float angle;

    // The Material component from the Renderer.
    private Material mat;

    void Start()
    {
        // Get the material.
        mat = GetComponent<Renderer>().material;

        // Initialize the scale and times.
        transform.localScale = new Vector3(0.0f, goal_thickness, 1.0f);
        grow_time = 0.2f;
        remaining_time = grow_time;

        // Set the angle to originate from the player.
        angle = Mathf.Atan(transform.position.y / transform.position.x);
    }

    // Animation coroutine.
    IEnumerator Animate()
    {
        // Loop forever.
        while (true)
        {
            // Every frame (roughly), update the frame index and the corresponding texture to the material.
            // Goes by 13 to make the animation look faster, and is prime to ensure all frames are used.
            yield return new WaitForSeconds(0.0167f);
            cur_index = (cur_index + 13) % 64;
            mat.SetTexture("_MainTex", frames[cur_index]);
        }
    }

    // When enabled, do the animation.
    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    // Every frame:
    void Update()
    {
        // If the beam hasn't reached full size and is growing:
        if (transform.localScale.x < 20.0f && up)
        {
            // Subtract time difference from remaining time.
            remaining_time -= Time.deltaTime;

            // Set the scale to be proportional to the remaining time.
            transform.localScale = new Vector3(Mathf.Min(20.0f,(grow_time - remaining_time) / grow_time * 20.0f), goal_thickness, 1.0f);

            // Uodate the position to be centered based on this new scale.
            transform.position = new Vector3(Mathf.Cos(angle) * (transform.localScale.x / 2.0f + 2.25f), Mathf.Sin(angle) * (transform.localScale.x / 2.0f + 2.25f), -0.01f);

            // Reset the texture scale to match the local scale.
            if (transform.localScale.x != 0.0f) mat.SetTextureScale("_MainTex", new Vector2(transform.localScale.x / 20.0f, 1.0f));
        }
        // Otherwiae, if just finished growing, swap state to shrinking.
        else if (up)
        {
            up = false;
        }
        // Otherwise, do the same as the first statement but reverse (shrinking).
        // If time is up, reset the bonus score and destroy the beam.
        else
        {
            remaining_time += Time.deltaTime;
            transform.localScale = new Vector3(Mathf.Max(0.0f, (grow_time - remaining_time) / grow_time * 20.0f), goal_thickness, 1.0f);
            transform.position = new Vector3(Mathf.Cos(angle) * (22.25f - transform.localScale.x / 2.0f), Mathf.Sin(angle) * (22.25f - transform.localScale.x / 2.0f), -0.01f);
            if (transform.localScale.x != 0.0f) mat.SetTextureScale("_MainTex", new Vector2(transform.localScale.x / 20.0f, 1.0f));
            if (remaining_time >= grow_time)
            {
                GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyManager>().ResetBonus();
                Destroy(gameObject);
            }
        }
    }

}
