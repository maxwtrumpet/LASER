using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The button for going to the next level.
public class ButtonNext : MonoBehaviour
{
    void Start()
    {
        // Subscribe to button clicks.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, load the next level.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject) UnityEngine.SceneManagement.SceneManager.LoadScene("Level" + (int.Parse(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name[5..]) + 1));
    }
}
