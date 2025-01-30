using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    Vector2 vel = Vector2.zero;
    float speed_factor = -1.0f;
    Rigidbody2D rb;

    void Init()
    {
        GetComponent<Animator>().speed = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        float top_or_bottom = Random.Range(0.0f, 25.0f);
        if (top_or_bottom <= 9.0f) transform.position = new Vector3(18.0f, Random.Range(0.0f, 7.5f), 0.0f);
        else transform.position = new Vector3(Random.Range(1.0f, 17.0f), 8.5f, 0.0f);
        speed_factor = transform.position.magnitude / 10.0f;
        vel = transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Atan(transform.position.y / transform.position.x) * 180 / Mathf.PI));
        vel.Normalize();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (speed_factor == -1.0f) Init();
        rb.velocity = new Vector2(-vel.x * speed_factor, -vel.y * speed_factor);
    }

}
