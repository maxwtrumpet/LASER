using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 vel = transform.position;
        float distance = transform.position.magnitude;
        float speed_factor = (distance - 10.0f) * 0.5f + 6.0f;
        vel.Normalize();
        GetComponent<Rigidbody2D>().velocity = new Vector2(-vel.x * speed_factor, -vel.y * speed_factor);
    }

}
