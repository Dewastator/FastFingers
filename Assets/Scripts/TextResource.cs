using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource")]

public class TextResource : ScriptableObject
{
    public string language;
    public string text;
}
