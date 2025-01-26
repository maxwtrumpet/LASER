using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlay : MonoBehaviour
{
    GameObject menu_buttons;
    GameObject level_buttons;

    // Start is called before the first frame update
    void Start()
    {
        menu_buttons = GameObject.FindGameObjectWithTag("menu");
        level_buttons = GameObject.FindGameObjectWithTag("level");
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // Update is called once per frame
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            level_buttons.SetActive(true);
            menu_buttons.SetActive(false);
        }
    }
}
