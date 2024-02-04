using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Asteroid : MonoBehaviour, IPooledObject, IFallingObject
{
    [SerializeField]
    private TMP_Text wordText;

    [SerializeField]
    private TMP_Text wrongText;
    public void OnObjectSpawned()
    {

    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    public void SetText(string text)
    {
        wordText.text = text;
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

}
