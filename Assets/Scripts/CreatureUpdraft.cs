using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureUpdraft : MonoBehaviour
{
    public float updraftForce = 2.0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        // despawn and set maxCreatureCount inside the CreatureSpawner.cs -1 so a new one doesnt spawn
        

        if(collision.gameObject.GetComponent<Creature>())
        {

            Debug.Log("Creature entered updraft");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * updraftForce, ForceMode2D.Impulse);

        }
        
    }
}
