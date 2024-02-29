using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Backspace : MonoBehaviour
{
    
    [SerializeField]
    private TMP_InputField _playerInput;

    private void OnMouseDown()
    {
        InvokeRepeating("DeleteLetter", 0f, 0.1f);
    }
    
    public void DeleteLetter()
    {
        if (_playerInput.text.Length > 0)
            _playerInput.text = _playerInput.text.Remove(_playerInput.text.Length - 1);
    }
    private void OnMouseUp()
    {
        CancelInvoke();
    }

}
