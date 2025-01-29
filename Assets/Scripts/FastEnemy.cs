using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : MonoBehaviour
{

    Vector3 cur_dest = new Vector3(-1.0f, -1.0f, -1.0f);
    [SerializeField] float ease_factor = 0.2f;

    // Start is called before the first frame update
    void Initialize()
    {
        float top_or_bottom = Random.Range(0.0f, 25.0f);
        if (top_or_bottom <= 9.0f) transform.position = new Vector3(18.0f, Random.Range(0.0f, 7.5f), 0.0f);
        else transform.position = new Vector3(Random.Range(1.0f, 17.0f), 8.5f, 0.0f);
        cur_dest = transform.position;
        EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Slow", 1.0f));
    }

    private void OnEnable()
    {
        if (cur_dest.x == -1.0f) Initialize();
        StartCoroutine(Move());
    }

    // Update is called once per frame
    IEnumerator Move()
    {
        while (cur_dest.magnitude > 1.0f)
        {
            yield return new WaitForSeconds(Random.Range(0.75f, 1.25f));
            if (cur_dest.magnitude <= 2.5f)
            {
                cur_dest = new Vector3(0.0f, 0.0f, 0.0f);
            }
            else
            {
                float dist_diff = Random.Range(0.25f, 1.0f);
                float distance = cur_dest.magnitude - dist_diff;
                float angle = Random.Range(0, 1.4f);
                cur_dest = new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance, 0.0f);
            }
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x + (cur_dest.x - transform.position.x) * ease_factor,
                                         transform.position.y + (cur_dest.y - transform.position.y) * ease_factor,
                                         0.0f);
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("fast").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Slow", 0.0f));
    }
}
