using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Manages the intro cutscene.
public class CutsceneManager : MonoBehaviour
{

    // The music manager prefab.
    [SerializeField] GameObject mm_prefab;

    // The text component.
    TextMeshPro tmp;

    // All the messages to be displayed.
    string[] messages = { "The year is 30XX.\nHumans have begun using fusion as their main power source.",
        "Suddenly, leagues of alien robots swarm  Earth. Space pirates!",
        "Having depleted their own fossil fuels, they scour the cosmos looking for planets to raid; Earth is next.",
        "To stop them, the world powers have united to build a ray gun that can destroy the robots.",
        "They put their trust in you to man this machine, and save humanity.",
        "This, soldier, is your Last Assignment: Save Earth from the Robots..." };


    void Start()
    {
        // If played before, skip the cutscene.
        if (PlayerPrefs.GetInt("played") == 1) UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");

        // Otherwise:
        else
        {
            // Get the text.
            tmp = FindAnyObjectByType<TextMeshPro>();

            // Instantiate the music manager if it doesn't already exist and turn off the drum layer.
            if (GameObject.FindGameObjectWithTag("music") == null) Instantiate(mm_prefab);
            EventBus.Publish(new MusicEvent("Drum 1", 0.0f));

            // Start the cutscene coroutine.
            StartCoroutine(PlayCutscene());
        }
    }

    // Cutscene coroutine.
    IEnumerator PlayCutscene()
    {

        // For every message:
        for (int i = 0; i < messages.Length; i++)
        {
            // Update the text.
            tmp.text = messages[i];

            // Fade in, wait for a time proportional to the message length, fade out, and briefly pause.
            yield return FadeIn();
            yield return new WaitForSeconds(messages[i].Length / 50.0f);
            yield return FadeOut();
            yield return new WaitForSeconds(0.5f);

        }

        // Load the main menu.
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Fade in Coroutine.
    IEnumerator FadeIn()
    {
        // While the text alpha is not full, increase every frame.
        while (tmp.color.a < 1.0f)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a + 0.01f);
            yield return new WaitForSeconds(0.0166f);
        }
    }

    // Fade out Coroutine.
    IEnumerator FadeOut()
    {
        // While the text alpha is not full, increase every frame.
        while (tmp.color.a > 0.0f)
        {
            tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, tmp.color.a - 0.01f);
            yield return new WaitForSeconds(0.0166f);
        }
    }

}
