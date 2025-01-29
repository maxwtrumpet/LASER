using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamManager : MonoBehaviour
{
    public float goal_thickness = 3.09f;
    bool reached = false;
    public float lifetime = 0.25f;
    public int damage;

    private void Start()
    {
        transform.localScale = new Vector3(3.09f, goal_thickness, 1.0f);
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0.0f) Destroy(gameObject);
    }

}
