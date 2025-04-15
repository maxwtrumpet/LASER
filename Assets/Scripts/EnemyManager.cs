using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

// The component that spawns enemies.
public class EnemyManager : MonoBehaviour
{
    // Enemy data:
    // - The list of possible enemies to be spawned.
    // - Their relative rate of spawning.
    // - What time they first appear.
    // - Confirmed spawn times.
    // - Which confirmed spawn times we're currently on.
    [SerializeField] GameObject[] enemy_prefabs;
    [SerializeField] float[] frequencies;
    [SerializeField] float[] first_appearances;
    [SerializeField] NestedArray<float>[] confirmed_appearances;
    int[] cur_index;

    // The music manager prefab.
    [SerializeField] GameObject music_prefab;

    // The level time limit. -1 indicates endless mode.
    public float time_limit = 60.0f;

    // The time range between spawns.
    [SerializeField] Vector2 spawn_range = new Vector2(1.0f, 3.0f);

    // Wrapper object for the win/lose screens and the game.
    GameObject game_objects;
    GameObject win_screen;
    GameObject lose_screen;

    // The score text.
    TextMeshPro tmp;

    // Total points and the current bonus streak.
    public int kill_points = 0;
    int bonus_points = 0;

    // Total number of enemies and time elapsed.
    int enemy_count = 0;
    float time_elapsed = 0.0f;

    private void Start()
    {
        // Get the text component.
        tmp = GetComponent<TextMeshPro>();

        // Get the music manager (instantiating it if applicable) and update the music for the given level.
        GameObject mm = GameObject.FindGameObjectWithTag("music");
        if (mm == null) mm = Instantiate(music_prefab);
        mm.GetComponent<MusicManager>().StartLevel(int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name[5..]) - 1);

        // Get wrapper objects.
        game_objects = GameObject.FindGameObjectWithTag("GameController");
        lose_screen = GameObject.FindGameObjectWithTag("lose");
        win_screen = GameObject.FindGameObjectWithTag("win");

        // Subscribe to kill events.
        EventBus.Subscribe<KillEvent>(_OnKill);

        // Initialize all current confirmed spawn indicies to 0.
        cur_index = new int[enemy_prefabs.Length];
        for (int i = 0; i < cur_index.Length; i++) cur_index[i] = 0;

    }

    // Whenever enabled, start the enemy spawn coroutine.
    private void OnEnable()
    {
        StartCoroutine(SpawnEnemies());
    }

    // When receving a kill event:
    void _OnKill(KillEvent e)
    {
        // If this kill is valid for a bonus (not gnats):
        if (e.bonus)
        {
            // Add the event points plus bonus points.
            kill_points += e.points + bonus_points;

            // Increase the bonus and decrease the enemy count.
            bonus_points += 5;
            enemy_count--;

        }

        // Otherwise, just increase by the event points.
        else
        {
            kill_points += e.points;
        }

        // Update the score text.
        tmp.text = "Score: " + kill_points;
        
        // If the level is complete (past time limit, 0 enemies):
        if (time_limit != -1.0f && time_elapsed >= time_limit && enemy_count == 0)
        {
            // Get the current scene and update the high score if applicable.
            string scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
            if (PlayerPrefs.GetInt(scene) < kill_points) PlayerPrefs.SetInt(scene, kill_points);

            // Publish a win event, enable the win screen and disable the game.
            EventBus.Publish(new WinEvent());
            win_screen.SetActive(true);
            game_objects.SetActive(false);

        }

    }

    // Reset the kill bonus to 0.
    public void ResetBonus()
    {
        bonus_points = 0;
    }

    // Enemy spawning coroutine:
    IEnumerator SpawnEnemies()
    {
        // Spawn enemies while the level has not finished or there's a boss on the screen (forever in endless):
        while (time_limit == -1.0f || time_elapsed < time_limit || GameObject.FindGameObjectWithTag("boss") != null)
        {

            // If in endless mode, slowly decrease the spawn rate from 3 to 1 seconds from 300 to 600 seconds of pla ytime.
            if (time_limit == -1.0f && time_elapsed >= 300.0f) spawn_range.y = Mathf.Max(1.0f, 3.0f - (time_elapsed - 300.0f) / 150.0f);

            // Randomly pick a time to wait within the interval.
            float next_interval = Random.Range(spawn_range.x, spawn_range.y);
            yield return new WaitForSeconds(next_interval);

            // Store the previous time and increase the current time.
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

            // Instantiate the enemy and increase the count.
            Instantiate(new_enemy, game_objects.transform).GetComponent<HealthManager>().lose_screen = lose_screen;
            enemy_count++;

        }
    }
}
