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
    private FMOD.Studio.EventInstance instance;
    private float[][] parameters = new float[9][]
    {
        new float[11] {1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f},
        new float[11] {1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f}
    };

    public void Reset()
    {
        instance.setParameterByName("Ab Resolve", 0.0f);
        instance.setParameterByName("Ab Stay", 0.0f);
        instance.setParameterByName("Bass High", 1.0f);
        instance.setParameterByName("Bass Low", 0.0f);
        instance.setParameterByName("Bb High", 0.0f);
        instance.setParameterByName("Bb Low", 0.0f);
        instance.setParameterByName("Drum 1", 1.0f);
        instance.setParameterByName("Drum 2", 0.0f);
        instance.setParameterByName("Drum 3", 0.0f);
        instance.setParameterByName("Eb", 0.0f);
        instance.setParameterByName("F", 0.0f);
        instance.setParameterByName("Melody High", 0.0f);
        instance.setParameterByName("Melody Low", 0.0f);
        instance.setParameterByName("Ostinato Fast", 0.0f);
        instance.setParameterByName("Ostinato Slow", 0.0f);
    }

    void _OnMusic(MusicEvent e)
    {
        instance.setParameterByName(e.parameter, e.value);
    }

    public void StartLevel(int level)
    {
        instance.setParameterByName("Bass High", parameters[level][0]);
        instance.setParameterByName("Melody Low", parameters[level][1]);
        instance.setParameterByName("Bb Low", parameters[level][2]);
        instance.setParameterByName("Ab Stay", parameters[level][3]);
        instance.setParameterByName("Drum 1", parameters[level][4]);
        instance.setParameterByName("Eb", parameters[level][5]);
        instance.setParameterByName("Bb High", parameters[level][6]);
        instance.setParameterByName("F", parameters[level][7]);
        instance.setParameterByName("Ab Resolve", parameters[level][8]);
        instance.setParameterByName("Drum 2", parameters[level][9]);
        instance.setParameterByName("Melody High", parameters[level][10]);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/music/Theme");
        instance.start();
        Reset();
        EventBus.Subscribe<MusicEvent>(_OnMusic);
    }
}
