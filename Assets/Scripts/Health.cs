using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField]
    List<Image> healthIcons = new List<Image>();
    private int currentHealth = 3;
    public UnityEvent OnDeath;

    public void DecreaseHealth()
    {
        currentHealth--;
            healthIcons[currentHealth].DOFillAmount(0, 0.6f).OnComplete(()=> Death());
    }

    public void Death()
    {
        if (currentHealth == 0)
            OnDeath.Invoke();
    }
}
