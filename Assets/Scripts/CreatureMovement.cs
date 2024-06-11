using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float squashAmount = 0.8f; // The amount to squash and stretch the creature
    public float stretchSpeed = 2.0f; // The speed of the stretching effect

    public bool isInvincible = false; // Whether the creature is invincible
    public bool isBasicallyDead = false; // Whether the creature is basically dead

    private bool isStretching = false; // Whether the creature is currently stretching

    public GameObject Goal; // The goal object
    // public GameObject Floor; // The floor object
    public GameObject ShiluetteCollider; // The shiluette 2d collider object

    private Vector3 originalScale; // The original scale of the creature
    public ParticleSystem deathParticles;
    // public AudioClip deathSound;
    public AudioSource creatureAudioSource;

    public float timeInDanger = 0.0f;

    public int steepestAngle = 45; // The steepest angle the creature can walk on

    // sound effect for creature
    public AudioClip[] creatureSounds;


    void Start()
    {
        float startSize = Random.Range(2, 6);
        startSize = startSize / 3;
        transform.localScale = new Vector3(startSize, startSize, startSize);
        originalScale = transform.localScale;


        Goal = GameObject.Find("Goal");
        ShiluetteCollider = GameObject.Find("ShiluetteCollider");

        jumpForce = Random.Range(7, 15);
    
        speed = Random.Range(2, 4);

        creatureSounds = Resources.LoadAll<AudioClip>("CreatureNoisesV2");
    }

    void Update()
    {
        Vector3 direction = (Goal.transform.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(direction * speed);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // play a random creature sound
        if (isInvincible || isBasicallyDead) {
            return;
        } else {
            // play a sound effect 10% of the time
            if (Random.Range(0, 10) == 0) {
                creatureAudioSource.PlayOneShot(creatureSounds[Random.Range(0, creatureSounds.Length)]);
            }
        }



        // only gameobjects without the CreatureMove script are considered the floor
        if (!collision.gameObject.GetComponent<Creature>() && collision.gameObject != ShiluetteCollider)
        {
            // if angle is less than 45 degrees
            if (collision.contacts[0].normal.y > Mathf.Sin(steepestAngle * Mathf.Deg2Rad))
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                StartCoroutine(RegularSquashAndStretch());
            }
            else
            {

            }
        } else if (collision.gameObject == ShiluetteCollider)
        {
            // check if angle is less than 45 degrees
            Vector3 normal = collision.contacts[0].normal;
            float angle = Vector3.Angle(Vector3.up, normal);
            if (angle < 45)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                StartCoroutine(RegularSquashAndStretch());
            }
            else
            {
                // GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce/8, ForceMode2D.Impulse);
                StartCoroutine(SmallSquashAndStretch());
            }
            // GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce/2, ForceMode2D.Impulse);
            StartCoroutine(SmallSquashAndStretch());
        }


    }
    IEnumerator SmallSquashAndStretch() {
        if (isInvincible || isBasicallyDead) {
            yield break;
        } else {
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

        yield return new WaitForSeconds(0.4f);

        if (isInvincible || isBasicallyDead) {
            yield break;
        } else {

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
         }
        }

    }

    IEnumerator RegularSquashAndStretch()
    {
        if (isInvincible || isBasicallyDead) {
            yield break;
        } else {
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

        if (isInvincible || isBasicallyDead) {
            yield break;
        } else {
            for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float progress = t / duration;
            transform.localScale = Vector3.Lerp(squashedScale, originalScale, progress);
            yield return null;
        }

        transform.localScale = originalScale;
        }


        
    }
    }

}
