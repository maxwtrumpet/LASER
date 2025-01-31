using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{

    [SerializeField] GameObject mm_prefab;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("played") == 0) UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        else
        {
            if (GameObject.FindGameObjectWithTag("music") == null) Instantiate(mm_prefab);
            EventBus.Publish(new MusicEvent("Drum 1", 0.0f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
