using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonResume : MonoBehaviour
{
    GameObject pause_menu;
    GameObject game_objects;

    // Start is called before the first frame update
    void Start()
    {
        game_objects = GameObject.FindGameObjectWithTag("GameController");
        pause_menu = GameObject.FindGameObjectWithTag("pause");
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            game_objects.SetActive(true);
            pause_menu.SetActive(false);
        }
    }
}
