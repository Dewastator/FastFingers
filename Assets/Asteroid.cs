using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Asteroid : MonoBehaviour, IPooledObject, IFallingObject
{
    [SerializeField]
    private TMP_Text wordText;

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
}
