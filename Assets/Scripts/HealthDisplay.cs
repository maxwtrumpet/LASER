using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{

    private int prev_health = 4;
    int health = 4;

    SpriteRenderer sr;
    [SerializeField] Sprite cream;
    [SerializeField] Sprite brown;
    [SerializeField] Sprite pink;

    Subscription<ShootEvent> shoot_event_subscription;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (prev_health != health)
        {
            if (health == 3) sr.sprite = cream;
            else if (health == 2) sr.sprite = brown;
            else if (health == 1) sr.sprite = pink;
            else Destroy(gameObject);
        }
        prev_health = health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Controller ct))
        {
            Debug.Log("Subscribed!");
            shoot_event_subscription = EventBus.Subscribe<ShootEvent>(_OnShot);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Unsubscribed!");
        EventBus.Unsubscribe<ShootEvent>(shoot_event_subscription);
    }

    void _OnShot(ShootEvent e)
    {
        health -= e.damage;
    }
}
