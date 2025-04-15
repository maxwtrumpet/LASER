using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Component for resetting enemy beam bonuses.
public class BonusReset : MonoBehaviour
{
    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyManager>().ResetBonus();
    }
}
