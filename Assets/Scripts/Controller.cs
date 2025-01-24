using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    Transform tf;
    Camera cam;
    Vector3 left_stick = new Vector3(0,0,-10);
    Vector3 right_stick = new Vector3(0, 0, 0);

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

        bool fire = (Input.GetAxis("Fire1") == 1) && (Input.GetAxis("Fire2") == 1) && Input.GetKeyDown(KeyCode.Joystick1Button7) && Input.GetKeyDown(KeyCode.Joystick1Button8);
        if (fire) Debug.Log("Hello!");
    }

    void LateUpdate()
    {
        cam.transform.position = left_stick;

    }
}
