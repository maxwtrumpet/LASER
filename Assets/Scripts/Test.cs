using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    string MergeAlternately(string word1, string word2)
    {
        int max_length = Mathf.Max(word1.Length, word2.Length);
        string answer = "";
        for (int i = 0; i < max_length; i++)
        {
            if (i < word1.Length) answer += word1[i];
            if (i < word2.Length) answer += word2[i];
        }
        return answer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
