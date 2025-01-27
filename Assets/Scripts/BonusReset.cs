using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusReset : MonoBehaviour
{
    private void OnDestroy()
    {
        GameObject.FindGameObjectWithTag("enemy").GetComponent<EnemyManager>().ResetBonus();
    }
}
