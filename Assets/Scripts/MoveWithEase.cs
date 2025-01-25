using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithEase : MonoBehaviour
{

    [SerializeField] float ease_factor = 0.05f;
    public Vector3 desired_dest;

    // Update is called once per frame
    void Update()
    {
        if (transform.position != desired_dest)
        {
            transform.position += (desired_dest - transform.position) * ease_factor;
        }
    }
}
