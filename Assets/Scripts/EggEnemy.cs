using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggEnemy : MonoBehaviour
{
    [SerializeField] GameObject gnat_prefab;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(10.0f);
        yield return SpawnGnats();
    }

    IEnumerator SpawnGnats()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Instantiate(gnat_prefab).transform.position = transform.position;
        }
    }
}
