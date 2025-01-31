using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillEvent
{
    public int points = 1;
    public bool bonus = true;
    public KillEvent(int _new_points) { points = _new_points; bonus = _new_points != 1; }
}

public class HealthManager : MonoBehaviour
{

    [SerializeField] int health = 4;
    [SerializeField] int points = 1;
    [SerializeField] int explosion_index = 0;
    public GameObject lose_screen;

    void LateUpdate()
    {
        if (health <= 0)
        {
            EventBus.Publish(new ExplosionEvent(explosion_index));
            EventBus.Publish(new KillEvent(points));
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out BeamManager bm))
        {
            health -= bm.damage;
        }
        else if (collision.name == "Center")
        {
            EventBus.Publish(new ExplosionEvent(4));
            EventBus.Publish(new MusicEvent("Ab Resolve", 0.0f));
            EventBus.Publish(new MusicEvent("Ab Stay", 0.0f));
            EventBus.Publish(new MusicEvent("Bass High", 0.0f));
            EventBus.Publish(new MusicEvent("Bass Low", 0.0f));
            EventBus.Publish(new MusicEvent("Bb High", 0.0f));
            EventBus.Publish(new MusicEvent("Bb Low", 1.0f));
            EventBus.Publish(new MusicEvent("Drum 1", 0.0f));
            EventBus.Publish(new MusicEvent("Drum 2", 0.0f));
            EventBus.Publish(new MusicEvent("Drum 3", 0.0f));
            EventBus.Publish(new MusicEvent("Eb", 0.0f));
            EventBus.Publish(new MusicEvent("F", 0.0f));
            EventBus.Publish(new MusicEvent("Melody High", 0.0f));
            EventBus.Publish(new MusicEvent("Melody Low", 1.0f));
            EventBus.Publish(new MusicEvent("Ostinato Fast", 0.0f));
            EventBus.Publish(new MusicEvent("Ostinato Slow", 0.0f));
            lose_screen.SetActive(true);
            GameObject.FindGameObjectWithTag("GameController").SetActive(false);
        }
    }
}
