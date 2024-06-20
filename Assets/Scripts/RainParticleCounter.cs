using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainParticleCounter : MonoBehaviour
{
    private int rainParticleCount = 0;
    private bool isWaterRaising = true;
    public float maxHeight = 10.0f;

    public float minHeight = 0.0f;

    public float timeUntilWaterGoesDown = 2.0f;

    public GameObject WaterPrefab;

    // OnTriggerEnter is called when the Collider other enters the trigger
    private float timeSinceLastRaindrop = 0f;
    private Vector3 originalPosition;



    void Start()
    {
        WaterPrefab = GameObject.Find("Water");
        originalPosition = WaterPrefab.transform.position;
        StartCoroutine(LowerWaterLevel());
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.name == "Rain")
        {
            rainParticleCount += 3;
            Debug.Log("Rain particle count: " + rainParticleCount);

            if (isWaterRaising && WaterPrefab.transform.position.y < maxHeight)
            {
                WaterPrefab.transform.position = new Vector3(WaterPrefab.transform.position.x, WaterPrefab.transform.position.y + 0.02f, WaterPrefab.transform.position.z);
            }

            timeSinceLastRaindrop = 0f;
        }
    }

    private IEnumerator LowerWaterLevel()
    {
        while (true)
        {
            if (timeSinceLastRaindrop > timeUntilWaterGoesDown && WaterPrefab.transform.position.y > minHeight)
            {
                WaterPrefab.transform.position = new Vector3(WaterPrefab.transform.position.x, WaterPrefab.transform.position.y - 0.02f, WaterPrefab.transform.position.z);
            }

            timeSinceLastRaindrop += Time.deltaTime;
            yield return null;
        }  
    }
}