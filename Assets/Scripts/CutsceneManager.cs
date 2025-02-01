using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneManager : MonoBehaviour
{

    [SerializeField] GameObject mm_prefab;
    TextMeshPro tmp;
    string[] messages = { "The year is 30XX.\nHumans have begun using fusion as their main power source.", "Suddenly, leagues of aliens robots swarm  Earth. Space pirates!", "Having depleted their own fossil fuels, they scour the cosmos looking for planets to raid; Earth is next.", "To stop them, the world powers have united to build a ray gun that can destroy the robots.", "They put your trust in you to man this machine, and save humanity.", "This, soldier, is your Last Assignment: Save Earth from the Robots..." };


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("played") == 1) UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        else
        {
            tmp = FindAnyObjectByType<TextMeshPro>();
            if (GameObject.FindGameObjectWithTag("music") == null) Instantiate(mm_prefab);
            EventBus.Publish(new MusicEvent("Drum 1", 0.0f));
            StartCoroutine(PlayCutscene());
        }
    }

    IEnumerator PlayCutscene()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            tmp.text = messages[i];
            yield return FadeIn();
            yield return new WaitForSeconds(messages[i].Length / 50.0f);
            yield return FadeOut();
            yield return new WaitForSeconds(0.5f);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeIn()
    {
        while (tmp.color.a < 1.0f)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + 0.01f);
            yield return new WaitForSeconds(0.0166f);
        }
    }

    IEnumerator FadeOut()
    {
        while (tmp.color.a > 0.0f)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - 0.01f);
            yield return new WaitForSeconds(0.0166f);
        }
    }

}
