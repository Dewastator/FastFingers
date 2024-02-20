using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EndlessModeScore : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    public int currentScore;

    public void IncreaseScore()
    {
        currentScore += 1;
        scoreText.text = currentScore.ToString();

    }

    
}
