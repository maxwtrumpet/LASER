using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{

    EnemyManager em;
    float total_time = 0;

    // Start is called before the first frame update
    void Start()
    {
        em = GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        total_time += Time.deltaTime;
        transform.localScale = new Vector3(Mathf.Min(3.125f, total_time / em.time_limit * 3.125f), 3.125f, 1.0f);
    }
}
