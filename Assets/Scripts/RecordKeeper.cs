using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// The component displying personal records.
public class RecordKeeper : MonoBehaviour
{
    void Start()
    {
        // Mark the game as played.
        PlayerPrefs.SetInt("played", 1);

        // Get all the text displays from the component's children.
        TextMeshPro[] scores = GetComponentsInChildren<TextMeshPro>();

        // Update the level scores based on stored data.
        scores[6].text = PlayerPrefs.GetInt("Level1") + "\n" + PlayerPrefs.GetInt("Level4") + "\n" + PlayerPrefs.GetInt("Level7");
        scores[7].text = PlayerPrefs.GetInt("Level2") + "\n" + PlayerPrefs.GetInt("Level5") + "\n" + PlayerPrefs.GetInt("Level8");
        scores[8].text = PlayerPrefs.GetInt("Level3") + "\n" + PlayerPrefs.GetInt("Level6") + "\n" + PlayerPrefs.GetInt("Level9");
        scores[9].text = PlayerPrefs.GetInt("EndlessTime") + " seconds";
        scores[10].text = PlayerPrefs.GetInt("EndlessScore") + " points";

    }
}
