using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    float opacity = 1.0f;
    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        opacity -= 0.01f;
        if (opacity <= 0.0f) Destroy(gameObject);
        else sr.color = new Color(0.0f, 0.0f, 0.0f, opacity);
    }
}
