using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 vel = new Vector3(19.5f, Random.Range(1.5f, 11.5f), 0.0f);
        transform.position = vel;
        vel.Normalize();
        GetComponent<Rigidbody2D>().velocity = new Vector2(-vel.x / 2.5f, -vel.y / 2.5f);
        EventBus.Publish<MusicEvent>(new MusicEvent("Drum 3", 1.0f));
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("boss").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Drum 3", 0.0f));
    }

}
