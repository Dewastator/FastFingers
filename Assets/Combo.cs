using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Combo : MonoBehaviour
{
    [SerializeField]
    TMP_Text comboText;
    private int vibrato = 5;

    Vector3 originalPosition;
    private void Start()
    {
        originalPosition = comboText.transform.position;
    }
    public void IncreaseCombo(int value)
    {
        comboText.transform.position = originalPosition;
        comboText.color = Color.white;
        comboText.text = string.Format("x{0}" ,value);
        float scale = value / 10 + 1;
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(comboText.transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f))
            .AppendInterval(0.2f)
            .Append(comboText.DOColor(new Color(1, 1, 1, 0f), 0.5f));
        
    }
}
