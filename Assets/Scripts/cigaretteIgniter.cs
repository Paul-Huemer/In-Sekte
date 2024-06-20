using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cigaretteIgniter : MonoBehaviour
{
    
    private void Start()
    {
        Debug.Log("Cigarette Igniter Script Started");
    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Cigarette Igniter Triggered by: " + collision.gameObject.name);
        // if object has CigaretteBurning script, set it on fire
        if (collision.gameObject.GetComponent<CigaretteBurning>())
        {
            collision.gameObject.GetComponent<CigaretteBurning>().burnMask.SetActive(true);
            
            collision.gameObject.GetComponent<CigaretteBurning>().isOnFire = true;
        }

        
    }
}
