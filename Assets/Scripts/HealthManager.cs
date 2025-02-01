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
    [SerializeField] GameObject explosion_prefab;
    [SerializeField] Sprite boss_dead;
    public GameObject lose_screen;
    float countdown = -1.0f;

    private void Update()
    {
        if (countdown != -1.0f)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0.0f) Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (health <= 0 && countdown == -1.0f)
        {
            EventBus.Publish(new ExplosionEvent(explosion_index));
            EventBus.Publish(new KillEvent(points));
            if (points == 1000)
            {
                GameObject explosion = Instantiate(explosion_prefab, transform);
                explosion.transform.position = transform.position;
                BossExplosion be = explosion.AddComponent<BossExplosion>();
                be.iteration = 0;
                be.explosion_prefab = explosion_prefab;
                gameObject.GetComponent<BossEnemy>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = boss_dead;
                countdown = 1.5f;
            }
            else
            {
                Instantiate(explosion_prefab, transform.parent).transform.position = transform.position;
                Destroy(gameObject);
            }
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
            EnemyManager em = FindAnyObjectByType<EnemyManager>();
            if (em.time_limit == -1.0f)
            {
                if (PlayerPrefs.GetInt("EndlessScore") < em.kill_points) PlayerPrefs.SetInt("EndlessScore", em.kill_points);
                FindAnyObjectByType<EndlessTimer>().CheckTime();
            }
            lose_screen.SetActive(true);
            GameObject.FindGameObjectWithTag("GameController").SetActive(false);
        }
    }
}
