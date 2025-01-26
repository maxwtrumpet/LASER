using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCancel : MonoBehaviour
{

    GameObject menu_buttons;
    GameObject level_buttons;

    // Start is called before the first frame update
    void Start()
    {
        menu_buttons = GameObject.FindGameObjectWithTag("menu");
        level_buttons = GameObject.FindGameObjectWithTag("level");
        EventBus.Subscribe<ButtonPress>(_OnClick);
        level_buttons.SetActive(false);
    }

    // Update is called once per frame
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            menu_buttons.SetActive(true);
            level_buttons.SetActive(false);
        }
    }
}
