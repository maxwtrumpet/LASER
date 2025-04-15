using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The button for quitting the game.
public class ButtonQuit : MonoBehaviour
{
    void Start()
    {
        // Subscribe to button clicks.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, quit the game.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
