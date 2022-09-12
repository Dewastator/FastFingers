using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterSpawner : MonoBehaviour
{
    ObjectPooler pooler;
    GameObject spawnedObject;
    // Start is called before the first frame update
    public void Spawn()
    {
        pooler = ObjectPooler.Instance;
        StartCoroutine("SpawnLetters");
    }

    IEnumerator SpawnLetters()
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        spawnedObject = pooler.SpawnFromPool("Letter", transform.position, transform.rotation);
        spawnedObject.transform.SetParent(transform);
        StopAllCoroutines();
        StartCoroutine("SpawnLetters");
    }
    private void OnDisable()
    {
        if (transform.childCount == 0)
            return;

        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
}
