using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms.Impl;

public class EndlessMode : MonoBehaviour
{

    [SerializeField]
    TextResource textResource;
    [SerializeField]
    private int maxNumberOfWords = 1000;
    [SerializeField]
    protected TMP_InputField _playerInput;
    [SerializeField]
    Transform objectSpawner;

    [SerializeField]
    private GameObject objectVFX;

    public ListOfStrings words;
    private string[] allLines;
    int spaceIndex = 0;
    protected string _currentWord;
    private bool isWrongText;

    public ListOfGameObjects fallingObjects;
    private IFallingObject currentObject;

    public UnityEvent OnEndlessModeStarted;
    public UnityEvent OnTakeHit;

    private int objectIndex;
    private int wordIndex;
    private int comboAmount;
    private int comboNumber = 10;
    public BoolValue canSpawn;

    private EndlessModeScore score;
    private Combo combo;


    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<EndlessModeScore>();
        combo = GetComponent<Combo>();
        words.list.Clear();
        words.list = GetWords().Split(' ').ToList();
        _currentWord = words.list[0];
        StartCoroutine(StartEndlessMode());
        objectVFX.SetActive(false);

#if UNITY_ANDROID
#endif
    }

    private IEnumerator StartEndlessMode()
    {
        yield return new WaitForSeconds(1f);
        OnEndlessModeStarted.Invoke();
    }

    private string GetWords()
    {
        allLines = textResource.text.Split();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < maxNumberOfWords; i++)
        {
            var randWord = allLines[UnityEngine.Random.Range(0, allLines.Length)] + " ";
            if (!sb.ToString().Contains(randWord) && randWord.Length <= 8)
            {
                sb.Append(randWord);
            }
        }

        return sb.ToString();
    }

    public void CheckText()
    {
        if (!canSpawn.value)
            return;

        if (currentObject != null && !currentObject.Enabled() || currentObject.GetText() == "")
            return;

        if (spaceIndex > 0) //Protecting twice text check after space at word submition
        {
            spaceIndex = 0;
            return;
        }
        HighlightText();
        
        if (_playerInput.text.IsStringEmpty() && !Input.GetKey(KeyCode.Backspace))
        {
            _playerInput.text = "";
        }
        if (!_playerInput.text.IsStringEmpty() && _playerInput.text[_playerInput.text.Length - 1] == ' ')
        {
            spaceIndex++;
            HandleSubmittion(_playerInput.text.Trim(_playerInput.text[_playerInput.text.Length - 1]) == _currentWord);
        }
    }

    private void ResetTextAndSetCurrentWord()
    {
        _playerInput.text = "";
        wordIndex++;
        _currentWord = words.list[wordIndex];
        isWrongText = false;
    }

    private void HandleSubmittion(bool correct)
    {
        if (correct)
        {
            comboAmount++;
            combo.IncreaseCombo(comboAmount);
            if (comboAmount == comboNumber)
            {
                comboAmount = 0;
                canSpawn.value = false;
                HandleComboBonus();
                return;
            }
        }
        else
        {
            //Play animation for wrong answers

            comboAmount = 0;

            //For now just destroy
            OnTakeHit.Invoke();
        }
        HandleObjectsDeath(correct);
    }

    private void HandleComboBonus()
    {

        for (int i = 0; i < 20; i++)
        {
            var go = objectSpawner.GetChild(i).gameObject;
            if (go.activeInHierarchy && !go.GetComponent<IFallingObject>().IsAlreadyDead())
            {
                HandleObjectsDeath(true);
            }
        }
        canSpawn.value = true;

    }

    public void HandleObjectsDeath(bool correct)
    {

        objectVFX.transform.SetParent(null);
        objectVFX.SetActive(false);
        currentObject.Destroy(correct);

        if (correct)
        {
            score.IncreaseScore();
        }

        if (objectIndex == objectSpawner.childCount / 2)
            objectSpawner.GetComponent<ObjectSpawner>().spawnSpeed -= 0.2f;

        if (objectIndex == objectSpawner.childCount - 1)
        {
            objectIndex = -1;
        }

        objectIndex++;
        currentObject = objectSpawner.GetChild(objectIndex).gameObject.GetComponent<IFallingObject>();

        objectVFX.transform.SetParent(objectSpawner.GetChild(objectIndex));
        objectVFX.transform.localPosition = new Vector3(0.07f, -0.07f, 0);
        objectVFX.SetActive(true);

        ResetTextAndSetCurrentWord();
    }

    private void HighlightText()
    {
        if (!_playerInput.text.IsStringEmpty() && _currentWord.Length >= _playerInput.text.Length && _playerInput.text[_playerInput.text.Length - 1] != _currentWord[_playerInput.text.Length - 1] && !isWrongText)
        {
            currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), words.list[wordIndex], "<mark=#FF0000>" + words.list[wordIndex] + "</mark>"));
            isWrongText = true;
        }
        if (_playerInput.text.Length > _currentWord.Length && _playerInput.text[_playerInput.text.Length - 1] != ' ' && !isWrongText)
        {
            currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), words.list[wordIndex], "<mark=#FF0000>" + words.list[wordIndex] + "</mark>"));
            isWrongText = true;
        }
        if (!_playerInput.text.IsStringEmpty() && _currentWord.Length >= _playerInput.text.Length && _currentWord.Contains(_playerInput.text) && isWrongText && _currentWord[0] == _playerInput.text[0])
        {
            currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), "<mark=#FF0000>" + words.list[wordIndex] + "</mark>", words.list[wordIndex]));
            isWrongText = false;
        }
        if (_playerInput.text.IsStringEmpty() && isWrongText)
        {
            currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), "<mark=#FF0000>" + words.list[wordIndex] + "</mark>", words.list[wordIndex]));
            isWrongText = false;
        }
    }
                                                
    public void SetCurrentFallingObject()
    {
        if (currentObject == null)
        {
            currentObject = objectSpawner.GetChild(0).GetComponent<IFallingObject>();
            objectVFX.SetActive(true);
            objectVFX.transform.SetParent(objectSpawner.GetChild(0));
            objectVFX.transform.localPosition = new Vector3(0.07f, -0.07f, 0);

        }
    }
}
