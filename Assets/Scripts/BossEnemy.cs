using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{

    Vector3 vel = Vector3.zero;

    void Init()
    {
        GetComponent<Animator>().speed = 0.75f;
        transform.position = new Vector3(19.5f, Random.Range(1.25f, 8.25f), 0.0f);
        vel = new Vector3(-transform.position.x, 1.25f - transform.position.y, 0.0f);
        vel.Normalize();
        EventBus.Publish<MusicEvent>(new MusicEvent("Drum 3", 1.0f));
        transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Atan(vel.y / vel.x) * 180 / Mathf.PI));
    }

    private void OnEnable()
    {
        if (vel == Vector3.zero) Init();
        GetComponent<Rigidbody>().velocity = new Vector2(vel.x / 2.5f, vel.y / 2.5f);
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("boss").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Drum 3", 0.0f));
    }

}
