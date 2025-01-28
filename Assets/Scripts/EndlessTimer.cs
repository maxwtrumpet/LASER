using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessTimer : MonoBehaviour
{
    float time_elapsed = 0.0f;
    TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        time_elapsed += Time.deltaTime;
        tmp.text = "Time: " + (int)time_elapsed;
    }
}
