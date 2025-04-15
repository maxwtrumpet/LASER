using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The button that starts endless mode.
public class ButtonEndless : MonoBehaviour
{
    void Start()
    {
        // Subscribe to button clicks.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, load endless.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject) UnityEngine.SceneManagement.SceneManager.LoadScene("Level10");
    }
}
