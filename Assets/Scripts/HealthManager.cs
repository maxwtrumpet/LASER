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
    int cur_health;
    [SerializeField] int points = 1;
    [SerializeField] int explosion_index = 0;
    [SerializeField] GameObject explosion_prefab;
    [SerializeField] Sprite boss_dead;
    [SerializeField] RuntimeAnimatorController damage_0;
    [SerializeField] RuntimeAnimatorController damage_1;
    [SerializeField] RuntimeAnimatorController damage_2;
    [SerializeField] RuntimeAnimatorController damage_3;
    public GameObject lose_screen;
    Animator am;
    float countdown = -1.0f;
    GameObject cur_beam = null;

    private void Start()
    {
        cur_health = health;
        if (points != 1)
        {
            am = GetComponent<Animator>();
            am.runtimeAnimatorController = damage_0;
        }
    }

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
        if (cur_health <= 0 && countdown == -1.0f)
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
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().velocity = Vector2.zero;
                gameObject.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = boss_dead;
                countdown = 1.5f;
            }
            else
            {
                Instantiate(explosion_prefab, transform.parent).transform.position = new Vector3(transform.position.x, transform.position.y, -0.02f);
                Destroy(gameObject);
            }
        }
        else if (cur_health <= health / 4)
        {
            am.runtimeAnimatorController = damage_3;
        }
        else if (cur_health <= health / 2)
        {
            am.runtimeAnimatorController = damage_2;
        }
        else if (cur_health <= health * 3 / 4)
        {
            am.runtimeAnimatorController = damage_1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != cur_beam && other.TryGetComponent(out BeamManager bm))
        {
            cur_beam = other.gameObject;
            cur_health -= bm.damage;
        }
        else if (other.name == "Center")
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

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == cur_beam) cur_beam = null;
    }
}
