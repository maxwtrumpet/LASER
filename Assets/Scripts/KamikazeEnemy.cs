using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KamikazeEnemy : MonoBehaviour
{
    float speed_factor = -1.0f;
    // Start is called before the first frame update
    void OnEnable()
    {
        if (speed_factor == -1.0f)
        {
            speed_factor = (transform.position.magnitude - 10.0f) * 0.5f + 6.0f;
            EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Fast", 1.0f));
        }
        Debug.Log(transform.position);
        Vector3 vel = transform.position;
        vel.Normalize();
        GetComponent<Rigidbody2D>().velocity = new Vector2(-vel.x * speed_factor, -vel.y * speed_factor);
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("kamikaze").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Fast", 0.0f));
    }

}
