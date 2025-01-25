using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{

    [SerializeField] int health = 4;

    void LateUpdate()
    {
        if (health <= 0) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out AutoDestroy ad))
        {
            health -= ad.damage;
        }
        else if (collision.name == "Center")
        {
            Debug.Log("Game Over!");
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
