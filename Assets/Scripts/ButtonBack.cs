using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBack : MonoBehaviour
{
    GameObject menu_buttons;
    GameObject control_buttons;

    // Start is called before the first frame update
    void Start()
    {
        menu_buttons = GameObject.FindGameObjectWithTag("menu");
        control_buttons = GameObject.FindGameObjectWithTag("controls");
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // Update is called once per frame
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            menu_buttons.SetActive(true);
            control_buttons.SetActive(false);
        }
    }
}