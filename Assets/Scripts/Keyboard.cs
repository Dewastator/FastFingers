using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{

    [SerializeField]
    private RectTransform keyboardBackground;

    [SerializeField]
    private Key keyPrefab;
    [SerializeField]
    private Backspace backspacePrefab;

    [Header("KeyboardLines")]
    [SerializeField]
    private KeyboardLines[] lines;
    [Range(0, 1)]
    [SerializeField]
    private float keyToLineRatio;
    [Range(0, 1)]
    [SerializeField]
    private float keySpacingX;
    [Range(0, 1)]
    [SerializeField]
    private float backSpaceWidth;
    // Start is called before the first frame update
    void Start()
    {
        //CreateKeys();
        //PlaceKeys();
    }

    private void PlaceKeys()
    {
        Vector2 pos = Vector2.zero;
        int currentChildIndex = 0;
        float lineHeight = keyboardBackground.rect.height / lines.Length;
        float keyWidth = lineHeight * keyToLineRatio;
        float xSpacing = keySpacingX * lineHeight;
        var backSpaceOffset = backSpaceWidth * lineHeight;

        for (int i = 0; i < lines.Length; i++)
        {
            bool containsBackSpace = lines[i].keys.Contains(".");

            float halfLine = (float)lines[i].keys.Length / 2;
            

            float posX = keyboardBackground.anchoredPosition.x - (keyWidth + xSpacing) * halfLine + (keyWidth + xSpacing) / 2;
            float posY = keyboardBackground.rect.height / 2 - lineHeight / 2 - i * lineHeight;


            for (int j = 0; j < lines[i].keys.Length; j++)
            {
                float keyX = posX + j * (keyWidth + xSpacing);
                bool isBackSpaceKey = lines[i].keys[j] == '.';

                if (containsBackSpace && !isBackSpaceKey)
                    keyX += (keyWidth + xSpacing) / 2;

                if (isBackSpaceKey)
                    keyX += keyWidth + xSpacing;

                var key = keyboardBackground.GetChild(currentChildIndex).GetComponent<RectTransform>();

                key.anchoredPosition = new Vector2(keyX, posY);
                key.sizeDelta = new Vector2(keyWidth, keyWidth);

                if (isBackSpaceKey)
                { 
                    key.sizeDelta = new Vector2(keyWidth * 1.5f, keyWidth);
                }
                currentChildIndex++;
            }
        }

    }

    private void CreateKeys()
    {
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].keys.Length; j++)
            {
                char key = lines[i].keys[j];

                if (key == '.')
                {
                    Backspace backspace = Instantiate(backspacePrefab, keyboardBackground);
                }
                else
                {
                    Key keyInstance = Instantiate(keyPrefab, keyboardBackground);
                    keyInstance.SetKey(key);
                }
                
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}

[System.Serializable]
struct KeyboardLines
{
    public string keys;
}
