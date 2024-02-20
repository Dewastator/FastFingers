using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BoolValue",menuName = "ScriptableObjects/Bool")]
public class BoolValue : ScriptableObject
{
    public bool value;

    public void Started()
    {
        value = true;
    }
}
