using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// The buttons for loading levels
public class ButtonLevel : MonoBehaviour
{
    // The scene name and level #.
    string scene = "Level";
    int level = 0;

    void Start()
    {
        // Get the level number from the button text and append it to the scene name.
        level = int.Parse(GetComponentInChildren<TextMeshPro>().text);
        scene += level;

        
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject) UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
