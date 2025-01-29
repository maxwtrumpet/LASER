using System.Collections;
using System.Collections.Generic;
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
    public GameObject lose_screen;

    void LateUpdate()
    {
        if (health <= 0)
        {
            EventBus.Publish<KillEvent>(new KillEvent(points));
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
            lose_screen.SetActive(true);
            GameObject.FindGameObjectWithTag("GameController").SetActive(false);
        }
    }
}
