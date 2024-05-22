using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureUpdraft : MonoBehaviour
{
    public float updraftForce = 2.0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Creature>())
        {
            // TODO Play a sound effect here
            
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * updraftForce, ForceMode2D.Impulse);
        }
        
    }
}
