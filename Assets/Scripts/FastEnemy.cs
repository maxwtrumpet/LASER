using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemy : MonoBehaviour
{

    Vector3 cur_dest = new Vector3(-1.0f, -1.0f, -1.0f);
    [SerializeField] float ease_factor = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        cur_dest = transform.position;
        EventBus.Publish<MusicEvent>(new MusicEvent("Ostinato Slow", 1.0f));
    }

    private void OnEnable()
    {
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
                float angle = Random.Range(0, Mathf.PI / 2);
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
