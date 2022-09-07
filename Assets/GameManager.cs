using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Text;
using System.IO;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _playerInput;
    [SerializeField]
    private TMP_Text _transcript;
    [SerializeField]
    Score score;
    private string _currentWord;
    private List<string> words = new List<string>();
    private int maxWords = 200;
    List<char> chars = new List<char>();
    char[] _currChars;
    int i = 0;
    int lineIndex = 0;
    int firstWord;
    
    private void Awake()
    {
        _transcript.text = GetWords();

    }

    private string GetWords()
    {
        string[] allLines = File.ReadAllLines("Assets/Resources/randomWords.txt");
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < maxWords; i++)
        {
            var randWord = allLines[UnityEngine.Random.Range(0, allLines.Length)] + " ";
            if (!sb.ToString().Contains(randWord))
                sb.Append(allLines[UnityEngine.Random.Range(0, allLines.Length)] + " ");
        }
        return sb.ToString();
    }

    private void Start()
    {
        words = _transcript.text.Split(' ').ToList();
        _currentWord = words[0];

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
        if (isStringEmpty(_playerInput.text))
            Debug.Log(_currentWord);
        if (!isStringEmpty(_playerInput.text) && _playerInput.text[_playerInput.text.Length - 1] == ' ')
        {
            i++;
            PaintWord(_playerInput.text.Trim(_playerInput.text[_playerInput.text.Length-1]) == _currentWord);
            PopulateChars();
            _playerInput.text = "";
            words.RemoveAt(0);
            _currentWord = words.Count > 0 ? words[0] : "";
            
                
        }
        
        if (_transcript.textInfo.lineInfo[lineIndex].characterCount == chars.Count())
        {
            lineIndex++;
            chars = new List<char>();
            _transcript.transform.localPosition = new Vector2(_transcript.transform.localPosition.x, _transcript.transform.localPosition.y + 60f);
        }
    }

    private void PaintWord(bool correct)
    {
        if (correct)
        {
            if(firstWord != 0)
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, " "+ words[0], "<color=green> " + words[0] + "</color>");
            else
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, words[0], "<color=green>" + words[0] + "</color>");

            score.correctWords++;
        }
        else
        {
            if (firstWord != 0)
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, " " + words[0], "<color=red> " + words[0] + "</color>");
            else
                _transcript.text = ReplaceFirstOccurrence(_transcript.text, words[0], "<color=red>" + words[0] + "</color>");

            score.wrongWords++;
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
        score.SetScore();
    }
    
    public void FinishGame()
    {

    }
}
