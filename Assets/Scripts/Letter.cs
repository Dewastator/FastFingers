using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Letter : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private float upForce = 1f;
    [SerializeField]
    private float sideForce = 0.1f;
    [SerializeField]
    TMP_Text letterText;
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public void OnObjectSpawned()
    {

        letterText.text = chars[Random.Range(0, chars.Length - 1)].ToString();
        float xOffset = Random.Range(-sideForce, sideForce);
        float yoffset = Random.Range(upForce - 1, upForce);
        Vector2 force = new Vector2(xOffset, yoffset);

        GetComponent<Rigidbody2D>().velocity = force;
    }

    

}
