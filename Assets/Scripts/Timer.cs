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
    float timepassed = 1;
    [SerializeField]
    TMP_Text timerText;
    public UnityEvent OnTimeElapsed;
    float startTimer;
    
    private void Start()
    {
        startTimer = timer;
        timerText.text = FormatTime(timer);
    }
    public void StartTimer()
    {
        StartCoroutine("TimerAnimation");
    }
    IEnumerator TimerAnimation()
    {
        while(timer != -1)
        {
            if (Time.time > timepassed)
            {
                timepassed = Time.time + 1;
                timer--;
                timerText.text = FormatTime(timer);
            }
            yield return null;
        }
    }
    public string FormatTime(float time)
    {
        if(time == -1)
        {
            OnTimeElapsed.Invoke();
            return string.Format("{0:00}", 0);

        }
        float minutes = Mathf.FloorToInt(timer / 60);
        float seconds = Mathf.FloorToInt(timer % 60);
        return string.Format("{0:00}", time);
    }

    public void ResetTimer()
    {
        timer = startTimer;
        timerText.text = FormatTime(timer);
        StopAllCoroutines();
    }
}
