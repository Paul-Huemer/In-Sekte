using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainParticleCounter : MonoBehaviour
{
    private int rainParticleCount = 0;
    private bool isWaterRaising = true;

    public GameObject WaterPrefab;

    // OnTriggerEnter is called when the Collider other enters the trigger

    void Start()
    {
        WaterPrefab = GameObject.Find("Water");
    }

    private void OnParticleCollision(GameObject other)
    {
        // check if the particle is a rain particle from "Rain" particle system
        if (other.name == "Rain")
        {
            rainParticleCount += 1;
            Debug.Log("Rain particle count: " + rainParticleCount);

            // constantly raise the water level as according to the rain particle count
            if (isWaterRaising)
            {
                WaterPrefab.transform.position = new Vector3(WaterPrefab.transform.position.x, WaterPrefab.transform.position.y + 0.01f, WaterPrefab.transform.position.z);
            }

                

        }

        
        

    }
}