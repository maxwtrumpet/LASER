using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : MonoBehaviour
{

    Vector2 vel = Vector2.zero;
    float speed_factor = -1.0f;
    Rigidbody2D rb;
    [SerializeField] GameObject smoke_prefab;
    float prev_distance = 0.0f;

    void Init()
    {
        GetComponent<Animator>().speed = 0.75f;
        rb = GetComponent<Rigidbody2D>();
        float top_or_bottom = Random.Range(0.0f, 25.0f);
        if (top_or_bottom <= 9.0f) transform.position = new Vector3(18.0f, Random.Range(0.0f, 7.5f), 0.0f);
        else transform.position = new Vector3(Random.Range(1.0f, 17.0f), 8.5f, 0.0f);
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -Mathf.Atan(transform.position.x/transform.position.y)*180/Mathf.PI));
        speed_factor = (transform.position.magnitude - 10.0f) * 0.625f + 4.0f;
        EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Fast", 1.0f));
        vel = transform.position;
        prev_distance = transform.position.magnitude;
        vel.Normalize();
        Instantiate(smoke_prefab, transform.parent).transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        if (speed_factor == -1.0f) Init();
        rb.velocity = new Vector2(-vel.x * speed_factor, -vel.y * speed_factor);
    }

    private void Update()
    {
        float cur_distance = transform.position.magnitude;
        if (prev_distance - cur_distance >= 0.16f)
        {
            Instantiate(smoke_prefab, transform.parent).transform.SetPositionAndRotation(transform.position, transform.rotation);
            prev_distance = cur_distance;
        }
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("kamikaze").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Fast", 0.0f));
    }

}
