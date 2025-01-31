using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossExplosion : MonoBehaviour
{

    public int iteration = 0;
    public GameObject explosion_prefab;
    public bool[] used_positions = { false, false, false };
    Vector3[] positions = { new Vector3(-0.15f, 0.35f, 0.0f),
                            new Vector3(-0.3f, -0.45f, 0.0f),
                            new Vector3(0.4f, -0.125f, 0.0f)};

    private void Start()
    {
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        if (iteration == 0)
        {
            int location = Mathf.RoundToInt(Random.Range(-0.499f, 2.499f));
            transform.localPosition = positions[location];
            used_positions[location] = true;
        }
        else if (iteration == 1)
        {
            float location = Random.Range(0.0f, 1.0f);
            int index;
            if (location < 0.5f)
            {
                if (used_positions[0]) index = 1;
                else index = 0;
            }
            else
            {
                if (used_positions[2]) index = 1;
                else index = 2;
            }
            transform.localPosition = positions[index];
            used_positions[index] = true;
        }
        else
        {
            if (!used_positions[0]) transform.localPosition = positions[0];
            else if (!used_positions[1]) transform.localPosition = positions[1];
            else transform.localPosition = positions[2];
        }
    }

    private void OnDestroy()
    {
        if (iteration != 2)
        {
            EventBus.Publish(new ExplosionEvent(4));
            GameObject new_explosion = Instantiate(explosion_prefab, transform.parent);
            BossExplosion be = new_explosion.AddComponent<BossExplosion>();
            be.iteration = iteration + 1;
            be.explosion_prefab = explosion_prefab;
            be.used_positions = used_positions;
        }
    }
}
