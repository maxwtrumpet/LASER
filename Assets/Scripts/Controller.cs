using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject beam_prefab;
    [SerializeField] Sprite black;
    [SerializeField] Sprite green;
    [SerializeField] Sprite yellow;
    [SerializeField] Sprite orange;
    [SerializeField] Sprite red;
    [SerializeField] float aim_ease_factor = 0.01f;
    [SerializeField] float shoot_ease_factor = 0.2f;
    [SerializeField] float charge_ease_factor = 0.025f;
    [SerializeField] float charge_1 = 0.9f;
    [SerializeField] float charge_2 = 0.6f;
    [SerializeField] float charge_3 = 0.3f;
    Transform tf;
    GameObject pause_menu;
    GameObject game_objects;
    SpriteRenderer[] guides;
    Vector3 prev_left_stick = new Vector3(0, 0, 0);
    Vector3 left_stick = new Vector3(0,0,0);
    Vector3 right_stick = new Vector3(0, 0, 0);
    Vector2 prev_triggers = new Vector2(0, 0);
    Vector2 cur_triggers = new Vector2(-1.0f, -1.0f);
    Vector4 buttons_down = new Vector4(0, 0, 0, 0);
    bool left_bumper = false;
    bool right_bumper = false;
    public float cur_tint = 1.0f;
    private FMOD.Studio.EventInstance instance;
    FMOD.Studio.EventInstance[,] gun_sounds = new FMOD.Studio.EventInstance[4,3];
    Controllers controls;

    // Buttons:
    // 1 - Down (A)
    // 2 - Right (B)
    // 4 - Left (X)
    // 5 - Up (Y)
    // 7 - Left Bumper
    // 8 - Right Bumper
    // 12 - Menu Button
    // 14 - Left Joystick
    // 15 - Right Joystick
    // 16 - Select Button


    // Start is called before the first frame update
    void Start()
    {

        pause_menu = GameObject.FindGameObjectWithTag("pause");
        game_objects = GameObject.FindGameObjectWithTag("GameController");

        controls = new Controllers();
        controls.Gameplay.Enable();
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

        tf = GetComponent<Transform>();
        guides = GetComponentsInChildren<SpriteRenderer>();
        instance = FMODUnity.RuntimeManager.CreateInstance("event:/effects/cannon");
        gun_sounds[0, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun0-0");
        gun_sounds[0, 0].setVolume(0.25f);
        gun_sounds[0, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun0-1");
        gun_sounds[0, 1].setVolume(0.25f);
        gun_sounds[0, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun0-2");
        gun_sounds[0, 2].setVolume(0.25f);
        gun_sounds[1, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun1-0");
        gun_sounds[1, 0].setVolume(0.25f);
        gun_sounds[1, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun1-1");
        gun_sounds[1, 1].setVolume(0.25f);
        gun_sounds[1, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun1-2");
        gun_sounds[1, 2].setVolume(0.25f);
        gun_sounds[2, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun2-0");
        gun_sounds[2, 0].setVolume(0.25f);
        gun_sounds[2, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun2-1");
        gun_sounds[2, 1].setVolume(0.25f);
        gun_sounds[2, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun2-2");
        gun_sounds[2, 2].setVolume(0.25f);
        gun_sounds[3, 0] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun3-0");
        gun_sounds[3, 0].setVolume(0.1f);
        gun_sounds[3, 1] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun3-1");
        gun_sounds[3, 1].setVolume(0.1f);
        gun_sounds[3, 2] = FMODUnity.RuntimeManager.CreateInstance("event:/effects/gun3-2");
        gun_sounds[3, 2].setVolume(0.1f);
        instance.start();
        instance.setVolume(0.5f);
    }

    float to2Pi(float x, float y)
    {
        float angle = Mathf.Atan(y / x);
        if (x < 0) angle += Mathf.PI;
        else if (y < 0) angle += Mathf.PI * 2;
        return angle;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(left_stick.x == 0 && right_stick.y == 0) && !(prev_left_stick.x == 0 && prev_left_stick.y == 0))
        {
            float prev_angle = to2Pi(prev_left_stick.x, prev_left_stick.y);
            float new_angle = to2Pi(left_stick.x, left_stick.y);

            if (prev_angle < Mathf.PI / 2 && new_angle > Mathf.PI * 3 / 2) prev_angle += Mathf.PI * 2;

            float angle_delta = prev_angle - new_angle;

            if (angle_delta < Mathf.PI / 2 && angle_delta > 0)
            {
                cur_tint -= angle_delta / 50.0f;
                if (cur_tint < 0.0f) cur_tint = 0.0f;

                if (cur_tint == 0.0f)
                {
                    guides[0].sprite = red;
                    guides[1].sprite = red;
                }
                else if (cur_tint < charge_3)
                {
                    guides[0].sprite = orange;
                    guides[1].sprite = orange;
                }
                else if (cur_tint < charge_2)
                {
                    guides[0].sprite = yellow;
                    guides[1].sprite = yellow;
                }
                else if (cur_tint < charge_1)
                {
                    guides[0].sprite = green;
                    guides[1].sprite = green;
                }
            }
        }

        if (right_stick.x < 0) right_stick.x = 0;
        if (right_stick.y < 0) right_stick.y = 0;
        if (right_stick.x != 0.0f || right_stick.y != 0.0f) {
            right_stick.Normalize();
            float desired_angle = Mathf.Atan(right_stick.y / right_stick.x);
            float current_angle = Mathf.Atan(tf.position.y / tf.position.x);
            float final_angle = current_angle + (desired_angle - current_angle) * aim_ease_factor;
            Debug.Log(desired_angle + ", " + right_stick.x + ", " + right_stick.y);
            tf.SetPositionAndRotation(new Vector3(Mathf.Cos(final_angle), Mathf.Sin(final_angle), 0), Quaternion.Euler(0.0f, 0.0f, final_angle * 180.0f / Mathf.PI));
        }

        if (buttons_down.x == 0) {
            if (cur_triggers.x > prev_triggers.x) buttons_down.x = 10;
        }
        else {
            if (cur_triggers.x != 1) buttons_down.x = 0;
            else buttons_down.x--;
        }
        if (buttons_down.y == 0)
        {
            if (left_bumper) buttons_down.y = 10;
        }
        else
        {
            if (left_bumper) buttons_down.y--;
            else buttons_down.y = 0;
        }
        if (buttons_down.z == 0)
        {
            if (right_bumper) buttons_down.z = 10;
        }
        else
        {
            if (right_bumper) buttons_down.z--;
            else buttons_down.z = 0;
        }
        if (buttons_down.w == 0)
        {
            if (cur_triggers.y > prev_triggers.y) buttons_down.w = 10;
        }
        else
        {
            if (cur_triggers.y != 1) buttons_down.w = 0;
            else buttons_down.w--;
        }
        
        bool fire = (buttons_down.x > 0 || cur_tint > 0.0f) && (buttons_down.y > 0 || cur_tint > charge_2) && (buttons_down.z > 0) && (buttons_down.w > 0 || cur_tint > charge_3);
        if (fire && cur_tint < charge_1) {
            GameObject cur_beam = Instantiate(beam_prefab);
            cur_beam.transform.SetPositionAndRotation(transform.position, transform.rotation);
            cur_beam.transform.localScale = new Vector3(2.4f, guides[0].transform.localPosition.y * 11.0f, 1.0f);
            AutoDestroy ad = cur_beam.GetComponent<AutoDestroy>();

            if (cur_tint == 0.0f) ad.damage = 7;
            else if (cur_tint < charge_3) ad.damage = 4;
            else if (cur_tint < charge_2) ad.damage = 2;
            else ad.damage = 1;

            int main_idx = 0;
            int rand_idx = Mathf.RoundToInt(Random.Range(-0.499f, 2.499f));
            if (cur_tint == 0.0f) main_idx = 3;
            else if (cur_tint < charge_3) main_idx = 2;
            else if (cur_tint < charge_2) main_idx = 1;
            gun_sounds[main_idx, rand_idx].start();

            ad.lifetime = (charge_1 - cur_tint) / charge_1 * 0.325f + 0.075f;
            cur_tint = 1.0f;
            guides[0].sprite = black;
            guides[1].sprite = black;
        }
    }

    void LateUpdate()
    {

        prev_left_stick = left_stick;
        prev_triggers = cur_triggers;

        float new_size = 0.05f;
        if (cur_tint < charge_1) new_size = (charge_1 - cur_tint) / charge_1 * 0.95f + 0.05f;
        float real_size = guides[0].transform.localPosition.y + (new_size - guides[0].transform.localPosition.y) * shoot_ease_factor;

        guides[0].transform.localPosition = new Vector3(0.0f, real_size, 0.0f);
        guides[1].transform.localPosition = new Vector3(0.0f, -real_size, 0.0f);
        guides[2].transform.localPosition = new Vector3(0.0f, real_size, 0.0f);
        guides[3].transform.localPosition = new Vector3(0.0f, -real_size, 0.0f);

        float ease_tint = guides[2].transform.localScale.x + (1.0f - cur_tint - guides[2].transform.localScale.x) * charge_ease_factor;
        guides[2].transform.localScale = new Vector3(ease_tint, 1.0f, 1.0f);
        guides[3].transform.localScale = new Vector3(ease_tint, 1.0f, 1.0f);

        instance.setParameterByName("power", 1.0f - cur_tint);

        if (controls.Gameplay.Pause.WasPressedThisFrame())
        {
            pause_menu.SetActive(true);
            game_objects.SetActive(false);
        }
        
    }
}
