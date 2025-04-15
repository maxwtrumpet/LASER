using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Buttons used to swap between menus.
public class ButtonSwap : MonoBehaviour
{
    // The wrapper objects for the menu and other relevant screen.
    [SerializeField] GameObject menu_screen;
    [SerializeField] GameObject other_screen;

    void Start()
    {
        // Subscribe to button clicks.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, enable the other screen and disable the menu.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            other_screen.SetActive(true);
            menu_screen.SetActive(false);
        }
    }
}
