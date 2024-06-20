using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureUpdraft : MonoBehaviour
{
    public float updraftForce = 2.0f;
    
    public float updraftDirection = 0.0f;

    public bool showDirectionGizmo = true;

    public bool showAreaGizmo = true;

    public Vector3 size;

    public ParticleSystem updraftParticles;

    void Start()
    {
     size = transform.localScale;   
    GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (showDirectionGizmo)
        {
            Vector3 direction = Quaternion.Euler(0, 0, updraftDirection) * Vector3.up;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, direction *8);
        }

        if (showAreaGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, size);
            
        }


    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Creature>())
        {
            // TODO Play a sound effect here

            updraftParticles.Play();
            
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Quaternion.Euler(0, 0, updraftDirection) * Vector2.up * updraftForce, ForceMode2D.Impulse);
        }
        
    }
}
