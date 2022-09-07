using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    TMP_Text correctWordsText;
    [SerializeField]
    TMP_Text wrongWordsText;

    public float correctWords;
    public float wrongWords;
    
    public void SetScore()
    {
        correctWordsText.text = "Correct words: " + correctWords.ToString();
        wrongWordsText.text = "Wrong words: " + wrongWords.ToString();

    }
}
