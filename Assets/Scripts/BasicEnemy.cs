using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Vector3 vel = transform.position;
        vel.Normalize();
        GetComponent<Rigidbody2D>().velocity = new Vector2(-vel.x, -vel.y);
    }

}
