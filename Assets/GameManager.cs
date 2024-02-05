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
    Score score;
    private string _currentWord;
    private List<string> words = new List<string>();
    private int maxWords = 200;
    List<char> chars = new List<char>();
    char[] _currChars;
    int spaceIndex = 0;
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
    bool isWrongText;
    bool gameStarted;
    public UnityEvent TimeModeStartedEvent;

    private void Awake()
    {
        foreach (TextResource text in listOfResources)
        {
            textResources.Add(text.language, text);
        }
        _transcript.text = GetWords("English");
        _wrongTranscript.text = _transcript.text;
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
        words = _transcript.text.Split(' ').ToList();
        _currentWord = words[0];
        originalTextPosition = _transcript.transform.localPosition;
    }

    private void SaveCharsForNewLineCheck()
    {
        _currChars = _currentWord.ToCharArray();
        foreach (char ch in _currChars)
        {
            chars.Add(ch);

        }
        chars.Add(' ');
        NewLine();

    }

    public void CheckText()
    {
        if (spaceIndex > 0) //Protecting twice text check after space or word submition
        {
            spaceIndex = 0;
            return;
        }
        HighlightText();
        if (!isStringEmpty(_playerInput.text) && !Input.GetKey(KeyCode.Backspace))
        {
            score.allKeystrokes++;
            if (!gameStarted)
                StartGame();
        }
        else if(!Input.GetKey(KeyCode.Backspace))
        {
            _playerInput.text = "";
        }
        if (!isStringEmpty(_playerInput.text) && _playerInput.text[_playerInput.text.Length - 1] == ' ')
        {
            spaceIndex++;
            PaintWord(_playerInput.text.Trim(_playerInput.text[_playerInput.text.Length - 1]) == _currentWord);
            SaveCharsForNewLineCheck();
            ResetTextAndSetCurrentWord();
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        TimeModeStartedEvent.Invoke();
    }

    private void ResetTextAndSetCurrentWord()
    {
        _playerInput.text = "";
        words.RemoveAt(0);
        _currentWord = words.Count > 0 ? words[0] : "";
        isWrongText = false;
    }

    private void HighlightText()
    {
        if (!isStringEmpty(_playerInput.text) && _currentWord.Length >= _playerInput.text.Length && _playerInput.text[_playerInput.text.Length - 1] != _currentWord[_playerInput.text.Length - 1] && !isWrongText)
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, words[0], "<mark=#FF0000>" + words[0] + "</mark>");
            isWrongText = true;
        }
        if (_playerInput.text.Length > _currentWord.Length && _playerInput.text[_playerInput.text.Length - 1] != ' ' && !isWrongText)
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, words[0], "<mark=#FF0000>" + words[0] + "</mark>");
            isWrongText = true;
        }
        if (!isStringEmpty(_playerInput.text) && _currentWord.Length >= _playerInput.text.Length && _currentWord.Contains(_playerInput.text) && isWrongText && _currentWord[0] == _playerInput.text[0])
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words[0] + "</mark>", words[0]);
            isWrongText = false;
        }
        if (isStringEmpty(_playerInput.text) && isWrongText)
        {
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words[0] + "</mark>", words[0]);
            isWrongText = false;
        }
    }

    private void NewLine()
    {
        //Comparing number of typed chars submitted, with TMP chars in current line to create a new line
        if (_transcript.textInfo.lineInfo[lineIndex].characterCount == chars.Count())
        {
            lineIndex++;
            chars = new List<char>();
            _transcript.transform.localPosition = new Vector2(_transcript.transform.localPosition.x, _transcript.transform.localPosition.y + 60f);
            _wrongTranscript.transform.localPosition = new Vector2(_wrongTranscript.transform.localPosition.x, _wrongTranscript.transform.localPosition.y + 60f);
        }
    }

    private void PaintWord(bool correct)
    {
        if (correct)
        {
            if (firstWord != 0)
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, " " + words[0], "<color=green> " + words[0] + "</color>");
            else
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, words[0], "<color=green>" + words[0] + "</color>");

            score.correctWords++;
            allCorrectWords.Append(words[0] + " ");

        }
        else
        {
            if (firstWord != 0)
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, " " + words[0], "<color=red> " + words[0] + "</color>");
            else
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, words[0], "<color=red>" + words[0] + "</color>");

            score.wrongWords++;
            allFalseWords.Append(_playerInput.text + " ");
            _wrongTranscript.text = ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words[0] + "</mark>", words[0]);

        }

        firstWord++;
    }

    public bool isStringEmpty(string a)
    {
        return a.Trim().Length == 0;
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
        score.wpm = (((double)allCorrectWords.Length / 5) - (double)allFalseWords.Length);
        score.accuracy = ((float)allCorrectWords.Length / score.allKeystrokes) * 100;
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
        words = _transcript.text.Split(' ').ToList();
        _currentWord = words[0];
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
        gameStarted = false;
    }
}
