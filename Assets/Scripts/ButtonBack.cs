using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The button to go back to the main menu.
public class ButtonBack : MonoBehaviour
{

    // Container objects for the menu and current buttons.
    GameObject menu_buttons;
    GameObject current_buttons;

    void Start()
    {
        // Find the menu buttons. Current buttons are within the parent container.
        menu_buttons = GameObject.FindGameObjectWithTag("menu");
        current_buttons = transform.parent.gameObject;

        // Subscribe to click events.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, enable the menu buttons and disable the current.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            menu_buttons.SetActive(true);
            current_buttons.SetActive(false);
        }
    }
}