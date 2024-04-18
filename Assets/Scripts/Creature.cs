using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public float speed;
public float jumpForce;
public float squashAmount = 0.8f; // The amount to squash and stretch the creature
public float stretchSpeed = 2.0f; // The speed of the stretching effect

private bool isStretching = false; // Whether the creature is currently stretching

public GameObject Goal; // The goal object
public GameObject Floor; // The floor object

private Vector3 originalScale; // The original scale of the creature

void Start()
{
    originalScale = transform.localScale;

    Floor = GameObject.Find("Floor");
    Goal = GameObject.Find("Goal");

    jumpForce = Random.Range(2, 7);
    speed = Random.Range(1, 2);


}

void Update()
{
    // Calculate the direction towards the goal
    Vector3 direction = (Goal.transform.position - transform.position).normalized;

    // Apply a force in the direction of the goal
    GetComponent<Rigidbody>().AddForce(direction * speed);

    // If the creature is stretching, stretch it back to its original scale
    if (isStretching)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * stretchSpeed);
        if (transform.localScale == originalScale)
        {
            isStretching = false;
        }
    }
}

void OnCollisionEnter(Collision collision)
{
    // When the creature touches the floor, make it bounce and squash it
    if (collision.gameObject == Floor)
    {
        Vector3 force = Vector3.up * jumpForce;
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        // Calculate the direction of the force
        Vector3 forceDirection = force.normalized;

        // Squash the creature in the direction of the force
        Vector3 squashScale = Vector3.Scale(Vector3.one + squashAmount * forceDirection, originalScale);
        GetComponent<Transform>().localScale = squashScale;

        isStretching = true;
    }
}

void OnCollisionStay(Collision collision)
{
    // When the creature is in contact with the floor, apply a force to make it bounce
    if (collision.gameObject == Floor)
    {
        Vector3 force = Vector3.up * (jumpForce/4);
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
    }
}


}
