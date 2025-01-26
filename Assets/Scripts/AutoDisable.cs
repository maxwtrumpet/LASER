using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    private void LateUpdate()
    {
        gameObject.SetActive(false);
        enabled = false;
    }
}
