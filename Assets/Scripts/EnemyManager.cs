using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] GameObject[] enemy_prefabs;
    [SerializeField] float[] frequencies;
    [SerializeField] float[] first_appearances;
    [SerializeField] NestedArray<float>[] confirmed_appearances;
    [SerializeField] GameObject music_prefab;
    public float time_limit = 60.0f;
    [SerializeField] Vector2 spawn_range = new Vector2(1.0f, 3.0f);
    GameObject game_objects;
    GameObject win_screen;
    GameObject lose_screen;
    TextMeshPro tmp;
    public int kill_points = 0;
    int bonus_points = 0;
    int[] cur_index;
    int enemy_count = 0;
    float time_elapsed = 0.0f;
    // Start is called before the first frame update
    private void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        GameObject mm = GameObject.FindGameObjectWithTag("music");
        if (mm == null) mm = Instantiate(music_prefab);
        mm.GetComponent<MusicManager>().StartLevel(int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name[5..]) - 1);
        game_objects = GameObject.FindGameObjectWithTag("GameController");
        lose_screen = GameObject.FindGameObjectWithTag("lose");
        win_screen = GameObject.FindGameObjectWithTag("win");
        EventBus.Subscribe<KillEvent>(_OnKill);
        cur_index = new int[enemy_prefabs.Length];
        for (int i = 0; i < cur_index.Length; i++)
        {
            cur_index[i] = 0;
        }

    }

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemies());
    }

    void _OnKill(KillEvent e)
    {
        if (e.bonus)
        {
            kill_points += e.points + bonus_points;
            bonus_points += 5;
            enemy_count--;
        }
        else
        {
            kill_points += e.points;
        }
        tmp.text = "Score: " + kill_points;
        
        if (time_limit != -1.0f && time_elapsed >= time_limit && enemy_count == 0)
        {
            string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (PlayerPrefs.GetInt(scene) < kill_points) PlayerPrefs.SetInt(scene, kill_points);
            win_screen.SetActive(true);
            game_objects.SetActive(false);
        }
    }

    public void ResetBonus()
    {
        bonus_points = 0;
    }

    IEnumerator SpawnEnemies()
    {
        while (time_limit == -1.0f || time_elapsed < time_limit || GameObject.FindGameObjectWithTag("boss") != null)
        {
            if (time_limit == -1.0f) spawn_range.y = 3.0f - time_elapsed / 300.0f;
            float next_interval; next_interval = Random.Range(spawn_range.x, spawn_range.y);
            yield return new WaitForSeconds(next_interval);
            float prev_time = time_elapsed;
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
                    if (cur_index[i] < confirmed_appearances[i].row.Length && time_elapsed >= confirmed_appearances[i].row[cur_index[i]] && prev_time < confirmed_appearances[i].row[cur_index[i]])
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
            Instantiate(new_enemy, game_objects.transform).GetComponent<HealthManager>().lose_screen = lose_screen;
            enemy_count++;

        }
    }
}
