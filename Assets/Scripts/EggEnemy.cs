using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggEnemy : MonoBehaviour
{
    [SerializeField] GameObject gnat_prefab;
    GameObject lose_screen;
    float remaining_time = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        lose_screen = GetComponent<HealthManager>().lose_screen;
        float top_or_bottom = Random.Range(0.0f, 25.0f);
        if (top_or_bottom <= 9.0f)
        {
            transform.position = new Vector3(18.0f, Random.Range(0.0f, 6.5f), 0.0f);
            GetComponent<MoveWithEase>().desired_dest = new Vector3(16.0f, transform.position.y);
        }
        else {
            transform.position = new Vector3(Random.Range(0.0f, 16.0f), 8.5f, 0.0f);
            GetComponent<MoveWithEase>().desired_dest = new Vector3(transform.position.x, 6.5f);
        }
        EventBus.Publish<MusicEvent>(new MusicEvent("Bass Low", 1.0f));
    }

    private void OnEnable()
    {
        if (remaining_time <= 0.0f)
        {
            StartCoroutine(SpawnGnats());
        }
    }

    void Update()
    {
        if (remaining_time > 0.0f)
        {
            remaining_time -= Time.deltaTime;
            if (remaining_time <= 0.0f) StartCoroutine(SpawnGnats());
        }
    }

    IEnumerator SpawnGnats()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject gnat = Instantiate(gnat_prefab, transform.parent);
            gnat.transform.position = transform.position;
            gnat.GetComponent<HealthManager>().lose_screen = lose_screen;
        }
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("egg").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Bass Low", 0.0f));
    }

}
