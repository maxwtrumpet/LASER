using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Initializes the music manager.
public class MusicSpawner : MonoBehaviour
{

    // The MM pregab.
    [SerializeField] GameObject manager_prefab;

    // Initialize the MM if applicable and reset the music layers.
    void Start()
    {
        GameObject mm = GameObject.FindGameObjectWithTag("music");
        if (mm == null) Instantiate(manager_prefab);
        else mm.GetComponent<MusicManager>().Reset();
    }
}
