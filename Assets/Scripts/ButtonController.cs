using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPress {
    public GameObject selected_button;
    public ButtonPress(GameObject _b_in) { selected_button = _b_in; }
}

[System.Serializable]
public class NestedArray<T>
{
    public T[] row;
}

public class ButtonController : MonoBehaviour
{
    [SerializeField] NestedArray<GameObject>[] buttons;
    [SerializeField] int row = 0;
    [SerializeField] int column = 0;
    Vector2 JoystickInput = Vector2.zero;
    bool cur_input = false;
    Controllers controls;
    private ButtonEvent changed = new ButtonEvent(0);
    private ButtonEvent select = new ButtonEvent(1);

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controllers();
        controls.Gameplay.Enable();
        buttons[row].row[column].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
        controls.Gameplay.LeftStick.performed += ctx => JoystickInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.LeftStick.canceled += ctx => JoystickInput = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (JoystickInput.magnitude < 0.5f)
        {
            cur_input = false;
        }
        else if (cur_input == false)
        {
            buttons[row].row[column].GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
            cur_input = true;
            if (Mathf.Abs(JoystickInput.x) > Mathf.Abs(JoystickInput.y))
            {
                int change = (int)Mathf.Sign(JoystickInput.x);
                if (column + change >= 0 && column + change < buttons[row].row.Length)
                {
                    EventBus.Publish(changed);
                    column += change;
                }
            }
            else
            {
                int change = -(int)Mathf.Sign(JoystickInput.y);
                if (row + change >= 0 && row + change < buttons.Length)
                {
                    EventBus.Publish(changed);
                    row += change;
                    if (column >= buttons[row].row.Length) column = buttons[row].row.Length - 1;
                }
            }
            buttons[row].row[column].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
        }
        if (controls.Gameplay.Select.WasPressedThisFrame())
        {
            EventBus.Publish(select);
            EventBus.Publish<ButtonPress>(new ButtonPress(buttons[row].row[column]));
        }
    }
}
