using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creatureGoal : MonoBehaviour
{
    private CreatureSpawner creatureSpawner;
    private int maxCreatureCount;

    public int creatureCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        creatureSpawner = GameObject.Find("CreatureSpawner").GetComponent<CreatureSpawner>();
        maxCreatureCount = creatureSpawner.maxCreatureCount;
        
        
    }

    // Update is called once per frame

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // despawn and set maxCreatureCount inside the CreatureSpawner.cs -1 so a new one doesnt spawn
        

        if(collision.gameObject.GetComponent<Creature>())
        {
            creatureSpawner.maxCreatureCount -= 1;
            creatureCount += 1;
            Destroy(collision.gameObject);
        }
        
    }


}
