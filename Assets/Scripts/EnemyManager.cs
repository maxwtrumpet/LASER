using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] GameObject[] enemy_prefabs;
    [SerializeField] float[] frequencies;
    [SerializeField] float[] first_appearances;
    float[][] guranteed_times;
    int[] cur_index;
    float time_elapsed = 0.0f;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        cur_index = new int[enemy_prefabs.Length];
        for (int i = 0; i < cur_index.Length; i++)
        {
            cur_index[i] = 0;
        }

        // Have to do this manually because you can't do double array serialized field...
        guranteed_times = new float[enemy_prefabs.Length][];
        guranteed_times[0] = new float[1];
        guranteed_times[0][0] = 0.0f;
        guranteed_times[1] = new float[1];
        guranteed_times[1][0] = 15.0f;
        guranteed_times[2] = new float[1];
        guranteed_times[2][0] = 30.0f;
        guranteed_times[3] = new float[1];
        guranteed_times[3][0] = 45.0f;
        guranteed_times[4] = new float[5];
        guranteed_times[4][0] = 60.0f;
        guranteed_times[4][1] = 120.0f;
        guranteed_times[4][2] = 180.0f;
        guranteed_times[4][3] = 240.0f;
        guranteed_times[4][4] = 300.0f;

        yield return SpawnEnemies();
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {

            // Get random time between 1 and 3 seconds to spawn new enemy. Add it to elapsed time.
            float next_interval = Random.Range(1, 3);
            yield return new WaitForSeconds(next_interval);
            time_elapsed += next_interval;

            // Loop throguh all enemies and see if the time has past for them to start spawning.
            // If this is the first time they're appearing, gurantee they spawn.
            float total_chance = 0.0f;
            int auto_appear = -1;
            for (int i = 0; i < first_appearances.Length; i++)
            {
                if (time_elapsed >= first_appearances[i])
                {
                    total_chance += frequencies[i];
                    if (cur_index[i] < guranteed_times[i].Length && time_elapsed >= guranteed_times[i][cur_index[i]] && time_elapsed - next_interval < guranteed_times[i][cur_index[i]])
                    {
                        auto_appear = i;
                        cur_index[i]++;
                    }
                }
            }

            // Choose the enemy to spawn. Randomize based on the weighting or choose the first appearance enemy.
            GameObject new_enemy = null;
            if (auto_appear == -1)
            {
                float enemy_type = Random.Range(0.0f, total_chance);
                float running_total = 0.0f;
                for (int i = 0; i < enemy_prefabs.Length; i++)
                {
                    running_total += frequencies[i];
                    if (enemy_type <= running_total)
                    {
                        new_enemy = enemy_prefabs[i];
                        break;
                    }
                }
            }
            else
            {
                new_enemy = enemy_prefabs[auto_appear];
            }

            // Instantiate the enemy and spawn them on the edge of the FOV.
            new_enemy = Instantiate(new_enemy);
            float top_or_bottom = Random.Range(0.0f, 25.0f);
            if (top_or_bottom <= 9.0f) new_enemy.GetComponent<Rigidbody2D>().transform.position = new Vector3(18.0f, Random.Range(0.0f,9.0f), 0.0f);
            else new_enemy.GetComponent<Rigidbody2D>().transform.position = new Vector3(Random.Range(0.0f, 17.0f), 10.0f, 0.0f);

        }
    }
}
