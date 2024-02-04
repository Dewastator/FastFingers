using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ListOfGameObjects", menuName = "Scriptables/ListOfGameObjects")]
public class ListOfGameObjects : ScriptableObject
{
    public List<GameObject> list = new List<GameObject>();
}