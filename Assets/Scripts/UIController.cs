using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private bool isLangActive = true;
    GameObject languageImage;
    public void ActivateLanguage()
    {
        languageImage.SetActive(isLangActive);
        isLangActive = !isLangActive;
    }
}
