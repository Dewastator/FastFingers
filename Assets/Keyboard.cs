using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour
{

    string keyboardLetters = "qwertyuiopasdfghjklzxcvbnm";

    [Header("KeyboardLines")]
    [SerializeField]
    private KeyboardLines[] lines;
    // Start is called before the first frame update
    void Start()
    {
        
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
