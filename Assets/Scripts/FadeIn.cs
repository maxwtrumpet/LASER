using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The component for fading in the menu.
public class FadeIn : MonoBehaviour
{

    // The current alpha value and the SR component.
    float opacity = 1.0f;
    SpriteRenderer sr;

    // Get the SR.
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Decrease the alpha of the black screen until it's 0, then destroy it.
    void Update()
    {
        opacity -= 0.01f;
        if (opacity <= 0.0f) Destroy(gameObject);
        else sr.color = new Color(0.0f, 0.0f, 0.0f, opacity);
    }
}
