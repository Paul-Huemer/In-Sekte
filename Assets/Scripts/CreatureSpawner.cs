using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour
{
    public bool showGizmos = true;
    public GameObject creaturePrefab;
    public int maxCreatureCount = 10;
    public GameObject SpawnPoint;
    public float spawnRadius = 10.0f;

    public float spawnInterval = 4.0f;

    private GameObject[] creatures = new GameObject[10];

    void Start()
    {
        creatures = new GameObject[10]; // Initialize the array with a size of 10
        StartCoroutine(SpawnCreaturesOverTime());
    }

    IEnumerator SpawnCreaturesOverTime()
    {
        while (true)
        {
            int creatureCount = creatures.Count(creature => creature != null);

            if (creatureCount < maxCreatureCount)
            {
                SpawnCreature();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnCreature()
    {
        Vector3 spawnPosition = SpawnPoint.transform.position + Random.insideUnitSphere * spawnRadius;
        GameObject newCreature = Instantiate(creaturePrefab, spawnPosition, Quaternion.identity);

        for (int i = 0; i < creatures.Length; i++)
        {
            if (creatures[i] == null)
            {
                creatures[i] = newCreature;
                break;
            }
        }
    }


}
