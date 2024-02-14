using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NormalMode : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _transcript;
    [SerializeField]
    private TMP_Text _wrongTranscript;
    [SerializeField]
    protected TMP_InputField _playerInput;

    [SerializeField]
    protected Score score;

    public TextResource textResource;
    public UnityEvent TimeModeStartedEvent;

    private string[] allLines;
    private int maxWords = 200;
    private List<string> words = new List<string>();
    Vector2 originalTextPosition;
    int spaceIndex = 0;
    List<char> chars = new List<char>();
    char[] _currChars;
    int lineIndex = 0;
    int firstWord;


    private bool isWrongText;
    private bool gameStarted;

    protected string _currentWord;

    protected StringBuilder allWords = new StringBuilder();
    protected StringBuilder allCorrectWords = new StringBuilder();
    protected StringBuilder allFalseWords = new StringBuilder();

    protected List<string> allWordsTyped = new List<string>();

    void Start()
    {
        originalTextPosition = _transcript.transform.localPosition;
        SetupText();
    }


    public void SetupText()
    {
        isWrongText = false;
        _transcript.text = GetWords();
        _wrongTranscript.text = _transcript.text;
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

    private string GetWords()
    {
        allLines = textResource.text.Split();

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

    public void CheckText()
    {
        if (spaceIndex > 0) //Protecting twice text check after space at word submition
        {
            spaceIndex = 0;
            return;
        }
        HighlightText();
        if (!_playerInput.text.IsStringEmpty() && !Input.GetKey(KeyCode.Backspace))
        {
            if (!gameStarted)
                StartTimer();

            score.allKeystrokes++;
        }
        else if (!Input.GetKey(KeyCode.Backspace))
        {
            _playerInput.text = "";
        }
        if (!_playerInput.text.IsStringEmpty() && _playerInput.text[_playerInput.text.Length - 1] == ' ')
        {
            spaceIndex++;
            PaintWord(_playerInput.text.Trim(_playerInput.text[_playerInput.text.Length - 1]) == _currentWord);
            SaveCharsForNewLineCheck();
            ResetTextAndSetCurrentWord();
        }
    }
    private void StartTimer()
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
        if (!_playerInput.text.IsStringEmpty() && _currentWord.Length >= _playerInput.text.Length && _playerInput.text[_playerInput.text.Length - 1] != _currentWord[_playerInput.text.Length - 1] && !isWrongText)
        {
            _wrongTranscript.text = DataHelper.ReplaceFirstOccurrence(_wrongTranscript.text, words[0], "<mark=#FF0000>" + words[0] + "</mark>");
            isWrongText = true;
        }
        if (_playerInput.text.Length > _currentWord.Length && _playerInput.text[_playerInput.text.Length - 1] != ' ' && !isWrongText)
        {
            _wrongTranscript.text = DataHelper.ReplaceFirstOccurrence(_wrongTranscript.text, words[0], "<mark=#FF0000>" + words[0] + "</mark>");
            isWrongText = true;
        }
        if (!_playerInput.text.IsStringEmpty() && _currentWord.Length >= _playerInput.text.Length && _currentWord.Contains(_playerInput.text) && isWrongText && _currentWord[0] == _playerInput.text[0])
        {
            _wrongTranscript.text = DataHelper.ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words[0] + "</mark>", words[0]);
            isWrongText = false;
        }
        if (_playerInput.text.IsStringEmpty() && isWrongText)
        {
            _wrongTranscript.text = DataHelper.ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words[0] + "</mark>", words[0]);
            isWrongText = false;
        }
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
                _transcript.text = DataHelper.ReplaceFirstOccurrence(_transcript.text, " " + words[0], "<color=green> " + words[0] + "</color>");
            else
                _transcript.text = DataHelper.ReplaceFirstOccurrence(_transcript.text, words[0], "<color=green>" + words[0] + "</color>");

            score.correctWords++;
            allCorrectWords.Append(words[0] + " ");

        }
        else
        {
            if (firstWord != 0)
                _transcript.text = DataHelper.ReplaceFirstOccurrence(_transcript.text, " " + words[0], "<color=red> " + words[0] + "</color>");
            else
                _transcript.text = DataHelper.ReplaceFirstOccurrence(_transcript.text, words[0], "<color=red>" + words[0] + "</color>");

            score.wrongWords++;
            allFalseWords.Append(_playerInput.text + " ");
            _wrongTranscript.text = DataHelper.ReplaceFirstOccurrence(_wrongTranscript.text, "<mark=#FF0000>" + words[0] + "</mark>", words[0]);
            CalculateUncorrectedErrors();
        }

        firstWord++;
    }

    private void CalculateUncorrectedErrors()
    {
        for (int i = 0; i < words[0].Length; i++)
        {
            if (i >= _playerInput.text.Length)
                break;

            if (_playerInput.text[i] != words[0][i])
            {
                score.uncorrectedErrors++;
            }
        }
    }

    public void EvaluateScore()
    {
        score.allKeystrokes -= _playerInput.text.Length;
        double wpmCalc = (((double)allCorrectWords.Length / 5) - score.uncorrectedErrors);
        if (wpmCalc > 0)
            score.wpm = wpmCalc;
        Debug.Log(allCorrectWords.Length);
        score.accuracy = ((float)allCorrectWords.Length / score.allKeystrokes) * 100;
        allWordsTyped.Add(allCorrectWords.ToString());
        score.SetScore();
    }

}
