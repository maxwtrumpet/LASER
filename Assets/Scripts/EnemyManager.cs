using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] GameObject[] enemy_prefabs;
    [SerializeField] float[] frequencies;
    float total_chance = 0.0f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        foreach(float frq in frequencies)
        {
            total_chance += frq;
        }
        yield return SpawnEnemies();
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 3));
            float distance = Random.Range(5, 10);
            float angle = Random.Range(0, Mathf.PI / 2);
            float enemy_type = Random.Range(0.0f, total_chance);
            GameObject new_enemy = null;
            float running_total = 0.0f;
            for (int i = 0; i < enemy_prefabs.Length; i++)
            {
                running_total += frequencies[i];
                if (enemy_type <= running_total)
                {
                    new_enemy = Instantiate(enemy_prefabs[i]);
                    break;
                }
            }
            new_enemy.GetComponent<Rigidbody2D>().transform.position = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0.0f);
        }
    }
}
