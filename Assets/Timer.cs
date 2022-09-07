using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField]
    float timer = 120;
    float timepassed = 0;
    [SerializeField]
    TMP_Text timerText;
    public UnityEvent OnTimeElapsed;
    private void Start()
    {
    }
    private void Update()
    {
        if (Time.time > timepassed && timer != 0)
        {
            timepassed = Time.time + 1;
            timerText.text = FormatTime(timer);
            timer--;
            if (timer == 0)
                OnTimeElapsed.Invoke();
        }
    }
    public string FormatTime(float time)
    {
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
