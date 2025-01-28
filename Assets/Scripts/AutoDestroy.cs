using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{

    public float lifetime;
    public int damage;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f) Destroy(gameObject);
    }

}
