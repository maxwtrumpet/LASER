using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class MusicEvent
{
    public string parameter = "Bass High";
    public float value = 1.0f;
    public MusicEvent(string _new_parameter, float _new_value) {
        parameter = _new_parameter;
        value = _new_value;
    }
}

public class MusicManager : MonoBehaviour
{
    [SerializeField] GameObject em;
    private FMOD.Studio.EventInstance instance;
    private int kill_points = 0;
    TextMeshPro tmp;

    void _OnKill(KillEvent e)
    {
        int prev_points = kill_points;
        kill_points += e.points;
        tmp.text = "Score: " + kill_points + " ";
        if (prev_points < 40 && kill_points >= 40)
        {
            instance.setParameterByName("Melody High", 1.0f);
            instance.setParameterByName("Melody Low", 0.0f);
        }
        else if (prev_points < 35 && kill_points >= 35)
        {
            instance.setParameterByName("Drum 2", 1.0f);
        }
        else if (prev_points < 30 && kill_points >= 30)
        {
            instance.setParameterByName("F", 1.0f);
            instance.setParameterByName("Ab Resolve", 0.0f);
            instance.setParameterByName("Ab Stay", 0.0f);
        }
        else if (prev_points < 25 && kill_points >= 25)
        {
            instance.setParameterByName("Bb High", 1.0f);
            instance.setParameterByName("Bb Low", 0.0f);
        }
        else if (prev_points < 20 && kill_points >= 20)
        {
            instance.setParameterByName("Eb", 1.0f);
        }
        else if (prev_points < 15 && kill_points >= 15)
        {
            instance.setParameterByName("Drum 1", 1.0f);
        }
        else if (prev_points < 10 && kill_points >= 10)
        {
            instance.setParameterByName("Ab Stay", 1.0f);
        }
        else if (prev_points < 5 && kill_points >= 5)
        {
            instance.setParameterByName("Bb Low", 1.0f);
        }
        else if(prev_points < 1 && kill_points >= 1)
        {
            instance.setParameterByName("Melody Low", 1.0f);
        }
    }

    void _OnMusic(MusicEvent e)
    {
        instance.setParameterByName(e.parameter, e.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/music/Theme");
        instance.start();
        instance.setParameterByName("Bass High", 1.0f);
        tmp = em.GetComponent<TextMeshPro>();
        tmp.text = "Score: 0 ";
        EventBus.Subscribe<KillEvent>(_OnKill);
        EventBus.Subscribe<MusicEvent>(_OnMusic);
    }
}
