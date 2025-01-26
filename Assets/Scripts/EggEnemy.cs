using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggEnemy : MonoBehaviour
{
    [SerializeField] GameObject gnat_prefab;
    float remaining_time = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.position.x == 18.0f) GetComponent<MoveWithEase>().desired_dest = new Vector3(16.0f, transform.position.y);
        else GetComponent<MoveWithEase>().desired_dest = new Vector3(transform.position.x, 8.0f);
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
            Instantiate(gnat_prefab, transform).transform.position = transform.position;
        }
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectsWithTag("egg").Length == 0) EventBus.Publish<MusicEvent>(new MusicEvent("Bass Low", 0.0f));
    }

}
