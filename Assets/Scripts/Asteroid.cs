using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour, IPooledObject, IFallingObject
{
    [SerializeField]
    private TMP_Text wordText;

    [SerializeField]
    private TMP_Text wrongText;
    
    [SerializeField]
    private TMP_Text pointText;

    [SerializeField]
    GameObject canvas;

    private Animator animator;

    [SerializeField]
    private float Speed = 4f;

    public bool isDead;
    private bool isEnabled;
    private void Start()
    {
        animator = GetComponent<Animator>();
        canvas.SetActive(true);
    }
    public void OnObjectSpawned()
    {
        
    }
    private void OnEnable()
    {
        Speed += 0.1f;
    }
    public void Destroy(bool correct)
    {
        isDead = true;

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        canvas.SetActive(false);
        animator.SetBool("IsExploding", true);
        var animDur = animator.GetCurrentAnimatorStateInfo(0).length;

        Invoke("DeactivateObject", animDur);
        if(correct)
            PointAnimation();
    }

    private void DeactivateObject()
    {
        gameObject.SetActive(false);
        Enable(false);
        transform.position = Vector3.zero;
        canvas.SetActive(true);
        isDead = false;
    }

    public void SetText(string text)
    {
        wordText.text = text;
        isEnabled = true;
    }

    public string GetText()
    {
        return wordText.text;
    }
    
    public void SetWrongText(string text)
    {
        wrongText.text = text;
    }

    public string GetWrongText()
    {
        return wrongText.text;
    }

    public bool Enabled()
    {
        return isEnabled;
    }

    public bool IsAlreadyDead()
    {
        return isDead;
    }

    private void Update()
    {
        transform.position -= transform.up * Time.deltaTime * Speed;
    }
    public void PointAnimation(int value = 1)
    {
        pointText.color = Color.white;
        pointText.text = string.Format("+{0}", value);
        float scale = value / 10 + 1;
        pointText.transform.localScale = new Vector3(scale, scale, scale);
        pointText.transform.localPosition = Vector3.zero;
        Sequence mySequence = DOTween.Sequence();

        mySequence
            .Insert(0.2f, pointText.DOColor(new Color(1f, 1f, 1f, 0f), 0.2f))
            .Insert(0, pointText.transform.DOMoveY(pointText.transform.position.y + 0.2f, 0.4f));
    }

    public void Enable(bool enable)
    {
        isEnabled = enable;
    }
}
