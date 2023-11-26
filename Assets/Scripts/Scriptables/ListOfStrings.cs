using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfStrings", menuName = "ScriptableObjects/ListOfStrings")]
public class ListOfStrings : ScriptableObject
{
    public List<string> list = new List<string>();
}
