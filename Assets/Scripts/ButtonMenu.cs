using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The button for returning to the menu.
public class ButtonMenu : MonoBehaviour
{
    void Start()
    {
        // Subscribe to button clicks.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, load the main menu.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject) UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
