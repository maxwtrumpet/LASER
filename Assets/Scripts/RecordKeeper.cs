using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecordKeeper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("played", 1);
        TextMeshPro[] scores = GetComponentsInChildren<TextMeshPro>();
        scores[6].text = PlayerPrefs.GetInt("Level1") + "\n" + PlayerPrefs.GetInt("Level4") + "\n" + PlayerPrefs.GetInt("Level7");
        scores[7].text = PlayerPrefs.GetInt("Level2") + "\n" + PlayerPrefs.GetInt("Level5") + "\n" + PlayerPrefs.GetInt("Level8");
        scores[8].text = PlayerPrefs.GetInt("Level3") + "\n" + PlayerPrefs.GetInt("Level6") + "\n" + PlayerPrefs.GetInt("Level9");
        scores[9].text = PlayerPrefs.GetInt("EndlessTime") + " seconds";
        scores[10].text = PlayerPrefs.GetInt("EndlessScore") + " points";
    }
}
