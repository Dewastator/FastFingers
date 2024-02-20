using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSpawner : MonoBehaviour
{
    private float spawnTime;
    [SerializeField]
    public float spawnSpeed = 10;
    [SerializeField]
    private Transform spawnPosition;
    public BoolValue gameStarted;
    [SerializeField]
    private float xMin, xMax;
    public ListOfStrings words;
    public IntValue wordIndex;
    private int i;
    public UnityEvent ObjectSpawned;
    public Dictionary<string,IFallingObject> spawnedFallingObjects = new Dictionary<string, IFallingObject>();

    public BoolValue canSpawn;
    // Start is called before the first frame update
    void Start()
    {
        gameStarted.value = false;
        canSpawn.value = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn.value)
            return;

        if (spawnTime < Time.time)
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        spawnTime = Time.time + spawnSpeed;
        GameObject go = ObjectPooler.Instance.SpawnFromPool("Asteroid2", new Vector2(Random.Range(xMin, xMax), spawnPosition.position.y), spawnPosition.rotation);
        IFallingObject fallingObject = go.GetComponent<IFallingObject>();
        fallingObject.SetText(words.list[i]);
        fallingObject.SetWrongText(words.list[i]);
        ObjectSpawned.Invoke();
        i++;
    }
}
