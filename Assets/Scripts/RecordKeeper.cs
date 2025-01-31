using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordKeeper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TextMeshPro[] scores = GetComponentsInChildren<TextMeshPro>();
        scores[6].text = PlayerPrefs.GetInt("Level1") + "\n" + "Test" + "\n" + "Test";
    }
}
