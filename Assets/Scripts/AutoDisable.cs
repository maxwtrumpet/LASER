using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For objects that have to start disabled but need other components to initialize.
public class AutoDisable : MonoBehaviour
{
    // Disable the game object and this componennt.
    // Re-enabling the object will not reactivate this componennt.
    private void LateUpdate()
    {
        gameObject.SetActive(false);
        enabled = false;
    }
}
