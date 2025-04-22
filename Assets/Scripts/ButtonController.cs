using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.InputSystem;

// The button press event, which stores which button was clicked.
public class ButtonPress {
    public GameObject selected_button;
    public ButtonPress(GameObject _b_in) { selected_button = _b_in; }
}

// A nested array, forced to be serializeable.
[System.Serializable]
public class NestedArray<T>
{
    public T[] row;
}

// The component for controlling buttons on a specific menu.
public class ButtonController : MonoBehaviour
{
    // The nested array of buttons on this menu.
    [SerializeField] NestedArray<GameObject>[] buttons;

    // The current row and column.
    [SerializeField] int row = 0;
    [SerializeField] int column = 0;

    // The current joystick input.
    Vector2 JoystickInput = Vector2.zero;

    // Whether or not the button changed, and what input mode is being used.
    bool cur_input = false;

    // The Unity controller object.
    Controllers controls;

    // Music events for moving and selecting buttons.
    private ButtonEvent changed = new ButtonEvent(0);
    private ButtonEvent select = new ButtonEvent(1);

    void Start()
    {
        // Create a controller object and enable it.
        controls = new Controllers();
        controls.Gameplay.Enable();

        // Make funtions for the left stick being moved/canceled and for select being pressed.
        controls.Gameplay.LeftStick.performed += ctx => JoystickInput = ctx.ReadValue<Vector2>();
        controls.Gameplay.LeftStick.canceled += ctx => JoystickInput = Vector2.zero;
        controls.Gameplay.Select.performed += ctx => PlayerPrefs.SetInt("keyboard",ctx.control.path == "/Keyboard/space" ? 1 : 0);

        // Turn the default button's selected sprite on.
        buttons[row].row[column].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        // If joystick is not pressed sufficiently, no input is registered.
        if (JoystickInput.magnitude < 0.5f)
        {
            cur_input = false;
        }

        // Otherwise, if this is the first frame an input is registered:
        else if (cur_input == false)
        {

            // Mark there being an input and disable the selected sprite of the current button.
            cur_input = true;
            buttons[row].row[column].GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;

            // If there was more joystick movement in the x than in the y:
            if (Mathf.Abs(JoystickInput.x) > Mathf.Abs(JoystickInput.y))
            {
                // Get the sign of the change, and if this change doesn't go out of bounds, play the changed sound and update the column. 
                int change = (int)Mathf.Sign(JoystickInput.x);
                if (column + change >= 0 && column + change < buttons[row].row.Length)
                {
                    EventBus.Publish(changed);
                    column += change;
                }
            }

            // Otherwise:
            else
            {

                // Get the sign of the change, and if this change doesn't go out of bounds:
                int change = -(int)Mathf.Sign(JoystickInput.y);
                if (row + change >= 0 && row + change < buttons.Length)
                {

                    // Play the changed sound and update the column. 
                    EventBus.Publish(changed);
                    row += change;

                    // If the current column is out of bounds, clip it to the last column in this row.
                    if (column >= buttons[row].row.Length) column = buttons[row].row.Length - 1;

                }
            }

            // Enable the selected sprite of the new button.
            buttons[row].row[column].GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
        }

        // If select button was pressed this frame, play the select sound and publish a button click event.
        if (controls.Gameplay.Select.WasPressedThisFrame())
        {
            EventBus.Publish(select);
            EventBus.Publish(new ButtonPress(buttons[row].row[column]));
        }
    }

}