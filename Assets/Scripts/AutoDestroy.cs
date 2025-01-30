using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    [SerializeField] float lifetime = 0.5f;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f) Destroy(gameObject);
    }
}
