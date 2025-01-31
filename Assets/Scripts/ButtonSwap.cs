using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwap : MonoBehaviour
{
    [SerializeField] GameObject menu_screen;
    [SerializeField] GameObject other_screen;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<ButtonPress>(_OnClick);
    }

    // Update is called once per frame
    void _OnClick(ButtonPress e)
    {
        if (e.selected_button == gameObject)
        {
            other_screen.SetActive(true);
            menu_screen.SetActive(false);
        }
    }
}
