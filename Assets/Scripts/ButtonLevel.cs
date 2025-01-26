using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ButtonLevel : MonoBehaviour
{
    string scene = "Level";

    // Start is called before the first frame update
    void Start()
    {
        scene += GetComponentInChildren<TextMeshPro>().text;
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // Update is called once per frame
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject) UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }
}
