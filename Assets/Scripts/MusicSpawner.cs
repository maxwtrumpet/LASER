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
        GameObject mm = GameObject.FindGameObjectWithTag("music");
        if (mm == null) Instantiate(manager_prefab);
        else mm.GetComponent<MusicManager>().Reset();
    }
}
