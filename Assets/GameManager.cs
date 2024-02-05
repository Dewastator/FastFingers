using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine.Events;
using System.Xml;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Button languageButton;
    [SerializeField]
    private TMP_InputField _playerInput;
    [SerializeField]
    private TMP_Text _transcript;
    [SerializeField]
    private TMP_Text _wrongTranscript;
    [SerializeField]
    private TMP_Text keystrokesText;
    [SerializeField]
    Score score;
    private string _currentWord;
    public ListOfStrings words;
    private int maxWords = 200;
    List<char> chars = new List<char>();
    char[] _currChars;
    int i = 0;
    int lineIndex = 0;
    int firstWord;
    [SerializeField]
    Dictionary<string, TextResource> textResources = new Dictionary<string, TextResource>();
    [SerializeField]
    List<TextResource> listOfResources = new List<TextResource>();
    string[] allLines;
    Vector2 originalTextPosition;
    public string currentLanguage;
    StringBuilder allWords = new StringBuilder();
    StringBuilder allCorrectWords = new StringBuilder();
    StringBuilder allFalseWords = new StringBuilder();
    public List<string> allWordsTyped = new List<string>();
    int falseIndex;
    bool isWrongText;
    public BoolValue gameStarted;
    public ListOfGameObjects fallingObjects;

    public IFallingObject currentObject;
    private void Awake()
    {
        words.list = new List<string>();
        foreach(TextResource text in listOfResources)
        {
            textResources.Add(text.language, text);
        }
    }
    private string GetWords(string language)
    {
        allLines = textResources[language].text.Split();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < maxWords; i++)
        {
            var randWord = allLines[UnityEngine.Random.Range(0, allLines.Length)] + " ";
            if (!sb.ToString().Contains(randWord) && randWord.Length <= 7)
            {
                sb.Append(randWord);
            }

        }
        return sb.ToString();
    }

    private void Start()
    {
        gameStarted.value = false;

        words.list = GetWords("English").Split(' ').ToList();
        _currentWord = words.list[0];
        originalTextPosition = _transcript.transform.localPosition;
    }

    public void GameStarted()
    {
        gameStarted.value = true;
    }
    public void SetCurrentObject()
    {
        currentObject = fallingObjects.list[0].GetComponent<IFallingObject>();
    }
    private void PopulateChars()
    {
        _currChars = _currentWord.ToCharArray();
        foreach (char ch in _currChars)
        {
            chars.Add(ch);

        }
        chars.Add(' ');
    }

    public void CheckText()
    {
        if (i > 0)
        {
            i = 0;
            return;
        }
        HighlightText();
        if(!isStringEmpty(_playerInput.text) && !Input.GetKey(KeyCode.Backspace))
        {
            score.allKeystrokes++;
            keystrokesText.text = score.allKeystrokes.ToString();
        }
        if (!isStringEmpty(_playerInput.text) && _playerInput.text[_playerInput.text.Length - 1] == ' ')
        {
            i++;
            PaintWord(_playerInput.text.Trim(_playerInput.text[_playerInput.text.Length - 1]) == _currentWord);
            PopulateChars();
            _playerInput.text = "";
            words.list.RemoveAt(0);
            _currentWord = words.list.Count > 0 ? words.list[0] : "";
            falseIndex = 0;
            isWrongText = false;
        }
    }

    private void HighlightText()
    {
        if (!isStringEmpty(_playerInput.text) && _currentWord.Length >= _playerInput.text.Length && _playerInput.text[_playerInput.text.Length - 1] != _currentWord[_playerInput.text.Length - 1] && !isWrongText)
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, words.list[0], "<mark=#FF0000>" + words.list[0] + "</mark>");
            isWrongText = true;
        }
        if (_playerInput.text.Length > _currentWord.Length && _playerInput.text[_playerInput.text.Length - 1] != ' ' && !isWrongText)
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, words.list[0], "<mark=#FF0000>" + words.list[0] + "</mark>");
            isWrongText = true;
        }
        if (!isStringEmpty(_playerInput.text) && _currentWord.Length >= _playerInput.text.Length &&  _currentWord.Contains(_playerInput.text) && isWrongText && _currentWord[0] == _playerInput.text[0])
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words.list[0] + "</mark>", words.list[0]);
            isWrongText = false;
        }
        if (isStringEmpty(_playerInput.text) && isWrongText)
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words.list[0] + "</mark>", words.list[0]);
            isWrongText = false;
        }
    }

    private void PaintWord(bool correct)
    {
        if (correct)
        {
            if(firstWord != 0)
                currentObject.SetText(ReplaceFirstOccurrence(currentObject.GetText(), " "+ words.list[0], "<color=green> " + words.list[0] + "</color>"));
            else
                currentObject.SetText(ReplaceFirstOccurrence(currentObject.GetText(), " " + words.list[0], "<color=green> " + words.list[0] + "</color>"));

            score.correctWords++;
            allCorrectWords.Append(words.list[0] + " ");

        }
        else
        {
            if (firstWord != 0)
                currentObject.SetText(ReplaceFirstOccurrence(currentObject.GetText(), " " + words.list[0], "<color=green> " + words.list[0] + "</color>"));
            else
                currentObject.SetText(ReplaceFirstOccurrence(currentObject.GetText(), " " + words.list[0], "<color=green> " + words.list[0] + "</color>"));

            score.wrongWords++;
            allFalseWords.Append(_playerInput.text + " ");
            currentObject.SetWrongText(ReplaceFirstOccurrence(currentObject.GetWrongText(), "<mark=#FF0000>" + words.list[0] + "</mark>", words.list[0]));

        }

        firstWord++;
    }

    public bool isStringEmpty(string a)
    {
        
        return a.Length == 0;
    }

    public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
    {
        int Place = Source.IndexOf(Find);
        string result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
        return result;
    }

    public void EvaluateScore()
    {
        score.allKeystrokes -= _playerInput.text.Length;
        score.wpm = (((double)allCorrectWords.Length / 5) - (double)allFalseWords.Length) ;
        score.accuracy = ((float)allCorrectWords.Length / score.allKeystrokes) * 100;
        Debug.Log((double)allCorrectWords.Length);
        Debug.Log((double)allFalseWords.Length);
        Debug.Log(score.allKeystrokes);
        Debug.Log(score.correctWords);
        allWordsTyped.Add(allCorrectWords.ToString());
        score.SetScore();
    }
    public void ChangeLanguage(string language)
    {
        currentLanguage = language;
        FinishGame();
    }
    public void FinishGame()
    {
        isWrongText = false;
        languageButton.interactable = true;
        _transcript.text = GetWords(currentLanguage);
        _wrongTranscript.text = _transcript.text;
        score.ResetScore();
        words.list = _transcript.text.Split(' ').ToList();
        _currentWord = words.list[0];
        firstWord = 0;
        lineIndex = 0;
        chars = new List<char>();
        _transcript.transform.localPosition = originalTextPosition;
        _wrongTranscript.transform.localPosition = originalTextPosition;
        allCorrectWords = new StringBuilder();
        allFalseWords = new StringBuilder();
        if (_playerInput.text != "")
        {
            _playerInput.text = "";
        }
    }

    
}
