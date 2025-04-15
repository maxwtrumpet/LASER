using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The progress bar for levels.
public class ProgressBar : MonoBehaviour
{

    // The enemy manager object and total level time.
    EnemyManager em;
    float total_time = 0;

    // Get the enemy manager.
    void Start()
    {
        em = GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyManager>();
    }

    // Update the total time and change the progress bar proportionally.
    void Update()
    {
        total_time += Time.deltaTime;
        transform.localScale = new Vector3(Mathf.Min(3.125f, total_time / em.time_limit * 3.125f), 3.125f, 1.0f);
    }
}
