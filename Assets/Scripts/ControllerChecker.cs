using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.InputSystem.LowLevel;

public class ControllerChecker : MonoBehaviour
{

    TextMeshPro tmp;
    int total_controllers = 0;

    private void Start()
    {
        tmp = GetComponent<TextMeshPro>();
        InputSystem.onDeviceChange += OnDeviceChange;
        foreach (var device in InputSystem.devices)
        {
            OnDeviceChange(device, InputDeviceChange.Added);
        }
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    bool CheckName(string device_name, string display_name)
    {
        device_name = device_name.ToLower();
        display_name = display_name.ToLower();
        return device_name.Contains("xbox") ||
               display_name.Contains("xbox") ||
               device_name.Contains("dualshock") ||
               device_name.Contains("dualsense") ||
               display_name.Contains("playstation") ||
               display_name.Contains("dualshock") ||
               display_name.Contains("dualsense");
    }

    public void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad && CheckName(gamepad.name, gamepad.displayName))
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    total_controllers++;
                    break;

                case InputDeviceChange.Removed:
                    total_controllers--;
                    break;

                case InputDeviceChange.Disconnected:
                    total_controllers--;
                    break;

                case InputDeviceChange.Reconnected:
                    total_controllers++;
                    break;
            }
        }

        if (total_controllers == 0) tmp.enabled = true;
        else tmp.enabled = false;
    }
}
