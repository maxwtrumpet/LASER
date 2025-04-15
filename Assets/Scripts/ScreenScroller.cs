using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The component for scrolling the menu screen.
public class ScreenScroller : MonoBehaviour
{

    // Game Objects for differnt layers in the background.
    // Two of each type for infinite scrolling.
    [SerializeField] GameObject[] floors = new GameObject[2];
    [SerializeField] GameObject[] bg_2 = new GameObject[2];
    [SerializeField] GameObject[] bg_3 = new GameObject[2];
    [SerializeField] GameObject[] bg_4 = new GameObject[2];

    // Scroll each layer progeressively slover.
    // Wrap the object around if it reaches the end of the screen.
    void Update()
    {
        float change = floors[0].transform.localPosition.x + 0.05f;
        if (change >= 18.0f) floors[0].transform.localPosition = new Vector3(-18.0f + change - 18.0f, -5.0f, 0.0f);
        else floors[0].transform.localPosition = new Vector3(change, -5.0f, 0.0f);
        change = floors[1].transform.localPosition.x + 0.05f;
        if (change >= 18.0f) floors[1].transform.localPosition = new Vector3(-18.0f + change - 18.0f, -5.0f, 0.0f);
        else floors[1].transform.localPosition = new Vector3(change, -5.0f, 0.0f);

        change = bg_2[0].transform.localPosition.x + 0.001f;
        if (change >= 18.0f) bg_2[0].transform.localPosition = new Vector3(-18.0f + change - 18.0f, 0.0f, 0.0f);
        else bg_2[0].transform.localPosition = new Vector3(change, 0.0f, 0.0f);
        change = bg_2[1].transform.localPosition.x + 0.001f;
        if (change >= 18.0f) bg_2[1].transform.localPosition = new Vector3(-18.0f + change - 18.0f, 0.0f, 0.0f);
        else bg_2[1].transform.localPosition = new Vector3(change, 0.0f, 0.0f);

        change = bg_3[0].transform.localPosition.x + 0.005f;
        if (change >= 18.0f) bg_3[0].transform.localPosition = new Vector3(-18.0f + change - 18.0f, 0.0f, 0.0f);
        else bg_3[0].transform.localPosition = new Vector3(change, 0.0f, 0.0f);
        change = bg_3[1].transform.localPosition.x + 0.005f;
        if (change >= 18.0f) bg_3[1].transform.localPosition = new Vector3(-18.0f + change - 18.0f, 0.0f, 0.0f);
        else bg_3[1].transform.localPosition = new Vector3(change, 0.0f, 0.0f);

        change = bg_4[0].transform.localPosition.x + 0.01f;
        if (change >= 18.0f) bg_4[0].transform.localPosition = new Vector3(-18.0f + change - 18.0f, 0.0f, 0.0f);
        else bg_4[0].transform.localPosition = new Vector3(change, 0.0f, 0.0f);
        change = bg_4[1].transform.localPosition.x + 0.01f;
        if (change >= 18.0f) bg_4[1].transform.localPosition = new Vector3(-18.0f + change - 18.0f, 0.0f, 0.0f);
        else bg_4[1].transform.localPosition = new Vector3(change, 0.0f, 0.0f);
    }
}
