using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Kill event, which stores the enemy's points and if they're applicable for a bonus.
public class KillEvent
{
    public int points = 1;
    public bool bonus = true;
    public KillEvent(int _new_points) { points = _new_points; bonus = _new_points != 1; }
}

// The component managing enemy health.
public class HealthManager : MonoBehaviour
{

    // Total and current health.
    [SerializeField] int health = 4;
    int cur_health;

    // The points the enemy is worth.
    [SerializeField] int points = 1;

    // The size explosion this enemy gives and its prefab.
    [SerializeField] int explosion_index = 0;
    [SerializeField] GameObject explosion_prefab;

    // The dead boss sprite and countdown timer, if applicable.
    [SerializeField] Sprite boss_dead;
    float countdown = -1.0f;

    // Differnt animations for levels of damage and the animator component.
    [SerializeField] RuntimeAnimatorController damage_0;
    [SerializeField] RuntimeAnimatorController damage_1;
    [SerializeField] RuntimeAnimatorController damage_2;
    [SerializeField] RuntimeAnimatorController damage_3;
    Animator am;

    // The losing screen.
    public GameObject lose_screen;

    // The current beam being attacked by.
    GameObject cur_beam = null;

    private void Start()
    {

        // Set current health to the max.
        cur_health = health;

        // If not a gnat, set the animation to the default.
        if (points != 1)
        {
            am = GetComponent<Animator>();
            am.runtimeAnimatorController = damage_0;
        }
    }

    // For dead bosses, decrease the countdown and destroy when it's over.
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
        // If the enemy is dead and not in countdown mode:
        if (cur_health <= 0 && countdown == -1.0f)
        {

            // Publish explosion and kill events.
            EventBus.Publish(new ExplosionEvent(explosion_index));
            EventBus.Publish(new KillEvent(points));

            // If this is a boss:
            if (points == 1000)
            {
                // Instantiate an explosion and add a BE component,
                GameObject explosion = Instantiate(explosion_prefab, transform);
                explosion.transform.position = transform.position;
                BossExplosion be = explosion.AddComponent<BossExplosion>();

                // Initialize the BE elements.
                be.iteration = 0;
                be.explosion_prefab = explosion_prefab;

                // Disable all functional aspects of the boss and show the dead sprite.
                gameObject.GetComponent<BossEnemy>().enabled = false;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                gameObject.GetComponent<Rigidbody>().velocity = Vector2.zero;
                gameObject.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().sprite = boss_dead;

                // Start the countdown.
                countdown = 1.5f;
            }

            // Otherwise, just instantiate the appropriate explosion and destroy this enemy.
            else
            {
                Instantiate(explosion_prefab, transform.parent).transform.position = new Vector3(transform.position.x, transform.position.y, -0.02f);
                Destroy(gameObject);
            }
        }

        // Update the damage level based on remaining health.
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

    // When colliding with an obkect.
    private void OnTriggerEnter(Collider other)
    {
        // If this is a new beam, store the reference and subtract health accordingly.
        if (other.gameObject != cur_beam && other.TryGetComponent(out BeamManager bm))
        {
            cur_beam = other.gameObject;
            cur_health -= bm.damage;
        }

        // If this is the player:
        else if (other.name == "Center")
        {

            // Play an explosion effect and cancel most music layers.
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

            // If this is endless mode, update the maximum time and score if applicable.
            EnemyManager em = FindAnyObjectByType<EnemyManager>();
            if (em.time_limit == -1.0f)
            {
                if (PlayerPrefs.GetInt("EndlessScore") < em.kill_points) PlayerPrefs.SetInt("EndlessScore", em.kill_points);
                FindAnyObjectByType<EndlessTimer>().CheckTime();
            }

            // Turn on the lose screen and turn off the game.
            lose_screen.SetActive(true);
            GameObject.FindGameObjectWithTag("GameController").SetActive(false);

        }
    }

    // If exiting collision with a beam, remove the beam reference.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == cur_beam) cur_beam = null;
    }
}
