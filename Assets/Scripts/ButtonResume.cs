using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The button for resuming the game.
public class ButtonResume : MonoBehaviour
{

    // Wrapper objects for game objects and the pause menu.
    GameObject pause_menu;
    GameObject game_objects;

    void Start()
    {
        // Get the wrapper objects.
        game_objects = GameObject.FindGameObjectWithTag("GameController");
        pause_menu = GameObject.FindGameObjectWithTag("pause");

        // Subscribe to button clicks.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, enable the game and disable the pause menu.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            game_objects.SetActive(true);
            pause_menu.SetActive(false);
        }
    }
}
