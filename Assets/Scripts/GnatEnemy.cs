using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnatEnemy : MonoBehaviour
{

    Rigidbody2D rb = null;
    [SerializeField] float gravity_scale = 1.0f;
    [SerializeField] Sprite[] sprites;
    SpriteRenderer sr;
    Vector2 cur_vel = Vector2.zero;

    private void OnEnable()
    {
        if (rb == null) Init();
        else rb.velocity = cur_vel;
        StartCoroutine(ChangeSprite());
    }

    // Start is called before the first frame update
    void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        float angle = Random.Range(0.0f, Mathf.PI*2);
        rb.velocity = new Vector2(Mathf.Cos(angle) * 2.0f, Mathf.Sin(angle) * 2.0f);
        rb.drag = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        cur_vel = rb.velocity;
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
        float angle = Mathf.Abs(Mathf.Atan(transform.position.y / transform.position.x));
        Vector2 gravity = new Vector2(Mathf.Sign(transform.position.x) * Mathf.Cos(angle) * -gravity_scale, Mathf.Sign(transform.position.y) * Mathf.Sin(angle) * -gravity_scale);
        rb.AddForce(gravity);
    }

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
