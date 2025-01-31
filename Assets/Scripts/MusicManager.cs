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

public class ExplosionEvent
{
    public int index;
    public ExplosionEvent(int _new_index) { index = _new_index; }
}

public class ButtonEvent
{
    public int index;
    public ButtonEvent(int _new_index) { index = _new_index; }
}

public class GunEvent
{
    public int type = 0;
    public int index = 0;
    public GunEvent(int type_in, int index_in) { type = type_in; index = index_in; }
}

public class MusicManager : MonoBehaviour
{
    private FMOD.Studio.EventInstance music;
    private FMOD.Studio.EventInstance[] explosions = new FMOD.Studio.EventInstance[5];
    private FMOD.Studio.EventInstance[] buttons = new FMOD.Studio.EventInstance[2];
    private FMOD.Studio.EventInstance[,] guns = new FMOD.Studio.EventInstance[4,3];
    private float[][] parameters = new float[10][]
    {
        new float[11] {1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f, 0.0f},
        new float[11] {1.0f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 0.0f},
        new float[11] {1.0f, 0.0f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f},
        new float[11] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f}
    };

    public void Reset()
    {
        music.setParameterByName("Ab Resolve", 0.0f);
        music.setParameterByName("Ab Stay", 0.0f);
        music.setParameterByName("Bass High", 1.0f);
        music.setParameterByName("Bass Low", 0.0f);
        music.setParameterByName("Bb High", 0.0f);
        music.setParameterByName("Bb Low", 0.0f);
        music.setParameterByName("Drum 1", 1.0f);
        music.setParameterByName("Drum 2", 0.0f);
        music.setParameterByName("Drum 3", 0.0f);
        music.setParameterByName("Eb", 0.0f);
        music.setParameterByName("F", 0.0f);
        music.setParameterByName("Melody High", 0.0f);
        music.setParameterByName("Melody Low", 0.0f);
        music.setParameterByName("Ostinato Fast", 0.0f);
        music.setParameterByName("Ostinato Slow", 0.0f);
    }

    void _OnButton(ButtonEvent e)
    {
        buttons[e.index].start();
    }

    void _OnExplosion(ExplosionEvent e)
    {
        explosions[e.index].start();
    }

    void _OnMusic(MusicEvent e)
    {
        music.setParameterByName(e.parameter, e.value);
    }

    void _OnGun(GunEvent e)
    {
        guns[e.type, e.index].start();
    }

    public void StartLevel(int level)
    {
        music.setParameterByName("Bass High", parameters[level][0]);
        music.setParameterByName("Melody Low", parameters[level][1]);
        music.setParameterByName("Bb Low", parameters[level][2]);
        music.setParameterByName("Ab Stay", parameters[level][3]);
        music.setParameterByName("Drum 1", parameters[level][4]);
        music.setParameterByName("Eb", parameters[level][5]);
        music.setParameterByName("Bb High", parameters[level][6]);
        music.setParameterByName("F", parameters[level][7]);
        music.setParameterByName("Ab Resolve", parameters[level][8]);
        music.setParameterByName("Drum 2", parameters[level][9]);
        music.setParameterByName("Melody High", parameters[level][10]);
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
        music = FMODUnity.RuntimeManager.CreateInstance("event:/music/Theme");
        explosions[0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/explosion_0");
        explosions[0].setVolume(0.25f);
        explosions[1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/explosion_1");
        explosions[1].setVolume(0.25f);
        explosions[2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/explosion_2");
        explosions[2].setVolume(0.25f);
        explosions[3] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/explosion_3");
        explosions[3].setVolume(0.25f);
        explosions[4] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/explosion_4");
        explosions[4].setVolume(0.25f);
        buttons[0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/button_change");
        buttons[0].setVolume(0.25f);
        buttons[1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/button_select");
        buttons[1].setVolume(0.25f);
        guns[0, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun0-0");
        guns[0, 0].setVolume(0.25f);
        guns[0, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun0-1");
        guns[0, 1].setVolume(0.25f);
        guns[0, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun0-2");
        guns[0, 2].setVolume(0.25f);
        guns[1, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun1-0");
        guns[1, 0].setVolume(0.25f);
        guns[1, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun1-1");
        guns[1, 1].setVolume(0.25f);
        guns[1, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun1-2");
        guns[1, 2].setVolume(0.25f);
        guns[2, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun2-0");
        guns[2, 0].setVolume(0.25f);
        guns[2, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun2-1");
        guns[2, 1].setVolume(0.25f);
        guns[2, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun2-2");
        guns[2, 2].setVolume(0.25f);
        guns[3, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun3-0");
        guns[3, 0].setVolume(0.1f);
        guns[3, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun3-1");
        guns[3, 1].setVolume(0.1f);
        guns[3, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun3-2");
        guns[3, 2].setVolume(0.1f);
        music.start();
        Reset();
        EventBus.Subscribe<MusicEvent>(_OnMusic);
        EventBus.Subscribe<ExplosionEvent>(_OnExplosion);
        EventBus.Subscribe<ButtonEvent>(_OnButton);
        EventBus.Subscribe<GunEvent>(_OnGun);
    }
}
