using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// The component for controlling the player.
public class Controller : MonoBehaviour
{

    // Beam attack prefab.
    [SerializeField] GameObject beam_prefab;

    // Sprites for different beam charge levels and the gun.
    [SerializeField] Sprite black;
    [SerializeField] Sprite green;
    [SerializeField] Sprite yellow;
    [SerializeField] Sprite orange;
    [SerializeField] Sprite red;
    [SerializeField] Sprite idle_gun;

    // Visual ease factors for  shooting and charging.
    [SerializeField] float shoot_ease_factor = 0.2f;
    [SerializeField] float charge_ease_factor = 0.025f;

    // Constant values to reach each charge level.
    [SerializeField] float charge_1 = 0.9f;
    [SerializeField] float charge_2 = 0.6f;
    [SerializeField] float charge_3 = 0.3f;

    // This game object's transform, Animator, and SpriteRenderer components.
    Transform tf;
    SpriteRenderer[] guides;
    Animator am;

    // Wrapper objects for the pause menu and game.
    GameObject pause_menu;
    GameObject game_objects;

    // Varaibles for storing joystick, bumper, and trigger input data.
    Vector3 prev_left_stick = new Vector3(0, 0, 0);
    Vector3 left_stick = new Vector3(0,0,0);
    Vector3 right_stick = new Vector3(0, 0, 0);
    Vector2 prev_triggers = new Vector2(0, 0);
    Vector2 cur_triggers = new Vector2(-1.0f, -1.0f);
    bool left_bumper = false;
    bool right_bumper = false;
    Vector4 buttons_down = new Vector4(0, 0, 0, 0);

    // Whether or not keyboard controls are being used.
    bool keyboard = true;

    // The current beam charge, between 0 and 1. A lower number means more charge.
    public float cur_tint = 1.0f;

    // The cannon sound.
    private FMOD.Studio.EventInstance cannon;

    // The controls instance.
    Controllers controls;


    void Start()
    {

        // Get the wrapper objects.
        pause_menu = GameObject.FindGameObjectWithTag("pause");
        game_objects = GameObject.FindGameObjectWithTag("GameController");

        // Get the keyboard mode from Player Prefs.
        keyboard = PlayerPrefs.GetInt("keyboard") == 1;

        // Make a new controller instance and enable it.
        controls = new Controllers();
        controls.Gameplay.Enable();

        // Map input controls to their respective variables. 
        controls.Gameplay.LeftStick.performed += ctx => left_stick = ctx.ReadValue<Vector2>();
        controls.Gameplay.LeftStick.canceled += ctx => left_stick = Vector3.zero;
        controls.Gameplay.RightStick.performed += ctx => right_stick = ctx.ReadValue<Vector2>();
        controls.Gameplay.RightStick.canceled += ctx => right_stick = Vector3.zero;
        controls.Gameplay.RightTrigger.performed += ctx => cur_triggers.y = 1.0f;
        controls.Gameplay.RightTrigger.canceled += ctx => cur_triggers.y = -1.0f;
        controls.Gameplay.LeftTrigger.performed += ctx => cur_triggers.x = 1.0f;
        controls.Gameplay.LeftTrigger.canceled += ctx => cur_triggers.x = -1.0f;
        controls.Gameplay.RightBumper.performed += ctx => right_bumper = true;
        controls.Gameplay.RightBumper.canceled += ctx => right_bumper = false;
        controls.Gameplay.LeftBumper.performed += ctx => left_bumper = true;
        controls.Gameplay.LeftBumper.canceled += ctx => left_bumper = false;

        // Get all the relevant components.
        am = GetComponentInChildren<Animator>();
        tf = GetComponent<Transform>();
        guides = GetComponentsInChildren<SpriteRenderer>();

        // Get the cannon sound effect and start it.
        cannon = FMODUnity.RuntimeManager.CreateInstance("event:/effects/cannon");
        cannon.start();
        cannon.setVolume(0.5f);

    }

    // Simple function converting an x and y coordinate into an angle between 0 and 2pi.
    float to2Pi(float x, float y)
    {
        float angle = Mathf.Atan(y / x);
        if (x < 0) angle += Mathf.PI;
        else if (y < 0) angle += Mathf.PI * 2;
        return angle;
    }

    void Update()
    {

        // BEAM CHARGE UPDATAE

        // If in keyboard mode:
        if (keyboard)
        {
            // Get the current scroll value.
            float scroll = controls.Gameplay.Scroll.ReadValue<float>();

            // If positive:
            if (scroll > 0.0f)
            {
                // Cap the tint change at 0.01, and scale below that. Ensure tint stays at or above 0.
                if (scroll > 200.0f) cur_tint -= 0.01f;
                else cur_tint -= scroll / 20000.0f;
                if (cur_tint < 0.0f) cur_tint = 0.0f;

                // Change the beam guide sprites if the charge has passed a threshold.
                if (cur_tint == 0.0f)
                {
                    guides[1].sprite = red;
                    guides[2].sprite = red;
                }
                else if (cur_tint < charge_3)
                {
                    guides[1].sprite = orange;
                    guides[2].sprite = orange;
                }
                else if (cur_tint < charge_2)
                {
                    guides[1].sprite = yellow;
                    guides[2].sprite = yellow;
                }
                else if (cur_tint < charge_1)
                {
                    guides[1].sprite = green;
                    guides[2].sprite = green;
                }
            }
        }

        // Otherwise:
        else
        {

            // If the current and previous inputs are both nonzero:
            if (!(left_stick.x == 0 && left_stick.y == 0) && !(prev_left_stick.x == 0 && prev_left_stick.y == 0))
            {

                // Convert both angles to radians.
                float prev_angle = to2Pi(prev_left_stick.x, prev_left_stick.y);
                float new_angle = to2Pi(left_stick.x, left_stick.y);

                // Get qa position difference between angles.
                if (prev_angle < Mathf.PI / 2 && new_angle > Mathf.PI * 3 / 2) prev_angle += Mathf.PI * 2;
                float angle_delta = prev_angle - new_angle;

                // If angle delta is within the correct range:
                if (angle_delta < Mathf.PI / 2 && angle_delta > 0)
                {

                    // Change the tint proportionally and ensure it stays at or above zero.
                    cur_tint -= angle_delta / 50.0f;
                    if (cur_tint < 0.0f) cur_tint = 0.0f;

                    // Change the beam guide sprites if the charge has passed a threshold.
                    if (cur_tint == 0.0f)
                    {
                        guides[1].sprite = red;
                        guides[2].sprite = red;
                    }
                    else if (cur_tint < charge_3)
                    {
                        guides[1].sprite = orange;
                        guides[2].sprite = orange;
                    }
                    else if (cur_tint < charge_2)
                    {
                        guides[1].sprite = yellow;
                        guides[2].sprite = yellow;
                    }
                    else if (cur_tint < charge_1)
                    {
                        guides[1].sprite = green;
                        guides[2].sprite = green;
                    }
                }
            }
        }

        // BEAM DIRECTION UPDATE

        // Get rid of negative inputs and normalize.
        if (right_stick.x < 0) right_stick.x = 0;
        if (right_stick.y < 0) right_stick.y = 0;
        right_stick.Normalize();

        // If the input is nonzero:
        if (right_stick.x != 0.0f || right_stick.y != 0.0f) {

            // Get the desired angle and current angle.
            float desired_angle = Mathf.Atan(right_stick.y / right_stick.x);
            float current_angle = Mathf.Atan(tf.position.y / tf.position.x);
            float final_angle = current_angle;

            // If the difference between the two is greater than a significant threshold, alter the angle by that amount.
            float factor = 0.005f + cur_tint / 100.0f;
            if (Mathf.Abs(current_angle - desired_angle) > factor)
            {
                if (current_angle < desired_angle) final_angle = current_angle + factor;
                else final_angle = current_angle - factor;
            }

            // Limit the angle to between 0 and pi/2.
            if (final_angle > Mathf.PI / 2) final_angle = Mathf.PI / 2;
            else if (final_angle < 0.0f) final_angle = 0.0f;

            // Update the position and rotation of the beam accordingly.
            tf.SetPositionAndRotation(new Vector3(Mathf.Cos(final_angle), Mathf.Sin(final_angle), 0), Quaternion.Euler(0.0f, 0.0f, final_angle * 180.0f / Mathf.PI));
        }

        // BEAM SHOOT UPDATE

        // Keep track of how many buttons (triggers + bumpers) were pressed.
        // Each of the four buttons get a 10 frame window to be pressed together.
        // When that countdown ends, you have to repress the button.
        // This allows for some leniency but will not let you just hold them all down.
        int buttons_pressed = 0;

        if (buttons_down.x == 0) {
            if (cur_triggers.x > prev_triggers.x)
            {
                buttons_pressed++;
                buttons_down.x = 10;
            }
        }
        else {
            if (cur_triggers.x != 1) buttons_down.x = 0;
            else
            {
                buttons_pressed++;
                buttons_down.x--;
            }
        } // LEFT TRIGGER

        if (buttons_down.y == 0)
        {
            if (left_bumper)
            {
                buttons_pressed++;
                buttons_down.y = 10;
            }
        }
        else
        {
            if (left_bumper)
            {
                buttons_pressed++;
                buttons_down.y--;
            }
            else buttons_down.y = 0;
        } // LEFT BUMPER

        if (buttons_down.z == 0)
        {
            if (right_bumper)
            {
                buttons_pressed++;
                buttons_down.z = 10;
            }
        }
        else
        {
            if (right_bumper)
            {
                buttons_pressed++;
                buttons_down.z--;
            }
            else buttons_down.z = 0;
        } // RIGHT BUMPER

        if (buttons_down.w == 0)
        {
            if (cur_triggers.y > prev_triggers.y)
            {
                buttons_pressed++;
                buttons_down.w = 10;
            }
        }
        else
        {
            if (cur_triggers.y != 1) buttons_down.w = 0;
            else
            {
                buttons_pressed++;
                buttons_down.w--;
            }
        } // RIGHT TRIGGER

        // Set the number of necessary buttons based on the charge threshold.
        int charge_count = 0;
        if (cur_tint == 0.0f) charge_count = 4;
        else if (cur_tint <= charge_3) charge_count = 3;
        else if (cur_tint <= charge_2) charge_count = 2;
        else if (cur_tint <= charge_1) charge_count = 1;

        // If the beam can be fired this frame:
        if (charge_count != 0 && buttons_pressed >= charge_count) {

            // Create a new beam and set its position and rotation.
            GameObject cur_beam = Instantiate(beam_prefab, transform.parent.parent);
            cur_beam.transform.SetPositionAndRotation(transform.GetChild(transform.childCount-1).position, transform.rotation);

            // Get the beam manager component. Set it's goal thickness, grow time, and damage based on the charge level.
            BeamManager bm = cur_beam.GetComponent<BeamManager>();
            bm.goal_thickness = guides[1].transform.localPosition.y * 2.0f;
            bm.grow_time = (charge_1 - cur_tint) / charge_1 * 0.2f + 0.05f;
            if (cur_tint == 0.0f) bm.damage = 7;
            else if (cur_tint < charge_3) bm.damage = 4;
            else if (cur_tint < charge_2) bm.damage = 2;
            else bm.damage = 1;

            // Generate a sound effect based on the charge threshold.
            // There are three different possible sound for each level, chosen randomly.
            int main_idx = 0;
            int rand_idx = Mathf.RoundToInt(Random.Range(-0.499f, 2.499f));
            if (cur_tint == 0.0f) main_idx = 3;
            else if (cur_tint < charge_3) main_idx = 2;
            else if (cur_tint < charge_2) main_idx = 1;
            EventBus.Publish(new GunEvent(main_idx, rand_idx));

            // Reset the tint and guide sprites.
            cur_tint = 1.0f;
            guides[1].sprite = black;
            guides[2].sprite = black;
        }
    }

    void LateUpdate()
    {

        // Update the previous states.
        prev_left_stick = left_stick;
        prev_triggers = cur_triggers;

        // Update the guide positions based on the current charge and ease factor.
        float new_size = 0.05f;
        if (cur_tint < charge_1) new_size = (charge_1 - cur_tint) / charge_1 * 0.95f + 0.05f;
        float real_size = guides[1].transform.localPosition.y + (new_size - guides[1].transform.localPosition.y) * shoot_ease_factor;
        guides[1].transform.localPosition = new Vector3(1.25f, real_size, -0.02f);
        guides[2].transform.localPosition = new Vector3(1.25f, -real_size, -0.02f);
        guides[3].transform.localPosition = new Vector3(1.281f, real_size, -0.02f);
        guides[4].transform.localPosition = new Vector3(1.281f, -real_size, -0.02f);

        // Update the charge visuals based on the current charge and ease factor.
        float ease_tint;
        if ((1.0f - cur_tint) * 3.125f >= guides[3].transform.localScale.x) ease_tint = (1.0f - cur_tint) * 3.125f;
        else ease_tint = guides[3].transform.localScale.x + ((1.0f - cur_tint) * 3.125f - guides[3].transform.localScale.x) * charge_ease_factor;
        guides[3].transform.localScale = new Vector3(ease_tint, 3.125f, 1.0f);
        guides[4].transform.localScale = new Vector3(ease_tint, 3.125f, 1.0f);

        // Set the power parameter based on the current charge.
        cannon.setParameterByName("power", 1.0f - cur_tint);

        // Change the gun animation speed based on the current charge.
        if (cur_tint == 0.0f)
        {
            am.enabled = true;
            am.speed = 2.4f;
        }
        else if (cur_tint <= charge_3)
        {
            am.enabled = true;
            am.speed = 1.85f;
        }
        else if (cur_tint <= charge_2)
        {
            am.enabled = true;
            am.speed = 1.3f;
        }
        else if (cur_tint <= charge_1)
        {
            am.enabled = true;
            am.speed = 0.75f;
        }
        else
        {
            am.enabled = false;
            guides[0].sprite = idle_gun;
            am.speed = 0.0f;
        }

        // If pause was pressed this frame, play a button sound, turn on the pause menu, and turn off the game.
        if (controls.Gameplay.Pause.WasPressedThisFrame())
        {
            EventBus.Publish(new ButtonEvent(1));
            pause_menu.SetActive(true);
            game_objects.SetActive(false);
        }

    }

    // When destroyed, stop the cannon sound immediately and release it.
    private void OnDestroy()
    {
        cannon.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        cannon.release();
    }
}
