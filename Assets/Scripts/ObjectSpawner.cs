using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawner : MonoBehaviour
{
    private float spawnTime;
    [SerializeField]
    private float spawnSpeed = 10;
    [SerializeField]
    private Transform spawnPosition;
    public BoolValue gameStarted;
    [SerializeField]
    private float xMin, xMax;
    public ListOfStrings words;
    private IntValue wordIndex;
    private int i;
    public ListOfGameObjects fallingObjects;
    public UnityEvent GameStartedEvent;
    // Start is called before the first frame update
    void Start()
    {
        gameStarted.value = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted.value)
        {
            if (spawnTime < Time.time)
            {
                spawnTime = Time.time + spawnSpeed;
                var go = ObjectPooler.Instance.SpawnFromPool("Asteroid2", new Vector2(Random.Range(xMin, xMax), spawnPosition.position.y), spawnPosition.rotation);
                go.GetComponent<IFallingObject>().SetText(words.list[i]);
                fallingObjects.list.Add(go);
                i++;
            }
        }
    }

}
