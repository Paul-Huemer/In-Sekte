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
    // public GameObject Floor; // The floor object

    private Vector3 originalScale; // The original scale of the creature

    void Start()
    {
        int startSize = Random.Range(4, 8);
        transform.localScale = new Vector3(startSize, startSize, startSize);
        originalScale = transform.localScale;

        // Floor = GameObject.Find("Floor");
        Goal = GameObject.Find("Goal");

        jumpForce = Random.Range(7, 15);
        speed = Random.Range(2, 4);


    }

    void Update()
    {
        Vector3 direction = (Goal.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // only gameobjects without the CreatureMove script are considered the floor
        if (!collision.gameObject.GetComponent<Creature>())
        {
            GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            StartCoroutine(RegularSquashAndStretch());
        } else {

        }


    }
    IEnumerator SmallSquashAndStretch() {
        Vector3 originalScale = transform.localScale;
        originalScale.x = originalScale.z;
        originalScale.y = originalScale.z;

        Vector3 squashedScale = new Vector3(originalScale.x * 0.9f, originalScale.y * 1.1f, originalScale.z);
        float duration = 0.1f;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            transform.localScale = Vector3.Lerp(originalScale, squashedScale, progress);
            yield return null;
        }

        transform.localScale = squashedScale;

        yield return new WaitForSeconds(0.2f);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
    }

    IEnumerator RegularSquashAndStretch()
    {
        Vector3 originalScale = transform.localScale;
        originalScale.x = originalScale.z;
        originalScale.y = originalScale.z;

        Vector3 squashedScale = new Vector3(originalScale.x * 0.8f, originalScale.y * 1.2f, originalScale.z);
        float duration = 0.2f;


        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            transform.localScale = Vector3.Lerp(originalScale, squashedScale, progress);
            yield return null;
        }


        transform.localScale = squashedScale;


        yield return new WaitForSeconds(0.2f);


        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
    }

}
