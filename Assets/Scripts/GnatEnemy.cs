using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnatEnemy : MonoBehaviour
{

    // RB and SR components.
    Rigidbody rb = null;
    SpriteRenderer sr;

    // "Gravity" force towards the player.
    [SerializeField] float gravity_scale = 1.0f;

    // All different gnat sprites.
    [SerializeField] Sprite[] sprites;
    
    // Current velocity.
    Vector2 cur_vel = Vector2.zero;

    // When enabled:
    private void OnEnable()
    {
        // If not initialized, do so. Otherwise, set the velocity.
        if (rb == null) Init();
        else rb.velocity = cur_vel;

        // Run the sprite coroutine.
        StartCoroutine(ChangeSprite());

    }

    void Init()
    {
        // Get the components.
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody>();

        // Pick a random angle to fly from the egg, and set the velocity accordingly.
        float angle = Random.Range(0.0f, Mathf.PI*2);
        rb.velocity = new Vector2(Mathf.Cos(angle) * 2.0f, Mathf.Sin(angle) * 2.0f);

        // Modify the drag to be 0.1.
        rb.drag = 0.1f;
    }

    void Update()
    {
        // Store the current velocity.
        cur_vel = rb.velocity;

        // Wrap the movement around the bottom and left side of the screen.
        // Maintains velocity.
        if (transform.position.y < 0.0f)
        {
            transform.position = new Vector3(0.0f, transform.position.x, 0.0f);
            rb.velocity = new Vector3(-rb.velocity.y, rb.velocity.x);
        }
        else if (transform.position.x < 0.0f)
        {
            transform.position = new Vector3(transform.position.y, 0.0f, 0.0f);
            rb.velocity = new Vector3(rb.velocity.y, -rb.velocity.x);
        }

        // Calculate new gravity force based on the angle to the player and apply it.
        float angle = Mathf.Abs(Mathf.Atan(transform.position.y / transform.position.x));
        Vector2 gravity = new Vector2(Mathf.Sign(transform.position.x) * Mathf.Cos(angle) * -gravity_scale, Mathf.Sign(transform.position.y) * Mathf.Sin(angle) * -gravity_scale);
        rb.AddForce(gravity);
    }

    // Sprite changing coroutine. Randomly change sprites every half second.
    IEnumerator ChangeSprite()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            int sprite_num = (int)Random.Range(0, 3.999f);
            sr.sprite = sprites[sprite_num];
        }
    }

}
