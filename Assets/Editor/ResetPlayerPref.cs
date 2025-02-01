using UnityEditor;
using UnityEngine;

public class ResetPlayerPref
{
    [MenuItem("Tools/Reset Specific PlayerPref")]
    public static void ResetSpecificPref()
    {
        string key = "played"; // Change this to the key you want to reset
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            Debug.Log($"PlayerPref '{key}' has been reset.");
        }
        else
        {
            Debug.Log($"PlayerPref '{key}' does not exist.");
        }
    }
}