using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Change the controllers text based on which input system is being used.
public class ControlsToggle : MonoBehaviour
{
    void OnEnable()
    {
        if (PlayerPrefs.GetInt("keyboard") == 1) gameObject.GetComponent<TextMeshPro>().text = "Destroy enemy invaders to protect your base!\nScroll the mouse wheel to charge your energy \nbeam and use w and d to aim. A higher charge \ndeals more damage but is slower to move. To \nshoot, press the space bar. Press escape to \npause the game. Good luck!";
        else gameObject.GetComponent<TextMeshPro>().text = "Destroy enemy invaders to protect your base!\nSpin the left stick to charge your energy beam\nand use the right stick to aim. To shoot, use \nthe bumpers and triggers. The more power, \nthe more buttons you press. A higher charge\ndeals more damage but is slower to move.\nPress start to pause the game. Good luck!";
    }
}
