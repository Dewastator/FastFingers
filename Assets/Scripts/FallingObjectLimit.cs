using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallingObjectLimit : MonoBehaviour
{
    [SerializeField]
    LayerMask fallingObject;

    public UnityEvent OnLimitHit;

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == 6)
            OnLimitHit.Invoke();
    }
}
