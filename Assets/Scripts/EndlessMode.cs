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
    ObjectSpawner objectSpawner;

    [SerializeField]
    private GameObject objectVFX;

    public ListOfStrings wordsList;
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

    private readonly HashSet<string> words = new HashSet<string>();

    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<EndlessModeScore>();
        combo = GetComponent<Combo>();
        words.Clear();
        wordsList.list.Clear();
        SetWords();
        wordsList.list = words.ToList();
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

    private void SetWords()
    {
        allLines = textResource.text.Split();

        for (int i = 0; i < maxNumberOfWords; i++)
        {
            var randWord = allLines[UnityEngine.Random.Range(0, allLines.Length)] + " ";
            if (!words.Contains(randWord) && randWord.Length <= 8)
            {
                words.Add(randWord.Trim());
            }
        }
    }

    public void CheckText()
    {
        if (!canSpawn.value)
            return;

        if (spaceIndex > 0) //Protecting twice text check after space at word submition
        {
            spaceIndex = 0;
            return;
        }
        //HighlightText();
        
        if (_playerInput.text.IsStringEmpty() && !Input.GetKey(KeyCode.Backspace))
        {
            _playerInput.text = "";
        }
        if (!_playerInput.text.IsStringEmpty() && _playerInput.text[_playerInput.text.Length - 1] == ' ')
        {
            spaceIndex++;
            HandleSubmittion(words.Contains(_playerInput.text.Trim()));
        }
    }

    private void ResetTextInput()
    {
        _playerInput.text = "";
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

        }
        HandleObjectsDeath(correct);
    }

    private void HandleComboBonus()
    {

        for (int i = 0; i < 20; i++)
        {
            var go = objectSpawner.transform.GetChild(i).gameObject;
            if (go.activeInHierarchy && !go.GetComponent<IFallingObject>().IsAlreadyDead())
            {
                go.GetComponent<IFallingObject>().Destroy(true);
                score.IncreaseScore();
            }
        }
        canSpawn.value = true;
        ResetTextInput();
    }

    public void HandleObjectsDeath(bool correct, bool combo = false)
    {

        //objectVFX.transform.SetParent(null);
        //objectVFX.SetActive(false);
        
        if (correct)
        {
            score.IncreaseScore();

            currentObject = objectSpawner.spawnedFallingObjects[_playerInput.text.Trim()];

            currentObject.Destroy(correct);
        }

        //if (objectIndex == objectSpawner.childCount / 2)
        //    objectSpawner.GetComponent<ObjectSpawner>().spawnSpeed -= 0.2f;

        //if (objectIndex == objectSpawner.childCount - 1)
        //{
        //    objectIndex = -1;
        //}

        objectIndex++;
        if(objectIndex == 10)
        {
            objectSpawner.spawnSpeed -= 0.2f;
            objectIndex = 0;
        }

        

        //objectVFX.transform.SetParent(objectSpawner.GetChild(objectIndex));
        //objectVFX.transform.localPosition = new Vector3(0.07f, -0.07f, 0);
        //objectVFX.SetActive(true);

        ResetTextInput();
    }

    private void HighlightText()
    {
        //if (!_playerInput.text.IsStringEmpty() && _currentWord.Length >= _playerInput.text.Length && _playerInput.text[_playerInput.text.Length - 1] != _currentWord[_playerInput.text.Length - 1] && !isWrongText)
        //{
        //    currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), words.list[wordIndex], "<mark=#FF0000>" + words.list[wordIndex] + "</mark>"));
        //    isWrongText = true;
        //}
        //if (_playerInput.text.Length > _currentWord.Length && _playerInput.text[_playerInput.text.Length - 1] != ' ' && !isWrongText)
        //{
        //    currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), words.list[wordIndex], "<mark=#FF0000>" + words.list[wordIndex] + "</mark>"));
        //    isWrongText = true;
        //}
        //if (!_playerInput.text.IsStringEmpty() && _currentWord.Length >= _playerInput.text.Length && _currentWord.Contains(_playerInput.text) && isWrongText && _currentWord[0] == _playerInput.text[0])
        //{
        //    currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), "<mark=#FF0000>" + words.list[wordIndex] + "</mark>", words.list[wordIndex]));
        //    isWrongText = false;
        //}
        //if (_playerInput.text.IsStringEmpty() && isWrongText)
        //{
        //    currentObject.SetWrongText(DataHelper.ReplaceFirstOccurrence(currentObject.GetWrongText(), "<mark=#FF0000>" + words.list[wordIndex] + "</mark>", words.list[wordIndex]));
        //    isWrongText = false;
        //}
    }
                                                
}
