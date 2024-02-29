using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    [SerializeField]
    private TMP_Text keyText;
    [SerializeField]
    private TMP_InputField _playerInput;

    private Button keyButton;

    private string _key;

    private void Awake()
    {
        keyButton = GetComponent<Button>();
    }
    private void OnEnable()
    {
        keyButton.onClick.AddListener(() => OnButtonClick());
    }
    private void OnDisable()
    {
        keyButton.onClick.RemoveListener(() => OnButtonClick());
    }
    public void SetKey(char key) 
    {
        _key = key.ToString();
        keyText.text = _key.ToString();
    }

    public void SetPosition(Vector2 position)
    {
        GetComponent<RectTransform>().position = position;
    }

    public void OnButtonClick()
    {
        string text = keyText.text;
        if(keyText.text == "/n")
            text = " ";

        _playerInput.text += text.ToLower().ToString();
    }
}
