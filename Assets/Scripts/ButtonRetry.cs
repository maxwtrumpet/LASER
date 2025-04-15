using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// The button for retrying the level.
public class ButtonRetry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        // Subscribe to button clicks.
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // When receiving a click event for this button, reload the scene.
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }
    }
}
