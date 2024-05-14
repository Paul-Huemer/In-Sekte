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
            collision.gameObject.GetComponent<Creature>().isInvincible = true;
            StartCoroutine(DestroyCreature(collision.gameObject));
        }
        
    }

    //  destroy creature by making it smaller gradually
    IEnumerator DestroyCreature(GameObject creatureToDestroy)
    {
        float timeElapsed = 0;
        Vector3 originalScale = creatureToDestroy.transform.localScale;
        while (timeElapsed < 2.0f)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / 2.0f;
            creatureToDestroy.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            yield return null;
        }
        // wait 1 second 
        yield return new WaitForSeconds(1.0f);
        Destroy(creatureToDestroy);
    }


}
