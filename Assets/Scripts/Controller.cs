using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] float EASE_FACTOR = 0.2f;
    Transform tf;
    Camera cam;
    Vector3 left_stick = new Vector3(0,0,-10);
    Vector3 right_stick = new Vector3(0, 0, 0);
    Vector2 prev_triggers = new Vector2(0, 0);
    Vector2 cur_triggers = new Vector2(0, 0);
    Vector4 buttons_down = new Vector4(0, 0, 0, 0);

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
        tf = GetComponent<Transform>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

        left_stick.x = Input.GetAxis("Horizontal");
        left_stick.y = Input.GetAxis("Vertical");

        right_stick.x = Input.GetAxis("Horizontal2");
        right_stick.y = Input.GetAxis("Vertical2");
        if (right_stick.x < 0) right_stick.x = 0;
        if (right_stick.y < 0) right_stick.y = 0;
        if (right_stick.x != 0 || right_stick.y != 0) {
            right_stick.Normalize();
            float desired_angle = Mathf.Atan(right_stick.y / right_stick.x);
            float current_angle = Mathf.Atan(tf.position.y / tf.position.x);
            float final_angle = current_angle + (desired_angle - current_angle) * EASE_FACTOR;
            tf.position = new Vector3(Mathf.Cos(final_angle), Mathf.Sin(final_angle), 0);
        }

        cur_triggers.x = Input.GetAxis("Fire1");
        cur_triggers.y = Input.GetAxis("Fire2");
        if (buttons_down.x == 0) {
            if (cur_triggers.x > prev_triggers.x) buttons_down.x = 5;
        }
        else {
            if (cur_triggers.x != 1) buttons_down.x = 0;
            else buttons_down.x--;
        }
        if (buttons_down.y == 0)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button7)) buttons_down.y = 5;
        }
        else
        {
            if (Input.GetKey(KeyCode.Joystick1Button7)) buttons_down.y--;
            else buttons_down.y = 0;
        }
        if (buttons_down.z == 0)
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button8)) buttons_down.z = 5;
        }
        else
        {
            if (Input.GetKey(KeyCode.Joystick1Button8)) buttons_down.z--;
            else buttons_down.z = 0;
        }
        if (buttons_down.w == 0)
        {
            if (cur_triggers.y > prev_triggers.y) buttons_down.w = 5;
        }
        else
        {
            if (cur_triggers.y != 1) buttons_down.w = 0;
            else buttons_down.w--;
        }
        Debug.Log(buttons_down);
        
        bool fire = buttons_down.x > 0 && buttons_down.y > 0 && buttons_down.z > 0 && buttons_down.w > 0;
        if (fire) {
            GameObject new_proj = Instantiate(projectilePrefab);
            new_proj.GetComponent<Rigidbody2D>().velocity = new Vector2(tf.position.x, tf.position.y);
        }
    }

    void LateUpdate()
    {
        cam.transform.position = left_stick;
        prev_triggers = cur_triggers;

    }
}
