using System;
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
    [SerializeField]
    TMP_Text wpmText;
    [SerializeField]
    TMP_Text accuracyText;
    public float correctWords;
    public float wrongWords;
    public double wpm;
    public float correctKeystrokes;
    public float allKeystrokes;
    public float accuracy;
    public int uncorrectedErrors;
    public void SetScore()
    {
        wpmText.text = "<color=green>" + System.Math.Round(wpm).ToString() + "WPM"+ "</color>";

        correctWordsText.text = "Correct words: " + "<color=green>" + correctWords.ToString() + "</color>";
        wrongWordsText.text = "Wrong words: " + "<color=red>" + wrongWords.ToString() + "</color>";
        accuracyText.text = "Accuracy " + String.Format("{0:.##}", accuracy) + "%";
    }

    public void ResetScore()
    {
        correctWords = wrongWords = accuracy = allKeystrokes = correctKeystrokes = uncorrectedErrors = 0;
        wpm = 0;
    }
}
