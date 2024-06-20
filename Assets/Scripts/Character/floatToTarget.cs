using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretch : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float force = 1f;
    [SerializeField] private GameObject target;
    [SerializeField] private Rigidbody2D rb;

    private Vector2 forceDirection = Vector2.zero;
    
    // Update is called once per frame
    void Update()
    {
        // Attract to target
        forceDirection = (target.transform.position - transform.position).normalized * force;
        rb.AddForce(forceDirection);
    }
}
