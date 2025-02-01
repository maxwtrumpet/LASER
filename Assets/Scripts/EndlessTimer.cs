using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndlessTimer : MonoBehaviour
{
    float time_elapsed = 0.0f;
    TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        EventBus.Subscribe<MusicEvent>(_OnMusic);
    }

    void _OnMusic(MusicEvent e)
    {
        if (e.parameter == "Bass Low" && e.value == 0.0f && time_elapsed >= 360) EventBus.Publish(new MusicEvent("Bass Low", 1.0f));
        else if (e.parameter == "Ostinato Slow" && e.value == 0.0f && time_elapsed >= 420) EventBus.Publish(new MusicEvent("Ostinato Slow", 1.0f));
        else if (e.parameter == "Ostinato Fast" && e.value == 0.0f && time_elapsed >= 600) EventBus.Publish(new MusicEvent("Ostinato Fast", 1.0f));
    }

    // Update is called once per frame
    void Update()
    {
        float prev_time = time_elapsed;
        time_elapsed += Time.deltaTime;
        tmp.text = "Time: " + (int)time_elapsed;

        if (prev_time < 600 && time_elapsed >= 600)
        {
            EventBus.Publish(new MusicEvent("Ostinato Fast", 1.0f));
        }
        else if (prev_time < 510 && time_elapsed >= 510)
        {
            EventBus.Publish(new MusicEvent("Melody Low", 1.0f));
        }
        else if (prev_time < 420 && time_elapsed >= 420)
        {
            EventBus.Publish(new MusicEvent("Ostinato Slow", 1.0f));
        }
        else if (prev_time < 360 && time_elapsed >= 360)
        {
            EventBus.Publish(new MusicEvent("Bass Low", 1.0f));
        }
        else if (prev_time < 300 && time_elapsed >= 300)
        {
            EventBus.Publish(new MusicEvent("Melody High", 1.0f));
            EventBus.Publish(new MusicEvent("Melody Low", 0.0f));
        }
        else if (prev_time < 240 && time_elapsed >= 240)
        {
            EventBus.Publish(new MusicEvent("Drum 2", 1.0f));
        }
        else if (prev_time < 210 && time_elapsed >= 210)
        {
            EventBus.Publish(new MusicEvent("F", 1.0f));
            EventBus.Publish(new MusicEvent("Ab Resolve", 0.0f));
            EventBus.Publish(new MusicEvent("Ab Stay", 0.0f));
        }
        else if (prev_time < 180 && time_elapsed >= 180)
        {
            EventBus.Publish(new MusicEvent("Bb High", 1.0f));
            EventBus.Publish(new MusicEvent("Bb Low", 0.0f));
        }
        else if (prev_time < 150 && time_elapsed >= 150)
        {
            EventBus.Publish(new MusicEvent("Eb", 1.0f));
        }
        else if (prev_time < 120 && time_elapsed >= 120)
        {
            EventBus.Publish(new MusicEvent("Drum 1", 1.0f));
        }
        else if (prev_time < 90 && time_elapsed >= 90)
        {
            EventBus.Publish(new MusicEvent("Ab Stay", 1.0f));
        }
        else if (prev_time < 60 && time_elapsed >= 60)
        {
            EventBus.Publish(new MusicEvent("Bb Low", 1.0f));
        }
        else if (prev_time < 30 && time_elapsed >= 30)
        {
            EventBus.Publish(new MusicEvent("Melody Low", 1.0f));
        }

    }

    public void CheckTime()
    {
        if (PlayerPrefs.GetInt("EndlessTime") < (int)time_elapsed) PlayerPrefs.SetInt("EndlessTime", (int)time_elapsed);
    }
}
