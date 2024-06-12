using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPoolWood = 5;
    private int amountToPoolStone = 5;

    public static ObjectPool instance;
    public List<Vector3> SpawnPositions;
    public List<Quaternion> SpawnRotations;
    public float respawnTime;

    [SerializeField] private GameObject Wood;
    [SerializeField] private GameObject Stone;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        int j = amountToPoolWood;
        for (int i = 0; i < amountToPoolWood; i++)
        {
            GameObject node = Instantiate(Wood);
            node.GetComponent<Outline>().enabled = false;
            pooledObjects.Add(node);
            SpawnNode(i, SpawnPositions[i], SpawnRotations[i]);
        }
        for (int i = 0; i < amountToPoolStone; i++)
        {
            GameObject node = Instantiate(Stone);
            node.GetComponent<Outline>().enabled = false;
            pooledObjects.Add(node);
            SpawnNode(j + i, SpawnPositions[j + i], SpawnRotations[j + i]);
        }
    }

    public GameObject GetPooledObject(GameObject node)
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeSelf)
                RespawnNode(pooledObjects[i]);
        }
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (Vector3.Distance(pooledObjects[i].transform.position, node.transform.position) < 0.1f && pooledObjects[i].activeSelf)
                return pooledObjects[i];
        }
        return null;
    }

    public void SpawnNode(int index, Vector3 position, Quaternion rotation)
    {
        GameObject node = pooledObjects[index];
        node.transform.position = position;
        node.transform.rotation = rotation;
        node.SetActive(true);
    }

    public void RespawnNode(GameObject node)
    {
        StartCoroutine(RespawnTimer(node));
    }

    private IEnumerator RespawnTimer(GameObject node)
    {
        yield return new WaitForSeconds(respawnTime);

        node.SetActive(true);
    }
}
