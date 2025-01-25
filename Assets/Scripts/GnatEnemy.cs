using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnatEnemy : MonoBehaviour
{

    Rigidbody2D rb;
    [SerializeField] float gravity_scale = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        float angle = Random.Range(0.0f, Mathf.PI*2);
        rb.velocity = new Vector2(Mathf.Cos(angle) * 0.8f, Mathf.Sin(angle) * 0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Abs(Mathf.Atan(transform.position.y / transform.position.x));
        Vector2 gravity = new Vector2(Mathf.Sign(transform.position.x) * Mathf.Cos(angle) * -gravity_scale, Mathf.Sign(transform.position.y) * Mathf.Sin(angle) * -gravity_scale);
        rb.AddForce(gravity);
    }
}
