using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pool", menuName = "ScriptableObjects/Pool")]
public class Pool : ScriptableObject
{
    public string tag;
    public GameObject poolPrefab;
    public int size;
}
