using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cigaretteIgniter : MonoBehaviour
{
    
    // if trigger is entered, set the cigarette on fire
    private void Start()
    {
        Debug.Log("Cigarette Igniter Script Started");
    
    }

    // on collision enter debug log the name of the object that collided with the cigarette
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Cigarette Igniter Triggered by: " + collision.gameObject.name);
        // if object has CigaretteBurning script, set it on fire
        if (collision.gameObject.GetComponent<CigaretteBurning>())
        {
            // activate the burnMask
            collision.gameObject.GetComponent<CigaretteBurning>().burnMask.SetActive(true);
            
            collision.gameObject.GetComponent<CigaretteBurning>().isOnFire = true;
        }

        
    }
}
