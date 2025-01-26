using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicSpawner : MonoBehaviour
{
    [SerializeField] GameObject manager_prefab;

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("music") == null) Instantiate(manager_prefab).GetComponent<MusicManager>();
    }
}
